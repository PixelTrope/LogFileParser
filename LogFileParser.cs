using System;
using System.Collections.Generic;
using System.IO;

namespace Text_Parsers
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            List<string> list = new List<string>();
            List<string> outp = new List<string>();

            // variable names '?line' refer to strings containing a line or line substring of text
            string line; // input line
            string aline; // next line for logical operations
            string mline; // next-next line for logical operations
            string p1; // substring (part 1)
            string p2; // substring (part 2)
            string oline; // output line

            int ii; // list index integer
            int llen; // length of line
            int allen; // length of aline
            int mllen; // length of mline
            int lepos; // position of substring in line
            int alepos; // position of substring in aline
            int mlepos; // position of substring in mline

            float read_delay; // timing in decimal seconds for input synching
            float act_delay; // timing in decimal seconds for processing synching
            float wait_delay; // timing in decimal seconds for output synching

            bool validRecord = false; // flag for logical operations

            // FN as in File Name, FP as in File Path, FPN as in File Path & Name
            string FN_ini = "D:\\Developer2022\\CS\\Projects\\LogFileParser\\LogFileParser.ini"; // Path defaults to app folder
            string FN_log_in; // File Name of active log to parse, built from RSI name, read from INI file
            string FP_log_in; // File Path of active log to parse, read from INI file
            string FPN_log_in; // Full File Path & name of active log to parse
            string FPN_log_out; // Full File Path & name of this app's runtime log
            string CMD_out; // command line to shell

            // Read file into list lines
            Console.WriteLine("Reading INI file..." + FN_ini);
            using (StreamReader reader = new StreamReader(FN_ini))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line);
                } //while
                reader.Close();
                Console.WriteLine("Read INI file.");
            }// Using

            // Parse INI lines; assign values
            ii = 0;
            Console.WriteLine("Parsing INI list.");
            while (ii < list.Count)
            {
                line = list[ii].ToUpper();
                Console.Write(line);
                if (line.Contains("="))
                {
                    llen = line.Length;
                    lepos = line.IndexOf('=') + 1;
                    p1 = line.Substring(0, lepos);
                    p1 = p1.Trim(' ');
                    p2 = line.Substring(lepos, allen - lepos);
                    p2 = p2.Trim(' ');
                    switch (p1)
                    {
                        case "INPUTLOGPATH":
                            FP_log_in = p2;
                            break;
                        case "OUTPUTLOGPATH":
                            FPN_log_out = p2;
                            break;
                        case "RSI_HANDLE":
                            FN_log_in = p2;
                            break;
                        case "CHATLOGNUMLINESBEFOREFLUSH":
                            //
                            break;
                        case "READ_DELAY":
                            read_delay = float.Parse(p2);
                            break;
                        case "ACT_DELAY":
                            act_delay = float.Parse(p2);
                            break;
                        case "WAIT_DELAY":
                            wait_delay = float.Parse(p2);
                            break;
                    }
                    FPN_log_in = FP_log_in + FN_log_in + ".log";
                    FPN_log_out = FPN_log_out + DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’-’mm’-’ss_fffffffK") + "-LogFileParser.log";
                }
            } //while
        } //public
    } //class
} //namespace

/*
                if (String.Equals(line, "\t\tVESSEL"))
                {
                    validRecord = false;
                    Console.WriteLine("VESSEL found at line ");
                    Console.WriteLine(ii);
                } //if 1
                if (String.Equals(line, "\t\t\tname = Mega Spaceplane XI"))
                {
                    validRecord = true;
                    Console.WriteLine("MSPXI found at line ");
                    Console.WriteLine(ii);
                } //if2
                if (validRecord == true)
                {
                    if (line.Contains("name = LiquidFuel") || line.Contains("name = Oxidizer") || line.Contains("name = MonoPropellant") || line.Contains("name = Ore"))
                    {
                        aline = list[ii + 1];
                        allen = aline.Length;
                        alepos = aline.IndexOf('=') + 1;
                        mline = list[ii + 2];
                        mllen = mline.Length;
                        mlepos = mline.IndexOf('=') + 1;
                        p1 = aline.Substring(0, alepos);
                        p2 = mline.Substring(mlepos, mllen - mlepos);
                        oline = p1 + p2;
                        outp.Add(line);
                        ii++;
                        outp.Add(oline);
                        ii++;
                        outp.Add(mline);
                        Console.WriteLine("Adjusting values:");
                        Console.WriteLine(line, oline, aline, mline);
                    } //if 4
                    else
                    {
                        outp.Add(line);
                        ii++;
                    } //else 4
                } //if 3
                else
                {
                    outp.Add(line);
                    ii++;
                } //else 3

     // Save outp lines
        Console.WriteLine("Saving file.");
        using (TextWriter tw = new StreamWriter("SavedList.txt"))
        {
            foreach (String s in outp) tw.WriteLine(s);
            tw.Close();
            Console.WriteLine("Done!");
        } //using

        */
