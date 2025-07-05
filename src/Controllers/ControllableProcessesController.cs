using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Dvin.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Attributes;
using Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Tash.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Controllers;

public class ControllableProcessesController(
        ITashDatabase tashDatabase, ISimpleLogger simpleLogger,
        IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor,
        IDvinRepository dvinRepository)
            : ODataController {
    private bool _HasExceptionFilterAttributeBeenSet;


    [HttpGet, EnableQuery]
    public async Task<IActionResult> GetAsync() {
        await SetExceptionFilterAttributeIfNecessaryAsync();

        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController) + nameof(GetAsync)))) {
            var methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack("Returning all controllable processes", methodNamesFromStack);
            return Ok(tashDatabase.ControllableProcesses);
        }
    }

    [HttpGet, EnableQuery]
    public async Task<IActionResult> GetAsync(int key) {
        await SetExceptionFilterAttributeIfNecessaryAsync();

        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController) + nameof(GetAsync)))) {
            var methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack($"Get controllable process with id={key}", methodNamesFromStack);
            var controllableProcessFromDb = tashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb == null) {
                LogInformationWithCallStack($"No controllable process found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            LogInformationWithCallStack($"Returning controllable process with id={key}", methodNamesFromStack);
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
    public async Task<IActionResult> PutAsync(int key, [FromBody] ControllableProcess process) {
        await SetExceptionFilterAttributeIfNecessaryAsync();

        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController) + nameof(PutAsync)))) {
            var methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack($"Put controllable process with id={key}", methodNamesFromStack);
            if (!ModelState.IsValid) {
                LogInformationWithCallStack($"Model is not valid, cannot put controllable process with id={key}", methodNamesFromStack);
                return BadRequest(ModelState);
            }

            var controllableProcessesFromDb = tashDatabase.ControllableProcesses;
            var controllableProcessFromDb = controllableProcessesFromDb.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb != null) {
                LogInformationWithCallStack($"Controllable process deleted for id={key}", methodNamesFromStack);
                controllableProcessesFromDb.Remove(controllableProcessFromDb);
            }

            process.ProcessId = key;
            controllableProcessesFromDb.Add(process);
            LogInformationWithCallStack($"Controllable process inserted for id={key}", methodNamesFromStack);
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
    public async Task<IActionResult> PatchAsync(int key, Delta<ControllableProcess> patch) {
        await SetExceptionFilterAttributeIfNecessaryAsync();

        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController) + nameof(PatchAsync)))) {
            var methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack($"Patch controllable process with id={key}", methodNamesFromStack);
            if (!ModelState.IsValid) {
                LogInformationWithCallStack($"Model is not valid, cannot patch controllable process with id={key}", methodNamesFromStack);
                return BadRequest(ModelState);
            }

            var controllableProcessFromDb = tashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb == null) {
                LogInformationWithCallStack($"No controllable process found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            patch.Patch(controllableProcessFromDb);

            LogInformationWithCallStack($"Controllable process updated for id={key}", methodNamesFromStack);
            return Updated(controllableProcessFromDb);
        }
    }

    /// <summary>
    /// Used by SaveChangesAsync Insert action
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] ControllableProcess process) {
        await SetExceptionFilterAttributeIfNecessaryAsync();

        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(ControllableProcessesController) + nameof(PostAsync)))) {
            var methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack($"Post controllable process with id={process.ProcessId}", methodNamesFromStack);
            var controllableProcessFromDb = tashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == process.ProcessId);
            if (controllableProcessFromDb != null) {
                LogInformationWithCallStack($"A controllable process with id={process.ProcessId} already exists", methodNamesFromStack);
                return BadRequest();
            }

            tashDatabase.ControllableProcesses.Add(process);
            LogInformationWithCallStack($"Controllable process inserted for id={process.ProcessId}", methodNamesFromStack);
            return Created(process);
        }
    }

    /// <summary>
    /// Used by SaveChangesAsync Delete action
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<IActionResult> DeleteControllableProcessAsync(int key) {
        await SetExceptionFilterAttributeIfNecessaryAsync();

        using (simpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(DeleteControllableProcessAsync)))) {
            var methodNamesFromStack = methodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            LogInformationWithCallStack($"Delete controllable process with id={key}", methodNamesFromStack);
            var controllableProcessFromDb = tashDatabase.ControllableProcesses.FirstOrDefault(p => p.ProcessId == key);
            if (controllableProcessFromDb == null) {
                LogInformationWithCallStack($"No controllable process found with id={key}", methodNamesFromStack);
                return NotFound();
            }

            tashDatabase.ControllableProcesses.Remove(controllableProcessFromDb);
            LogInformationWithCallStack($"Controllable process deleted for id={key}", methodNamesFromStack);
            return StatusCode((int) HttpStatusCode.NoContent);
        }
    }

    private async Task SetExceptionFilterAttributeIfNecessaryAsync() {
        if (_HasExceptionFilterAttributeBeenSet) { return; }

        var errorsAndInfos = new ErrorsAndInfos();
        var dvinApp = await dvinRepository.LoadAsync(Constants.TashAppId, errorsAndInfos);
        if (errorsAndInfos.AnyErrors()) {
            throw new Exception("Tash app not registered");
        }
        DvinExceptionFilterAttribute.SetExceptionLogFolder(new Folder(dvinApp.ExceptionLogFolder));
        _HasExceptionFilterAttributeBeenSet = true;
    }

    private void LogInformationWithCallStack(string message, IList<string> methodNamesFromStack) {
        try {
            simpleLogger.LogInformationWithCallStack(message, methodNamesFromStack);
            // ReSharper disable once EmptyGeneralCatchClause
        } catch {
        }
    }
}