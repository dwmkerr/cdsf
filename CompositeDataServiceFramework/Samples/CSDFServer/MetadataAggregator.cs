using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Metadata.Edm;
using System.Xml.Serialization;
using System.IO;

namespace CSDFServer
{
  public class MetadataAggregator
  {
    public void AggregateMetadata(string metadata)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(MetadataWorkspace));
      workspace = (MetadataWorkspace)serializer.Deserialize(new StringReader(metadata));
    }

    protected MetadataWorkspace workspace = new MetadataWorkspace();
  }
}
