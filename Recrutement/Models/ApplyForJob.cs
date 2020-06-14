using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace Recrutement.Models
{
    public class ApplyForJob
    {
        public int Id { get; set; }
        public string Message { get; set;}
        public DateTime Date { get; set; }
        public string CV { get; set; }
        public int JobId { get; set; }
        public string UserId { get; set; }


        public virtual Job job { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}