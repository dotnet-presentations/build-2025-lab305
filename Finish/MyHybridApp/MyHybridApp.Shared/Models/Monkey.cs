using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHybridApp.Shared.Models
{
    public class Monkey
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int Population { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
