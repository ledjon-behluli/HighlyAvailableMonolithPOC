using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using HighlyAvailableMonolithPOC.Folders.Commands;
using Microsoft.AspNetCore.Http;
using System;
using HighlyAvailableMonolithPOC.Application.Queries;
using HighlyAvailableMonolithPOC.Application.Commands;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFolder([FromRoute] Guid id)
        {
            var folderDto = await sender.Send(new GetFolderQuery()
            {
                FolderId = id
            });

            return Ok(folderDto);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFolder([FromRoute] Guid id)
        {
            await sender.Send(new DeleteFolderCommand()
            {
                FolderId = id
            });

            return NoContent();
        }
    }
}
