using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities.Products
{
    public class Laptop:Product
    {
        public string? OperatingSystem { get; set; }
        public int? RAM { get; set; }
        public string? Processor { get; set; }
        public int? ProcessorGeneration { get; set; }
        public string? VideoCard { get; set; }
        public string? VideoCardType { get; set; }
        public int? VideoCardRam { get; set; }
        public bool? Hdmi { get; set; }
        public bool? TouchingScreen { get; set; }
        public bool? FingerPrintReeder { get; set; }
        public string? ScreenType { get; set; }
        public string? ScreenResolution { get; set; }
        public bool? OpticalDrive { get; set; }
        public string? DeviceWeight { get; set; }
        public decimal? ScreenSize { get; set; }
        public int? SsdCapacity { get; set; }
        public int? HarddiskCapacity { get; set; }
        public bool? CardReeder { get; set; }
        public string? LaptopType { get; set; }

        
    }
}
