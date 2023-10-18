using System;
using System.Collections.Generic;

namespace ASP_Project.Models;

public partial class Manufacturer
{
    public int Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}
