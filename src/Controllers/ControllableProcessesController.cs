using System.Linq;
using System.Net;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Controllers;

public class ControllableProcessesController : ODataController {
    private readonly ITashDatabase TashDatabase;
    private readonly ISimpleLogger SimpleLogger;
    private readonly IMethodNamesFromStackFramesExtractor MethodNamesFromStackFramesExtractor;

    public ControllableProcessesController(ITashDatabase tashDatabase, ISimpleLogger simpleLogger, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor) {
        TashDatabase = tashDatabase;
        SimpleLogger = simpleLogger;
        MethodNamesFromStackFramesExtractor = methodNamesFromStackFramesExtractor;
    }

    [HttpGet, EnableQuery]
    public IActionResult Get() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController) + nameof(Get), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack("Returning all controllable processes", methodNamesFromStack);
            return Ok(TashDatabase.ControllableProcesses);
        }
    }

    [HttpGet, EnableQuery]
    public IActionResult Get(int key) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController) + nameof(Get), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack($"Get controllable process with id={key}", methodNamesFromStack);
            var controllableProcessFromDb = TashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb == null) {
                SimpleLogger.LogInformationWithCallStack($"No controllable process found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            SimpleLogger.LogInformationWithCallStack($"Returning controllable process with id={key}", methodNamesFromStack);
            return Ok(controllableProcessFromDb);
        }
    }

    /// <summary>
    /// Used by SaveChangesAsync Update (!) action with SaveChangesOptions.ReplaceOnUpdate
    /// </summary>
    /// <param name="key"></param>
    /// <param name="process"></param>
    /// <returns></returns>
    [HttpPut]
    public IActionResult Put(int key, [FromBody] ControllableProcess process) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController) + nameof(Put), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack($"Put controllable process with id={key}", methodNamesFromStack);
            if (!ModelState.IsValid) {
                SimpleLogger.LogInformationWithCallStack($"Model is not valid, cannot put controllable process with id={key}", methodNamesFromStack);
                return BadRequest(ModelState);
            }

            var controllableProcessesFromDb = TashDatabase.ControllableProcesses;
            var controllableProcessFromDb = controllableProcessesFromDb.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb != null) {
                SimpleLogger.LogInformationWithCallStack($"Controllable process deleted for id={key}", methodNamesFromStack);
                controllableProcessesFromDb.Remove(controllableProcessFromDb);
            }

            process.ProcessId = key;
            controllableProcessesFromDb.Add(process);
            SimpleLogger.LogInformationWithCallStack($"Controllable process inserted for id={key}", methodNamesFromStack);
            return Updated(process);
        }
    }

    /// <summary>
    /// Used by SaveChangesAsync Update action without SaveChangesOptions.ReplaceOnUpdate
    /// </summary>
    /// <param name="key"></param>
    /// <param name="patch"></param>
    /// <returns></returns>
    [HttpPatch]
    public IActionResult Patch(int key, Delta<ControllableProcess> patch) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController) + nameof(Patch), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack($"Patch controllable process with id={key}", methodNamesFromStack);
            if (!ModelState.IsValid) {
                SimpleLogger.LogInformationWithCallStack($"Model is not valid, cannot patch controllable process with id={key}", methodNamesFromStack);
                return BadRequest(ModelState);
            }

            var controllableProcessFromDb = TashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb == null) {
                SimpleLogger.LogInformationWithCallStack($"No controllable process found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            patch.Patch(controllableProcessFromDb);

            SimpleLogger.LogInformationWithCallStack($"Controllable process updated for id={key}", methodNamesFromStack);
            return Updated(controllableProcessFromDb);
        }
    }

    /// <summary>
    /// Used by SaveChangesAsync Insert action
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Post([FromBody] ControllableProcess process) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController) + nameof(Post), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack($"Post controllable process with id={process.ProcessId}", methodNamesFromStack);
            var controllableProcessFromDb = TashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == process.ProcessId);
            if (controllableProcessFromDb != null) {
                SimpleLogger.LogInformationWithCallStack($"A controllable process with id={process.ProcessId} already exists", methodNamesFromStack);
                return BadRequest();
            }

            TashDatabase.ControllableProcesses.Add(process);
            SimpleLogger.LogInformationWithCallStack($"Controllable process inserted for id={process.ProcessId}", methodNamesFromStack);
            return Created(process);
        }
    }

    /// <summary>
    /// Used by SaveChangesAsync Delete action
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpDelete]
    public IActionResult DeleteControllableProcess(int key) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(DeleteControllableProcess), SimpleLogger.LogId))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            SimpleLogger.LogInformationWithCallStack($"Delete controllable process with id={key}", methodNamesFromStack);
            var controllableProcessFromDb = TashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb == null) {
                SimpleLogger.LogInformationWithCallStack($"No controllable process found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            TashDatabase.ControllableProcesses.Remove(controllableProcessFromDb);
            SimpleLogger.LogInformationWithCallStack($"Controllable process deleted for id={key}", methodNamesFromStack);
            return StatusCode((int) HttpStatusCode.NoContent);
        }
    }
}