﻿namespace WholeSaler.Api.DTOs.Category
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<SubCategoryDTo> SubCategories { get; set; }
    }
}
