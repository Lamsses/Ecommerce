﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.Models;
public class SelectableCategory
{
    public int category_id { get; set; }
    public string? Name { get; set; }
    public bool IsSelected { get; set; } = false;
}
