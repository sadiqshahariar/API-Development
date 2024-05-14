using System;
using System.Collections.Generic;

namespace Newroz_Home_Task.Models;

public partial class Quoteinformation
{
    public int Id { get; set; }

    public string Quote { get; set; } = null!;

    public string Author { get; set; } = null!;
}
