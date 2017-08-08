using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCore
{
    public class MeterErrorCol
    {

        #region 电能表检定
        private string strGLFXDM; // 功率方向代码
        public string StrGLFXDM
        {
            get
            {
                return strGLFXDM;
            }
            set
            {
                strGLFXDM = value;
               
            }
        }
        private string strGLYSDM; // 功率因素代码
        public string StrGLYSDM
        {
            get
            {
                return strGLYSDM;
            }
            set
            {
                strGLYSDM = value;
               
            }
        }
        private string strFZDLDM; // 负载电流代码
        public string StrFZDLDM
        {
            get
            {
                return strFZDLDM;
            }
            set
            {
                strFZDLDM = value;
               
            }
        }
        private string strXBDM; // 相别代码
        public string StrXBDM
        {
            get
            {
                return strXBDM;
            }
            set
            {
                strXBDM = value;
                
            }
        }
        private string strFZLXDM; // 负载类型代码
        public string StrFZLXDM
        {
            get
            {
                return strFZLXDM;
            }
            set
            {
                strFZLXDM = value;
               
            }
        }
        private string strFYDM; // 分元代码
        public string StrFYDM
        {
            get
            {
                return strFYDM;
            }
            set
            {
                strFYDM = value;
                
            }
        }
        private string strWC1; // 误差1
        public string StrWC1
        {
            get
            {
                return strWC1;
            }
            set
            {
                strWC1 = value;
                
            }
        }
        private string strWC2; // 误差2
        public string StrWC2
        {
            get
            {
                return strWC2;
            }
            set
            {
                strWC2 = value;
           
            }
        }
        private string strWC3; // 误差3
        public string StrWC3
        {
            get
            {
                return strWC3;
            }
            set
            {
                strWC3 = value;
               
            }
        }
        private string strWC4; // 误差4
        public string StrWC4
        {
            get
            {
                return strWC4;
            }
            set
            {
                strWC4 = value;
               
            }
        }
        private string strWC5; // 误差5
        public string StrWC5
        {
            get
            {
                return strWC5;
            }
            set
            {
                strWC5 = value;
               
            }
        }
        private string strWCPJZ; // 误差平均值
        public string StrWCPJZ
        {
            get
            {
                return strWCPJZ;
            }
            set
            {
                strWCPJZ = value;
               
            }
        }
        private string strXYZ; // 误差修约值
        public string StrXYZ
        {
            get
            {
                return strXYZ;
            }
            set
            {
                strXYZ = value;
                
            }
        }
        private string strJLDM; // 结论代码
        public string StrJLDM
        {
            get
            {
                return strJLDM;
            }
            set
            {
                strJLDM = value;
               
            }
        }
        private string strWCCZXYZ; // 误差差值修约值
        public string StrWCCZXYZ
        {
            get
            {
                return strWCCZXYZ;
            }
            set
            {
                strWCCZXYZ = value;
                
            }
        }
        private string strWCCZ; //不平平衡负载与平衡负载的误差差值
        public string StrWCCZ
        {
            get
            {
                return strWCCZ;
            }
            set
            {
                strWCCZ = value;
              
            }
        }
        private string strDQBM; //地区编码
        public string StrDQMB
        {
            get
            {
                return strDQBM;
            }
            set
            {
                strDQBM = value;
               
            }
        }
        #endregion
    }
}
