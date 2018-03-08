using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJB.LuisModels
{
    public class LuisEntity
    {
        public string Entity { get; set; }

        public string Type { get; set; }

        public string StartIndex { get; set; }

        public string EndIndex { get; set; }

        public float Score { get; set; }
    }
}