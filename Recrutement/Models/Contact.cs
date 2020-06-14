using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recrutement.Models
{
    public class Contact
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Objet { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Email { get; set; }
    }
}