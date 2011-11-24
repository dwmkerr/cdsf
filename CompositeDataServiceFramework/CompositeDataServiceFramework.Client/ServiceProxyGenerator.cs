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

        public string OutputFilePath
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
            //  Create a temporary file.
            string outputFilePath = Path.GetTempFileName();

            //  Generate the command string.
            string commandString = BuildCommandString(outputFilePath);

            //  Execute the command.
            Process process = System.Diagnostics.Process.Start(commandString);

            //  Wait for the process to end.
            process.WaitForExit();
        }

        private string BuildCommandString()
        {
            //  Create the command.
            string command = @"%windir%\Microsoft.NET\Framework\v3.5\DataSvcUtil.exe ";

            //  If we are enabling data binding, add the data service collection flag.
            if (SupportDataBinding)
                command += @"/dataservicecollection ";

            //  Set the language.
            command += @"/language:" + ProxyLanguage.ToString();

            //  Set the output file name.
            command += @"/out:" + OutputFilePath;

            //  Set the service uri.
            command += @"/uri:" + ServiceUri;

            //  Return the command string.
            return command;
        }
    }
}
