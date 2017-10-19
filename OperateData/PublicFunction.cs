﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Collections.ObjectModel;
using DataCore;
using System.IO;
//using System.Data.OracleClient;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
namespace OperateData
{
    public class PublicFunction
    {
        public string Sql_word_1 = "Provider=Microsoft.ACE.OleDb.12.0;Data Source=";
        public string Sql_word_2 = ";Persist Security Info=False";
        public readonly string BaseConfigPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml";
        public readonly string datapath = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "AccessLink", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
        public readonly string datapath_tmp = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "AccessLink", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
        public readonly string OracleLink = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "OracleLink", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
       
        /// <summary>
        /// 执行access  _SQL
        /// </summary>
        /// <param name="SQL"></param>
        private void ExcuteAccess(string SQL)
        {
            using (OleDbConnection conn = new OleDbConnection(datapath))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                OleDbCommand cmd = new OleDbCommand(SQL, conn);
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// 执行access  _SQL
        /// </summary>
        /// <param name="SQL"></param>
        //public bool ExcuteAccess(List<string> SQL_list)
        //{
        //    try
        //    {
        //        string Path = "";
        //        Path = datapath_tmp.Substring(0, datapath_tmp.LastIndexOf(@"\")) + @"\ClouMeterDataTmp.mdb";
        //        using (OleDbConnection conn = new OleDbConnection(Sql_word_1 + Path + Sql_word_2))
        //        {
        //            if (conn.State == ConnectionState.Closed)
        //                conn.Open();
        //            foreach (string temp in SQL_list)
        //            {
        //                OleDbCommand cmd = new OleDbCommand(temp, conn);
        //                cmd.ExecuteNonQuery();
        //            }

        //        }
        //        return true;
        //    }
        //    catch (Exception Exlist)
        //    {
        //        return false;
        //    }
     
        //}

        /// <summary>
        /// 执行access  _SQL
        /// </summary>
        /// <param name="SQL"></param>
        public static bool ExcuteAccess(List<string> SQL_list)
        {
            try
            {
                string Path = "";
                string Sql_word_2 = ";Persist Security Info=False";
                Path = DataCore.Global.GB_Base.AccessLink.Substring(0, DataCore.Global.GB_Base.AccessLink.LastIndexOf(@"\")) + @"\ClouMeterDataTmp.mdb";
                using (OleDbConnection conn = new OleDbConnection(Path + Sql_word_2))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    foreach (string temp in SQL_list)
                    {
                        OleDbCommand cmd = new OleDbCommand(temp, conn);
                        cmd.ExecuteNonQuery();
                    }

                }
                return true;
            }
            catch (Exception Exlist)
            {
                return false;
            }

        }
        /// <summary>
        /// 执行access  _SQL
        /// </summary>
        /// <param name="SQL"></param>
        public static bool ExcuteAccess(List<string> SQL_list,string DataPath,int copy)
        {
            try
            {
                string Path = "";
                string sql_word_1 = "Provider=Microsoft.ACE.OleDb.12.0;Data Source=";
                string Sql_word_2 = ";Persist Security Info=False";
                Path = DataCore.Global.GB_Base.AccessLink.Substring(0, DataCore.Global.GB_Base.AccessLink.LastIndexOf(@"\")) + @"\ClouMeterDataTmp.mdb";
                using (OleDbConnection conn = new OleDbConnection(DataPath + Sql_word_2))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    foreach (string temp in SQL_list)
                    {
               
                        OleDbCommand cmd = new OleDbCommand(temp, conn);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                return true;
            }
            catch (Exception Exlist)
            {
                return false;
            }

        }
        /// <summary>
        /// 执行access  _SQL
        /// </summary>
        /// <param name="SQL"></param>
        public bool ExcuteAccess(List<string> SQL_list,string Tmppath,bool Is3000G)
        {
            try
            {

                using (OleDbConnection conn = new OleDbConnection(datapath))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    foreach (string temp in SQL_list)
                    {
                        OleDbCommand cmd = new OleDbCommand(temp, conn);
                        cmd.ExecuteNonQuery();
                    }

                }
                return true;
            }
            catch (Exception Exlist)
            {
                return false;
            }

        }
        /// <summary>
        /// 执行access  _SQL
        /// </summary>
        /// <param name="SQL"></param>
        public bool ExcuteAccess(List<string> SQL_list,string local)
        {
            try
            {

                using (OleDbConnection conn = new OleDbConnection(datapath))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    foreach (string temp in SQL_list)
                    {
                        OleDbCommand cmd = new OleDbCommand(temp, conn);
                        cmd.ExecuteNonQuery();
                    }

                }
                return true;
            }
            catch (Exception Exlist)
            {
                return false;
            }

        }

        /// <summary>
        /// 执行access  _SQL
        /// </summary>
        /// <param name="SQL"></param>
        public List<string> ExcuteSqlServer(string SQL, string Message_out)
        {

            List<string> OutMessage = new List<string>();
            string Con = string.Format("Server={0};Database=Meters;Trusted_Connection=SSPI",OperateData.FunctionXml.ReadElement( "NewUser/CloumMIS/Item", "Name", "txt_SqlServerName", "Value", "", BaseConfigPath));
            using (SqlConnection conn = new SqlConnection(Con))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader myreader = null;
                    myreader = cmd.ExecuteReader();
                    while (myreader.Read())
                    {
                        OutMessage.Add(myreader[Message_out].ToString().Trim());
                    }
                    return OutMessage;
                    conn.Close();
                }
                catch (Exception e)
                {
                    conn.Close();
                    return OutMessage;
                }


            }
        }

        /// <summary>
        /// 执行access  _SQL
        /// </summary>
        /// <param name="SQL"></param>
        public  List<string> ExcuteAccess(string SQL,string Message_out)
        {
            
            List<string> OutMessage = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(datapath))
            {
                try
                {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
               
                    OleDbCommand cmd = new OleDbCommand(SQL, conn);
                    OleDbDataReader myreader = null;
                    myreader = cmd.ExecuteReader();
                    while (myreader.Read())
                    {
                        OutMessage.Add(myreader[Message_out].ToString().Trim());
                    }
                    return OutMessage;
                    conn.Close();
                }
                catch (Exception e)
                {
                    conn.Close();
                    return OutMessage;
                }
               

            }
        }
        public ObservableCollection<MeterBaseInfoFactor> GetBaseInfo(string SQL, string Soft_Type)
        {
            ObservableCollection<MeterBaseInfoFactor> BaseInfo=new ObservableCollection<MeterBaseInfoFactor> ();
            switch (OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "Cmb_Facory", "Value", "", BaseConfigPath))
            {
                case "科陆电子":
                    BaseInfo= GetBaseInfo("", SQL, Soft_Type);
                     break;
                case "格宁":
                     BaseInfo= GetGeNingBaseInfo("", SQL);
                    break;
                case "涵普":
                    BaseInfo = GetHanPuBaseInfo("", SQL);
                    break;
            }
            return BaseInfo;
        }
        #region 别的厂家
        public ObservableCollection<MeterBaseInfoFactor> GetGeNingBaseInfo(string CheckTime, string SQL)
        {
            ObservableCollection<MeterBaseInfoFactor> Temp_Base = new ObservableCollection<MeterBaseInfoFactor>();
            using (OleDbConnection conn = new OleDbConnection(datapath))
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
                            PK_LNG_METER_ID = Myreader["MeterID"].ToString(),
                            LNG_BENCH_POINT_NO = Myreader["MeterPlace"].ToString(),
                            AVR_ASSET_NO = Myreader["SerialNo"].ToString(),
                            AVR_UB = Myreader["Voltage"].ToString(),
                            AVR_IB = Myreader["Current"].ToString(),
                            AVR_TEST_PERSON = Myreader["TESTER"].ToString(),
                            AVR_TOTAL_CONCLUSION = Myreader["Conclusion"].ToString(),
                            CHR_UPLOAD_FLAG = Myreader["Net"].ToString() == "x" ? "已上传" : "未上传",
                            AVR_SEAL_1 = Myreader["qfh1"].ToString(),
                            AVR_SEAL_2 = Myreader["qfh2"].ToString(),
                            AVR_SEAL_3 = Myreader["qfh3"].ToString(),
                        });
                    }
                    conn.Close();
                    return Temp_Base;
                }
        }
        public ObservableCollection<MeterBaseInfoFactor> GetHanPuBaseInfo(string CheckTime, string SQL)
        {
            ObservableCollection<MeterBaseInfoFactor> Temp_Base = new ObservableCollection<MeterBaseInfoFactor>();
            string ServerName = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_SqlServerName", "Value", "", BaseConfigPath);
            string Con = string.Format("Server={0};Database=Meters;Trusted_Connection=SSPI", ServerName);
            using (SqlConnection conn = new SqlConnection(Con))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(SQL, conn);
                SqlDataReader Myreader = null;
                Myreader = cmd.ExecuteReader();

                while (Myreader.Read())
                {
                    Temp_Base.Add(new MeterBaseInfoFactor()
                    {
                        PK_LNG_METER_ID = Myreader["TestID"].ToString().Trim(),
                        LNG_BENCH_POINT_NO = Myreader["Seat"].ToString().Trim(),
                        AVR_ASSET_NO = Myreader["Barcode"].ToString().Trim(),
                        AVR_UB = Myreader["Voltage"].ToString().Trim(),
                        AVR_IB = Myreader["CurrentA"].ToString().Trim(),
                        AVR_TEST_PERSON = Myreader["TestedBy"].ToString().Trim(),
                        AVR_TOTAL_CONCLUSION = Myreader["Result"].ToString().Trim() == "1" ? "合格" : "不合格",
                        CHR_UPLOAD_FLAG ="",
                        AVR_SEAL_1 = Myreader["SealNo"].ToString(),
                        AVR_SEAL_2 = Myreader["SealNoLT"].ToString(),
                        AVR_SEAL_3 = Myreader["SealNoRT"].ToString(),
                    });
                }
                conn.Close();
                return Temp_Base;
            }
        }
        #endregion

        public ObservableCollection<MeterBaseInfoFactor> GetBaseInfo(string CheckTime,string SQL,string Soft_Type)
        {
            ObservableCollection<MeterBaseInfoFactor> Temp_Base = new ObservableCollection<MeterBaseInfoFactor>();
            int itemsNum = 0;
            if (Soft_Type == "CL3000S")
            {
                using (OleDbConnection conn = new OleDbConnection(datapath))
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
                            Int_ItemsNum=itemsNum,
                            PK_LNG_METER_ID = Myreader["PK_LNG_METER_ID"].ToString(),
                            LNG_BENCH_POINT_NO = Myreader["LNG_BENCH_POINT_NO"].ToString(),
                            AVR_ASSET_NO = Myreader["AVR_ASSET_NO"].ToString().Trim() == "" ? Myreader["AVR_BAR_CODE"].ToString().Trim() : Myreader["AVR_ASSET_NO"].ToString().Trim(),
                            AVR_UB = Myreader["AVR_UB"].ToString(),
                            AVR_IB = Myreader["AVR_IB"].ToString(),
                            AVR_TEST_PERSON = Myreader["AVR_TEST_PERSON"].ToString(),
                            AVR_TOTAL_CONCLUSION = Myreader["AVR_TOTAL_CONCLUSION"].ToString(),
                            CHR_UPLOAD_FLAG = Myreader["CHR_UPLOAD_FLAG"].ToString().Trim()=="1"?"已上传":"未上传",
                            AVR_SEAL_1 = Myreader["AVR_SEAL_1"].ToString().Trim() == "" ? Myreader["AVR_SEAL_3"].ToString() : Myreader["AVR_SEAL_1"].ToString(),
                            AVR_SEAL_2 = Myreader["AVR_SEAL_2"].ToString(),
                            AVR_SEAL_3 = Myreader["AVR_SEAL_3"].ToString(),
                            DTM_VALID_DATE=Myreader["DTM_VALID_DATE"].ToString(),
                            DTM_TEST_DATE = Myreader["DTM_TEST_DATE"].ToString().Trim(),
                        });
                        itemsNum++;
                    }
                    conn.Close();
                    return Temp_Base;
                }

            }
            else if (Soft_Type == "CL3220NW")
            {
                using (OleDbConnection conn = new OleDbConnection(datapath))
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
                            Int_ItemsNum = itemsNum,
                            PK_LNG_METER_ID = Myreader["INT_MYID"].ToString(),
                            LNG_BENCH_POINT_NO = Myreader["INT_TABLE_NO"].ToString(),
                            AVR_ASSET_NO = Myreader["STR_BARCODE"].ToString().Trim() ,
                            AVR_UB = Myreader["STR_VOLTAGE"].ToString(),
                            AVR_IB = Myreader["STR_CURRENT"].ToString(),
                            AVR_TEST_PERSON = Myreader["STR_INSPECTOR_MEMBER"].ToString(),
                            AVR_TOTAL_CONCLUSION = Myreader["STR_TEST_CONCLUSION"].ToString(),
                            //CHR_UPLOAD_FLAG = Myreader["BOL_UPLOAD_FLAG"].ToString().Trim() ,
                            CHR_UPLOAD_FLAG = Myreader["BOL_UPLOAD_FLAG"].ToString().Trim() == "True" ? "已上传" : "未上传",
                            AVR_SEAL_1 = Myreader["STR_SEAL001"].ToString().Trim(),
                            AVR_SEAL_2 = "",
                            AVR_SEAL_3 = "",
                            //DTM_VALID_DATE = Myreader["DTM_VALID_DATE"].ToString(),
                            DTM_TEST_DATE = Myreader["DTE_TESTING_DATE"].ToString().Trim(),
                        });
                        itemsNum++;
                    }
                    conn.Close();
                    return Temp_Base;
                }
            }
            else
            {
                using (OleDbConnection conn = new OleDbConnection(datapath))
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
                            Int_ItemsNum = itemsNum,
                            PK_LNG_METER_ID = Myreader["intMyId"].ToString(),
                            LNG_BENCH_POINT_NO = Myreader["intBno"].ToString(),
                            AVR_ASSET_NO = Myreader["chrJlbh"].ToString(),
                            AVR_UB = Myreader["chrUb"].ToString(),
                            AVR_IB = Myreader["chrIb"].ToString(),
                            AVR_TEST_PERSON = Myreader["chrJyy"].ToString(),
                            AVR_TOTAL_CONCLUSION = Myreader["chrJdjl"].ToString(),
                            CHR_UPLOAD_FLAG = Myreader["chrSetNetState"].ToString() == "1" ? "已上传" : "未上传",
                            AVR_SEAL_1 = Myreader["chrQianFeng1"].ToString(),
                            AVR_SEAL_2 = Myreader["chrQianFeng2"].ToString(),
                            AVR_SEAL_3 = Myreader["chrQianFeng3"].ToString(),
                            DTM_VALID_DATE = Myreader["datJjrq"].ToString(),
                            DTM_TEST_DATE = Myreader["datJdrq"].ToString().Trim(),
                        });
                        itemsNum++;
                    }
                    conn.Close();
                    return Temp_Base;
                }
            }
            
        }

        /// <summary>
        /// 写入本地日志文件
        /// </summary>
        /// <param name="ExceptLog">错误日志</param>
        /// <param name="File_Path">存放目录</param>
        public static void WriteLog(string ExceptLog, string File_Path)
        {
            try
            {
                if (!File.Exists(File_Path))
                {
                    #region 创建新文件并且写下Log
                    FileStream fs = new FileStream(File_Path, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    try
                    {
                        sw.WriteLine("{0} {1}", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), ExceptLog);
                        sw.Flush();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        if (sw != null)
                        {
                            sw.Close();
                        }
                    }

                    #endregion
                }
                else
                {
                    #region 写入LOG
                    FileStream fs = new FileStream(File_Path, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);

                    try
                    {
                        sw.WriteLine("{0} {1}", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), ExceptLog);
                        sw.Flush();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        if (sw != null)
                        {
                            sw.Close();
                        }
                    }

                    #endregion

                }
            }
            catch(Exception E)
            { 
            
            }
            
        }

        /// <summary>
        /// 写入本地日志文件(清空)
        /// </summary>
        /// <param name="ExceptLog">错误日志</param>
        /// <param name="File_Path">存放目录</param>
        public static void WriteLog(string ExceptLog, string File_Path,bool IsFirst)
        {
            try
            {
                if (!File.Exists(File_Path))
                {

                    #region 创建新文件并且写下Log
                    FileStream fs = new FileStream(File_Path, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    try
                    {
                        sw.WriteLine("{0} {1}", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), ExceptLog);
                        sw.Flush();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        if (sw != null)
                        {
                            sw.Close();
                        }
                    }

                    #endregion
                }
                else
                {
                    File.Delete(File_Path);
                    #region 创建新文件并且写下Log
                    FileStream fs = new FileStream(File_Path, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    try
                    {
                        sw.WriteLine("{0} {1}", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), ExceptLog);
                        sw.Flush();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    finally
                    {
                        if (sw != null)
                        {
                            sw.Close();
                        }
                    }

                    #endregion

                }
            }
            catch (Exception E)
            {

            }

        }
        public  void GetSingleOracleData(string sql,string GetKey, out List<string> RetureList)
        {
            List<string> tempList = new List<string>();
            try
                {

            using (OracleConnection conn = new OracleConnection(OracleLink))
            {
                
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    OracleCommand cmd = new OracleCommand(sql, conn);
                    OracleDataReader myreader = null;
                    myreader = cmd.ExecuteReader();
                    while (myreader.Read())
                    {
                        tempList.Add(myreader[GetKey].ToString().Trim());
                    }
                    conn.Close();
            }

                }
            catch (Exception exForMis)
            {

            }
            RetureList = tempList;
        }
       
     

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="strOleDbConntion">数据库链接字符串</param>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="strDbName">获取数据的字段名称</param>
        /// <returns></returns>
        public string GetSingleData(string strSQL, string strOleDbConntion, string strDbName)
        {
            string strResult = "";
            OleDbConnection AccessConntion = new OleDbConnection(strOleDbConntion);
            try
            {
                AccessConntion.Open();
                OleDbCommand ccmd = new OleDbCommand(strSQL, AccessConntion);
                OleDbDataReader red = ccmd.ExecuteReader();
                if (red.Read() == true)
                {
                    strResult = red[strDbName].ToString().Trim();
                }
                red.Close();
                AccessConntion.Close();
                AccessConntion.Dispose();
            }
            catch { }
            finally
            {
                AccessConntion.Close();
            }
            return strResult;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="strOleDbConntion">数据库链接字符串</param>
        /// <returns>返回第一个字段</returns>
        public string GetSingleData(string strSQL, string strOleDbConntion)
        {
            string strResult = "";
            OleDbConnection AccessConntion = new OleDbConnection(strOleDbConntion);
            try
            {
                AccessConntion.Open();
                OleDbCommand ccmd = new OleDbCommand(strSQL, AccessConntion);
                OleDbDataReader red = ccmd.ExecuteReader();
                if (red.Read() == true)
                {
                    strResult = red[0].ToString().Trim();
                }
                red.Close();
                AccessConntion.Close();
                AccessConntion.Dispose();
            }
            catch { }
            finally
            {
                AccessConntion.Close();
            }
            return strResult;
        }

        /// <summary>
        /// 误差结论遍历
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="strOleDbConntion">数据库链接字符串</param>
        /// <returns>返回第一个字段</returns>
        public string CheckErrorData(string strSQL, string strOleDbConntion)
        {
            string strResult = "未检";
            OleDbConnection AccessConntion = new OleDbConnection(strOleDbConntion);
            try
            {
               
                AccessConntion.Open();
                OleDbCommand ccmd = new OleDbCommand(strSQL, AccessConntion);
                OleDbDataReader red = null;
                red = ccmd.ExecuteReader();
                while (red.Read())
                {
                    strResult = "合格";
                    if (red[0].ToString().Trim().Contains("不"))
                    {
                        strResult = "不合格";
                        break;
                    }
                    
                }
                red.Close();
                AccessConntion.Close();
                AccessConntion.Dispose();
            }
            catch { }
            finally
            {
                AccessConntion.Close();
            }
            return strResult;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="strOleDbConntion">数据库链接字符串</param>
        /// <returns>返回第一个字段</returns>
        public string GetSingleData_SqlServer(string strSQL, string strOleDbConntion)
        {
            string strResult = "";
            SqlConnection AccessConntion = new SqlConnection(strOleDbConntion);
            try
            {
                AccessConntion.Open();
                SqlCommand ccmd = new SqlCommand(strSQL, AccessConntion);
                SqlDataReader red = ccmd.ExecuteReader();
                if (red.Read() == true)
                {
                    strResult = red[0].ToString().Trim();
                }
                red.Close();
                AccessConntion.Close();
                AccessConntion.Dispose();
            }
            catch { }
            finally
            {
                AccessConntion.Close();
            }
            return strResult;
        }

        public static int ExcuteToOracle(List<string> sql, out string ExceptionStr)
        {
            int result = 0;
            string OracleLink = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "OracleLink", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
            OracleConnection conn = null;
            string ERRORsql = "";
            try
            {
                using (conn = new OracleConnection(OracleLink))
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    foreach (String temp in sql)
                    {
                        ERRORsql = temp;
                        OracleCommand cmd = new OracleCommand(temp, conn);

                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();

                }
                ExceptionStr = "";
                return result;
            }
            catch (Exception e)
            {
                PublicFunction.WriteLog(ERRORsql.ToString(), DataCore.Global.GB_Base.LogPath + @"\Log.txt");
                PublicFunction.WriteLog(e.ToString(), DataCore.Global.GB_Base.LogPath + @"\Log.txt");

                ExceptionStr = e.Message.ToString();
                return -1;
            }
            finally
            {
                conn.Close();
            }
            ExceptionStr = "";
            return result;
        }
       
    }
}
