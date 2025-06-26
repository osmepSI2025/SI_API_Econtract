using Microsoft.AspNetCore.Mvc;

namespace SME_API_Econtract.Controllers
{
    [ApiController]
    [Route("api/SYS-E-CONTRACT")]
    public class EcontractController : ControllerBase
    {
        private readonly MProjectContractService _projectContractService;
        public EcontractController(MProjectContractService projectContractService)
        {
            _projectContractService = projectContractService;
        }
        [HttpGet("Project/{projectcode}")]
        public async Task<IActionResult> GetProjectByCode(string projectcode)
        {
            try
            {
                searchProjectData searchProjectData = new searchProjectData
                {
                    ProjectCode = projectcode
                };
                var result = await _projectContractService.GetAllAsyncSearch_MProjectContract(searchProjectData);
                if (result == null)
                {
                    return NotFound(new { message = "Project not found" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
        [HttpGet("Project-Batch")]
        public async Task<IActionResult> GetProjectByCodeBatch()
        {
            searchProjectData model = new searchProjectData();
                 await _projectContractService.BatchEndOfDay_MProjectContract("0");
             
                return Ok();
          
           
        }
    }
}
