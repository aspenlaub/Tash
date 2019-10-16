using Aspenlaub.Net.GitHub.CSharp.DvinStandard.Components;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.DataAccess;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Components {
    public static class TashContainerBuilder {
        // ReSharper disable once UnusedMember.Global
        public static ContainerBuilder RegisterForDvin(this ContainerBuilder builder) {
            builder.RegisterInstance<ITashDatabase>(new TashDatabase());
            return builder;
        }

        public static IServiceCollection UseTashDvinAndPegh(this IServiceCollection services, ICsArgumentPrompter csArgumentPrompter) {
            services.UseDvinAndPegh(csArgumentPrompter);
            services.AddSingleton<ITashDatabase>(new TashDatabase());
            return services;
        }
    }
}
