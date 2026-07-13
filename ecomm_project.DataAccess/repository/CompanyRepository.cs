using ecomm_project.DataAccess.Data;
using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_project.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.DataAccess.Repository
{
    
    public class CompanyRepository:Repositery<Company>,ICompanyRepository
    {
        private readonly ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context):base(context)
        {
            _context= context;
        }
    }
}
