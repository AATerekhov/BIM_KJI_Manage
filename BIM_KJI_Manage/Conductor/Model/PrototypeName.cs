﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propotype_Manage.Conductor.Model
{
    public class PrototypeName
    {
        public PrototypeName(string prefix)
        {
            Prefix = prefix;
        }

        public string Prefix { get; private set; }


    }
}
