﻿using HotelListing.DTOModels;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace HotelListing.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            //List<string> includes = null
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null //replacing above statement to avoid small changes with DB table names for example from breaking DBContext, more strongly typed
            );

        Task<IPagedList<T>> GetAll(
            RequestParams requestParams,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task<T> Get(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        Task Insert(T entity);

        Task InsertRange(IEnumerable<T> entities);

        Task Delete(int id);

        void DeletRenge(IEnumerable<T> entities);

        void Update(T entity);
    }
}
