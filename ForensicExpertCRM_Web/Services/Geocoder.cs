using ForensicExpertCRM_Web.Data;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace ForensicExpertCRM_Web.Services
{
    public class Geocoder
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public Geocoder(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;   
        }

        public async Task<(double lat, double lng)> GetCoordinatesAsync(string address)
        {


            var url = $"https://geocode-maps.yandex.ru/1.x/?apikey={_apiKey}&format=json&geocode={address}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get coordinates for address '{address}' from Yandex API");
            }

            var json = await response.Content.ReadAsStringAsync();

            var jObject = JObject.Parse(json);

            var pos = jObject.SelectToken("$.response.GeoObjectCollection.featureMember[0].GeoObject.Point.pos").Value<string>();
            var coords = pos.Split(' ');

            var lat = double.Parse(coords[1], CultureInfo.InvariantCulture);
            var lng = double.Parse(coords[0], CultureInfo.InvariantCulture);

            return (lat, lng);
        }

        public double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            const double r = 6371; // радиус Земли в километрах
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Asin(Math.Sqrt(a));
            return r * c;
        }

        private double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
