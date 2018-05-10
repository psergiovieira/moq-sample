using Infrastructure.BasicTypes;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Generics
{
    public class ServiceBase<TEntity> where TEntity : IIdentity
    {
        private IUnitOfWork _unitOfWork;
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
            catch (WebException wEx)
            {
                _unitOfWork.Rollback();
                throw wEx;
            }
            catch (ApplicationException adoex)
            {
                _unitOfWork.Rollback();
                throw adoex;

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }
    }
}
