using System;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Application.DTOs
{
    public class ProductCreateDto
    {
        public Guid ProductId { get; set; }
        public Guid FarmId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductPrice { get; set; }
        public ProductsCategory ProductCategory { get; set; }
    }
    public class ProductUpdateDto
    {
        public Guid ProductId { get; set; }
        public Guid FarmId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductPrice { get; set; }
        public ProductsCategory ProductCategory { get; set; }
    }
    public class ProductDisplayDto
    {
        public Guid ProductId { get; set; }
        public Guid FarmId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductPrice { get; set; }
        public ProductsCategory ProductCategory { get; set; }
    }
}