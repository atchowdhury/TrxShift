using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrxShift.DAO;

namespace TrxShift.DAL
{
    class ShiftManager
    {
        DBGateway aGateway = new DBGateway();
        public void TRXshifter(List<InputData> inputList)
        {
            int counter = inputList.Count;
            string queryVariable = "";
            string createLAPD = "";
            string createTRX = "";
            string deleteLAPD = "";
            string deleteTRX = "";
            foreach (InputData data in inputList)
            {
                counter--;
                if (counter > 0)
                {
                    queryVariable += "(trx.BSC=" + data.BSC + " and trx.lapdLinkName='" + data.LAPDName + 
                                     "') or";//" and trx.BCF=" + data.BCF + " and trx.TRX=" + data.TRX +
                }
                else
                {
                    queryVariable += "(trx.BSC=" + data.BSC + " and trx.lapdLinkName='" + data.LAPDName + 
                                     "')";
                }


            }
            string query = string.Format("select trx.bsc,trx.bcf,trx.bts,trx.trx,trx.plmn as trxPlmn, trx.name as trxName, " +
                                         "trx.adminState as trxAdminState,trx.channel0AdminState,trx.channel0Pcm," +
                                         "trx.channel0Subslot,trx.channel0Tsl,trx.channel0Type," +
                                         "trx.channel1AdminState,trx.channel1Pcm,trx.channel1Subslot," +
                                         "trx.channel1Tsl,trx.channel1Type,trx.channel2AdminState," +
                                         "trx.channel2Pcm,trx.channel2Subslot,trx.channel2Tsl,trx.channel2Type," +
                                         "trx.channel3AdminState,trx.channel3Pcm,trx.channel3Subslot,trx.channel3Tsl," +
                                         "trx.channel3Type,trx.channel4AdminState,trx.channel4Pcm,trx.channel4Subslot" +
                                         ",trx.channel4Tsl,trx.channel4Type,trx.channel5AdminState,trx.channel5Pcm," +
                                         "trx.channel5Subslot,trx.channel5Tsl,trx.channel5Type,trx.channel6AdminState," +
                                         "trx.channel6Pcm,trx.channel6Subslot,trx.channel6Tsl,trx.channel6Type," +
                                         "trx.channel7AdminState,trx.channel7Pcm,trx.channel7Subslot,trx.channel7Tsl," +
                                         "trx.channel7Type,trx.daPool_ID,trx.gprsEnabledTrx,trx.halfRateSupport," +
                                         "trx.initialFrequency,trx.lapdLinkName,trx.subslotsForSignalling,trx.tsc," +
                                         "trx.preferredBcchMark,lapd.plmn as lapdPlmn,lapd.bitRate,lapd.abisSigChannelTimeSlotPcm," +
                                         "lapd.abisSigChannelTimeSlotTsl,lapd.abisSigChannelSubSlot,lapd.adminState  as lapdAdminState,lapd.bsc,lapd.dChannelType," +
                                         "lapd.name as lapdName,lapd.parameterSetNumber,lapd.sapi,lapd.tei," +
                                         "lapd.logicalBCSUAddress from trx,lapd where ({0}) and trx.bsc=lapd.bsc and trx.lapdLinkName=lapd.name", queryVariable);
            DataSet aSet=aGateway.Select(query);

            List<string> cols = new List<string>();
            foreach (DataColumn dataColumn in aSet.Tables[0].Columns)
            {
                cols.Add(dataColumn.ColumnName);
            }

            foreach (DataRow dataRow in aSet.Tables[0].Rows)
            {
                //foreach (string col in cols)
                //{
                //    string s = dataRow[col].ToString();
                //}
                createLAPD = "<managedObject class=\"LAPD\" version=\"S15.3\" distName=\"" + dataRow[55].ToString() + "\" operation=\"create\">\n";
                deleteLAPD = "<managedObject class=\"LAPD\" version=\"S15\" distName=\"" + dataRow[55].ToString() + "\" operation=\"delete\"> </managedObject>\n";
                createLAPD += "<p name=\"" + cols[56] + "\">" + dataRow[56].ToString() + "</p>\n";
                InputData aData=inputList.Where(d =>d.BSC ==dataRow[0].ToString() && d.BCF ==dataRow[1].ToString() && d.BTS == dataRow[2].ToString() && d.TRX == dataRow[3].ToString()).First();
                
                createLAPD += "<p name=\"" + cols[57] + "\">" + aData.PCM + "</p>\n";
                createLAPD += "<p name=\"" + cols[58] + "\">" + aData.LAPDTSL + "</p>\n";
                createLAPD += "<p name=\"" + cols[59] + "\">" + aData.LAPDSSL + "</p>\n";
                int nextTSL = Convert.ToInt32(aData.PCMTsl) + 1;


                createLAPD += "<p name=\"adminState\">" + dataRow[60].ToString() + "</p>\n";
                createLAPD += "<p name=\"parentBSCId\">" + dataRow[61].ToString() + "</p>\n";
                createLAPD += "<p name=\"dChannelType\">" + dataRow[62].ToString() + "</p>\n";
                createLAPD += "<p name=\"name\">" + dataRow[63].ToString() + "</p>\n";
                createLAPD += "<p name=\"" + cols[64] + "\">" + dataRow[64].ToString() + "</p>\n";
                createLAPD += "<p name=\"" + cols[65] + "\">" + dataRow[65].ToString() + "</p>\n";
                createLAPD += "<p name=\"" + cols[66] + "\">" + dataRow[66].ToString() + "</p>\n";
                createLAPD += "</managedObject>\n";
                
                //for (int i = 64; i < 67; i++)
                //{
                    
                //       // createLAPD += "<p name=\"" + cols[i] + "\">" + dataRow[i].ToString() + "</p>\n";
                //        createLAPD += "<p name=\"" + cols[i] + "\">" + dataRow[i].ToString() + "</p>\n";
                    
                //    if (i == 67)
                //    {
                //        createLAPD += "</managedObject>\n";
                //    }
                    
                //}

                createTRX += createLAPD + "<managedObject class=\"TRX\" version=\"S15.3\" distName=\"" + dataRow[4].ToString() + "\" operation=\"create\">\n";
                deleteTRX += deleteLAPD + "<managedObject class=\"TRX\" version=\"S15.3\" distName=\"" + dataRow[4].ToString() + "\" operation=\"delete\"> </managedObject>\n";

                createTRX += "<p name=\"name\">" + dataRow[5].ToString() + "</p>\n";
                createTRX += "<p name=\"adminState\">" + dataRow[6].ToString() + "</p>\n";

                //for (int i = 7; i < 27; i++)
                //{


                createTRX += "<p name=\"channel0AdminState\">" + dataRow[7].ToString() + "</p>\n";
                createTRX += "<p name=\"channel0Pcm\">" + aData.PCM + "</p>\n";
                createTRX += "<p name=\"channel0Subslot\">" + dataRow[9].ToString() + "</p>\n";
                createTRX += "<p name=\"channel0Tsl\">" + aData.PCMTsl + "</p>\n";
                createTRX += "<p name=\"channel0Type\">" + dataRow[11].ToString() + "</p>\n";
                createTRX += "<p name=\"channel1AdminState\">" + dataRow[12].ToString() + "</p>\n";
                createTRX += "<p name=\"channel1Pcm\">" + aData.PCM + "</p>\n";
                createTRX += "<p name=\"channel1Subslot\">" + dataRow[14].ToString() + "</p>\n";
                createTRX += "<p name=\"channel1Tsl\">" + aData.PCMTsl + "</p>\n";
                createTRX += "<p name=\"channel1Type\">" + dataRow[16].ToString() + "</p>\n";
                createTRX += "<p name=\"channel2AdminState\">" + dataRow[17].ToString() + "</p>\n";
                createTRX += "<p name=\"channel2Pcm\">" + aData.PCM + "</p>\n";
                createTRX += "<p name=\"channel2Subslot\">" + dataRow[19].ToString() + "</p>\n";
                createTRX += "<p name=\"channel2Tsl\">" + aData.PCMTsl + "</p>\n";
                createTRX += "<p name=\"channel2Type\">" + dataRow[21].ToString() + "</p>\n";
                createTRX += "<p name=\"channel3AdminState\">" + dataRow[22].ToString() + "</p>\n";
                createTRX += "<p name=\"channel3Pcm\">" + aData.PCM + "</p>\n";
                createTRX += "<p name=\"channel3Subslot\">" + dataRow[24].ToString() + "</p>\n";
                createTRX += "<p name=\"channel3Tsl\">" + aData.PCMTsl + "</p>\n";
                createTRX += "<p name=\"channel3Type\">" + dataRow[26].ToString() + "</p>\n";


                createTRX += "<p name=\"channel4AdminState\">" + dataRow[27].ToString() + "</p>\n";
                createTRX += "<p name=\"channel4Pcm\">" + aData.PCM + "</p>\n";
                createTRX += "<p name=\"channel4Subslot\">" + dataRow[29].ToString() + "</p>\n";
                createTRX += "<p name=\"channel4Tsl\">" + nextTSL + "</p>\n";
                createTRX += "<p name=\"channel4Type\">" + dataRow[31].ToString() + "</p>\n";
                createTRX += "<p name=\"channel5AdminState\">" + dataRow[32].ToString() + "</p>\n";
                createTRX += "<p name=\"channel5Pcm\">" + aData.PCM + "</p>\n";
                createTRX += "<p name=\"channel5Subslot\">" + dataRow[34].ToString() + "</p>\n";
                createTRX += "<p name=\"channel5Tsl\">" + nextTSL + "</p>\n";
                createTRX += "<p name=\"channel5Type\">" + dataRow[36].ToString() + "</p>\n";
                createTRX += "<p name=\"channel6AdminState\">" + dataRow[37].ToString() + "</p>\n";
                createTRX += "<p name=\"channel6Pcm\">" + aData.PCM + "</p>\n";
                createTRX += "<p name=\"channel6Subslot\">" + dataRow[39].ToString() + "</p>\n";
                createTRX += "<p name=\"channel6Tsl\">" + nextTSL + "</p>\n";
                createTRX += "<p name=\"channel6Type\">" + dataRow[41].ToString() + "</p>\n";
                createTRX += "<p name=\"channel7AdminState\">" + dataRow[42].ToString() + "</p>\n";
                createTRX += "<p name=\"channel7Pcm\">" + aData.PCM + "</p>\n";
                createTRX += "<p name=\"channel7Subslot\">" + dataRow[44].ToString() + "</p>\n";
                createTRX += "<p name=\"channel7Tsl\">" + nextTSL + "</p>\n";
                createTRX += "<p name=\"channel7Type\">" + dataRow[46].ToString() + "</p>\n";





                    
                
                
                
                for (int i = 47; i < 55; i++)
                {
                    createTRX += "<p name=\"" + cols[i] + "\">" + dataRow[i].ToString() + "</p>\n";
                }

                createTRX += "</managedObject>\n";

                string s1 = cols[56];
                string s = dataRow[0].ToString();

            }


            string xmlFileCreate = "<?xml version=\"1.0\"?>\n <!DOCTYPE raml SYSTEM 'raml20.dtd'>\n <raml version=\"2.0\" xmlns=\"raml20.xsd\">\n<cmData type=\"plan\">\n<header>\n<log dateTime=\"\" action=\"created\" user=\"blOMC\" appInfo=\"blNokiaTool\"/>\n</header>\n";
            //xmlFileCreate += createTRX;
            createTRX = xmlFileCreate + createTRX;
            createTRX += "</cmData>\n</raml>";

            deleteTRX = xmlFileCreate + deleteTRX;
            deleteTRX += "</cmData>\n</raml>";




            using (StreamWriter aWriter = new StreamWriter("D:\\input\\createTRX.xml"))
            {
                aWriter.Write(createTRX);
                aWriter.Close();
            }

            using (StreamWriter aWriter = new StreamWriter("D:\\input\\deleteTRX.xml"))
            {
                aWriter.Write(deleteTRX);
                aWriter.Close();
            }


        }


    }
}
