using ecomm_project.DataAccess.repository.Irepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecomm_project.DataAccess.Repository.Irepository
{
    public  interface iunitofwork
    {
        IcategoryRepository category { get; }
        IcovertypeRepository CoverType { get; }
        IproductRepository product { get; }
        ICompanyRepository company { get; }
        IApplicationUserRepository applicationUser { get; }
        IShoppingCartRepository shoppingCart { get; }
        IOrderDetailsRepository orderDetails { get; }
        IOrderHeaderRepository orderHeader { get; }
       
        void Save();
    }
}
