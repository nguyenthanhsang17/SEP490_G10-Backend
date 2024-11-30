namespace VJN.ModelsDTO.DashBoardDTOs
{
    public class DashBoardDTO
    {
        public int? TotalUser { get; set; }
        public UserStatistics UserStatistics { get; set; }
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

    public class PackageStatisticsNumberSold
    {
        public int TotalPackagesSold { get; set; }
        public List<PopularPackageNumberSold> MostPopularPackages { get; set; }
    }
    public class PackageStatisticsRevenue
    {
        public int TotalPackagesSold { get; set; }
        public List<PopularPackageRevenue> MostPopularPackages { get; set; }
    }

    public class PopularPackageNumberSold
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public int NumberSold { get; set; }
    }

    public class PopularPackageRevenue
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public decimal TotalRevenue { get; set; }
    }
    public class MonthsYear
    {
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
