namespace VJN.ModelsDTO.UploadMultiFiles
{
    public class UploadMultiFileForJob
    {
        public int postid { get; set; }
        public List<IFormFile> files {  get; set; }

    }
}
