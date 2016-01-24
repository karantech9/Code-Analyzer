///////////////////////////////////////////////////////////////////////
// FileManager.cs - Collect Files based on given path and patterns   //
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

namespace CodeAnalyzer
{
    public class FileManager
    {
        private List<string> files = new List<string>();
        private List<string> patterns = new List<string>();
        private bool recurse = false; // to fetch file from SubDirectories


        // ----------------< fetch files for a given path & options >---------------- 
        private void findFiles(string path)
        {
            if (patterns.Count == 0)
                addPattern("*.*");
            foreach (string pattern in patterns)
            {
                string[] newFiles = Directory.GetFiles(path, pattern);
                for (int i = 0; i < newFiles.Length; ++i)
                    newFiles[i] = Path.GetFullPath(newFiles[i]);

                files.AddRange(newFiles);
            }
            if (recurse)
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                    findFiles(dir);
            }
        }


        //----------------< add default pattern >----------------
        private void addPattern(string pattern)
        {
            patterns.Add(pattern);
        }

        public List<string> getFiles(string path, List<string> pattern, bool S)
        {
            patterns = pattern;
            recurse = S;

            findFiles(path);

            return files;
        }
#if(TEST_FILEMANAGER)
        static void Main(string[] args)
        {
            Console.Write("\n Testing Filemanager");
            Console.Write("\n ================");
            string path = "../..";
            bool s = true;

            FileManager fm = new FileManager();
            fm.findFiles(path);
            List<string> files = fm.getFiles(path, fm.patterns, s);
            foreach (string file in files)
                Console.Write("\n {0}\n", file);
            Console.WriteLine();
        }
#endif
    }
}
