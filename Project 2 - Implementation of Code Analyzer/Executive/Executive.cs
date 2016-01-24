///////////////////////////////////////////////////////////////////////
// Executive.cs - Manage all Packages of Code Analyzer               //
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
 * This Package first get a arguments from user and send to commandLineParser
 * by which it got path, patterns and options. Further it passes this data to 
 * FileManager to collect files, these collected file passed to Analyzer for 
 * parsing.
 * 
 * At the end when all files get parsed, based on options got from user 
 * it will call diplay Package to print processing data on console or 
 * XMLFileRedirection Package to store outpit data in XML file.
 * 
 */
/* Required Files:
 *   CommandLineParser.cs, FileManager.cs, Analyzer.cs, Display.cs,
 *   XMLFileREdirection.cs  
 *   
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
namespace CodeAnalyzer
{
    class Executive
    {


        //----------------< process analysis and display >----------------
        private static void analysisAndDisplay(string[] args, string[] files, string path, List<string> patterns, List<string> options)
        {
            bool R = false;
            bool X = false;
            if (options.Contains("/R") || options.Contains("/r"))
                R = true;
            if (options.Contains("/X") || options.Contains("/x"))
                X = true;

            Analyzer.doAnalysis(files);

            Display.displayArgument(args, files, path, patterns, options);
            Display.displayTypeDefined(files);

            if (R)
            {
                Analyzer.doRelationAnalysis(files);
                Display.displayRelation(files);
            }
            else
            {
                Display.displayOutput(files);
            }
          
            if (X)
            {
                XMLFileRedirection xml = new XMLFileRedirection();
                xml.displayxml(R, files);
            }
        }
        private static string[] getFile(string path, List<string> patterns, List<string> options)
        {
            bool S = false;
            if (options.Contains("/S") || options.Contains("/s"))
                S = true;

            FileManager fm = new FileManager();
            return fm.getFiles(path, patterns, S).ToArray();
        }
#if(TEST_EXECUTIVE)
        static void Main(string[] args)
        {
            string path = null;
            List<string> patterns = new List<string>();
            List<string> options = new List<string>();

            if (args.Length == 0) //arguments null
            {
                Console.Write("\n Sorry! There is no Input");
                Console.Write("\n Please enter Path, Pattern or Option\n\n");
                return;
            }
            CommandLineParser clp = new CommandLineParser();
            path = clp.ProcessCommandlinePath(args);
            patterns = clp.ProcessCommandlinePattern(args);
            options = clp.ProcessCommandlineOption(args);

            if (!Directory.Exists(path)) // given path is invalid
            {
                Console.Write("\n Sorry! ");
                Console.Write("\n path is invalid ...\n\n");
                return;
            }

            string[] files = getFile(path, patterns, options);

            int x = files.Length;
            if (x != 0) // no files at given path
            {
                analysisAndDisplay(args, files, path, patterns, options);
            }
            else
            {
                Display.displayArgument(args, files, path, patterns, options);
                Console.Write("Sorry! There is no File for Your Input\n");
                Console.Write("Enter Correct Input For Analysis\n\n");
            }
        }
#endif
    }
}
