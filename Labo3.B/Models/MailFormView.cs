using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoviesDBManager.Models
{
    public class MailFormView
    {
        [Required]
        [Display(Name = "Nom du destinataire")]
        public string ToName { get; set; }

        [Required]
        [Display(Name = "Courriel du destinataire")]
        public string ToEmail { get; set; }

        [Required]
        [Display(Name = "Sujet")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Body { get; set; }
    }
}