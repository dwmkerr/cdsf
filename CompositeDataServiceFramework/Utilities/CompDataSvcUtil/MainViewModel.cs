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
              @"http://localhost:65110/CompositeDataServiceSample.svc/$metadata");

        public string ServiceUri
        {
            get { return (string)GetValue(ServiceUriProperty); }
            set { SetValue(ServiceUriProperty, value); }
        }

        private void DoGoCommand()
        {
            //  Create a metadata loader.
            WcfDataServiceMetadataLoader metadataLoader = new WcfDataServiceMetadataLoader();
        
            //  Attempt to load the metadata.
            if (metadataLoader.LoadMetadata(new Uri(ServiceUri)) == false)
                throw new Exception("Failed to load Service Metadata.");
        }

        private AsynchronousCommand goCommand;

        public AsynchronousCommand GoCommand
        {
            get { return goCommand; }
        }
    }
}
