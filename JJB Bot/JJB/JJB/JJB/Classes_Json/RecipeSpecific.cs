using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJB.Classes_Json
{
    [Serializable]
    public class RecipeSpecific
    {
        public Recipe_ recipe { get; set; }
    }
    [Serializable]
    public class Recipe_
    {
        public string publisher { get; set; }
        public string f2f_url { get; set; }
        public string[] ingredients { get; set; }
        public string source_url { get; set; }
        public string recipe_id { get; set; }
        public string image_url { get; set; }
        public float social_rank { get; set; }
        public string publisher_url { get; set; }
        public string title { get; set; }
    }
}