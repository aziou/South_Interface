
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Data;
using DataCore;
using System.Collections.Generic;
using System;
using System.Diagnostics;
namespace SoftType_3220
{
    public class csFunction : Mis_Interface_Driver.MisDriver
    {
       
        public readonly string BaseConfigPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml";
       
        
        
        public ObservableCollection<MeterBaseInfoFactor> GetBaseInfo(string CheckTime, string SQL)
        {
            ObservableCollection<MeterBaseInfoFactor> Temp_Base = new ObservableCollection<MeterBaseInfoFactor>();

            using (OleDbConnection conn = new OleDbConnection(DataCore.Global.GB_Base.AccessLink))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                OleDbCommand cmd = new OleDbCommand(SQL, conn);
                OleDbDataReader Myreader = null;
                Myreader = cmd.ExecuteReader();

                while (Myreader.Read())
                {
                    Temp_Base.Add(new MeterBaseInfoFactor()
                    {

                        PK_LNG_METER_ID = Myreader["PK_LNG_METER_ID"].ToString(),
                        LNG_BENCH_POINT_NO = Myreader["LNG_BENCH_POINT_NO"].ToString(),
                        AVR_ASSET_NO = Myreader["AVR_ASSET_NO"].ToString(),
                        AVR_UB = Myreader["AVR_UB"].ToString(),
                        AVR_IB = Myreader["AVR_IB"].ToString(),
                        AVR_TEST_PERSON = Myreader["AVR_TEST_PERSON"].ToString(),
                        AVR_TOTAL_CONCLUSION = Myreader["AVR_TOTAL_CONCLUSION"].ToString(),
                        CHR_UPLOAD_FLAG = Myreader["CHR_UPLOAD_FLAG"].ToString(),
                        AVR_SEAL_1 = Myreader["AVR_SEAL_1"].ToString(),
                        AVR_SEAL_2 = Myreader["AVR_SEAL_2"].ToString(),
                        AVR_SEAL_3 = Myreader["AVR_SEAL_3"].ToString(),
                    });
                }
                conn.Close();
                return Temp_Base;
            }
        }
        #region delete
        public override string DeleteMis(string PKid)
        {
            int excuteSuccess = 0;
            string ErrorResult;
            List<string> SealList = new List<string>();
            List<string> mysql = new List<string>();
            try
            {


                 DeletedMidDataBaeInfo(PKid, out SealList);


                 excuteSuccess = OperateData.PublicFunction.ExcuteToOracle(SealList, out ErrorResult);

                if (excuteSuccess == 0)
                {

                    return PKid + "删除中间库成功！";
                }
                else
                {

                    return PKid + "删除中间库失败！" + ErrorResult;
                }


            }
            catch (Exception e)
            {

                return PKid + "删除中间库失败！" + e.ToString();
            }


        }
        public override string DeleteAllMis()
        {
            int excuteSuccess = 0;
            string ErrorResult;
            List<string> SealList = new List<string>();
            List<string> mysql = new List<string>();
            try
            {


                DeletedAllMidDataBaeInfo(out SealList);


                excuteSuccess = OperateData.PublicFunction.ExcuteToOracle(SealList, out ErrorResult);

                if (excuteSuccess == 0)
                {

                    return "删除中间库工作单成功！";
                }
                else
                {

                    return "删除中间库工作单失败！" + ErrorResult;
                }


            }
            catch (Exception e)
            {

                return "删除中间库工作单失败！" + e.ToString();
            }


        }
        private void DeletedMidDataBaeInfo(string DeleteMeterZcbh, out List<string> DeleteSQL)
        {
            bool blnOK = true;
            List<string> lisSQL = new List<string>();
            string strSQL = "";
            strSQL = "delete from VT_SB_JKDNBJDJL  where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' and zcbh='" + DeleteMeterZcbh + "' ";
                    lisSQL.Add(strSQL);
                    strSQL = "delete from vt_sb_jkzdjcjl  where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' and zcbh='" + DeleteMeterZcbh + "' ";
                    lisSQL.Add(strSQL);
                    strSQL = "delete from VT_SB_JKFYBGJL  where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' and zcbh='" + DeleteMeterZcbh + "' ";
                    lisSQL.Add(strSQL);
                    strSQL = "delete from VT_SB_JKDNBJDWC   where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' and zcbh='" + DeleteMeterZcbh + "' ";
                    lisSQL.Add(strSQL);
                    strSQL = "delete from VT_SB_JKXLWCJL   where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' and zcbh='" + DeleteMeterZcbh + "' ";
                    lisSQL.Add(strSQL);
                    strSQL = "delete from VT_SB_JKSDTQWCJL  where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' and zcbh='" + DeleteMeterZcbh + "' ";
                    lisSQL.Add(strSQL);
                    strSQL = "delete from VT_SB_JKDNBSSJL  where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' and zcbh='" + DeleteMeterZcbh + "' ";
                    lisSQL.Add(strSQL);
                    strSQL = "delete from vt_sb_jkrjswc   where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' and zcbh='" + DeleteMeterZcbh + "' ";
                    lisSQL.Add(strSQL);
                    strSQL = "delete from VT_SB_JKDNBZZJL   where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' and zcbh='" + DeleteMeterZcbh + "' ";
                    lisSQL.Add(strSQL);
                    DeleteSQL = lisSQL;
        }
        private void DeletedAllMidDataBaeInfo(out List<string> DeleteSQL)
        {
            bool blnOK = true;
            List<string> lisSQL = new List<string>();
            string strSQL = "";
            strSQL = "delete from VT_SB_JKDNBJDJL  where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "'";
            lisSQL.Add(strSQL);
            strSQL = "delete from vt_sb_jkzdjcjl  where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' ";
            lisSQL.Add(strSQL);
            strSQL = "delete from VT_SB_JKFYBGJL  where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' ";
            lisSQL.Add(strSQL);
            strSQL = "delete from VT_SB_JKDNBJDWC   where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' ";
            lisSQL.Add(strSQL);
            strSQL = "delete from VT_SB_JKXLWCJL   where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' ";
            lisSQL.Add(strSQL);
            strSQL = "delete from VT_SB_JKSDTQWCJL  where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "'  ";
            lisSQL.Add(strSQL);
            strSQL = "delete from VT_SB_JKDNBSSJL  where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "'  ";
            lisSQL.Add(strSQL);
            strSQL = "delete from vt_sb_jkrjswc   where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "'  ";
            lisSQL.Add(strSQL);
            strSQL = "delete from VT_SB_JKDNBZZJL   where  gzdbh='" + DataCore.Global.GB_Base.MeterGZDBH + "' ";
            lisSQL.Add(strSQL);
            DeleteSQL = lisSQL;
        }
        #endregion 

        #region 上传数据
        public override string UpadataBaseInfo(string PKid, out List<string> Col_For_Seal)
        {
            DataCore.Global.GB_Base.MeterGZDBH = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "TheWorkNum", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
      
            int excuteSuccess = 0;
            string ErrorResult;
            DataCore.Global.GB_Base.MeterId = PKid;
            string Runtime = "";
            List<string> SealList = new List<string>();
            List<string> mysql = new List<string>();
            try
            {

              
                mysql = Get_VT_SB_JKDNBJDJL(PKid, out SealList);
               

               
                excuteSuccess = OperateData.PublicFunction.ExcuteToOracle(mysql, out ErrorResult);
               

                if (excuteSuccess == 0)
                {
                    Col_For_Seal = SealList;
                    return DataCore.Global.GB_Base.MeterZcbh + "基本信息上传到中间库成功！" ;
                }
                else
                {
                    Col_For_Seal = SealList;
                    return DataCore.Global.GB_Base.MeterZcbh + "基本信息上传到中间库失败！" + ErrorResult;
                }


            }
            catch (Exception e)
            {
                Col_For_Seal = SealList;
                return DataCore.Global.GB_Base.MeterZcbh + "基本信息上传到中间库失败！" + e.ToString();
            }


        }

        public override string UpdataErrorInfo(string OnlyIdNum)
        {

            int excuteSuccess;
            string ErrorReason;
            string Runtime = "";
            List<string> mysql = new List<string>();
            try
            {
              
                mysql = Get_VT_SB_JKDNBJDWC(OnlyIdNum);
               
               
                excuteSuccess = OperateData.PublicFunction.ExcuteToOracle(mysql, out ErrorReason);
              
              



               
                if (excuteSuccess == 0)
                {
                    if (mysql.Count == 0)
                    {
                        return "没有基本误差数据！";
                    }
                    else
                    {
                        return "基本误差数据上传到中间库成功！";
                    }
                   
                }
                else
                {
                    return "基本误差上传到中间库失败！" + ErrorReason;
                }




            }
            catch (Exception e)
            {
                return "基本误差上传到中间库失败！" + e.ToString();
            }


        }

        public override string UpdataJKRJSWCInfo(string OnlyIdNum)
        {

            int excuteSuccess;
            string ErrorReason;
            List<string> mysql = new List<string>();
            try
            {

                //mysql = Get_VT_SB_JKRJSWC(OnlyIdNum);



                excuteSuccess = OperateData.PublicFunction.ExcuteToOracle(mysql, out ErrorReason);
                if (excuteSuccess == 0)
                {
                    if (mysql.Count == 0)
                    {
                        return "没有日计时数据！";
                    }
                    else
                    {
                        return "日计时数据上传到中间库成功！";
                    }
                   
                }
                else
                {
                    return "日计时数据上传到中间库失败！" + ErrorReason;
                }




            }
            catch (Exception e)
            {
                return "日计时数据上传到中间库失败！" + e.ToString();
            }


        }

        public override string UpdataJKXLWCJLInfo(string OnlyIdNum, out List<string> Col_For_Demand)
        {

            int excuteSuccess;
            string ErrorReason;
            List<string> mysql = new List<string>();
            List<string> list_Demand = new List<string>();
            try
            {

                //mysql = Get_VT_SB_JKXLWCJL(OnlyIdNum, out list_Demand);



                excuteSuccess = OperateData.PublicFunction.ExcuteToOracle(mysql, out ErrorReason);
                if (excuteSuccess == 0)
                {
                    MakeUp_Result(ref list_Demand, true);
                    Col_For_Demand = list_Demand;
                    if (mysql.Count == 0)
                    {
                        return "没有电表需量数据数据！";
                    }
                    else
                    {
                        return "电表需量数据上传到中间库成功！";
                    }
                  
                }
                else
                {
                    MakeUp_Result(ref list_Demand, false);
                    Col_For_Demand = list_Demand;
                    return "电表需量数据上传到中间库失败！" + ErrorReason;
                }




            }
            catch (Exception e)
            {
                Col_For_Demand = null;
                return "电表需量数据上传到中间库失败！" + e.ToString();
            }


        }

