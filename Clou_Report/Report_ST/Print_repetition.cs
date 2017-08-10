using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DataCore.ReportModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data.OleDb;
using System.Data;
using NPOI.SS.Util;
namespace Clou_Report.Report_ST
{
    public class Print_repetition
    {
        public static bool Print(ObservableCollection<ST_Repetition> WcInfo)
        {
            bool result=true;
            try
            {
                foreach (ST_Repetition temp in WcInfo)
                {
                    string FileName = temp.AVR_ASSET_NO.ToString().Trim();
                    FileName=DataCore.Global.GB_Base.SaveReportPath + @"\"+DateTime.Now.ToString("yyyyMMdd");
                    HSSFWorkbook workbook = new HSSFWorkbook();
                    ISheet sheet = workbook.CreateSheet("重复性试验");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0,0,0,12));
                    #region 居中style
                    ICellStyle style = workbook.CreateCellStyle();
                    style.VerticalAlignment = VerticalAlignment.Center;
                    style.Alignment = HorizontalAlignment.Center;
                  
                    #endregion
                    IRow TitleRow = sheet.CreateRow(0);
                    ICell icell = TitleRow.CreateCell(0);
                    icell.SetCellValue("重复性试验"+temp.DTM_TEST_DATE);
                    icell.CellStyle = style;

                    #region 组合被插入值
                    List<string> FristKey = new List<string>();
                    List<string> FristValue = new List<string>();
                    FristKey.Add("资产编号：");
                    FristKey.Add("生产厂家：");
                    FristKey.Add("表型号：");
                   
                    FristValue.Add(temp.AVR_ASSET_NO);
                    FristValue.Add(temp.AVR_FACTORY);
                    FristValue.Add(temp.AVR_METER_MODEL);
                   
                   
                    IRow First = sheet.CreateRow(1);
                    for (int First_int = 0; First_int < 3; First_int++)
                    {
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 1 + First_int * 5, 4 + First_int * 5));
                        ICell Cell_Tile_key = First.CreateCell(0 + First_int * 5);
                        ICell Cell_Tile_Value = First.CreateCell(1 + First_int * 5);
                        Cell_Tile_key.SetCellValue(FristKey[First_int]);
                        Cell_Tile_Value.SetCellValue(FristValue[First_int]);
                    }
                    FristKey.Clear();
                    FristValue.Clear();
                    FristKey.Add("表名称：");
                    FristKey.Add("等级：");
                    FristKey.Add("常数");
                    FristValue.Add(temp.AVR_METER_NAME);
                    FristValue.Add(temp.AVR_AR_CLASS);
                    FristValue.Add(temp.AVR_AR_CONSTANT);
                    First = sheet.CreateRow(2);
                    for (int First_int = 0; First_int < 3; First_int++)
                    {
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 1 + First_int * 5, 4 + First_int * 5));
                        ICell Cell_Tile_key = First.CreateCell(0 + First_int * 5);
                        ICell Cell_Tile_Value = First.CreateCell(1 + First_int * 5);
                        Cell_Tile_key.SetCellValue(FristKey[First_int]);
                        Cell_Tile_Value.SetCellValue(FristValue[First_int]);
                    }
                    FristKey.Clear();
                    FristValue.Clear();
                    FristKey.Add("电压：");
                    FristKey.Add("电流：");
                    FristKey.Add("检定结果");
                    FristValue.Add(temp.AVR_UB);
                    FristValue.Add(temp.AVR_IB);
                    FristValue.Add(temp.AVR_TOTAL_CONCLUSION);
                    First = sheet.CreateRow(3);
                    for (int First_int = 0; First_int < 3; First_int++)
                    {
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 1 + First_int * 5, 4 + First_int * 5));
                        ICell Cell_Tile_key = First.CreateCell(0 + First_int * 5);
                        ICell Cell_Tile_Value = First.CreateCell(1 + First_int * 5);
                        Cell_Tile_key.SetCellValue(FristKey[First_int]);
                        Cell_Tile_Value.SetCellValue(FristValue[First_int]);
                    }
                    FristKey.Clear();
                    FristValue.Clear();
                    FristKey.Add("检定员：");
                    FristKey.Add("核验员：");
                    FristKey.Add("");
                    FristValue.Add(temp.AVR_TEST_PERSON);
                    FristValue.Add(temp.AVR_AUDIT_PERSON);
                    FristValue.Add("");
                    First = sheet.CreateRow(4);
                    for (int First_int = 0; First_int < 3; First_int++)
                    {
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 1 + First_int * 5, 4 + First_int * 5));
                        ICell Cell_Tile_key = First.CreateCell(0 + First_int * 5);
                        ICell Cell_Tile_Value = First.CreateCell(1 + First_int * 5);
                        Cell_Tile_key.SetCellValue(FristKey[First_int]);
                        Cell_Tile_Value.SetCellValue(FristValue[First_int]);
                    }
                    #endregion
                    IRow dataRow = sheet.CreateRow(5);
                    IRow RowError = sheet.CreateRow(6);
                    #region 列头
                    List<string> ColumnName = new List<string>();
                    ColumnName.Add("误差点");
                    ColumnName.Add("误差1");
                    ColumnName.Add("误差2");
                    ColumnName.Add("误差3");
                    ColumnName.Add("误差4");
                    ColumnName.Add("误差5");
                    ColumnName.Add("误差6");
                    ColumnName.Add("误差7");
                    ColumnName.Add("误差8");
                    ColumnName.Add("误差9");
                    ColumnName.Add("误差10");
                    ColumnName.Add("误差平均值");
                    ColumnName.Add("误差化整值");
                    #endregion
                    for (int i = 0; i < ColumnName.Count; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(ColumnName[i]);

                    }
                    #region 添加内容
                    RowError.CreateCell(0).SetCellValue(temp.ErrorNama);
                    RowError.CreateCell(1).SetCellValue(temp.Wc_001);
                    RowError.CreateCell(2).SetCellValue(temp.Wc_002);
                    RowError.CreateCell(3).SetCellValue(temp.Wc_003);
                    RowError.CreateCell(4).SetCellValue(temp.Wc_004);
                    RowError.CreateCell(5).SetCellValue(temp.Wc_005);
                    RowError.CreateCell(6).SetCellValue(temp.Wc_006);
                    RowError.CreateCell(7).SetCellValue(temp.Wc_007);
                    RowError.CreateCell(8).SetCellValue(temp.Wc_008);
                    RowError.CreateCell(9).SetCellValue(temp.Wc_009);
                    RowError.CreateCell(10).SetCellValue(temp.Wc_010);
                    RowError.CreateCell(11).SetCellValue(temp.Wc_pjz);
                    RowError.CreateCell(12).SetCellValue(temp.Wc_HZZ);
                    #endregion

                    for (int i = 0; i <= ColumnName.Count; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                    using (MemoryStream ms = new MemoryStream())
                    {
                        if (!Directory.Exists(FileName))
                        {
                            Directory.CreateDirectory(FileName);
                        }
                        using (FileStream fs = new FileStream(FileName + @"\" + temp.AVR_ASSET_NO.ToString().Trim() + ".xls", FileMode.Create, FileAccess.Write))
                        {
                            DataCore.Global.GB_Base.SaveExcel = FileName + @"\" + temp.AVR_ASSET_NO.ToString().Trim() + ".xls";
                            workbook.Write(fs);
                        }
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static ST_Repetition GetCL3000SData(string MeterID)
        {
            ST_Repetition repetition = new ST_Repetition();
            using (OleDbConnection conn = new OleDbConnection(DataCore.Global.GB_Base.AccessLink))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                string sql = "select * from METER_INFO where 1=1 and  PK_LNG_METER_ID='" + MeterID + "'";
                OleDbCommand cmd = new OleDbCommand(sql, conn);
                OleDbDataReader myreader = null;
                myreader = cmd.ExecuteReader();

                while (myreader.Read())
                {
                    repetition.AVR_ASSET_NO = myreader["AVR_ASSET_NO"].ToString().Trim();
                    repetition.AVR_FACTORY = myreader["AVR_FACTORY"].ToString().Trim();
                    repetition.AVR_METER_MODEL = myreader["AVR_METER_MODEL"].ToString().Trim();
                    repetition.AVR_AR_CONSTANT = myreader["AVR_AR_CONSTANT"].ToString().Trim();
                    repetition.AVR_AR_CLASS = myreader["AVR_AR_CLASS"].ToString().Trim();
                    repetition.AVR_METER_NAME = myreader["AVR_METER_NAME"].ToString().Trim();
                    repetition.AVR_UB = myreader["AVR_UB"].ToString().Trim();
                    repetition.AVR_IB = myreader["AVR_IB"].ToString().Trim();
                    repetition.DTM_TEST_DATE = myreader["DTM_TEST_DATE"].ToString().Trim();
                    repetition.AVR_TEMPERATURE = myreader["AVR_TEMPERATURE"].ToString().Trim();
                    repetition.AVR_TOTAL_CONCLUSION = myreader["AVR_TOTAL_CONCLUSION"].ToString().Trim();
                    repetition.AVR_TEST_PERSON = myreader["AVR_TEST_PERSON"].ToString().Trim();
                    repetition.AVR_AUDIT_PERSON = myreader["AVR_AUDIT_PERSON"].ToString().Trim();
                }

                sql = "select * from METER_ERROR where 1=1 and  FK_LNG_METER_ID='" + MeterID + "'";
                conn.Close();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd.CommandText = sql;
                myreader = null;
                myreader = cmd.ExecuteReader();
                string ErrorName = "";
                string ErrorH = "",ErrorCol="";
                string[] Error;
                while (myreader.Read())
                {
                    ErrorH = Get_GLFXDM(myreader["CHR_POWER_TYPE"].ToString().Trim());
                    ErrorName = ErrorH + " " + myreader["AVR_IB_MULTIPLE"].ToString().Trim() + " " + myreader["AVR_POWER_FACTOR"].ToString().Trim();
                    char[] split = { '|' };
                    Error = myreader["AVR_ERROR_MORE"].ToString().Trim().Split(split);
                    repetition.Wc_HZZ = Error[Error.Length - 1];
                    repetition.Wc_pjz = Error[Error.Length - 2];
                    repetition.ErrorNama = ErrorName;
                    repetition.Wc_001 = 0 > Error.Length-3 ? "" : Error[0];
                    repetition.Wc_002 = 1 > Error.Length-3? "" : Error[1];
                    repetition.Wc_003 = 2 > Error.Length -3  ? "" : Error[2];
                    repetition.Wc_004 = 3 > Error.Length - 3 ? "" : Error[3];
                    repetition.Wc_005 = 4 > Error.Length - 3 ? "" : Error[4];
                    repetition.Wc_006 = 5 > Error.Length - 3 ? "" : Error[5];
                    repetition.Wc_007 = 6 > Error.Length - 3 ? "" : Error[6];
                    repetition.Wc_008 = 7 > Error.Length - 3 ? "" : Error[7];
                    repetition.Wc_009 = 8 > Error.Length - 3 ? "" : Error[8];
                    repetition.Wc_010 = 9 > Error.Length - 3 ? "" : Error[9];
                }

            }
            return repetition;

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
                    strResults = "正向有功";
                    break;
                case "1":
                    strResults = "反向有功";
                    break;
                case "2":
                    strResults = "正向无功";
                    break;
                case "3":
                    strResults = "反向无功";
                    break;
                case "4":
                    strResults = "无功第一象限";
                    break;
                case "5":
                    strResults = "无功第二象限";
                    break;
                case "6":
                    strResults = "无功第三象限";
                    break;
                case "7":
                    strResults = "无功第四象限";
                    break;
            }

            return strResults;
        }
    }
}
