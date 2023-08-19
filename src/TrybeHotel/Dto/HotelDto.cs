namespace TrybeHotel.Dto
{
    public class HotelDto
    {
        public int HotelId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int CityId { get; set; }
        public string? CityName { get; set; }


    }
}

// [
//     {
// 		  "hotelId": 1,
// 		  "name": "Trybe Hotel SP",
// 		  "address": "Avenida Paulista, 1400",
// 		  "cityId": 1,
// 		  "cityName": "São Paulo"
// 	  },

//   /*...*/
// ]