        public override string UpdataSDTQWCJLInfo(string OnlyIdNum)
        {

            int excuteSuccess;
            string ErrorReason;
            List<string> mysql = new List<string>();
            try
            {

                //mysql = Get_VT_SB_JKSDTQWCJL(OnlyIdNum);



                excuteSuccess = OperateData.PublicFunction.ExcuteToOracle(mysql, out ErrorReason);
                if (excuteSuccess == 0)
                {
                    if (mysql.Count == 0)
                    {
                        return "没有时段投切数据数据！";
                    }
                    else
                    {
                        return "时段投切数据上传到中间库成功！";
                    }
                   
                }
                else
                {
                    return "时段投切数据上传到中间库失败！" + ErrorReason;
                }




            }
            catch (Exception e)
            {
                return "时段投切数据上传到中间库失败！" + e.ToString();
            }


        }

        public override string UpdataDNBSSJLInfo(string OnlyIdNum)
        {

            int excuteSuccess;
            string ErrorReason;
            List<string> mysql = new List<string>();
            try
            {

               // mysql = Get_VT_SB_JKDNBSSJL(OnlyIdNum);



                excuteSuccess = OperateData.PublicFunction.ExcuteToOracle(mysql, out ErrorReason);
                if (excuteSuccess == 0)
                {
                    if (mysql.Count == 0)
                    {
                        return "没有电表底度数据！";
                    }
                    else
                    {
                        return "电表底度上传到中间库成功！";
                    }

                }
                else
                {
                    return "电表底度上传到中间库失败！" + ErrorReason;
                }




            }
            catch (Exception e)
            {
                return "电表底度上传到中间库失败！" + e.ToString();
            }


        }

        public override string UpdataDNBZZJLInfo(string OnlyIdNum)
        {

            int excuteSuccess;
            string ErrorReason;
            List<string> mysql = new List<string>();
            try
            {

               // mysql = VT_SB_JKDNBZZJL(OnlyIdNum);



                excuteSuccess = OperateData.PublicFunction.ExcuteToOracle(mysql, out ErrorReason);
                if (excuteSuccess == 0)
                {
                    if (mysql.Count == 0)
                    {
                        return "没有电表走字数据数据！";
                    }
                    else
                    {
                        return "电表走字数据上传到中间库成功！";
                    }
                  
                  
                }
                else
                {
                    return "电表走字数据上传到中间库失败！" + ErrorReason;
                }




            }
            catch (Exception e)
            {
                return "电表走字数据上传到中间库失败！" + e.ToString();
            }


        }

        #endregion



        /// <summary>
        /// 电能表检定记录
        /// </summary>
        /// <returns></returns>
        private static List<string> Get_VT_SB_JKDNBJDJL(string PK_ID, out List<string> Col_For_Seal)
        {
            List<string> lis_Sql = new List<string>();
            List<string> lis_seal = new List<string>();
            string strFZLXDM = "", Runtime = "", MeterConstant = "";
            string str_CheckTime = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "CheckTimeFlag", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");

            string str_CheckName = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "cmb_Jyy", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
            string strSQL = "SELECT * FROM CTD_TERMINAL_INFO where INT_MYID=" + PK_ID + " and DTE_TESTING_DATE=#" + DataCore.Global.GB_Base.MultiCheckTime + "#";
            OleDbConnection AccessConntion = new OleDbConnection(DataCore.Global.GB_Base.AccessLink);
            AccessConntion.Open();
            OleDbCommand ccmd = new OleDbCommand(strSQL, AccessConntion);


            Stopwatch watch = new Stopwatch();
            watch.Start();
            OleDbDataReader OldRead = ccmd.ExecuteReader();
            watch.Stop();
            // Runtime = watch.ElapsedMilliseconds.ToString() + "ms";

            watch = new Stopwatch();
            watch.Start();

            OldRead.Read();
            string strOracleSQL = "insert into vt_sb_jkzdjcjl (";
            string strOracleSQL_Name = "";
            string strOracleSQL_Value = "";
            string UpdateResult = "";
            try
            {
                DataCore.Global.GB_Base.MeterZcbh = OldRead["STR_BARCODE"].ToString().Trim();
                UpdateResult = string.Format("终端：{0}", OldRead["STR_BARCODE"].ToString().Trim());
                //if (DataCore.Global.GB_Base.MeterZcbh == "")
                //    DataCore.Global.GB_Base.MeterZcbh = OldRead["AVR_MADE_NO"].ToString().Trim();
                //if (DataCore.Global.GB_Base.MeterZcbh == "")
                //    DataCore.Global.GB_Base.MeterZcbh = OldRead["AVR_BAR_CODE"].ToString().Trim();

                //strFZLXDM = OldRead["CHR_CT_CONNECTION_FLAG"].ToString().Trim();
                DataCore.Global.GB_Base.MeterCheckDate = OldRead["DTE_TESTING_DATE"].ToString().Trim();
                //DataCore.Global.GB_Base.MeterXBDM = OldRead["AVR_WIRING_MODE"].ToString().Trim();

                string strValue = OldRead["STR_CURRENT"].ToString().Trim();
                int iIb = strValue.IndexOf("(");
                string strIb = strValue.Substring(0, iIb);

                #region 常数 AVR_AR_CONSTANT
                //MeterConstant = OldRead["AVR_AR_CONSTANT"].ToString().Trim();
                //if (MeterConstant.Contains("("))
                //{
                //    DataCore.Global.GB_Base.Meter_Constant = Convert.ToDouble(MeterConstant.Substring(0, MeterConstant.IndexOf("(")));
                //}
                //else
                //{
                //    DataCore.Global.GB_Base.Meter_Constant = Convert.ToDouble(MeterConstant);
                //}

                #endregion

                #region ----------基本检定记录

                strOracleSQL_Name = strOracleSQL_Name + "GZDBH,";
                strOracleSQL_Value = strOracleSQL_Value + "'" + DataCore.Global.GB_Base.MeterGZDBH;
                strOracleSQL_Name = strOracleSQL_Name + "ZCBH,";
                strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.MeterZcbh;
                strOracleSQL_Name = strOracleSQL_Name + "SJBZ,";
                strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.FirstCheckFlag + "";
                //strOracleSQL_Name = strOracleSQL_Name + "BW,";
                //strOracleSQL_Value = strOracleSQL_Value + "','" + OldRead["LNG_BENCH_POINT_NO"].ToString().Trim();
                strOracleSQL_Name = strOracleSQL_Name + "WD,";
                strOracleSQL_Value = strOracleSQL_Value + "','" + OldRead["STR_TEMPERATURE"].ToString().Trim();

                strOracleSQL_Name = strOracleSQL_Name + "SD,";
                strOracleSQL_Value = strOracleSQL_Value + "','" + OldRead["STR_HUMIDITY"].ToString().Trim();
                //strOracleSQL_Name = strOracleSQL_Name + "JDYJDM,"; //检定依据代码
                //strOracleSQL_Value = strOracleSQL_Value + "','01";

                strOracleSQL_Name = strOracleSQL_Name + "JCJLDM,";  //总结论代码
                strValue = OldRead["STR_TEST_CONCLUSION"].ToString().Trim();
                strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                //strOracleSQL_Name = strOracleSQL_Name + "WGJCJLDM,";    //外观标志检查结论代码
                //strValue = Get_METER_RESULTS("100");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);
                //strOracleSQL_Name = strOracleSQL_Name + "WGBZJCJLDM,";    //外观标志检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                #region ----//不检定项目

                strValue = "W";
                //strOracleSQL_Name = strOracleSQL_Name + "YQJJCJLDM,";    //元器件检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "JYXNSYJLDM,";    //绝缘性能试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "MCDYSYJLDM,";    //脉冲电压试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                ////strOracleSQL_Name = strOracleSQL_Name + "MCDYSYJLDM,";    //脉冲电压试验结论代码
                ////strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                ////strOracleSQL_Name = strOracleSQL_Name + "JDQZDNSZWCSYJLDM,";    //计度器总电能示值误差试验结论代码
                ////strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                ////strOracleSQL_Name = strOracleSQL_Name + "DQYQSYJLDM,";    //电气要求试验结论代码
                ////strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;

                //strOracleSQL_Name = strOracleSQL_Name + "GLXHSYJLDM,";    //功率消耗试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" +ResultsCode(Get_METER_POWER_CONSUM_DATA());
                //strOracleSQL_Name = strOracleSQL_Name + "DYDYYXSYJLDM,";    //电源电压影响试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                ////strOracleSQL_Name = strOracleSQL_Name + "DYFWSYJLDM,";    //电压范围试验结论代码
                ////strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "DYZJHDSZDYXSYJLDM,";    //电压暂降或短时中断影响试验结论代码

                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(Get_METER_COMMUNICATION("012"));

                //strOracleSQL_Name = strOracleSQL_Name + "DYDSZDDSZDYXSYJLDM,";    //电压短时中断对时钟的影响试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "DYCSJZDDSZYXSYJLDM,";    //电压长时间中断对时钟影响试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "DYCSJZDDDNBYXSYJLDM,";    //电压长时间中断对电能表影响试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "DYHZLDYTSZDYXSYJLDM,";    //电压和直流电源同时中断对电能表程序和存贮数据的影响试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                ////strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "DYFWSYJLDM,";    //电压范围试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //#region 功能检查

                //#endregion

                //strOracleSQL_Name = strOracleSQL_Name + "DNJLGNJCJLDM,";    //电能计量功能检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "DLDJGNJCJLDM,";    //电量冻结功能检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "FLHSDGNJCJLDM,";    //费率和时段功能检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(Get_METER_COMMUNICATION("004"));
                //strOracleSQL_Name = strOracleSQL_Name + "SJJLGNJCJLDM,";    //事件记录功能检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "MCSCGNJCJLDM,";    //脉冲输出功能检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "XSGNJCJLDM,";    //显示功能检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "YZNRJCJLDM,";    //预置内容检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "AQFHGNJCJLDM,";    //安全防护功能检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "TDDGNJCJLDM,";    //通断电功能检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "TXGNJCJLDM,";    //通信功能检查结论代码
                //if (Get_METER_CARRIER_WAVE() == "不合格")
                //{

                //    strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(Get_METER_CARRIER_WAVE());
                //   // strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(Get_METER_COMMUNICATION("001"));
                //    strOracleSQL_Name = strOracleSQL_Name + "TXGYYZXJCJLDM,";    //通信规约一致性检查结论代码 METER_STANDARD_DLT_DATA
                //    strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(Get_METER_CARRIER_WAVE());
                //}
                //else 
                //{
                //    strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(Get_METER_COMMUNICATION("001"));
                //    //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(Get_METER_CARRIER_WAVE());
                //    strOracleSQL_Name = strOracleSQL_Name + "TXGYYZXJCJLDM,";    //通信规约一致性检查结论代码 METER_STANDARD_DLT_DATA
                //    strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(Get_METER_COMMUNICATION("001"));
                //}


                //strOracleSQL_Name = strOracleSQL_Name + "SJCSXKGRSYJLDM,";    //数据传输线抗干扰试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                #region 误差一致性

                //strOracleSQL_Name = strOracleSQL_Name + "WCBCSYJLDM,";    //误差变差试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + Get_METER_CONSISTENCY_DATA("511");
                //strOracleSQL_Name = strOracleSQL_Name + "WCYZXSYJLDM,";    //误差一致性试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + Get_METER_CONSISTENCY_DATA("411");
                //strOracleSQL_Name = strOracleSQL_Name + "FZDLSYBCSYJLDM,";    //负载电流升降变差试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + Get_METER_CONSISTENCY_DATA("611");
                //strOracleSQL_Name = strOracleSQL_Name + "YZXSYJLDM,";    //一致性试验结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + Get_METER_RESULTS("1031");//1031
                #endregion
                #endregion

                //strOracleSQL_Name = strOracleSQL_Name + "JLDYSYJLDM,";  //交流电压试验结论代码
                //strValue = Get_METER_RESULTS("102");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);
                strOracleSQL_Name = strOracleSQL_Name + "ZQDSYJLDM,";  //准确度要求试验结论代码---- 基本误差试验代替
                strValue = Get_METER_Error();
                strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);
                //strOracleSQL_Name = strOracleSQL_Name + "CSSYJLDM,";  //常数试验结论代码
                //strValue = Get_METER_RESULTS("106");
                //if (strValue == "") strValue = Get_METER_Run("");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                //strOracleSQL_Name = strOracleSQL_Name + "QDDL,";  //起动电流
                //strValue = Get_METER_START_NO_LOAD("1091", "AVR_CURRENT") + "A";
                ////if (strValue != "")
                ////strValue = (double.Parse(strValue) * double.Parse(strIb)).ToString().Trim();
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                strOracleSQL_Name = strOracleSQL_Name + "QDSYJLDM,";  //起动试验结论代码
                strValue = Get_METER_RESULTS("启动");
                strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                //strOracleSQL_Name = strOracleSQL_Name + "QDSYDYZ,";  //潜动试验电压值
                //strValue = Get_METER_START_NO_LOAD("1101115", "AVR_VOLTAGE");
                //if (strValue == "")
                //    strValue = "1";
                //// strValue = OldRead["AVR_UB"].ToString().Trim();
                //strValue = (double.Parse(strValue) * double.Parse(OldRead["AVR_UB"].ToString().Trim())).ToString().Trim() + "V";
                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                //strOracleSQL_Name = strOracleSQL_Name + "FQDLZ,";  //防潜电流值
                //strOracleSQL_Value = strOracleSQL_Value + "','0";
                strOracleSQL_Name = strOracleSQL_Name + "QISYJLDM,";  //潜动试验结论代码
                strValue = Get_METER_RESULTS("潜动");
                strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                strOracleSQL_Name = strOracleSQL_Name + "JBWCSYJLDM,";  //基本误差试验结论代码
                 strValue = Get_METER_Error();
                strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                strOracleSQL_Name = strOracleSQL_Name + "WGJGJCJLDM,";  //外观
                strValue = "合格";
                strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                strOracleSQL_Name = strOracleSQL_Name + "YBGNSYJLDM,";  //一般功能试验结论代码
                strValue = Get_METER_RESULTS("响应时间") + Get_METER_RESULTS("485通道测试") + Get_METER_RESULTS("下发通讯参数");
                if (strValue.IndexOf("不") > 0)
                {
                    strValue = "不合格";
                }
                else
                {
                    strValue = "合格";
                }
                strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                strOracleSQL_Name = strOracleSQL_Name + "SJCJSYJLDM,";  //数据采集试验结论代码
                strValue = Get_METER_RESULTS("读日数据") + Get_METER_RESULTS("读月数据") + Get_METER_RESULTS("实时召测") + Get_METER_RESULTS("曲线数据");
                if (strValue.IndexOf("不") > 0)
                {
                    strValue = "不合格";
                }
                else
                {
                    strValue = "合格";
                }
                strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);
                //strOracleSQL_Name = strOracleSQL_Name + "RJSWCSYJLDM,";  //日计时误差试验结论代码
                //strValue = Get_METER_COMMUNICATION("002");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                //strOracleSQL_Name = strOracleSQL_Name + "RJSWCZ,";  //日计时误差值
                //strValue = Get_METER_COMMUNICATION("00201");
                //if (DataCore.Global.GB_Base.CompanyDQBH != "080000")
                //{
                //    int iTmpPoint = strValue.IndexOf("|");
                //    if (iTmpPoint > 0)
                //        strValue = strValue.Substring(iTmpPoint + 1, strValue.Length - iTmpPoint - 1);
                //}

