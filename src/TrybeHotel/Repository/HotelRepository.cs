using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Desenvolva o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            return (from hotel in _context.Hotels
                    join city in _context.Cities on hotel.CityId equals city.CityId
                    select new HotelDto { HotelId = hotel.HotelId, Name = hotel.Name, Address = hotel.Address, CityId = city.CityId, CityName = city.Name }).ToList();
        }

        // 5. Desenvolva o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
            throw new NotImplementedException();
        }
    }
}