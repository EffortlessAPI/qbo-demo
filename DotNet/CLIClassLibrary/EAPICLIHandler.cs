using CLIClassLibrary.RoleHandlers;
using EAPI.CLI.Lib.DataClasses;
using Newtonsoft.Json;
using Plossum.CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YP.SassyMQ.Lib.RabbitMQ;

namespace SSoTme.Default.Lib.CLIHandler
{

    public partial class EAPICLIHandler : EAPICLIHandlerBase<AppUser>
    {
        public static string C_PROJECT_NAME = "demo-sd1";

        public EAPICLIHandler(string[] args)
            : base(args)
        {
            
        }

        protected override string HandleCustomRequest()
        {
            if (!String.IsNullOrEmpty(this.action)) return this.HandleAction();
            else throw new Exception($"Sytnax error: cli -action XYZ -bodyData {{...}} -as Admin");

        }

        protected override string HandleAction()
        {
            if (!String.IsNullOrEmpty(this.bodyFile))
            {
                var fileInfo = new FileInfo(this.bodyFile);
                if (!fileInfo.Exists) throw new Exception($"-bodyFile {fileInfo.FullName} does not exists.");
                else if (String.IsNullOrEmpty(this.bodyData)) this.bodyData = File.ReadAllText(fileInfo.FullName);
            }
            var result = this.RoleHandler.Handle(this.action, this.bodyData, this.where, this.maxpages, this.view);
            if (!String.IsNullOrEmpty(this.output)) File.WriteAllText(this.output, result);
            return result;
        }


        protected override string GetCustomHelp()
        {
            return this.GetStandardHelp();
        }

        protected override bool CustomReloadCache()
        {
            var smqGuest = new SMQGuest(this.amqps);

            //  - smqGuest.GetProducts();

            // or load other "slow moving" data available globally.
            return true;
        }

        protected override void CheckDefaultCLIParameters(string firstArgument)
        {
            if (!this.help &&
                !this.reloadCache &&
                !this.whoami &&
                String.IsNullOrEmpty(this.authenticate) &&
                String.IsNullOrEmpty(this.action) &&
                !String.IsNullOrEmpty(firstArgument))
            {
                if (String.IsNullOrEmpty(firstArgument))
                {
                    this.help = true;
                }
                else
                {
                    this.action = firstArgument;
                }
            }
        }
    }
}
