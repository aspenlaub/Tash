using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Aspenlaub.Net.GitHub.CSharp.Tash {
    public static class TashModelBuilder {
        public static IEdmModel GetEdmModel() {
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