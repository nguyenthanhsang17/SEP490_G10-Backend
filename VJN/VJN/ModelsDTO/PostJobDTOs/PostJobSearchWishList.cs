namespace VJN.ModelsDTO.PostJobDTOs
{
    public class PostJobSearchWishList
    {
        public int sort {  get; set; }
        public int pageNumber { get; set; }
        public decimal? Latitude { get; set; } //ko cân giao dien lay luoon cuar user
        public decimal? Longitude { get; set; }//ko cân giao dien lay luoon cuar user

    }
}
