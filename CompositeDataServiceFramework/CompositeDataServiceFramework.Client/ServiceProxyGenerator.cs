using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace CompositeDataServiceFramework.Client
{
    public enum ServiceProxyLanguage
    {
        CSharp,
        VB
    }

    public class ServiceProxyGenerator
    {
        /// <summary>
        /// Gets or sets a value indicating whether the service should support data binding.
        /// </summary>
        /// <value>
        ///   <c>true</c> if data binding is supported; otherwise, <c>false</c>.
        /// </value>
        public bool SupportDataBinding
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the output file path.
        /// </summary>
        /// <value>
        /// The output file path.
        /// </value>
        public string OutputFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the output file contents.
        /// </summary>
        /// <value>
        /// The output file contents.
        /// </value>
        public string OutputFileContents
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the proxy language.
        /// </summary>
        /// <value>
        /// The proxy language.
        /// </value>
        public ServiceProxyLanguage ProxyLanguage
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the service URI.
        /// </summary>
        /// <value>
        /// The service URI.
        /// </value>
        public string ServiceUri
        {
            get;
            set;
        }

        /// <summary>
        /// Generates this instance.
        /// </summary>
        public void Generate()
        {
            //  Create the temporary output file path.
            OutputFilePath = Path.GetTempFileName();

            //  Generate the command string.
            string commandPath = GetDataServiceUtilityPath();
            string commandString = BuildCommandArguments();

            //  Execute the command.
            Process process = Process.Start(commandPath, commandString);

            //  Wait for the process to end.
            process.WaitForExit();

            //  Get the contents of the newly generated file.
            using (FileStream stream = new FileStream(OutputFilePath, FileMode.Open))
            {
                //  Create the reader.
                using (TextReader reader = new StreamReader(stream))
                {
                    //  Get the file contents.
                    OutputFileContents = reader.ReadToEnd();
                }
            }
        }

        private string BuildCommandArguments()
        {
            //  Create the command.
            string command = string.Empty;

            //  If we are enabling data binding, add the data service collection flag.
            if (SupportDataBinding)
                command += @" /dataservicecollection";

            //  Always use version 2.0.
            command += " /Version:2.0";

            //  Set the language.
            command += @" /language:" + ProxyLanguage.ToString();

            //  Set the output file name.
            command += @" /out:" + OutputFilePath;

            //  Set the service uri.
            command += @" /uri:" + ServiceUri;

            //  Return the command string.
            return command;
        }

        private string GetDataServiceUtilityPath()
        {
            //  Get the windir path.
            string winpath = Environment.GetEnvironmentVariable("windir");

            //  Return the data service utility path.
            return winpath + @"\Microsoft.NET\Framework\v3.5\DataSvcUtil.exe";
        }
    }
}
