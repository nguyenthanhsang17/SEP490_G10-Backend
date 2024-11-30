namespace VJN.ModelsDTO.DashBoardDTOs
{
    public class DashBoardDTO
    {
        public int? TotalUser { get; set; }
        public UserStatistics UserStatistics { get; set; }
        public RevenueStatistics RevenueStatistics { get; set; }
        public PackageStatistics PackageStatistics { get; set; }
    }
    public class UserStatistics
    {
        public double EmployersNumber { get; set; }
    }

    public class RevenueStatistics
    {
        public decimal TotalRevenue { get; set; }
        public List<MonthlyRevenue> MonthlyRevenue { get; set; }
    }

    public class MonthlyRevenue
    {
        public string Month { get; set; } // ISO-8601 format: "YYYY-MM"
        public decimal Revenue { get; set; }
    }

    public class PackageStatistics
    {
        public int TotalPackagesSold { get; set; }
        public List<PopularPackage> MostPopularPackages { get; set; }
    }

    public class PopularPackage
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public int NumberSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
    public class MonthsYear
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
