using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;

namespace CompositeDataService
{
    public class MetadataProvider
    {
        /// <summary>
        /// Appends the metadata.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public void AppendMetadata(string metadata)
        {
            //  Create a reader for the metadata.
            XmlReader reader = new XmlTextReader(new StringReader(metadata));

            //  Read each node.
            while (reader.Read())
            {

            }
        }

        XmlDocument metadata = new XmlDocument();
    }
}