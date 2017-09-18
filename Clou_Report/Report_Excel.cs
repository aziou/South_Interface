using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.ObjectModel;
using Clou_Report.Model;
namespace Clou_Report
{
    public class Report_Excel
    {
        public  void OutputExcel(object o)
        {
            LocalBaseInfo TempLocalBaseInfo = o as LocalBaseInfo;
           
        }
        public readonly string BaseConfigPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml";
        public string FileName = DataCore.Global.GB_Base.MeterCheckName + "_" + DataCore.Global.GB_Base.MeterCheckTime;

        public  void OutputExcel(List<string> Meter_zcbh, List<string> Meter_seal_1, List<string> Meter_seal_2, List<string> Meter_seal_3,string MeterCheckTime,string MeterGZDBH,string MeterTestPerson)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                HSSFWorkbook wk = new HSSFWorkbook();
                ISheet TableEx = wk.CreateSheet("电表铅封信息");
                IRow tbRow_0 = TableEx.CreateRow(0);
                TableEx.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 2));
                ICell cell = tbRow_0.CreateCell(0);
                cell.SetCellValue("工作单号：");
                cell = tbRow_0.CreateCell(1);
                cell.SetCellValue(MeterGZDBH);
                cell = tbRow_0.CreateCell(3);
                cell.SetCellValue("操作员：");
                cell = tbRow_0.CreateCell(4);
                cell.SetCellValue(MeterTestPerson);
                ICellStyle styleCenter = wk.CreateCellStyle();
                styleCenter.Alignment = HorizontalAlignment.Center;
                cell.CellStyle = styleCenter;
                #region row 2
                tbRow_0 = TableEx.CreateRow(1);
                cell = tbRow_0.CreateCell(0);
                cell.SetCellValue("设备总数：");
                cell = tbRow_0.CreateCell(1);
                cell.SetCellValue(Meter_zcbh.Count);
                #endregion
                #region row 3
                tbRow_0 = TableEx.CreateRow(2);
                cell = tbRow_0.CreateCell(0);
                cell.SetCellValue("操作时间：");
                cell = tbRow_0.CreateCell(1);
                cell.SetCellValue(MeterCheckTime);
                #endregion
                
                IRow tbRow = TableEx.CreateRow(3);
                #region 列头
                List<string> ColName = new List<string>();
                ColName.Add("序号");
                ColName.Add("资产编号");
                ColName.Add("左耳封印");
                ColName.Add("右耳封印");
                ColName.Add("编程封印");
                for (int col = 0; col < 5; col++)
                {
                    ICell CellCol = tbRow.CreateCell(col);
                    CellCol.SetCellValue(ColName[col]);
                    CellCol.CellStyle = styleCenter;
                }
                #endregion

                for (int row = 0; row < Meter_zcbh.Count; row++)
                {
                    IRow MeterInfoRow = TableEx.CreateRow(4 + row);
                    ICell InfoCell = MeterInfoRow.CreateCell(0);
                    InfoCell.SetCellValue((row+1).ToString());
                    InfoCell.CellStyle = styleCenter;
                    InfoCell = MeterInfoRow.CreateCell(1);
                    InfoCell.SetCellValue(Meter_zcbh[row]);

                    if (Meter_seal_1.Count > 0)
                    {
                        InfoCell = MeterInfoRow.CreateCell(2);
                        InfoCell.SetCellValue(Meter_seal_1[row]);
                    }
                    if (Meter_seal_2.Count > 0)
                    {
                        InfoCell = MeterInfoRow.CreateCell(3);
                        InfoCell.SetCellValue(Meter_seal_2[row]);
                    }

                    if (Meter_seal_3.Count > 0)
                    {
                        InfoCell = MeterInfoRow.CreateCell(4);
                        InfoCell.SetCellValue(Meter_seal_3[row]);
                    }
                }

                for (int i = 0; i <= 5; i++)
                {
                    TableEx.AutoSizeColumn(i);
                }
                string CheckTime = Convert.ToDateTime(MeterCheckTime).ToString("yyyy年MM月dd日HH时mm分");
                string strFileName = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_Report", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml") + @"\" + CheckTime+"_"+MeterTestPerson+"_"+"工作单信息" + @".xls";


                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    wk.Write(fs);
                }
                DataCore.Global.GB_Base.ReportSuccess = true;

            }
            catch (Exception exSeal)
            {
                DataCore.Global.GB_Base.ReportSuccess = false;
            }
           
            //using (FileStream fs = File.OpenWrite(@"C:\Users\screw\Desktop\工作簿2.xls")) //打开一个xls文件，如果没有则自行创建，如果存在myxls.xls文件则在创建是不要打开该文件！
            //{
            //    wk.Write(fs);   //向打开的这个xls文件中写入mySheet表并保存。
            //    //MessageBox.Show("提示：创建成功！");
            //}
        }
        public static string str_Path = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_Report", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
        public void OutputExcelForZj(ObservableCollection<MemberForZJ> ColmemberforZj, List<string> ColunmName,bool Flag)
        {
            switch (OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "Cmb_Facory", "Value", "", BaseConfigPath))
            {
                case "科陆电子":
                    OutputExcelForZj(ColmemberforZj,ColunmName);
                    break;
                case "格宁":
                    OutputExcelForZj_GeNing(ColmemberforZj, ColunmName);
                    break;
                case "涵普":
                    break;
            }
        }
        public void OutputExcelForZj(ObservableCollection<MemberForZJ> ColmemberforZj,List<string> ColunmName)
        {

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("计量物资检定信息数据");

            //填充表头   
            IRow dataRow = sheet.CreateRow(0);
            int ColInt = 0;
            foreach (string Colname in ColunmName)
            {
                dataRow.CreateCell(ColInt).SetCellValue(Colname);
                ColInt++;
            }
            //填充内容   
            for (int i = 0; i < ColmemberforZj.Count; i++)
            {
                dataRow = sheet.CreateRow(i + 1);

                dataRow.CreateCell(0).SetCellValue(ColmemberforZj[i].StrZCBH.ToString());
                dataRow.CreateCell(1).SetCellValue(ColmemberforZj[i].StrEquipment.ToString());
                dataRow.CreateCell(2).SetCellValue(ColmemberforZj[i].StrEquipmentType.ToString());
                dataRow.CreateCell(3).SetCellValue(ColmemberforZj[i].StrEquipmentSize.ToString());
                dataRow.CreateCell(4).SetCellValue(ColmemberforZj[i].StrFactory.ToString());
                dataRow.CreateCell(5).SetCellValue(ColmemberforZj[i].StrElectric.ToString());
                dataRow.CreateCell(6).SetCellValue(ColmemberforZj[i].StrCheckTime.ToString());
                dataRow.CreateCell(7).SetCellValue(ColmemberforZj[i].StrResult.ToString());
                
            }
            for (int i = 0; i <= ColunmName.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            string FileName = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_equipment", "Value", "", BaseConfigPath) + "_" + Convert.ToDateTime(ColmemberforZj[0].StrCheckTime).ToString("yyyyMMdd_HH时mm分");
            //保存   
            using (MemoryStream ms = new MemoryStream())
            {
                if (!Directory.Exists(str_Path + @"\" +Convert.ToDateTime(ColmemberforZj[0].StrCheckTime).ToString("yyyyMMdd")))
                { 
                    Directory.CreateDirectory(str_Path + @"\" +Convert.ToDateTime(ColmemberforZj[0].StrCheckTime).ToString("yyyyMMdd"));
                 }
                using (FileStream fs = new FileStream(str_Path + @"\" + Convert.ToDateTime(ColmemberforZj[0].StrCheckTime).ToString("yyyyMMdd") + @"\" +
             FileName + @".xls", FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }
        }
        public void OutputExcelForZj_GeNing(ObservableCollection<MemberForZJ> ColmemberforZj, List<string> ColunmName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("计量物资检定信息数据");

            //填充表头   
            IRow dataRow = sheet.CreateRow(0);
            int ColInt = 0;
            foreach (string Colname in ColunmName)
            {
                dataRow.CreateCell(ColInt).SetCellValue(Colname);
                ColInt++;
            }
            //填充内容   
            for (int i = 0; i < ColmemberforZj.Count; i++)
            {
                dataRow = sheet.CreateRow(i + 1);

                dataRow.CreateCell(0).SetCellValue(ColmemberforZj[i].StrZCBH.ToString());
                dataRow.CreateCell(1).SetCellValue(ColmemberforZj[i].StrEquipment.ToString());
                dataRow.CreateCell(2).SetCellValue(ColmemberforZj[i].StrEquipmentType.ToString());
                dataRow.CreateCell(3).SetCellValue(ColmemberforZj[i].StrEquipmentSize.ToString());
                dataRow.CreateCell(4).SetCellValue(ColmemberforZj[i].StrFactory.ToString());
                dataRow.CreateCell(5).SetCellValue(ColmemberforZj[i].StrElectric.ToString());
                dataRow.CreateCell(6).SetCellValue(ColmemberforZj[i].StrCheckTime.ToString());
                dataRow.CreateCell(7).SetCellValue(ColmemberforZj[i].StrResult.ToString());

            }
            for (int i = 0; i <= ColunmName.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            string FileName = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_equipment", "Value", "", BaseConfigPath) + "_" + Convert.ToDateTime(ColmemberforZj[0].StrCheckTime).ToString("yyyyMMdd_HH时mm分");
            //保存   
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(str_Path + @"\" + FileName + @".xls", FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }
        }
        public void OutputExcelForZj_HanPu(ObservableCollection<MemberForZJ> ColmemberforZj, List<string> ColunmName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("计量物资检定信息数据");

            //填充表头   
            IRow dataRow = sheet.CreateRow(0);
            int ColInt = 0;
            foreach (string Colname in ColunmName)
            {
                dataRow.CreateCell(ColInt).SetCellValue(Colname);
                ColInt++;
            }
            //填充内容   
            for (int i = 0; i < ColmemberforZj.Count; i++)
            {
                dataRow = sheet.CreateRow(i + 1);

                dataRow.CreateCell(0).SetCellValue(ColmemberforZj[i].StrZCBH.ToString());
                dataRow.CreateCell(1).SetCellValue(ColmemberforZj[i].StrEquipment.ToString());
                dataRow.CreateCell(2).SetCellValue(ColmemberforZj[i].StrEquipmentType.ToString());
                dataRow.CreateCell(3).SetCellValue(ColmemberforZj[i].StrEquipmentSize.ToString());
                dataRow.CreateCell(4).SetCellValue(ColmemberforZj[i].StrFactory.ToString());
                dataRow.CreateCell(5).SetCellValue(ColmemberforZj[i].StrElectric.ToString());
                dataRow.CreateCell(6).SetCellValue(ColmemberforZj[i].StrCheckTime.ToString());
                dataRow.CreateCell(7).SetCellValue(ColmemberforZj[i].StrResult.ToString());

            }
            for (int i = 0; i <= ColunmName.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            string FileName = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_equipment", "Value", "", BaseConfigPath) + "_" + Convert.ToDateTime(ColmemberforZj[0].StrCheckTime).ToString("yyyyMMdd_HH时mm分");
            //保存   
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(str_Path + @"\" + FileName + @".xls", FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }
        }

        #region print the certificate 
        public void PrintExcelCertificate(List<string> zcbh, List<string> Jjrq,string fileIndex)
        {
            DataCore.Global.GB_Base.MeterCheckTime = Convert.ToDateTime(DataCore.Global.GB_Base.MeterCheckTime).ToString("yyyy年MM月dd日HH时mm分ss秒");
            FileName = DataCore.Global.GB_Base.MeterCheckName + "_" + DataCore.Global.GB_Base.MeterCheckTime;
            HSSFWorkbook workbook = new HSSFWorkbook();
            string SavePath = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_Report", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");

            using (FileStream fs = File.Open(SavePath+@"\"+DataCore.Global.GB_Base.ExcelReportName, FileMode.Open,
          FileAccess.Read, FileShare.ReadWrite))
            {
                
                workbook = new HSSFWorkbook(fs);
                fs.Close();
            }
            #region variable
            int cellStart = 0, cellEnd = 7, colNum = 1, interval=6,RowInterval=12;
            #endregion

            ISheet sheet = workbook.GetSheet("Sheet1");

            IRow dataRow;
            foreach (string temp in zcbh)
            {
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(cellStart, cellEnd, colNum, colNum));
                dataRow = sheet.GetRow(cellStart);
                
                dataRow.GetCell(colNum).SetCellValue(Convert.ToDateTime(Jjrq[0]).ToString("yyyy  MM  dd"));
                cellStart = cellStart + RowInterval;
                cellEnd = cellEnd + RowInterval;
                
                while (cellStart >= 60)
                {
                    sheet.AutoSizeColumn(colNum);
                    cellStart = 0; cellEnd = 7; colNum = colNum + interval; 
                }
            }
            #region variable
            cellStart = 0; cellEnd = 7; colNum = 2; interval = 6;
            #endregion
            foreach (string temp in Jjrq)
            {
                dataRow = sheet.GetRow(cellStart);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(cellStart, cellEnd, colNum, colNum));
                dataRow.GetCell(colNum ).SetCellValue(temp);
                cellStart = cellStart + RowInterval;
                cellEnd = cellEnd + RowInterval;
                while (cellStart >= 60)
                {
                    sheet.AutoSizeColumn(colNum);
                    cellStart = 0; cellEnd = 7; colNum = colNum + interval;
                }
            }


         

            //保存   
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(SavePath + @"\" + FileName+"_"+fileIndex + @".xls", FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                    DataCore.Global.GB_Base.ExcelResult = "success";
                }
            }

        }

        
        #endregion
    }
    public class LocalBaseInfo
    {
        private List<string> lis_MeterZcbh;
        public List<string> List_MeterZcbh
        { get; set; }

        private List<string> lis_Seal001;
        public List<string> List_Seal001
        { get; set; }

        private List<string> lis_Seal002;
        public List<string> List_Seal002
        { get; set; }

        private List<string> lis_Seal003;
        public List<string> List_Seal003
        { get; set; }

        private string strGZDBH;
        public string StrGZDBH
        { get; set; }

        private string strCheckMan;
        public string StrCheckMan
        { get; set; }

        private string meterCheck;
        public string MeterCheck
        { get; set; }
    }
}
