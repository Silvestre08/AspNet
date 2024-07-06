
namespace CityInfo.Api.Models
{
    public class CityDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int NumberOfPointsOfInterest => PointsOfInterest.Count;

        public List<PointOfInterestDto> PointsOfInterest { get; internal set; } = new List<PointOfInterestDto>();
    }
}
