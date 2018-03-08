using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJB.Essentials
{
    public class Item
    {
        public string id { get; set; }

        public string Answer { get; set; }

        public string Intent { get; set; }

        public string Entity { get; set; }

        public MessageAttachment Attachment { get; set; }

        public List<MessageButton> Buttons { get; set; }

        public List<MessageCard> Cards { get; set; }

    }
}