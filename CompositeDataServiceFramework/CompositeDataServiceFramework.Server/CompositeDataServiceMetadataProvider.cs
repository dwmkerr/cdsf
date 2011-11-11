using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public class CompositeDataServiceMetadataProvider : IDataServiceMetadataProvider
    {
        /// <summary>
        /// Container name for the data source.
        /// </summary>
        /// <returns>String that contains the name of the container.</returns>
        public string ContainerName
        {
            get { return "CompositeDataServiceContainer"; }
        }

        /// <summary>
        /// Namespace name for the data source.
        /// </summary>
        /// <returns>String that contains the namespace name.</returns>
        public string ContainerNamespace
        {
            get { return "CompositeDataServiceContainerNamespace"; }
        }

        /// <summary>
        /// Attempts to return all types that derive from the specified resource type.
        /// </summary>
        /// <param name="resourceType">The base <see cref="T:System.Data.Services.Providers.ResourceType"/>.</param>
        /// <returns>
        /// An <see cref="T:System.Collections.Generic.IEnumerable`1"/> collection of derived <see cref="T:System.Data.Services.Providers.ResourceType"/> objects.
        /// </returns>
        public IEnumerable<ResourceType> GetDerivedTypes(ResourceType resourceType)
        {
            // We don't support type inheritance yet 
            yield break; 
        }

        /// <summary>
        /// Gets the <see cref="T:System.Data.Services.Providers.ResourceAssociationSet"/> instance when given the source association end.
        /// </summary>
        /// <param name="resourceSet">Resource set of the source association end.</param>
        /// <param name="resourceType">Resource type of the source association end.</param>
        /// <param name="resourceProperty">Resource property of the source association end.</param>
        /// <returns>
        /// A <see cref="T:System.Data.Services.Providers.ResourceAssociationSet"/> instance.
        /// </returns>
        public ResourceAssociationSet GetResourceAssociationSet(ResourceSet resourceSet, ResourceType resourceType, ResourceProperty resourceProperty)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether a resource type has derived types.
        /// </summary>
        /// <param name="resourceType">A <see cref="T:System.Data.Services.Providers.ResourceType"/> object to evaluate.</param>
        /// <returns>
        /// true when <paramref name="resourceType"/> represents an entity that has derived types; otherwise false.
        /// </returns>
        public bool HasDerivedTypes(ResourceType resourceType)
        {
            // We don’t support inheritance yet 
            return false; 
        }

        /// <summary>
        /// Gets all available containers.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1"/> collection of <see cref="T:System.Data.Services.Providers.ResourceSet"/> objects.</returns>
        public IEnumerable<ResourceSet> ResourceSets
        {
            get 
            {
                //  Get the resource sets in the set of composite resource sets.
                return from r in resourceSets.Values select r.ResourceSet; 
            } 
        }

        /// <summary>
        /// Returns all the types in this data source.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1"/> collection of <see cref="T:System.Data.Services.Providers.ResourceType"/> objects.</returns>
        public IEnumerable<ResourceType> Types
        {
            get
            {
                //  Get the resource types in the set of composite resource types.
                return from r in resourceTypes.Values select r.ResourceType;
            } 
        }

        /// <summary>
        /// Returns all the service operations in this data source.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1"/> collection of <see cref="T:System.Data.Services.Providers.ServiceOperation"/> objects.</returns>
        public IEnumerable<ServiceOperation> ServiceOperations
        {
            get
            {
                //  Get the resource types in the set of composite resource types.
                return from r in serviceOperations.Values select r.ServiceOperation;
            } 
        }

        /// <summary>
        /// Tries to get a resource set based on the specified name.
        /// </summary>
        /// <param name="name">Name of the <see cref="T:System.Data.Services.Providers.ResourceSet"/> to resolve.</param>
        /// <param name="resourceSet">Returns the resource set or a null value if a resource set with the given <paramref name="name"/> is not found.</param>
        /// <returns>
        /// true when resource set with the given <paramref name="name"/> is found; otherwise false.
        /// </returns>
        public bool TryResolveResourceSet(string name, out ResourceSet resourceSet)
        {
            //  Null the resource set.
            resourceSet = null;

            //  Get the apporpriate composite resource set.
            CompositeResourceSet compositeResourceSet;
            if (resourceSets.TryGetValue(name, out compositeResourceSet) == false)
                return false;

            //  Set the resource set.
            resourceSet = compositeResourceSet.ResourceSet;

            //  Success.
            return true; 
        }

        /// <summary>
        /// Tries to get a resource type based on the specified name.
        /// </summary>
        /// <param name="name">Name of the type to resolve.</param>
        /// <param name="resourceType">Returns the resource type or a null value if a resource type with the given <paramref name="name"/> is not found.</param>
        /// <returns>
        /// true when resource type with the given <paramref name="name"/> is found; otherwise false.
        /// </returns>
        public bool TryResolveResourceType(string name, out ResourceType resourceType)
        {
            //  Null the resource type.
            resourceType = null;

            //  Get the apporpriate composite resource type.
            CompositeResourceType compositeResourceType;
            if (resourceTypes.TryGetValue(name, out compositeResourceType) == false)
                return false;

            //  Set the resource type.
            resourceType = compositeResourceType.ResourceType;

            //  Success.
            return true; 
        }

        /// <summary>
        /// Tries to get a service operation based on the specified name.
        /// </summary>
        /// <param name="name">Name of the service operation to resolve.</param>
        /// <param name="serviceOperation">Returns the service operation or a null value if a service operation with the given <paramref name="name"/> is not found.</param>
        /// <returns>
        /// true when service operation with the given <paramref name="name"/> is found; otherwise false.
        /// </returns>
        public bool TryResolveServiceOperation(string name, out ServiceOperation serviceOperation)
        {
            //  Null the service operation.
            serviceOperation = null;

            //  Get the apporpriate composite service operation.
            CompositeServiceOperation compositeServiceOperation;
            if (serviceOperations.TryGetValue(name, out compositeServiceOperation) == false)
                return false;

            //  Set the resource type.
            serviceOperation = compositeServiceOperation.ServiceOperation;

            //  Success.
            return true; 
        }


        /// <summary>
        /// Tries to get the composite resource set.
        /// </summary>
        /// <param name="name">The name of the resource set.</param>
        /// <param name="compositeResourceSet">The composite resource set.</param>
        /// <returns>True if the composite resource set was found.</returns>
        public bool TryResolveCompositeResourceSet(string name, out CompositeResourceSet compositeResourceSet)
        {
            return resourceSets.TryGetValue(name, out compositeResourceSet);
        }

        /// <summary>
        /// Tries to get the composite resource type.
        /// </summary>
        /// <param name="name">The name of the resource type.</param>
        /// <param name="compositeResourceSet">The composite resource type.</param>
        /// <returns>True if the composite resource type was found.</returns>
        public bool TryResolveCompositeResourceType(string name, out CompositeResourceType compositeResourceType)
        {
            return resourceTypes.TryGetValue(name, out compositeResourceType);
        }

        /// <summary>
        /// Tries to get the composite service operation.
        /// </summary>
        /// <param name="name">The name of the service operation.</param>
        /// <param name="compositeResourceSet">The composite service operation.</param>
        /// <returns>True if the composite service operation was found.</returns>
        public bool TryResolveCompositeServiceOperation(string name, out CompositeServiceOperation compositeServiceOperation)
        {
            return serviceOperations.TryGetValue(name, out compositeServiceOperation);
        }

        /// <summary>
        /// Adds the type of the composite resource.
        /// </summary>
        /// <param name="type">The type.</param>
        public void AddCompositeResourceType(CompositeResourceType type)
        {
            type.ResourceType.SetReadOnly();
            resourceTypes.Add(type.Name, type);
        }

        /// <summary>
        /// Adds the resource set.
        /// </summary>
        /// <param name="set">The set.</param>
        public void AddCompositeResourceSet(CompositeResourceSet set)
        {
            set.ResourceSet.SetReadOnly();
            resourceSets.Add(set.Name, set);
        }

        /// <summary>
        /// Adds the service operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        public void AddCompositeServiceOperation(CompositeServiceOperation operation)
        {
            operation.ServiceOperation.SetReadOnly();
            serviceOperations.Add(operation.Name, operation);
        }

        /// <summary>
        /// The composite resource types.
        /// </summary>
        private Dictionary<string, CompositeResourceType> resourceTypes =
            new Dictionary<string, CompositeResourceType>();

        /// <summary>
        /// The composite resource sets.
        /// </summary>
        private Dictionary<string, CompositeResourceSet> resourceSets =
            new Dictionary<string, CompositeResourceSet>();

        /// <summary>
        /// The composite service operations.
        /// </summary>
        private Dictionary<string, CompositeServiceOperation> serviceOperations =
            new Dictionary<string, CompositeServiceOperation>();
    }
}
