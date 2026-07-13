using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ecomm_project.models
{
    public class covertype
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
    }
}
