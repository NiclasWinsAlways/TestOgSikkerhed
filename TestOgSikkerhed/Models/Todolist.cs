using System;
using System.Collections.Generic;

namespace TestOgSikkerhed.Models;

public partial class Todolist
{
    public int Id { get; set; }

    public int CprId { get; set; }

    public string Item { get; set; } = null!;

    public virtual Cpr Cpr { get; set; } = null!;
}
