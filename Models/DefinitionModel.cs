using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageMakerWebProject.Models
{
    public class DefinitionModel
    {
        public int Id { get; set; }

        [Display(Name = "Definition")]
        [MinLength(1, ErrorMessage = "Definition must be at least 1 characters.")]
        [MaxLength(500, ErrorMessage = "Definition must be at most 100 characters.")]
        public string Text { get; set; }

        public int WordId { get; set; }
    }
}