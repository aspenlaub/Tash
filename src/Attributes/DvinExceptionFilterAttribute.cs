using Aspenlaub.Net.GitHub.CSharp.Dvin.Components;
using Aspenlaub.Net.GitHub.CSharp.Dvin.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
// ReSharper disable ClassNeverInstantiated.Global

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Attributes {
    public class DvinExceptionFilterAttribute : ExceptionFilterAttribute {
        private static IFolder ExceptionLogFolder;

        // ReSharper disable once UnusedMember.Global
        public static void SetExceptionLogFolder(IFolder exceptionLogFolder) {
            ExceptionLogFolder = exceptionLogFolder;
        }

        public override void OnException(ExceptionContext context) {
            ExceptionSaver.SaveUnhandledException(ExceptionLogFolder, context.Exception, nameof(Dvin), _ => { });
            context.Result = new JsonResult(InternalServerError.Create("An exception was logged. We are sorry for the inconvenience."));
        }
    }
}
