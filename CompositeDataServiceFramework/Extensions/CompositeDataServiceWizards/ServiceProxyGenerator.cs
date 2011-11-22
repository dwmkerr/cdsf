using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CompositeDataServiceWizards
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
        /// Gets or sets the name of the output file.
        /// </summary>
        /// <value>
        /// The name of the output file.
        /// </value>
        public string OutputFileName
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
            //  Generate the command string.
            string commandString = GenerateCommandString();

            //  Execute the command.
            Process process = System.Diagnostics.Process.Start(commandString);

            //  ***
        }

        private string GenerateCommandString()
        {
            //  Create the command.
            string command = @"%windir%\Microsoft.NET\Framework\v3.5\DataSvcUtil.exe ";

            //  If we are enabling data binding, add the data service collection flag.
            if (SupportDataBinding)
                command += @"/dataservicecollection ";

            //  Set the language.
            command += @"/language:" + ProxyLanguage.ToString();

            //  Set the output file name.
            command += @"/out:" + OutputFileName;

            //  Set the service uri.
            command += @"/uri:" + ServiceUri;

            //  Return the command string.
            return command;
        }
    }
}
