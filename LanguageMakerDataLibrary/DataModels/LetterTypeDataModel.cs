using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageMakerDataLibrary.DataModels
{
    public class LetterTypeDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LanguageId { get; set; }
    }
}
