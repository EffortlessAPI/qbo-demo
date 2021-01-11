using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace YP.SassyMQ.Lib.RabbitMQ
{
    public partial class SMQUser : SMQActorBase
    {

        public SMQUser(String amqpConnectionString)
            : base(amqpConnectionString, "user")
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
        /// AddPortfolio - 
        /// </summary>
        public Task AddPortfolio(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.AddPortfolio(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// AddPortfolio - 
        /// </summary>
        public Task AddPortfolio(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.AddPortfolio(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// AddPortfolio - 
        /// </summary>
        public Task AddPortfolio(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.crud.user.addportfolio", payload, replyHandler, timeoutHandler, waitTimeout);
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
            return this.SendMessage("crudcoordinator.crud.user.getportfolios", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// UpdatePortfolio - 
        /// </summary>
        public Task UpdatePortfolio(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.UpdatePortfolio(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// UpdatePortfolio - 
        /// </summary>
        public Task UpdatePortfolio(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.UpdatePortfolio(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// UpdatePortfolio - 
        /// </summary>
        public Task UpdatePortfolio(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.crud.user.updateportfolio", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// DeletePortfolio - 
        /// </summary>
        public Task DeletePortfolio(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.DeletePortfolio(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// DeletePortfolio - 
        /// </summary>
        public Task DeletePortfolio(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.DeletePortfolio(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// DeletePortfolio - 
        /// </summary>
        public Task DeletePortfolio(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.crud.user.deleteportfolio", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
    }
}

                    
