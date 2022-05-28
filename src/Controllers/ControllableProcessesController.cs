using System.Linq;
using System.Net;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Controllers;

public class ControllableProcessesController : ODataController {
    private readonly ITashDatabase TashDatabase;
    private readonly ISimpleLogger SimpleLogger;
    private readonly string LogId;

    public ControllableProcessesController(ITashDatabase tashDatabase, ISimpleLogger simpleLogger, ILogConfigurationFactory logConfigurationFactory) {
        TashDatabase = tashDatabase;
        SimpleLogger = simpleLogger;
        var logConfiguration = logConfigurationFactory.Create();
        SimpleLogger.LogSubFolder = logConfiguration.LogSubFolder;
        LogId = logConfiguration.LogId;
    }

    [HttpGet, EnableQuery]
    public IActionResult Get() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), LogId))) {
            SimpleLogger.LogInformation("Returning all controllable processes");
            return Ok(TashDatabase.ControllableProcesses);
        }
    }

    [HttpGet, EnableQuery]
    public IActionResult Get(int key) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), LogId))) {
            SimpleLogger.LogInformation($"Get controllable process with id={key}");
            var controllableProcessFromDb = TashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb == null) {
                SimpleLogger.LogInformation($"No controllable process found with id={key}");
                return NotFound();
            }

            SimpleLogger.LogInformation($"Returning controllable process with id={key}");
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
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), LogId))) {
            SimpleLogger.LogInformation($"Put controllable process with id={key}");
            if (!ModelState.IsValid) {
                SimpleLogger.LogInformation($"Model is not valid, cannot put controllable process with id={key}");
                return BadRequest(ModelState);
            }

            var controllableProcessesFromDb = TashDatabase.ControllableProcesses;
            var controllableProcessFromDb = controllableProcessesFromDb.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb != null) {
                SimpleLogger.LogInformation($"Controllable process deleted for id={key}");
                controllableProcessesFromDb.Remove(controllableProcessFromDb);
            }

            process.ProcessId = key;
            controllableProcessesFromDb.Add(process);
            SimpleLogger.LogInformation($"Controllable process inserted for id={key}");
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
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), LogId))) {
            SimpleLogger.LogInformation($"Patch controllable process with id={key}");
            if (!ModelState.IsValid) {
                SimpleLogger.LogInformation($"Model is not valid, cannot patch controllable process with id={key}");
                return BadRequest(ModelState);
            }

            var controllableProcessFromDb = TashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb == null) {
                SimpleLogger.LogInformation($"No controllable process found with id={key}");
                return NotFound();
            }

            patch.Patch(controllableProcessFromDb);

            SimpleLogger.LogInformation($"Controllable process updated for id={key}");
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
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), LogId))) {
            SimpleLogger.LogInformation($"Post controllable process with id={process.ProcessId}");
            var controllableProcessFromDb = TashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == process.ProcessId);
            if (controllableProcessFromDb != null) {
                SimpleLogger.LogInformation($"A controllable process with id={process.ProcessId} already exists");
                return BadRequest();
            }

            TashDatabase.ControllableProcesses.Add(process);
            SimpleLogger.LogInformation($"Controllable process inserted for id={process.ProcessId}");
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
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController), LogId))) {
            SimpleLogger.LogInformation($"Delete controllable process with id={key}");
            var controllableProcessFromDb = TashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb == null) {
                SimpleLogger.LogInformation($"No controllable process found with id={key}");
                return NotFound();
            }

            TashDatabase.ControllableProcesses.Remove(controllableProcessFromDb);
            SimpleLogger.LogInformation($"Controllable process deleted for id={key}");
            return StatusCode((int) HttpStatusCode.NoContent);
        }
    }
}