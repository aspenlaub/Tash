using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Tash.Attributes;
using Aspenlaub.Net.GitHub.CSharp.Tash.Components;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Tash {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddOData();

            services.AddControllersWithViews(mvc => mvc.EnableEndpointRouting = false);

            services.AddMvc(config => config.Filters.Add(new DvinExceptionFilterAttribute()));

            services.UseTashDvinAndPegh(new DummyCsArgumentPrompter());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
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
            builder.EntitySet<ControllableProcessTask>("ControllableProcessTasks");

            return builder.GetEdmModel();
        }
    }
}
