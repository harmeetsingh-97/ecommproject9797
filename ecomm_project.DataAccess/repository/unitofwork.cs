using ecomm_project.DataAccess.Data;
using ecomm_project.DataAccess.repository;
using ecomm_project.DataAccess.repository.Irepository;
using ecomm_project.DataAccess.Repository.Irepository;
using ecomm_project.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.DataAccess.Repository
{
    public class Unitofwork : iunitofwork
    {
        private readonly ApplicationDbContext _context;
        public Unitofwork(ApplicationDbContext context)
        {
            _context = context;
            category = new CategoryRepository(context);
            CoverType = new covertypeRepository(context);
            product = new ProductRepository(context);
            company=new CompanyRepository(context);
            applicationUser=new ApplicationUserRepository(context);
            shoppingCart=new ShoppingCartRepository(context);
            orderHeader=new OrderHeaderRepository(context);
            orderDetails=new OrderDetailsRepository(context);
        }


        public IcategoryRepository category { private set; get; }

        public IcovertypeRepository CoverType {  private set; get; }
        public IproductRepository product { private set; get; } 
        public ICompanyRepository company { private set; get; }
        public IApplicationUserRepository applicationUser { private set; get; }
        public IShoppingCartRepository shoppingCart { private set; get; }
        public IOrderHeaderRepository orderHeader { private set; get; }
        public IOrderDetailsRepository orderDetails { private set; get; }
     

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

   


