using VJN.ModelsDTO.ItemOfCvDTOs;

namespace VJN.ModelsDTO.CvDTOs
{
    public class CvUpdateDTO
    {
        public string? NameCv { get; set; }
        public ICollection<ItemOfcvDTOforUpdate> ItemOfCvs { get; set; }
    }
}
