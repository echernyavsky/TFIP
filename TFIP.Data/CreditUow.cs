﻿using System;
using System.Data.Entity;
using TFIP.Business.Entities;
using TFIP.Data.Contracts;

namespace TFIP.Data
{
    public class CreditUow : ICreditUow
    {
        private readonly DbContext dbContext;

        /// <summary>
        /// Creates eticket unity of work with the provided repository provider
        /// </summary>
        /// <param name="context">Db context </param>
        /// <param name="repositoryProvider">Repository provider</param>
        public CreditUow(CreditDbContext context, IRepositoryProvider repositoryProvider)
        {
            dbContext = context;
            ConfigureDbContext();
            RepositoryProvider = repositoryProvider;
        }

        protected IRepositoryProvider RepositoryProvider { get; set; }

        #region IDisposable Members
        

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


        #region ICreditUow Members

        public IBaseRepository<Attachment> Attachments
        {
            get
            {
                return GetBaseRepo<Attachment>();
            }
        }

        public IBaseRepository<AttachmentHeader> AttachmentHeaders
        {
            get
            {
                return GetBaseRepo<AttachmentHeader>();
            }
        }

        public IBaseRepository<CreditRequest> CreditRequests
        {
            get
            {
                return GetBaseRepo<CreditRequest>();
            }
        }

        public IBaseRepository<CreditType> CreditTypes
        {
            get
            {
                return GetBaseRepo<CreditType>();
            }
        }

        public IBaseRepository<Guarantor> Guarantors
        {
            get
            {
                return GetBaseRepo<Guarantor>();
            }
        }

        public IBaseRepository<IndividualClient> IndividualClients
        {
            get
            {
                return GetBaseRepo<IndividualClient>();
            }
        }

        public IBaseRepository<JuridicalClient> JuridicalClients
        {
            get
            {
                return GetBaseRepo<JuridicalClient>();
            }
        }

        public IBaseRepository<Notification> Notifications
        {
            get
            {
                return GetBaseRepo<Notification>();
            }
        }

        public IBaseRepository<Payment> Payments
        {
            get
            {
                return GetBaseRepo<Payment>();
            }
        }

        /// <summary>
        /// Commits info from DbContext to database
        /// </summary>
        public void Commit()
        {
            dbContext.SaveChanges();
        }
        #endregion

        /// <summary>
        /// Dispose if the appropriate parameter value
        /// </summary>
        /// <param name="disposing">If disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dbContext != null)
                {
                    dbContext.Dispose();
                }
            }
        }

        private IBaseRepository<T> GetBaseRepo<T>() where T : class, IEntity
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }

        private T GetRepo<T>() where T : class
        {
            var repository = RepositoryProvider.GetRepository<T>();
            return repository;
        }

        /// <summary>
        /// Creates dbcontext for eTicket database
        /// </summary>
        protected void ConfigureDbContext()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
        }
    }
}