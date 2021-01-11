using CLIClassLibrary.RoleHandlers;
using Newtonsoft.Json;
using Plossum.CommandLine;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using YP.SassyMQ.Lib.RabbitMQ;

namespace SSoTme.Default.Lib.CLIHandler
{
    public abstract partial class EAPICLIHandlerBase<T> : EAPICLIHandlerBase
    {
        private RoleHandlerBase _roleHandler;
        private T _loggedInUser;

        public EAPICLIHandlerBase(string[] args)
        {
            this.amqps = $"amqps://smqPublic:smqPublic@effortlessapi-rmq.ssot.me/{EAPICLIHandler.C_PROJECT_NAME}";
            var list = args.ToList();
            list.Insert(0, "cli");
            this.Parser = new CommandLineParser(this);
            this.Parser.Parse(list.ToArray());
        }

        internal static string GetMostRecentUser()
        {
            var di = new DirectoryInfo(ProjectRootPath);
            if (!di.Exists) di.Create();
            var lastModified = di.GetFiles().OrderByDescending(fi => fi.LastWriteTime).FirstOrDefault();
            if (lastModified is null) return "Guest";
            else
            {
                var lastName = Path.GetFileNameWithoutExtension(lastModified.Name);
                return lastName;
            }
        }

        internal static string GetToken(string runas)
        {
            var root = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var fileInfo = new FileInfo(Path.Combine(ProjectRootPath, $"{runas}.token"));
            if (!fileInfo.Directory.Exists) fileInfo.Directory.Create();
            if (!fileInfo.Exists) return String.Empty;
            else
            {
                var accessToken = File.ReadAllText(fileInfo.FullName);
                File.WriteAllText(fileInfo.FullName, accessToken);
                return accessToken;
            }
        }

        public string ProcessRequest()
        {
            this.SetDefaultCLIParameters();
            if (this.help) return this.ShowHelp();
            else if (this.reloadCache) return this.ReloadCacheNow();
            else if (this.whoami) return this.CheckWhoIAmNow();
            else if (!String.IsNullOrEmpty(this.action)) return this.HandleAction();
            else if (!String.IsNullOrEmpty(this.authenticate)) return this.Authenticate();
            else return this.HandleCustomRequest();
        }

        protected abstract string HandleAction();

        protected abstract string HandleCustomRequest();

        private string CheckWhoIAmNow()
        {
            if (!this.AccountFileInfo.Exists) throw new Exception($"Must must first authenticate as {runas}");
            else
            {
                var json = File.ReadAllText(this.AccountFileInfo.FullName);
                return JsonConvert.SerializeObject(new { WhoAmI = JsonConvert.DeserializeObject(json), Role = this.runas }, Formatting.Indented);
            }
        }

        public T LoggedInUser
        {
            get
            {
                if (_loggedInUser is null)
                {
                    if (!AccountFileInfo.Exists) throw new Exception("Must authenticate and check who-am-i before this will work.");

                    var accountJson = File.ReadAllText(AccountFileInfo.FullName);
                    _loggedInUser = JsonConvert.DeserializeObject<T>(accountJson);
                }
                return _loggedInUser;
            }
            set => _loggedInUser = value;
        }

        private string ReloadCacheNow()
        {
            // Load Static Data Here
            var success = this.CustomReloadCache();

            return "{\"Success\":true}";
        }

        protected abstract bool CustomReloadCache();

