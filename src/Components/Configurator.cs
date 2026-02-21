using Aspenlaub.Net.GitHub.CSharp.Tash.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;

// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Components;

public static class Configurator {
    public static void ConfigureServices(IServiceCollection services) {
        IEdmModel model = TashModelBuilder.GetEdmModel();
        services.AddControllers()
            .AddOData(opt => opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(null)
                .AddRouteComponents("", model)
                .Conventions.Add(new TashConvention())
            );

        services.UseTashDvinAndPegh(Constants.TashApplicationName);

        services.AddControllers(opt => opt.Filters.Add<DvinExceptionFilterAttribute>());
    }

    public static void Configure(IApplicationBuilder app) {
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
    }
}