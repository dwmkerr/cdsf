using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Providers;

namespace CompositeDataServiceFramework.Server
{
    public delegate object CreateResourceDelegate();
    public delegate void AddResourceDelegate(ResourceType resourceType, object resourceValue);
    public delegate void DeleteResourceDelegate(object resourceValue);

    public class CompositeResourceSet
    {
        public string Name
        {
            get;
            set;
        }

        public string ResourceTypeName
        {
            get
            {
                return ResourceSet.ResourceType.Name;
            }

        }

        public CompositeDataSource DataSource
        {
            get;
            set;
        }

        public ResourceSet ResourceSet
        {
            get;
            set;
        }

        public IQueryable QueryRoot
        {
            get;
            set;
        }

        public object DoCreateResource()
        {
            var action = CreateResourceAction;
            if (action != null)
                return action();
            return null;
        }

        public void DoAddResource(ResourceType resourceType, object resourceValue)
        {
            var action = AddResourceAction;
            if (action != null)
                action(resourceType, resourceValue);
        }

        public void DoDeleteResource(object resourceValue)
        {
            var action = DeleteResourceAction;
            if (action != null)
                action(resourceValue);
        }

        public event CreateResourceDelegate CreateResourceAction;

        public event AddResourceDelegate AddResourceAction;

        public event DeleteResourceDelegate DeleteResourceAction;
    }
}
