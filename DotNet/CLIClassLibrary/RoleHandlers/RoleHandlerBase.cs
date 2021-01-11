using Newtonsoft.Json;
using SSoTme.Default.Lib.CLIHandler;
using System;
using System.Collections.Generic;
using System.Text;
using YP.SassyMQ.Lib.RabbitMQ;

namespace CLIClassLibrary.RoleHandlers
{
    public abstract class RoleHandlerBase
    {
        public abstract string Handle(string invoke, string data, string where, int maxPages, string view);

        protected string SerializePayload(StandardPayload reply)
        {
            return JsonConvert.SerializeObject(reply, Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public abstract void AddHelp(StringBuilder sb, string helpTerm);
    }

    public abstract class RoleHandlerBase<T> : RoleHandlerBase
        where T : SMQActorBase
    {
        private T _smqActor;

        public T SMQActor
        {
            get { 
                if (_smqActor is null)
                {
                    _smqActor = Activator.CreateInstance(typeof(T), this.AMQPS) as T;
                    this.SMQActor.AccessToken = this.AccessToken;
                }
                return _smqActor; }
        }

        public RoleHandlerBase(string amqps, string accessToken)
        {
            this.AMQPS = amqps;
            this.AccessToken = accessToken;
        }

        public string AMQPS { get; }
        public string AccessToken { get; }
    }
}
