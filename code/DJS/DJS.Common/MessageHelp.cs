﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DJS.Common
{
    public class MessageHelp
    {
        /// <summary>
        /// 状态 false：失败 true：成功
        /// </summary>
        public bool State { set; get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { set; get; }
    }
}
