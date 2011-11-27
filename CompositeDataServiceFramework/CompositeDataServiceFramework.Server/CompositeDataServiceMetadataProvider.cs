using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;
using System.Reflection;
using System.ServiceModel.Web;
using System.Data.Services;
using System.Data.Metadata.Edm;

namespace CompositeDataServiceFramework.Server
{
    /// <summary>
    /// The Composite Data Service Metadata Provider provides metadata for
    /// a composite data service.
    /// </summary>
    public class CompositeDataServiceMetadataProvider : IDataServiceMetadataProvider
    {
        /// <summary>
        /// Gets or sets the name of the base.
        /// </summary>
        /// <value>
        /// The name of the base.
        /// </value>
        public string BaseName
        {
            get;
            set;
        }

        /// <summary>
        /// Container name for the data source.
        /// </summary>
        /// <returns>String that contains the name of the container.</returns>
        public string ContainerName
        {
            get 
            {
                //  Get the name of the class, we'll use that as the base for the container name.
                return BaseName + "Container";
            }
        }

        /// <summary>
        /// Namespace name for the data source.
        /// </summary>
        /// <returns>String that contains the namespace name.</returns>
        public string ContainerNamespace
        {
            get
            {
                //  Get the name of the class, we'll use that as the base for the namespace.
                return BaseName + "Namespace";
            }
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
            //  TODO: We don't support type inheritance yet - this should be added.
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
            //  We store the resource association set in the custom property.
            return resourceProperty.CustomState as ResourceAssociationSet;
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
            //  Try and get the resource set.
            return resourceSets.TryGetValue(name, out compositeResourceSet);
        }

        /// <summary>
        /// Tries the type of the resolve composite resource set for resource type.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="compositeResourceSet">The composite resource set.</param>
        /// <returns></returns>
        public bool TryResolveCompositeResourceSetForResourceType(string name, out CompositeResourceSet compositeResourceSet)
        {
            //  We haven't found the resource set.
            compositeResourceSet = null;

            //  Go through each resource set.
            foreach (var resourceSet in resourceSets.Values)
            {
                if (resourceSet.ResourceTypeName == name)
                {
                    //  We've found the resource set.
                    compositeResourceSet = resourceSet;
                    return true;
                }
            }

            //  Failed to find the resource set.
            return false;
        }

        /// <summary>
        /// Tries to get the composite resource association set.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="compositeResourceAssociationSet">The composite resource association set.</param>
        /// <returns></returns>
        public bool TryResolveCompositeResourceAssociationSet(string name, out CompositeResourceAssociationSet compositeResourceAssociationSet)
        {
            //  Try and get the resource association set.
            return resourceAssociationSets.TryGetValue(name, out compositeResourceAssociationSet);
        }

        /// <summary>
        /// Tries to get the composite resource type.
        /// </summary>
        /// <param name="name">The name of the resource type.</param>
        /// <param name="compositeResourceSet">The composite resource type.</param>
        /// <returns>True if the composite resource type was found.</returns>
        public bool TryResolveCompositeResourceType(string name, out CompositeResourceType compositeResourceType)
        {
            //  Try and get the resource type.
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
            //  Try and get the service operation.
            return serviceOperations.TryGetValue(name, out compositeServiceOperation);
        }

        /// <summary>
        /// Adds the type of the composite resource.
        /// </summary>
        /// <param name="type">The type.</param>
        public void AddCompositeResourceType(CompositeResourceType type)
        {
            //  Add the composite resource type.
            resourceTypes.Add(type.Name, type);
        }

        /// <summary>
        /// Adds the resource set.
        /// </summary>
        /// <param name="set">The set.</param>
        public void AddCompositeResourceSet(CompositeResourceSet set)
        {
            //  Add the composite resource set.
            resourceSets.Add(set.Name, set);
        }

        /// <summary>
        /// Adds the resource association set.
        /// </summary>
        /// <param name="set">The set.</param>
        public void AddCompositeResourceAssociationSet(CompositeResourceAssociationSet set)
        {
            //  Add the resource association set.
            resourceAssociationSets.Add(set.Name, set);
        }

        /// <summary>
        /// Adds the service operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        public void AddCompositeServiceOperation(CompositeServiceOperation operation)
        {
            //  Add the service operation.
            serviceOperations.Add(operation.Name, operation);
        }

        /// <summary>
        /// Freezes this instance, making sure that all resource sets and types are 
        /// set to read only.
        /// </summary>
        public void Freeze()
        {
            //  Freeze each resource type.
            foreach(var rt in resourceTypes.Values.Where((rt) => { return rt.ResourceType.IsReadOnly == false; }))
                rt.ResourceType.SetReadOnly();

            //  Freeze each resource set.
            foreach (var rs in resourceSets.Values.Where((rs) => { return rs.ResourceSet.IsReadOnly == false; }))
                rs.ResourceSet.SetReadOnly();

            //  Freeze each service operation.
            foreach (var so in serviceOperations.Values.Where((so) => { return so.ServiceOperation.IsReadOnly == false; }))
                so.ServiceOperation.SetReadOnly();
        }

