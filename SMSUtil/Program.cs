using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Twilio;

namespace SMSUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            var accountSid = string.Empty;
            var authToken = string.Empty;
            var sender = string.Empty;
            var receiver = string.Empty;
            var message = string.Empty;

            if (args.Length != 5 && args.Length != 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("   SMSUtil.exe <ACCOUNT SID> <AUTH TOKEN> <FROM #> <TO #> <MESSAGE>");
                Console.WriteLine("   With registry settings - SMSUtil.exe <TO #> <MESSAGE>");
                Environment.Exit(1);
            }

            if (args.Length == 5)
            {
                accountSid = args[0];
                authToken = args[1];
                sender = args[2];
                receiver = args[3];
                message = args[4];
            }
            else
            {
                const string keyName = "HKEY_CURRENT_USER\\SMSUtil";

                try
                {
                    accountSid = Registry.GetValue(keyName, "AccountSID", string.Empty).ToString();
                    authToken = Registry.GetValue(keyName, "AuthToken", string.Empty).ToString();
                    sender = Registry.GetValue(keyName, "Sender", string.Empty).ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: an exception has occured when reading registry value ({0}), exiting...", e.Message);
                    Environment.Exit(2);
                }

                receiver = args[0];
                message = args[1];
            }


            var twilioClient = new TwilioRestClient(accountSid, authToken);
            twilioClient.SendMessage(sender, receiver, message, string.Empty);
        }
    }
}
