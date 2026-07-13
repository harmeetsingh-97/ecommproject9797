using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ecomm_project.models
{
    public class product
    {
        public int id { get; set; }
        [Required]
        public string Title { get; set;}
        [Required]

        public string Description {get; set;}
        [Required]
        public string Author {get; set;}
        [Required]
        public string ISBN { get; set; }
        [Required]
        [Range(1, 1000)]
        public double ListPrice { get; set; }
        [Required]
        [Range (1, 1000)]
        public double Price { get; set; }
        [Required]
        [Range(1, 1000)]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 1000)]
        public double Price100 { get; set; }
        [Display(Name = "Image url")]
        public string ImageUrl { get; set; }
        [Required]
        [Display(Name="Category")]
        public int categoryid { get; set; }
        public category category { get; set; }
        [Required]
        [Display (Name ="CoverType")]

        public int covertypeid { get; set; }
        public covertype covertype { get; set; }
    }
}   
