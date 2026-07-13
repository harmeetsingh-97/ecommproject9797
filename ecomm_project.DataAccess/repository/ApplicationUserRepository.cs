using ecomm_project.DataAccess.Data;
using ecomm_project.DataAccess.repository.Irepository;
using ecomm_project.DataAccess.Repository;
using ecomm_project.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.DataAccess.repository
{
    public class ApplicationUserRepository : Repositery<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context=context;
        }

    }
}
