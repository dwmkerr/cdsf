using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Data.Metadata.Edm;
using System.Xml;

namespace CSDFServer
{
  public class EdmxFile
  {
    public void Load(string data)
    {
        var xelement = ExtractCsdlFromEdmx(data);
        var xreader = XElementToXmlReader(xelement);
        EdmItemCollection eic = new EdmItemCollection(new List<XmlReader> { xreader });
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

    private XElement ExtractCsdlFromEdmx(string edmxFile)
    {
        XElement schemaNode = null;
 
      XDocument edmxDoc = XDocument.Load(new StringReader(edmxFile));
      if (edmxDoc != null)
      {
          XElement edmxNode = edmxDoc.Element(edmxNamespace + "Edmx");
          if (edmxNode != null)
          {
              XElement dataServicesNode = edmxNode.Element(edmxNamespace + "DataServices");
              if (dataServicesNode != null)
              {
                  schemaNode = (XElement)dataServicesNode.Nodes().FirstOrDefault();
                  return schemaNode;
              }
          }
      }
      return schemaNode;
    }

    private XElement ExtractCsdlContent(string edmxFile)
    {
      XElement csdlContent = null;
 
      XDocument edmxDoc = XDocument.Load(new StringReader(edmxFile));
      if (edmxDoc != null)
      {
        XElement edmxNode = edmxDoc.Element(edmxNamespace + "Edmx");
       if (edmxNode != null)
       {
         XElement runtimeNode = edmxNode.Element(edmxNamespace + "Runtime");
         if (runtimeNode != null)
         {
           XElement conceptualModelsNode = runtimeNode.Element(edmxNamespace +
            "ConceptualModels");
          if (conceptualModelsNode != null)
          {
            csdlContent = conceptualModelsNode.Element(edmxCsdlNamespace + "Schema");
          }
         }
       }
      }
      return csdlContent;
    }

    private XElement ExtractSsdlContent(string edmxFile)
    {
      XElement ssdlContent = null;

      XDocument edmxDoc = XDocument.Load(new StringReader(edmxFile));
      if (edmxDoc != null)
      {
        XElement edmxNode = edmxDoc.Element(edmxNamespace + "Edmx");
        if (edmxNode != null)
        {
          XElement runtimeNode = edmxNode.Element(edmxNamespace + "Runtime");
          if (runtimeNode != null)
          {
            XElement conceptualModelsNode = runtimeNode.Element(edmxNamespace +
             "StorageModels");
            if (conceptualModelsNode != null)
            {
              ssdlContent = conceptualModelsNode.Element(edmxSsdlNamespace + "Schema");
            }
          }
        }
      }
      return ssdlContent;
    }

    private XElement ExtractMslContent(string edmxFile)
    {
      XElement ssdlContent = null;

      XDocument edmxDoc = XDocument.Load(new StringReader(edmxFile));
      if (edmxDoc != null)
      {
        XElement edmxNode = edmxDoc.Element(edmxNamespace + "Edmx");
        if (edmxNode != null)
        {
          XElement runtimeNode = edmxNode.Element(edmxNamespace + "Runtime");
          if (runtimeNode != null)
          {
            XElement conceptualModelsNode = runtimeNode.Element(edmxNamespace +
             "StorageModels");
            if (conceptualModelsNode != null)
            {
              ssdlContent = conceptualModelsNode.Element(edmxSsdlNamespace + "Schema");
            }
          }
        }
      }
      return ssdlContent;
    }

    private readonly XNamespace edmxNamespace = "http://schemas.microsoft.com/ado/2007/06/edmx";
    private readonly XNamespace edmxCsdlNamespace = "http://schemas.microsoft.com/ado/2006/04/edm";
    private readonly XNamespace edmxSsdlNamespace = "http://schemas.microsoft.com/ado/2006/04/edm/ssdl";
    private readonly XNamespace edmxMslNamespace = "urn:schemas-microsoft-com:windows:storage:mapping:CS";
  }
}
