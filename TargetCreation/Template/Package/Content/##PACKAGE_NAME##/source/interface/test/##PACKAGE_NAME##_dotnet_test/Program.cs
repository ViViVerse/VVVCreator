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
 *  ##DATE##
 * ------------------------------------------------------------------------------------------------------------------------------------------------------------
 * Copyright (c):
 *  ##COPYRIGHT##
 *
 **************************************************************************************************************************************************************
 **************************************************************************************************************************************************************
\**************************************************************************************************************************************************************/



using System;
using System.Runtime.InteropServices;



namespace vvv_dotnet_test
{
    class Program
    {
        /// <summary>
        /// Called when an exception thrown by the VVV is deleted. This function is also called for each peer and inner exception.
        /// </summary>
        /// <param name="message">The error message related to the exception.</param>
        /// <param name="param">The custom parameter provided at the call of SetExceptionHook. Could be a pointer to an object.</param>
        private static void exceptionDisplay([MarshalAs(UnmanagedType.LPWStr)] string message, IntPtr param)
        {
            Console.WriteLine(message);
        } // exceptionDisplay

        /// <summary>
        /// Called whenever the slope metabolic is delivered.
        /// </summary>
        /// <param name="param">The custom parameter provided at the subscription. Could be a pointer to an object. Not used.</param>
        /// <param name="Slope">The slope value.</param>
        private static void slopeDisplay(double slope, IntPtr param)
        {
            Console.WriteLine("Slope: " + slope.ToString() + "rad");
        } // slopeDisplay

        static void Main(string[] args)
        {
            // Should the VVV throw exceptions, display them in the console.
            VVV.ExceptionDelegate exDel = new VVV.ExceptionDelegate(exceptionDisplay);
            VVV.BaseInterface.SetExceptionHook(exDel, IntPtr.Zero);

            // Synthesise the vivid. the dna file path is relative to the configuration folder.
            using (VVV.Vivid vivid = new VVV.Vivid("vvv_base\\experiment\\vvv_simulators\\vvv_simulators.vvvdna"))
            {
                // Get the slope sensor (actually: the simulator).
                VVV.HoloTaxon hota = new VVV.HoloTaxon("SlopeSensor");
                using (VVV.SlopeSensor slopeSensor = new VVV.SlopeSensor(vivid, ref hota, VVV.DonorQueryFlags.AllDerived))
                {
                    // Subscribe to the slope metabolic. We need a delegate for this.
                    VVV.SlopeSensor.SlopeDeliveryDelegate slopeDel = new VVV.SlopeSensor.SlopeDeliveryDelegate(slopeDisplay);
                    slopeSensor.SubscribeToSlopeMetabolic(VVV.BaseInterface.DeliverOnChangeSync, slopeDel, IntPtr.Zero);

                    // Wait until the user hits a key. During this time, the delegate is called and will write the slope value to the console.
                    System.Console.ReadKey();
                } // using (VVV.SlopeSensor
            } // using (VVV.Vivid vivid
        } // Main
    }  //  class Program
}  //  namespace vvv_dotnet_test
