using ecomm_project.DataAccess.Data;
using ecomm_project.DataAccess.Repository.Irepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ecomm_project.DataAccess.Repository
{
    public class Repositery<T> : Irepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbset;
        public Repositery(ApplicationDbContext context)
        {
            _context = context;
            dbset = _context.Set<T>();
        }
        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeproperties = null)
        {
            IQueryable<T> query = dbset;
            if(filter != null)
                query = query.Where(filter);
            if(!string.IsNullOrEmpty(includeproperties))
            {
                foreach(var includeprop in includeproperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop.Trim());
                }
            }
            return query.FirstOrDefault();
        }

        public T get(int id)
        {
            return dbset.Find(id);
        }

        public IEnumerable<T> GetAllOrders(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includeproperties = null)
        {
            IQueryable<T> query = dbset;//change in project copy
            if (filter != null)//also this
                query = query.Where(filter);//also this
                if (includeproperties != null)//also this
                {
                    foreach (var includeprop in includeproperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                    query = query.Include(includeprop);
                    } 
                }
            
            if (orderby != null)
                return orderby(query).ToList();
            return query.ToList();
        }

        public void Remove(int id)
        {
            dbset.Remove(get(id));
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void Removerange(IEnumerable<T> entities)
        {
            dbset.RemoveRange(entities);
        }

        public void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public void Update(T entity)
        {
            _context.ChangeTracker.Clear();
           dbset.Update(entity);
        }
    }
}
