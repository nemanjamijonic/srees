namespace SREES.Common.Models
{
    public class PaginatedResponse<T>
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public T Data { get; set; }

        public PaginatedResponse(T data, int totalCount, int currentPage, int pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
