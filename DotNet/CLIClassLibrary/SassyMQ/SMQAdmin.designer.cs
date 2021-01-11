using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace YP.SassyMQ.Lib.RabbitMQ
{
    public partial class SMQAdmin : SMQActorBase
    {

        public SMQAdmin(String amqpConnectionString)
            : base(amqpConnectionString, "admin")
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
            return this.SendMessage("crudcoordinator.crud.admin.addportfolio", payload, replyHandler, timeoutHandler, waitTimeout);
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
            return this.SendMessage("crudcoordinator.crud.admin.getportfolios", payload, replyHandler, timeoutHandler, waitTimeout);
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
            return this.SendMessage("crudcoordinator.crud.admin.updateportfolio", payload, replyHandler, timeoutHandler, waitTimeout);
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
            return this.SendMessage("crudcoordinator.crud.admin.deleteportfolio", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// AddAppUser - 
        /// </summary>
        public Task AddAppUser(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.AddAppUser(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// AddAppUser - 
        /// </summary>
        public Task AddAppUser(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.AddAppUser(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// AddAppUser - 
        /// </summary>
        public Task AddAppUser(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.crud.admin.addappuser", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// GetAppUsers - 
        /// </summary>
        public Task GetAppUsers(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.GetAppUsers(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// GetAppUsers - 
        /// </summary>
        public Task GetAppUsers(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.GetAppUsers(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// GetAppUsers - 
        /// </summary>
        public Task GetAppUsers(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.crud.admin.getappusers", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// UpdateAppUser - 
        /// </summary>
        public Task UpdateAppUser(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.UpdateAppUser(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// UpdateAppUser - 
        /// </summary>
        public Task UpdateAppUser(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.UpdateAppUser(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// UpdateAppUser - 
        /// </summary>
        public Task UpdateAppUser(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.crud.admin.updateappuser", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// DeleteAppUser - 
        /// </summary>
        public Task DeleteAppUser(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.DeleteAppUser(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// DeleteAppUser - 
        /// </summary>
        public Task DeleteAppUser(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.DeleteAppUser(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// DeleteAppUser - 
        /// </summary>
        public Task DeleteAppUser(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.crud.admin.deleteappuser", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
    }
}

                    
