using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            // init dummy data
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New Your City",
                    Description = "New York Description",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "New Your City",
                            Description = "New York Description",
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "New Your City 2",
                            Description = "New York 2 Description",
                        },
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "Antwerp Description",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "New Your City",
                            Description = "New York Description",
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "New Your City 2",
                            Description = "New York 2 Description",
                        },
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "Paris Description",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "New Your City",
                            Description = "New York Description",
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "New Your City 2",
                            Description = "New York 2 Description",
                        },
                    }
                }
            };
        }
    }
}
