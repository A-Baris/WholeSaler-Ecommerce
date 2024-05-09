﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities.Embeds.Order
{
    public class ShippingAddress
    {
        public string? Header { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Neighborhood { get; set; }
        public string? ApartmentInfo { get; set; }
        public string? ZipCode { get; set; }
        public string? Description { get; set; }
    }
}
