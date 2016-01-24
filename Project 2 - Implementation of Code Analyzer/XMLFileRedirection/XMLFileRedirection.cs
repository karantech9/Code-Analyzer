///////////////////////////////////////////////////////////////////////
// XMLFileRedirection.cs - store output data in to XML file          //
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
 *   XMLFileRedirection - which used data stored in RepositoryForOutput
 *   & RepositoryForRelation for storing output in XML File.
 */
/* Required Files:
 *   Parser.cs 
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
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

namespace CodeAnalyzer
{
    public class XMLFileRedirection
    {      
        // ---------------< store output for option R >----------------
        private void xmlForRelation()
        {
            List<ElemRelation> relation = RepositoryForRelation.storageForRelationship_;
            int relation_size = relation.Count;
            if (relation_size == 0)
                return;
            if (relation.Count == 0)
            {
                Console.Write("\n There is not Relationship Data for XML File. ");
                return;
            }
            Console.Write("\n Create XML file using XDocument for Relationship");
            Console.Write("\n ================================================\n\n");

            XDocument xml = new XDocument();
            xml.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            XComment comment = new XComment("Demonstration XML");
            xml.Add(comment);
            XElement root = new XElement("CODEANALYSIS");
            xml.Add(root);
            foreach (ElemRelation e in relation)
            {                                                                      
                XElement relationship = new XElement("RelationshipType");
                root.Add(relationship);
                XElement relationshipType = new XElement("Relation", Convert.ToString(e.relationType));
                relationship.Add(relationshipType);

                XElement from = new XElement("FROM");
                root.Add(from);
                XElement fromF = new XElement("From", Convert.ToString(e.fromName));
                from.Add(fromF);

                XElement to = new XElement("TO");
                root.Add(to);
                XElement toF = new XElement("To", Convert.ToString(e.toName));
                to.Add(toF);

                xml.Save("X_Relationship_Data.xml");
            }
            Console.Write(" The Relationships is stored in:\n");
            Console.Write(" " + Directory.GetCurrentDirectory() + "\\X_Relationship_Data.xml\n\n\n");
        }

        // ----------< store size and complexity of function along with data types >--------------
        public void displayxml(bool R, string[] files)
        {        
                List<Elem> table = RepositoryForOutput.storageForOutput_;
                int table_size = table.Count;
                if (table_size == 0)
                {
                    Console.Write("\n There is not Data for XML File. ");
                    return;
                }
                Console.Write("\n\n\n Create XML file using XDocument");
                Console.Write("\n =================================\n\n");
                int i = 0;
                int j = 0;
                XDocument xml = new XDocument();
                xml.Declaration = new XDeclaration("1.0", "utf-8", "yes");
                XComment comment = new XComment("Project2 XML");
                xml.Add(comment);
                XElement root = new XElement("PROJECT2");
                xml.Add(root);
                XElement file = null;
                XElement ns = null;
                XElement cl = null;
                foreach (Elem elem in table)
                {
                check:
                    if (elem.filename.Equals(files[i]))
                    {


                        if (j == 0)
                        {
                            file = new XElement("FileName", elem.filename);
                            root.Add(file);
                            j++;
                        }
                        if (elem.type.Equals("namespace"))
                        {
                            ns = new XElement("NameSpace", elem.name);
                            file.Add(ns);
                        }
                        if (elem.type.Equals("class"))
                        {
                            cl = new XElement("Class", elem.name);
                            ns.Add(cl);

                        }
                        if (elem.type.Equals("function"))
                        {
                            XElement fun = new XElement("function", elem.name);
                            cl.Add(fun);
                            int sizei = elem.end - elem.begin;
                            XElement size = new XElement("Size", Convert.ToString(sizei));
                            fun.Add(size);
                            XElement complexity = new XElement("Complexity", Convert.ToString(elem.complexity));
                            fun.Add(complexity);
                        }

                        xml.Save("X_Analysis_Data.xml");
                    }
                    else 
                    {
                        j = 0;
                        i++;
                        goto check;
                    }

                }
                Console.Write(" The Size and Complexity is stored in:\n");
                Console.Write(" " + Directory.GetCurrentDirectory() + "\\X_Analtsis_Data.xml\n\n\n");
             if (R)
                 xmlForRelation();
            }
        private static void repositoryForTestUP()
        {
            Elem e = new Elem();
            e.type = "namespace";
            e.name = "karan";
            e.begin = 10;
            e.end = 70;
            RepositoryForOutput.storageForOutput_.Add(e);
            Elem e1 = new Elem();
            e1.type = "class";
            e1.name = "Display";
            e1.begin = 12;
            e1.end = 68;
            RepositoryForOutput.storageForOutput_.Add(e1);
            Elem e2 = new Elem();
            e2.type = "function";
            e2.name = "test";
            e2.begin = 30;
            e2.end = 65;
            e2.complexity = 5;
            RepositoryForOutput.storageForOutput_.Add(e2);
            ElemRelation er = new ElemRelation();
            er.relationType = "Aggregation";
            er.fromName = "Display";
            er.toName = "Xyz";
            RepositoryForRelation.storageForRelationship_.Add(er);
            ElemRelation er1 = new ElemRelation();
            er1.relationType = "Inheritance";
            er1.fromName = "Display";
            er1.toName = "Test";
            RepositoryForRelation.storageForRelationship_.Add(er1);
            ElemRelation er2 = new ElemRelation();
            er2.relationType = "Composition";
            er2.fromName = "Display";
            er2.toName = "ValueType";
            RepositoryForRelation.storageForRelationship_.Add(er2);
        }
        
#if(TEST_XMLFILE)
        static void Main(string[] args)
        {
            Console.Write("\n Testing XML File Output");
            Console.Write("\n ========================\n");
            bool R = true;
            string[] files = null;
            repositoryForTestUP();
            XMLFileRedirection xml = new XMLFileRedirection();
            xml.displayxml(R, files);
        }
#endif
    }
}
