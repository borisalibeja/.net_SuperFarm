using System;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Domain.Entities;
public class Product
{
    public Guid ProductId { get; set; }
    public Guid FarmId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductPrice { get; set; }
    public ProductsCategory ProductCategory { get; set; }
}