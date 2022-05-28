using System.Collections.Generic;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Test.Controllers;

public class ODataResponse<T> {
    public List<T> Value { get; set; }
}