                //strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;





                //strOracleSQL_Name = strOracleSQL_Name + "JDQZDNSZWCSYJLDM,";  //计度器总电能示值误差试验结论代码
                //strValue = Get_METER_COMMUNICATION("005");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                //strOracleSQL_Name = strOracleSQL_Name + "FLSDDNSSWCSYJLDM,";  //费率时段电能示数误差试验结论代码
                //strValue = Get_METER_COMMUNICATION("006");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);


                //strOracleSQL_Name = strOracleSQL_Name + "XLZQWCSYJLDM,";  //需量周期误差试验结论代码
                //string str_xlzqwc = "";
                //strValue = Get_METER_COMMUNICATION("01402");
                //str_xlzqwc = strValue + ",";
                //strValue = Get_METER_COMMUNICATION("01502");
                //str_xlzqwc = str_xlzqwc+strValue + ",";
                //strValue = Get_METER_COMMUNICATION("01602");
                //str_xlzqwc = str_xlzqwc + strValue + ",";
                //if (str_xlzqwc.Contains("合格"))
                //{
                //    strValue = "合格";
                //}
                //else if (str_xlzqwc.Contains("不合格"))
                //{
                //    strValue = "不合格";
                //}
                //else
                //{
                //    strValue = "未检定";
                //}
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                //strOracleSQL_Name = strOracleSQL_Name + "ZDXLWCSYJLDM,";  //最大需量误差试验结论代码
                //strValue = Get_METER_COMMUNICATION("015");
                //if (strValue == "")strValue= Get_METER_COMMUNICATION("014");
                //if (strValue == "") strValue=Get_METER_COMMUNICATION("016");

                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);
                //strOracleSQL_Name = strOracleSQL_Name + "ZDXLGNJCJLDM,";    //最大需量功能检查结论代码
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                //strValue = Get_METER_CONSISTENCY_DATA("411020700");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);
                //strOracleSQL_Name = strOracleSQL_Name + "WCBCSYJLDM,";  //误差变差试验结论代码
                //strValue = Get_METER_CONSISTENCY_DATA("511020700");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);
                //strOracleSQL_Name = strOracleSQL_Name + "WCYZXSYJLDM,";  //误差一致性试验结论代码
                //strValue = Get_METER_CONSISTENCY_DATA("611010100");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);
                //strOracleSQL_Name = strOracleSQL_Name + "FZDLSYBCSYJLDM,";  //负载电流升降变差试验结论代码
                //strValue = Get_METER_CONSISTENCY_DATA("411010700");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                //strOracleSQL_Name = strOracleSQL_Name + "TXGNJCJLDM,";  //通信功能检查结论代码
                //strValue = Get_METER_COMMUNICATION("001");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                //strOracleSQL_Name = strOracleSQL_Name + "SDTQWCSYJLDM,";  //时段投切误差试验结论代码
                //strValue = Get_METER_COMMUNICATION("004");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                //strOracleSQL_Name = strOracleSQL_Name + "GPSDSJLDM,";  //GPS对时结论代码
                //strValue = Get_METER_COMMUNICATION("007");
                //strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);

                strOracleSQL_Name = strOracleSQL_Name + "JDRYBH,";  //检定员编号
                strOracleSQL_Value = strOracleSQL_Value + "','" + OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_Jyy", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
                ;
                strOracleSQL_Name = strOracleSQL_Name + "HYRYBH,";  //核验员编号
                strValue = OldRead["STR_CHECK_MEMBER"].ToString().Trim();
                string strSection = "MIS_Info/UserName/Item";
                string strXmlValue = "";
                strOracleSQL_Value = strOracleSQL_Value + "','" + OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_Hyy", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");

                strOracleSQL_Name = strOracleSQL_Name + "DQBM,";  //地区编码
                strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.CompanyDQBH;

                strOracleSQL_Name = strOracleSQL_Name + "BZZZZCBH,";  //电能计量标准设备资产编号
                strSection = "NewUser/CloumMIS/Item";
                strXmlValue = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_equipment", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
                ;
                strOracleSQL_Value = strOracleSQL_Value + "','" + strXmlValue;

                #region ----------日期型处理

                strOracleSQL_Name = strOracleSQL_Name + "JDRQ,";  //检定日期
                // strValue = OldRead["DTM_TEST_DATE"].ToString().Trim();
                strValue = Convert.ToDateTime(OldRead["DTE_TESTING_DATE"]).ToShortDateString().Trim() + " " + DateTime.Now.ToLongTimeString().Trim();
                strOracleSQL_Value = strOracleSQL_Value + "',to_date('" + strValue + "','yyyy-mm-dd hh24:mi:ss')";

                strOracleSQL_Name = strOracleSQL_Name + "HYRQ";  //核验日期
                strValue = Convert.ToDateTime(OldRead["DTE_TESTING_DATE"]).ToShortDateString().Trim() + " " + DateTime.Now.ToLongTimeString().Trim();
                strOracleSQL_Value = strOracleSQL_Value + ",to_date('" + strValue + "','yyyy-mm-dd hh24:mi:ss')";

                #endregion


                strOracleSQL = strOracleSQL + strOracleSQL_Name + ")  Values (" + strOracleSQL_Value + ")";

                #endregion

                lis_Sql.Add(strOracleSQL);

                #region ----------铅封上传


                string strSealValuePath = "NewUser/CloumMIS/Item";
                string str_Seal01 = "", str_Seal02 = "", str_Seal03 = "";
                List<string> ColSeal = new List<string>();
                str_Seal01 = OperateData.FunctionXml.ReadElement(strSealValuePath, "Name", "cmb_Seal01", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
                str_Seal02 = OperateData.FunctionXml.ReadElement(strSealValuePath, "Name", "cmb_Seal02", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
                str_Seal03 = OperateData.FunctionXml.ReadElement(strSealValuePath, "Name", "cmb_Seal03", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
                str_Seal01 = SwitchSealNum(str_Seal01);
                str_Seal02 = SwitchSealNum(str_Seal02);
                str_Seal03 = SwitchSealNum(str_Seal03);

                ColSeal.Add(str_Seal01); ColSeal.Add(str_Seal02); ColSeal.Add(str_Seal03);
                for (int iCirc = 1; iCirc < 2; iCirc++)
                {
                    if (OldRead["STR_TEST_CONCLUSION"].ToString().Trim() == "不合格") continue;
                    string strCode = "STR_SEAL00" + iCirc.ToString().Trim();
                    strOracleSQL_Name = "";
                    strOracleSQL_Value = "";
                    if (OldRead[strCode].ToString().Trim() != "")
                    {

                        strOracleSQL = "insert into VT_SB_JKFYBGJL (";

                        strOracleSQL_Name = strOracleSQL_Name + "GZDBH,";
                        strOracleSQL_Value = strOracleSQL_Value + "'" + DataCore.Global.GB_Base.MeterGZDBH;
                        strOracleSQL_Name = strOracleSQL_Name + "ZCBH,";
                        strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.MeterZcbh;
                        strOracleSQL_Name = strOracleSQL_Name + "BGBZ,";
                        strOracleSQL_Value = strOracleSQL_Value + "','10";

                        strOracleSQL_Name = strOracleSQL_Name + "FYZCBH,";
                        strOracleSQL_Value = strOracleSQL_Value + "','" + OldRead[strCode].ToString().Trim();
                        lis_seal.Add(OldRead[strCode].ToString().Trim());
                        strOracleSQL_Name = strOracleSQL_Name + "JFWZDM,";//加封位置代码-------
                        strValue = ColSeal[iCirc - 1];
                        //strValue = Get_JFWZDM(iCirc.ToString().Trim());
                        strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;

                        strOracleSQL_Name = strOracleSQL_Name + "DQBM,";  //地区编码
                        strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.CompanyDQBH;

                        strOracleSQL_Name = strOracleSQL_Name + "JFSJ";//时间
                        strValue = Convert.ToDateTime(OldRead["DTE_TESTING_DATE"]).ToShortDateString().Trim() + " " + DateTime.Now.ToLongTimeString().Trim();
                        //strValue = OldRead["DTM_TEST_DATE"].ToString().Trim();

                        strOracleSQL_Value = strOracleSQL_Value + "',to_date('" + strValue + "','yyyy-mm-dd hh24:mi:ss')";

                        strOracleSQL = strOracleSQL + strOracleSQL_Name + ")  Values (" + strOracleSQL_Value + ")";

                        lis_Sql.Add(strOracleSQL);
                    }
                }

                #endregion


                AccessConntion.Close();
                OldRead.Close();

                watch.Stop();
                Runtime = Runtime + "执行：" + watch.ElapsedMilliseconds.ToString() + "ms";
            }
            catch (Exception Error)
            {
                AccessConntion.Close();
                OldRead.Close();
            }
            Col_For_Seal = lis_seal;
            return lis_Sql;

        }
        /// <summary>
        /// 被检表多功能信息
        /// </summary>
        /// <param name="strSection"></param>
        /// <param name="RESULT_ID"></param>
        /// <returns></returns>
        private static string Get_METER_COMMUNICATION(string RESULT_ID)
        {
            string strResults = "";
            string strSQL = " SELECT AVR_VALUE FROM METER_COMMUNICATION where AVR_PROJECT_NO='" + RESULT_ID + "' and FK_LNG_METER_ID='" + DataCore.Global.GB_Base.MeterId + "'";

            OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
            strResults = MyDb.GetSingleData(strSQL, DataCore.Global.GB_Base.AccessLink);


            return strResults;
        }
        private static string Get_METER_CARRIER_WAVE()
        {
            string strResults = "";
            string strSQL = " SELECT AVR_CONCLUSION FROM METER_CARRIER_WAVE where FK_LNG_METER_ID='" + DataCore.Global.GB_Base.MeterId + "'";

            OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
            strResults = MyDb.GetSingleData(strSQL, DataCore.Global.GB_Base.AccessLink);


            return strResults;
        }
        private static string Get_METER_POWER_CONSUM_DATA()
        {
            string strResults = "";
            string strSQL = " SELECT AVR_CONCLUSION FROM METER_POWER_CONSUM_DATA where FK_LNG_METER_ID='" + DataCore.Global.GB_Base.MeterId + "'";

            OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
            strResults = MyDb.GetSingleData(strSQL, DataCore.Global.GB_Base.AccessLink);


            return strResults;
        }

        private static string Get_METER_STANDARD_DLT_DATA()
        {
            string strResult = "";
            string strSQL = "select AVR_CONC FROM METER_STANDARD_DLT_DATA where FK_LNG_METER_ID='" + DataCore.Global.GB_Base.MeterId + "'";
            OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
            strResult = MyDb.CheckErrorData(strSQL, DataCore.Global.GB_Base.AccessLink);


            return strResult;
        }
        private static string Get_METER_COMMUNICATION_HanPu(string RESULT_ID, string meterId)
        {
            string strResults = "";
            string strSQL = " SELECT DialEnd FROM [Meters].[dbo].[PM_Constant] where TestID='" + meterId + "'";
            string ServerName = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_SqlServerName", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
            string Con = string.Format("Server={0};Database=Meters;Trusted_Connection=SSPI", ServerName);
            OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
            strResults = MyDb.GetSingleData_SqlServer(strSQL, Con);


            return strResults;
        }
        private static string Get_METER_COMMUNICATION(string RESULT_ID,string meterId)
        {
            string strResults = "";
            string strSQL = " SELECT AVR_VALUE FROM METER_COMMUNICATION where AVR_PROJECT_NO='" + RESULT_ID + "' and FK_LNG_METER_ID='" + meterId + "'";

            OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
            strResults = MyDb.GetSingleData(strSQL, DataCore.Global.GB_Base.AccessLink);


            return strResults;
        }
        private static string Get_METER_CONSISTENCY_DATA(string ItemNum)
        {
            string strResults = "";
            string strSQL = " SELECT AVR_CONC FROM METER_CONSISTENCY_DATA where AVR_ITEM_NO LIKE '" + ItemNum + "%' and FK_LNG_METER_ID='" + DataCore.Global.GB_Base.MeterId + "'";

            OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
            strResults = MyDb.CheckErrorData(strSQL, DataCore.Global.GB_Base.AccessLink);


            return ResultsCode(strResults);
        }
        /// <summary>
        /// 日计量误差记录
        /// </summary>
        /// <returns></returns>
        private static List<string> Get_VT_SB_JKRJSWC(string PK_ID)
        {
            #region Fake
            Random ran = new Random();
            int n = ran.Next(10, 55);
            double[] ArrValue=new double[6];
            for (int i = 0; i < 5; i++)
            {
                n = ran.Next(10, 99);
                ArrValue[i] = Math.Round(n * 0.001, 3);
            }
            ArrValue[5] = Math.Round((ArrValue[0] + ArrValue[1] + ArrValue[2] + ArrValue[3] + ArrValue[4]) / 5,3);
            #endregion
            if (DataCore.Global.GB_Base.IsFakeData)
            {
                List<string> listSQL = new List<string>();

                string strValue = "";
                string strOracleSQL_Name = "";
                string strOracleSQL_Value = "";
                string strOracleSQL = "insert into vt_sb_jkrjswc (";
                #region 规避重复数据
                List<string> ProjectCol = new List<string>();
                #endregion
                try
                {
                    strValue = Get_METER_COMMUNICATION("00202");
                   
                        strOracleSQL_Name = strOracleSQL_Name + "GZDBH,";   //工作单编号
                        strOracleSQL_Value = strOracleSQL_Value + "'" + DataCore.Global.GB_Base.MeterGZDBH;
                        strOracleSQL_Name = strOracleSQL_Name + "ZCBH,";   //资产编号
                        strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.MeterZcbh;

                        char[] csplit = { '|' };
                        //string[] strParm = null;
                        //strParm = strValue.Split(csplit);
                        strOracleSQL_Name = strOracleSQL_Name + "CSZ1,";   //误差1
                        if (ArrValue.Length > 0)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + ArrValue[0].ToString().Trim();
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";

                        strOracleSQL_Name = strOracleSQL_Name + "CSZ2,";   //误差2
                        if (ArrValue.Length > 1)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + ArrValue[1].ToString().Trim();
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";

                        strOracleSQL_Name = strOracleSQL_Name + "CSZ3,";   //误差3
                        if (ArrValue.Length > 2)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + ArrValue[2].ToString().Trim();
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";
                        strOracleSQL_Name = strOracleSQL_Name + "CSZ4,";   //误差4
                        if (ArrValue.Length > 3)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + ArrValue[3].ToString().Trim();
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";

                        strOracleSQL_Name = strOracleSQL_Name + "CSZ5,";   //误差5
                        if (ArrValue.Length > 4)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + ArrValue[4].ToString().Trim();
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";


                        strValue = Get_METER_COMMUNICATION("00201");//平均值与 化整值
                        
                            
                            strOracleSQL_Name = strOracleSQL_Name + "PJZ,";   //平均值
                            if (ArrValue.Length > 0)
                                strOracleSQL_Value = strOracleSQL_Value + "','" + ArrValue[5].ToString().Trim();
                            else
                                strOracleSQL_Value = strOracleSQL_Value + "','";


                     

                        strOracleSQL_Name = strOracleSQL_Name + "DQBM";   //地区编码
                        strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.CompanyDQBH;


                        strOracleSQL = strOracleSQL + strOracleSQL_Name + ")  Values (" + strOracleSQL_Value + "')";

                        listSQL.Add(strOracleSQL);
                    }

                
                catch { }

                return listSQL;
            }
            else
            {
                List<string> listSQL = new List<string>();

                string strValue = "";
                string strOracleSQL_Name = "";
                string strOracleSQL_Value = "";
                string strOracleSQL = "insert into vt_sb_jkrjswc (";
                #region 规避重复数据
                List<string> ProjectCol = new List<string>();
                #endregion
                try
                {
                    strValue = Get_METER_COMMUNICATION("00202");
                    if (strValue.IndexOf("|") > 0)  //有误差值
                    {
                        strOracleSQL_Name = strOracleSQL_Name + "GZDBH,";   //工作单编号
                        strOracleSQL_Value = strOracleSQL_Value + "'" + DataCore.Global.GB_Base.MeterGZDBH;
                        strOracleSQL_Name = strOracleSQL_Name + "ZCBH,";   //资产编号
                        strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.MeterZcbh;

                        char[] csplit = { '|' };
                        string[] strParm = null;
                        strParm = strValue.Split(csplit);
                        strOracleSQL_Name = strOracleSQL_Name + "CSZ1,";   //误差1
                        if (strParm.Length > 0)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[0];
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";

                        strOracleSQL_Name = strOracleSQL_Name + "CSZ2,";   //误差2
                        if (strParm.Length > 1)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[1];
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";

                        strOracleSQL_Name = strOracleSQL_Name + "CSZ3,";   //误差3
                        if (strParm.Length > 2)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[2];
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";
                        strOracleSQL_Name = strOracleSQL_Name + "CSZ4,";   //误差4
                        if (strParm.Length > 3)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[3];
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";

                        strOracleSQL_Name = strOracleSQL_Name + "CSZ5,";   //误差5
                        if (strParm.Length > 4)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[4];
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";


                        strValue = Get_METER_COMMUNICATION("00201");//平均值与 化整值
                        if (strValue.IndexOf("|") > 0)  //有误差值
                        {
                            strParm = strValue.Split(csplit);
                            strOracleSQL_Name = strOracleSQL_Name + "PJZ,";   //平均值
                            if (strParm.Length > 0)
                                strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[0];
                            else
                                strOracleSQL_Value = strOracleSQL_Value + "','";


                        }

                        strOracleSQL_Name = strOracleSQL_Name + "DQBM";   //地区编码
                        strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.CompanyDQBH;


                        strOracleSQL = strOracleSQL + strOracleSQL_Name + ")  Values (" + strOracleSQL_Value + "')";

                        listSQL.Add(strOracleSQL);
                    }

                }
                catch { }

                return listSQL;
            }
            
        }
        /// <summary>
        /// 电能表误差记录
        /// </summary>
        /// <returns></returns>
        private static List<string> Get_VT_SB_JKDNBJDWC(string PK_ID)
        {
            ObservableCollection<DataCore.Struct._3220ErrorScheme> errorStruct = new ObservableCollection<DataCore.Struct._3220ErrorScheme>();
        
            #region 获取方案的误差信息
            OleDbConnection AccessConntion = new OleDbConnection(DataCore.Global.GB_Base.AccessSchemeLink);
            try
            {
                //string SchemeSQL = "SELECT * FROM CTS_SCHEME_ITEMS WHERE STR_ITEM_NAM='基本误差' ORDER BY INI_ITEM_INDEX DESC";
                string SchemeSQL = "select * from CTS_SCHEME_ITEMS where STR_ITEM_NAME='基本误差'";
                AccessConntion.Open();
                OleDbCommand ccmd = new OleDbCommand(SchemeSQL, AccessConntion);
                Stopwatch watch = new Stopwatch();
                
                OleDbDataReader red = ccmd.ExecuteReader();
          

              
                char[] csplit = { '|' };
                string[] strParm = null;
             
               
                while (red.Read() == true)
                {
                    strParm = red["STR_PARAMETER_VALUE"].ToString().Trim().Split(csplit);
                    errorStruct.Add(new DataCore.Struct._3220ErrorScheme()
                    {
                        ErrorGLFX = strParm[1],
                        ErrorFYDM = strParm[2],
                        ErrorGLYS = strParm[3],
                        ErrorFZDL = strParm[4],
                    });
                }
                red.Close();
                AccessConntion.Close();
                AccessConntion.Dispose();
          
                watch.Stop();
              



            }
            catch (Exception error) { }
            finally
            {
                AccessConntion.Close();
            }


            #endregion
            List<string> listSQL = new List<string>();
            string strSQL = "SELECT * FROM CTD_VERIFY_DATA WHERE  INT_TERMINAL_MYID=" + PK_ID + "and STR_SUBITEM_NAME_CN='基本误差' order by INT_ITEM_INDEX ";
            string strValue = "";
            string strOracleSQL_Name = "";
            string strOracleSQL_Value = "";
            string strOracleSQL = "insert into VT_SB_JKDNBJDWC (";

            string Error_avr = "", Error_limit = "", Runtime="";
            string ErrorResult = "Y";
            #region 去除重复信息
            List<string> proCol = new List<string>();


            #endregion
            AccessConntion = new OleDbConnection(DataCore.Global.GB_Base.AccessLink);
            try
            {
                #region 获取误差表的信息
                AccessConntion.Open();
                OleDbCommand ccmd = new OleDbCommand(strSQL, AccessConntion);


              


                Stopwatch watch = new Stopwatch();
                watch.Start();
                OleDbDataReader red = ccmd.ExecuteReader();
                watch.Stop();
                Runtime = watch.ElapsedMilliseconds.ToString() + "ms";

                watch = new Stopwatch();
                watch.Start();


                int count = 0;
                while (red.Read() == true)
                {
                    strOracleSQL = "insert into VT_SB_JKDNBJDWC (";
                    strOracleSQL_Value = "";
                    strOracleSQL_Name = "";

                    strOracleSQL_Name = strOracleSQL_Name + "GZDBH,";   //工作单编号
                    strOracleSQL_Value = strOracleSQL_Value + "'" + DataCore.Global.GB_Base.MeterGZDBH;
                    strOracleSQL_Name = strOracleSQL_Name + "ZCBH,";   //资产编号
                    strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.MeterZcbh;

                    strOracleSQL_Name = strOracleSQL_Name + "GLFXDM,";   //功率方向代码
                    strValue = Get_GLFXDM(errorStruct[count].ErrorGLFX);
                    strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                    strOracleSQL_Name = strOracleSQL_Name + "GLYSDM,";   //功率因数代码
                    strValue = Get_GLYSDM(errorStruct[count].ErrorGLYS);
                    strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                    strOracleSQL_Name = strOracleSQL_Name + "FZDLDM,";   //负载电流代码
                    strValue = Get_FZDLDM(errorStruct[count].ErrorFZDL);
                    strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;

                    strOracleSQL_Name = strOracleSQL_Name + "XBDM,";   //相别代码 三相、单相
                    strValue = Get_XBDM(DataCore.Global.GB_Base.MeterXBDM);
                    strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;

                    strOracleSQL_Name = strOracleSQL_Name + "FZLXDM,";   //负载类型代码 平衡负载、不平衡负载-1、2、3、4
                    strValue = Get_FZLXDM(errorStruct[count].ErrorFYDM);
                    //strValue = "01";
                    //if (csPublicMember.strFZLXDM == "01" || csPublicMember.strFZLXDM == "1") 
                    //    strValue = "02";
                    strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;

                    strOracleSQL_Name = strOracleSQL_Name + "FYDM,";   //分元代码 01、02、03、04
                    strValue = Get_FZLXDM(errorStruct[count].ErrorFYDM);
                    strOracleSQL_Value = strOracleSQL_Value + "','" + strValue.PadLeft(2, '0');

                    strValue = red["STR_VERIFY_DATA"].ToString().Trim();
                    char[] csplit = { '|' };
                    string[] strParm = null;
                    string[] strLimite = null;
                    strParm = strValue.Split(csplit);

                    //Error_limit = red["AVR_UPPER_LIMIT"].ToString().Trim();
                    //strLimite = Error_limit.Split(csplit);

                   // Error_limit = strLimite[0];
                    if (strParm.Length > 2 && !(red["STR_VERIFY_CONCLUSION"].ToString().Trim().IndexOf("格", 0) >= 0))
                    {
                        ErrorResult = Convert.ToDouble(Error_limit) - Math.Abs(Convert.ToDouble(strParm[strParm.Length - 2])) > 0 ? "Y" : "N";
                    }


                    strOracleSQL_Name = strOracleSQL_Name + "WCZ1,";   //误差1
                    if (strParm.Length > 1)
                        strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[0];
                    else
                        strOracleSQL_Value = strOracleSQL_Value + "','";

                    strOracleSQL_Name = strOracleSQL_Name + "WCZ2,";   //误差2
                    if (strParm.Length > 2)
                        strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[1];
                    else
                        strOracleSQL_Value = strOracleSQL_Value + "','";

                    strOracleSQL_Name = strOracleSQL_Name + "WCZ3,";   //误差3
                    if (strParm.Length > 4)
                        strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[2];
                    else
                        strOracleSQL_Value = strOracleSQL_Value + "','";

                    strOracleSQL_Name = strOracleSQL_Name + "WCZ4,";   //误差4
                    if (strParm.Length > 5)
                        strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[3];
                    else
                        strOracleSQL_Value = strOracleSQL_Value + "','";

                    strOracleSQL_Name = strOracleSQL_Name + "WCZ5,";   //误差5
                    if (strParm.Length > 7)
                        strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[4];
                    else
                        strOracleSQL_Value = strOracleSQL_Value + "','";

                    strOracleSQL_Name = strOracleSQL_Name + "WCPJZ,";   //误差平均值
                    if (strParm.Length > 2)
                        strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[strParm.Length - 2];


                    else
                        strOracleSQL_Value = strOracleSQL_Value + "','";

                    strOracleSQL_Name = strOracleSQL_Name + "WCXYZ,";   //误差修约值
                    if (strParm.Length > 2)
                        strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[strParm.Length - 1];
                    else
                        strOracleSQL_Value = strOracleSQL_Value + "','";



                    strOracleSQL_Name = strOracleSQL_Name + "JLDM,";   //结论代码
                    strValue = red["STR_VERIFY_CONCLUSION"].ToString().Trim();
                    if (!(red["STR_VERIFY_CONCLUSION"].ToString().Trim().IndexOf("格", 0) >= 0))
                    {
                        strOracleSQL_Value = strOracleSQL_Value + "','" + ErrorResult;

                    }
                    else
                    {
                        strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue);


                    }

                    strOracleSQL_Name = strOracleSQL_Name + "WCCZ,";   //不平衡负载与平衡负载的误差差值
                    strValue = "";
                    strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;

                    strOracleSQL_Name = strOracleSQL_Name + "WCCZXYZ,";   //误差差值修约值
                    strValue = "";
                    strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;

                    strOracleSQL_Name = strOracleSQL_Name + "DQBM";  //地区编码
                    strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.CompanyDQBH;


                    strOracleSQL = strOracleSQL + strOracleSQL_Name + ")  Values (" + strOracleSQL_Value + "')";
                  
                    listSQL.Add(strOracleSQL);
                    count++;
                }
                red.Close();
                AccessConntion.Close();
                AccessConntion.Dispose();
                #endregion
                watch.Stop();
                Runtime = Runtime + "执行：" + watch.ElapsedMilliseconds.ToString() + "ms";

              

            }
            catch (Exception error) { }
            finally
            {
                AccessConntion.Close();
            }


            return listSQL;
        }
        /// <summary>
        /// 电能表示数记录表
        /// </summary>
        /// <returns></returns>
        private static List<string> Get_VT_SB_JKDNBSSJL(string PK_ID)
        {
            
            List<string> listSQL = new List<string>();
            string strValue = "";
            string strTypeCode = "";
            string strOracleSQL_Name = "";
            string strOracleSQL_Value = "";
            string strOracleSQL = "insert into VT_SB_JKDNBSSJL (";
            #region 防止重复数据
            List<string> ProjectCol = new List<string>();
            #endregion
            try
            {
                for (int iCirc = 1; iCirc < 5; iCirc++) //P+,P-,Q+,Q-, Q1,Q2,Q3,Q4
                {

                    strTypeCode = "0170" + iCirc.ToString().Trim();    //01701正向有功 /01702反向有功 /01703正向无功/01704反向无功

                    strValue = Get_METER_COMMUNICATION(strTypeCode, PK_ID);
                    if (strValue.IndexOf("|") > 0)  //有误差值
                    {
                        char[] csplit = { '|' };//总|尖|峰|平|谷|
                        string[] strParm = null;
                        strParm = strValue.Split(csplit);
                        if (strParm.Length >= 4)
                        {
                            for (int iCircLx = 0; iCircLx < strParm.Length; iCircLx++) //总|尖|峰|平|谷|
                            {
                                if (iCircLx == 1) continue;
                                strOracleSQL_Name = "";
                                strOracleSQL_Value = "";
                                strOracleSQL = "insert into VT_SB_JKDNBSSJL (";

                                //if (!OutRunSampleInfo(ref ProjectCol, strTypeCode))
                                //{
                                //    continue;
                                //}

                                strOracleSQL_Name = strOracleSQL_Name + "GZDBH,";   //工作单编号
                                strOracleSQL_Value = strOracleSQL_Value + "'" + DataCore.Global.GB_Base.MeterGZDBH;
                                strOracleSQL_Name = strOracleSQL_Name + "ZCBH,";   //资产编号
                                strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.MeterZcbh;

                                strOracleSQL_Name = strOracleSQL_Name + "SSLXDM,";   //示数类型代码
                                strValue = strTypeCode + iCircLx.ToString().Trim();
                                strValue = Get_SSLXDM(strValue);
                                strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;

                                strOracleSQL_Name = strOracleSQL_Name + "BSS,";   //表示值
                                strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[iCircLx];

                                strOracleSQL_Name = strOracleSQL_Name + "DQBM,";   //地区编码
                                strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.CompanyDQBH;

                                strOracleSQL_Name = strOracleSQL_Name + "CBSJ";  //抄表时间----检定日期代替
                                strValue = DataCore.Global.GB_Base.MeterCheckDate;
                                strOracleSQL_Value = strOracleSQL_Value + "',to_date('" + strValue + "','yyyy-mm-dd hh24:mi:ss')";

                                strOracleSQL = strOracleSQL + strOracleSQL_Name + ")  Values (" + strOracleSQL_Value + ")";


                                listSQL.Add(strOracleSQL);
                               



                            }
                        }
                    }
                }
            }
            catch { }
            return listSQL;
        }
        /// <summary>
        /// 电能表投切记录表
        /// </summary>
        /// <returns></returns>
        private static List<string> Get_VT_SB_JKSDTQWCJL(string PK_ID)
        {
            
            List<string> listSQL = new List<string>();
            string strValue = "";
            string strOracleSQL_Name = "";
            string strOracleSQL_Value = "";
            string strOracleSQL = "insert into VT_SB_JKSDTQWCJL (";
            #region  规避重复数据
            List<string> ProjectCol = new List<string>();
            #endregion

            try
            {
                for (int iCirc = 1; iCirc < 7; iCirc++)
                {
                   
                    strValue = "0040" + iCirc.ToString();    //00401/00402/00403


                    if (!OutRunSampleInfo(ref ProjectCol, strValue))
                    {

                        continue;
                    }
                    strValue = Get_METER_COMMUNICATION(strValue);
                    if (strValue.IndexOf("|") > 0)  //有误差值
                    {

                        strOracleSQL_Name = "";
                        strOracleSQL_Value = "";
                        strOracleSQL = "insert into VT_SB_JKSDTQWCJL (";

                        char[] csplit = { '|' };
                        string[] strParm = null;
                        strParm = strValue.Split(csplit);
                        if (strParm.Length >= 4)
                        {
                            strOracleSQL_Name = strOracleSQL_Name + "GZDBH,";   //工作单编号
                            strOracleSQL_Value = strOracleSQL_Value + "'" + DataCore.Global.GB_Base.MeterGZDBH;
                            strOracleSQL_Name = strOracleSQL_Name + "ZCBH,";   //资产编号
                            strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.MeterZcbh;

                            strOracleSQL_Name = strOracleSQL_Name + "SD,";   //时段
                            strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[3] + "时段";


                            strOracleSQL_Name = strOracleSQL_Name + "BZTQSJ,";   //标准投切时间
                            if (strParm.Length > 0)
                                strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[0];
                            else
                                strOracleSQL_Value = strOracleSQL_Value + "','";

                            strOracleSQL_Name = strOracleSQL_Name + "SJTQSJ,";   //实际投切时间
                            if (strParm.Length > 1)
                                strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[1];
                            else
                                strOracleSQL_Value = strOracleSQL_Value + "','";

                            strOracleSQL_Name = strOracleSQL_Name + "TQWC,";   //投切误差
                            if (strParm.Length > 2)
                            {
                                strValue = strParm[2].Replace(":", "");
                                strValue = int.Parse(strValue).ToString();
                                strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;
                            }
                            else
                                strOracleSQL_Value = strOracleSQL_Value + "','";


                            strOracleSQL_Name = strOracleSQL_Name + "DQBM";   //地区编码
                            strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.CompanyDQBH;


                            strOracleSQL = strOracleSQL + strOracleSQL_Name + ")  Values (" + strOracleSQL_Value + "')";

                            listSQL.Add(strOracleSQL);
                        }
                    }
                }
            }
            catch { }




            return listSQL;
        }
        /// <summary>
        /// 电能表需量记录表
        /// </summary>
        /// <returns></returns>
        private static List<string> Get_VT_SB_JKXLWCJL(string PK_ID,out List<string> Col_For_demand)
        {

            List<string> listSQL = new List<string>();
            List<string> listDemand = new List<string>();
            
            string strValue = "";
            string strOracleSQL_Name = "";
            string strOracleSQL_Value = "";
            string strOracleSQL = "insert into VT_SB_JKXLWCJL (";
            #region  规避重复数据
            List<string> ProjectCol = new List<string>();
            #endregion

            try
            {
                for (int iCirc = 14; iCirc < 17; iCirc++)
                {
                    strOracleSQL_Name = "";
                    strOracleSQL_Value = "";
                    strOracleSQL = "insert into VT_SB_JKXLWCJL (";

                    strValue = iCirc.ToString("000") + "11";    //01401/01501/01601
                    if (!OutRunSampleInfo(ref ProjectCol, strValue))
                    {

                        continue;
                    }

                    strValue = Get_METER_COMMUNICATION(strValue);
                    if (strValue.IndexOf("|") > 0)  //有误差值
                    {
                        char[] csplit = { '|' };
                        string[] strParm = null;
                        strParm = strValue.Split(csplit);

                        strOracleSQL_Name = strOracleSQL_Name + "GZDBH,";   //工作单编号
                        strOracleSQL_Value = strOracleSQL_Value + "'" +DataCore.Global.GB_Base.MeterGZDBH ;
                        strOracleSQL_Name = strOracleSQL_Name + "ZCBH,";   //资产编号
                        strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.MeterZcbh;
                
                        if (iCirc == 14)
                        {
                            strValue = "02";   //0.1IB
                            listDemand.Add("上传最大需量0.1IB");
                        }
                        if (iCirc == 15)
                        {
                            strValue = "05";   //IB
                            listDemand.Add("上传最大需量1IB");

                        }
                        if (iCirc == 16)
                        {
                            strValue = "06";   //Imax
                            listDemand.Add("上传最大需量Imax");

                        } 
                        strOracleSQL_Name = strOracleSQL_Name + "FZDLDM,";   //负载电流代码
                        strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;


                        strOracleSQL_Name = strOracleSQL_Name + "BZZDXL,";   //标准最大需量
                        if (strParm.Length > 0)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[0];
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";

                        strOracleSQL_Name = strOracleSQL_Name + "SJXL,";   //实际需量
                        if (strParm.Length > 1)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[1];
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";

                        strOracleSQL_Name = strOracleSQL_Name + "WCZ,";   //误差值
                        if (strParm.Length > 2)
                            strOracleSQL_Value = strOracleSQL_Value + "','" + strParm[2];
                        else
                            strOracleSQL_Value = strOracleSQL_Value + "','";


                        strValue = Get_METER_COMMUNICATION(iCirc.ToString("000"));//结论代码
                        strOracleSQL_Name = strOracleSQL_Name + "JLDM,";
                        strOracleSQL_Value = strOracleSQL_Value + "','" + ResultsCode(strValue) + "','";

                        strOracleSQL_Name = strOracleSQL_Name + "DQBM";   //地区编码
                        strOracleSQL_Value = strOracleSQL_Value + DataCore.Global.GB_Base.CompanyDQBH;


                        strOracleSQL = strOracleSQL + strOracleSQL_Name + ")  Values (" + strOracleSQL_Value + "')";

                        listSQL.Add(strOracleSQL);
                    }
                }
            }
            catch { }
            Col_For_demand = listDemand;
            return listSQL;
        }
        /// <summary>
        /// 电能表走字记录表
        /// </summary>
        /// <returns></returns>
        private static List<string> VT_SB_JKDNBZZJL(string PK_ID)
        {
            Random n = new Random();
            int fake = n.Next(1000, 2000);
            int fakeError = n.Next(10, 50);
            double fakeNum = fake * 0.001, changeValue = 1;
            if (DataCore.Global.GB_Base.IsFakeData)
            {
                bool blnOK = true;
                List<string> listSQL = new List<string>();
                string strValue = "";
                string strTypeCode = "";
                string strSQL = "select * from METER_ENERGY_TEST_DATA  where  FK_LNG_METER_ID='" + PK_ID + "'";
                string strOracleSQL_Name = "";
                string strOracleSQL_Value = "";
                string strOracleSQL = "insert into VT_SB_JKDNBZZJL (";
                OleDbConnection AccessConntion = new OleDbConnection(DataCore.Global.GB_Base.AccessLink);
                try
                {
                    AccessConntion.Open();
                    OleDbCommand ccmd = new OleDbCommand(strSQL, AccessConntion);
                    OleDbDataReader red = ccmd.ExecuteReader();
                    #region 防止重复数据
                    List<string> ProjectCol = new List<string>();
                    #endregion
                   
                        strOracleSQL_Name = "";
                        strOracleSQL_Value = "";
                        strOracleSQL = "insert into VT_SB_JKDNBZZJL (";

                       


                        strOracleSQL_Name = strOracleSQL_Name + "GZDBH,";   //工作单编号
                        strOracleSQL_Value = strOracleSQL_Value + "'" + DataCore.Global.GB_Base.MeterGZDBH;
                        strOracleSQL_Name = strOracleSQL_Name + "ZCBH,";   //资产编号
                        strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.MeterZcbh;

                        strOracleSQL_Name = strOracleSQL_Name + "SSLXDM,";   //示数类型代码
                        strValue = "1";
                        //strTypeCode = "0170" + strValue;    //01701正向有功 /01702反向有功 /01703正向无功/01704反向无功
                        strTypeCode = strValue + "总"; // 总、峰平谷尖
                        strValue = Get_SSLXDM(strTypeCode);
                        strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;

                        strOracleSQL_Name = strOracleSQL_Name + "bzqqss,";   //标准器起示数
                        strOracleSQL_Value = strOracleSQL_Value + "','0";
                        strOracleSQL_Name = strOracleSQL_Name + "bzqzss,";   //标准器止示数
                        strOracleSQL_Value = strOracleSQL_Value + "','" + (changeValue ).ToString().Trim();
                        strOracleSQL_Name = strOracleSQL_Name + "qss,";   //被检电能表起示数
                        strOracleSQL_Value = strOracleSQL_Value + "','" + fakeNum.ToString().Trim();
                        strOracleSQL_Name = strOracleSQL_Name + "zss,";   //被检电能表止示数
                        strOracleSQL_Value = strOracleSQL_Value + "','" + (fakeNum + changeValue + fakeError * 0.001).ToString().Trim();
                        strOracleSQL_Name = strOracleSQL_Name + "zzwc,";   //走字误差
                        strOracleSQL_Value = strOracleSQL_Value + "','" + (fakeError * 0.001).ToString().Trim();

                        strOracleSQL_Name = strOracleSQL_Name + "DQBM";   //地区编码
                        strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.CompanyDQBH;

                        strOracleSQL = strOracleSQL + strOracleSQL_Name + ")  Values (" + strOracleSQL_Value + "')";

                        listSQL.Add(strOracleSQL);

                    }
                
                catch { }

                return listSQL;
            }
            else
            {
                bool blnOK = true;
                List<string> listSQL = new List<string>();
                double RunElect=0,MeterRunBefore=0,MeterRunAfter=0,runError=0;
                string strValue = "";
                string strTypeCode = "";
                string strSQL = "select * from METER_ENERGY_TEST_DATA  where  FK_LNG_METER_ID='" + PK_ID + "'";
                string strOracleSQL_Name = "";
                string strOracleSQL_Value = "";
                string strOracleSQL = "insert into VT_SB_JKDNBZZJL (";
                OleDbConnection AccessConntion = new OleDbConnection(DataCore.Global.GB_Base.AccessLink);
                try
                {
                    AccessConntion.Open();
                    OleDbCommand ccmd = new OleDbCommand(strSQL, AccessConntion);
                    OleDbDataReader red = ccmd.ExecuteReader();
                    #region 防止重复数据
                    List<string> ProjectCol = new List<string>();
                    #endregion
                    while (red.Read() == true)
                    {
                        strOracleSQL_Name = "";
                        strOracleSQL_Value = "";
                        strOracleSQL = "insert into VT_SB_JKDNBZZJL (";

                        if (!OutRunSampleInfo(ref ProjectCol, red["AVR_PROJECT_NO"].ToString().Trim()))
                        {
                            continue;
                        }
                        #region 计算标准表走字电量
                        MeterRunBefore = Convert.ToDouble(red["AVR_START_ENERGY"].ToString().Trim());
                        MeterRunAfter = Convert.ToDouble(red["AVR_STANDARD_METER_ENERGY"].ToString().Trim());
                        MeterRunAfter = MeterRunBefore + Convert.ToDouble(red["AVR_STANDARD_METER_ENERGY"].ToString().Trim()) / DataCore.Global.GB_Base.Meter_Constant;

                        #endregion

                        strOracleSQL_Name = strOracleSQL_Name + "GZDBH,";   //工作单编号
                        strOracleSQL_Value = strOracleSQL_Value + "'" + DataCore.Global.GB_Base.MeterGZDBH;
                        strOracleSQL_Name = strOracleSQL_Name + "ZCBH,";   //资产编号
                        strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.MeterZcbh;

                        strOracleSQL_Name = strOracleSQL_Name + "SSLXDM,";   //示数类型代码
                        strValue = red["CHR_POWER_TYPE"].ToString().Trim();
                        //strTypeCode = "0170" + strValue;    //01701正向有功 /01702反向有功 /01703正向无功/01704反向无功
                        strTypeCode = strValue + red["AVR_RATES"].ToString().Trim(); // 总、峰平谷尖
                        strValue = Get_SSLXDM(strTypeCode);
                        strOracleSQL_Value = strOracleSQL_Value + "','" + strValue;

                        strOracleSQL_Name = strOracleSQL_Name + "bzqqss,";   //标准器起示数
                        strOracleSQL_Value = strOracleSQL_Value + "','0";
                        strOracleSQL_Name = strOracleSQL_Name + "bzqzss,";   //标准器止示数
                        strOracleSQL_Value = strOracleSQL_Value + "','" + red["AVR_DIF_ENERGY"].ToString().Trim();
                        RunElect = Convert.ToDouble(red["AVR_DIF_ENERGY"].ToString().Trim());
                        strOracleSQL_Name = strOracleSQL_Name + "qss,";   //被检电能表起示数
                        strOracleSQL_Value = strOracleSQL_Value + "','" + MeterRunBefore.ToString("#0.0000").Trim();
                        
                        strOracleSQL_Name = strOracleSQL_Name + "zss,";   //被检电能表止示数
                        strOracleSQL_Value = strOracleSQL_Value + "','" + MeterRunAfter.ToString("#0.0000").Trim();
                       
                        strOracleSQL_Name = strOracleSQL_Name + "zzwc,";   //走字误差
                        runError = (MeterRunAfter - MeterRunBefore - RunElect) / RunElect * 100;
                        strOracleSQL_Value = strOracleSQL_Value + "','" + runError.ToString("#0.0000").Trim();
                        //strOracleSQL_Value = strOracleSQL_Value + "','" + (Convert.ToDouble(red["AVR_END_ENERGY"].ToString().Trim()) - Convert.ToDouble(red["AVR_START_ENERGY"].ToString().Trim())).ToString("#0.000");
                       
                        strOracleSQL_Name = strOracleSQL_Name + "DQBM";   //地区编码
                        strOracleSQL_Value = strOracleSQL_Value + "','" + DataCore.Global.GB_Base.CompanyDQBH;

                        strOracleSQL = strOracleSQL + strOracleSQL_Name + ")  Values (" + strOracleSQL_Value + "')";

                        listSQL.Add(strOracleSQL);

                    }
                }
                catch { }

                return listSQL;
            }
           
        }
        /// <summary>
        /// 转换铅封位置代码
        /// </summary>
        /// <param name="str_Seal"></param>
        /// <returns></returns>
        private static string SwitchSealNum(string str_Seal)
        {
            string str_SealNum = "";
            switch (str_Seal.Trim())
            {
                case "左耳封":
                    str_SealNum = "01";
                    break;
                case "右耳封":
                    str_SealNum = "02";
                    break;
                case "编程小门":
                    str_SealNum = "07";
                    break;
                case "表盖封印":
                    str_SealNum = "05";
                    break;
            }
            return str_SealNum;
        }

       
        /// <summary>
        /// 结论与代号转换
        /// </summary>
        /// <param name="strResult"></param>
        /// <returns></returns>
        private static string ResultsCode(string strResult)
        {
            string strResultCode = "W";
            if (strResult.Trim().IndexOf("合格", 0) >= 0)
            {
                strResultCode = "Y";

                if (strResult.Trim().IndexOf("不", 0) >= 0)
                {
                    strResultCode = "N";
                }
            }


            else
                strResultCode = "W";
            return strResultCode;
        }
        /// <summary>
        /// 多功能结论读取
        /// </summary>
        /// <param name="strSection"></param>
        /// <param name="RESULT_ID"></param>
        /// <returns></returns>
        private static string Get_METER_RESULTS(string RESULT_ID)
        {
            string strResults = "";
            string strSQL = " SELECT STR_VERIFY_CONCLUSION FROM CTD_VERIFY_DATA where STR_ITEM_NAME_CN='" + RESULT_ID + "' and INT_TERMINAL_MYID=" + DataCore.Global.GB_Base.MeterId + "";

            OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
            strResults = MyDb.GetSingleData(strSQL, DataCore.Global.GB_Base.AccessLink);


            return strResults;
        }
        /// <summary>
        /// 走字数据
        /// </summary>
        /// <param name="strSection"></param>
        /// <param name="RESULT_ID"></param>
        /// <returns></returns>
        private static string Get_METER_Run(string RESULT_ID)
        {
            string strResults = "";
            string strSQL = " SELECT AVR_CONCLUSION FROM METER_ENERGY_TEST_DATA where  FK_LNG_METER_ID='" + DataCore.Global.GB_Base.MeterId + "'";

            OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
            strResults = MyDb.GetSingleData(strSQL, DataCore.Global.GB_Base.AccessLink);


            return strResults;
        }
        /// <summary>
        /// 误差数据遍历
        /// </summary>
        /// <param name="strSection"></param>
        /// <param name="RESULT_ID"></param>
        /// <returns></returns>
        private static string Get_METER_Error()
        {
            string strResults = "";
            string strSQL = " SELECT STR_VERIFY_CONCLUSION FROM CTD_VERIFY_DATA where  INT_TERMINAL_MYID=" + DataCore.Global.GB_Base.MeterId + " and STR_SUBITEM_NAME_CN='基本误差'";

            OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
            strResults = MyDb.CheckErrorData(strSQL, DataCore.Global.GB_Base.AccessLink);


            return strResults;
        }
        /// <summary>
        /// 启动潜动数据
        /// </summary>
        /// <param name="strSection"></param>
        /// <param name="RESULT_ID"></param>
        /// <returns></returns>
        private static string Get_METER_START_NO_LOAD(string RESULT_ID, string strName)
        {
            string strResults = "";
            string strSQL = " SELECT " + strName + " FROM METER_START_NO_LOAD where AVR_PROJECT_NO='" + RESULT_ID + "' and  FK_LNG_METER_ID='" + DataCore.Global.GB_Base.MeterId + "'";

            OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
            strResults = MyDb.GetSingleData(strSQL, DataCore.Global.GB_Base.AccessLink);


            return strResults;
        }

        /// <summary>
        /// 一致性试验数据
        /// </summary>
        /// <param name="strSection"></param>
        /// <param name="RESULT_ID"></param>
        /// <returns></returns>
        //private static string Get_METER_CONSISTENCY_DATA(string RESULT_ID)
        //{
        //    string strResults = "";
        //    string strSQL = " SELECT AVR_CONC FROM METER_CONSISTENCY_DATA where FK_LNG_SCHEME_ID=" + RESULT_ID + " and FK_LNG_METER_ID='" + DataCore.Global.GB_Base.MeterId + "'";

        //    OperateData.PublicFunction MyDb = new OperateData.PublicFunction();
        //    strResults = MyDb.GetSingleData(strSQL, DataCore.Global.GB_Base.AccessLink);



        //    return strResults;
        //}
        private static bool OutRunSampleInfo(ref List<string> projectCol, string NowProjectId)
        {
            if (projectCol.Count < 1)
            {
                projectCol.Add(NowProjectId);
                return true;
            }
            else
            {
                if (projectCol.Contains(NowProjectId))
                {
                    return false;
                }
                else
                {
                    projectCol.Add(NowProjectId);
                    return true;
                }
            }

        }
        /// <summary>
        /// 功率方向代码
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private static string Get_GLFXDM(string strValue)
        {
            string strResults = "1";
            switch (strValue)
            {
                case "0":
                    strResults = "1";
                    break;
                case "1":
                    strResults = "3";
                    break;
                case "2":
                    strResults = "2";
                    break;
                case "3":
                    strResults = "4";
                    break;
                case "4":
                    strResults = "5";
                    break;
                case "5":
                    strResults = "6";
                    break;
                case "6":
                    strResults = "7";
                    break;
                case "7":
                    strResults = "8";
                    break;
                case "正向有功":
                    strResults = "1";
                    break;
                case "反向有功":
                    strResults = "3";
                    break;
                case "正向无功":
                    strResults = "2";
                    break;
                case "反向无功":
                    strResults = "4";
                    break;
            }

            return strResults;
        }

        /// <summary>
        /// 功率方向代码
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private static string Get_GLFXDM(string strValue,bool IsSpecial)
        {
            string strResults = "1";
            switch (strValue)
            {
                case "0":
                    strResults = "1";
                    break;
                case "1":
                    strResults = "1";
                    break;
                case "2":
                    strResults = "2";
                    break;
                case "3":
                    strResults = "4";
                    break;
                case "4":
                    strResults = "5";
                    break;
                case "5":
                    strResults = "6";
                    break;
                case "6":
                    strResults = "7";
                    break;
                case "7":
                    strResults = "8";
                    break;
            }

            return strResults;
        }
        /// <summary>
        /// 负载电流
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private static string Get_FZDLDM(string strValue)
        {
            string strResults = "";
            switch (strValue.ToUpper())
            {
                case "0.05IB":
                case ".05IB":
                    strResults = "01";
                    break;
                case "0.1IB":
                case ".1IB":
                    strResults = "02";
                    break;
                case "0.2IB":
                case ".2IB":
                    strResults = "03";
                    break;
                case "0.5IB":
                case ".5IB":
                    strResults = "04";
                    break;
                case "1.0IB":
                case "1IB":
                    strResults = "05";
                    break;
                case "IMAX":
                    strResults = "06";
                    break;
                case "0.5IMAX":
                    strResults = "07";
                    break;
                case "4IB":
                case "4.0IB":
                    strResults = "08";
                    break;
                case "3IB":
                case "3.0IB":
                    strResults = "09";
                    break;
                case "2IB":
                case "2.0IB":
                    strResults = "10";
                    break;
                case "0.02IB":
                case ".02IB":
                    strResults = "11";
                    break;
                case "0.01IB":
                case ".01IB":
                    strResults = "12";
                    break;
                case "0.03IB":
                case ".03IB":
                    strResults = "15";
                    break;
                case "0.5(IMAX-IB)":
                    strResults = "16";
                    break;
            }

            return strResults;
        }
        /// <summary>
        /// 功率因数代码
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private static string Get_GLYSDM(string strValue)
        {
            string strResults = "05";
            switch (strValue)
            {
                case "0.5L":
                case ".5L":
                    strResults = "1";
                    break;
                case "1.0":
                case "1":
                    strResults = "2";
                    break;
                case "0.8C":
                case ".8C":
                    strResults = "3";
                    break;
                case "0.5C":
                case ".5C":
                    strResults = "4";
                    break;
                case "0.25L":
                case ".25L":
                    strResults = "5";
                    break;
                case "0.25C":
                case ".25C":
                    strResults = "6";
                    break;
            }
           

            return strResults;
        }
        /// <summary>
        /// 示数类型代码
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private static string Get_SSLXDM(string strValue)
        {
            string strResults = "121";
            switch (strValue.Trim())
            {
                case "017010"://正向有功 总
                case "1总"://正向有功 总
                    strResults = "121";
                    break;
                case "017011"://正向有功 峰
                case "1峰":
                    strResults = "122";
                    break;
                case "017012"://正向有功 平
                case "1平":
                    strResults = "123";
                    break;
                case "017013"://正向有功 谷
                case "1谷":
                    strResults = "124";
                    break;
                case "017014"://正向有功 尖
                case "1尖":
                    strResults = "125";
                    break;
                case "017020"://反向有功 总
                case "2总":
                    strResults = "221";
                    break;
                case "017021"://         峰
                case "2峰":
                    strResults = "225";
                    break;
                case "017022"://         平
                case "2平":
                    strResults = "222";
                    break;
                case "017023"://         谷
                case "2谷":
                    strResults = "223";
                    break;
                case "017024"://         尖
                case "2尖":
                    strResults = "224";
                    break;
                case "017030"://正向无功 总
                case "3总":
                    strResults = "131";
                    break;
                case "017031"://         峰
                case "3峰":
                    strResults = "132";
                    break;
                case "017032"://         平
                case "3平":
                    strResults = "133";
                    break;
                case "017033"://         谷
                case "3谷":
                    strResults = "135";
                    break;
                case "017034"://         尖
                case "3尖":
                    strResults = "134";
                    break;
                case "017040"://反向无功 总
                case "4总":
                    strResults = "231";
                    break;
                case "017041"://         峰
                case "4峰":
                    strResults = "232";
                    break;
                case "017042"://         平
                case "4平":
                    strResults = "236";
                    break;
                case "017043"://         谷
                case "4谷":
                    strResults = "238";
                    break;
                case "017044"://         尖
                case "4尖":
                    strResults = "237";
                    break;
            }

            return strResults;
        }
        /// <summary>
        /// 相别代码
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private static string Get_XBDM(string strValue)
        {
            string strResults = "02";
            switch (strValue)
            {
                case "5":
                    strResults = "01";
                    break;
                case "0":
                    strResults = "02";
                    break;
                case "1":
                    strResults = "02";
                    break;


            }
            return strResults;
        }

        /// <summary>
        /// 平衡与不平衡负载
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private static string Get_FZLXDM(string strValue)
        {
            string strResults = "1";
            switch (strValue)
            {
                case "H":
                    strResults = "1";
                    break;
                case "A":
                    strResults = "2";
                    break;
                case "B":
                    strResults = "3";
                    break;
                case "C":
                    strResults = "4";
                    break;


            }
            return strResults;
        }

        private static void MakeUp_Result(ref List<string> Col_Result, bool IsSuccess)
        {
            if (IsSuccess)
            {
                for (int i = 0; i < Col_Result.Count; i++)
                {
                    Col_Result[i] = Col_Result[i] + "成功";
                }
            }
            else
            {
                for (int i = 0; i < Col_Result.Count; i++)
                {
                    Col_Result[i] = Col_Result[i] + "失败";
                }
            }
        }
      
         public  void UpdateToOracle(object o)
        {
            List<string > Lis_Id=(List<string>)o;
            UpdateToOracle(Lis_Id);
        }
    }
}
