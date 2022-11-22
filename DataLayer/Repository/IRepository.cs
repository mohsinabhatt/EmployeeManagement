using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IRepository
    {
        IQueryable<T> GetAll<T>() where T : class;


        T GetById<T>(Guid id) where T : class;


        int AddAndSave<T>(T model) where T : class;


        int UpdateAndSave<T>(T model) where T : class;


        int DeleteAndSave<T>(T model) where T : class;


        IQueryable<T> FindBy<T>(System.Linq.Expressions.Expression<Func<T,bool>> predicate) where T : class;    

    }
}
