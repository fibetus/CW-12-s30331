namespace CW12_s30331.DTOs.TripDTOs;

public class TripWithPagesGetDto
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public ICollection<TripGetDto> Trips { get; set; }
}