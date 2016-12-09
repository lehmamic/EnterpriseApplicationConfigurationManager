using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Zuehlke.Eacm.Web.Backend.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Zuehlke.Eacm.Web.Backend.EndToEndTests.FixtureSupport
{
    public class WebHostFixture<TStartup> : IDisposable where TStartup : class
    {
        private readonly TestServer server;

        public WebHostFixture()
        {
            var builder = new WebHostBuilder()
                .UseStartup<TStartup>()
                .ConfigureServices(services => services.AddDbContext<EacmDbContext>(opt => opt.UseInMemoryDatabase()));

            this.server = new TestServer(builder);

            this.Client = this.server.CreateClient();
            this.Client.BaseAddress = new Uri("http://localhost:5000");
        }

        public HttpClient Client { get; }

        public void Dispose()
        {
            this.Client.Dispose();
            this.server.Dispose();
        }
    }
}