        public IEnumerable<CompositeServiceOperation> ExposeServiceOperationsFromObject(object o)
        {
            //  Get the object type.
            Type objectType = o.GetType();

            //  Get each methodinfo.
            MethodInfo[] methodInfos = objectType.GetMethods();

            //  Go through each method.
            foreach (var methodInfo in methodInfos)
            {
                //  TODO*** We should also be able to handle WebInvoke(Method=POST) functions.
                //  Does this method have the WebGet property?
                if (Attribute.IsDefined(methodInfo, typeof(WebGetAttribute)))
                {                    
                    //  Create a set of service operation parameters.
                    List<ServiceOperationParameter> serivceOperationParameters = new List<ServiceOperationParameter>();

                    //  Add each parameter.
                    foreach (var parameterInfo in methodInfo.GetParameters())
                        serivceOperationParameters.Add(new ServiceOperationParameter(parameterInfo.Name, MapClrTypeToResourceType(parameterInfo.ParameterType)));

                    //  Unless we determine otherwise, the result kind will be void
                    //  and the result set will be null.
                    ServiceOperationResultKind resultKind = ServiceOperationResultKind.Void;
                    ResourceType resultType = null;
                    ResourceSet resultSet = null;

                    //  We can only set return type information if the return
                    //  type is not set (i.e. it's void).
                    if (methodInfo.ReturnType != typeof(void))
                    {
                        //  Get the return parameter type.
                        var clrReturnType = methodInfo.ReturnType;

                        //  Are we returning a collection?
                        if (clrReturnType.IsSubclassOfRawGeneric(typeof(IQueryable<>)) ||
                            clrReturnType.IsSubclassOfRawGeneric(typeof(IEnumerable<>)))
                        {
                            //  Set the result type.
                            resultKind = ServiceOperationResultKind.QueryWithMultipleResults;

                            //  Set the result set.
                            CompositeResourceSet compositeResourceSet;
                            if (TryResolveCompositeResourceSetForResourceType(clrReturnType.GetGenericArguments()[0].Name,
                                out compositeResourceSet) == false)
                                throw new DataServiceException("Unable to build metadata for the Service Operation '" + methodInfo.Name + "'.");
                            resultSet = compositeResourceSet.ResourceSet;

                            //  Set the resource type.
                            resultType = compositeResourceSet.ResourceSet.ResourceType;
                        }
                        //  Is it a resource type?
                        else if (resourceTypes.ContainsKey(clrReturnType.Name))
                        {
                            //  Set the result kind.
                            resultKind = ServiceOperationResultKind.QueryWithSingleResult;

                            //  Set the result type.
                            resultType = resourceTypes[clrReturnType.Name].ResourceType;

                            //  Set the result set.
                            CompositeResourceSet compositeResourceSet;
                            if (TryResolveCompositeResourceSetForResourceType(resultType.Name, out compositeResourceSet) == false)
                                throw new DataServiceException("Unable to build metadata for the Service Operation '" + methodInfo.Name + "'.");
                            resultSet = compositeResourceSet.ResourceSet;
                        }
                        //  Otherwise try and use a direct return value.
                        else
                        {
                            //  Set the result kind.
                            resultKind = ServiceOperationResultKind.DirectValue;

                            //  Try and get the primitive resource type.
                            try
                            {
                                resultType = MapClrTypeToResourceType(clrReturnType);
                            }
                            catch
                            {
                                throw new DataServiceException("Service Operation '" + methodInfo.Name + "' cannot return type '" + clrReturnType.Name + "'.");
                            }
                        }
                    }

                    //  TODO: Create the method. At the moment we force get
                    //  but we want to support POST as well.
                    string method = "GET";
                    
                    //  Create the Service Operation.
                    var serviceOperation = new ServiceOperation(methodInfo.Name,
                        resultKind, resultType, resultSet, method, serivceOperationParameters);

                    //  Create the Composite Service Operation.
                    var compositeDataServiceOperation = new CompositeServiceOperation()
                    {
                        Name = serviceOperation.Name,
                        ServiceOperation = serviceOperation
                    };

                    //  Build the invoke action.
                    var methodInfoToInvoke = methodInfo;
                    compositeDataServiceOperation.InvokeServiceOperationAction +=
                        (paramerters)
                            =>
                        {
                            return methodInfoToInvoke.Invoke(o, paramerters);
                        };

                    //  Return the composite service operaiton.
                    yield return compositeDataServiceOperation;
                }
            }
        }


        /// <summary>
        /// This function maps an EdmType to a ResourceType - a fairly common operation.
        /// </summary>
        /// <param name="edmType">The EdmType.</param>
        /// <returns>The ResourceType compatible with the EdmType.</returns>
        public static ResourceType MapEdmTypeToResourceType(EdmType edmType)
        {
            //  We cannot handle anything apart from primitive types.
            if (edmType is PrimitiveType == false)
                throw new DataServiceException("Cannot map EDM Type '" + edmType.Name + "'");
            
            //  Return the mapped ResourceType from the Primitive EdmType CLR Equivalent Type.
            return ResourceType.GetPrimitiveResourceType(((PrimitiveType)edmType).ClrEquivalentType);
        }

        /// <summary>
        /// This function maps a CLR type to a ResourceType if possible.
        /// </summary>
        /// <param name="clrType">The CLR Type.</param>
        /// <returns>The ResourceType.</returns>
        public static ResourceType MapClrTypeToResourceType(Type clrType)
        {
            //  Try and get the resource type.
            ResourceType resourceType = ResourceType.GetPrimitiveResourceType(clrType);
            
            //  Check for bad mappings.
            if(resourceType == null)
                throw new DataServiceException("Cannot map CLR Type '" + clrType.Name + "'");
                        
            //  Return the resource type.
            return resourceType;
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
        /// The composite resource association sets.
        /// </summary>
        private Dictionary<string, CompositeResourceAssociationSet> resourceAssociationSets =
            new Dictionary<string, CompositeResourceAssociationSet>();

        /// <summary>
        /// The composite service operations.
        /// </summary>
        private Dictionary<string, CompositeServiceOperation> serviceOperations =
            new Dictionary<string, CompositeServiceOperation>();
    }
}
