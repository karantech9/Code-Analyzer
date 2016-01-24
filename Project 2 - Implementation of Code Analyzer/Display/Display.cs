///////////////////////////////////////////////////////////////////////
// display.cs - display output on console parsed by parser           //
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
 * This Package used the data store at RepositoryForOutput and 
 * RepositoryForRelation in Parser Package to display processed 
 * data of various file in order.
 */
/* Required Files:
 *   IRulesAndActions.cs, RulesAndActions.cs, Parser.cs, Semi.cs, Toker.cs
 *   ScopeStack.cs  
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
    public class DistinctItemComparer : IEqualityComparer<ElemRelation>
    {

        public bool Equals(ElemRelation x, ElemRelation y)
        {
            return x.filename == y.filename &&
                x.relationType == y.relationType &&
                x.fromName == y.fromName &&
                x.toName == y.toName;
        }

        public int GetHashCode(ElemRelation obj)
        {
            return obj.filename.GetHashCode() ^
                obj.relationType.GetHashCode() ^
                obj.fromName.GetHashCode() ^
                obj.toName.GetHashCode();
        }
    }
    public class RemoveDuplicate
    {
        public List<ElemRelation> removeDuplicate()
        {
            List<ElemRelation> table = RepositoryForRelation.storageForRelationship_;
            var distinctItems = table.Distinct(new DistinctItemComparer());
            return distinctItems.ToList();
        }
    }
    public class Display
    {
        
        // ----------------< display arguments >----------------
        public static void displayArgument(string[] args, string[] files, string path, List<string> patterns, List<string> options)
        {
            Console.Write("\n \t\t\tParsing of Commandline Arguments Are:\n");
            Console.Write("==============================================================================");
            Console.Write("\n\n Arguments: ");

            foreach (string arg in args)
            {
                Console.Write("  {0}", arg);
            }

            Console.Write("\n\n Path: {0}\n", path);
            Console.Write("\n List of Files:");
            foreach (string file in files)
                Console.Write("\n  {0}", file);

            Console.Write("\n\n File Pattern: ");
            foreach (string patt in patterns)
                Console.Write("{0}\t", patt);

            Console.Write("\n Options: ");
            foreach (string patt in options)
                Console.Write("{0}\t", patt);
            Console.Write("\n\n");
        }


        // ----------------< to display diffrent heading before actual file data >----------------
        private static void displayFileHeading(string filename)
        {
            Console.Write("\n\n Processing Files: {0}", filename);
            Console.Write("\n --------------------------------------------------------------------\n");
        }

        private static void displayFileHeading1(string filename)
        {
            Console.Write("\n\n Processing Files: {0}", filename);
            Console.Write("\n --------------------------------------------------------------------\n");
            Console.Write(" {0,9} {1,25} {2,14} {3,14}\n", "Type", "Name", "Size", "Complexity");
        }

        private static void displayFileHeading2(string filename)
        {
            Console.Write("\n\n Processing Files: {0}", filename);
            Console.Write("\n --------------------------------------------------------------------\n");
            Console.Write(" {0,5} {1,25} {2,25}\n", "RelationShip", "Type1", "Type2" );
        }

        // -------< display all the types defined and function with entering and leaving line >----------
        public static void displayTypeDefined(string[] files)
        {
            Console.Write("\n\n=================== Types Defined and Function of the File Set =================");
            List<Elem> table = RepositoryForOutput.storageForOutput_;
            displayFileHeading(files[0]);
            int i = 0;

            foreach (Elem e in table)
            {
            check:
                if (!e.filename.Equals(files[i]))
                {
                    i++;
                    displayFileHeading(files[i]);
                    goto check;
                }
                Console.Write("\n {0,10} ===> {1,20}  Entering: {2,2}  Leaving: {3,2}", e.type, e.name, e.begin, e.end);
            }

            for (int j = i + 1; j <= files.Length - 1; j++)
            {
                displayFileHeading(files[j]);
            }

            Console.WriteLine();
        }


        // ----------------< display file summary >----------------
        private static void displaySummary(ref int[] counterForAll)
        {
            Console.Write("\n\n ----File Summary:----\n");

            Console.Write(" namespace:{0,3}\n delegate: {1,3}\n", counterForAll[0], counterForAll[1]);
            Console.Write(" class:    {0,3}\n interface:{1,3}\n", counterForAll[2], counterForAll[3]);
            Console.Write(" struct:   {0,3}\n enum:     {1,3}\n", counterForAll[4], counterForAll[5]);
            Console.Write(" function: {0,3}\n", counterForAll[6]);
            Console.WriteLine();

            Array.Clear(counterForAll, 0, counterForAll.Length);
    
        }


        // ----------------< increament counter of types defined and function >----------------
        private static void counterIncreament(ref int[] counterForAll, string type)
        {           
            if (type.Equals("namespace"))
                counterForAll[0]++;
            if (type.Equals("delegate"))
                counterForAll[1]++;
            if (type.Equals("class"))
                counterForAll[2]++;
            if (type.Equals("interface"))
                counterForAll[3]++;
            if (type.Equals("struct"))
                counterForAll[4]++;
            if (type.Equals("enum"))
                counterForAll[5]++;
            if (type.Equals("function"))
                counterForAll[6]++;
        }

        //----------------< display complexity and size of function file by file >----------------
        public static void displayOutput(string[] files)
        {
            Console.Write("\n\n======================= Output with Size and Complexity ========================");
            List<Elem> table = RepositoryForOutput.storageForOutput_;
            displayFileHeading1(files[0]);
            int[] counterForAll = new int[7]; 
            int i = 0;
            string lastfile = null;
            foreach (Elem e in table)
            {

            check:
                if (!e.filename.Equals(files[i]))
                {
                    displaySummary(ref counterForAll);
                    i++;
                    displayFileHeading1(files[i]);
                    goto check;
                }

                if (e.type.Equals("function"))
                {
                    Console.Write("\n {0,10}, {1,25}, {2,10}, {3,10}", e.type, e.name, e.end - e.begin, e.complexity);
                    counterIncreament(ref counterForAll, e.type);
                }
                else
                {
                    Console.Write("\n {0,10}, {1,25},", e.type, e.name);
                    counterIncreament(ref counterForAll, e.type);
                }
             lastfile = e.filename;                
            }
            if (files[i].Equals(lastfile))
            {
                displaySummary(ref counterForAll);
            }
            for (int j = i + 1; j <= files.Length - 1; j++)
            {
                displayFileHeading1(files[j]);
                displaySummary(ref counterForAll);
            }          
        }


        // ----------------< display Relationship data file by file >----------------
        public static void displayRelation(string[] files)
        {
            Console.Write("\n\n======================= Output with a Relationship Data ========================");

            RemoveDuplicate d = new RemoveDuplicate();
            List<ElemRelation> table = d.removeDuplicate();

            displayFileHeading2(files[0]);
            int i = 0;

            foreach (ElemRelation e in table)
            {

            check:
                if (!e.filename.Equals(files[i]))
                {
                    i++;
                    displayFileHeading2(files[i]); ;
                    goto check;
                }
                Console.Write("\n {0,5}, {1,25}, {2,25}", e.relationType, e.fromName, e.toName);
            }

            for (int j = i + 1; j <= files.Length - 1; j++)
            {
                displayFileHeading2(files[j]);
            }
            Console.WriteLine();
        }
        private static string[] getFiles(string path, List<string> patterns)
        {
            List<string> files = new List<string>();
            foreach (string pattern in patterns)
            {
                string[] newFiles = Directory.GetFiles(path, pattern);
                for (int i = 0; i < newFiles.Length; ++i)
                    newFiles[i] = Path.GetFullPath(newFiles[i]);

                files.AddRange(newFiles);
            }
            return files.ToArray();
        }

        //----------------< Test Stub data for repository >----------------
        private static void repositoryForTestUP(string filename)
        {
            Elem e = new Elem();
            e.type = "namespace";
            e.name = "karan";
            e.begin = 10;
            e.end = 70;
            e.filename = filename;
            RepositoryForOutput.storageForOutput_.Add(e);
            Elem e1 = new Elem();
            e1.type = "class";
            e1.name = "Display";
            e1.begin = 12;
            e1.end = 68;
            e1.filename = filename;
            RepositoryForOutput.storageForOutput_.Add(e1);
            Elem e2 = new Elem();
            e2.type = "function";
            e2.name = "test";
            e2.begin = 30;
            e2.end = 65;
            e2.filename = filename;
            e2.complexity = 5;
            RepositoryForOutput.storageForOutput_.Add(e2);
            ElemRelation er = new ElemRelation();
            er.relationType = "Aggregation";
            er.fromName = "Display";
            er.toName = "Xyz";
            er.filename = filename;
            RepositoryForRelation.storageForRelationship_.Add(er);
            ElemRelation er1 = new ElemRelation();
            er1.relationType = "Inheritance";
            er1.fromName = "Display";
            er1.toName = "Test";
            er1.filename = filename;
            RepositoryForRelation.storageForRelationship_.Add(er1);
            ElemRelation er2 = new ElemRelation();
            er2.relationType = "Composition";
            er2.fromName = "Display";
            er2.toName = "ValueType";
            er2.filename = filename;
            RepositoryForRelation.storageForRelationship_.Add(er2);
        }
#if(TEST_DISPLAY)
        static void Main(string[] args)
        {
            Console.Write("\n Testing Display Package");
            Console.Write("\n =======================\n");

            string path = "../../";
            string[] arg = { "../../", "*.cs"};
            List<string> patterns = new List<string>();
            patterns.Add("*.cs");
            List<string> options = new List<string>();
            string[] files = getFiles(path, patterns);

            repositoryForTestUP(files[0]);
            displayArgument(arg, files, path, patterns, options);
            displayTypeDefined(files);
            displayOutput(files);
            displayRelation(files);
        }
#endif
    }
}
