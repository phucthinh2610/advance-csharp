using advance_csharp.dto.Request.AppVersion;
using advance_csharp.dto.Response.AppVersion;
using advance_csharp.service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace advance_csharp.Controllers
{
    /// <summary>
    /// Api Controller Application
    /// </summary>
    [Route("api/application")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly ILoggingService _loggingService;

        public ApplicationController(IApplicationService applicationService, ILoggingService loggingService)
        {
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        }

        /// <summary>
        /// get-version
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("get-version")]
        [HttpGet()]
        [MyAppAuthentication("User")]
        public async Task<IActionResult> GetVersion([FromQuery] AppVersionGetListRequest request)
        {
            try
            {
                AppVersionGetListResponse response = await _applicationService.GetApplicationVersionList(request);
                _loggingService.LogInfo(JsonSerializer.Serialize(response));
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                // send to logging service
                _loggingService.LogError(ex);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// get-version-admin
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("get-version-admin")]
        [HttpGet()]
        [MyAppAuthentication("Admin")]
        public async Task<IActionResult> GetVersionAdmin([FromQuery] AppVersionGetListRequest request)
        {
            try
            {
                AppVersionGetListResponse response = await _applicationService.GetApplicationVersionList(request);
                _loggingService.LogInfo(JsonSerializer.Serialize(response));
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                // send to logging service
                _loggingService.LogError(ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}