using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJB.LuisModels
{
    public class LuisJson
    {
        public string Query { get; set; }

        public List<LuisIntent> Intents { get; set; }

        public List<LuisEntity> Entities { get; set; }
    }
}