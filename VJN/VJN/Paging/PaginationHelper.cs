namespace VJN.Paging
{
    public static class PaginationHelper
    {
        public static PagedResult<T> GetPaged<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number should be greater than zero.", nameof(pageNumber));

            if (pageSize <= 0)
                throw new ArgumentException("Page size should be greater than zero.", nameof(pageSize));
            var count = source.Count();

            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }
    }
}
