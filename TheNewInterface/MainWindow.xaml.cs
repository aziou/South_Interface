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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataCore;
using System.Collections.ObjectModel;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;
using TheNewInterface.ViewModel;
using OperateOracle;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Windows.Controls.Primitives;
namespace TheNewInterface
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = TheNewInterface.ViewModel.AllMeterInfo.CreateInstance();
            Grid_showItem.DataContext = ViewModel.ViewMember.CreateInstance();
            DgBasicError.DataContext = ViewModel.ViewMember.CreateInstance();
            DgBasicError1.DataContext = ViewModel.ViewMember.CreateInstance();
            DgBasicError2.DataContext = ViewModel.ViewMember.CreateInstance();
            DgBasicError3.DataContext = ViewModel.ViewMember.CreateInstance();
            DgBasicError4.DataContext = ViewModel.ViewMember.CreateInstance();
            DgBasicError5.DataContext = ViewModel.ViewMember.CreateInstance();

            
        }
        public string TableName = "VT_SB_JKDNBJDJL";
        Thread UpdateThread;
        public readonly string BaseConfigPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml";
        #region Tab 001
        private void Btn_Config_Click(object sender, RoutedEventArgs e)
        {
            string CurPath = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_DataPath", "Value", "", BaseConfigPath), NowPath = "";
            BasePage basepage = new BasePage();
            basepage.ShowDialog();
            NowPath = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_DataPath", "Value", "", BaseConfigPath);
            if (NowPath != CurPath)
                ReLoadCheckTime();
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btn_update.IsEnabled = false;
                if (OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "cmb_Company", "Value", "", BaseConfigPath) == "湛江")
                    PrintTheWZ();
                if (cmb_WorkNumList.Text.Trim() == "")
                {
                    MessageBox.Show("请选择工单号！");
                    btn_update.IsEnabled = true;
                    return;
                }
                //OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", cmb_WorkNumList.Text.ToString(), BaseConfigPath);
                if (MessageBox.Show("请确定你要上传的工作单为：" + cmb_WorkNumList.Text, "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", cmb_WorkNumList.Text, BaseConfigPath);
                    DataCore.Global.GB_Base.MeterGZDBH = cmb_WorkNumList.Text;
                }
                else
                {
                    btn_update.IsEnabled = true;
                    return;
                }

                OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "CheckTimeFlag", "Value", cmb_CheckTime.Text, BaseConfigPath);
                //OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", "07522300987", BaseConfigPath);

                int MeterCount = ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count;
                List<string> UpDateMeterId = new List<string>();
                for (int i = 0; i < MeterCount; i++)
                {
                    if (ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].BolIfup == true)
                    {
                        UpDateMeterId.Add(ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].PK_LNG_METER_ID);
                    }
                }
                string testseal = ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[0].AVR_SEAL_1;
                if (UpDateMeterId.Count == 0)
                {
                    MessageBox.Show("你没有选择要上传的表", "提示", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    btn_update.IsEnabled = true;
                    return;
                }
                this.UpdateProgress.Maximum = UpDateMeterId.Count;
                listBox_UpInfo.Items.Clear();

                UpDateInfomation upinfo = new UpDateInfomation();
                upinfo.Lis_PkId = UpDateMeterId;
                SoftType_G.csFunction s_function = new SoftType_G.csFunction();
                UpdateThread = new Thread(new ParameterizedThreadStart(UpdateToOracle));
                UpdateThread.Start(upinfo);
            }
            catch(Exception ErrorUp)
            {
              //  MessageBox.Show(ErrorUp.Message);
            }
          
           
        }
        #region update
        public void UpdateToOracle(object o)
        {
            UpDateInfomation Lis_Id = o as UpDateInfomation;
            UpdateInfoThread(Lis_Id.Lis_PkId.Count, Lis_Id.Lis_PkId);
        }
        public void UpdateInfoThread(double countItem, List<string> lis_UP_ID)
        {
            try
            {
                double i = 0;
                int sleepTime = 100;
                string ErrorWord = "";
                int ErrorMeterNum = 0;

                double t;
                List<string> MeterUp_UpId = new List<string>();
                List<string> SplitList = new List<string>();
                List<string> MeterUp_info = new List<string>();
                List<string> Seal_info = new List<string>();
                List<string> Demand_info = new List<string>();
                Mis_Interface_Driver.MisDriver cs_Function = null;
                switch (csPublicMember.strSoftType)
                {
                    case "CL3000G":
                    case "CL3000F":
                    case "CL3000DV80":
                        cs_Function = new SoftType_G.csFunction();
                        break;
                    case "CL3000S":
                        cs_Function = new SoftType_S.csFunction();
                        break;
                   

                }
                if (DataCore.Global.GB_Base.IsTerminal == true)
                {
                    switch (csPublicMember.strSoftType)
                    {
                        case "CL3000G":
                        case "CL3000F":
                        case "CL3000DV80":
                            cs_Function = new SoftType_G_ZD.csFunction();
                            break;
                        case "CL3000S":
                            cs_Function = new SoftType_S_ZD.csFunction();
                            break;
                        case "CL3220NW":
                            cs_Function = new SoftType_3220.csFunction();
                            break;
                    }

                    
                }
                Stopwatch Watch = new Stopwatch();
                Watch.Start();
                // SoftType_G.csFunction cs_G_Function = new SoftType_G.csFunction();
                ShowWord(DateTime.Now.ToString(), "开始执行");
                int tempiny = ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count;
                foreach (MeterBaseInfoFactor temp in ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo)
                {
                    if (temp.AVR_TOTAL_CONCLUSION.Trim() != "合格" && DataCore.Global.GB_Base.IsUpDisOK == true)
                    {
                        continue;
                    }
                    if (temp.BolIfup == true)
                    {
                        t = i + 1;
                        i = t < countItem ? t : countItem;
                        MeterUp_info.Clear();
                        Stopwatch watch = new Stopwatch();
                        watch.Start();

                        DataCore.Global.GB_Base.MultiCheckTime = temp.DTM_TEST_DATE.ToString().Trim();

                        cs_Function.DeleteMis(temp.AVR_ASSET_NO); 

                        MeterUp_info.Add("第" + temp.LNG_BENCH_POINT_NO.ToString() + "表位" + cs_Function.UpadataBaseInfo(temp.PK_LNG_METER_ID, out Seal_info));

                        #region Add SEAL
                        foreach (string temp_id in Seal_info)
                        {
                            MeterUp_info.Add("添加铅封：" + temp_id + "成功");
                        }
                        #endregion

                        MeterUp_info.Add(cs_Function.UpdataErrorInfo(temp.PK_LNG_METER_ID));

                        MeterUp_info.Add(cs_Function.UpdataJKRJSWCInfo(temp.PK_LNG_METER_ID));


                        MeterUp_info.Add(cs_Function.UpdataJKXLWCJLInfo(temp.PK_LNG_METER_ID, out Demand_info));

                        #region Add demand
                        foreach (string temp_id in Demand_info)
                        {
                            MeterUp_info.Add(temp_id);
                        }
                        #endregion

                        MeterUp_info.Add(cs_Function.UpdataSDTQWCJLInfo(temp.PK_LNG_METER_ID));


                        MeterUp_info.Add(cs_Function.UpdataDNBSSJLInfo(temp.PK_LNG_METER_ID));



                        //ShowWord(watch.ElapsedMilliseconds.ToString(), "上传第"+i.ToString()+"表：");
                        ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[Convert.ToInt16(temp.Int_ItemsNum)].CHR_UPLOAD_FLAG = "已上传";
                        MeterUp_info.Add(cs_Function.UpdataDNBZZJLInfo(temp.PK_LNG_METER_ID));
                        MeterUp_UpId.Add(temp.PK_LNG_METER_ID);
                        foreach (string temp_id in MeterUp_info)
                        {
                            SplitList = TransStr(temp_id);
                            foreach (string split in SplitList)
                            {
                                listBox_UpInfo.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string, double>(UpDateMeter), split, i);
                                Thread.Sleep(sleepTime);
                            }

                        }
                        foreach (string temp_id in MeterUp_info)
                        {
                            if (temp_id.IndexOf("失败") > 0) ErrorMeterNum = ErrorMeterNum + 1;
                            break;
                        }

                    }



                }
                Watch.Stop();
                ShowWord(Watch.ElapsedMilliseconds.ToString(), "执行完毕");
                MessageBox.Show("成功上传 :" + (Convert.ToInt16(countItem) - ErrorMeterNum).ToString() + "个表,失败:" + ErrorMeterNum + "个.");

                try
                {
                    UpdateThread.Abort();
                }
                catch (Exception e)
                { }
                finally
                {

                    if (listBox_UpInfo.Items.Count != 0)
                    {
                        this.listBox_UpInfo.Dispatcher.Invoke(new Action(() =>
                        {
                            SetMeterUpdateFlag(MeterUp_UpId, cmb_CheckTime.Text.ToString(), 1);
                            this.listBox_UpInfo.UpdateLayout();
                            btn_update.IsEnabled = true;
                            this.listBox_UpInfo.ScrollIntoView(listBox_UpInfo.Items[listBox_UpInfo.Items.Count - 1]);
                        }));
                    }


                }

            }
            catch(Exception errorU)
            {
               // MessageBox.Show(errorU.Message);
            }
          

        }

        private void UpDateMeter(string Meter_update_info, double progressCount)
        {



            SoftType_G.csFunction cs_G_Function = new SoftType_G.csFunction();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            listBox_UpInfo.Items.Add(Meter_update_info);


            listBox_UpInfo.UpdateLayout();
            listBox_UpInfo.ScrollIntoView(listBox_UpInfo.Items[listBox_UpInfo.Items.Count - 1]);

            this.UpdateProgress.Value = progressCount;
            // listBox_UpInfo.UpdateLayout();
            watch.Stop();
            // listBox_UpInfo.Items.Add("使用时间为：" + watch.ElapsedMilliseconds.ToString() + "毫秒");



        }
        #endregion
        #region  SetMeterUpdateFlag

        private void SetMeterUpdateFlag(List<string> MeterId, string CheckTime, int D_or_U)
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
                    ColUpdate = "BOL_UPLOAD_FLAG";
                    ColZCBH = "INT_MYID";
                    ColChecktime = "DTE_TESTING_DATE";
                    break;

            }
            List<string> SQL = new List<string>();
            if (csPublicMember.strSoftType == "CL3000S")
            {
                foreach (string temp in MeterId)
                {
                    SQL.Add(string.Format("update {0} set {1} ='{2}' where {3}='{4}' and {5} =#{6}#", csPublicMember.strTableName, ColUpdate, D_or_U.ToString(), ColZCBH, temp, ColChecktime, CheckTime));
                }
            }
            else if (csPublicMember.strSoftType == "CL3220NW")
            {
                string Flag = D_or_U.ToString() == "0" ? "True" : "False";
                foreach (string temp in MeterId)
                {
                    SQL.Add(string.Format("update {0} set {1} ='{2}' where {3}={4} and {5} =#{6}#", csPublicMember.strTableName, ColUpdate, Flag, ColZCBH, temp, ColChecktime, CheckTime));
                }
            }
            else
            {
                foreach (string temp in MeterId)
                {
                    SQL.Add(string.Format("update {0} set {1} ='{2}' where {3}={4} and {5} =#{6}#", csPublicMember.strTableName, ColUpdate, D_or_U.ToString(), ColZCBH, temp, ColChecktime, CheckTime));
                }
            }
            OperateData.PublicFunction csPublic = new OperateData.PublicFunction();

            csPublic.ExcuteAccess(SQL, "");


        }
        #endregion
        private void btn_download_Click(object sender, RoutedEventArgs e)
        {
            DownLoadPage downloadpage = new DownLoadPage();
            downloadpage.ShowDialog();
        }

        private void btn_deldteMis_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_WorkNumList.Text.Trim() == "")
            {
                MessageBox.Show("请选择工单号！");
                return;
            }
            //OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", cmb_WorkNumList.Text.ToString(), BaseConfigPath);
            if (MessageBox.Show("请确定你要删除的工作单为：" + cmb_WorkNumList.Text, "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", cmb_WorkNumList.Text, BaseConfigPath);
                DataCore.Global.GB_Base.MeterGZDBH = cmb_WorkNumList.Text;
            }
            else
            {
                return;
            }

            //  OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", "07522300987", BaseConfigPath);

            int MeterCount = ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count;
            List<string> UpDateMeterId = new List<string>();
            for (int i = 0; i < MeterCount; i++)
            {
                if (ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].BolIfup == true)
                {
                    UpDateMeterId.Add(ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].AVR_ASSET_NO);
                }
            }
            if (UpDateMeterId.Count == 0)
            {
                MessageBox.Show("你没有选择要删除的表", "提示", MessageBoxButton.YesNo, MessageBoxImage.Error);
                btn_update.IsEnabled = true;
                return;
            }
            this.UpdateProgress.Maximum = UpDateMeterId.Count;
            listBox_UpInfo.Items.Clear();
            UpDateInfomation upinfo = new UpDateInfomation();
            upinfo.Lis_PkId = UpDateMeterId;

            UpdateThread = new Thread(new ParameterizedThreadStart(DeleteToOracle));
            UpdateThread.Start(upinfo);
        }
        public void DeleteToOracle(object o)
        {
            UpDateInfomation Lis_Id = o as UpDateInfomation;
            DeleteInfoThread(Lis_Id.Lis_PkId.Count, Lis_Id.Lis_PkId);
        }

        public void DeleteToALLOracle(object o)
        {
            DeleteInfoThread();
        }
        public void DeleteInfoThread(double countItem, List<string> lis_UP_ID)
        {

            double i = 0;
            int sleepTime = 150; ;
            double t;
            List<string> SplitList = new List<string>();
            List<string> MeterUp_info = new List<string>();
            List<string> MeterUp_Up = new List<string>();
            List<string> Seal_info = new List<string>();
            List<string> Demand_info = new List<string>();
            Mis_Interface_Driver.MisDriver cs_Function = null;
            switch (csPublicMember.strSoftType)
            {
                case "CL3000G":
                case "CL3000F":
                case "CL3000DV80":
                    cs_Function = new SoftType_G.csFunction();
                    break;
                case "CL3000S":
                    cs_Function = new SoftType_S.csFunction();
                    break;
                case "CL3220NW":
                    cs_Function = new SoftType_3220.csFunction();
                    break;

            }
            // SoftType_G.csFunction cs_G_Function = new SoftType_G.csFunction();

            foreach (MeterBaseInfoFactor temp in ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo)
            {
                if (temp.BolIfup == true)
                {
                    t = i + 1;
                    i = t < countItem ? t : countItem;
                    MeterUp_info.Clear();

                    ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[Convert.ToInt16(temp.Int_ItemsNum)].CHR_UPLOAD_FLAG = "未上传";
                    MeterUp_info.Add(cs_Function.DeleteMis(temp.AVR_ASSET_NO.Trim()));
                    MeterUp_Up.Add(temp.PK_LNG_METER_ID);
                    foreach (string temp_id in MeterUp_info)
                    {
                        SplitList = TransStr(temp_id);
                        foreach (string split in SplitList)
                        {
                            listBox_UpInfo.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string, double>(UpDateMeter), split, i);
                            Thread.Sleep(sleepTime);
                        }

                        //listBox_UpInfo.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string, double>(UpDateMeter), temp_id, i);
                        //Thread.Sleep(sleepTime);
                    }

                }



            }

            MessageBox.Show("成功删除 :" + countItem + "个表");
            try
            {
                UpdateThread.Abort();
            }
            catch (Exception e)
            { }
            finally
            {
                if (listBox_UpInfo.Items.Count != 0)
                {
                    this.listBox_UpInfo.Dispatcher.Invoke(new Action(() =>
                    {
                        SetMeterUpdateFlag(MeterUp_Up, cmb_CheckTime.Text.ToString(), 0);
                        this.listBox_UpInfo.UpdateLayout();

                        this.listBox_UpInfo.ScrollIntoView(listBox_UpInfo.Items[listBox_UpInfo.Items.Count - 1]);
                    }));
                }


            }


        }
        public void DeleteInfoThread()
        {

            double i = 0;
            int sleepTime = 150; ;
            double t;
            List<string> SplitList = new List<string>();
            List<string> MeterUp_info = new List<string>();
            List<string> MeterUp_Up = new List<string>();
            List<string> Seal_info = new List<string>();
            List<string> Demand_info = new List<string>();
            Mis_Interface_Driver.MisDriver cs_Function = null;
            switch (csPublicMember.strSoftType)
            {
                case "CL3000G":
                case "CL3000F":
                case "CL3000DV80":
                    cs_Function = new SoftType_G.csFunction();
                    break;
                case "CL3000S":
                    cs_Function = new SoftType_S.csFunction();
                    break;

            }
            // SoftType_G.csFunction cs_G_Function = new SoftType_G.csFunction();

            MeterUp_info.Add(cs_Function.DeleteAllMis());
            foreach (string temp_id in MeterUp_info)
            {
                SplitList = TransStr(temp_id);
                foreach (string split in SplitList)
                {
                    listBox_UpInfo.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string, double>(UpDateMeter), split, i);
                    Thread.Sleep(sleepTime);
                }

                //listBox_UpInfo.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string, double>(UpDateMeter), temp_id, i);
                //Thread.Sleep(sleepTime);
            }
            foreach (MeterBaseInfoFactor temp in ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo)
            {

                t = i + 1;

                MeterUp_info.Clear();

                ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[Convert.ToInt16(temp.LNG_BENCH_POINT_NO) - 1].CHR_UPLOAD_FLAG = "未上传";

                MeterUp_Up.Add(temp.PK_LNG_METER_ID);

            }

            try
            {
                UpdateThread.Abort();
            }
            catch (Exception e)
            { }
            finally
            {
                if (listBox_UpInfo.Items.Count != 0)
                {
                    this.listBox_UpInfo.Dispatcher.Invoke(new Action(() =>
                    {
                        SetMeterUpdateFlag(MeterUp_Up, cmb_CheckTime.Text.ToString(), 0);
                        this.listBox_UpInfo.UpdateLayout();

                        this.listBox_UpInfo.ScrollIntoView(listBox_UpInfo.Items[listBox_UpInfo.Items.Count - 1]);
                    }));
                }


            }


        }
        private void DeleteMeter(string Meter_update_info, double progressCount)
        {



            SoftType_G.csFunction cs_G_Function = new SoftType_G.csFunction();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            listBox_UpInfo.Items.Add(Meter_update_info);


            listBox_UpInfo.UpdateLayout();
            listBox_UpInfo.ScrollIntoView(listBox_UpInfo.Items[listBox_UpInfo.Items.Count - 1]);

            this.UpdateProgress.Value = progressCount;
            // listBox_UpInfo.UpdateLayout();
            watch.Stop();
            // listBox_UpInfo.Items.Add("使用时间为：" + watch.ElapsedMilliseconds.ToString() + "毫秒");



        }
        private void btn_MisConfig_Click(object sender, RoutedEventArgs e)
        {
            OracleConfig oracleConfig = new OracleConfig();
            oracleConfig.ShowDialog();
        }

        private void cmb_CheckTime_Loaded(object sender, RoutedEventArgs e)
        {
           

        }
        private void ReLoadCheckTime()
        {
            cmb_CheckTime.Items.Clear();
            string strSection = "NewUser/CloumMIS/Item";
            string datapath = OperateData.FunctionXml.ReadElement(strSection, "Name", "txt_DataPath", "Value", "", BaseConfigPath);
            csPublicMember.str_DataPath = datapath;
            csPublicMember.strSoftType = OperateData.FunctionXml.ReadElement(strSection, "Name", "cmb_SoftType", "Value", "", BaseConfigPath);
            csPublicMember.showInfo_less = (bool)chk_ShowLess.IsChecked;
            #region 软件类型判断
            switch (csPublicMember.strSoftType)
            {
                case "CL3000G":
                case "CL3000F":
                case "CL3000DV80":
                    csPublicMember.strCondition = "datJdrq";
                    csPublicMember.strTableName = "meterinfo";
                    csPublicMember.strBino = "intBno";
                    break;
                case "CL3000S":
                    csPublicMember.strCondition = "DTM_TEST_DATE";
                    csPublicMember.strTableName = "METER_INFO";
                    csPublicMember.strBino = "LNG_BENCH_POINT_NO";
                    break;
                case "CL3220NW":
                    csPublicMember.strCondition = "DTE_TESTING_DATE";
                    csPublicMember.strTableName = "CTD_TERMINAL_INFO";
                    csPublicMember.strBino = "INT_TABLE_NO";
                    break;

            }

            #endregion


            LoadCheckTime(csPublicMember.str_DataPath, csPublicMember.strCondition, csPublicMember.strTableName, csPublicMember.showInfo_less);
            
        }
        private void ReLoadCheckTime(ComboBox cmb)
        {
            cmb.Items.Clear();
            string strSection = "NewUser/CloumMIS/Item";
            string datapath = OperateData.FunctionXml.ReadElement(strSection, "Name", "txt_DataPath", "Value", "", BaseConfigPath);
            csPublicMember.str_DataPath = datapath;
            csPublicMember.strSoftType = OperateData.FunctionXml.ReadElement(strSection, "Name", "cmb_SoftType", "Value", "", BaseConfigPath);
            csPublicMember.showInfo_less = (bool)chk_ShowLess.IsChecked;
            #region 软件类型判断
            switch (csPublicMember.strSoftType)
            {
                case "CL3000G":
                case "CL3000F":
                case "CL3000DV80":
                    csPublicMember.strCondition = "datJdrq";
                    csPublicMember.strTableName = "meterinfo";
                    csPublicMember.strBino = "intBno";
                    break;
                case "CL3000S":
                    csPublicMember.strCondition = "DTM_TEST_DATE";
                    csPublicMember.strTableName = "METER_INFO";
                    csPublicMember.strBino = "LNG_BENCH_POINT_NO";
                    break;

            }

            #endregion


            LoadCheckTime(csPublicMember.str_DataPath, csPublicMember.strCondition, csPublicMember.strTableName, csPublicMember.showInfo_less, cmb);

        }
        /// <summary>
        /// 加载检定日期
        /// </summary>
        /// <param name="dataPath"></param>
        /// <param name="Condition"></param>
        /// <param name="TableName"></param>
        private void LoadCheckTime(string dataPath, string Condition, string TableName, bool IsLess)
        {
            List<string> TimeList = new List<string>();

            OperateData.PublicFunction PbFunction = new OperateData.PublicFunction();
            string Less_SQL = IsLess == true ? " DISTINCT TOP 20" : " DISTINCT";
            string Sql = "";
            #region 厂家选择
            switch (OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "Cmb_Facory", "Value", "", BaseConfigPath))
            {
                case "科陆电子":
                    Sql = string.Format("select  {3} {0} from {1} order by {2} desc", Condition, TableName, Condition, Less_SQL);
                    break;
                case "格宁":
                    Sql = string.Format("select  {3} {0} from {1} order by {2} desc", "TESTDATE", "ResultData", "TESTDATE", Less_SQL);
                    Condition = "TESTDATE";
                    break;
                case "涵普":
                    Sql = string.Format("SELECT {0} FinishTime FROM [Meters].[dbo].[PM_Meters] order by FinishTime desc", Less_SQL);
                    break;
            }

            #endregion
            if (dataPath.Trim() == "")
            {
                return;
            }
            else
            {
                if (OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "Cmb_Facory", "Value", "", BaseConfigPath) == "涵普")
                {
                    TimeList = PbFunction.ExcuteSqlServer(Sql, "FinishTime");
                }
                else
                {
                    TimeList = PbFunction.ExcuteAccess(Sql, Condition);
                }

            }
            try
            {
                cmb_CheckTime.Items.Clear();
                cmb_ConditionValue_2.Items.Clear();
                if (!(TimeList.Count > 0))
                {
                    MessageBox.Show("请配置数据库的正确路径（或当前数据库没有数据）！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    BasePage basepage = new BasePage();
                    basepage.ShowDialog();
                    if (OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "Cmb_Facory", "Value", "", BaseConfigPath) == "涵普") return;
                    //ReLoadCheckTime();
                }
                foreach (string temp in TimeList)
                {
                    cmb_CheckTime.Items.Add(temp);
                    if (cmb_Condition_2.SelectedIndex == 0)
                    {
                        cmb_ConditionValue_2.Items.Add(temp);
                    }
                    cmb_local_Input.Items.Add(temp);
                }
            }
            catch (Exception exAddCheckTime)
            {

            }
        }

        private void LoadCheckTime(string dataPath, string Condition, string TableName, bool IsLess,ComboBox cmb)
        {
            List<string> TimeList = new List<string>();

            OperateData.PublicFunction PbFunction = new OperateData.PublicFunction();
            string Less_SQL = IsLess == true ? " DISTINCT TOP 20" : " DISTINCT";
            string Sql = "";
            #region 厂家选择
            switch (OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "Cmb_Facory", "Value", "", BaseConfigPath))
            {
                case "科陆电子":
                    Sql = string.Format("select  {3} {0} from {1} order by {2} desc", Condition, TableName, Condition, Less_SQL);
                    break;
                case "格宁":
                    Sql = string.Format("select  {3} {0} from {1} order by {2} desc", "TESTDATE", "ResultData", "TESTDATE", Less_SQL);
                    Condition = "TESTDATE";
                    break;
                case "涵普":
                    Sql = string.Format("SELECT {0} FinishTime FROM [Meters].[dbo].[PM_Meters] order by FinishTime desc", Less_SQL);
                    break;
            }

            #endregion
            if (dataPath.Trim() == "")
            {
                return;
            }
            else
            {
                if (OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "Cmb_Facory", "Value", "", BaseConfigPath) == "涵普")
                {
                    TimeList = PbFunction.ExcuteSqlServer(Sql, "FinishTime");
                }
                else
                {
                    TimeList = PbFunction.ExcuteAccess(Sql, Condition);
                }

            }
            try
            {
                cmb.Items.Clear();
                
                if (!(TimeList.Count > 0))
                {
                    MessageBox.Show("请配置数据库的正确路径（或当前数据库没有数据）！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    BasePage basepage = new BasePage();
                    basepage.ShowDialog();
                    if (OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "Cmb_Facory", "Value", "", BaseConfigPath) == "涵普") return;
                    //ReLoadCheckTime();
                }
                foreach (string temp in TimeList)
                {
                    cmb.Items.Add(temp);
                    
                }
            }
            catch (Exception exAddCheckTime)
            {

            }
        }
        private void chk_ShowLess_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_Condition.SelectedIndex == 0)
            {
                csPublicMember.showInfo_less = (bool)chk_ShowLess.IsChecked;

                LoadCheckTime(csPublicMember.str_DataPath, csPublicMember.strCondition, csPublicMember.strTableName, csPublicMember.showInfo_less);

            }

        }

        private void cmb_CheckTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_Condition.SelectedIndex == 0&& cmb_Condition_2.SelectedIndex<0)
            {
                if (cmb_CheckTime.SelectedValue == null) return;
                string CheckTime = cmb_CheckTime.SelectedValue.ToString();
                string Sql = string.Format("Select  * from {0} where {1} {4}#{2}# order by  {3} asc", csPublicMember.strTableName, csPublicMember.strCondition, CheckTime, csPublicMember.strBino, cmb_TimeCondition.SelectedValue.ToString());


                #region 厂家选择
                switch (OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "Cmb_Facory", "Value", "", BaseConfigPath))
                {
                    case "科陆电子":
                        Sql = string.Format("Select  * from {0} where {1} {4}#{2}# order by  {3} asc", csPublicMember.strTableName, csPublicMember.strCondition, CheckTime, csPublicMember.strBino, cmb_TimeCondition.SelectedValue.ToString());

                        break;
                    case "格宁":
                        Sql = string.Format("select * from {0} where {1} = '{2}' ", "ResultData", "TESTDATE", CheckTime);

                        break;
                    case "涵普":
                        Sql = string.Format("SELECT *  FROM [Meters].[dbo].[PM_Meters] WHERE FinishTime='{0}' ", CheckTime);
                        break;
                }

                #endregion
                try
                {
                    List<MeterBaseInfoFactor> tempBaseInfo = new List<MeterBaseInfoFactor>();
                    ObservableCollection<MeterBaseInfoFactor> baseInfo = new ObservableCollection<MeterBaseInfoFactor>();
                    OperateData.PublicFunction csFunction = new OperateData.PublicFunction();
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    baseInfo = csFunction.GetBaseInfo(Sql, csPublicMember.strSoftType);
                    watch.Stop();
                    ShowWord(watch.ElapsedMilliseconds.ToString(), "加载检定数据时间：");
                    ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo = baseInfo;

                    foreach (MeterBaseInfoFactor temp in ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo)
                    {
                        temp.BolIfup = true;
                    }
                    chk_SelectAll.IsChecked = true;
                }
                catch (Exception Ex)
                {
                }
            }
        }

        private void chk_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            Load_Checked();
            int MeterCount = ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count;
            int intSelectNum = 0;
            for (int i = 0; i < MeterCount; i++)
            {
                if (ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].BolIfup == true)
                {
                    intSelectNum++;
                }
            }
            ViewModel.AllMeterInfo.CreateInstance().SelectMeterNum = "当前选择了<<" + intSelectNum.ToString() + ">>个表";
        }

        private void Load_Checked()
        {
            if (chk_SelectAll.IsChecked == true)
            {
                int count = dg_Info.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].BolIfup = true;
                }
            }
            else
            {
                int count = dg_Info.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].BolIfup = false;
                }
            }
        }

        private void chk_Terminal_Click(object sender, RoutedEventArgs e)
        {


            if (chk_Terminal.IsChecked == true)
            {
                DataCore.Global.GB_Base.IsTerminal = true;
            }
            else
            {
                DataCore.Global.GB_Base.IsTerminal = false;
            }
            loadMisWorkNum();
        }
        List<T> FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            try
            {
                List<T> TList = new List<T> { };
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {
                        TList.Add((T)child);
                    }
                    else
                    {
                        List<T> childOfChildren = FindVisualChild<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                }
                return TList;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return null;
            }
        }
        private void btn_testFunction_Click(object sender, RoutedEventArgs e)
        {
            //listBox_UpInfo.UpdateLayout();
            //List<TextBox> txtList = FindVisualChild<TextBox>(this.dg_Info);
            //foreach (TextBox temp in txtList)
            //{
            //    temp.IsReadOnly = temp.IsReadOnly?false:true;
            //}
            DataCore.Global.GB_Base.CheckTimeCondition = cmb_TimeCondition.Text.Trim();
            SoftType_S.csFunction function = new SoftType_S.csFunction();
            List<string> ColName = new List<string>();
            ColName.Add("资产编号");
            ColName.Add("设备类别");
            ColName.Add("设备类型");
            ColName.Add("设备型号");
            ColName.Add("生产厂家");
            ColName.Add("起码");
            ColName.Add("检定日期");
            ColName.Add("检定结论");
            ColName.Add("区县局领料人");
            ColName.Add("区县局领料时间");
            ColName.Add("使用单位");
            ColName.Add("是否已使用");
            ObservableCollection<Clou_Report.Model.MemberForZJ> ZJMember = new ObservableCollection<Clou_Report.Model.MemberForZJ>();
            List<string> outputId = new List<string>();
            foreach (MeterBaseInfoFactor temp in ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo)
            {
                if (temp.BolIfup == true)
                {
                    outputId.Add(temp.PK_LNG_METER_ID);
                }
            }
            if (outputId.Count == 0) { MessageBox.Show("请选择要导出的表！", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }
            ZJMember = function.SetMemberForZj(cmb_CheckTime.Text, true, outputId);
            Clou_Report.Report_Excel ExcelforZj = new Clou_Report.Report_Excel();
            ExcelforZj.OutputExcelForZj(ZJMember, ColName);
            MessageBox.Show("导出物资成功", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        }
        /// <summary>
        /// 导出物资信息
        /// </summary>
        private void PrintTheWZ()
        {
            try
            {
                SoftType_S.csFunction function = new SoftType_S.csFunction();
                List<string> ColName = new List<string>();
                ColName.Add("资产编号");
                ColName.Add("设备类别");
                ColName.Add("设备类型");
                ColName.Add("设备型号");
                ColName.Add("生产厂家");
                ColName.Add("起码");
                ColName.Add("检定日期");
                ColName.Add("检定结论");
                ColName.Add("区县局领料人");
                ColName.Add("区县局领料时间");
                ColName.Add("使用单位");
                ColName.Add("是否已使用");
                ObservableCollection<Clou_Report.Model.MemberForZJ> ZJMember = new ObservableCollection<Clou_Report.Model.MemberForZJ>();
                ZJMember = function.SetMemberForZj(cmb_CheckTime.Text, true);
                Clou_Report.Report_Excel ExcelforZj = new Clou_Report.Report_Excel();
                ExcelforZj.OutputExcelForZj(ZJMember, ColName);
                //MessageBox.Show("导出物资成功", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception WZEX)
            {

            }

        }

        private void cmb_WorkNumList_Loaded(object sender, RoutedEventArgs e)
        {
            loadMisWorkNum();
            //chk_SelectAll.IsChecked = true;
        }
        private void loadMisWorkNum()
        {
            string GetWorkSQL = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "WorkNumLoad", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
            if (chk_Terminal.IsChecked == true)
            {
                GetWorkSQL = string.Format(GetWorkSQL, "VT_SB_JKZDJCGZD");
            }
            else
            {
                GetWorkSQL = string.Format(GetWorkSQL, "vt_sb_jkdnbjdgzd");
            }
            List<string> WorkNumList = new List<string>();
            OperateData.PublicFunction csfunction = new OperateData.PublicFunction();
            //#region 获取Oracle 工作单
            ////            Thread GetWorkNum;
            ////            GetWorkNum = new Thread(ParameterizedThreadStart(csfunction.GetSingleOracleData));

            ////#endregion
            csfunction.GetSingleOracleData(GetWorkSQL, "GZDBH", out WorkNumList);
            cmb_WorkNumList.Items.Clear();
            if (!(WorkNumList.Count > 0))
            {
                return;
            }
            foreach (string temp in WorkNumList)
            {
                cmb_WorkNumList.Items.Add(temp);
            }
        }
        private void cmb_Condition_Loaded(object sender, RoutedEventArgs e)
        {
            cmb_Condition.Items.Add("检定时间:");
            cmb_Condition.Items.Add("资产编号:");
            cmb_Condition.SelectedIndex = 0;
            cmb_Condition_2.Items.Add("检定时间:");
            cmb_Condition_2.Items.Add("资产编号:");
           

            
            cmb_TimeCondition.Items.Add("=");
            cmb_TimeCondition.Items.Add(">=");
            cmb_TimeCondition.Items.Add("<=");
            cmb_TimeCondition.SelectedIndex = 0;
            cmb_TimeCondition_2.Items.Add("=");
            cmb_TimeCondition_2.Items.Add(">=");
            cmb_TimeCondition_2.Items.Add("<=");
           
            chk_SelectAll.IsChecked = true;
            Load_Checked();
        }

        private void cmb_Condition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            switch (cmb_Condition.SelectedIndex)
            {
                case 0:
                    ReLoadCheckTime();
                    if (cmb_CheckTime.Items.Count > 0)
                    {
                        cmb_TimeCondition.SelectedValue = "=";
                        cmb_CheckTime.SelectedIndex = 0;
                    }
                    break;
                case 1:
                    cmb_CheckTime.Items.Clear();
                    break;
                default:
                    break;
            }
        }

        private void btn_FindMeter_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_Condition_2.SelectedIndex >= 0 && cmb_Condition.SelectedIndex >=0 && cmb_TimeCondition.SelectedIndex >= 0 && cmb_TimeCondition_2.SelectedIndex >= 0)
            {

                #region 软件类型判断
                switch (csPublicMember.strSoftType)
                {
                    case "CL3000G":
                    case "CL3000F":
                    case "CL3000DV80":
                        csPublicMember.strCondition = "chrjlbh";
                        csPublicMember.strTableName = "meterinfo";
                        break;
                    case "CL3000S":
                        csPublicMember.strCondition = "AVR_ASSET_NO";
                        csPublicMember.strTableName = "METER_INFO";
                        break;

                }

                #endregion
                try
                {
                    string strTj2 = cmb_Condition_2.Text.Trim(), strTj2Fh = cmb_TimeCondition_2.Text.Trim(), strTj2Value = cmb_ConditionValue_2.Text.Trim();
                    string strFH = cmb_TimeCondition.Text.Trim(),strTj1=cmb_Condition.Text.Trim();
                    strTj2 = GetDataColName(strTj2);
                    strTj1 = GetDataColName(strTj1);
                    strTj2Value = GetDataColValue(strTj2,strTj2Value);
                    string strTj1Value = GetDataColValue(strTj1, cmb_CheckTime.Text);
                    string Sql = string.Format("Select  * from {0} where {1} {3}{2} and {4} {5} {6}", csPublicMember.strTableName, strTj1, strTj1Value, strFH, strTj2, strTj2Fh, strTj2Value);
                    List<MeterBaseInfoFactor> tempBaseInfo = new List<MeterBaseInfoFactor>();
                    ObservableCollection<MeterBaseInfoFactor> baseInfo = new ObservableCollection<MeterBaseInfoFactor>();
                    OperateData.PublicFunction csFunction = new OperateData.PublicFunction();
                    baseInfo = csFunction.GetBaseInfo("", Sql, csPublicMember.strSoftType);

                    ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo = baseInfo;

                }
                catch (Exception Ex)
                {
                }
                return;
            }
            if (cmb_Condition.SelectedIndex == 1)
            {
                if (cmb_CheckTime.Text == "")
                {
                    MessageBox.Show("请输入你要查询的资产编号！");
                    return;
                }
                #region 软件类型判断
                switch (csPublicMember.strSoftType)
                {
                    case "CL3000G":
                    case "CL3000F":
                    case "CL3000DV80":
                        csPublicMember.strCondition = "chrjlbh";
                        csPublicMember.strTableName = "meterinfo";
                        break;
                    case "CL3000S":
                        csPublicMember.strCondition = "AVR_ASSET_NO";
                        csPublicMember.strTableName = "METER_INFO";
                        break;

                }

                #endregion
                try
                {

                    string Sql = string.Format("Select  * from {0} where {1} ='{2}'", csPublicMember.strTableName, csPublicMember.strCondition, cmb_CheckTime.Text);
                    List<MeterBaseInfoFactor> tempBaseInfo = new List<MeterBaseInfoFactor>();
                    ObservableCollection<MeterBaseInfoFactor> baseInfo = new ObservableCollection<MeterBaseInfoFactor>();
                    OperateData.PublicFunction csFunction = new OperateData.PublicFunction();
                    baseInfo = csFunction.GetBaseInfo("", Sql, csPublicMember.strSoftType);

                    ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo = baseInfo;

                }
                catch (Exception Ex)
                {
                }
            }
        }
        private string GetDataColName(string Name)
        {
            string ColName = "";
            switch (Name)
            {
                case "检定时间:":
                    ColName = "DTM_TEST_DATE";
                    break;
                case "资产编号:":
                    ColName = "AVR_ASSET_NO";
                    break;

            }
            return ColName;
        }
        private string GetDataColValue(string Name,string ColValue)
        {
            string ColName = "";
            switch (Name)
            {
                case "DTM_TEST_DATE":
                    ColName = @"#" + ColValue + @"#";
                    break;
                case "AVR_ASSET_NO":
                    ColName = "'" + ColValue + "'";
                    break;

            }
            return ColName;
        }
        #endregion

        #region 删除中间库
        private void btn_deldteAllMis_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_WorkNumList.Text.Trim() == "")
            {
                MessageBox.Show("请选择工单号！");
                return;
            }
            //OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", cmb_WorkNumList.Text.ToString(), BaseConfigPath);
            if (MessageBox.Show("请确定你要删除的工作单为：" + cmb_WorkNumList.Text, "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", cmb_WorkNumList.Text, BaseConfigPath);

            }
            else
            {
                return;
            }


            //  OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", "07522300987", BaseConfigPath);


            this.UpdateProgress.Maximum = 1;
            listBox_UpInfo.Items.Clear();
            UpDateInfomation upinfo = new UpDateInfomation();
            UpdateThread = new Thread(new ParameterizedThreadStart(DeleteAllToOracle));
            UpdateThread.Start(upinfo);
        }
        public void DeleteAllToOracle(object o)
        {
            UpDateInfomation Lis_Id = o as UpDateInfomation;
            DeleteAllInfoThread();
        }
        public void DeleteAllInfoThread()
        {

            double i = 0;
            int sleepTime = 150; ;
            double t;
            List<string> SplitList = new List<string>();
            List<string> MeterUp_info = new List<string>();
            List<string> Seal_info = new List<string>();
            List<string> Demand_info = new List<string>();
            Mis_Interface_Driver.MisDriver cs_Function = null;
            switch (csPublicMember.strSoftType)
            {
                case "CL3000G":
                case "CL3000F":
                case "CL3000DV80":
                    cs_Function = new SoftType_G.csFunction();
                    break;
                case "CL3000S":
                    cs_Function = new SoftType_S.csFunction();
                    break;
                case "CL3220NW":
                    cs_Function = new SoftType_3220.csFunction();
                    break;

            }
            // SoftType_G.csFunction cs_G_Function = new SoftType_G.csFunction();
            MeterUp_info.Add(cs_Function.DeleteAllMis());

            foreach (string temp_id in MeterUp_info)
            {
                SplitList = TransStr(temp_id);
                foreach (string split in SplitList)
                {
                    listBox_UpInfo.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string, double>(UpDateMeter), split, i);
                    Thread.Sleep(sleepTime);
                }
                //listBox_UpInfo.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string, double>(UpDateMeter), temp_id, 1);
                //Thread.Sleep(sleepTime);
            }
            foreach (MeterBaseInfoFactor temp in ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo)
            {

                t = i + 1;
                ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[Convert.ToInt16(temp.Int_ItemsNum)].CHR_UPLOAD_FLAG = "未上传";
                MeterUp_info.Clear();
            }

            MessageBox.Show("成功删除整张工作单");
            try
            {
                UpdateThread.Abort();
            }
            catch (Exception e)
            { }
            finally
            {
                if (listBox_UpInfo.Items.Count != 0)
                {
                    this.listBox_UpInfo.Dispatcher.Invoke(new Action(() =>
                    {

                        this.listBox_UpInfo.UpdateLayout();

                        this.listBox_UpInfo.ScrollIntoView(listBox_UpInfo.Items[listBox_UpInfo.Items.Count - 1]);
                    }));
                }


            }


        }

        private void DeleteAllMeter(string Meter_update_info, double progressCount)
        {



            SoftType_G.csFunction cs_G_Function = new SoftType_G.csFunction();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            listBox_UpInfo.Items.Add(Meter_update_info);


            listBox_UpInfo.UpdateLayout();
            listBox_UpInfo.ScrollIntoView(listBox_UpInfo.Items[listBox_UpInfo.Items.Count - 1]);

            this.UpdateProgress.Value = progressCount;
            // listBox_UpInfo.UpdateLayout();
            watch.Stop();
            // listBox_UpInfo.Items.Add("使用时间为：" + watch.ElapsedMilliseconds.ToString() + "毫秒");



        }
        #endregion

        #region Tab Excel
        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            #region 查询中间库的数据

            if (txt_MisZcbh.Text.Trim() == "")
            {
                MessageBox.Show("请输入资产编号！", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            showTheTable(txt_MisZcbh.Text.Trim());

            #endregion
        }

        public void showTheTable(string keyWord)
        {
            try
            {
                ViewMember.CreateInstance().IfRunTheItemChanged = 1;
                operateData operate = new operateData();
                ObservableCollection<MeterInfoItem> MeterList = new ObservableCollection<MeterInfoItem>();
                string ZcbhWord = keyWord;
                if (keyWord.Contains("ZP") || keyWord.Contains("ZF"))
                {
                    TableName = "VT_SB_JKZDJCJL";
                }
                else
                {
                    TableName = "VT_SB_JKDNBJDJL";
                }
                #region

                #endregion


                List<DataTableMember> tempTableMember = new List<DataTableMember>();
                string KeyWordType = "ZCBH";
                List<string> lis_TimeHead = new List<string>();
                List<string> lis_TimeEnd = new List<string>();
                List<string> lis_ReZcbh = new List<string>();
                int LineNum = 0;
                operate.GetTheTime(TableName, KeyWordType, ZcbhWord, "=", out tempTableMember);

                if (tempTableMember.Count == 0)
                {
                    MessageBox.Show("无法找到该表的信息！", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                for (int meterNum = 0; meterNum < 1; meterNum++)
                {
                    string TimeHead = (Convert.ToDateTime(tempTableMember[meterNum].StrJdrq).AddMinutes(-1d)).ToString("yyyy/MM/dd HH:mm:ss");
                    lis_TimeHead.Add(TimeHead);
                    ViewMember.CreateInstance().ThisMeterWorkNum = tempTableMember[meterNum].StrGZDBH.ToString();
                    string TimeEnd = (Convert.ToDateTime(tempTableMember[meterNum].StrJdrq).AddMinutes(1d)).ToString("yyyy/MM/dd HH:mm:ss");
                    lis_TimeEnd.Add(TimeEnd);
                    ViewMember.CreateInstance().CheckTime = tempTableMember[meterNum].StrJdrq;
                    ViewMember.CreateInstance().Operater = tempTableMember[meterNum].StrJyy;

                }
                # region 获取基本信息
                for (int TimeListNum = 0; TimeListNum < lis_TimeHead.Count; TimeListNum++)
                {
                    if (operate.ShowTheConditionTable(TableName, lis_TimeHead[TimeListNum], lis_TimeEnd[TimeListNum], ViewMember.CreateInstance().Operater, keyWord, out tempTableMember) == 0)
                    {
                        operate.ShowTheConditionTable(TableName, lis_TimeHead[TimeListNum], lis_TimeEnd[TimeListNum], ViewMember.CreateInstance().Operater, keyWord, out tempTableMember);

                        int currentMeterNum;
                        int currentIndex = 0;

                        for (int i = 0; i < tempTableMember.Count; i++)
                        {

                            if (tempTableMember[i].StrJlbh == ZcbhWord)
                            {
                                currentMeterNum = Convert.ToInt16(tempTableMember[i].IntMeterNum);
                                currentIndex = i;
                                break;
                            }
                        }
                        int FirstIndex = 0;
                        int LastIndex = currentIndex;
                        for (int i = currentIndex; i < tempTableMember.Count - 1; i++)
                        {
                            if (tempTableMember[i + 1].IntMeterNum < tempTableMember[i].IntMeterNum)
                            {
                                LastIndex = i;
                                break;
                            }
                            else
                            {
                                LastIndex = tempTableMember.Count - 1;
                            }
                        }
                        for (int i = currentIndex; i > 0; i--)
                        {
                            if (tempTableMember[i].IntMeterNum < tempTableMember[i - 1].IntMeterNum)
                            {
                                FirstIndex = i;
                                break;
                            }
                        }


                        #region 代码转中文
                        for (int j = FirstIndex; j <= LastIndex; j++)
                        {
                            if (tempTableMember[j].StrJddw == "")
                            {
                                tempTableMember[j].StrJddw = "本局";
                            }
                            switch (tempTableMember[j].StrJyy)
                            {
                                case "8E0257BE35ED1489E0440018FE2DCF1D":
                                    tempTableMember[j].StrJyy = "陈佩茹";
                                    break;
                                case "acb7d25b68a24e8fbee725b0149e8a96":
                                    tempTableMember[j].StrJyy = "安琪儿";
                                    break;
                                case "7FD9789A0A2649D195CA49A441DE28EE":
                                    tempTableMember[j].StrJyy = "陈曦彤";
                                    break;
                                case "8E0257BE31911489E0440018FE2DCF1D":
                                    tempTableMember[j].StrJyy = "陈雯丽";
                                    break;
                                case "8E0257BE278F1489E0440018FE2DCF1D":
                                    tempTableMember[j].StrJyy = "郭兴林";
                                    break;
                                case "yx_hexiaojun":
                                    tempTableMember[j].StrJyy = "何晓军";
                                    break;
                                case "yx_jiangqinghong":
                                    tempTableMember[j].StrJyy = "江庆洪";
                                    break;
                                case "yx_laishengsi":
                                    tempTableMember[j].StrJyy = "赖盛斯";
                                    break;
                                case "yx_lijinxia":
                                    tempTableMember[j].StrJyy = "李金霞";
                                    break;
                                case "yx_limi":
                                    tempTableMember[j].StrJyy = "李密";
                                    break;
                                case "yx_lina":
                                    tempTableMember[j].StrJyy = "李娜";
                                    break;
                                case "8E0257BE2F6B1489E0440018FE2DCF1D":
                                    tempTableMember[j].StrJyy = "刘岚";
                                    break;
                                case "yx_wangshaosi":
                                    tempTableMember[j].StrJyy = "王少思";
                                    break;
                                case "yx_wangzhenglin":
                                    tempTableMember[j].StrJyy = "王正林";
                                    break;
                                case "9E79AE1983D84C0E83D3E0DC49977F38":
                                    tempTableMember[j].StrJyy = "王子慧";
                                    break;
                                case "yx_weichunyan":
                                    tempTableMember[j].StrJyy = "韦春艳";
                                    break;
                                case "09C368A980B041AEA98999BF9C6B80E0":
                                    tempTableMember[j].StrJyy = "杨立瑾";
                                    break;
                                case "yx_yangxuechong":
                                    tempTableMember[j].StrJyy = "杨薛冲";
                                    break;
                                case "yx_zhangjun":
                                    tempTableMember[j].StrJyy = "张军";
                                    break;
                                case "227EA885C6BF4F46918308254012ED1E":
                                    tempTableMember[j].StrJyy = "张小燕";
                                    break;
                                case "yx_zhangzhen":
                                    tempTableMember[j].StrJyy = "张珍";
                                    break;
                                case "BC0D3C2FC3F94746A730FEC04A71B898":
                                    tempTableMember[j].StrJyy = "朱玲";
                                    break;
                                case "8E0257BE2F091489E0440018FE2DCF1D":
                                    tempTableMember[j].StrJyy = "朱新涛";
                                    break;
                                case "yx_qinhaishan":
                                    tempTableMember[j].StrJyy = "覃海山";
                                    break;
                                default:
                                    break;

                            }
                            switch (tempTableMember[j].StrHyy)
                            {
                                case "8E0257BE35ED1489E0440018FE2DCF1D":
                                    tempTableMember[j].StrHyy = "陈佩茹";
                                    break;
                                case "acb7d25b68a24e8fbee725b0149e8a96":
                                    tempTableMember[j].StrHyy = "安琪儿";
                                    break;
                                case "7FD9789A0A2649D195CA49A441DE28EE":
                                    tempTableMember[j].StrHyy = "陈曦彤";
                                    break;
                                case "8E0257BE31911489E0440018FE2DCF1D":
                                    tempTableMember[j].StrHyy = "陈雯丽";
                                    break;
                                case "8E0257BE278F1489E0440018FE2DCF1D":
                                    tempTableMember[j].StrHyy = "郭兴林";
                                    break;
                                case "yx_hexiaojun":
                                    tempTableMember[j].StrHyy = "何晓军";
                                    break;
                                case "yx_jiangqinghong":
                                    tempTableMember[j].StrHyy = "江庆洪";
                                    break;
                                case "yx_laishengsi":
                                    tempTableMember[j].StrHyy = "赖盛斯";
                                    break;
                                case "yx_lijinxia":
                                    tempTableMember[j].StrHyy = "李金霞";
                                    break;
                                case "yx_limi":
                                    tempTableMember[j].StrHyy = "李密";
                                    break;
                                case "yx_lina":
                                    tempTableMember[j].StrHyy = "李娜";
                                    break;
                                case "8E0257BE2F6B1489E0440018FE2DCF1D":
                                    tempTableMember[j].StrHyy = "刘岚";
                                    break;
                                case "yx_wangshaosi":
                                    tempTableMember[j].StrHyy = "王少思";
                                    break;
                                case "yx_wangzhenglin":
                                    tempTableMember[j].StrHyy = "王正林";
                                    break;
                                case "9E79AE1983D84C0E83D3E0DC49977F38":
                                    tempTableMember[j].StrHyy = "王子慧";
                                    break;
                                case "yx_weichunyan":
                                    tempTableMember[j].StrHyy = "韦春艳";
                                    break;
                                case "09C368A980B041AEA98999BF9C6B80E0":
                                    tempTableMember[j].StrHyy = "杨立瑾";
                                    break;
                                case "yx_yangxuechong":
                                    tempTableMember[j].StrHyy = "杨薛冲";
                                    break;
                                case "yx_zhangjun":
                                    tempTableMember[j].StrHyy = "张军";
                                    break;
                                case "227EA885C6BF4F46918308254012ED1E":
                                    tempTableMember[j].StrHyy = "张小燕";
                                    break;
                                case "yx_zhangzhen":
                                    tempTableMember[j].StrHyy = "张珍";
                                    break;
                                case "BC0D3C2FC3F94746A730FEC04A71B898":
                                    tempTableMember[j].StrHyy = "朱玲";
                                    break;
                                case "8E0257BE2F091489E0440018FE2DCF1D":
                                    tempTableMember[j].StrHyy = "朱新涛";
                                    break;
                                case "yx_qinhaishan":
                                    tempTableMember[j].StrHyy = "覃海山";
                                    break;
                                default:
                                    break;

                            }

                        }
                        #endregion

                        for (int i = FirstIndex; i <= LastIndex; i++)
                        {
                            LineNum++;
                            lis_ReZcbh.Add(tempTableMember[i].StrJlbh.ToString());
                            MeterList.Add(new MeterInfoItem()
                            {
                                ID = LineNum,
                                StrBZZZZCBH = tempTableMember[i].StrBZZZZCBH,
                                StrJlbh = tempTableMember[i].StrJlbh,
                                StrJdjl = tempTableMember[i].StrJdjl,
                                StrGZDBH = tempTableMember[i].StrGZDBH,
                                StrWD = tempTableMember[i].StrWD,
                                StrSD = tempTableMember[i].StrSD,
                                IntMeterNum = tempTableMember[i].IntMeterNum,
                                StrJdrq = tempTableMember[i].StrJdrq,
                                StrJyy = tempTableMember[i].StrJyy,
                                StrJddw = tempTableMember[i].StrJddw,
                                StrHyy = tempTableMember[i].StrHyy,
                                StrTestType = tempTableMember[i].StrTestType,
                                StrBlx = tempTableMember[i].StrBlx,
                                StrBmc = tempTableMember[i].StrBmc,
                                StrUb = tempTableMember[i].StrUb,
                                StrIb = tempTableMember[i].StrIb,
                                StrMeterLevel = tempTableMember[i].StrMeterLevel,
                                StrManufacture = tempTableMember[i].StrManufacture,
                                StrWcResult = tempTableMember[i].StrWcResult,
                                StrShellSeal = tempTableMember[i].StrShellSeal,
                                StrCodeSeal = tempTableMember[i].StrCodeSeal,
                                StrYXRQ = (Convert.ToDateTime(tempTableMember[i].StrJdrq).AddYears(5)).ToString(),

                            });
                        }

                    }
                }
                #endregion

                #region Get One MetetInfo
                for (int ReZcbh = 0; ReZcbh < lis_ReZcbh.Count; ReZcbh++)
                {
                    operate.ShowTheConditionTable(TableName, lis_ReZcbh[ReZcbh], ViewMember.CreateInstance().Operater, keyWord, out tempTableMember);
                    for (int j = 0; j < tempTableMember.Count; j++)
                    {
                        LineNum++;
                        MeterList.Add(new MeterInfoItem()
                        {
                            ID = LineNum,
                            StrBZZZZCBH = tempTableMember[j].StrBZZZZCBH,
                            StrJlbh = tempTableMember[j].StrJlbh,
                            StrJdjl = tempTableMember[j].StrJdjl,
                            StrGZDBH = tempTableMember[j].StrGZDBH,
                            StrWD = tempTableMember[j].StrWD,
                            StrSD = tempTableMember[j].StrSD,
                            IntMeterNum = tempTableMember[j].IntMeterNum,
                            StrJdrq = tempTableMember[j].StrJdrq,
                            StrJyy = tempTableMember[j].StrJyy,
                            StrJddw = tempTableMember[j].StrJddw,
                            StrHyy = tempTableMember[j].StrHyy,
                            StrTestType = tempTableMember[j].StrTestType,
                            StrBlx = tempTableMember[j].StrBlx,
                            StrBmc = tempTableMember[j].StrBmc,
                            StrUb = tempTableMember[j].StrUb,
                            StrIb = tempTableMember[j].StrIb,
                            StrMeterLevel = tempTableMember[j].StrMeterLevel,
                            StrManufacture = tempTableMember[j].StrManufacture,
                            StrWcResult = tempTableMember[j].StrWcResult,
                            StrShellSeal = tempTableMember[j].StrShellSeal,
                            StrCodeSeal = tempTableMember[j].StrCodeSeal,
                            StrYXRQ = (Convert.ToDateTime(tempTableMember[j].StrJdrq).AddYears(5)).ToString(),

                        });

                    }

                }


                #endregion


                #region
                for (int i = 0; i < MeterList.Count; i++)
                {
                    List<DataTableMember> temp = new List<DataTableMember>();
                    operate.ShowTheLockTable(MeterList[i].StrJlbh, MeterList[i].StrGZDBH, out temp);

                    if ((keyWord.Substring(0, 1) == "E" || keyWord.Substring(0, 1) == "F" || keyWord.Contains("ZP") || keyWord.Contains("ZF")))
                    {
                        if (temp.Count != 0)
                        {
                            MeterList[i].StrShellSeal = temp[0].StrFYZCBH;
                            MeterList[i].StrCodeSeal = " ";

                        }
                        else
                        {
                            MeterList[i].StrShellSeal = " ";
                            MeterList[i].StrCodeSeal = " ";
                        }
                    }
                    else
                    {
                        if (temp.Count > 0)
                        {
                            if (temp[0].StrJFWZDM == "07")
                            {
                                if (temp.Count == 1)
                                {
                                    MeterList[i].StrCodeSeal = temp[0].StrFYZCBH;
                                }
                                else
                                {
                                    MeterList[i].StrCodeSeal = temp[0].StrFYZCBH;
                                    MeterList[i].StrShellSeal = temp[1].StrFYZCBH;
                                }


                            }
                            else
                            {
                                if (temp.Count == 1)
                                {
                                    MeterList[i].StrCodeSeal = temp[0].StrFYZCBH;
                                }
                                else
                                {
                                    MeterList[i].StrCodeSeal = temp[0].StrFYZCBH;
                                    MeterList[i].StrShellSeal = temp[1].StrFYZCBH;
                                }


                            }
                        }

                    }

                }
                string blank = " ";
                for (int j = 0; j < MeterList.Count; j++)
                {
                    if (MeterList[j].StrJlbh.Length == 24)
                    {
                        string A = MeterList[j].StrJlbh.Substring(0, 4);
                        string B = MeterList[j].StrJlbh.Substring(4, 4);
                        string C = MeterList[j].StrJlbh.Substring(8, 4);
                        string D = MeterList[j].StrJlbh.Substring(12, 4);
                        string E = MeterList[j].StrJlbh.Substring(16, 4);
                        string F = MeterList[j].StrJlbh.Substring(20, 4);
                        MeterList[j].StrJlbh = A + blank + B + blank + C + blank + D + blank + E + blank + F;
                    }
                }
                for (int j = 0; j < MeterList.Count; j++)
                {
                    if (MeterList[j].StrShellSeal != null)
                    {
                        if (MeterList[j].StrShellSeal.Length == 24)
                        {
                            string A = MeterList[j].StrShellSeal.Substring(0, 4);
                            string B = MeterList[j].StrShellSeal.Substring(4, 4);
                            string C = MeterList[j].StrShellSeal.Substring(8, 4);
                            string D = MeterList[j].StrShellSeal.Substring(12, 4);
                            string E = MeterList[j].StrShellSeal.Substring(16, 4);
                            string F = MeterList[j].StrShellSeal.Substring(20, 4);
                            MeterList[j].StrShellSeal = A + blank + B + blank + C + blank + D + blank + E + blank + F;
                        }
                    }

                }
                for (int j = 0; j < MeterList.Count; j++)
                {
                    if (MeterList[j].StrCodeSeal != null)
                    {
                        if (MeterList[j].StrCodeSeal.Length == 24)
                        {
                            string A = MeterList[j].StrCodeSeal.Substring(0, 4);
                            string B = MeterList[j].StrCodeSeal.Substring(4, 4);
                            string C = MeterList[j].StrCodeSeal.Substring(8, 4);
                            string D = MeterList[j].StrCodeSeal.Substring(12, 4);
                            string E = MeterList[j].StrCodeSeal.Substring(16, 4);
                            string F = MeterList[j].StrCodeSeal.Substring(20, 4);
                            MeterList[j].StrCodeSeal = A + blank + B + blank + C + blank + D + blank + E + blank + F;
                        }
                    }

                }
                if (ViewMember.CreateInstance().MeterInfoList != null)
                    ViewMember.CreateInstance().MeterInfoList.Clear();
                ViewMember.CreateInstance().MeterInfoList = MeterList;

                #endregion




            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        #region BackGroudWorker
        BackgroundWorker BgExcel;
        public string MisMeter = "";
        public bool Flag = false;
        void BgExcel_DoWorker(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
                {
                    Loading.Visibility = System.Windows.Visibility.Visible;

                }
                ));
            while (!Flag)
            {
                System.Threading.Thread.Sleep(500);
            }

        }
        void BgExcel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Loading.Visibility = System.Windows.Visibility.Collapsed;
            this.Dispatcher.Invoke(new Action(() =>
            {
                Flag = true;
                MessageBox.Show("成功导出");
            }
                ));

        }
        public void OutPutAllInfoToExcel(object temp)
        {
            MisInfo str_meterzcbh = temp as MisInfo;
            OperateOracle.csFunctionOracle function = new OperateOracle.csFunctionOracle();
            string Message = function.OutPutAllInfoToExcel(str_meterzcbh.MeterZCBH);
            Flag = DataCore.Global.GB_Base.ReportSuccess;
        }
        #endregion
        private void btn_OutPutExcel_Click(object sender, RoutedEventArgs e)
        {


            if (txt_MisZcbh.Text == "" || txt_MisZcbh.Text == null)
            {
                MessageBox.Show("资产编号为空，请输入", "提示");
                return;
            }

            MisMeter = txt_MisZcbh.Text.Trim();
            Flag = false;
            Thread PutExcel;
            MisInfo misinfo = new MisInfo();
            misinfo.MeterZCBH = MisMeter;
            PutExcel = new Thread(new ParameterizedThreadStart(OutPutAllInfoToExcel));
            PutExcel.IsBackground = true;
            PutExcel.Start(misinfo);
            BgExcel = new BackgroundWorker();
            BgExcel.DoWork += new DoWorkEventHandler(BgExcel_DoWorker);
            BgExcel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgExcel_RunWorkerCompleted);
            BgExcel.RunWorkerAsync();


            //Message = ""; 
            //MessageBox.Show(Message);

            // MessageBox.Show(Message);

        }

        #endregion

        private void Grid_showItem_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            operateData operate = new operateData();



            if (ViewMember.CreateInstance().IfRunTheItemChanged != 1)
            {

                int count = Grid_showItem.SelectedIndex;
                string OnlyId_Meter = ViewMember.CreateInstance().MeterInfoList[count].StrJlbh;
                ViewMember.CreateInstance().ThisMeterWorkNum = ViewMember.CreateInstance().MeterInfoList[count].StrGZDBH;
                OnlyId_Meter = OnlyId_Meter.Replace(" ", "");
                ViewMember.CreateInstance().MeterOnlyId = OnlyId_Meter;

                #region 基本信息
                List<DataTableMember> ErrorList = new List<DataTableMember>();
                operate.ShowTheBasicErrorTable(ViewMember.CreateInstance().MeterOnlyId, ViewMember.CreateInstance().ThisMeterWorkNum, out ErrorList);
                ObservableCollection<MeterInfoItem> MeterList = new ObservableCollection<MeterInfoItem>();
                for (int i = 0; i < ErrorList.Count; i++)
                {
                    if (ErrorList[i].StrXBDM == "02")
                    {
                        ErrorList[i].StrXBDM = "三相";
                    }
                    switch (ErrorList[i].StrGLFXDM)
                    {
                        case "1":
                            ErrorList[i].StrGLFXDM = "正向有功";
                            break;
                        case "2":
                            ErrorList[i].StrGLFXDM = "正向无功";
                            break;
                        case "3":
                            ErrorList[i].StrGLFXDM = "反向有功";
                            break;
                        case "4":
                            ErrorList[i].StrGLFXDM = "反向无功";
                            break;
                        default:

                            break;

                    }
                    switch (ErrorList[i].StrGLYSDM)
                    {
                        case "1":
                            ErrorList[i].StrGLYSDM = "0.5(L)";
                            break;
                        case "2":
                            ErrorList[i].StrGLYSDM = "1";
                            break;
                        case "3":
                            ErrorList[i].StrGLYSDM = "0.8(c)";
                            break;
                        default:

                            break;
                    }
                    switch (ErrorList[i].StrFZLXDM)
                    {
                        case "1":
                            ErrorList[i].StrFZLXDM = "平衡负载";
                            break;
                        case "2":
                            ErrorList[i].StrFZLXDM = "不平衡负载A相";
                            break;
                        case "3":
                            ErrorList[i].StrFZLXDM = "不平衡负载B相";
                            break;
                        case "4":
                            ErrorList[i].StrFZLXDM = "不平衡负载C相";
                            break;
                        default:

                            break;

                    }
                    switch (ErrorList[i].StrFYDM)
                    {
                        case "01":
                            ErrorList[i].StrFZLXDM = "合元";
                            break;
                        case "02":
                            ErrorList[i].StrFZLXDM = "A相";
                            break;
                        case "03":
                            ErrorList[i].StrFZLXDM = "B相";
                            break;
                        case "04":
                            ErrorList[i].StrFZLXDM = "C相";
                            break;
                        default:

                            break;

                    }
                    switch (ErrorList[i].StrFZDLDM)
                    {
                        case "01":
                            ErrorList[i].StrFZDLDM = "0.05Ib";
                            break;

                        case "02":
                            ErrorList[i].StrFZDLDM = "0.1Ib";
                            break;

                        case "05":
                            ErrorList[i].StrFZDLDM = "Ib";
                            break;

                        case "06":
                            ErrorList[i].StrFZDLDM = "Imax";
                            break;

                        case "07":
                            ErrorList[i].StrFZDLDM = "0.5Imax";
                            break;

                        case "11":
                            ErrorList[i].StrFZDLDM = "0.02Ib";
                            break;

                        case "12":
                            ErrorList[i].StrFZDLDM = "0.01Ib";
                            break;
                        default:
                            break;
                    }

                }
                foreach (DataTableMember temp in ErrorList)
                {
                    MeterList.Add(new MeterInfoItem()
                    {
                        ID = temp.ID,
                        StrGLFXDM = temp.StrGLFXDM,
                        StrGLYSDM = temp.StrGLYSDM,
                        StrFZDLDM = temp.StrFZDLDM,
                        StrXBDM = temp.StrXBDM,
                        StrFZLXDM = temp.StrFZLXDM,
                        StrFYDM = temp.StrFYDM,
                        StrWC1 = temp.StrWC1,
                        StrWC2 = temp.StrWC2,
                        StrWC3 = temp.StrWC3,
                        StrWC4 = temp.StrWC4,
                        StrWC5 = temp.StrWC5,
                        StrWCPJZ = temp.StrWCPJZ,
                        StrXYZ = temp.StrXYZ,
                        StrJLDM = temp.StrJLDM,
                        StrWCCZ = temp.StrWCCZ,
                        StrWCCZXYZ = temp.StrWCCZXYZ,
                        StrDQMB = temp.StrDQMB,

                    });
                }
                #endregion
                #region 日计时
                List<DataTableMember> DayErrorList = new List<DataTableMember>();
                operate.ShowTheDayCalcTable(ViewMember.CreateInstance().MeterOnlyId, ViewMember.CreateInstance().ThisMeterWorkNum, out DayErrorList);

                ObservableCollection<MeterInfoItem> DayList = new ObservableCollection<MeterInfoItem>();
                foreach (DataTableMember temp in DayErrorList)
                {
                    DayList.Add(new MeterInfoItem()
                    {
                        ID = temp.ID,
                        StrCSZ1 = temp.StrCSZ1,
                        StrCSZ2 = temp.StrCSZ2,
                        StrCSZ3 = temp.StrCSZ3,
                        StrCSZ4 = temp.StrCSZ4,
                        StrCSZ5 = temp.StrCSZ5,
                        StrPJZ = temp.StrPJZ,
                        StrRJSDQBM = temp.StrRJSDQBM,


                    });
                }
                #endregion
                #region 需量记录表
                List<DataTableMember> NeedRecordErrorList = new List<DataTableMember>();
                operate.ShowTheNeedRecordTable(ViewMember.CreateInstance().MeterOnlyId, ViewMember.CreateInstance().ThisMeterWorkNum, out NeedRecordErrorList);

                ObservableCollection<MeterInfoItem> NeedRecord_List = new ObservableCollection<MeterInfoItem>();
                for (int j = 0; j < NeedRecordErrorList.Count; j++)
                {
                    switch (NeedRecordErrorList[j].StrFZDLDM)
                    {
                        case "01":
                            NeedRecordErrorList[j].StrFZDLDM = "0.05Ib";
                            break;

                        case "02":
                            NeedRecordErrorList[j].StrFZDLDM = "0.1Ib";
                            break;

                        case "05":
                            NeedRecordErrorList[j].StrFZDLDM = "Ib";
                            break;

                        case "06":
                            NeedRecordErrorList[j].StrFZDLDM = "Imax";
                            break;

                        case "07":
                            NeedRecordErrorList[j].StrFZDLDM = "0.5Imax";
                            break;

                        case "11":
                            NeedRecordErrorList[j].StrFZDLDM = "0.02Ib";
                            break;

                        case "12":
                            NeedRecordErrorList[j].StrFZDLDM = "0.01Ib";
                            break;
                        default:
                            break;
                    }
                }
                if (NeedRecordErrorList != null)
                {
                    foreach (DataTableMember temp in NeedRecordErrorList)
                    {
                        NeedRecord_List.Add(new MeterInfoItem()
                        {
                            ID = temp.ID,
                            StrFZDLDM = temp.StrFZDLDM,
                            StrBZZDXL = temp.StrBZZDXL,
                            StrSJXL = temp.StrSJXL,
                            StrWCZ = temp.StrWCZ,
                            StrXLJLDM = temp.StrXLJLDM,
                            StrXLDQBM = temp.StrXLDQBM,


                        });
                    }
                }

                #endregion
                #region 时段投切
                List<DataTableMember> TimeTQErrorList = new List<DataTableMember>();
                operate.ShowTheTimeTQTable(ViewMember.CreateInstance().MeterOnlyId, ViewMember.CreateInstance().ThisMeterWorkNum, out TimeTQErrorList);

                ObservableCollection<MeterInfoItem> TiemTQ_List = new ObservableCollection<MeterInfoItem>();
                foreach (DataTableMember temp in TimeTQErrorList)
                {
                    TiemTQ_List.Add(new MeterInfoItem()
                    {
                        ID = temp.ID,
                        StrTime = temp.StrTime,
                        StrBZTQSJ = temp.StrBZTQSJ,
                        StrSJTQSJ = temp.StrSJTQSJ,
                        StrTQWC = temp.StrTQWC,
                        StrTQDQBM = temp.StrTQDQBM,


                    });
                }
                #endregion
                #region 电能表示数
                List<DataTableMember> DisPlayMeterList = new List<DataTableMember>();
                operate.ShowTheDiaplayTable(ViewMember.CreateInstance().MeterOnlyId, ViewMember.CreateInstance().ThisMeterWorkNum, out DisPlayMeterList);

                ObservableCollection<MeterInfoItem> DisPlayMeter_List = new ObservableCollection<MeterInfoItem>();
                for (int s = 0; s < DisPlayMeterList.Count; s++)
                {
                    switch (DisPlayMeterList[s].StrSSLXDM)
                    {
                        case "121":
                            DisPlayMeterList[s].StrSSLXDM = "正有功总";
                            break;

                        case "123":
                            DisPlayMeterList[s].StrSSLXDM = "正有功峰";
                            break;

                        case "124":
                            DisPlayMeterList[s].StrSSLXDM = "正有功平";
                            break;

                        case "125":
                            DisPlayMeterList[s].StrSSLXDM = "正有功谷";
                            break;

                        case "131":
                            DisPlayMeterList[s].StrSSLXDM = "正无功总";
                            break;

                        case "133":
                            DisPlayMeterList[s].StrSSLXDM = "正无功峰";
                            break;

                        case "134":
                            DisPlayMeterList[s].StrSSLXDM = "正无功谷";
                            break;

                        case "135":
                            DisPlayMeterList[s].StrSSLXDM = "正无功平";
                            break;

                        case "221":
                            DisPlayMeterList[s].StrSSLXDM = "反有功总";
                            break;

                        case "222":
                            DisPlayMeterList[s].StrSSLXDM = "反有功峰";
                            break;

                        case "223":
                            DisPlayMeterList[s].StrSSLXDM = "反有功平";
                            break;

                        case "224":
                            DisPlayMeterList[s].StrSSLXDM = "反有功谷";
                            break;

                        case "231":
                            DisPlayMeterList[s].StrSSLXDM = "反无功总";
                            break;

                        case "236":
                            DisPlayMeterList[s].StrSSLXDM = "反无功峰";
                            break;

                        case "237":
                            DisPlayMeterList[s].StrSSLXDM = "反无功谷";
                            break;

                        case "238":
                            DisPlayMeterList[s].StrSSLXDM = "反无功平";
                            break;
                        default:
                            break;

                    }
                }
                foreach (DataTableMember temp in DisPlayMeterList)
                {
                    DisPlayMeter_List.Add(new MeterInfoItem()
                    {
                        ID = temp.ID,
                        StrSSLXDM = temp.StrSSLXDM,
                        StrBSS = temp.StrBSS,
                        StrCBSJ = temp.StrCBSJ,
                        StrSSDQBM = temp.StrSSDQBM,



                    });
                }
                #endregion
                #region 电能表走字结论
                List<DataTableMember> RunWordList = new List<DataTableMember>();
                operate.ShowTheRunWordTable(ViewMember.CreateInstance().MeterOnlyId, ViewMember.CreateInstance().ThisMeterWorkNum, out RunWordList);

                ObservableCollection<MeterInfoItem> RunWord_List = new ObservableCollection<MeterInfoItem>();
                for (int zz = 0; zz < RunWordList.Count; zz++)
                {
                    switch (RunWordList[zz].StrZZSSLXDM)
                    {
                        case "121":
                            RunWordList[zz].StrZZSSLXDM = "正有功总";
                            break;

                        case "123":
                            RunWordList[zz].StrZZSSLXDM = "正有功峰";
                            break;

                        case "124":
                            RunWordList[zz].StrZZSSLXDM = "正有功平";
                            break;

                        case "125":
                            RunWordList[zz].StrZZSSLXDM = "正有功谷";
                            break;

                        case "131":
                            RunWordList[zz].StrZZSSLXDM = "正无功总";
                            break;

                        case "133":
                            RunWordList[zz].StrZZSSLXDM = "正无功峰";
                            break;

                        case "134":
                            RunWordList[zz].StrZZSSLXDM = "正无功谷";
                            break;

                        case "135":
                            RunWordList[zz].StrZZSSLXDM = "正无功平";
                            break;

                        case "221":
                            RunWordList[zz].StrZZSSLXDM = "反有功总";
                            break;

                        case "222":
                            RunWordList[zz].StrZZSSLXDM = "反有功峰";
                            break;

                        case "223":
                            RunWordList[zz].StrZZSSLXDM = "反有功平";
                            break;

                        case "224":
                            RunWordList[zz].StrZZSSLXDM = "反有功谷";
                            break;

                        case "231":
                            RunWordList[zz].StrZZSSLXDM = "反无功总";
                            break;

                        case "236":
                            RunWordList[zz].StrZZSSLXDM = "反无功峰";
                            break;

                        case "237":
                            RunWordList[zz].StrZZSSLXDM = "反无功谷";
                            break;

                        case "238":
                            RunWordList[zz].StrZZSSLXDM = "反无功平";
                            break;
                        default:
                            break;

                    }


                }
                foreach (DataTableMember temp in RunWordList)
                {
                    RunWord_List.Add(new MeterInfoItem()
                    {
                        ID = temp.ID,
                        StrZZSSLXDM = temp.StrZZSSLXDM,
                        StrBZQQSS = temp.StrBZQQSS,
                        StrBZQZSS = temp.StrBZQZSS,
                        StrQSS = temp.StrQSS,
                        StrZSS = temp.StrZSS,
                        StrZZWC = temp.StrZZWC,
                        StrZZDQBM = temp.StrZZDQBM,


                    });
                }
                #endregion

                ViewMember.CreateInstance().WcItemList = MeterList;
                labInfoCount1.Content = MeterList.Count.ToString();
                ViewMember.CreateInstance().DayJSWCList = DayList;
                labInfoCount2.Content = DayList.Count.ToString();
                ViewMember.CreateInstance().NeedRecordList = NeedRecord_List;
                labInfoCount3.Content = NeedRecord_List.Count.ToString();
                ViewMember.CreateInstance().TimeTQList = TiemTQ_List;
                labInfoCount4.Content = TiemTQ_List.Count.ToString();
                ViewMember.CreateInstance().DiaPlayList = DisPlayMeter_List;
                labInfoCount5.Content = DisPlayMeter_List.Count.ToString();
                ViewMember.CreateInstance().RunWordList = RunWord_List;
                labInfoCount6.Content = RunWord_List.Count.ToString();




            }
            ViewMember.CreateInstance().IfRunTheItemChanged = 0;

        }

        private void btn_ExcelSeal_Click(object sender, RoutedEventArgs e)
        {
            Thread PutExcel;
            List<string> ZCBH_list = new List<string>();
            List<string> Seal01_list = new List<string>();
            List<string> Seal02_list = new List<string>();
            List<string> Seal03_list = new List<string>();
            string CheckMan="", WorkNum;
            WorkNum = cmb_WorkNumList.Text;
            Clou_Report.LocalBaseInfo localBaseInfo = new Clou_Report.LocalBaseInfo();
            Clou_Report.Report_Excel report_Excel = new Clou_Report.Report_Excel();
            foreach (MeterBaseInfoFactor temp in ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo)
            {
                CheckMan = temp.AVR_TEST_PERSON.Trim();
                ZCBH_list.Add(temp.AVR_ASSET_NO.Trim());
                Seal01_list.Add(temp.AVR_SEAL_1.Trim());
                Seal02_list.Add(temp.AVR_SEAL_2.Trim());
                Seal03_list.Add(temp.AVR_SEAL_3.Trim());
            }
            localBaseInfo.List_MeterZcbh = ZCBH_list;
            localBaseInfo.List_Seal001 = Seal01_list;
            localBaseInfo.List_Seal002 = Seal02_list;
            localBaseInfo.List_Seal003 = Seal03_list;
            localBaseInfo.StrCheckMan = CheckMan;
            localBaseInfo.StrGZDBH = WorkNum;
            localBaseInfo.MeterCheck = cmb_CheckTime.Text.ToString();
            Flag = false;
            PutExcel = new Thread(new ParameterizedThreadStart(OutSeal));
            PutExcel.IsBackground = true;
            PutExcel.Start(localBaseInfo);
            BgExcel = new BackgroundWorker();
            BgExcel.DoWork += new DoWorkEventHandler(BgExcel_DoWorker);
            BgExcel.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgExcel_RunWorkerCompleted);
            BgExcel.RunWorkerAsync();
        }
        public void OutSeal(object temp)
        {
            Clou_Report.Report_Excel report_Excel = new Clou_Report.Report_Excel();
            Clou_Report.LocalBaseInfo LocalTemp = temp as Clou_Report.LocalBaseInfo;
            report_Excel.OutputExcel(LocalTemp.List_MeterZcbh, LocalTemp.List_Seal001, LocalTemp.List_Seal002, LocalTemp.List_Seal003, LocalTemp.MeterCheck, LocalTemp.StrGZDBH, LocalTemp.StrCheckMan);

            Flag = true;
        }

        private void btn_deldteMisAll_Click(object sender, RoutedEventArgs e)
        {
            if (cmb_WorkNumList.Text.Trim() == "")
            {
                MessageBox.Show("请选择工单号！");
                return;
            }
            //OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", cmb_WorkNumList.Text.ToString(), BaseConfigPath);
            if (MessageBox.Show("请确定你要删除的工作单为：" + cmb_WorkNumList.Text, "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", cmb_WorkNumList.Text, BaseConfigPath);
                DataCore.Global.GB_Base.MeterGZDBH = cmb_WorkNumList.Text;
            }
            else
            {
                return;
            }


            //  OperateData.FunctionXml.UpdateElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", "07522300987", BaseConfigPath);


            this.UpdateProgress.Maximum = 1;
            listBox_UpInfo.Items.Clear();
            UpDateInfomation upinfo = new UpDateInfomation();
            UpdateThread = new Thread(new ParameterizedThreadStart(DeleteAllToOracle));
            UpdateThread.Start(upinfo);
        }

        private void ShowWord(string Time, string RunItem)
        {
            this.listBox_UpInfo.Dispatcher.Invoke(new Action(() =>
            {
                // this.listBox_UpInfo.Items.Clear();
                this.listBox_UpInfo.Items.Add(RunItem + Time + "毫秒");
                this.listBox_UpInfo.UpdateLayout();


            }));
        }
        private void ShowWord(string RunItem)
        {
            this.listBox_UpInfo.Dispatcher.Invoke(new Action(() =>
            {
                // this.listBox_UpInfo.Items.Clear();
                this.listBox_UpInfo.Items.Add(RunItem);
                this.listBox_UpInfo.UpdateLayout();


            }));
        }

        private void chk_ShowLess_Checked(object sender, RoutedEventArgs e)
        {
            ReLoadCheckTime();
        }
        #region Tab 003 view local data
        private void btn_local_Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmb_local_condition.SelectedIndex == 1)
                {
                    string sql = "Select * from meter_info where AVR_ASSET_NO>='"+cmb_local_Input.Text.ToString().Trim()+"'";
                    ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                    DataGrid_BaseInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.DefaultView;
                }
                //string sql = "Select * from meter_info where DTM_TEST_DATE>=#2017/5/17 14:35:18#";
                // ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                DataGrid_BaseInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.DefaultView;
            }
            catch (Exception Local_Search_ex)
            {
            }

        }








        private void DataGrid_BaseInfo_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            
            if (DataGrid_BaseInfo.SelectedIndex == null || DataGrid_BaseInfo.SelectedIndex < 0) return;
            string pk_id = ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.Rows[DataGrid_BaseInfo.SelectedIndex][0].ToString();
            if (DataCore.Global.GB_Base.SoftType == "CL3000S")
            {
                #region 加载其他表
                string sql = "Select * from METER_RESULTS where FK_LNG_METER_ID='" + pk_id + "'";
                ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase = null;
                ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                DataGrid_ResultInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase.DefaultView;

                sql = "Select * from METER_COMMUNICATION where FK_LNG_METER_ID='" + pk_id + "'";
                ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase = null;
                ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                DataGrid_MultiInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase.DefaultView;

                sql = "Select * from METER_ERROR where FK_LNG_METER_ID='" + pk_id + "'";
                ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase = null;
                ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                DataGrid_ErrorInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase.DefaultView;

                sql = "Select * from METER_START_NO_LOAD where FK_LNG_METER_ID='" + pk_id + "'";
                ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase = null;
                ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                DataGrid_QiDiInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase.DefaultView;

                sql = "Select * from METER_ENERGY_TEST_DATA where FK_LNG_METER_ID='" + pk_id + "'";
                ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase = null;
                ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                DataGrid_RunInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase.DefaultView;

                #endregion
            }
            else
            {
                #region 加载其他表
                string sql = "Select * from METER_RESULTS where FK_LNG_METER_ID=" + pk_id + "";
                //ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase = null;
                //ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                //DataGrid_ResultInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase.DefaultView;

                sql = "Select * from MeterDgn where intMyId=" + pk_id + "";
                ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase = null;
                ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                DataGrid_MultiInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase.DefaultView;

                sql = "Select * from MeterError where intMyId=" + pk_id + "";
                ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase = null;
                ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                DataGrid_ErrorInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase.DefaultView;

                sql = "Select * from MeterQdQid where intMyId=" + pk_id + "";
                ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase = null;
                ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                DataGrid_QiDiInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase.DefaultView;

                sql = "Select * from MeterZzData where intMyId=" + pk_id + "";
                ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase = null;
                ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                DataGrid_RunInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase.DefaultView;

                #endregion
            }
         
        }

        private void btn_local_Test_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show(ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.Rows[2][4].ToString().Trim());
        }

        private void btn_local_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid_BaseInfo.IsReadOnly == false)
            {
                DataGrid_BaseInfo.IsReadOnly = true;
                DataGrid_ResultInfo.IsReadOnly = true;
                DataGrid_MultiInfo.IsReadOnly = true;
                DataGrid_ErrorInfo.IsReadOnly = true;
                DataGrid_QiDiInfo.IsReadOnly = true;
                DataGrid_RunInfo.IsReadOnly = true;
                btn_local_Edit.Content = "点击编辑";
            }
            else
            {
                DataGrid_BaseInfo.IsReadOnly = false;
                DataGrid_ResultInfo.IsReadOnly = false;
                DataGrid_MultiInfo.IsReadOnly = false;
                DataGrid_ErrorInfo.IsReadOnly = false;
                DataGrid_QiDiInfo.IsReadOnly = false;
                DataGrid_RunInfo.IsReadOnly = false;
                btn_local_Edit.Content = "编辑状态..";
            }

        }

        private void DataGrid_BaseInfo_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string newValue = (e.EditingElement as TextBox).Text.ToString().Trim();
            var con_name = sender as DataGrid;
            string ConName = con_name.Name;
            string ChangeKey = "";
            string ChangeValue = "", reslutname = "";
            string SelectValue = ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.Rows[DataGrid_BaseInfo.SelectedIndex][0].ToString();
            string SelectKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.Columns[0].ColumnName; ;
            string Sql = "";
            if (DataCore.Global.GB_Base.SoftType == "CL3000S")
            {
                switch (ConName)
                {
                    case "DataGrid_BaseInfo":
                        ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                        ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                        Sql = string.Format("update METER_INFO set {0}='{1}' where {2}='{3}'", ChangeKey, ChangeValue, SelectKey, SelectValue);
                        break;
                    case "DataGrid_ResultInfo":
                        ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                        ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                        reslutname = ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase.Rows[con_name.SelectedIndex][4].ToString().Trim();
                        SelectKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase.Columns[0].ColumnName;
                        Sql = string.Format("update METER_RESULTS set {0}='{1}' where {2}='{3}' and AVR_RESULT_NAME='{4}' ", ChangeKey, ChangeValue, SelectKey, SelectValue, reslutname);
                        break;
                    case "DataGrid_MultiInfo":
                        ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                        ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                        reslutname = ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase.Rows[con_name.SelectedIndex][3].ToString().Trim();
                        SelectKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase.Columns[0].ColumnName;
                        Sql = string.Format("update METER_COMMUNICATION set {0}='{1}' where {2}='{3}' and AVR_PROJECT_NO='{4}'", ChangeKey, ChangeValue, SelectKey, SelectValue, reslutname);
                        break;
                    case "DataGrid_ErrorInfo":
                        ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                        ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                        reslutname = ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase.Rows[con_name.SelectedIndex][3].ToString().Trim();
                        SelectKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase.Columns[0].ColumnName;
                        Sql = string.Format("update METER_ERROR set {0}='{1}' where {2}='{3}' and AVR_PROJECT_NO='{4}' ", ChangeKey, ChangeValue, SelectKey, SelectValue, reslutname);
                        break;
                    case "DataGrid_QiDiInfo":
                        ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                        ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                        reslutname = ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase.Rows[con_name.SelectedIndex][3].ToString().Trim();
                        SelectKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase.Columns[0].ColumnName;
                        Sql = string.Format("update METER_START_NO_LOAD set {0}='{1}' where {2}='{3}' and AVR_PROJECT_NO='{4}' ", ChangeKey, ChangeValue, SelectKey, SelectValue, reslutname);
                        break;
                    case "DataGrid_RunInfo":
                        ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                        ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                        reslutname = ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase.Rows[con_name.SelectedIndex][3].ToString().Trim();
                        SelectKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase.Columns[0].ColumnName;
                        Sql = string.Format("update METER_ENERGY_TEST_DATA set {0}='{1}' where {2}='{3}' and AVR_PROJECT_NO='{4}'", ChangeKey, ChangeValue, SelectKey, SelectValue, reslutname);
                        break;
                }
            }
            else
            {
                switch (ConName)
                {
                    case "DataGrid_BaseInfo":
                        ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                        ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                        Sql = string.Format("update MeterInfo set {0}='{1}' where {2}={3}", ChangeKey, ChangeValue, SelectKey, SelectValue);
                        break;
                    //case "DataGrid_ResultInfo":
                    //    ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                    //    ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                    //    reslutname = ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase.Rows[con_name.SelectedIndex][4].ToString().Trim();
                    //    SelectKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().ResultDataBase.Columns[0].ColumnName;
                    //    Sql = string.Format("update METER_RESULTS set {0}='{1}' where {2}={3} and AVR_RESULT_NAME='{4}' ", ChangeKey, ChangeValue, SelectKey, SelectValue, reslutname);
                    //    break;
                    case "DataGrid_MultiInfo":
                        ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                        ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                        reslutname = ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase.Rows[con_name.SelectedIndex][3].ToString().Trim();
                        SelectKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().MultiDataBase.Columns[0].ColumnName;
                        Sql = string.Format("update MeterDgn set {0}='{1}' where {2}={3} and chrProjectNo='{4}'", ChangeKey, ChangeValue, SelectKey, SelectValue, reslutname);
                        break;
                    case "DataGrid_ErrorInfo":
                        ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                        ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                        reslutname = ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase.Rows[con_name.SelectedIndex][3].ToString().Trim();
                        SelectKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().ErrorDataBase.Columns[0].ColumnName;
                        Sql = string.Format("update MeterError set {0}='{1}' where {2}={3} and chrProjectNo='{4}' ", ChangeKey, ChangeValue, SelectKey, SelectValue, reslutname);
                        break;
                    case "DataGrid_QiDiInfo":
                        ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                        ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                        reslutname = ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase.Rows[con_name.SelectedIndex][3].ToString().Trim();
                        SelectKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().QiDongDataBase.Columns[0].ColumnName;
                        Sql = string.Format("update MeterQdQid set {0}='{1}' where {2}={3} and chrProjectNo='{4}' ", ChangeKey, ChangeValue, SelectKey, SelectValue, reslutname);
                        break;
                    case "DataGrid_RunInfo":
                        ChangeKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase.Columns[con_name.CurrentCell.Column.DisplayIndex].ColumnName;
                        ChangeValue = (e.EditingElement as TextBox).Text.ToString().Trim();
                        reslutname = ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase.Rows[con_name.SelectedIndex][3].ToString().Trim();
                        SelectKey = ViewLocalData.ClouModel.ClouMember.CreateInstance().RunDataBase.Columns[0].ColumnName;
                        Sql = string.Format("update MeterZzData set {0}='{1}' where {2}={3} and chrProjectNo='{4}'", ChangeKey, ChangeValue, SelectKey, SelectValue, reslutname);
                        break;
                }
            }
          
            // bool Result = true;
            bool Result = ViewLocalData.ViewData.ExceuteSql(Sql);
            MessageBox.Show((Result == true ? "修改成功！" + Sql : "修改失败！" + Sql));
        }

        private void Load_localPage_Conctrol()
        {
            cmb_local_condition.Items.Clear();
            cmb_local_condition.Items.Add("检定时间");
            cmb_local_condition.Items.Add("资产编号");
            //cmb_local_condition.SelectedIndex = 0;
            cmb_local_symbol.Items.Clear();
            cmb_local_symbol.Items.Add("=");
            cmb_local_symbol.Items.Add(">=");
            cmb_local_symbol.Items.Add("<=");
            //cmb_local_symbol.SelectedIndex = 0;
        }
        #endregion

        private void cmb_local_Input_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            csPublicMember.strSoftType = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "cmb_SoftType", "Value", "", BaseConfigPath);
            if (csPublicMember.strSoftType == "CL3000S")
            {
                if (cmb_local_condition.SelectedIndex == 0)
                {
                    if (cmb_local_Input.SelectedValue == null) return;
                    string sql = "Select * from meter_info where DTM_TEST_DATE=#" + cmb_local_Input.SelectedValue + "#";
                    ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase = null;
                    ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                    DataGrid_BaseInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.DefaultView;
                }
            }
            else
            {
                if (cmb_local_condition.SelectedIndex == 0)
                {
                    if (cmb_local_Input.SelectedValue == null) return;
                    string sql = "Select * from MeterInfo where datJdrq=#" + cmb_local_Input.SelectedValue + "#";
                    ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase = null;
                    ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase = ViewLocalData.ViewData.ViewDataBase(sql);
                    DataGrid_BaseInfo.ItemsSource = ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.DefaultView;
                }
               // MessageBox.Show("目前只有3000S软件才有这个功能，其他软件类型尚未完成");
            }

        }

        private void cmb_local_Input_Loaded(object sender, RoutedEventArgs e)
        {
            Load_localPage_Conctrol();
        }

        private void cmb_local_symbol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_local_symbol.SelectedIndex == 0)
            {

            }

        }

        private void cmb_local_condition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_local_condition.SelectedIndex == 0)
            {
                cmb_local_symbol.SelectedIndex = 0;
                cmb_local_condition.SelectedIndex = 0;
                cmb_local_Input.Items.Clear();
                string strSection = "NewUser/CloumMIS/Item";
                string datapath = OperateData.FunctionXml.ReadElement(strSection, "Name", "txt_DataPath", "Value", "", BaseConfigPath);
                csPublicMember.str_DataPath = datapath;
                csPublicMember.strSoftType = OperateData.FunctionXml.ReadElement(strSection, "Name", "cmb_SoftType", "Value", "", BaseConfigPath);
                csPublicMember.showInfo_less = (bool)chk_ShowLess.IsChecked;
                #region 软件类型判断
                switch (csPublicMember.strSoftType)
                {
                    case "CL3000G":
                    case "CL3000F":
                    case "CL3000DV80":
                        csPublicMember.strCondition = "datJdrq";
                        csPublicMember.strTableName = "meterinfo";
                        csPublicMember.strBino = "intBno";
                        break;
                    case "CL3000S":
                        csPublicMember.strCondition = "DTM_TEST_DATE";
                        csPublicMember.strTableName = "METER_INFO";
                        csPublicMember.strBino = "LNG_BENCH_POINT_NO";
                        break;

                }

                #endregion


                LoadCheckTime(csPublicMember.str_DataPath, csPublicMember.strCondition, csPublicMember.strTableName, csPublicMember.showInfo_less);
            }
            else
            {
                cmb_local_Input.Items.Clear();
                cmb_local_symbol.SelectedIndex = 0;
            }


        }

        public class UpDateInfomation
        {
            private List<string> lis_PkId;
            public List<string> Lis_PkId
            {
                get;
                set;
            }

        }

        public class MisInfo
        {
            private string meterZCBH;
            public string MeterZCBH
            { get; set; }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void rbtnFirst_True_Click(object sender, RoutedEventArgs e)
        {
            DataCore.Global.GB_Base.FirstCheckFlag = "1";
        }

        private void rbtnFirst_False_Click(object sender, RoutedEventArgs e)
        {
            DataCore.Global.GB_Base.FirstCheckFlag = "0";
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmb_Condition_2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combox = (ComboBox)sender as ComboBox;
            #region 判断是哪一组
            switch (combox.Name)
            {
                case "cmb_Condition_2":
                    combox = cmb_ConditionValue_2;
                    break;
            }
            #endregion
            switch (cmb_Condition.SelectedIndex)
            {
                case 0:
                    ReLoadCheckTime(combox);
                    if (cmb_CheckTime.Items.Count > 0)
                    {
                        cmb_TimeCondition.SelectedValue = "=";
                        cmb_CheckTime.SelectedIndex = 0;
                    }
                    break;
                case 1:
                    cmb_CheckTime.Items.Clear();
                    break;
                default:
                    break;
            }
        }

        private void chk_condition2_Click(object sender, RoutedEventArgs e)
        {
            if (chk_condition2.IsChecked == true)
            {
                cmb_Condition_2.IsEnabled = true;
                cmb_TimeCondition_2.IsEnabled = true;
                cmb_ConditionValue_2.IsEnabled = true;
               // MessageBox.Show("dagou");
            }
            else
            {
                cmb_Condition_2.IsEnabled = false;
                cmb_TimeCondition_2.IsEnabled = false;
                cmb_ConditionValue_2.IsEnabled = false;
                cmb_Condition_2.Text = "";
                cmb_TimeCondition_2.Text = "";
                cmb_ConditionValue_2.Text = "";
               // MessageBox.Show("budagou");
            }
        }

        private List<string> TransStr(string longStr)
        {
            List<string> ChangeItem = new List<string>();
            try
            {
                int byteLength = Encoding.Default.GetByteCount(longStr);
                int tempCout = 0, startIndex = 0, endIndex = 0,multiCount=0;
                ASCIIEncoding ascii = new ASCIIEncoding();
                byte[] str_byt = ascii.GetBytes(longStr);
                for (int i = 0; i < longStr.Length; i++)
                {
                    if ((int)str_byt[i] == 63)
                    {
                        tempCout = tempCout + 2;
                    }
                    else
                    {
                        tempCout++;
                    }
                    while (tempCout >= 24)
                    {
                        endIndex = i - startIndex + 1;
                        ChangeItem.Add(longStr.Substring(startIndex, endIndex));
                        tempCout = 0;
                        multiCount++;
                        startIndex = startIndex + endIndex;
                        
                    }
                    if (i == longStr.Length - 1 && tempCout < 24)
                    {
                        if (endIndex == 0)
                        { endIndex = i - startIndex + 1; }
                        else {
                            endIndex = i - startIndex+1 ;
                        }
                        ChangeItem.Add(longStr.Substring(startIndex, endIndex));
                    }
                }
            }
            catch (Exception ex)
            {
                ChangeItem.Clear();
                ChangeItem.Add(longStr);
            }
           
            
            return ChangeItem;
        }

        private void btn_Puthege_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //string str = "52B293C9S1A1543609752B293C9S1A1543609752B293C9S1A15436097";
                //List<string> resta = new List<string>();
                //resta=TransStr(str);
                //foreach (string remp in resta)
                //{
                //    listBox_UpInfo.Items.Add(remp);
                //}
                #region Print 合格证
                #region Determine if there is any data
                if (ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count == null || ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count < 0)
                {
                    MessageBox.Show("没有检定数据！");
                    return;
                }
                #endregion

                DataCore.Global.GB_Base.ExcelReportName = "HGZ_Sx.xls";
                int fileIndex = 1, cycleIndex = 0;
                Clou_Report.Report_Excel Print = new Clou_Report.Report_Excel();
                #region Get information on certificate
                List<string> lisZcbh = new List<string>();
                List<string> lisJjrq = new List<string>();

                foreach (MeterBaseInfoFactor temp in ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo)
                {
                    DataCore.Global.GB_Base.MeterCheckTime = temp.DTM_TEST_DATE.Trim();
                    DataCore.Global.GB_Base.MeterCheckName = temp.AVR_TEST_PERSON.Trim();
                    if (temp.BolIfup == true)
                    {
                        lisZcbh.Add(temp.AVR_ASSET_NO.Trim());
                        lisJjrq.Add(temp.DTM_VALID_DATE.Trim());
                    }
                    cycleIndex = cycleIndex + 1;
                    if (cycleIndex >= 24)
                    {
                        Print.PrintExcelCertificate(lisZcbh, lisJjrq, fileIndex.ToString());
                        fileIndex++;
                        lisZcbh.Clear();
                        lisJjrq.Clear();
                    }
                }
                if (lisZcbh.Count < 24)
                {
                    Print.PrintExcelCertificate(lisZcbh, lisJjrq, fileIndex.ToString());
                }
                if (DataCore.Global.GB_Base.ExcelResult == "success")
                {
                    MessageBox.Show("导出合格证成功");
                }
                else
                {
                    MessageBox.Show("导出合格证失败");
                }
                #endregion
                #endregion
                //List<string> selectId = new List<string>();
                //selectId = GetSelectMeter();

                //#region 导出重复性试验
                ////获取数据
                //ObservableCollection<DataCore.ReportModel.ST_Repetition> ST_errorCol = new ObservableCollection<DataCore.ReportModel.ST_Repetition>();
                //DataCore.ReportModel.ST_Repetition error_ST = new DataCore.ReportModel.ST_Repetition();
                //switch (DataCore.Global.GB_Base.SoftType)
                //{
                //    case "CL3000G":
                //    case "CL3000F":
                //    case "CL3000DV80":

                //        break;
                //    case "CL3000S":
                //        foreach (string temp in selectId)
                //        {
                //            error_ST = Clou_Report.Report_ST.Print_repetition.GetCL3000SData(temp);
                //            ST_errorCol.Add(error_ST);
                //        }
                //        break;

                //}
                //#endregion
                //bool result = Clou_Report.Report_ST.Print_repetition.Print(ST_errorCol);
                //if (result)
                //{
                //    MessageBox.Show("导出成功！");
                //    System.Diagnostics.Process.Start(DataCore.Global.GB_Base.SaveExcel);
                //}
                //else
                //{
                //    MessageBox.Show("导出失败！");
                //}
            }
            catch
            { 
            
            }
        }

        private List<string> GetSelectMeter()
        {
            List<string> lisMeterID = new List<string>();
            int MeterCount = ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count;
            int intSelectNum = 0;
            for (int i = 0; i < MeterCount; i++)
            {
                if (ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].BolIfup == true)
                {
                    lisMeterID.Add(ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].PK_LNG_METER_ID);
                }
            }
            return lisMeterID;
        }

        private void btn_TransTmp_Click(object sender, RoutedEventArgs e)
        {
            if(cmb_local_Input.SelectedIndex<0)
            {
                MessageBox.Show("请选择需要导入临时库的数据！");
                return;
            }
            Thread TransThread;
            TransThread = new Thread(new ThreadStart(TransDataToTmpDataBase));
            TransThread.IsBackground=true;
            TransThread.Start();

        }

        /// <summary>
        /// 转移数据到Tmp数据库
        /// </summary>
        private void TransDataToTmpDataBase()
        {
            string strScheme = "";
            string NewID_METER = "";
            ViewLocalData.OperateDataBase viewBase = new ViewLocalData.OperateDataBase();

           // viewBase.Test();
            
            #region DataBase TableName
            List<string> TableName = new List<string>();
            TableName.Add("METER_INFO");
            TableName.Add("METER_CARRIER_WAVE");
            TableName.Add("METER_COMMUNICATION");
            TableName.Add("METER_CONSISTENCY_DATA");
            TableName.Add("METER_ENERGY_TEST_DATA");
            TableName.Add("METER_ERROR");
            TableName.Add("METER_FREEZE");
            TableName.Add("METER_FUN_ENERGY_MEASURE");
            TableName.Add("METER_FUN_EVENT_RECORD");
            TableName.Add("METER_FUN_LOAD_RECORD");
            TableName.Add("METER_FUN_MAX_DEMAND");
            TableName.Add("METER_FUN_RATES_TIME_CONS");
            TableName.Add("METER_FUN_SHOW");
            TableName.Add("METER_FUN_TIME_KEEPING");
            TableName.Add("METER_FUNCTION");
            TableName.Add("METER_HIGH_VOLTAGE");
            TableName.Add("METER_INFRARED_DATA");
            TableName.Add("METER_POWER_CONSUM_DATA");
            TableName.Add("METER_RATES_CONTROL");
            TableName.Add("METER_RESULTS");
            TableName.Add("METER_SPECIAL_DATA");
            TableName.Add("METER_STANDARD_DLT_DATA");
            TableName.Add("METER_START_NO_LOAD");
            TableName.Add("METER_WIRELESS_DATA");
            #endregion

            TableName = viewBase.GetDataBaseTable();

            ViewLocalData.OperateDataBase.DeleteTmpData(TableName);
            List<string> MeterIDList = new List<string>();
            List<string> MeterBnumList = new List<string>();
            for (int i = 0; i<ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.DefaultView.Count; i++)
            {
                MeterIDList.Add(ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.DefaultView[i][0].ToString());
                MeterBnumList.Add(ViewLocalData.ClouModel.ClouMember.CreateInstance().DataBase.DefaultView[i][2].ToString());
            }
            ViewLocalData.OperateDataBase.DeleteTmpBaseInfo(MeterBnumList);
           // ViewLocalData.OperateDataBase.TransDataToTmpDatabase(MeterIDList, TableName);
            try
            {
                Random rdNum = new Random();
                int RandomNum = rdNum.Next(0, 1000);
                int RandomNum001 = rdNum.Next(0, 25);
                foreach (string tempIdList in MeterIDList)
                {
                    #region newID
                    NewID_METER = DateTime.Now.ToString("yyyyMMddHH") + RandomNum.ToString() + RandomNum001.ToString("D3");
                    RandomNum001++;
                    #endregion
                    viewBase.MakeInserSql(TableName, tempIdList, NewID_METER);
                }

                MessageBox.Show("导入临时库成功！");
            }
            catch(Exception errorWord)
            {
                MessageBox.Show("导入临时库失败！" + errorWord.Message);
            }
            

        }

        private void btn_StopUpdate_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateThread.Abort();
            btn_update.IsEnabled = true;
            ShowWord(DateTime.Now.ToString(), "已经手动停止上传数据。");
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            int MeterCount = ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count;
            int intSelectNum = 0;
            for (int i = 0; i < MeterCount; i++)
            {
                if (ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo[i].BolIfup == true)
                {
                    intSelectNum++;
                }
            }
            ViewModel.AllMeterInfo.CreateInstance().SelectMeterNum = "当前选择了<<" + intSelectNum.ToString() + ">>个表";
        }

        private void btn_AutoMakeSeal_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.AllMeterInfo.CreateInstance().MeterBaseInfo.Count <= 0)
            {
                MessageBox.Show("当前没有数据可以添加铅封");
                return;
            }
            UI.InputWindow inputwindow = new UI.InputWindow();
            inputwindow.ShowDialog();
        }


    


    }
}
