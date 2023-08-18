using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 2. Desenvolva o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
            return (from cities in _context.Cities
                    select new CityDto { Name = cities.Name, CityId = cities.CityId }).ToList();
        }

        // 3. Desenvolva o endpoint POST /city
        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();
            var newCity = _context.Cities.Where(d => city.Name == d.Name).First();
            return new CityDto { Name = newCity.Name, CityId = newCity.CityId };
        }

    }
}