using MyHybridApp.Shared.Models;
using MyHybridApp.Shared.Services;
using System.Diagnostics;
using System.Net.Http.Json;

namespace MyHybridApp.Services
{
    public class MonkeyService : IMonkeyService
    {
        private List<Monkey> _cachedMonkeys = new List<Monkey>();
        private readonly HttpClient _httpClient = HttpClientHelper.GetHttpClient();
        private string _monkeyUri = HttpClientHelper.MonkeyUrl;
        
        public async Task<List<Monkey>> GetMonkeysAsync() 
        {
            if (_cachedMonkeys.Count > 0)
                return _cachedMonkeys;

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Monkey>>(_monkeyUri);
                _cachedMonkeys = response ?? new List<Monkey>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching data: {ex.Message}");    
            }    
            
            return _cachedMonkeys;
        }

        public Monkey? GetMonkeyByName(string name)
        {
            return _cachedMonkeys?.FirstOrDefault(m => m.Name == name);
        }
    }
}
