using IniParser;
using IniParser.Model;
using System;

namespace ckir_crasher_2.Classes
{
    internal class iniFileHelper
    {
        private static string FileName = "app.conf";

        private static FileIniDataParser fileIniDataParser = new FileIniDataParser();
        private static IniData parsedData = fileIniDataParser.ReadFile(FileName);

        private static void Save()
        {
            fileIniDataParser.WriteFile(FileName, parsedData);

        }

        public static void changeValue(string header, string keyname, string value)
        {
            parsedData[header].GetKeyData(keyname).Value = value;
            Save();
        }

        public static string getValue(string header, string keyname)
        {
            return parsedData[header].GetKeyData(keyname).Value;
        }

        public static bool getValue_bool(string header, string keyname) {
            if(parsedData[header].GetKeyData(keyname).Value == "true" || parsedData[header].GetKeyData(keyname).Value == "True" || parsedData[header].GetKeyData(keyname).Value == "1")
            {
                return true;
            }
            else
            {
                return false;
            }

            
        }
    }
}
