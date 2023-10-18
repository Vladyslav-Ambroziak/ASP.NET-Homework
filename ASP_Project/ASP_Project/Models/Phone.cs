using System;
using System.Collections.Generic;

namespace ASP_Project.Models;

public partial class Phone
{
    public int Id { get; set; }

    public int IdManufacturer { get; set; }

    public string Series { get; set; } = null!;

    public int? IdMemory { get; set; }

    public string Color { get; set; } = null!;

    public int BatteryCapacity { get; set; }

    public float ScreenDiagonal { get; set; }

    public int Camera { get; set; }
    public float Price { get; set; }

    public virtual Manufacturer IdManufacturerNavigation { get; set; } = null!;

    public virtual Memory IdMemoryNavigation { get; set; } = null!;
}
