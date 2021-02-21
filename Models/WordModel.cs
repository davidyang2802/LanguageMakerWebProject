using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageMakerWebProject.Models
{
    public class WordModel
    {
        public int Id { get; set; }

        [Display(Name = "Word")]
        [MinLength(1, ErrorMessage = "Word must be at least one character.")]
        [MaxLength(50, ErrorMessage = "Word must be at most 50 characters.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Classification is required")]
        public int ClassificationId { get; set; }

        [MaxLength(500, ErrorMessage = "Description must be at most 500 characters.")]
        public string Description { get; set; }

        public string Pronounciation { get; set; }
    }
}