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
                List<string> tempSea2 = new List<string>();

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
                string strValue2= "";
                for (int i = 0, j = 0; i < ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count; i++,j++)
                {

                    if (ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].BolIfup == false) continue;
                    strValue = string.Format("{0}{1}", StartString, (long.Parse(LastNumber) + j).ToString().PadLeft(LastNumberLen, '0'));
                    if (rbtn_two.IsChecked == true)
                    {
                        strValue2 = string.Format("{0}{1}", StartString, (long.Parse(LastNumber) + j+1).ToString().PadLeft(LastNumberLen, '0'));
                        ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].AVR_SEAL_2 = strValue2;

                        tempSea2.Add(strValue2);
                    }
                    ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].AVR_SEAL_1 = strValue;

                    tempZcbh.Add(ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].PK_LNG_METER_ID);
                    tempSeal.Add(strValue);
                    
                }
                if (rbtn_two.IsChecked == true)
                {
                    SetMeterSeal(tempZcbh, tempSeal, tempSea2);
                }
                else
                {
                    SetMeterSeal(tempZcbh, tempSeal);
                
                }
                
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
                    ColUpdate = "chrQianFeng1";
                    ColZCBH = "intMyId";
                    ColChecktime = "datJdrq";
                    break;
                case "CL3000S":
                    ColUpdate = "AVR_SEAL_1";
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
                int cout = 0;
                foreach (string temp in MeterId)
                {
                    SQL.Add(string.Format("update {0} set {1} ='{2}' where {3}={4}", csPublicMember.strTableName, ColUpdate, Seal001[cout], ColZCBH, temp));
                    cout++;
                }
            }
            else if (csPublicMember.strSoftType == "CL3220NW")
            {
                int cout = 0;
                foreach (string temp in MeterId)
                {
                    SQL.Add(string.Format("update {0} set {1} ='{2}' where {3}='{4}' ", csPublicMember.strTableName, ColUpdate, Seal001[cout], ColZCBH, temp));
                    cout++;
                }
               
            }
             else
            {
                int cout = 0;
                foreach (string temp in MeterId)
                {
                    SQL.Add(string.Format("update {0} set {1} ='{2}' where {3}={4}", csPublicMember.strTableName, ColUpdate, Seal001[cout], ColZCBH, temp));
                    cout++;
                }
            }

            OperateData.PublicFunction csPublic = new OperateData.PublicFunction();

            csPublic.ExcuteAccess(SQL, "");


        }

        private void SetMeterSeal(List<string> MeterId, List<string> Seal001,List<string> Seal002)
        {
            string ColUpdate = "", ColZCBH = "", ColChecktime = "", ColUpdate2="";
            switch (csPublicMember.strSoftType)
            {
                case "CL3000G":
                case "CL3000F":
                case "CL3000DV80":
                    ColUpdate = "chrQianFeng1";
                    ColZCBH = "intMyId";
                    ColChecktime = "datJdrq";
                    ColUpdate2 = "chrQianFeng2";
                    break;
                case "CL3000S":
                    ColUpdate = "AVR_SEAL_1";
                    ColZCBH = "PK_LNG_METER_ID";
                    ColChecktime = "DTM_TEST_DATE";
                    ColUpdate2 = "AVR_SEAL_2";
                    break;
                case "CL3220NW":
                    ColUpdate = "STR_SEAL001";
                    ColZCBH = "STR_BARCODE";
                    ColChecktime = "DTE_TESTING_DATE";
                    ColUpdate2 = "";
                    break;

            }
            List<string> SQL = new List<string>();
            if (csPublicMember.strSoftType == "CL3000S")
            {
                for (int i = 0; i < MeterId.Count; i++)
                {
                    SQL.Add(string.Format("update {0} set {1} ='{2}',{5}='{6}' where {3}='{4}'", csPublicMember.strTableName, ColUpdate, Seal001[i], ColZCBH, MeterId[i],ColUpdate2, Seal002[i]));
                }
               
            }
            else if (csPublicMember.strSoftType == "CL3220NW")
            {
                int cout = 0;
                foreach (string temp in MeterId)
                {
                    SQL.Add(string.Format("update {0} set {1} ='{2}',{5}='{6}' where {3}={4} ", csPublicMember.strTableName, ColUpdate, Seal001[cout], ColZCBH, temp, ColUpdate2,Seal002[cout]));
                    cout++;
                }

            }
            else
            {
              
                for (int i = 0; i < MeterId.Count; i++)
                {
                    SQL.Add(string.Format("update {0} set {1} ='{2}',{5}='{6}' where {3}={4}", csPublicMember.strTableName, ColUpdate, Seal001[i], ColZCBH, MeterId[i], ColUpdate2, Seal002[i]));
                }
            }

            OperateData.PublicFunction csPublic = new OperateData.PublicFunction();

            csPublic.ExcuteAccess(SQL, "");


        }
    }
}
