﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAutomation.DropBoxAPI.ResponseModels
{
    public class Root
    {
        public List<Entry> entries { get; set; }
        public string cursor { get; set; }
        public bool has_more { get; set; }
    }
}
