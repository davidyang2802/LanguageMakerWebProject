using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageMakerDataLibrary.DataModels
{
    public class WordPatternDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pattern { get; set; }
        public int LanguageId { get; set; }
    }
}
