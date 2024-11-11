using Microsoft.AspNetCore.Mvc;

namespace VJN.ModelsDTO.ImagePostJobDTO
{
    public class ImagePostJobForUpdateDTO
    {
        public int? postid {  get; set; }
        public List<IFormFile> files { get; set; }
        public List<int> imageIds { get; set; }
    }
}
