using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public abstract class CompositeDataSource
    {
        public abstract void Initialise();

        private Dictionary<string, ResourceType> resourceTypes =
            new Dictionary<string, ResourceType>();

        private Dictionary<string, ResourceSet> resourceSets =
            new Dictionary<string, ResourceSet>();

        private Dictionary<string, ServiceOperation> serviceOperations =
            new Dictionary<string, ServiceOperation>();

        public string Name
        {
            get;
            set;
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
    }
}
