using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace microDrum
{
    public class UtilityIniFile
    {
        public static void DeleteSection(string Section, string IniFile)
        {
            if (String.IsNullOrEmpty(Section)) return;

            if (File.Exists(IniFile))
            {
                string[] Ini = File.ReadAllLines(IniFile);
                //Prima cerca la sezione
                bool bSection = false;
                for (int i = 0; i < Ini.Length; i++)
                {
                    string line = Ini[i].Trim();
                    //se c'è cerca il parametro
                    if (bSection)
                    {
                        if (line.StartsWith("["))
                        {
                            continue;
                        }
                        //se c'è modifica il valore
                        else
                        {
                            Ini[i] = "";
                        }
                    }
                    else
                    {
                        if (line.ToUpper() == "[" + Section.ToUpper() + "]") { bSection = true; Ini[i] = ""; }
                    }
                }

                List<string> newFile = new List<string>();
                foreach (string s in Ini)
                    if (!String.IsNullOrEmpty(s)) { if (s.StartsWith("[")) newFile.Add(""); newFile.Add(s); }
                File.WriteAllLines(IniFile, newFile.ToArray());
            }
        }

        public static string[] GetKeys(string Section, string IniFile)
        {
            if (!File.Exists(IniFile)) return (null);

            string[] Ini = File.ReadAllLines(IniFile);
            List<string> Keys = new List<string>();

            bool bSection = false;
            foreach (string i in Ini)
            {
                string line = i.Trim();
                if (bSection)
                {
                    if (line.StartsWith("["))
                        break; //E' finita la sezione
                    if (line.Contains("="))
                    {
                        Keys.Add( line.Split('=')[0]);
                    }
                }
                else
                {
                    if (line.ToUpper() == "[" + Section.ToUpper() + "]") bSection = true;
                }
            }
            return (Keys.ToArray());
        }

        public static string GetIniString(string Section, string Key, string IniFile)
        {
            if (!File.Exists(IniFile)) return ("");

            string[] Ini = File.ReadAllLines(IniFile);

            bool bSection = false;
            foreach (string i in Ini)
            {
                string line = i.Trim();
                if (bSection)
                {
                    if (line.StartsWith("[")) return (""); //E' finita la sezione e non è stata trovata la stringa
                    if (line.ToUpper().StartsWith(Key.ToUpper()))
                    {
                        return (line.Substring(line.IndexOf('=') + 1));
                    }
                }
                else
                {
                    if (line.ToUpper() == "[" + Section.ToUpper() + "]") bSection = true;
                }
            }
            return ("");
        }

        public static void SetIniString(string Section, string Key, string Value, string IniFile)
        {
            if (String.IsNullOrEmpty(Section) || String.IsNullOrEmpty(Key)) return;

            if (File.Exists(IniFile))
            {
                string[] Ini = File.ReadAllLines(IniFile);
                //Prima cerca la sezione
                bool bSection = false;
                for (int i = 0; i < Ini.Length; i++)
                {
                    string line = Ini[i].Trim();
                    //se c'è cerca il parametro
                    if (bSection)
                    {
                        //se non c'è aggiungi il parametro ed il valore
                        if (line.StartsWith("["))
                        {
                            Ini[i] = Key + "=" + Value + Environment.NewLine + Ini[i];
                            File.WriteAllLines(IniFile, Ini);
                            return;
                        }
                        //se c'è modifica il valore
                        else if (line.ToUpper().StartsWith(Key.ToUpper()))
                        {
                            Ini[i] = Key + "=" + Value;
                            File.WriteAllLines(IniFile, Ini);
                            return;
                        }
                    }
                    else
                    {
                        if (line.ToUpper() == "[" + Section.ToUpper() + "]") bSection = true;
                    }
                }
                if (bSection)//Caso particolare in cui c'è la sezione ma è l'ultima
                {
                    File.AppendAllText(IniFile, Key + "=" + Value + Environment.NewLine);
                    return;
                }

                //se non c'è creala, aggiungi il paramero ed il valore
                File.AppendAllText(IniFile, Environment.NewLine + "[" + Section + "]" + Environment.NewLine + Key + "=" + Value + Environment.NewLine);
            }
            else
            {
                string Ini = "[" + Section + "]" + Environment.NewLine + Key + "=" + Value + Environment.NewLine;
                File.WriteAllText(IniFile, Ini);
            }
        }
    }
}
