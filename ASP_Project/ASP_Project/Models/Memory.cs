using System;
using System.Collections.Generic;

namespace ASP_Project.Models;

public partial class Memory
{
    public int Id { get; set; }

    public int Amount { get; set; }

    public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
}
