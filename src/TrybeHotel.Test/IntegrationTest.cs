namespace TrybeHotel.Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Diagnostics;
using System.Xml;
using System.IO;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;

public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    public HttpClient _clientTest;

    public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        //_factory = factory;
        _clientTest = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TrybeHotelContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ContextTest>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestDatabase");
                });
                services.AddScoped<ITrybeHotelContext, ContextTest>();
                services.AddScoped<ICityRepository, CityRepository>();
                services.AddScoped<IHotelRepository, HotelRepository>();
                services.AddScoped<IRoomRepository, RoomRepository>();
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<ContextTest>())
                {
                    appContext.Database.EnsureCreated();
                    appContext.Database.EnsureDeleted();
                    appContext.Database.EnsureCreated();
                    appContext.Cities.Add(new City { CityId = 1, Name = "Manaus" });
                    appContext.Cities.Add(new City { CityId = 2, Name = "Palmas" });
                    appContext.SaveChanges();
                    appContext.Hotels.Add(new Hotel { HotelId = 1, Name = "Trybe Hotel Manaus", Address = "Address 1", CityId = 1 });
                    appContext.Hotels.Add(new Hotel { HotelId = 2, Name = "Trybe Hotel Palmas", Address = "Address 2", CityId = 2 });
                    appContext.Hotels.Add(new Hotel { HotelId = 3, Name = "Trybe Hotel Ponta Negra", Address = "Address 3", CityId = 1 });
                    appContext.SaveChanges();
                    appContext.Rooms.Add(new Room { RoomId = 1, Name = "Room 1", Capacity = 2, Image = "Image 1", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 2, Name = "Room 2", Capacity = 3, Image = "Image 2", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 3, Name = "Room 3", Capacity = 4, Image = "Image 3", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 4, Name = "Room 4", Capacity = 2, Image = "Image 4", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 5, Name = "Room 5", Capacity = 3, Image = "Image 5", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 6, Name = "Room 6", Capacity = 4, Image = "Image 6", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 7, Name = "Room 7", Capacity = 2, Image = "Image 7", HotelId = 3 });
                    appContext.Rooms.Add(new Room { RoomId = 8, Name = "Room 8", Capacity = 3, Image = "Image 8", HotelId = 3 });
                    appContext.Rooms.Add(new Room { RoomId = 9, Name = "Room 9", Capacity = 4, Image = "Image 9", HotelId = 3 });
                    appContext.SaveChanges();
                }
            });
        }).CreateClient();
    }

    [Trait("Rotas", "City")]
    [Theory(DisplayName = "Testa se a rota GET de City traz todas as cidades:")]
    [InlineData("/city")]
    public async Task TestGetAllCities(string url)
    {
        List<City> allCities = new() { new City { CityId = 1, Name = "Manaus" }, new City { CityId = 2, Name = "Palmas" } };
        var response = await _clientTest.GetAsync(url);
        response.StatusCode.Equals(200);
        response.Content.Equals(allCities);
    }

    [Theory(DisplayName = "Testa se a rota POST de City cria uma cidade:")]
    [InlineData("/city", "{\"name\": \"Rio de Janeiro\"}")]
    public async Task TestCreateCity(string url, string city)
    {
        CityDto createdRio = new() { Name = "Rio de Janeiro", CityId = 3 };
        var response = await _clientTest.PostAsync(url, new StringContent(city, System.Text.Encoding.UTF8, "application/json"));
        response.StatusCode.Equals(201);
        response.Content.Equals(createdRio);
    }

    [Trait("Rotas", "Hotel")]
    [Theory(DisplayName = "Testa se a rota GET de Hotel traz todos os hotéis:")]
    [InlineData("/hotel")]

    public async Task TestGetAllHotels(string url)
    {
        List<Hotel> allHotels = new() { new Hotel { HotelId = 1, Name = "Trybe Hotel Manaus", Address = "Address 1", CityId = 1 }, new Hotel { HotelId = 2, Name = "Trybe Hotel Palmas", Address = "Address 2", CityId = 2 }, new Hotel { HotelId = 3, Name = "Trybe Hotel Ponta Negra", Address = "Address 3", CityId = 1 } };
        var response = await _clientTest.GetAsync(url);
        response.StatusCode.Equals(200);
        response.Content.Equals(allHotels);
    }

    [Theory(DisplayName = "Testa se a rota POST de hotel cria um hotel:")]
    [InlineData("/hotel", "{ \"name\": \"Hotel Golden River\", \"address\": \"Av. Bernardo Vieira de Mello, 1204\", \"cityId\": \"1\"}")]
    public async Task TestCreateHotel(string url, string hotel)
    {
        HotelDto createdHotel = new() { Name = "Hotel Golden River", Address = "Av. Bernardo Vieira de Mello, 1204", CityId = 1, CityName = "Manaus" };
        var response = await _clientTest.PostAsync(url, new StringContent(hotel, System.Text.Encoding.UTF8, "application/json"));
        response.StatusCode.Equals(201);
        response.Content.Equals(createdHotel);
    }

    [Trait("Rotas", "Room")]
    [Theory(DisplayName = "Testa se a rota DELETE de Room deleta um quarto")]
    [InlineData("/room/1")]
    public async Task TestRemoveRoom(string url)
    {
        var response = await _clientTest.DeleteAsync(url);
        response.StatusCode.Equals(204);
        response?.Content.Equals(null);
    }

    [Theory(DisplayName = "Testa se a rota POST de hotel cria um hotel:")]
    [InlineData("/hotel", "{ \"name\": \"Suite básica\", \"capacity\": \"2\", \"image\": \"image suite\", \"hotelId\": \"1\"}")]
    public async Task TestCreateHotelRoom(string url, string hotel)
    {
        RoomDto createdRoom = new() { RoomId = 4, Name = "Suite básica", Capacity = 2, Image = "image suite", Hotel = new HotelDto { HotelId = 1, Name = "Trybe Hotel Manaus", Address = "Address 1", CityId = 1, CityName = "São Paulo" } };
        var response = await _clientTest.PostAsync(url, new StringContent(hotel, System.Text.Encoding.UTF8, "application/json"));
        response.StatusCode.Equals(201);
        response.Content.Equals(createdRoom);
    }


    //     [Theory(DisplayName = "Testa se a rota GET de Room traz os quartos daquele hotel")]
    //     [InlineData("/room/1")]
    //     public async Task TestGetRooms(string url)
    //     {
    //         List<RoomDto> allHotelRooms = new() { new RoomDto { RoomId = 1, Name = "Room 1", Capacity = 2, Image = "Image 1", Hotel =  }
    // }
    // var response = _clientTest.GetAsync(url);



    //     }
}