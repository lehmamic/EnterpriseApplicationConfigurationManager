using System;
using Microsoft.EntityFrameworkCore;
using Zuehlke.Eacm.Web.Backend.DataAccess;

namespace Zuehlke.Eacm.Web.Backend.Tests.FixtureSupport
{
    public abstract class DatabaseFixture : IDisposable
    {
        private bool disposed;

        protected DatabaseFixture()
        {
            var builder = new DbContextOptionsBuilder<EacmDbContext>();
            builder.UseInMemoryDatabase();

            this.DbContext = new EacmDbContext(builder.Options);
        }

        ~DatabaseFixture()
        {
            this.Dispose(false);
        }

        public EacmDbContext DbContext { get; }

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