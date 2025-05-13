using ChildrenLeisure.DAL.Data;
using ChildrenLeisure.DAL.Entities;
using ChildrenLeisure.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildrenLeisure.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        T Add(T entity);
        T Update(T entity);
        bool Delete(int id);
    }
}
