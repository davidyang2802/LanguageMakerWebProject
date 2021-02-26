using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageMakerWebProject.Models
{
    public class LetterModel
    {
        public int Id { get; set; }

        [Display(Name = "Letter")]
        [MinLength(1, ErrorMessage = "Letter must be at least one character.")]
        [MaxLength(15, ErrorMessage = "Letter must be at most 15 characters.")]
        public string Name { get; set; }

        [Display(Name = "Letter Type")]
        [MinLength(1, ErrorMessage = "Letter must be at least one character.")]
        [MaxLength(30, ErrorMessage = "Letter must be at most 30 characters.")]
        public string LetterType { get; set; }

        [MaxLength(20, ErrorMessage = "Pronounciation must be at most 20 characters.")]
        public string Pronounciation { get; set; }

        [MaxLength(500, ErrorMessage = "Description must be at most 500 characters.")]
        public string Description { get; set; }
    }
}