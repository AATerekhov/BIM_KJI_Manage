﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeConductor.Model
{
    /// <summary>
    /// Папка в архиве
    /// </summary>
    public class FieldPrototype
    {
        public string Name { get; private set; }
        public string Path { get; set; }

        public FieldPrototype(string name, string path)
        {
            Name = name;
            Path = path;
        }

        readonly List<PrototypeName> _prefix = new List<PrototypeName>();
        /// <summary>
        /// Список файлов
        /// </summary>
        public List<PrototypeName> Prefix
        {
            get { return _prefix; }
        }
    }
}
