using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageMakerDataLibrary.DataModels
{
    public class WordDataModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int LanguageId { get; set; }
        public int ClassificationId { get; set; }
        public string Description { get; set; }
        public string Pronounciation { get; set; }
    }
}
