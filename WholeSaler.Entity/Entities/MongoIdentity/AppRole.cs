﻿using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSaler.Entity.Entities.Enums;

namespace WholeSaler.Entity.Entities.MongoIdentity
{

    [CollectionName("Roles")]
    public class AppRole : MongoIdentityRole<Guid>
    {
      
    }
}
