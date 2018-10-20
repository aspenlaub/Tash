using Aspenlaub.Net.GitHub.CSharp.Dvin.Attributes;
using Aspenlaub.Net.GitHub.CSharp.Tash.DataAccess;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Tash {
    public class Startup {
        private readonly ITashDatabase vTashDatabase = new TashDatabase();

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddOData();
            services.AddMvc(config => config.Filters.Add(new DvinExceptionFilterAttribute()));

            services.AddSingleton(vTashDatabase);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseMvc(routebuilder => {
                routebuilder.Select().Expand().Filter().OrderBy(QueryOptionSetting.Allowed).MaxTop(null).Count();
                routebuilder.MapODataServiceRoute("ODataRoute", "", GetEdmModel());
            });
        }

        private static IEdmModel GetEdmModel() {
            var builder = new ODataConventionModelBuilder {
                Namespace = "Aspenlaub.Net.GitHub.CSharp.Tash",
                ContainerName = "DefaultContainer"
            };
            builder.EntitySet<ControllableProcess>("ControllableProcesses");

            return builder.GetEdmModel();
        }
    }
}
