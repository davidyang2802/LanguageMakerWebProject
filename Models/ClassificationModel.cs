using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageMakerWebProject.Models
{
    public class ClassificationModel
    {
        public int Id { get; set; }

        [MinLength(1, ErrorMessage = "Name must be at least one character.")]
        [MaxLength(30, ErrorMessage = "Name must be at most 20 characters.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description must be at most 20 characters.")]
        public string Description { get; set; }
    }
}