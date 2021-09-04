using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using HighlyAvailableMonolithPOC.Folders.Commands;
using Microsoft.AspNetCore.Http;
using System;

namespace HighlyAvailableMonolithPOC.Controllers
{
    [ApiController]
    [Route("/folder")]
    public class FoldersController : ControllerBase
    {
        private readonly ISender sender;

        public FoldersController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder([FromBody] CreateFolderCommand command)
        {
            var id = await sender.Send(command);
            return Ok(id);
        }

        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        [HttpPost("/folder/{id}/upload")]
        public async Task<IActionResult> UploadFile([FromRoute] Guid id, [FromForm] IFormFile file)
        {
            var command = new UploadFileCommand()
            {
                FolderId = id,
                FileName = file.FileName,
                Content = file.OpenReadStream()
            };

            await sender.Send(command);
            return NoContent();
        }
    }
}
