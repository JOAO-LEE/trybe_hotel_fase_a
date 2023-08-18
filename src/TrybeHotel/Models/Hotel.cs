namespace TrybeHotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// 1. Implemente as models da aplicação
public class Hotel
{
  public int HotelId { get; set; }
  public string Name { get; set; }
  public string Address { get; set; }
  public int CityId { get; set; }
  public ICollection<Room> Room { get; set; }

}

// HotelId: Chave primária (int)
// Name: string
// Address: string
// CityId: Chave estrangeira para a model City (int) Cada hotel tem vários quartos. A propriedade de navegação para Room deve se chamar Rooms (anulável). Cada hotel tem apenas uma cidade. A propriedade de navegação para City deve se chamar City (anulável).