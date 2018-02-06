using Microsoft.Owin.Hosting;
using System;

namespace FSBT_HHT_Services
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://" + APIConstant.Instance.localIPAddress + ":15267/";
            try
            {
                using (WebApp.Start<Startup>(url: baseAddress))
                {
                    Console.WriteLine("Start Services");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                LogFile.write("0", ex.Message);
                Environment.Exit(1);
                //Console.WriteLine("Problem occured");
                //Console.ReadLine();
            }
        }
    }
}
