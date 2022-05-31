using System;
using System.Collections.Generic;
using System.IO;

namespace Text_Parsers
{
    class MainClass
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
        string[] cut = { " ", " " };

        // FN as in File Name, FP as in File Path, FPN as in File Path & Name
        static string FN_ini = "LogFileParser.ini"; // Path defaults to app folder
        static string FPN_log_out = " "; // Full File Path & name of this app's runtime log
        string FN_log_in = " "; // File Name of active log to parse, built from RSI name, read from INI file
        string FP_log_in = " "; // File Path of active log to parse, read from INI file
        string FPN_log_in = " "; // Full File Path & name of active log to parse
        string FPN_audio_player = " ";
        string[] FP_sound_board = { " ", " ", " " };
        string CMD_out = " "; // command line to shell

        int ii = 0; // list index integer
        int llen = 0; // length of line
        int allen = 0; // length of aline
        int mllen = 0; // length of mline
        int lepos = 0; // position of substring in line
        int alepos = 0; // position of substring in aline
        int mlepos = 0; // position of substring in mline
        int line_control = 1; // number of lines before flush to input log file

        float read_delay = 1; // timing in decimal seconds for input synching
        float act_delay = 0; // timing in decimal seconds for processing synching
        float wait_delay = 0; // timing in decimal seconds for output synching

        // bool validRecord = true; // flag for logical operations
        bool exit_local = false;
        bool exit_global = false;

        public static void Main(string[] args)
        {
            MainClass parse = new MainClass();
            if (parse.Exists(FN_ini))
            {
                Console.WriteLine("Parsing Log File Parser INI file " + FN_ini);
                parse.INI(FN_ini);
            }
            else
            {
                Console.WriteLine(FN_ini + " is missing from the application folder.");
                parse.exit_global = true;
            }
            while (!parse.exit_global)
            {
                parse.exit_global = parse.LOGIN(parse.FPN_log_in);
                parse.LOGOUT(FPN_log_out); // exit log write
            }
        } //Main

        public void INI(string FN)
        {
            using (StreamReader reader = new StreamReader(FN))
            {
                // Parse INI lines; assign values
                Console.WriteLine("Parsing INI list.");
                while ((line = reader.ReadLine()) != null)
                {
                    aline = line.ToUpper();
                    // Console.WriteLine(aline);
                    if (aline.Contains("="))
                    {
                        string[] cut = aline.Split("=", System.StringSplitOptions.TrimEntries);
                        p1 = cut[0];
                        p2 = cut[1];
                        p2 = p2.Trim('"');
                        Console.WriteLine("{0}, {1}", p1, p2);
                        switch (p1)
                        {
                            case "INPUTLOGPATH":
                                FP_log_in = Environment.ExpandEnvironmentVariables(p2);
                                break;
                            case "OUTPUTLOGPATH":
                                FPN_log_out = Environment.ExpandEnvironmentVariables(p2);
                                break;
                            case "AUDIOPLAYEREXEPATH":
                                FPN_audio_player = Environment.ExpandEnvironmentVariables(p2);
                                break;
                            case "SOUNDBOARDPATH00":
                                FP_sound_board[0] = Environment.ExpandEnvironmentVariables(p2);
                                break;
                            case "SOUNDBOARDPATH01":
                                FP_sound_board[1] = Environment.ExpandEnvironmentVariables(p2);
                                break;
                            case "SOUNDBOARDPATH02":
                                FP_sound_board[2] = Environment.ExpandEnvironmentVariables(p2);
                                break;
                            case "RSI_HANDLE":
                                FN_log_in = p2 + ".log";
                                break;
                            case "CHATLOGNUMLINESBEFOREFLUSH":
                                line_control = int.Parse(p2);
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
                        } // switch
                    } // if contains "="
                } //while
                FPN_log_in = FP_log_in + FN_log_in;
                FPN_log_out = FPN_log_out + DateTime.Now.ToString("yyyy-MM-dd_THH`mm`ss") + "-LogFileParser.log";
                reader.Close();
                Console.WriteLine("Read INI file.");
            }// Using
        } // INI

        public bool Exists(string FPN)
        {
            Console.WriteLine("{0} {1}", "Checking for file:", FPN);
            return File.Exists(FPN);
        } // Exists

        public bool LOGIN(string FN)
        {
            bool onexit = false;

            // Note the FileShare.ReadWrite, allowing the emulator to modify the file
            using (FileStream fileStream = File.Open(FN, FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite))
            {
                fileStream.Seek(0, SeekOrigin.End);
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    while (!onexit)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(read_delay));
                        line = streamReader.ReadToEnd();
                        // find in line block for Environment.NewLine each line
                        Console.Out.Write(line);
                        if (line.Contains("SB0P"))
                        {
                            System.Diagnostics.Process.Start('"' + FPN_audio_player + '"' + " " + '"' + "Y:\\PixelTrope2022\\Resources\\Audio\\MxO_Emu\\MxO_Edits\\Soundboards\\Agent Gray\\I_Am_Agent_Gray.wav" + '"');
                        }
                        if (line.Contains("!Exit!"))
                        {
                            onexit = true;
                        }
                    }
                }
            }
            return onexit;
        } // LOG IN

        public void LOGOUT(string FN)
        {
            Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", FP_log_in, FPN_log_out, FN_log_in, line_control, read_delay, act_delay, wait_delay);
            Console.WriteLine("Log In: {0}\nLog Out: {1}", FPN_log_in, FPN_log_out);
        }  // LOG OUT

    } //class
} //namespace

/*
 code from previous application for harvesting

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

        list.Add(line);
*/