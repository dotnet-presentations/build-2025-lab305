using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyHybridApp.Shared.Models;

namespace MyHybridApp.Shared.Services
{
    public interface IMonkeyService
    {
        Task<List<Monkey>> GetMonkeysAsync();
        Monkey? GetMonkeyByName(string name);
    }
}
