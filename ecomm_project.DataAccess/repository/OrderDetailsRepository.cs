using ecomm_project.DataAccess.Data;
using ecomm_project.DataAccess.repository.Irepository;
using ecomm_project.DataAccess.Repository;
using ecomm_project.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.DataAccess.repository
{
    public class OrderDetailsRepository:Repositery<OrderDetails>,IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailsRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
    }
}
