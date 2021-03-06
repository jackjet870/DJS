﻿using DJS.IDAL;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DJS.BLL
{
    public class Jobs
    {
        #region 属性
        /// <summary>
        /// 任务接口
        /// </summary>
        private static IDAL.IJobs iJobs = null;

        #endregion

        #region 构造函数

        static Jobs()
        {
            iJobs = DJS.DAL.DataAccessFactory.DataAccessFactory.CreateIJobs();
        }

        #endregion

        #region  获取所有Quartz中 Jobs +static List<Model.Jobs> GetJobsForQuartz()
        /// <summary>
        /// 获取所有Quartz中 Jobs
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetJobsForQuartz()
        {
            List<Model.Jobs> models = new List<Model.Jobs>();

            IList<string> groups = Common.QuartzHelp.quartzHelp.GetJobGroupNames();
            if (groups != null && groups.Count > 0)
            {
                foreach (string group in groups)
                {
                    ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(group);
                    if (jobkeys != null && jobkeys.Count > 0)
                    {
                        foreach (JobKey jobkey in jobkeys)
                        {
                            IJobDetail JobDetail = Common.QuartzHelp.quartzHelp.GetJobDetail(jobkey);
                            ITrigger iTrigger = Common.QuartzHelp.quartzHelp.GetTriggersOfJob(jobkey).FirstOrDefault();

                            TriggerState state = Common.QuartzHelp.quartzHelp.GetTriggerState(iTrigger.Key);

                            if (JobDetail != null)
                            {
                                Model.Jobs model = new Model.Jobs();
                                model.Name = jobkey.Name;
                                model.GroupName = jobkey.Group;
                                if (iTrigger != null)
                                {
                                    model.TriggerGroup = iTrigger.Key.Group;
                                    model.TriggerName = iTrigger.Key.Name;
                                    model.State = (int)state;
                                }
                                models.Add(model);
                            }
                        }
                    }
                }
            }

            return models;
        }

        /// <summary>
        /// 获取所有Quartz中 Jobs
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetJobsForQuartz(Predicate<Model.Jobs> m)
        {
            List<Model.Jobs> models = GetJobsForQuartz();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(m);
            }
            return models;
        }
        #endregion

        #region  获取所有Quartz中 Jobs数量 +static int GetJobsForQuartzCount()
        /// <summary>
        /// 获取所有Quartz中 Jobs数量
        /// </summary>
        /// <returns></returns>
        public static int GetJobsForQuartzCount()
        {
            int counts = 0;
            List<Model.Jobs> models = GetJobsForQuartz();
            if (models != null && models.Count > 0)
            {
                counts = models.Count;
            }
            return counts;
        }
        #endregion

        #region 根据任务组和任务名称触发任务 +static bool TriggerJob(string groupName,string name)
        /// <summary>
        /// 根据任务组和任务名称触发任务
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool TriggerJob(string groupName, string name)
        {
            bool ret = true;

            ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(groupName);

            if (jobkeys != null && jobkeys.Count > 0)
            {
                JobKey key = jobkeys.First(m => m.Name == name && m.Group == groupName);
                if (key != null)
                {
                    ret = Common.QuartzHelp.quartzHelp.TriggerJob(key);
                }
            }
            else
            {
                ret = false;
            }

            return ret;
        }
        #endregion

        #region 获取所有任务 +List<Model.Jobs> GetJobs()
        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetJobs()
        {
            List<Model.Jobs> models = new List<Model.Jobs>();
            List<Model.Jobs> quartzs = GetJobsForQuartz();
            models = GetModels();
            if (models != null && models.Count > 0)
            {
                models.ForEach(delegate(Model.Jobs job)
                {
                    job.State = (int)Model.Enums.TriggerState.Complete;
                    job.StateName = Common.EnumHelp.enumHelp.GetDescription(Model.Enums.TriggerState.Complete);

                    Model.Enums.TimeType st = (Model.Enums.TimeType)job.Type;
                    job.TypeName = Common.EnumHelp.enumHelp.GetDescription(st);
                });
                foreach (Model.Jobs model in models)
                {
                    Model.Jobs job = quartzs.Find(m => m.Name == model.Name);
                    if (job != null)
                    {
                        model.State = job.State;
                        Model.Enums.TriggerState st = (Model.Enums.TriggerState)job.State;
                        model.StateName = Common.EnumHelp.enumHelp.GetDescription(st);
                    }
                }
            }
            return models;
        }
        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetJobs(Predicate<Model.Jobs> m)
        {
            List<Model.Jobs> models = GetJobs();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(m);
            }
            return models;
        }
        #endregion

        #region 添加任务包括添加到Quartz +bool AddJobs(Model.Jobs model)
        /// <summary>
        /// 添加任务包括添加到Quartz
        /// </summary>
        /// <returns></returns>
        public static bool AddJobs(Model.Jobs model)
        {
            bool ret = true;
            try
            {
                if (model.Type == (int)Model.Enums.TimeType.Periodicity)
                {
                    Common.QuartzHelp.quartzHelp.AddJob(model.AssType, model.Crons, model.Name, model.GroupName, model.TriggerName, model.TriggerGroup);
                }
                if (model.Type == (int)Model.Enums.TimeType.Disposable)
                {
                    Common.QuartzHelp.quartzHelp.AddJob(model.AssType, model.Time, model.Name, model.GroupName, model.TriggerName, model.TriggerGroup);
                }
                ret = iJobs.Add(model);

            }
            catch (Exception ex)
            {
                ret = false;
                Common.LogHelp.logHelp.WriteLog(ex.Message, Model.Enums.LogType.Error);
            }
            return ret;
        }
        #endregion

        #region 根据id获取JobKey
        /// <summary>
        /// 根据id获取JobKey
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private static JobKey GetJobkeyById(Guid Id)
        {
            JobKey key = new JobKey("", "");
            Model.Jobs model = GetModelById(Id);
            if (model != null)
            {
                ISet<JobKey> jobkeys = Common.QuartzHelp.quartzHelp.GetJobKeys(model.GroupName);
                if (jobkeys != null && jobkeys.Count > 0)
                {
                    key = jobkeys.First(m => m.Name == model.Name && m.Group == model.GroupName);
                }
            }
            return key;
        }
        #endregion

        #region 根据id删除数据包括删除Quartz +static bool DelByIdAndQuartz(Guid Id)
        /// <summary>
        /// 根据id删除数据包括删除Quartz
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelByIdAndQuartz(Guid Id)
        {
            bool ret = true;
            try
            {
                JobKey key = GetJobkeyById(Id);
                if (key != null && key.Name != "")
                {
                    ret = Common.QuartzHelp.quartzHelp.DeleteJob(key);
                }
                ret = DelById(Id);
            }
            catch (Exception ex)
            {
                ret = false;
                Common.LogHelp.logHelp.WriteLog(ex.Message, Model.Enums.LogType.Error);
            }
            return ret;
        }
        #endregion

        #region 重新添加任务Quartz +bool ReAddJobs(Model.Jobs model)
        /// <summary>
        /// 重新添加任务Quartz
        /// </summary>
        /// <returns></returns>
        public static bool ReAddJobs(Model.Jobs model)
        {
            bool ret = true;
            try
            {
                if (model.Type == (int)Model.Enums.TimeType.Periodicity)
                {
                    Common.QuartzHelp.quartzHelp.AddJob(model.AssType, model.Crons, model.Name, model.GroupName, model.TriggerName, model.TriggerGroup);
                }
                if (model.Type == (int)Model.Enums.TimeType.Disposable)
                {
                    Common.QuartzHelp.quartzHelp.AddJob(model.AssType, model.Time, model.Name, model.GroupName, model.TriggerName, model.TriggerGroup);
                }

            }
            catch (Exception ex)
            {
                ret = false;
                Common.LogHelp.logHelp.WriteLog(ex.Message, Model.Enums.LogType.Error);
            }
            return ret;
        }
        #endregion

        #region 获取任务数据 +static List<Model.Jobs> GetModels()
        /// <summary>
        /// 获取任务数据
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetModels()
        {
            return iJobs.GetModels();
        }
        /// <summary>
        /// 获取任务数据
        /// </summary>
        /// <returns></returns>
        public static List<Model.Jobs> GetModels(Predicate<Model.Jobs> pre)
        {
            List<Model.Jobs> models = GetModels();
            if (models != null && models.Count > 0)
            {
                models = models.FindAll(pre);
            }
            return models;
        }
        #endregion

        #region 根据ID获取任务数据 +static Model.Jobs GetModelById(Guid Id)
        /// <summary>
        /// 根据ID获取任务数据
        /// </summary>
        /// <returns></returns>
        public static Model.Jobs GetModelById(Guid Id)
        {
            Model.Jobs model = iJobs.GetModelById(Id);
            return model;
        }
        #endregion

        #region 根据名称判断是否存在 +static bool IsExist(string name)
        /// <summary>
        /// 根据名称判断是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>true:存在 false:不存在</returns>
        public static bool IsExist(string name)
        {
            return iJobs.IsExist(name);
        }
        #endregion

        #region 根据id删除数据 +static bool DelById(Guid Id)
        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool DelById(Guid Id)
        {
            bool ret = true;
            try
            {
                ret = BLL.JobFiles.DelByJobId(Id);
                ret= iJobs.DelById(Id);
            }
            catch (Exception ex)
            { 
            }
            return ret;
        }
        #endregion

        #region 添加 +static bool Add(Model.Jobs model)
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Add(Model.Jobs model)
        {
            return iJobs.Add(model);
        }
        #endregion

        #region 根据id设置配置信息 +static bool SetConfigById(Guid Id)
        /// <summary>
        /// 根据id设置配置信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static bool SetConfigById(Guid Id)
        {
            bool ret = true;
            try
            {
                Model.Jobs model = GetModelById(Id);
                if (model != null)
                {
                    Model.DllMgr ddlmgr = BLL.DllMgr.GetModelById(model.DLLID);
                    if (ddlmgr != null)
                    {

                        DJS.SDK.IConfigClient iconfig = (DJS.SDK.IConfigClient)Common.AssemblyHelp.assembly.GetDllTypeI(model.DLLName, model.AssType.Namespace, model.AssType.Name);

                        iconfig.SetConfig(model.Name);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ret;
        }
        #endregion
         
    }
}
