using Imagekit.Sdk;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using VJN.ModelsDTO.MediaItemDTOs;
using VJN.Services;

namespace VJN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ImagekitClient _imagekitClient;
        private readonly IMediaItemService _mediaItemService;
        private readonly IImagePostJobService _imagepostJobService;

        public UploadController(IMediaItemService mediaItemService, IImagePostJobService imagepostJobService)
        {
            _imagekitClient = new ImagekitClient("public_Q+yi7A0O9A+joyXIoqM4TpVqOrQ=", "private_e2V3fNLKwK0pGwSrEmFH+iKQtks=", "https://ik.imagekit.io/ryf3sqxfn");
            _mediaItemService = mediaItemService;
            _imagepostJobService = imagepostJobService;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult<int>> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not provided or file is empty.");

            using (var memoryStream = new System.IO.MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                try
                {
                    FileCreateRequest uploadRequest = new FileCreateRequest
                    {
                        file = fileBytes,
                        fileName = file.FileName
                    };
                    Result result = _imagekitClient.Upload(uploadRequest);
                    var media = new MediaItemDTO
                    {
                        Url = result.url,
                        Status = true
                    };
                    var mediaID = await _mediaItemService.CreateMediaItem(media);
                    return Ok(new { mediaid = mediaID });
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, $"Upload failed: {ex.Message}");
                }
            }
        }

        [HttpPost]
        [Route("UploadMultipleFilesForJob")]
        public async Task<ActionResult<int>> UploadImage([FromForm] List<IFormFile> files, int postid)
        {
            if (files == null || !files.Any())
                return BadRequest("No files provided.");

            var mediaIds = new List<int>();

            var uploadTasks = files.Select(async file =>
            {
                if (file.Length == 0)
                    throw new ArgumentException("One or more files are empty.");

                using (var memoryStream = new System.IO.MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();

                    FileCreateRequest uploadRequest = new FileCreateRequest
                    {
                        file = fileBytes,
                        fileName = file.FileName
                    };

                    Result result = _imagekitClient.Upload(uploadRequest);
                    var media = new MediaItemDTO
                    {
                        Url = result.url,
                        Status = true
                    };
                    return await _mediaItemService.CreateMediaItem(media);
                }
            });

            try
            {
                mediaIds = (await Task.WhenAll(uploadTasks)).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Upload failed: {ex.Message}");
            }

            var result = await _imagepostJobService.createImagePostJob(postid, mediaIds);
            return Ok(result);
        }

    }
}
