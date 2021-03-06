﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apex.MVVM;
using System.Collections.ObjectModel;

namespace CompDataSvcUtil
{
    public class EntityContainerViewModel : ViewModel
    {
        private NotifyingProperty NameProperty =
          new NotifyingProperty("Name", typeof(string), default(string));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        private ObservableCollection<EntitySetViewModel> entitySets = new ObservableCollection<EntitySetViewModel>();

        private ObservableCollection<EntityTypeViewModel> entityTypes = new ObservableCollection<EntityTypeViewModel>();

        private ObservableCollection<AssociationTypeViewModel> associationTypes = new ObservableCollection<AssociationTypeViewModel>();

        private ObservableCollection<ServiceOperationViewModel> serviceOperations = new ObservableCollection<ServiceOperationViewModel>();

        public ObservableCollection<EntitySetViewModel> EntitySets
        {
            get { return entitySets; }
        }

        public ObservableCollection<EntityTypeViewModel> EntityTypes
        {
            get { return entityTypes; }
        }

        public ObservableCollection<AssociationTypeViewModel> AssociationTypes
        {
            get { return associationTypes; }
        }

        public ObservableCollection<ServiceOperationViewModel> ServiceOperations
        {
            get { return serviceOperations; }
        }

        public IEnumerable<DataServiceItemGroup> Children
        {
            get
            {
                DataServiceItemGroup group = new DataServiceItemGroup()
                {
                    Name = "Entity Sets",
                    Children = entitySets.ToArray()
                };
                yield return group; 
                group = new DataServiceItemGroup()
                {
                    Name = "Entity Types",
                    Children = entityTypes.ToArray()
                };
                yield return group; 
                group = new DataServiceItemGroup()
                {
                    Name = "Association Types",
                    Children = associationTypes.ToArray()
                };
                group = new DataServiceItemGroup()
                {
                    Name = "Service Operations",
                    Children = serviceOperations.ToArray()
                };
                yield return group;
            }
        }
    }
}
