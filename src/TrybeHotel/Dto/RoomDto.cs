namespace TrybeHotel.Dto
{
     public class RoomDto
     {
          public int RoomId { get; set; }
          public string? Name { get; set; }
          public int Capacity { get; set; }
          public string? Image { get; set; }
          public HotelDto? Hotel { get; set; }
     }
}

// [
//     {
// 		  "roomId": 1,
// 		  "name": "Suite básica",
// 		  "capacity": 2,
// 		  "image": "image suite",
// 		  "hotel": {
//   			"hotelId": 1,
// 			  "name": "Trybe Hotel SP",
// 			  "address": "Avenida Paulista, 1400",
// 			  "cityId": 1,
// 			  "cityName": "São Paulo"
// 		  }
// 	  },

//   /*...*/
// ]