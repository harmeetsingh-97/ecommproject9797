using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.models.ViewModels
{
    public class ProductVM
    {
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> CoverTypeList { get;set; }
        public product Product { get; set; }
    }
}
