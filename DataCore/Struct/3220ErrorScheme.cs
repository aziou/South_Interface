using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCore.Struct
{
    public class _3220ErrorScheme
    {
        private string errorGLFX;
        /// <summary>
        /// 功率方向
        /// </summary>
        public string ErrorGLFX { get; set; }

        private string errorGLYS;
        /// <summary>
        /// 功率因素
        /// </summary>
        public string ErrorGLYS { get; set; }

        private string errorFZDL;
        /// <summary>
        /// 负载电流
        /// </summary>
        public string ErrorFZDL { get; set; }

        private string errorFYDM;
        /// <summary>
        /// 分元 合元
        /// </summary>
        public string ErrorFYDM { get; set; }
    }
}
