using System;
using SuperFarm.Models.Enums;

namespace SuperFarm.Models.DTOs.ProductDTOs
{
    public class ProductDisplayDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public int FarmId { get; set; }
        public string? ProductPrice { get; set; }
        public ProductsCategory ProductCategory { get; set; }
    }
}