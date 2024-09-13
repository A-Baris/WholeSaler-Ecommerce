﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WholeSaler.Web.Models.ViewModels.UserVM
{
    public class UserAdressVM
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
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
