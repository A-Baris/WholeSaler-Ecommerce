
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.BaseProduct;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product.Consricted
{
    public class Laptop:ProductVm
    {
        [DisplayName("Operating System")]
        public string? OperatingSystem { get; set; }
        [DisplayName("Ram")]
        public int? RAM { get; set; }
        [DisplayName("Processor")]
        public string? Processor { get; set; }
        [DisplayName("Processor Generation")]
        public int? ProcessorGeneration { get; set; }
        [DisplayName("Video Card Brand")]
        public string? VideoCard { get; set; }
        [DisplayName("Video Card Type")]
        public string? VideoCardType { get; set; }
        [DisplayName("Video Card Ram")]
        public int? VideoCardRam { get; set; }
    
        [DisplayName("Screen Type")]
        public string? ScreenType { get; set; }
        [DisplayName("Screen Resolution\n(ex; 1024 x 800)")]
        public string? ScreenResolution { get; set; }
       
        [DisplayName("Device Weight")]
        public string? DeviceWeight { get; set; }
        [DisplayName("Screen Size / Inc")]
        public decimal? ScreenSize { get; set; }
        [DisplayName("SSD Memory Capacity")]
        public int? SsdCapacity { get; set; }
        [DisplayName("Harddisk Memory Capacity")]
        public int? HarddiskCapacity { get; set; }
        [HiddenInput]
        [DisplayName("Laptop Type")]
        public string? LaptopType { get; set; }
        public bool? Hdmi { get; set; }
        public bool? TouchingScreen { get; set; }
        public bool? FingerPrintReeder { get; set; }
        public bool? OpticalDrive { get; set; }
        public bool? CardReeder { get; set; }
    }
}
