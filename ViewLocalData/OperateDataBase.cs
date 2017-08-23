using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace ViewLocalData
{
    public class OperateDataBase
    {
        public static DataTable OutDataTable(string TableName, string strSql, out List<string> MeterIdList)
        {
            List<string> meterID = new List<string>();
            List<string> InsertSql = new List<string>();
            string str_Key, str_Value;
            using (OleDbConnection conn = new OleDbConnection(DataCore.Global.GB_Base.AccessLink))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(strSql, conn))
                {
                    try
                    {

                        DataTable dtable = new DataTable();
                        adapter.Fill(dtable);
                        Random rd = new Random();
                        int randomNum = rd.Next(0, 1000);
                        for (int i = 0; i < dtable.Rows.Count; i++)
                        {
                            dtable.Rows[i][0] = "123456" + DateTime.Now.ToString("yyyyMMddHH") + randomNum.ToString() + i.ToString("D2");
                            meterID.Add("123456" + DateTime.Now.ToString("yyyyMMddHH") + randomNum.ToString() + i.ToString("D2"));
                        }
                        //for (int i = 0; i < dtable.Rows.Count; i++)
                        //{
                            
                        //    //InsertSql.Add();
                        //}
                        MeterIdList = meterID;
                        return dtable;
                    }
                    catch (Exception exError)
                    {
                        DataTable dtable = new DataTable();
                        MeterIdList = meterID;
                        return dtable;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }
        public static DataTable OutOtherDataTable(string TableName, string strSql,string MeterIdNum)
        {
            List<string> meterID = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(DataCore.Global.GB_Base.AccessLink))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(strSql, conn))
                {
                    try
                    {

                        DataTable dtable = new DataTable();
                        adapter.Fill(dtable);
                        Random rd=new Random();
                        int randomNum=rd.Next(0,1000);
                        for (int i = 0; i < dtable.Rows.Count; i++)
                        {
                            dtable.Rows[i][0] = MeterIdNum;
                            
                        }
                      
                            return dtable;
                    }
                    catch (Exception exError)
                    {
                        DataTable dtable = new DataTable();
                       
                        return dtable;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }

        public  List<string> GetDataBaseTable()
        {
            DataTable TableName;
            List<string> TableList = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(DataCore.Global.GB_Base.AccessLink))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                TableName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                foreach (DataRow row in TableName.Rows)
                {
                   
                    TableList.Add(row["TABLE_NAME"].ToString());
                }
            }

            return TableList;

        }

        public  int MakeInserSql(List<string> TableNameList,string meterId)
        {
            try
            {
                DataTable TableStruct;
                string sqlKey="", sqlValue="",InserSQL="",IdColumnName="";
                List<string> InsertList = new List<string>();
                using (OleDbConnection conn = new OleDbConnection(DataCore.Global.GB_Base.AccessLink))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    foreach (string tempName in TableNameList)
                    {
                        TableStruct = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null,tempName,null });
                        foreach (DataRow temprow in TableStruct.Rows)
                        {
                            if (temprow["ORDINAL_POSITION"].ToString() == "1")
                            {
                                IdColumnName = temprow["COLUMN_NAME"].ToString().Trim();
                                break;
                            }
                        }
                        string sql = "select * from " + tempName + " where " + IdColumnName + "='" + meterId + "'";

                        OleDbCommand cmd = new OleDbCommand(sql, conn);

                        OleDbDataReader myreader = null;

                        myreader = cmd.ExecuteReader();
                        InsertList.Clear();
                        while (myreader.Read())
                        {
                            sqlKey = ""; sqlValue = ""; InserSQL = "";
                            foreach (DataRow temprow in TableStruct.Rows)
                            {
                                makeSql(ref sqlKey, ref sqlValue, temprow["COLUMN_NAME"].ToString(), myreader[temprow["COLUMN_NAME"].ToString()].ToString().Trim());
                            }
                            sqlKey = "("+sqlKey.Substring(1, sqlKey.Length - 1)+")";
                            sqlValue = "("+sqlValue.Substring(1, sqlValue.Length - 1)+")";
                            InserSQL = "Insert into " + "TMP_"+tempName +sqlKey+ " Values " + sqlValue;

                            InsertList.Add(InserSQL);

                        }
                        OperateData.PublicFunction.ExcuteAccess(InsertList);
                       
                    }
                   
                }
            }
            catch
            {
            }
            finally
            {
                
            }
            return 0;
        }

        public int MakeInserSql(List<string> TableNameList, string meterId,string NewMeterId)
        {
            try
            {
                DataTable TableStruct;
                string sqlKey = "", sqlValue = "", InserSQL = "", IdColumnName = "";
                List<string> InsertList = new List<string>();
                using (OleDbConnection conn = new OleDbConnection(DataCore.Global.GB_Base.AccessLink))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    foreach (string tempName in TableNameList)
                    {
                        TableStruct = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, null, tempName, null });
                        foreach (DataRow temprow in TableStruct.Rows)
                        {
                            if (temprow["ORDINAL_POSITION"].ToString() == "1")
                            {
                                IdColumnName = temprow["COLUMN_NAME"].ToString().Trim();
                                break;
                            }
                        }
                        string sql = "select * from " + tempName + " where " + IdColumnName + "='" + meterId + "'";

                        OleDbCommand cmd = new OleDbCommand(sql, conn);

                        OleDbDataReader myreader = null;

                        myreader = cmd.ExecuteReader();
                        InsertList.Clear();
                        while (myreader.Read())
                        {
                            sqlKey = ""; sqlValue = ""; InserSQL = "";
                            foreach (DataRow temprow in TableStruct.Rows)
                            {
                                if (temprow["ORDINAL_POSITION"].ToString() == "1")
                                {
                                    makeSql(ref sqlKey, ref sqlValue, temprow["COLUMN_NAME"].ToString(), NewMeterId);
                           
                                    continue;
                                }
                                makeSql(ref sqlKey, ref sqlValue, temprow["COLUMN_NAME"].ToString(), myreader[temprow["COLUMN_NAME"].ToString()].ToString().Trim());
                            }
                            sqlKey = "(" + sqlKey.Substring(1, sqlKey.Length - 1) + ")";
                            sqlValue = "(" + sqlValue.Substring(1, sqlValue.Length - 1) + ")";
                            InserSQL = "Insert into " + "TMP_" + tempName + sqlKey + " Values " + sqlValue;

                            InsertList.Add(InserSQL);

                        }
                        OperateData.PublicFunction.ExcuteAccess(InsertList);

                    }

                }
            }
            catch
            {
            }
            finally
            {

            }
            return 0;
        }

        public void makeSql(ref string str_key, ref string str_value, string dataKey, string dataValue)
        {
            str_key = str_key + "," + dataKey;
            str_value = str_value + "," + "'" + dataValue + "'"; 
        }

        public  void Test()
        {
            List<string> listName = new List<string>();
            listName = GetDataBaseTable();
            MakeInserSql(listName,"123");
        }
        public static void InsertDataToTmp(DataTable FillTable, string InsertTableName,string strSql)
        {
            string TmpDataPath = DataCore.Global.GB_Base.AccessLink.Substring(0, DataCore.Global.GB_Base.AccessLink.LastIndexOf(@"\")) + @"\ClouMeterDataTmp.mdb";
            string Sql_word_1 = "Provider=Microsoft.ACE.OleDb.12.0;Data Source=";
            string Sql_word_2 = ";Persist Security Info=False";
            DataTable MeterTable=new DataTable ();
            using (OleDbConnection conn = new OleDbConnection( TmpDataPath + Sql_word_2))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                string Sql = string.Format("select * from {0} where 1=1", "TMP_" + InsertTableName);
                //string Sql = string.Format("Insert into {0} select * from OPENDATASOURCE ({1})...{2} ", "TMP_" + InsertTableName, @"'Microsoft.ACE.OleDb.12.0';'Data Source=D:\A_temp\CL3000S-H-NW-20170627\CL3000S-H-NW\Resource\Client\DataBase\ClouMeterDataTmp.mdb;Persist Security Info=False'", InsertTableName);
                OleDbCommand cmd = new OleDbCommand(Sql, conn);
                OleDbDataAdapter ada = new OleDbDataAdapter(Sql, conn);
                ada.AcceptChangesDuringFill = false;
                ada.Fill(MeterTable);
                MeterTable = FillTable;
                MeterTable.PrimaryKey = new DataColumn[] { MeterTable.Columns[MeterTable.Columns[0].ColumnName] };
                OleDbCommandBuilder Comb = new OleDbCommandBuilder(ada);
                ada.InsertCommand = Comb.GetInsertCommand();
                ada.Update(MeterTable);
                
                //using (SqlBulkCopy bulkCopy = new SqlBulkCopy( TmpDataPath + Sql_word_2))
                //{
                //    bulkCopy.DestinationTableName = InsertTableName;
                //    bulkCopy.BatchSize = FillTable.Rows.Count;
                //    for (int i = 0; i < FillTable.Columns.Count; i++)
                //    {
                //        bulkCopy.ColumnMappings.Add(FillTable.Columns[i].ColumnName, FillTable.Columns[i].ColumnName);
                //    }
                //    bulkCopy.WriteToServer(FillTable);

                //}
           
            }
        }

        public static void DeleteTmpData(List<string> Tmp)
        { 
            string TmpDataPath = DataCore.Global.GB_Base.AccessLink.Substring(0, DataCore.Global.GB_Base.AccessLink.LastIndexOf(@"\")) + @"\ClouMeterDataTmp.mdb";
            string Sql_word_1 = "Provider=Microsoft.ACE.OleDb.12.0;Data Source=";
            string Sql_word_2 = ";Persist Security Info=False";

            using (OleDbConnection conn = new OleDbConnection(Sql_word_1 + TmpDataPath + Sql_word_2))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    foreach (string temp in Tmp)
                    {
                        if (temp == "METER_INFO")
                        {
                            continue;
                        }
                        string sql = "delete from " + "TMP_"+temp + " where 1=1";
                        OleDbCommand cmd = new OleDbCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception exError)
                {

                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }


            }
        }

        public static void DeleteTmpBaseInfo(List<string> lis_bnum)
        {
            string TmpDataPath = DataCore.Global.GB_Base.AccessLink.Substring(0, DataCore.Global.GB_Base.AccessLink.LastIndexOf(@"\")) + @"\ClouMeterDataTmp.mdb";
            string Sql_word_1 = "Provider=Microsoft.ACE.OleDb.12.0;Data Source=";
            string Sql_word_2 = ";Persist Security Info=False";

            using (OleDbConnection conn = new OleDbConnection(Sql_word_1 + TmpDataPath + Sql_word_2))
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    foreach (string temp in lis_bnum)
                    {

                        string sql = "delete from " + "TMP_METER_INFO  where LNG_BENCH_POINT_NO="+temp+"";
                        OleDbCommand cmd = new OleDbCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception exError)
                {

                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }


            }
        }

        public static void TransDataToTmpDatabase(List<string> AllMeterNumber,List<string> TableList)
        {
            List<string> NewMeterId = new List<string>();
            string strSql="";
            bool MeterInfoTable = true;
            DataTable meterDataTable = new DataTable();
            int intMeterNum = 0;
            foreach (string tempNum in AllMeterNumber)
            {
                foreach (string tempTable in TableList)
                {
                    if (MeterInfoTable)
                    {
                        strSql = string.Format("select * from {0} where PK_LNG_METER_ID = '{1}'", tempTable, tempNum);
                        meterDataTable = OutDataTable(tempTable, strSql, out NewMeterId);
                        InsertDataToTmp(meterDataTable, tempTable, "");
                        MeterInfoTable = false;
                    }
                    else
                    {
                        strSql = string.Format("select * from {0}where FK_LNG_METER_ID = '{1}'", tempTable, tempNum);
                        meterDataTable = OutOtherDataTable(tempTable, strSql, NewMeterId[intMeterNum]);
                        InsertDataToTmp(meterDataTable, tempTable, "");
                    }                  
                }
                intMeterNum = 0;
                MeterInfoTable = true;
            }
        }

        public static bool MultiInsertData(DataTable ds, string Columns, string tableName)
        {
            using (OleDbConnection connection = new OleDbConnection(""))
            {
                string SQLString = string.Format("select {0} from {1} where rownum=0", Columns, tableName);
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        OleDbDataAdapter myDataAdapter = new OleDbDataAdapter();
                        myDataAdapter.SelectCommand = new OleDbCommand(SQLString, connection);
                        myDataAdapter.UpdateBatchSize = 0;
                        OleDbCommandBuilder custCB = new OleDbCommandBuilder(myDataAdapter);
                        DataTable dt = ds;
                        DataTable dtTemp = dt.Clone();

                        int times = 0;
                        for (int count = 0; count < dt.Rows.Count; times++)
                        {
                            for (int i = 0; i < 400 && 400 * times + i < dt.Rows.Count; i++, count++)
                            {
                                dtTemp.Rows.Add(dt.Rows[count].ItemArray);
                            }
                            myDataAdapter.Update(dtTemp);
                            dtTemp.Rows.Clear();
                        }

                        dt.Dispose();
                        dtTemp.Dispose();
                        myDataAdapter.Dispose();
                        return true;
                    }
                    catch (Exception e)
                    {
                        connection.Close();
                        return false;
                    }
                }
            }
        }
    }
}
