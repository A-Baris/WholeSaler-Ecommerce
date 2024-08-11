using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities.Products
{
    public class Television:Product
    {
        public string? Inc { get; set; }
        public string? OperatingSystem { get; set; }
        public string? DisplayTech { get; set; }
        public string? Resolution { get; set; }
        public int? RefreshRate { get; set; }
        public int? YearOfProduction { get; set; }
        public bool? SateliteReceiver { get; set; }
        public bool? SmartTv { get; set; }
        public bool? Wifi { get; set; }
        public bool? Hdr { get; set; }
    }
}
