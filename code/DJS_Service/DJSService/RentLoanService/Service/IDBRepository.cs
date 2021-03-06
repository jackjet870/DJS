﻿using RentLoanService.Model;
using RentLoanService.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentLoanService.Services
{
    public class IDBRepository : DbContext
    {

        public IDBRepository()
            : base(ConstUtility.IDBEmailService)
        { 
        }

         
        public DbSet<EmailCategory> EmailCategorys { get; set; }

        public DbSet<EmailCategoryDetail> EmailCategoryDetails { get; set; }

        public DbSet<EmailLog> EmailLogs { get; set; }

        public DbSet<EmailModel> EmailModels { get; set; }

        public DbSet<EmailUserProfileDetail> EmailUserProfileDetails { get; set; }

        public DbSet<RentLoanRisk> RentLoanRisks { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
