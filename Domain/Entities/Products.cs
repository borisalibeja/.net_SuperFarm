using System;
using SuperFarm.Domain.Enums;

namespace SuperFarm.Domain.Entities;
public class Product
{
    public int Id { get; set; }
    public string? ProductName { get; set; }
    public int FarmId { get; set; }
    public string? ProductPrice { get; set; }
    public ProductsCategory ProductCategory { get; set; }
}