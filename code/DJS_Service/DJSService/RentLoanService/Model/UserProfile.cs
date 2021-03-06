﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanService.Model
{
    public class UserProfile
    {
        [Key]
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
