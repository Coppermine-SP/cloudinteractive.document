﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudinteractive.document
{
    internal interface IExportableDocument
    {
        public string[] Export();
    }
}
