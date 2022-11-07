/**************************************************************************************************************************************************************\
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
 *
 * File:
 *  Program.cs  
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Description:
 *  Tests the ViViVerse interface provided by vvv_dotnet.
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Created:
 *  2017-07-14
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Copyright (c):
 *  ViViVerse GmbH  
 *
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
\**************************************************************************************************************************************************************/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace vvv_dotnet_test
{
    class Program
    {
        //  this function is called whenever the slope metabolic is delivered
        private static void SlopeDisplay(double Slope)
        {
            Console.WriteLine("Slope: " + Slope.ToString() + "rad");
        }  //  SlopeDisplay

        static void Main(string[] args)
        {
            //  synthesise the vivid. the dna file path is relative to the configuration folder
            IntPtr Vivid = VVV.BaseInterface.SynthesiseVivid("vvv_base\\experiment\\vvv_simulators\\vvv_simulators.vvvdna");

            //  search for a slope sensor
            VVV.HoloTaxon Hota = new VVV.HoloTaxon("SlopeSensor");
            IntPtr QueryResult;
            UIntPtr DonorCount = VVV.BaseInterface.SearchForDonors(Vivid, ref Hota, VVV.DonorQueryFlags.AllDerived, out QueryResult);

            //  if a slope sensor can be found, get the SlopeSensor family from it
            VVV.Donor Donor;
            IntPtr Family;
            if (DonorCount.ToUInt32() >= 1 &&
                VVV.BaseInterface.GetDonorInfo(Vivid, QueryResult, 0, out Donor) &&
                (Family = VVV.BaseInterface.GetFamily(Vivid, Donor.Don, Donor.Hota.Family)) != null)
            {
                //  subscribe for the slope metabolic. we need a delegate for this
                //VVV.FamilySlopeSensor.SlopeDeliveryDelegate SlopeDel = new VVV.FamilySlopeSensor.SlopeDeliveryDelegate(SlopeDisplay);
                //IntPtr Sub = VVV.FamilySlopeSensor.SubscribeForSlopeMetabolic(Vivid, Family, VVV.BaseInterface.DeliverOnChangeSync, SlopeDel);

                //  wait until the user hits a key. during this time, the delegate is called and will write the slope value to the console
                System.Console.ReadKey();
                //System.Threading.Thread.Sleep(5000);

                //  unsubscribe from the metabolic
                //VVV.BaseInterface.UnsubscribeFromMetabolic(Vivid, Sub);
            }  //  if (DonorCount.ToUInt32() >= 1

            //  dissolve the vivid
            bool Dissolved = VVV.BaseInterface.DissolveVivid(Vivid);
        }  //  Main
    }  //  class Program
}  //  namespace vvv_dotnet_test
