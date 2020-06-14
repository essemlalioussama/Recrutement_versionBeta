using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recrutement.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Type de travail")]
        public string CategoryName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string CategoryDescription { get; set; }


        public virtual ICollection<Job> Jobs { get; set; }

    }
}