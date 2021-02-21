using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LanguageMakerWebProject.Models
{
    public class WordPatternModel
    {
        public int Id { get; set; }

        [MinLength(1, ErrorMessage = "Word Pattern name must be at least one character.")]
        [MaxLength(30, ErrorMessage = "Word Pattern name must be at most 30 characters.")]
        public string Name { get; set; }

        [MinLength(1, ErrorMessage = "Word Pattern must be at least one character.")]
        [MaxLength(500, ErrorMessage = "Word Pattern must be at most 500 characters.")]
        public string Pattern { get; set; }
    }
}