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
            VVV.Vivid vivid = VVV.BaseInterface.SynthesiseVivid("vvv_base\\experiment\\vvv_simulators\\vvv_simulators.vvvdna");

            //  search for a slope sensor
            VVV.HoloTaxon hota = new VVV.HoloTaxon("SlopeSensor");
            IntPtr queryResult;
            UIntPtr donorCount = VVV.BaseInterface.SearchForDonors(vivid, ref hota, VVV.DonorQueryFlags.AllDerived, out queryResult);

            //  if a slope sensor can be found, get the SlopeSensor family from it
            VVV.Donor donor;
            VVV.Family family;
            if (donorCount.ToUInt32() >= 1 &&
                VVV.BaseInterface.GetDonorInfo(vivid, queryResult, 0, out donor) &&
                (family = VVV.BaseInterface.GetFamily(vivid, donor.Don, donor.Hota.Family)).IsValid())
            {
                //  subscribe for the slope metabolic. we need a delegate for this
                //VVV.FamilySlopeSensor.SlopeDeliveryDelegate SlopeDel = new VVV.FamilySlopeSensor.SlopeDeliveryDelegate(SlopeDisplay);
                //IntPtr sub = VVV.FamilySlopeSensor.SubscribeForSlopeMetabolic(Vivid, Family, VVV.BaseInterface.DeliverOnChangeSync, SlopeDel);

                //  wait until the user hits a key. during this time, the delegate is called and will write the slope value to the console
                System.Console.ReadKey();
                //System.Threading.Thread.Sleep(5000);

                //  unsubscribe from the metabolic
                //VVV.BaseInterface.UnsubscribeFromMetabolic(vivid, sub);
            }  //  if (donorCount.ToUInt32() >= 1

            //  dissolve the vivid
            bool dissolved = VVV.BaseInterface.DissolveVivid(vivid);
        }  //  Main
    }  //  class Program
}  //  namespace vvv_dotnet_test
