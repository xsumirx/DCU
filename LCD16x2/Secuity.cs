using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using OpenNETCF;
using System.Security.Cryptography;

namespace LCD16x2
{
    public static class Secuity
    {
        private static String KEY = "0631EB4CDD167B3579A00D9C5945F6EB";

        public static bool VerifySecuity() {
            bool returnVal = false;
            try
            {
                string temp = GenrateKey();
                if (temp == KEY)
                {
                    returnVal = true;
                }
                else
                    returnVal = false;
            }
            catch(Exception ex) { }

            return returnVal;
        }

        private static string GenrateKey()
        {
            string resultingHash = "";
            try
            {
                string mac = GetMAC();
                string local = "Sumir Kumar Jha";

                string has1 = CreateMD5(mac);
                string has2 = CreateMD5(local);


                var has1Char = has1.ToCharArray();
                var has2Char = has2.ToCharArray();

                for (int i = 0; i < 32; i++)
                {
                    resultingHash += has1Char[i].ToString() + has2Char[i].ToString();
                }
            }
            catch (Exception ex) { }

            return CreateMD5(resultingHash);
        }

        private static string GetMAC()
        {
            string mac = "Error";
            try
            {
                OpenNETCF.Net.NetworkInformation.INetworkInterface[] i = OpenNETCF.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

                foreach (OpenNETCF.Net.NetworkInformation.INetworkInterface inter in i)
                {
                    OpenNETCF.Net.NetworkInformation.PhysicalAddress ph = inter.GetPhysicalAddress();
                    if (inter.NetworkInterfaceType == OpenNETCF.Net.NetworkInformation.NetworkInterfaceType.Ethernet && (inter.Name.Contains("ENET") || inter.Speed > 19900))
                    {
                        mac = ph.ToString();
                    }
                }
            }
            catch (Exception e) { }
            return mac;
        }

        private static string CreateMD5(string input)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                // Use input string to calculate MD5 hash
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
            }
            catch (Exception ex) { }
            return sb.ToString();
        }
    
    
    }
}
