using Aspenlaub.Net.GitHub.CSharp.Dvin.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.DataAccess;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable UnusedMethodReturnValue.Global

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Components;

public static class TashContainerBuilder {
    // ReSharper disable once UnusedMember.Global
    public static ContainerBuilder RegisterForTashDvinAndPegh(this ContainerBuilder builder, string applicationName, ICsArgumentPrompter csArgumentPrompter) {
        builder.UseDvinAndPegh(applicationName, csArgumentPrompter);
        builder.RegisterInstance<ITashDatabase>(new TashDatabase());
        return builder;
    }

    public static IServiceCollection UseTashDvinAndPegh(this IServiceCollection services, string applicationName, ICsArgumentPrompter csArgumentPrompter) {
        services.UseDvinAndPegh(applicationName, csArgumentPrompter);
        services.AddSingleton<ITashDatabase>(new TashDatabase());
        return services;
    }
}