        public static DirectoryInfo RootFileInfo
        {
            get
            {
                var di = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".eapi"));
                if (!di.Exists) di.Create();
                return di;
            }
        }

        public FileInfo AccountFileInfo
        {
            get { return new FileInfo(Path.Combine(ProjectRootPath, $"{runas}.json")); }
        }

        public string ShowHelp()
        {
            var customHelp = this.GetCustomHelp();
            if (!string.IsNullOrEmpty(customHelp)) return customHelp; 
            else return GetStandardHelp();
        }

        protected virtual string GetCustomHelp()
        {
            return null;
        }

        public string GetStandardHelp()
        {
            var sbHelpBuilder = new StringBuilder();
            var currentAssembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(currentAssembly.Location);
            sbHelpBuilder.AppendLine($"{EAPICLIHandler.C_PROJECT_NAME} CLI Help: {fvi.FileVersion}.");
            sbHelpBuilder.AppendLine();
            var helpTerm = this.Parser.RemainingArguments.FirstOrDefault();
            if (String.IsNullOrEmpty(helpTerm)) helpTerm = "general";
            this.RoleHandler.AddHelp(sbHelpBuilder, helpTerm);
            if (helpTerm == "general")
            {
                sbHelpBuilder.AppendLine();
                sbHelpBuilder.AppendLine();
                sbHelpBuilder.AppendLine($"Syntax:");
                sbHelpBuilder.AppendLine(this.Parser.UsageInfo.ToString());
                sbHelpBuilder.AppendLine();
                sbHelpBuilder.AppendLine($"Available Roles:");
                RoleHandlerFactory.ListRoles(sbHelpBuilder);
            }
            return sbHelpBuilder.ToString();
        }

        private void SetDefaultCLIParameters()
        {
            var firstArgument = this.Parser.RemainingArguments.FirstOrDefault();

            if (String.Equals(firstArgument, "help", StringComparison.OrdinalIgnoreCase))
            {
                this.help = true;
                this.Parser.RemainingArguments.RemoveAt(0);
            }
            if (String.Equals(firstArgument, "reloadCache", StringComparison.OrdinalIgnoreCase))
            {
                this.reloadCache = true;
                this.Parser.RemainingArguments.RemoveAt(0);
            }

            if (String.IsNullOrEmpty(this.runas)) this.runas = GetMostRecentUser();
            if (String.IsNullOrEmpty(this.runas)) this.runas = "guest";
            this.runas = this.runas.ToLower();

            firstArgument = this.Parser.RemainingArguments.FirstOrDefault();
            this.CheckDefaultCLIParameters(firstArgument);
        }

        protected abstract void CheckDefaultCLIParameters(string firstArgument);

        public string Authenticate()
        {
            var smqGuest = new SMQGuest(this.amqps);
            var authPayload = smqGuest.CreatePayload();
            authPayload.EmailAddress = this.authenticate;
            authPayload.DemoPassword = this.demoPassword;
            var result = "";
            smqGuest.ValidateTemporaryAccessToken(authPayload, (reply, bdea) =>
            {
                if (reply.HasNoErrors(bdea))
                {
                    this.SaveAuth(reply.AccessToken);
                    smqGuest.AccessToken = reply.AccessToken;
                    var whoAmIPL = smqGuest.CreatePayload();
                    smqGuest.WhoAmI(whoAmIPL, (whoAmIReply, whoAmIBDEA) =>
                    {
                        if (whoAmIReply.HasNoErrors(whoAmIBDEA))
                        {
                            var accountJSON = JsonConvert.SerializeObject(whoAmIReply.SingletonAppUser, Formatting.Indented);
                            File.WriteAllText(AccountFileInfo.FullName, accountJSON);
                        }
                    }).Wait(30000);
                }
                result = JsonConvert.SerializeObject(reply, Formatting.Indented, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }).Wait(30000);
            return result;
        }

        private void SaveAuth(string accessToken)
        {
            var fileInfo = new FileInfo(Path.Combine(ProjectRootPath, $"{this.runas}.token"));
            File.WriteAllText(fileInfo.FullName, accessToken);
        }

        public CommandLineParser Parser { get; protected set; }
        internal RoleHandlerBase RoleHandler
        {
            get
            {
                if (_roleHandler is null)
                {
                    _roleHandler = RoleHandlerFactory.CreateHandler(this.runas, this.amqps);
                }
                return _roleHandler;
            }
            private set => _roleHandler = value;
        }

        public static string RootPath { get { return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); } }

        public static string ProjectRootPath
        {
            get
            {
                var di = new DirectoryInfo(Path.Combine(RootPath, ".eapi", $"{EAPICLIHandler.C_PROJECT_NAME}"));
                if (!di.Exists) di.Create();
                return di.FullName;
            }
        }

        public static void HandleRequest(string[] args)
        {
            var handler = new EAPICLIHandler(args);
            Console.WriteLine(handler.ProcessRequest());
        }
    }

    public partial class EAPICLIHandlerBase
    {

    }
}