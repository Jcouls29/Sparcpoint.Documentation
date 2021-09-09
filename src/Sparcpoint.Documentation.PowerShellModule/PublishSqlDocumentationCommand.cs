using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.IO;
using System.IO;
using System.Management.Automation;
using System.Reflection;

namespace Sparcpoint.Documentation.PowerShellModule
{
    [Cmdlet(VerbsData.Publish, "SqlDocumentation")]
    public class PublishSqlDocumentationCommand : PSCmdlet
    {
        [Parameter(Mandatory = false)]
        public string TemplatePath { get; set; } = string.Empty;

        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string[] Input { get; set; }

        [Parameter(Mandatory = true)]
        public string Output { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter ExcludeIndexPage { get; set; } = false;

        private List<string> _SqlFiles { get; } = new List<string>();

        protected override void ProcessRecord()
        {
            _SqlFiles.AddRange(Input);
        }

        protected override void EndProcessing()
        {
            string fullPath = string.Empty;

            if (string.IsNullOrWhiteSpace(TemplatePath))
                fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @".\Templates\Default");
            else if (Path.IsPathRooted(TemplatePath))
                fullPath = TemplatePath;
            else
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), TemplatePath);

            if (!Directory.Exists(fullPath))
                ThrowTerminatingError(new ErrorRecord(new Exception("Template directory does not exist."), "Not Exists", ErrorCategory.ObjectNotFound, TemplatePath));

            try
            {
                var command = new BuildSql.BuildSqlCommand(new BuildSql.BuildSqlCommandOptions
                {
                    NoIndexPage = ExcludeIndexPage,
                    Output = Output,
                    Input = _SqlFiles,
                    Template = fullPath,
                    Verbose = false
                }, new ConsoleHost(this));

                command.Run().Wait();
            } catch (AggregateException ex)
            {
                ThrowTerminatingError(new ErrorRecord(ex.InnerExceptions[0], "", ErrorCategory.ObjectNotFound, this));
            }
        }

        private class ConsoleHost : IConsole
        {
            private readonly PSCmdlet _Cmdlet;

            public ConsoleHost(PSCmdlet cmdlet)
            {
                _Cmdlet = cmdlet ?? throw new ArgumentNullException(nameof(cmdlet));

                Out = new ConsoleWriter((message) =>
                {
                    cmdlet.WriteObject(message);
                });

                Error = new ConsoleWriter((message) =>
                {
                    cmdlet.WriteInformation(new InformationRecord(message, nameof(PublishSqlDocumentationCommand)));
                });
            }

            public IStandardStreamWriter Out { get; }
            public bool IsOutputRedirected => false;
            public IStandardStreamWriter Error { get; }
            public bool IsErrorRedirected => false;
            public bool IsInputRedirected => false;
        }

        private class ConsoleWriter : IStandardStreamWriter
        {
            private readonly Action<string> _Writer;

            public ConsoleWriter(Action<string> writer)
            {
                _Writer = writer ?? throw new ArgumentNullException(nameof(writer));
            }

            public void Write(string value)
            {
                _Writer(value);
            }
        }
    }

    // This class controls our dependency resolution
    public class ModuleContextHandler : IModuleAssemblyInitializer, IModuleAssemblyCleanup
    {
        // We catalog our dependencies here to ensure we don't load anything else
        private static IReadOnlyDictionary<string, int> s_dependencies = new Dictionary<string, int>
        {
            { "Microsoft.Extensions.DependencyInjection", 3 },
            { "Microsoft.Extensions.DependencyInjection.Abstractions", 3 },
        };

        // Set up the path to our dependency directory within the module
        private static string s_dependenciesDirPath = Path.GetFullPath(
            Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

        // This makes sure we only try to resolve dependencies when the module is loaded
        private static bool s_engineLoaded = false;

        public void OnImport()
        {
            // Set up our event when the module is loaded
            AppDomain.CurrentDomain.AssemblyResolve += HandleResolveEvent;
        }

        public void OnRemove(PSModuleInfo psModuleInfo)
        {
            // Unset the event when the module is unloaded
            AppDomain.CurrentDomain.AssemblyResolve -= HandleResolveEvent;
        }

        private static Assembly HandleResolveEvent(object sender, ResolveEventArgs args)
        {
            var asmName = new AssemblyName(args.Name);

            // Otherwise, if that assembly has been loaded, we must try to resolve its dependencies too
            string asmPath = Path.Combine(s_dependenciesDirPath, $"{asmName.Name}.dll");

            if (File.Exists(asmPath))
                return Assembly.LoadFile(asmPath);

            return null;
        }
    }
}
