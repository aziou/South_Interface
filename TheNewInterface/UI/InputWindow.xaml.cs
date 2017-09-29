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

namespace TheNewInterface.UI
{
    /// <summary>
    /// InputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputWindow : Window
    {
        public InputWindow()
        {
            InitializeComponent();
        }

        private void btn_make_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string strStartNumber = txt_input.Text;
                string StartString = string.Empty; //号码半部分
                string LastNumber = string.Empty; //号码后半部分
                int LastNumberLen = 0;
                List<string> tempZcbh = new List<string>();
                List<string> tempSeal = new List<string>();

                for (int i = strStartNumber.Length - 1; i >= 0; i--)
                {
                    if (LastNumber.Length >= 9)    //控制数字的大小防止溢出
                    {
                        break;
                    }

                    if ("0123456789".IndexOf(strStartNumber[i]) != -1)
                    {
                        LastNumber = strStartNumber[i] + LastNumber;
                    }
                    else
                    {//选项只提取字符串最后面第一个数字串
                        break;
                    }
                }
                StartString = strStartNumber.Substring(0, strStartNumber.Length - LastNumber.Length);
                LastNumberLen = LastNumber.Length;
                if (LastNumberLen == 0)
                {
                    LastNumber = "0";
                }
                string strValue = "";
                for (int i = 0, j = 0; i < ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count; i++,j++)
                {
                    if (ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].BolIfup == false) continue;
                    strValue = string.Format("{0}{1}", StartString, (long.Parse(LastNumber) + j).ToString().PadLeft(LastNumberLen, '0'));
                    ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].AVR_SEAL_1 = strValue;
                    tempZcbh.Add(ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].AVR_ASSET_NO);
                    tempSeal.Add(strValue);
                }
                SetMeterSeal(tempZcbh, tempSeal);
                this.Close();
            }
            catch
            { 
            }
           

        }

        private void SetMeterSeal(List<string> MeterId,List<string> Seal001)
        {
            string ColUpdate = "", ColZCBH = "", ColChecktime = "";
            switch (csPublicMember.strSoftType)
            {
                case "CL3000G":
                case "CL3000F":
                case "CL3000DV80":
                    ColUpdate = "chrSetNetState";
                    ColZCBH = "intMyId";
                    ColChecktime = "datJdrq";
                    break;
                case "CL3000S":
                    ColUpdate = "CHR_UPLOAD_FLAG";
                    ColZCBH = "PK_LNG_METER_ID";
                    ColChecktime = "DTM_TEST_DATE";
                    break;
                case "CL3220NW":
                    ColUpdate = "STR_SEAL001";
                    ColZCBH = "STR_BARCODE";
                    ColChecktime = "DTE_TESTING_DATE";
                    break;

            }
            List<string> SQL = new List<string>();
            if (csPublicMember.strSoftType == "CL3000S")
            {
                //foreach (string temp in MeterId)
                //{
                //    SQL.Add(string.Format("update {0} set {1} ='{2}' where {3}='{4}' and {5} =#{6}#", csPublicMember.strTableName, ColUpdate, D_or_U.ToString(), ColZCBH, temp, ColChecktime, CheckTime));
                //}
            }
            else if (csPublicMember.strSoftType == "CL3220NW")
            {
                int cout = 0;
                foreach (string temp in MeterId)
                {
                    SQL.Add(string.Format("update {0} set {1} ='{2}' where {3}='{4}' ", csPublicMember.strTableName, ColUpdate, Seal001[cout], ColZCBH, temp, ColChecktime));
                    cout++;
                }
               
            }
             else
            {
                //foreach (string temp in MeterId)
                //{
                //    SQL.Add(string.Format("update {0} set {1} ='{2}' where {3}={4} and {5} =#{6}#", csPublicMember.strTableName, ColUpdate, D_or_U.ToString(), ColZCBH, temp, ColChecktime, CheckTime));
                //}
            }

            OperateData.PublicFunction csPublic = new OperateData.PublicFunction();

            csPublic.ExcuteAccess(SQL, "");


        }
    }
}
