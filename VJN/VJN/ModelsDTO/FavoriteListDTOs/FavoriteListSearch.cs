namespace VJN.ModelsDTO.FavoriteListDTOs
{
    public class FavoriteListSearch
    {
        public string? Description { get; set; }
        public int? sort {  get; set; }//sort ==0 ko con sỏrt == 1 
        public int? pageNumber { get; set; }
    }
}
