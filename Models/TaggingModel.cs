using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageMakerWebProject.Models
{
    public class TaggingModel
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public int TagId { get; set; }
    }
}