﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities.Products
{
    public class Laptop:Product
    {

        public int? RAM { get; set; }
        public string? Processor { get; set; }
    }
}
