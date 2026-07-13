using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text;

namespace ecomm_project.models
{
    public  class Company
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Display(Name = "Street Address")]
        public string streetaddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        [Display(Name ="Postal Code")]
        public string postalcode{ get; set; }
        [Display(Name ="Phone Number")]
        public string phonenumber { get; set; }
        [Display(Name ="Is Authorized Company")]
        public bool isauthorized { get; set; }
    }
}
