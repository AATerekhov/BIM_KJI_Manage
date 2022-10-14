﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PrototypeConductor.Model;
using BIMPropotype_Lib.ViewModel;
using PrototypeObserver;
using BIMPropotype_Lib.Controller;

namespace PrototypeConductor.Controller
{
    public class Database//TODO: переделать на объектную
    {
        public PrefixDirectory PrefixDirectory { get; set; }
        public SelectObserver InSelectObserver { get; set; }

        public Database(PrefixDirectory prefixDirectory, SelectObserver selectObserver)
        {
            InSelectObserver = selectObserver;
            PrefixDirectory = prefixDirectory;
        }

        public List<FieldPrototype> GetFieldPrototypes(ModelDirectory modelDirectory)
        {
            return new List<FieldPrototype>(from item in PrefixDirectory.GetFields(modelDirectory.Path) select new FieldPrototype(new DirectoryInfo(item).Name, item));
        }
        public List<PrototypeName> GetPrototype(FieldPrototype fieldPrototype)
        {
            return new List<PrototypeName>(from item in PrefixDirectory.GetFiles(fieldPrototype.Path) select new PrototypeName(new FileInfo(item).Name));
        }
        public List<ModelDirectory> GetModelDirectories()
        {
            return new List<ModelDirectory>() { new ModelDirectory(PrefixDirectory.GetDataDirectory(), PrefixDirectory.ModelInfo.ModelName) };

        }

        public void LoadingPrototype() 
        {
            var loader = new BeamLoader(PrefixDirectory);
            loader.DeserializeXML();
            InSelectObserver.SelectedPrototype = loader.InBIMAssembly;
        }

    }
}
