using VJN.Models;
using VJN.ModelsDTO.ItemOfCvDTOs;

namespace VJN.ModelsDTO.CvDTOs
{
    public class CvDTODetail
    {
        public int CvId { get; set; }
        public int? UserId { get; set; }
        public string? NameCv { get; set; }
        public virtual ICollection<ItemOfcvDTOforView> ItemOfCvs { get; set; }
    }
}
