using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Repository : IRepository
    {

        private readonly AppDbContext dbContext;

        public Repository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public IQueryable<T> Get<T>() where T : class
        {
            return dbContext.Set<T>();
        }


        public T GetbyId<T>(Guid id) where T : class
        {
            return dbContext.Set<T>().Find(id);
        }


        public int AddAndSave<T>(T model) where T : class
        {
            dbContext.Set<T>().Add(model);
            return dbContext.SaveChanges();
        }


        public int UpdateAndSave<T>(T model) where T : class
        {
            dbContext.Set<T>().Update(model);
            return dbContext.SaveChanges();
        }

        public int DeleteAndSave<T>(T model) where T : class
        {
            dbContext.Set<T>().Remove(model);
            return dbContext.SaveChanges();
        }

        public IQueryable<T> FindBy<T>(Expression<Func<T, bool>> predicate) where T : class
        {
          return dbContext.Set<T>().Where(predicate);
        }

    }


    public static class EFExtensions
    {
        public static IQueryable<T> IncludeNav<T, TProperty>(this IQueryable<T> query, Expression<Func<T, TProperty>> expression) where T : class
        {
            return query.Include(expression);
        }


        public static IQueryable<T> IncludeNav<T>(this IQueryable<T> query, params string[] navProperties) where T : class
        {
            foreach (var navProperty in navProperties)
                query = query.Include(navProperty);
            return query;
        }
    }
}
