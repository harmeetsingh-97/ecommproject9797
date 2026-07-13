using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ecomm_project.models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Display(Name ="Street Address")]
        public string streetaddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        [Display(Name ="Postal Code")]
        public string postalcode { get; set; }
        [Display(Name ="Company")]
        public int? companyid { get; set; }
        [ForeignKey("companyid")]
        public Company company { get; set; }
        [NotMapped]
        public string role { get; set; }
    }
}
