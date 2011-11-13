using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public class CompositeDataServiceUpdateProvider : IDataServiceUpdateProvider
    {
        public CompositeDataServiceUpdateProvider( 
           CompositeDataServiceMetadataProvider metadataProvider, 
           CompositeDataServiceQueryProvider queryProvider) 
        {
            this.metadataProvider = metadataProvider;
            this.queryProvider = queryProvider;
        }

        private CompositeDataServiceContext GetContext()
        {
            return queryProvider.CurrentDataSource as CompositeDataServiceContext;
        }

        private CompositeDataServiceMetadataProvider metadataProvider;
        private CompositeDataServiceQueryProvider queryProvider;
        private List<Action> queuedActions = new List<Action>();

        public void SetConcurrencyValues(object resourceCookie, bool? checkForEquality, IEnumerable<KeyValuePair<string, object>> concurrencyValues)
        {
            throw new NotImplementedException();
        }

        public void AddReferenceToCollection(object targetResource, string propertyName, object resourceToBeAdded)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cancels a change to the data.
        /// </summary>
        public void ClearChanges()
        {
            //  Clear the queued actions.
            queuedActions.Clear();
        }

        /// <summary>
        /// Creates the resource of the specified type and that belongs to the specified container.
        /// </summary>
        /// <param name="containerName">The name of the entity set to which the resource belongs.</param>
        /// <param name="fullTypeName">The full namespace-qualified type name of the resource.</param>
        /// <returns>
        /// The object representing a resource of specified type and belonging to the specified container.
        /// </returns>
        public object CreateResource(string containerName, string fullTypeName)
        {
            //  Find the composite resource set.
            CompositeResourceSet compositeResourceSet;
            if (metadataProvider.TryResolveCompositeResourceSet(containerName, out compositeResourceSet) == false)
                throw new Exception("Failed to resolve parent resource set.");

            //  Create the resource.
            object resource = compositeResourceSet.DoCreateResource();

            //  Queue the add operation.
            queuedActions.Add(
                () =>
                {
                    //  Add the resource.
                    compositeResourceSet.DoAddResource(compositeResourceSet.ResourceSet.ResourceType, resource);
                });

            //  Return the resource.
            return resource;
        }

        public void DeleteResource(object targetResource)
        {
            var resourceSet = (from c in metadataProvider.ResourceSets
                              where c.ResourceType.Name == targetResource.GetType().Name
                              select c).FirstOrDefault();
            CompositeResourceSet compositeResourceSet;
            metadataProvider.TryResolveCompositeResourceSet(resourceSet.Name,
                out compositeResourceSet);

            queuedActions.Add(() => compositeResourceSet.DoDeleteResource(targetResource));
        }

        public object GetResource(IQueryable query, string fullTypeName)
        {
            //  Get the resource.
            var enumerator = query.GetEnumerator();
            
            //  Do we have one?
            if (!enumerator.MoveNext())
                throw new Exception("Resource not found");
            
            //  Is it unique?
            var resource = enumerator.Current;
            if (enumerator.MoveNext())
                throw new Exception("Resource not uniquely identified");

            //  If we have a type name, get the resource type.
            if (fullTypeName != null)
            {
                //  TODO: TryResolveResourceType full name, we need just the name.
                ResourceType type = null;
                if (!metadataProvider.TryResolveResourceType(
                   fullTypeName, out type))
                    throw new Exception("ResourceType not found");
                if (!type.InstanceType.IsAssignableFrom(resource.GetType()))
                    throw new Exception("Unexpected resource type");
            }
            return resource; 
        }

        /// <summary>
        /// Gets the value of the specified property on the target object.
        /// </summary>
        /// <param name="targetResource">An opaque object that represents a resource.</param>
        /// <param name="propertyName">The name of the property whose value needs to be retrieved.</param>
        /// <returns>
        /// The value of the object.
        /// </returns>
        public object GetValue(object targetResource, string propertyName)
        {
            //  Get the value.
            var value = targetResource
                      .GetType()
                      .GetProperties()
                      .Single(p => p.Name == propertyName)
                      .GetGetMethod()
                      .Invoke(targetResource, new object[] { });
            
            //  Return the value.
            return value;
        }

        public void RemoveReferenceFromCollection(object targetResource, string propertyName, object resourceToBeRemoved)
        {
            throw new NotImplementedException();
        }

        public object ResetResource(object resource)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the instance of the resource represented by the specified resource object.
        /// </summary>
        /// <param name="resource">The object representing the resource whose instance needs to be retrieved.</param>
        /// <returns>
        /// Returns the instance of the resource represented by the specified resource object.
        /// </returns>
        public object ResolveResource(object resource)
        {
            //  We do not use proxies, therefore we can return the resource.
            return resource; 
        }

        /// <summary>
        /// Saves all the changes that have been made by using the <see cref="T:System.Data.Services.IUpdatable"/> APIs.
        /// </summary>
        public void SaveChanges()
        {
            //  Perform each action.
            foreach (var queuedAction in queuedActions)
                queuedAction();
        }

          public void SetReference(object targetResource, string propertyName, object propertyValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the value of the property with the specified name on the target resource to the specified property value.
        /// </summary>
        /// <param name="targetResource">The target object that defines the property.</param>
        /// <param name="propertyName">The name of the property whose value needs to be updated.</param>
        /// <param name="propertyValue">The property value for update.</param>
        public void SetValue(object targetResource, string propertyName, object propertyValue)
        {
            //  Queue the set value action.
            queuedActions.Add(
               () =>
               {
                   //   Actually set the value.
                   DoSetValue(targetResource, propertyName, propertyValue);
               }
            );
        }

        private void DoSetValue(object targetResource, string propertyName, object propertyValue)
        {
            targetResource
               .GetType()
               .GetProperties()
               .Single(p => p.Name == propertyName)
               .GetSetMethod()
               .Invoke(targetResource, new[] { propertyValue });
        }
    }
}
