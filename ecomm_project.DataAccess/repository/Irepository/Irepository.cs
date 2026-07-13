using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static System.Net.WebRequestMethods;

namespace ecomm_project.DataAccess.Repository.Irepository
{
    public  interface Irepository<T> where T:class
    {
        void Add(T entity); 
        void Update(T entity);
        T get(int id);
        IEnumerable<T> GetAllOrders(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,string includeproperties = null);//category & covertpye
        T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeproperties = null);
        void Remove(int id);
        void Remove(T entity);
        void Removerange(IEnumerable<T> entities);
    }
}
