using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace Recrutement.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nom du travail")]
        public  String JobTitle { get; set; }
        [Required]
        [Display(Name = "Description du travail")]
        public String JobContent { get; set; }

        [Display(Name = "Image ")]
        public String JobImage { get; set; }

        [Display(Name = "Type du travail ")]
        public int CategoryId { get; set; }
        [Display(Name = "Type du travail ")]
        public virtual Categorie Category { get; set; }

        public string  UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<ApplyForJob> ApplyJobs { get; set; }

    }
}