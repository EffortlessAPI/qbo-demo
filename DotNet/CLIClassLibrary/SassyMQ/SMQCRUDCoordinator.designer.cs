using System;
using System.Linq;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace YP.SassyMQ.Lib.RabbitMQ
{
    public partial class SMQCRUDCoordinator : SMQActorBase
    {

        public SMQCRUDCoordinator(String amqpConnectionString)
            : base(amqpConnectionString, "crudcoordinator")
        {
        }

        protected override void CheckRouting(StandardPayload payload, BasicDeliverEventArgs  bdea)
        {
            var originalAccessToken = payload.AccessToken;
            try
            {
                switch (bdea.RoutingKey)
                {
                    
                    case "crudcoordinator.general.guest.requesttoken":
                        this.OnGuestRequestTokenReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.general.guest.validatetemporaryaccesstoken":
                        this.OnGuestValidateTemporaryAccessTokenReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.general.guest.whoami":
                        this.OnGuestWhoAmIReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.general.guest.whoareyou":
                        this.OnGuestWhoAreYouReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.utlity.guest.storetempfile":
                        this.OnGuestStoreTempFileReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.general.crudcoordinator.resetrabbitsassymqconfiguration":
                        this.OnCRUDCoordinatorResetRabbitSassyMQConfigurationReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.general.crudcoordinator.resetjwtsecretkey":
                        this.OnCRUDCoordinatorResetJWTSecretKeyReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.guest.getportfolios":
                        this.OnGuestGetPortfoliosReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.admin.addportfolio":
                        this.OnAdminAddPortfolioReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.admin.getportfolios":
                        this.OnAdminGetPortfoliosReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.admin.updateportfolio":
                        this.OnAdminUpdatePortfolioReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.admin.deleteportfolio":
                        this.OnAdminDeletePortfolioReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.user.addportfolio":
                        this.OnUserAddPortfolioReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.user.getportfolios":
                        this.OnUserGetPortfoliosReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.user.updateportfolio":
                        this.OnUserUpdatePortfolioReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.user.deleteportfolio":
                        this.OnUserDeletePortfolioReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.admin.addappuser":
                        this.OnAdminAddAppUserReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.admin.getappusers":
                        this.OnAdminGetAppUsersReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.admin.updateappuser":
                        this.OnAdminUpdateAppUserReceived(payload, bdea);
                        break;
                    
                    case "crudcoordinator.crud.admin.deleteappuser":
                        this.OnAdminDeleteAppUserReceived(payload, bdea);
                        break;
                    
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
        /// Responds to: RequestToken from Guest
        /// </summary>
        public event EventHandler<PayloadEventArgs> GuestRequestTokenReceived;
        protected virtual void OnGuestRequestTokenReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.GuestRequestTokenReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.GuestRequestTokenReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: ValidateTemporaryAccessToken from Guest
        /// </summary>
        public event EventHandler<PayloadEventArgs> GuestValidateTemporaryAccessTokenReceived;
        protected virtual void OnGuestValidateTemporaryAccessTokenReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.GuestValidateTemporaryAccessTokenReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.GuestValidateTemporaryAccessTokenReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: WhoAmI from Guest
        /// </summary>
        public event EventHandler<PayloadEventArgs> GuestWhoAmIReceived;
        protected virtual void OnGuestWhoAmIReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.GuestWhoAmIReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.GuestWhoAmIReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: WhoAreYou from Guest
        /// </summary>
        public event EventHandler<PayloadEventArgs> GuestWhoAreYouReceived;
        protected virtual void OnGuestWhoAreYouReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.GuestWhoAreYouReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.GuestWhoAreYouReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: StoreTempFile from Guest
        /// </summary>
        public event EventHandler<PayloadEventArgs> GuestStoreTempFileReceived;
        protected virtual void OnGuestStoreTempFileReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.GuestStoreTempFileReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.GuestStoreTempFileReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: ResetRabbitSassyMQConfiguration from CRUDCoordinator
        /// </summary>
        public event EventHandler<PayloadEventArgs> CRUDCoordinatorResetRabbitSassyMQConfigurationReceived;
        protected virtual void OnCRUDCoordinatorResetRabbitSassyMQConfigurationReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.CRUDCoordinatorResetRabbitSassyMQConfigurationReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.CRUDCoordinatorResetRabbitSassyMQConfigurationReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: ResetJWTSecretKey from CRUDCoordinator
        /// </summary>
        public event EventHandler<PayloadEventArgs> CRUDCoordinatorResetJWTSecretKeyReceived;
        protected virtual void OnCRUDCoordinatorResetJWTSecretKeyReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.CRUDCoordinatorResetJWTSecretKeyReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.CRUDCoordinatorResetJWTSecretKeyReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: GetPortfolios from Guest
        /// </summary>
        public event EventHandler<PayloadEventArgs> GuestGetPortfoliosReceived;
        protected virtual void OnGuestGetPortfoliosReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.GuestGetPortfoliosReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.GuestGetPortfoliosReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: AddPortfolio from Admin
        /// </summary>
        public event EventHandler<PayloadEventArgs> AdminAddPortfolioReceived;
        protected virtual void OnAdminAddPortfolioReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.AdminAddPortfolioReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.AdminAddPortfolioReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: GetPortfolios from Admin
        /// </summary>
        public event EventHandler<PayloadEventArgs> AdminGetPortfoliosReceived;
        protected virtual void OnAdminGetPortfoliosReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.AdminGetPortfoliosReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.AdminGetPortfoliosReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: UpdatePortfolio from Admin
        /// </summary>
        public event EventHandler<PayloadEventArgs> AdminUpdatePortfolioReceived;
        protected virtual void OnAdminUpdatePortfolioReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.AdminUpdatePortfolioReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.AdminUpdatePortfolioReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: DeletePortfolio from Admin
        /// </summary>
        public event EventHandler<PayloadEventArgs> AdminDeletePortfolioReceived;
        protected virtual void OnAdminDeletePortfolioReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.AdminDeletePortfolioReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.AdminDeletePortfolioReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: AddPortfolio from User
        /// </summary>
        public event EventHandler<PayloadEventArgs> UserAddPortfolioReceived;
        protected virtual void OnUserAddPortfolioReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.UserAddPortfolioReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.UserAddPortfolioReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: GetPortfolios from User
        /// </summary>
        public event EventHandler<PayloadEventArgs> UserGetPortfoliosReceived;
        protected virtual void OnUserGetPortfoliosReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.UserGetPortfoliosReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.UserGetPortfoliosReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: UpdatePortfolio from User
        /// </summary>
        public event EventHandler<PayloadEventArgs> UserUpdatePortfolioReceived;
        protected virtual void OnUserUpdatePortfolioReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.UserUpdatePortfolioReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.UserUpdatePortfolioReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: DeletePortfolio from User
        /// </summary>
        public event EventHandler<PayloadEventArgs> UserDeletePortfolioReceived;
        protected virtual void OnUserDeletePortfolioReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.UserDeletePortfolioReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.UserDeletePortfolioReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: AddAppUser from Admin
        /// </summary>
        public event EventHandler<PayloadEventArgs> AdminAddAppUserReceived;
        protected virtual void OnAdminAddAppUserReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.AdminAddAppUserReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.AdminAddAppUserReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: GetAppUsers from Admin
        /// </summary>
        public event EventHandler<PayloadEventArgs> AdminGetAppUsersReceived;
        protected virtual void OnAdminGetAppUsersReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.AdminGetAppUsersReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.AdminGetAppUsersReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: UpdateAppUser from Admin
        /// </summary>
        public event EventHandler<PayloadEventArgs> AdminUpdateAppUserReceived;
        protected virtual void OnAdminUpdateAppUserReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.AdminUpdateAppUserReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.AdminUpdateAppUserReceived(this, plea);
            }
        }

        /// <summary>
        /// Responds to: DeleteAppUser from Admin
        /// </summary>
        public event EventHandler<PayloadEventArgs> AdminDeleteAppUserReceived;
        protected virtual void OnAdminDeleteAppUserReceived(StandardPayload payload, BasicDeliverEventArgs bdea)
        {
            var plea = new PayloadEventArgs(payload, bdea);
            if (!ReferenceEquals(this.AdminDeleteAppUserReceived, null))
            {
                plea.Payload.IsHandled = true;
                this.AdminDeleteAppUserReceived(this, plea);
            }
        }

        /// <summary>
        /// ResetRabbitSassyMQConfiguration - 
        /// </summary>
        public Task ResetRabbitSassyMQConfiguration(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.ResetRabbitSassyMQConfiguration(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// ResetRabbitSassyMQConfiguration - 
        /// </summary>
        public Task ResetRabbitSassyMQConfiguration(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.ResetRabbitSassyMQConfiguration(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// ResetRabbitSassyMQConfiguration - 
        /// </summary>
        public Task ResetRabbitSassyMQConfiguration(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.general.crudcoordinator.resetrabbitsassymqconfiguration", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
        /// <summary>
        /// ResetJWTSecretKey - 
        /// </summary>
        public Task ResetJWTSecretKey(PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.ResetJWTSecretKey(this.CreatePayload(), replyHandler, timeoutHandler, waitTimeout);
        }

        /// <summary>
        /// ResetJWTSecretKey - 
        /// </summary>
        public Task ResetJWTSecretKey(String content, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            var payload = this.CreatePayload(content);
            return this.ResetJWTSecretKey(payload, replyHandler, timeoutHandler, waitTimeout);
        }
    
        
        /// <summary>
        /// ResetJWTSecretKey - 
        /// </summary>
        public Task ResetJWTSecretKey(StandardPayload payload, PayloadHandler replyHandler = null, PayloadHandler timeoutHandler = null, int waitTimeout = StandardPayload.DEFAULT_TIMEOUT)
        {
            return this.SendMessage("crudcoordinator.general.crudcoordinator.resetjwtsecretkey", payload, replyHandler, timeoutHandler, waitTimeout);
        }
        
        
    }
}

                    
