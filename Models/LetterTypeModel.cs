using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageMakerWebProject.Models
{
    public class LetterTypeModel
    {
        public int Id { get; set; }

        [Display(Name = "Letter Type")]
        [MinLength(1, ErrorMessage = "Letter Type must be a least one character.")]
        [MaxLength(30, ErrorMessage = "Letter Type must be at most 30 characters.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description must be at most 500 characters.")]
        public string Description { get; set; }
    }
}