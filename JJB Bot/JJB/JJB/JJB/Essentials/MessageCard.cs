using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJB.Essentials
{
    public class MessageCard
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Text { get; set; }

        public string Image { get; set; }

        public List<MessageButton> Buttons { get; set; }
    }
}