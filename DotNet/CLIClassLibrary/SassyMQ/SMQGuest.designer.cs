using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace YP.SassyMQ.Lib.RabbitMQ
{
    public partial class SMQGuest : SMQActorBase
    {

        public SMQGuest(String amqpConnectionString)
            : base(amqpConnectionString, "guest")
        {
        }

        protected override void CheckRouting(StandardPayload payload, BasicDeliverEventArgs  bdea)
        {
            var originalAccessToken = payload.AccessToken;
            try
            {
                switch (bdea.RoutingKey)
                {
                    
                }

            }
            catch (Exception ex)
            {
                payload.ErrorMessage = ex.Message;
            }
            var reply = payload.ReplyPayload is null ? payload  : payload.ReplyPayload;
            reply.IsHandled = payload.IsHandled;
            if (reply.AccessToken == originalAccessToken) reply.AccessToken = null;            
            this.Reply(reply, bdea.BasicProperties);
        }

        
        /// <summary>
        /// RequestToken - 
        /// </summary>
        public Task RequestToken(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.RequestToken(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// RequestToken - 
        /// </summary>
        public Task RequestToken(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.RequestToken(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// RequestToken - 
        /// </summary>
        public Task RequestToken(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.general.guest.requesttoken", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// ValidateTemporaryAccessToken - 
        /// </summary>
        public Task ValidateTemporaryAccessToken(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.ValidateTemporaryAccessToken(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// ValidateTemporaryAccessToken - 
        /// </summary>
        public Task ValidateTemporaryAccessToken(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.ValidateTemporaryAccessToken(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// ValidateTemporaryAccessToken - 
        /// </summary>
        public Task ValidateTemporaryAccessToken(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.general.guest.validatetemporaryaccesstoken", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// WhoAmI - 
        /// </summary>
        public Task WhoAmI(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.WhoAmI(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// WhoAmI - 
        /// </summary>
        public Task WhoAmI(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.WhoAmI(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// WhoAmI - 
        /// </summary>
        public Task WhoAmI(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.general.guest.whoami", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// WhoAreYou - 
        /// </summary>
        public Task WhoAreYou(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.WhoAreYou(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// WhoAreYou - 
        /// </summary>
        public Task WhoAreYou(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.WhoAreYou(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// WhoAreYou - 
        /// </summary>
        public Task WhoAreYou(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.general.guest.whoareyou", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// StoreTempFile - 
        /// </summary>
        public Task StoreTempFile(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.StoreTempFile(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// StoreTempFile - 
        /// </summary>
        public Task StoreTempFile(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.StoreTempFile(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// StoreTempFile - 
        /// </summary>
        public Task StoreTempFile(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.utlity.guest.storetempfile", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// GetPortfolios - 
        /// </summary>
        public Task GetPortfolios(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.GetPortfolios(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// GetPortfolios - 
        /// </summary>
        public Task GetPortfolios(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.GetPortfolios(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// GetPortfolios - 
        /// </summary>
        public Task GetPortfolios(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.crud.guest.getportfolios", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
    }
}

                    
