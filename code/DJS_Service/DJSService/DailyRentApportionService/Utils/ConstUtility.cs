﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyRentApportionService.Utils
{
    public class ConstUtility
    {
        #region 属性
        /// <summary>
        /// 任务组接口
        /// </summary>
        private static DJS.SDK.IConfigMgr iConfigMgr = Achieve.iConfigMgr;

        #endregion

        #region 构造函数

        public ConstUtility()
        {
            //iLog = DJS.SDK.DataAccess.CreateILog();
            iConfigMgr = Achieve.iConfigMgr;

            //邮件发送邮箱
            EXECUTE_STARTDATE = DateTime.Parse(iConfigMgr == null ? DateTime.MinValue.ToString() : iConfigMgr.GetConfig("ExecuteStartDate"));
            RE_CALCULATION_DAYS = int.Parse(iConfigMgr == null ? "0" : iConfigMgr.GetConfig("ReCalculationDays"));
            ConnectionStrings = iConfigMgr.GetConfig(iConfigMgr == null ? "" : "EFDbContext");
        }

        #endregion
        //邮件发送邮箱
        public static DateTime EXECUTE_STARTDATE = DateTime.MinValue;
        public static int RE_CALCULATION_DAYS = 0;
        public static string ConnectionStrings = "";


    }
}
