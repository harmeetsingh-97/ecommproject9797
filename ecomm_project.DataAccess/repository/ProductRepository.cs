using ecomm_project.DataAccess.Data;
using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_project.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.DataAccess.Repository
{
    public class ProductRepository:Repositery<product>, IproductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context):base(context) 
        {
            {
             _context = context;
            }
        }
    }
}
