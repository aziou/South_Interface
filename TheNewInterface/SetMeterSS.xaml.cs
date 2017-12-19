using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TheNewInterface
{
    /// <summary>
    /// SetMeterSS.xaml 的交互逻辑
    /// </summary>
    public partial class SetMeterSS : Window
    {
        public SetMeterSS()
        {
            InitializeComponent();
        }

        private void btn_input_Click(object sender, RoutedEventArgs e)
        {

            bool result = false;
            List<string> sqlList=new List<string> ();
            if (chk_ZXYG.IsChecked == true)
            {
                sqlList=makeSql("1");
                result=OperateData.PublicFunction.ExcuteAccess(sqlList, OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "AccessLink", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml"), 0);
                if (!result)
                {
                    MessageBox.Show("插入正向有功数据失败");
                }
            }
            if (chk_FXYG.IsChecked == true)
            {
                makeSql("2");
               result= OperateData.PublicFunction.ExcuteAccess(sqlList, OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "AccessLink", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml"), 0);
                if (!result)
                {
                    MessageBox.Show("插入反向有功数据失败");
                }

            } if (chk_ZXWG.IsChecked == true)
            {
                makeSql("3");
                result=OperateData.PublicFunction.ExcuteAccess(sqlList, OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "AccessLink", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml"), 0);
                if (!result)
                {
                    MessageBox.Show("插入正向无功数据失败");
                }
            } if (chk_FXWG.IsChecked == true)
            {
                makeSql("4");
                result=OperateData.PublicFunction.ExcuteAccess(sqlList, OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "AccessLink", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml"), 0);
                if (!result)
                {
                    MessageBox.Show("插入反向无功数据失败");
                }
            }

            if (result)
            {
                MessageBox.Show("数据插入成功！");
            }
           
        }

        private string ExChangeValue(ref string str_val)
        {
            string result = "";
          try
          {
              if (str_val.Trim() == string.Empty)
              {
                  str_val = "0.00";
              }
              else
              {
                  result = str_val;
              }
          }
            catch(Exception e)
          {
            
            }
          return result;
        }

        private List<string> makeSql(string eleType)
        {
          
            List<string> sqlList = new List<string>();
           
            try
            {
              string elect_z = ""; string elect_j = ""; string elect_f = ""; string elect_p = ""; string elect_g = "";
              string itemNo = "",meterId="",meterBwh="",sqlkey="",sqlValue="",avr_value="";
              #region 确定功率方向
              switch (eleType)
              { 
                  case "1":
                      itemNo = "01701";
                      elect_z = txt_zxygz.Text.ToString();
                      elect_j = txt_zxygj.Text.ToString();
                      elect_f = txt_zxygf.Text.ToString();
                      elect_p = txt_zxygp.Text.ToString();
                      elect_g = txt_zxygGu.Text.ToString();
                      break;
                  case "2":
                      itemNo = "01702";
                      elect_z = txt_fxygZ.Text.ToString();
                      elect_j = txt_fxygj.Text.ToString();
                      elect_f = txt_fxygf.Text.ToString();
                      elect_p = txt_fxygp.Text.ToString();
                      elect_g = txt_fxygGu.Text.ToString();
                      break;
                  case "3":
                      itemNo = "01703";
                      elect_z = txt_zxwgZ.Text.ToString();
                      elect_j = txt_zxwgJ.Text.ToString();
                      elect_f = txt_zxwgF.Text.ToString();
                      elect_p = txt_zxwgP.Text.ToString();
                      elect_g = txt_zxwgGu.Text.ToString();
                      break;
                  case "4":
                      itemNo = "01704";
                      elect_z = txt_fxwgz.Text.ToString();
                      elect_j = txt_fxwgj.Text.ToString();
                      elect_f = txt_fxwgf.Text.ToString();
                      elect_p = txt_fxwgp.Text.ToString();
                      elect_g = txt_fxwgGu.Text.ToString();
                      break;
              }
                avr_value=string.Format("{0}|{1}|{2}|{3}|{4}",elect_z,elect_j,elect_f,elect_p,elect_g);
              #endregion
              int MeterCount = ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count;
              int intSelectNum = 0;
            for (int i = 0; i < MeterCount; i++)
            {
                string SQL = "INSERT INTO {0}  ({1}) VALUES ({2})";
                meterId = ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].PK_LNG_METER_ID;
                meterBwh = ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].LNG_BENCH_POINT_NO;
                if (ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].BolIfup == true)
                {
                    intSelectNum++;
                }
                switch (DataCore.Global.GB_Base.SoftType)
                {
                    case "CL3000S":
                        sqlkey = "FK_LNG_METER_ID,AVR_DEVICE_ID,LNG_BENCH_POINT_NO,AVR_PROJECT_NO,AVR_VALUE";
                        sqlValue = "'" + meterId + "','243'," + int.Parse(meterBwh) + ",'" + itemNo + "','" + avr_value + "'";
                        SQL = string.Format(SQL, "METER_COMMUNICATION", sqlkey, sqlValue);
                        break;
                    case "CL3000G":
                        SQL = string.Format(SQL, "MeterDgn");
                        break;
                }

                sqlList.Add("delete from METER_COMMUNICATION where FK_LNG_METER_ID='" + meterId + "' and AVR_PROJECT_NO='"+itemNo+"'");
                sqlList.Add(SQL);
              
            }
               

            }
            catch
            { 
            
            }


            return sqlList;
        }
    }
}
