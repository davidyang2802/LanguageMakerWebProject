using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageMakerWebProject.Models
{
    public class TagModel
    {
        public int Id { get; set; }

        [Display(Name = "Tag")]
        [MinLength(1, ErrorMessage = "Tag must be at least 1 character.")]
        [MaxLength(30, ErrorMessage = "Tag must be at most 30 characters.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description must be at most 500 characters.")]
        public string Description { get; set; }
    }
}