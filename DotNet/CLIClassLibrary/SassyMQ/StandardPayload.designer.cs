using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using dc = EAPI.CLI.Lib.DataClasses;
using System.Collections.Generic;


namespace YP.SassyMQ.Lib.RabbitMQ
{
    public partial class StandardPayload
    {
        public string RoutingKey { get; set; }
        
        private StandardPayload(SMQActorBase actor, string content, bool final)
        {
            this.PayloadId = Guid.NewGuid().ToString();

            this.__Actor = actor;
            if (!ReferenceEquals(this.__Actor, null))
            {
                this.SenderId = actor.SenderId.ToString();
                this.SenderName = actor.SenderName;
                this.AccessToken = actor.AccessToken;
            }
            else
            {
                this.SenderId = Guid.NewGuid().ToString();
                this.SenderName = "Unnamed Actor";
                this.AccessToken = null;
            }

            this.Content = content;
        }

        // 2 odxml properties
        
        public String PortfolioId { get; set; }
        
        public dc.Portfolio Portfolio { get; set; }
        
        public List<dc.Portfolio> Portfolios { get; set; }
        
        public String AppUserId { get; set; }
        
        public dc.AppUser AppUser { get; set; }
        
        public List<dc.AppUser> AppUsers { get; set; }
        
        
        public String ToJSON() 
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        private void HandleReplyTo(object sender, PayloadEventArgs e)
        {
            if (e.Payload.IsHandled && e.BasicDeliverEventArgs.BasicProperties.CorrelationId == this.PayloadId)
            {
                this.ReplyPayload = e.Payload;
                this.ReplyBDEA = e.BasicDeliverEventArgs;
                this.ReplyRecieved = true;
            }
        }

       
        public Task WaitForReply(PayloadHandler payloadHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var actor = this.__Actor;
            if (ReferenceEquals(actor, null)) throw new Exception("Can't handle response if payload.Actor is null");
            else
            {
                actor.ReplyTo += this.HandleReplyTo;
                var waitTask = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        if (waitTimeout > 0 && !ReferenceEquals(payloadHandler, null))
                        {

                            this.TimedOutWaiting = false;
                            var startedAt = DateTime.Now;

                            while (!this.ReplyRecieved && !this.TimedOutWaiting && DateTime.Now < startedAt.AddSeconds(waitTimeout))
                            {
                                Thread.Sleep(100);
                            }

                            if (!this.ReplyRecieved) this.TimedOutWaiting = true;

                            var errorMessageReceived = !ReferenceEquals(this.ReplyPayload, null) && !String.IsNullOrEmpty(this.ReplyPayload.ErrorMessage);

                            if (this.ReplyRecieved && (!errorMessageReceived || ReferenceEquals(timeoutHandler, null)))
                            {
                                this.ReplyPayload.__Actor = actor;
                                payloadHandler(this.ReplyPayload, this.ReplyBDEA);
                            }
                            else if (!ReferenceEquals(timeoutHandler, null)) timeoutHandler(this.ReplyPayload, default(BasicDeliverEventArgs));
                        }

                    }
                    finally
                    {
                        actor.ReplyTo -= this.HandleReplyTo;
                    }
                });
                return waitTask;
            }
        }
    }
}