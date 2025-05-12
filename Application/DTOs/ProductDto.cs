using System;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Application.DTOs
{
    public class ProductCreateDto
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public ProductsCategory ProductCategory { get; set; }
        public int? ProductPrice { get; set; }
        public string? Currency { get; set; }
        public string? StockUnit { get; set; }
        public string? StockWeight { get; set; }
        public string? ImgUrl1 { get; set; }
        public string? ImgUrl2 { get; set; }
        public string? ImgUrl3 { get; set; }
        public string? ImgUrl4 { get; set; }
        public string? ImgUrl5 { get; set; }
    }
    public class ProductUpdateDto
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public ProductsCategory ProductCategory { get; set; }
        public int? ProductPrice { get; set; }
        public string? Currency { get; set; }
        public string? StockUnit { get; set; }
        public string? StockWeight { get; set; }
        public string? ImgUrl1 { get; set; }
        public string? ImgUrl2 { get; set; }
        public string? ImgUrl3 { get; set; }
        public string? ImgUrl4 { get; set; }
        public string? ImgUrl5 { get; set; }
    }
    public class ProductDisplayDto
    {
        public Guid ProductId { get; set; }
        public Guid FarmId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public ProductsCategory ProductCategory { get; set; }
        public int? ProductPrice { get; set; }
        public string? Currency { get; set; }
        public string? StockUnit { get; set; }
        public string? StockWeight { get; set; }
        public string? ImgUrl1 { get; set; }
        public string? ImgUrl2 { get; set; }
        public string? ImgUrl3 { get; set; }
        public string? ImgUrl4 { get; set; }
        public string? ImgUrl5 { get; set; }
        public DateTime? ProductCreatedAt { get; set; }
        public DateTime? ProductUpdatedAt { get; set; }
    }
}