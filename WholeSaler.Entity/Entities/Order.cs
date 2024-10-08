﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities.Embeds.Order;
using WholeSaler.Entity.Entities.Enums;

namespace WholeSaler.Entity.Entities
{
    public class Order: BaseEntity
    {
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? ShoppingCartId { get; set; }
        public List<Product>? Products { get; set; }
        public decimal? TotalOrderAmount { get; set; }
        public ShippingAddress? ShippingAddress { get; set; }
        public OrderPayment? OrderPayment { get; set; }
        public CancellationRequest? CancellationRequest { get; set; }

    }
}
