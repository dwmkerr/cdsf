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
            get { return this.resourceSets.Values; } 
        }

        public IEnumerable<ServiceOperation> ServiceOperations
        {
            // No service operations yet 
            get { yield break; } 
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
            // No service operations are supported yet 
            serviceOperation = null;
            return false; 
        }

        public IEnumerable<ResourceType> Types
        {
            get { return this.resourceTypes.Values; } 
        }

        public void AddResourceType(ResourceType type)
        {
            type.SetReadOnly();
            resourceTypes.Add(type.FullName, type);
        }

        public void AddResourceSet(ResourceSet set)
        {
            set.SetReadOnly();
            resourceSets.Add(set.Name, set);
        }

        private Dictionary<string, ResourceType> resourceTypes = 
            new Dictionary<string, ResourceType>();

        private Dictionary<string, ResourceSet> resourceSets = 
            new Dictionary<string, ResourceSet>();
    }
}
