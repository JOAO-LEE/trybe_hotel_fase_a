namespace TrybeHotel.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    // 1. Implemente as models da aplicação
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public ICollection<Hotel> Hotels { get; set; }

    }
}

// City representará as cidades da aplicação e deverá conter os seguintes campos: - `CityId`: Chave primária (int) - `Name`: string Cada cidade tem vários hotéis. A propriedade de navegação para `Hotel` deve se chamar `Hotels` (anulável).