using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static ODLMWebAPI.StaticStuff.Constants;
namespace ODLMWebAPI.Models
{
    public class TblAttributesTO
    {
        public Int32 idAttribute { get; set; }
        public String attributeDisplayName { get; set; }
        public String attributeName { get; set; }
        public Int32 pageId { get; set; }
        public Int32 idAttributesStatus { get; set; }
    }
}
