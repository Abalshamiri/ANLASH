using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using ANLASH.Storage;
using ANLASH.Controllers;

namespace ANLASH.Web.Host.Controllers
{
    /// <summary>
    /// Blob Storage Controller for file download
    /// </summary>
    [Route("api/services/app/[controller]")]
    public class BlobStorageController : ANLASHControllerBase
    {
        private readonly IBlobStorageAppService _blobStorageService;

        public BlobStorageController(IBlobStorageAppService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        /// <summary>
        /// Download file by ID
        /// </summary>
        [HttpGet("Download")]
        [AllowAnonymous] // Allow unauthenticated access for image display
        public async Task<IActionResult> Download(Guid id)
        {
            try
            {
                var file = await _blobStorageService.DownloadAsync(id);
                return File(file.FileBytes, file.ContentType, file.FileName);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
