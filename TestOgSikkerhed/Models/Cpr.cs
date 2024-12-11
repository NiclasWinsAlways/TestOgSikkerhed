using System;
using System.Collections.Generic;

namespace TestOgSikkerhed.Models;

public partial class Cpr
{
    public int Id { get; set; }

    public string CprNr { get; set; } = null!;

    public string User { get; set; } = null!;

    public virtual ICollection<Todolist> Todolists { get; set; } = new List<Todolist>();
}
