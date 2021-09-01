using Aspenlaub.Net.GitHub.CSharp.Dvin.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.DataAccess;
using Aspenlaub.Net.GitHub.CSharp.Tash.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
// ReSharper disable UnusedMethodReturnValue.Global

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Components {
    public static class TashContainerBuilder {
        // ReSharper disable once UnusedMember.Global
        public static ContainerBuilder RegisterForTashDvinAndPegh(this ContainerBuilder builder, ICsArgumentPrompter csArgumentPrompter) {
            builder.UseDvinAndPegh(csArgumentPrompter);
            builder.RegisterInstance<ITashDatabase>(new TashDatabase());
            builder.RegisterInstance<ILogConfiguration>(new LogConfiguration());
            return builder;
        }

        public static IServiceCollection UseTashDvinAndPegh(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter) {
            services.UseDvinAndPegh(csArgumentPrompter);
            services.AddSingleton<ITashDatabase>(new TashDatabase());
            services.AddSingleton<ILogConfiguration>(new LogConfiguration());
            return services;
        }
    }
}
