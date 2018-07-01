using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParsingAddress
{
    class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllLines("SortedAddress.txt", File.ReadLines("addresslist.txt").Skip(1).Select(x => AddressParse(x.Trim().ToLower())));
        }

        private static string AddressParse(string line)
        {
            string addressLine = line.Split('\t')[1];

            if (addressLine.StartsWith("lot") && addressLine.Split(',').Count() == 1
            ) //Find address name without delimiter
            {
                addressLine = addressLine.Substring(addressLine.IndexOf(" dp ") + 4);
                addressLine = addressLine.Substring(addressLine.IndexOf(" ")).Trim();
            }else if (addressLine.StartsWith("lot"))
            {
                addressLine = addressLine.Split(',')[1].Trim();
            }

            if (addressLine.Contains('/'))
                addressLine = addressLine.Split('/')[1];

            if (addressLine.Contains('-'))
                addressLine = addressLine.Split('-')[1];

            //Start formatting the street number 
            string streetNumber = addressLine.Split(' ')[0];

            //handle addresses like R 27 abc road
            if (streetNumber.Length == 1 && !(streetNumber.ToCharArray().Any(x => char.IsDigit(x))))
            {
                addressLine = addressLine.Substring(addressLine.IndexOf(" "));
                streetNumber = addressLine.Split(' ')[0];
            }

            if (streetNumber.ToCharArray().Any(x => char.IsDigit(x))) //only parse those with house no.
            {
                streetNumber = Regex.Replace(streetNumber, "[^0-9_ /-]+", "").Trim();
            }
            //End formatting street number

            addressLine = streetNumber + addressLine.Substring(addressLine.IndexOf(" "));

            addressLine = addressLine.Replace(" ", "");

            return string.Format("{0}, {1}", line.Split('\t')[0], addressLine);
        }
    }
}
