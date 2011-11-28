using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apex.MVVM;
using CompositeDataServiceFramework.Server;

namespace CompDataSvcUtil
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            goCommand = new AsynchronousCommand(DoGoCommand);
        }
        
        private NotifyingProperty ServiceUriProperty =
          new NotifyingProperty("ServiceUri", typeof(string), 
              //default(string));
              @"http://localhost:65110/CompositeDataServiceSample.svc");

        public string ServiceUri
        {
            get { return (string)GetValue(ServiceUriProperty); }
            set { SetValue(ServiceUriProperty, value); }
        }
        
        private NotifyingProperty DataServiceProperty =
          new NotifyingProperty("DataService", typeof(DataServiceViewModel), default(DataServiceViewModel));

        public DataServiceViewModel DataService
        {
            get { return (DataServiceViewModel)GetValue(DataServiceProperty); }
            set { SetValue(DataServiceProperty, value); }
        }
                

        private void DoGoCommand()
        {
            //  Create a metadata loader.
            WcfDataServiceMetadataLoader metadataLoader = new WcfDataServiceMetadataLoader();
        
            //  Attempt to load the metadata.
            if (metadataLoader.LoadMetadata(new Uri(ServiceUri)) == false)
                throw new Exception("Failed to load Service Metadata.");

            //  There must be at least one entity container.
            if (metadataLoader.EntityContainers.Count() < 1)
                throw new Exception("This service defines no entity containers.");

            //  Create the data service view model.
            DataServiceViewModel dataService = new DataServiceViewModel();

            //  Create the entity container.
            EntityContainerViewModel entityContainer = new EntityContainerViewModel() { Name = metadataLoader.EntityContainers.First().Name };

            //  Add each entity type.
            foreach (var entityType in metadataLoader.EntityTypes)
            {
                entityContainer.EntityTypes.Add(
                    new EntityTypeViewModel()
                    {
                        Name = entityType.Name 
                    });
            }

            //  Add each entity set.
            foreach (var entitySet in metadataLoader.EntitySets)
            {
                entityContainer.EntitySets.Add(
                    new EntitySetViewModel()
                    {
                        Name = entitySet.Name
                    });
            }

            //  Add each association type.
            foreach (var associationType in metadataLoader.AssociationTypes)
            {
                entityContainer.AssociationTypes.Add(
                    new AssociationTypeViewModel()
                    {
                        Name = associationType.Name
                    });
            }

            //  Add each service operation type.
            foreach (var serviceOperation in metadataLoader.FunctionImports)
            {
                entityContainer.ServiceOperations.Add(
                    new ServiceOperationViewModel()
                    {
                        Name = serviceOperation.Name
                    });
            }

            //  Add the data service.
            dataService.EntityContainers.Add(entityContainer);
            DataService = dataService;
        }

        private AsynchronousCommand goCommand;

        public AsynchronousCommand GoCommand
        {
            get { return goCommand; }
        }
    }
}
