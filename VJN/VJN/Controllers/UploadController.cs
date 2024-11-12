using Google.Apis.Util;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using VJN.Models;
using VJN.ModelsDTO.ImagePostJobDTO;
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
        private readonly IRegisterEmployerMediaService _registerEmployerMediaService;
        private readonly IReportMediaServices _reportMediaService;

        public UploadController(IMediaItemService mediaItemService, IImagePostJobService imagepostJobService, IRegisterEmployerMediaService registerEmployerMediaService, IReportMediaServices reportMediaService)
        {
            _imagekitClient = new ImagekitClient("public_Q+yi7A0O9A+joyXIoqM4TpVqOrQ=", "private_e2V3fNLKwK0pGwSrEmFH+iKQtks=", "https://ik.imagekit.io/ryf3sqxfn");
            _mediaItemService = mediaItemService;
            _imagepostJobService = imagepostJobService;
            _registerEmployerMediaService = registerEmployerMediaService;
            _reportMediaService = reportMediaService;
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
                Console.WriteLine("day la file name: " + file.FileName);
                try
                {
                    FileCreateRequest uploadRequest = new FileCreateRequest
                    {
                        file = fileBytes,
                        fileName = file.FileName,
                        overwriteFile = true
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

        [HttpPost("UploadMultipleFilesForRegisterEmployer")]
        public async Task<ActionResult> UploadMultipleFilesForRegisterEmployer([FromForm] List<IFormFile> files, int registerID)
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
            var result = await _registerEmployerMediaService.CreateRegisterEmployerMedia(registerID, mediaIds);
            return Ok(result);
        }

        [HttpPost("UploadMultipleFilesForReport")]
        public async Task<ActionResult> UploadMultipleFilesForReport([FromForm] List<IFormFile> files, int reportid)
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
            var result = await _reportMediaService.CreateReportMedia(reportid, mediaIds);
            return Ok(result);
        }

        [HttpPut("UpdateImagePostjob")]
        public async Task<ActionResult<bool>> UpdateImagePostjob([FromForm] ImagePostJobForUpdateDTO imageupdate)
        {
            Console.WriteLine("====================================================================");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imgbefore = await _imagepostJobService.GetImagePostJob(imageupdate.postid.Value);
            Console.WriteLine("sasadsdasdads:   " + imgbefore.Count());
            Console.WriteLine("sang :" + (imageupdate.files != null ? imageupdate.files.Count() : 0));

            var differentNumbers = imgbefore.Except(imageupdate.imageIds);
            if (differentNumbers.Count() != 0 || differentNumbers.Any())
            {
                var c = await _imagepostJobService.DeleteImagePost(differentNumbers.ToList(), imageupdate.postid.Value);
            }

            

            if (imageupdate.files == null || !imageupdate.files.Any())
            {
                return Ok(true);
            }
            else
            {
                var mediaIds = new List<int>();

                var uploadTasks = imageupdate.files.Select(async file =>
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
                Console.WriteLine(mediaIds.Count());
                var result = await _imagepostJobService.createImagePostJob(imageupdate.postid.Value, mediaIds);
                return Ok(result);
            }

        }
    }
}
