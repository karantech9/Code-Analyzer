///////////////////////////////////////////////////////////////////////
// CommandLineParser.cs - Parse the Commanl Line Arguments           //
// ver 1.0                                                           //
// Language:    C#, Visual Studio 13.0, .Net Framework 4.5           //
// Platform:    Dell Inspiron 17, Windows 8                          //
// Application: Demonstration for CIS 681, Project #2, Fall 2014     //
// Author:      Karankumar Patel, Syracuse University                //
//              (315) 751-5637, khpatel@syr.edu                      //
///////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * ------------------
 * This module defines the following class:
 *   CommandLineParser  - three methods processCommandLinePath,
 *   processCommandLinePattern and processCommandLineOption parse and
 *   identify path, patterns & options respectively
 */
/*     
 * Maintenance History:
 * --------------------
 * ver 1.0 : 9 Oct 2014
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace CodeAnalyzer
{
    public class CommandLineParser
    {
        // ---------------< process commandline to get path >------------------
        public string ProcessCommandlinePath(string[] args)
        {
            string path = "";
            for (int i = 0; i < args.Length; i++)
            {
                if (Directory.Exists(args[i]) || args[i].Contains("../") || args[i].Contains("./") || args[i].Contains(":/"))
                {
                    path = Path.GetFullPath(args[i]);
                    break; 
                }
            }
            return path;
        }


        // -----------------< process commandline to get patterns >------------------
        public List<string> ProcessCommandlinePattern(string[] args)
        {
            List<string> patterns = new List<string>();
            for (int i = 0; i < args.Length; i++)
            {
                string check = args[i];
                Match match = Regex.Match(check, @"[A-Za-z0-9\*]+\.[A-Za-z\*]+$");
                if (match.Success)
                    patterns.Add(args[i]);
            }
            return patterns;
        }


        // ----------------< process commandline to get options >-------------------
        public List<string> ProcessCommandlineOption(string[] args)
        {
            List<string> options = new List<string>();
             for (int i = 0; i < args.Length; i++)
             {
                 if (args[i].Contains("/X") || args[i].Contains("/R") || args[i].Contains("/S") || args[i].Contains("/s") || args[i].Contains("/r") || args[i].Contains("/x"))
                     options.Add(args[i]);
             }
             return options;
        }

        private void ShowCommandLine(string[] args)
        {
            Console.Write("\n  Commandline args are:\n");
            foreach (string arg in args)
            {
                Console.Write("  {0}", arg);
            }
            Console.Write("\n\n  current directory: {0}", System.IO.Directory.GetCurrentDirectory());
            Console.Write("\n\n");
        }

#if(TEST_COMMANDLINE)
        static void Main(string[] args)
        {

            Console.Write("\n  Demonstrating CommandLineParser");
            Console.Write("\n ======================\n");

            string[] arg = { "../../", "*.cs", "/s", "/r" };
            CommandLineParser ip = new CommandLineParser();
            string path = ip.ProcessCommandlinePath(arg);
            List<string> patterns = ip.ProcessCommandlinePattern(arg);
            List<string> options = ip.ProcessCommandlineOption(arg);
            ip.ShowCommandLine(arg);

            Console.Write("  Path: {0}\n", path);

            Console.Write("\n  File Pattern: ");
            foreach (string patt in patterns)
                Console.Write("{0}\t", patt);

            Console.Write("\n  Options: ");
            foreach (string patt in options)
                Console.Write("{0}\t", patt);
            Console.WriteLine("\n\n");

        }
#endif
    }
}
