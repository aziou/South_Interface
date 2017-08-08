using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace OperateData
{
    public class MakeData
    {
        public static string str_DQBM = OperateData.FunctionXml.ReadElement("NewUser/CloumMIS/Item", "Name", "txt_CompanyNum", "Value", "", System.AppDomain.CurrentDomain.BaseDirectory + @"\config\NewBaseInfo.xml");
        public  ObservableCollection<DataCore.MeterErrorCol> FakeErrorData()
        {
            ObservableCollection<DataCore.MeterErrorCol> tempCol = new ObservableCollection<DataCore.MeterErrorCol>();

            try
            {
                #region H
                AddColError(ref tempCol, "2", "1", "06", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "06", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "3", "06", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "1", "07", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "07", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "3", "07", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "1", "05", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "05", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "3", "05", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "1", "03", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "1", "02", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "02", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "01", "02", "1", "01", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                #endregion

                #region A
                AddColError(ref tempCol, "2", "1", "06", "02", "2", "02", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "06", "02", "2", "02", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "1", "05", "02", "2", "02", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "05", "02", "2", "02", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "1", "03", "02", "2", "02", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "02", "02", "2", "02", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                #endregion

                #region B
                AddColError(ref tempCol, "2", "1", "06", "02", "3", "03", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "06", "02", "3", "03", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "1", "05", "02", "3", "03", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "05", "02", "3", "03", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "1", "03", "02", "3", "03", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "02", "02", "3", "03", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                #endregion

                #region C
                AddColError(ref tempCol, "2", "1", "06", "02", "4", "04", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "06", "02", "4", "04", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "1", "05", "02", "4", "04", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "05", "02", "4", "04", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "1", "03", "02", "4", "04", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                AddColError(ref tempCol, "2", "2", "02", "02", "4", "04", "0.13890", "0.16030", "", "", "", "+0.1496", "+0.2", "Y", "", "", str_DQBM);
                #endregion


            }
            catch
            {

            }
            return tempCol;
        }

        private void AddColError(ref  ObservableCollection<DataCore.MeterErrorCol> COL,
                                    string glfx, string glys, string fzdl, string xbdm, string fzlxdm, string fydm,
                                    string wc1, string wc2, string wc3, string wc4, string wc5,
                                    string errorAvr, string errorXyz, string result, string errorCzXyz, string Xerror,
                                    string Dqbm)
        {
            try
            {
                COL.Add(new DataCore.MeterErrorCol()
                {
                    StrGLFXDM=glfx,
                    StrGLYSDM=glys,
                    StrFZDLDM=fzdl,
                    StrXBDM=xbdm,
                    StrFZLXDM=fzlxdm,
                    StrFYDM=fydm,
                    StrWC1=wc1,
                    StrWC2=wc2,
                    StrWC3=wc3,
                    StrWC4=wc4,
                    StrWC5=wc5,
                    StrWCPJZ=errorAvr,
                    StrXYZ=errorXyz,
                    StrJLDM=result,
                    StrWCCZXYZ=errorCzXyz,
                    StrWCCZ=Xerror,
                    StrDQMB=Dqbm,

                });
            }
            catch
            { 
            
            }

        }
    

    }
}
