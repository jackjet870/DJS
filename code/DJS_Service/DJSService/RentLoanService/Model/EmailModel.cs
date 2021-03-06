﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanService.Model
{
    public class EmailModel
    {
        public int ID { get; set; }

        public string ModelID { get; set; }

        public int Type { get; set; }

        public string Contents { get; set; }

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
    }
}
