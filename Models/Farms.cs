using System;

namespace SuperFarm.Models;

public class Farm
{
    public int user_id { get; set; }
    public string? farm_name { get; set; }
    public string? farm_address { get; set; }
    public DateTime creation_date { get; set; }
}