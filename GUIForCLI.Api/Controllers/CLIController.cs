using GUIForCLI.Api.Entities;
using GUIForCLI.Api.Entities.Helpers;
using GUIForCLI.Services;
using GUIForCLI.Parsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GUIForCLI.Api.Controllers
{
    [Route("api/disk")]
    [ApiController]
    public class CLIController : ControllerBase
    {
        public readonly ICmdService _cmdService;
        public readonly IGitService _gitService;
        public readonly IDiskManagementService _diskManagementService;
        public CLIController(ICmdService cmdService, IGitService gitService,IDiskManagementService diskManagementService)
        {
            _cmdService = cmdService;
            _gitService = gitService;
            _diskManagementService = diskManagementService;
        }

        [HttpPost("customtask",Name = "CustomTask")]
        public async Task<IActionResult> CustomCli(CLIRequest request)
        {
            CLIResponse<List<DriveInfoModel>?> response = new(StatusCodes.Status400BadRequest, "Error", null, null);
            string serviceRespone = string.Empty;

            bool isSuccess = false;
            

            try
            {
                (isSuccess, serviceRespone) = await _cmdService.CustomTask(request);
                response.Data = DriveInfoParser.ExtractDriveInfo(serviceRespone);
                response.Status = StatusCodes.Status200OK;
                response.Message = serviceRespone;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }

            return StatusCode(response.Status, response);
        }

        [HttpGet("disks",Name ="GetDisks")]
        public async Task<IActionResult> GetDisks()
        {
            CLIResponse<List<DriveInfoModel>?> response = new(StatusCodes.Status400BadRequest, "Error", null, null);
           
            bool isSuccess = false;


            try
            {
                response.Data = await _diskManagementService.GetDrives();
                response.Status = StatusCodes.Status200OK;
                response.Message = "Device data retrieved";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }

            return StatusCode(response.Status, response);
        }
    }
}
