﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
    /// <summary>
    /// 枚举操作类
    /// </summary>
    public class EnumHelp
    { 

        #region 单例模式创建对象
        //单例模式创建对象
        private static EnumHelp _enumHelp = null;
        // Creates an syn object.
        private static readonly object SynObject = new object();
        EnumHelp()
        {
        }

        public static EnumHelp enumHelp
        {
            get
            {
                // Double-Checked Locking
                if (null == _enumHelp)
                {
                    lock (SynObject)
                    {
                        if (null == _enumHelp)
                        {
                            _enumHelp = new EnumHelp();
                        }
                    }
                }
                return _enumHelp;
            }
        }
        #endregion

        #region 从枚举中获取Description +string GetDescription(Enum enumName)
        /// <summary>
        /// 从枚举中获取Description
        /// 说明：
        /// 单元测试-->通过
        /// </summary>
        /// <param name="enumName">需要获取枚举描述的枚举</param>
        /// <returns>描述内容</returns>
        public string GetDescription(Enum enumName)
        {
            string _description = string.Empty;
            FieldInfo _fieldInfo = enumName.GetType().GetField(enumName.ToString());
            object[] attrs = _fieldInfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (attrs != null && attrs.Length > 0)
            {
                _description = ((DescriptionAttribute)attrs[0]).Description;
            }
            else
                _description = enumName.ToString();
            return _description;
        } 
        #endregion
         
        #region 获取字段Description +DescriptionAttribute[] GetDescriptAttr(FieldInfo fieldInfo)
        /// <summary>
        /// 获取字段Description
        /// </summary>
        /// <param name="fieldInfo">FieldInfo</param>
        /// <returns>DescriptionAttribute[] </returns>
        public DescriptionAttribute[] GetDescriptAttr(FieldInfo fieldInfo)
        {
            if (fieldInfo != null)
            {
                return (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            }
            return null;
        } 
        #endregion

        #region 根据Description获取枚举 +T GetEnumName<T>(string description)
        /// <summary>
        /// 根据Description获取枚举
        /// 说明：
        /// 单元测试-->通过
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="description">枚举描述</param>
        /// <returns>枚举</returns>
        public T GetEnumName<T>(string description)
        {
            Type _type = typeof(T);
            foreach (FieldInfo field in _type.GetFields())
            {
                object[] attrs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    if (((DescriptionAttribute)attrs[0]).Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", description), "Description");
        } 
        #endregion

        #region 将枚举转换为ArrayList +ArrayList ToArrayList(Type type)
        /// <summary>
        /// 将枚举转换为ArrayList
        /// 说明：
        /// 若不是枚举类型，则返回NULL
        /// 单元测试-->通过
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>ArrayList</returns>
        public ArrayList ToArrayList(Type type)
        {
            if (type.IsEnum)
            {
                ArrayList _array = new ArrayList();
                Array _enumValues = Enum.GetValues(type);
                foreach (Enum value in _enumValues)
                {
                    _array.Add(GetDescription(value));
                }
                return _array;
            }
            return null;
        } 
        #endregion

        #region 将枚举转换为SelectLists + List<Model.SelectLists> ToSelectLists(Type type)
        /// <summary>
        /// 将枚举转换为SelectLists
        /// 说明：
        /// 若不是枚举类型，则返回NULL
        /// 单元测试-->通过
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <returns>SelectLists</returns>
        public List<Model.SelectLists> ToSelectLists(Type type)
        {
            if (type.IsEnum)
            {
                List<Model.SelectLists> lists = new List<Model.SelectLists>();
                Array _enumValues = Enum.GetValues(type);
                foreach (Enum value in _enumValues)
                {
                    Model.SelectLists list = new Model.SelectLists();
                    list.Value = Convert.ToInt32(value);
                    list.Name = GetDescription(value);
                    lists.Add(list);
                }
                return lists;
            }
            return null;
        } 
        #endregion
    }
}
