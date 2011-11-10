using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;
using System.Data.Metadata.Edm;
using System.Data.Objects;

namespace CompositeDataServiceFramework.Server
{
  public static class EntityToResourceMapping
  {

    public static ResourceType MapEntityType(EntityType entityType, ObjectContext context, string namespaceName)
    {
      //  *** How do we get the CLR tye/
      string parentNamespace = context.GetType().Namespace;
      var ss = context.GetType().Assembly;
      var type = (from t in ss.GetTypes() where t.Name == entityType.Name select t).FirstOrDefault(); 

      //  Create the resource type.
      ResourceType resourceType = new ResourceType(
        type,
        ResourceTypeKind.EntityType,
        null, // base types not supported.
        namespaceName,
        entityType.Name,
        entityType.Abstract
        );

      //  Add each property.
      //  ***TODO, keys, complex types.
      foreach (var propertyType in entityType.Properties)
      {
        var resourceProperty = new ResourceProperty(
               propertyType.Name,
               ResourcePropertyKind.Key |
               ResourcePropertyKind.Primitive,
               MapEdmType(propertyType.TypeUsage.EdmType)
            );
        resourceType.AddProperty(resourceProperty);
      }

      return resourceType;
    }

    private static ResourceType MapEdmType(EdmType edmType)
    {
      if (edmType is PrimitiveType)
        return ResourceType.GetPrimitiveResourceType(((PrimitiveType)edmType).ClrEquivalentType);
      throw new Exception("Don't know type " + edmType.Name);
    }
  }
}
