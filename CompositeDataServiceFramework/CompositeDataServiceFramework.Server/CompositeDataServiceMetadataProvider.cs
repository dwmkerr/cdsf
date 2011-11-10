using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public class CompositeDataServiceMetadataProvider : IDataServiceMetadataProvider
    {
        public string ContainerName
        {
            get { return "CompositeDataServiceContainer"; }
        }

        public string ContainerNamespace
        {
            get { return "CompositeDataServiceContainerNamespace"; }
        }

        public IEnumerable<ResourceType> GetDerivedTypes(ResourceType resourceType)
        {
            // We don't support type inheritance yet 
            yield break; 
        }

        public ResourceAssociationSet GetResourceAssociationSet(ResourceSet resourceSet, ResourceType resourceType, ResourceProperty resourceProperty)
        {
            throw new NotImplementedException("No relationships."); 
        }

        public bool HasDerivedTypes(ResourceType resourceType)
        {
            // We don’t support inheritance yet 
            return false; 
        }

        public IEnumerable<ResourceSet> ResourceSets
        {
            get { return resourceSets.Values; } 
        }

        public IEnumerable<ServiceOperation> ServiceOperations
        {
            get { return serviceOperations.Values; }
        }

        public IEnumerable<ResourceType> Types
        {
            get { return resourceTypes.Values; }
        }

        public bool TryResolveResourceSet(string name, out ResourceSet resourceSet)
        {
            return resourceSets.TryGetValue(name, out resourceSet); 
        }

        public bool TryResolveResourceType(string name, out ResourceType resourceType)
        {
            return resourceTypes.TryGetValue(name, out resourceType); 
        }

        public bool TryResolveServiceOperation(string name, out ServiceOperation serviceOperation)
        {
            return serviceOperations.TryGetValue(name, out serviceOperation);
        }

        public bool TryResolveResourceSet(string name, out ResourceSet resourceSet, out CompositeDataSource source)
        {
            resourceSet = null;
            source = null;
            if (resourceSets.TryGetValue(name, out resourceSet) == false)
                return false;
            if (resourceSetSources.TryGetValue(name, out source) == false)
                return false;
            return true;
        }

        public void AddResourceType(ResourceType type, CompositeDataSource source)
        {
            type.SetReadOnly();
            resourceTypes.Add(type.Name, type);
            resourceTypeSources.Add(type.FullName, source);
        }

        public void AddResourceSet(ResourceSet set, CompositeDataSource source)
        {
            set.SetReadOnly();
            resourceSets.Add(set.Name, set);
            resourceSetSources.Add(set.Name, source);
        }

        public void AddServiceOperation(ServiceOperation serviceOperation, CompositeDataSource source)
        {
          serviceOperation.SetReadOnly();
          serviceOperations.Add(serviceOperation.Name, serviceOperation);
          serviceOperationSources.Add(serviceOperation.Name, source);
        }

        private Dictionary<string, ResourceType> resourceTypes = 
            new Dictionary<string, ResourceType>();

        private Dictionary<string, ResourceSet> resourceSets = 
            new Dictionary<string, ResourceSet>();

        private Dictionary<string, ServiceOperation> serviceOperations =
            new Dictionary<string, ServiceOperation>();

        private Dictionary<string, CompositeDataSource> resourceTypeSources
            = new Dictionary<string, CompositeDataSource>();

        private Dictionary<string, CompositeDataSource> resourceSetSources
            = new Dictionary<string, CompositeDataSource>();

        private Dictionary<string, CompositeDataSource> serviceOperationSources
            = new Dictionary<string, CompositeDataSource>();
    }
}
