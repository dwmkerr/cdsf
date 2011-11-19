using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Metadata.Edm;
using System.Net;
using System.Xml.XPath;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace CompositeDataServiceFramework.Server
{
  public class WcfDataServiceMetadataLoader
  {
    public bool LoadMetadata(Uri serviceUri)
    {
      //  Create the metadata uri.
      Uri metadataUri = new Uri(serviceUri.ToString() + "/$metadata");

      //  Request the metadata.
      WebClient webClient = new WebClient();
      string metadata = webClient.DownloadString(metadataUri.ToString());

      //  Prepare a set of xml readers.
      List<XmlReader> csdlReaders = new List<XmlReader>();

      var csdlElements = ExtractCsdlFromEdmx(metadata);
      if (csdlElements != null)
      {
        //  Create a reader for the csdl.
        foreach(var csdlElement in csdlElements)
          csdlReaders.Add(XElementToXmlReader(csdlElement));
      }

      //  Create the EDM item collection, loaded from the CSDL fragments.
      edmItemCollection = new EdmItemCollection(csdlReaders);

      //  Bingo.
      return true;
    }

    private XmlReader XElementToXmlReader(XElement element)
    {
      StringBuilder sb = new StringBuilder();
      using (StringWriter writer = new StringWriter(sb))
      {
        element.Save(writer);
      }
      StringReader reader = new StringReader(sb.ToString());
      XmlReader xreader = new XmlTextReader(reader);

      return xreader;
    }

    private IEnumerable<XElement> ExtractCsdlFromEdmx(string edmxFile)
    {
      XDocument edmxDoc = XDocument.Load(new StringReader(edmxFile));
      if (edmxDoc != null)
      {
        XElement edmxNode = edmxDoc.Element(edmxNamespace + "Edmx");
        if (edmxNode != null)
        {
          XElement dataServicesNode = edmxNode.Element(edmxNamespace + "DataServices");
          if (dataServicesNode != null)
          {
            return dataServicesNode.Nodes().OfType<XElement>();
          }
        }
      }
      return null;
    }

    private readonly XNamespace edmxNamespace = "http://schemas.microsoft.com/ado/2007/06/edmx";
    private readonly XNamespace edmxCsdlNamespace = "http://schemas.microsoft.com/ado/2006/04/edm";
    private readonly XNamespace edmxSsdlNamespace = "http://schemas.microsoft.com/ado/2006/04/edm/ssdl";
    private readonly XNamespace edmxMslNamespace = "urn:schemas-microsoft-com:windows:storage:mapping:CS";

    private EdmItemCollection edmItemCollection;

    public EdmItemCollection EdmItemCollection
    {
      get { return edmItemCollection; }
    }

    public IEnumerable<EntityContainer> EntityContainers
    {
      get { return edmItemCollection.OfType<EntityContainer>(); }
    }

    public IEnumerable<EntitySet> EntitySets
    {
      get 
      {
        List<EntitySet> sets = new List<EntitySet>();
        foreach (var container in EntityContainers)
          sets.AddRange(container.BaseEntitySets.OfType<EntitySet>());
        return sets;
      }
    }

    public IEnumerable<EdmFunction> FunctionImports
    {
        get
        {
            List<EdmFunction> sets = new List<EdmFunction>();
            foreach (var container in EntityContainers)
                sets.AddRange(container.FunctionImports);
            return sets;
        }
    }

    public IEnumerable<EntityType> EntityTypes
    {
      get { return edmItemCollection.OfType<EntityType>(); }
    }

    public IEnumerable<AssociationType> AssociationTypes
    {
        get { return edmItemCollection.OfType<AssociationType>(); }
    }
  }
}
