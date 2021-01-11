
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace YP.SassyMQ.Lib.RabbitMQ
{
    public partial class StandardPayload
    {
        public const int DEFAULT_TIMEOUT = 30;

        public StandardPayload()
            : this(default(SMQActorBase))
        {

        }

        public StandardPayload(SMQActorBase actor)
            : this(actor, String.Empty)
        {

        }
        
        public StandardPayload(SMQActorBase actor, string content)
            : this(actor, content, true)
        {
        }
        
        public string PayloadId { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string AccessToken { get; set; }
        public string Content { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsHandled { get; set; }
        

        private SMQActorBase __Actor { get; set; }
        private bool TimedOutWaiting { get; set; }
        internal StandardPayload ReplyPayload { get; set; }
        private BasicDeliverEventArgs ReplyBDEA { get; set; }
        private bool ReplyRecieved { get; set; }
        
        public DateTime OnlineSince { get; set; }
        public string EmailAddress { get; set; }
        public string DemoPassword { get; set; }
        public string AirtableWhere { get; set; }
        public object SingletonAppUser { get; set; }
        public string WhoAreYouRelativeUrl { get; set; }
        public string WhoAreYouTextFileContents { get; set; }
        public byte[] WhoAreYouBinaryFileContents { get; set; }
        public int MaxPages { get; set; }
        public string View { get; set; }

        public void SetActor(SMQActorBase actor) 
        {
            this.__Actor = actor;
        }

        internal bool HasNoErrors(BasicDeliverEventArgs bdea)
        {
            if (!String.IsNullOrEmpty(this.ErrorMessage)) return false;
            else return true;
        }
    }
}