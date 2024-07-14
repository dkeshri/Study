using Dkeshri.SystemDesign.LowLevel.Interfaces;
using System;

namespace Dkeshri.SystemDesign.LowLevel.DesignPrincipal.SOLID
{

    // Liskov Substitution Principle

    public class SmartTV
    {
        public virtual string getOTTApplicationList()
        {
            return "YouTube, Prime Video";
        }
        public virtual string getWifiConnectionStatus()
        {
            return "Connected.";
        }
        public virtual string DisplayResolution(){
            return "HD";
        }
    }
    public class LG_TV_Android:SmartTV{
        public override string getOTTApplicationList()
        {
            return "YouTube, Prime Video, ALT balaji, voot, hotstar";
        }
        public override string getWifiConnectionStatus()
        {
            return "Not Conneted";
        }
        public override string DisplayResolution(){
            return "Full HD with 1920x1080";
        }

    }
    public class ONIDA_TV:SmartTV{
        public override string getOTTApplicationList()
        {
            throw new NotImplementedException();
        }
        public override string getWifiConnectionStatus()
        {
            throw new NotImplementedException();
        }
        public override string DisplayResolution(){
            return "Standard Qualiy";
        }
    }


    public class LiskovPrincipalDemo : IExecute
    {
        public void run()
        {
            
            SmartTV smartTV = new SmartTV();
            smartTV = new ONIDA_TV();
            string listOfOTTApplication = smartTV.getOTTApplicationList();
            string isWifiConnected = smartTV.getWifiConnectionStatus();
            string displayResolution = smartTV.DisplayResolution();

            Console.WriteLine("List of OTT Applications: "+listOfOTTApplication);
            Console.WriteLine("Wifi Conneciton Status: "+isWifiConnected);
            Console.WriteLine("Display Resolution: "+displayResolution);
        }
    }
}