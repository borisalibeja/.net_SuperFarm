using System;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Application.DTOs
{
    public class ProductCreateDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public int FarmId { get; set; }
        public string? ProductPrice { get; set; }
        public ProductsCategory ProductCategory { get; set; }
    }
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public int FarmId { get; set; }
        public string? ProductPrice { get; set; }
        public ProductsCategory ProductCategory { get; set; }
    }
    public class ProductDisplayDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public int FarmId { get; set; }
        public string? ProductPrice { get; set; }
        public ProductsCategory ProductCategory { get; set; }
    }
}