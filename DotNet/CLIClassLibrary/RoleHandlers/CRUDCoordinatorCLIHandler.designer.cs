using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using EAPI.CLI.Lib.DataClasses;
using YP.SassyMQ.Lib.RabbitMQ;

namespace CLIClassLibrary.RoleHandlers
{
    public partial class CRUDCoordinatorCLIHandler
    {
        public override void AddHelp(StringBuilder sb, string helpTerm)
        {
            sb.AppendLine($"Help for CRUDCoordinator.");
            
            helpTerm = helpTerm.ToLower();
            var found = helpTerm == "general";
            
            if (helpTerm == "general")
            {
                sb.AppendLine();
                
                sb.AppendLine($"void: ResetRabbitSassyMQConfiguration");
                sb.AppendLine($"void: ResetJWTSecretKey");                                            
            }
            
            sb.AppendLine($"{Environment.NewLine}Available Actions Matching: {helpTerm}");
            
            if ("resetrabbitsassymqconfiguration".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - ResetRabbitSassyMQConfiguration");
                if ("resetrabbitsassymqconfiguration".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.PrintResetRabbitSassyMQConfigurationHelp(sb);
                }
                found = true;
            }
            if ("resetjwtsecretkey".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - ResetJWTSecretKey");
                if ("resetjwtsecretkey".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.PrintResetJWTSecretKeyHelp(sb);
                }
                found = true;
            }
                       
            if (!found)
            {
                sb.AppendLine();
                sb.AppendLine($"{Environment.NewLine}UNABLE TO FIND COMMAND: {helpTerm} not found.");
            }
        }

        private string HandlerFactory(string invokeRequest, string payloadString, string where, int maxPages, string view)
        {
            var result = "";
            var payload = JsonConvert.DeserializeObject<StandardPayload>(payloadString);
            payload.SetActor(this.SMQActor);
            payload.AccessToken = this.SMQActor.AccessToken;
            payload.AirtableWhere = where;
            payload.MaxPages = maxPages;
            payload.View = view;

            switch (invokeRequest.ToLower())
            {
                case "resetrabbitsassymqconfiguration":
                    this.SMQActor.ResetRabbitSassyMQConfiguration(payload, (reply, bdea) =>
                    {
                        result = SerializePayload(reply);
                    }).Wait(30000);
                    break;                   

                case "resetjwtsecretkey":
                    this.SMQActor.ResetJWTSecretKey(payload, (reply, bdea) =>
                    {
                        result = SerializePayload(reply);
                    }).Wait(30000);
                    break;                   

                default:
                    throw new Exception($"Invalid request: {invokeRequest}");
            }

            return result;
        }
        
        
        public void PrintResetRabbitSassyMQConfigurationHelp(StringBuilder sb)
        {
            
        }
        
        public void PrintResetJWTSecretKeyHelp(StringBuilder sb)
        {
            
        }
        

    }
}
