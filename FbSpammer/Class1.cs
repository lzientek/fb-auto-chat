﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FbSpammer
{
    public class LocalizedStrings
    {
        private static Resources _localizedResources = new Resources();

        public Resources LocalizedResources { get { return _localizedResources; } }
    }
}
