﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceLibrary.Models;
public class CustomerLogsModel
{
    public int customerlog_id { get; set; }

    public int customer_id { get; set; }


    public string log_msg { get; set; }


}
