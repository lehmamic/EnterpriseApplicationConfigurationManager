using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Zuehlke.Eacm.Web.Backend.DataAccess;
using Zuehlke.Eacm.Web.Backend.ReadModel;

namespace Zuehlke.Eacm.Web.Backend.Tests.FixtureSupport
{
    public class DatabaseFixture : IDisposable
    {
        private bool disposed;

        public DatabaseFixture()
        {
            var builder = new DbContextOptionsBuilder<EacmDbContext>();
            builder.UseInMemoryDatabase();

            this.DbContext = new EacmDbContext(builder.Options);

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ReadModelProfile>();
            });

            this.Mapper = new Mapper(config);
        }

        ~DatabaseFixture()
        {
            this.Dispose(false);
        }

        public EacmDbContext DbContext { get; }

        public IMapper Mapper { get; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.DbContext.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}