﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WholeSaler.Entity.Entities.Embeds.Product
{
    public class Comment
    {
  
        public string? Username { get;set; }
        public string? Title { get;set; }
        public string? Body { get;set; }
        public DateTime? CreatedDate { get; set; }
    }
}
