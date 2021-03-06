﻿using Infrastructure.BasicTypes;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using System;
using System.Net;


namespace Infrastructure.Generics
{
    public class ServiceBase<TEntity> where TEntity : IIdentity
    {
        private readonly IUnitOfWork _unitOfWork;
        public IRepository<TEntity> Repository { get; set; }       

        public ServiceBase(IRepository<TEntity> repository, IUnitOfWork unitOfWork)
        {
            Repository = repository;
            _unitOfWork = unitOfWork;
        }

        protected void Transaction(Action metodo)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                metodo();
                _unitOfWork.Commit();
            }
            catch
            {           
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
