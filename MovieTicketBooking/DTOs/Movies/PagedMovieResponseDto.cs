namespace MovieTicketBooking.DTOs.Movies
{
    public class PagedMovieResponseDto
    {
        public IEnumerable<MovieSummaryDto> Movies { get; set; } = new List<MovieSummaryDto>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
