using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using dc = EAPI.CLI.Lib.DataClasses;
using RabbitMQ.Client.MessagePatterns;

/*
$smq-namespace: 
$namespace: YP.SassyMQ.Lib.RabbitMQ
$final-namespace: YP.SassyMQ.Lib.RabbitMQ
$dataclasses-namespace: EAPI.CLI.Lib.DataClasses
*/

namespace YP.SassyMQ.Lib.RabbitMQ
{

    public delegate void PayloadHandler(StandardPayload payload, BasicDeliverEventArgs bdea);
    /// <summary>
    /// Summary description for SMQActorBase
    /// </summary>
    public abstract partial class SMQActorBase
    {
        public IModel RMQChannel;
        public IConnection RMQConnection;
        protected ConnectionFactory RMQFactory;
        public Task MonitorTask { get; private set; }

        public Guid SenderId { get; private set; }
        public String AccessToken { get; set; }
        public String SenderName { get; set; }
        public string QueueName { get; }
        public string AllExchange { get; }
        public string Microphone { get; }

        public string AMQPConnectionString { get; private set; }
        public string Protocol { get;private set;}
        public string Username { get;private set;}
        public string Password { get;private set;}
        public string Hostname { get;private set; }
        public string VirtualHost { get;private set; }
        public SslOption Ssl { get; private set; }

        public static bool IsDebugMode { get; set; }
        public static bool ShowPings { get; set; }
        public JsonSerializerSettings DefaultJsonSerializerSettings { get; set; }
        
        public SMQActorBase()
        {
            this.DefaultJsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public void WaitForComplete(int timeout = -1)
        {
            var actorWaitTask = Task.Factory.StartNew(() =>
            {
                while (this.RMQChannel.IsOpen)
                {
                    Thread.Sleep(500);
                }
            });

            if (timeout == -1) actorWaitTask.Wait();
            else actorWaitTask.Wait(timeout);

            this.Disconnect();
        }

        public SMQActorBase(String amqpConnectionString, String actorName)
            : this()
        {
            if (string.IsNullOrEmpty(actorName)) throw new ArgumentException("actor.AllExchange is required.");

            this.AllExchange = String.Format("{0}.all", actorName);
            this.Microphone = String.Format("{0}mic", actorName);

            this.SenderId = Guid.NewGuid();
            this.SenderName = String.Format("{0}@{1}", Environment.UserName, Environment.MachineName);

            this.ParseAMQPConnectionString(amqpConnectionString);

            this.RMQFactory = new ConnectionFactory() { HostName = this.Hostname, VirtualHost = this.VirtualHost, UserName = this.Username, Password = this.Password, Ssl = this.Ssl };
            this.RMQConnection = this.RMQFactory.CreateConnection();
            this.RMQChannel = this.RMQConnection.CreateModel();

            var consumer = new EventingBasicConsumer(this.RMQChannel);
            consumer.Received += Consumer_Received;
            RMQChannel.BasicConsume("amq.rabbitmq.reply-to", true, consumer);

            this.QueueName = RMQChannel.QueueDeclare().QueueName;

            RMQChannel.QueueBind(queue: QueueName, exchange: this.AllExchange, routingKey: "#");

            // System.Console.WriteLine("CONNECTED: [*] Waiting for messages at {0}. To exit press CTRL+C", this.AllExchange);

            this.AfterConnect();

            this.ConnectAndMonitor();
        }

        private void ParseAMQPConnectionString(string amqpConnectionString)
        {
            this.AMQPConnectionString = amqpConnectionString;

            var parts = this.AMQPConnectionString.Split("://@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            this.Protocol = parts[0].SafeToString().ToLower();
            this.Username = parts[1];
            if (String.IsNullOrEmpty(this.Username)) this.Username = "guest";
            this.Password = parts[2];
            if (String.IsNullOrEmpty(this.Password)) this.Password = "guest";
            this.Hostname = parts[3];
            if (String.IsNullOrEmpty(this.Hostname)) this.Hostname = "guest";
            this.VirtualHost = parts[4];
            if (String.IsNullOrEmpty(this.VirtualHost)) this.VirtualHost = "/";

            this.Ssl = new SslOption(this.Hostname, enabled: this.Protocol == "amqps");
            this.Ssl.AcceptablePolicyErrors = System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch |System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors;
        }

        private void ConnectAndMonitor()
        {
            this.MonitorTask = new Task(() =>
            {
                var count = 0;
                var subscription = new Subscription(RMQChannel, QueueName);
                BasicDeliverEventArgs bdea = default(BasicDeliverEventArgs);
                while (this.RMQChannel.IsOpen)
                {
                        var gotMessage = subscription.Next(100, out bdea);
                        if (gotMessage)
                        {
                            var payload = new StandardPayload();
                            try
                            {
                                var msgText = string.Format("{0}{1}. {2} => {3}{0}", Environment.NewLine, ++count, bdea.Exchange, bdea.RoutingKey);
                                //var msgText = string.Format("{3}. {0}: {1} -> '{2}'", bdea.Exchange, bdea.RoutingKey, Encoding.UTF8.GetString(bdea.Body), ++count);

                                if (SMQActorBase.IsDebugMode) Console.WriteLine(msgText);

                                var body = Encoding.UTF8.GetString(bdea.Body);
                                payload = JsonConvert.DeserializeObject<StandardPayload>(body, this.DefaultJsonSerializerSettings);
                                if (ReferenceEquals(payload, null)) payload = new StandardPayload()
                                {
                                    ErrorMessage = "Payload missing."
                                };

                                this.OnMessageReceived(payload, bdea);
                                this.CheckRouting(payload, bdea);
                            }
                            catch (Exception ex)
                            {
                                payload.ErrorMessage = ex.Message;
                                this.Reply(payload, bdea.BasicProperties);
                            }
                            this.OnAfterMessageReceived(payload, bdea);
                        }
                }

                try
                {
                    if (this.RMQChannel.IsOpen) this.RMQChannel.Close();
                }
                catch { } // Ignore errors on closing the channel

                try
                {
                    if (this.RMQConnection.IsOpen) this.RMQConnection.Close();
                }
                catch { } // Ignore errrors on closing connection
            });

            this.MonitorTask.Start();
        }

        protected virtual void AfterConnect()
        {
            // do nothing
        }
        public event EventHandler<PayloadEventArgs> MessageReceived;
        protected virtual void OnMessageReceived(StandardPayload payload, BasicDeliverEventArgs  bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.MessageReceived, null)) this.MessageReceived(this, plea);
        }

