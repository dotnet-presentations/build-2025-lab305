using MyHybridApp.Shared.Models;

namespace MyHybridApp.Shared.Services
{
    public interface IMonkeyService
    {
        Task<List<Monkey>> GetMonkeysAsync();
        Monkey? GetMonkeyByName(string name);
    }
}
