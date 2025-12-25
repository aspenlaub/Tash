using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Tash.Attributes;
using Aspenlaub.Net.GitHub.CSharp.Tash.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;

// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Tash;

public class Startup(IConfiguration configuration) {
    public IConfiguration Configuration { get; } = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
        IEdmModel model = TashModelBuilder.GetEdmModel();
        services.AddControllers()
            .AddOData(opt => opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(null)
                .AddRouteComponents("", model)
                .Conventions.Add(new TashConvention())
            );

        services.AddRazorPages(); // for MapRazorPages()

        services.UseTashDvinAndPegh("Tash", new DummyCsArgumentPrompter());

        services.AddControllers(opt => opt.Filters.Add<DvinExceptionFilterAttribute>());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();

        app.UseRouting(); // for UseEndpoints()

        app.UseODataRouteDebug(""); // for display of end points at http://localhost:60404
        app.UseODataQueryRequest();

        app.Use(next => context => {
            Endpoint _ = context.GetEndpoint();

            return next(context);
        });

        app.UseEndpoints(ConfigureEndpoints);
    }

    private static void ConfigureEndpoints(IEndpointRouteBuilder endpoints) {
        endpoints.MapControllers();
        endpoints.MapDefaultControllerRoute(); // for http://localhost:60404/
        endpoints.MapRazorPages(); // for e.g. return View("Index");
    }
}