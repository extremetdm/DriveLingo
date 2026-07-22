using System;
using System.Collections.Generic;

namespace DriveLingo.Models
{
    public class Material
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string ReadTime { get; set; }
        public string ImageUrl { get; set; }
        public string PdfUrl { get; set; }
        public string Content { get; set; }
        public List<string> Readers { get; set; }

        public Material()
        {
            Readers = new List<string>();
        }
    }
}
