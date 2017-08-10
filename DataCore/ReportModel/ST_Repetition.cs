using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataCore.ReportModel
{
    /// <summary>
    /// 汕头局重复性试验模板
    /// </summary>
     public  class ST_Repetition:MeterBaseInfoFactor
    {
         private string errorName;
            /// <summary>
            /// 误差名
            /// </summary>
         public string ErrorNama
         {
             get;
             set;
         }

         private string wc_001;
         /// <summary>
         /// 误差1
         /// </summary>
         public string Wc_001 { get; set; }

         private string wc_002;
         /// <summary>
         /// 误差2
         /// </summary>
         public string Wc_002 { get; set; }

         private string wc_003;
         /// <summary>
         /// 误差3
         /// </summary>
         public string Wc_003 { get; set; }

         private string wc_004;
         /// <summary>
         /// 误差4
         /// </summary>
         public string Wc_004 { get; set; }

         private string wc_005;
         /// <summary>
         /// 误差5
         /// </summary>
         public string Wc_005 { get; set; }

         private string wc_006;
         /// <summary>
         /// 误差6
         /// </summary>
         public string Wc_006 { get; set; }

         private string wc_007;
         /// <summary>
         /// 误差7
         /// </summary>
         public string Wc_007 { get; set; }

         private string wc_008;
         /// <summary>
         /// 误差8
         /// </summary>
         public string Wc_008{ get; set; }

         private string wc_009;
         /// <summary>
         /// 误差9
         /// </summary>
         public string Wc_009{ get; set; }

         private string wc_010;
         /// <summary>
         /// 误差10
         /// </summary>
         public string Wc_010 { get; set; }


         private string wc_pjz;
         /// <summary>
         /// 误差PJZ
         /// </summary>
         public string Wc_pjz { get; set; }

         private string wc_HZZ;
         /// <summary>
         /// 误差HZZ
         /// </summary>
         public string Wc_HZZ { get; set; }


    }
}
