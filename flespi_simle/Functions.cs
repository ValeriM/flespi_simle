﻿using System;
using System.Web.Script.Serialization;
using System.IO;
namespace flespi_simle
{
    public static class Utils
    {
        public static void Print(string filename, string line, bool console = true)
        {
            if (console)
                Console.WriteLine(line);
            StreamWriter writer;
            if (File.Exists(filename))
                writer = new StreamWriter(filename, true);
            else
                writer = new StreamWriter(filename);
            writer.WriteLine(line);
            writer.Close();
        }
        public static string ReadFile(string fileName)
        {
            string ret;
            StreamReader reader = new StreamReader(fileName);
            ret = reader.ReadToEnd();
            reader.Close();
            return ret;
        }
    }
}