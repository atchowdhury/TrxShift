using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrxShift.DAL;


namespace TrxShift
{
    class Program
    {
        static void Main(string[] args)
        {

            string line;
            System.IO.StreamReader file =
           new System.IO.StreamReader(@"D:\input\input1.txt");
            List<InputData> aInputList = new List<InputData>();
            ShiftManager aManager = new ShiftManager();
            while ((line = file.ReadLine()) != null)
            {
                InputData aData = new InputData();
                string[] input = line.Split(' ');
                aData.BSC = input[0];
                aData.BCF = input[1];
                aData.BTS = input[2];
                aData.TRX = input[3];
                aData.PCM = input[4];
                aData.PCMTsl = input[5];
                aData.LAPDName = input[6];
                aData.LAPDTSL = input[7];
                aData.LAPDSSL = input[8];
                
                aInputList.Add(aData);
            }

            aManager.TRXshifter(aInputList);
        }
    }
}
