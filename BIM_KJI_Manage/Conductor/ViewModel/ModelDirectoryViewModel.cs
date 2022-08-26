﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Propotype_Manage.Conductor.Model;
using Propotype_Manage.Conductor.Controller;

namespace Propotype_Manage.Conductor.ViewModel
{
    public class ModelDirectoryViewModel : TreeViewItemViewModel
    {
        readonly ModelDirectory _directory;
        public Database Database { get; set; }
        public ModelDirectoryViewModel(ModelDirectory directory, Database database)
            : base(null, true)
        {
            _directory = directory;
            Database = database;
        }

        public string ModelName
        {
            get { return _directory.ModelName; }
        }

        protected override void LoadChildren()
        {
            foreach (FieldPrototype fieldPrototyfe in Database.GetFieldPrototypes(_directory))
                base.Children.Add(new FieldPrototypeViewModel(fieldPrototyfe, this,Database));
        }
    }
}
