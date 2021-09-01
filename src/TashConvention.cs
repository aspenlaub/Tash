﻿using Microsoft.AspNetCore.OData.Routing.Conventions;

namespace Aspenlaub.Net.GitHub.CSharp.Tash {
    public class TashConvention : IODataControllerActionConvention {
        public int Order => -100;

        public bool AppliesToAction(ODataControllerActionContext context) {
            return true; // apply to all controller
        }

        public bool AppliesToController(ODataControllerActionContext context) {
            return false; // continue for all others
        }
    }
}