        public event EventHandler<PayloadEventArgs> AfterMessageReceived;
        protected virtual void OnAfterMessageReceived(StandardPayload payload, BasicDeliverEventArgs  bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.AfterMessageReceived, null)) this.AfterMessageReceived(this, plea);
        }

        protected abstract void CheckRouting(StandardPayload payload, BasicDeliverEventArgs bdea);

        private void Consumer_Received(object sender, BasicDeliverEventArgs bdea)
        {
            var body = Encoding.UTF8.GetString(bdea.Body);
            var payload = JsonConvert.DeserializeObject<StandardPayload>(body, this.DefaultJsonSerializerSettings);
            if (!String.IsNullOrEmpty(payload.ErrorMessage)) this.Throw(new Exception("Error Processing Message:" + payload.ErrorMessage));
            this.OnReplyTo(payload, bdea);
        }
        
        public class ExceptionEventArgs : EventArgs
        {
            public Exception Exception { get; set; }
        }

        public static event EventHandler<ExceptionEventArgs> ExceptionOccurred;
        private void Throw(Exception exception)
        {
            if (!ReferenceEquals(ExceptionOccurred, null)) ExceptionOccurred(this, new ExceptionEventArgs() { Exception = exception });
        }

        public event System.EventHandler<PayloadEventArgs> ReplyTo;
        protected virtual void OnReplyTo(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.ReplyTo, null)) this.ReplyTo(this, plea);
        }

        public void Disconnect()
        {
            this.RMQChannel.Close();
        }

        public virtual StandardPayload CreatePayload()
        {
            StandardPayload payload = new StandardPayload(this);
            this.CheckPayload(payload);
            return payload;
        }
        
        public StandardPayload CreatePayload(string content)
        {
            var payload = this.CreatePayload();
            payload.Content = content;
            return payload;
        }
        
        public StandardPayload CreatePayloadFromJson(string json)
        {
            var payload = JsonConvert.DeserializeObject<StandardPayload>(json);
            payload.SetActor(this);
            return payload;
        }

        protected Task SendMessage(string routingKey, StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int timeout = StandardPayload.DEFAULT_TIMEOUT, string directMessageQueue = "")
        {
            if (SMQActorBase.IsDebugMode)
            {
                System.Console.WriteLine(routingKey);
            }

            var finalMic = String.IsNullOrEmpty(directMessageQueue) ? this.Microphone : "";
            var finalRoutinKey = String.IsNullOrEmpty(directMessageQueue) ? routingKey : directMessageQueue;

            IBasicProperties props = this.RMQChannel.CreateBasicProperties();
            props.ReplyTo = "amq.rabbitmq.reply-to";
            props.CorrelationId = payload.PayloadId.ToString();
            payload.RoutingKey = String.IsNullOrEmpty(directMessageQueue) ? "" : routingKey;
            var payloadJson = JsonConvert.SerializeObject(payload, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            this.RMQChannel.BasicPublish(finalMic, finalRoutinKey, props, Encoding.UTF8.GetBytes(payloadJson));
            return payload.WaitForReply(replyHandler, timeoutHandler, timeout);
        }

        public void CheckPayload(StandardPayload payload)
        {
            payload.SenderId = this.SenderId.ToString();
            payload.SenderName = this.SenderName;
            payload.PayloadId = Guid.NewGuid().ToString();
        }
        
        protected void Reply(StandardPayload payload, IBasicProperties basicProperties)
        {
            IBasicProperties props = this.RMQChannel.CreateBasicProperties();
            props.CorrelationId = basicProperties.CorrelationId;
            this.CheckPayload(payload);
            var payloadJson = JsonConvert.SerializeObject(payload, this.DefaultJsonSerializerSettings);
            this.RMQChannel.BasicPublish("", basicProperties.ReplyTo, props, Encoding.UTF8.GetBytes(payloadJson));
        }
    }

    public static class SassyMQExtensions
    {
        public static String SafeToString(this object obj)
        {
            if (ReferenceEquals(obj, null)) return String.Empty;
            else return obj.ToString();
        }
        
        public static String CLISafeToString(this object obj)
        {
            var str = obj.SafeToString();
            var acceptableCharArray = str.Where(whereChar => Char.IsLetterOrDigit(whereChar) || whereChar == '-');
            return String.Join("", acceptableCharArray);
        }
    }
}
