namespace CW12_s30331.DTOs.TripDTOs;

public class TripGetDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public ICollection<TripCountryGetDto> Countries { get; set; }
    public ICollection<TripClientGetDto> Clients { get; set; }
}