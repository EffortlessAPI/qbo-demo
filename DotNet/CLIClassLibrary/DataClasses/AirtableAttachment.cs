using System;
using System.Collections.Generic;
using System.Linq;

namespace EAPI.CLI.Lib.DataClasses
{

    public class AirtableAttachment
    {
        public string id { get; set; }
        public long? size { get; set; }
        public string url { get; set; }
        public string type { get; set; }
        public string filename { get; set; }
        public Thumbnails thumbnails { get; set; }
    }
    
    public class Thumbnails
    {
        public Thumbnail small { get; set; }
        public Thumbnail large { get;set; }        
    }
    
    public class Thumbnail
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}