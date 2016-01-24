///////////////////////////////////////////////////////////////////////////
// IRuleAndAction.cs - Interfaces & abstract bases for rules and actions //
// ver 1.2                                                               //
// Language:    C#, Visual Studio 13.0, .Net Framework 4.5               //
// Platform:    Dell Inspiron 17, Windows 8                              //
// Application: Demonstration for CIS 681, Project #2, Fall 2014         //
// Author:      Karankumar Patel, Syracuse University                    //
//              (315) 751-5637, khpatel@syr.edu                          //
// Source:      Jim Fawcett, CST 4-187, Syracuse University              //
//              (315) 443-3948, jfawcett@twcny.rr.com                    //
///////////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * ------------------
 * This module defines the following classes:
 *   IRule   - interface contract for Rules
 *   ARule   - abstract base class for Rules that defines some common ops
 *   IAction - interface contract for rule actions
 *   AAction - abstract base class for actions that defines common ops
 */
/* Required Files:
 *   IRuleAndAction.cs
 *   
 * Build command:
 *   Interfaces and abstract base classes only so no build
 *   
 * Maintenance History:
 * --------------------
 * ver 1.2 : 9 Oct 2014
 * - added filename arguments in doAction to store filename with 
 *   all the types and function
 * ver 1.1 : 11 Sep 2011
 * - added properties displaySemi and displayStack
 * ver 1.0 : 28 Aug 2011
 * - first release
 *
 * Note:
 * This package does not have a test stub as it contains only interfaces
 * and abstract classes.
 *
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace CodeAnalyzer
{
    /////////////////////////////////////////////////////////
    // contract for actions used by parser rules

    public interface IAction
    {
        void doAction(CSsemi.CSemiExp semi, string filename);
    }
    /////////////////////////////////////////////////////////
    // abstract action base supplying common functions

    public abstract class AAction : IAction
    {
        static bool displaySemi_ = false;   // default
        static bool displayStack_ = false;  // default

        public abstract void doAction(CSsemi.CSemiExp semi, string filename);

        public static bool displaySemi
        {
            get { return displaySemi_; }
            set { displaySemi_ = value; }
        }
        public static bool displayStack
        {
            get { return displayStack_; }
            set { displayStack_ = value; }
        }

        public virtual void display(CSsemi.CSemiExp semi)
        {
            if (displaySemi)
                for (int i = 0; i < semi.count; ++i)
                    Console.Write("{0} ", semi[i]);
        }
    }
    /////////////////////////////////////////////////////////
    // contract for parser rules

    public interface IRule
    {
        bool test(CSsemi.CSemiExp semi, string filename);
        void add(IAction action);
    }
    /////////////////////////////////////////////////////////
    // abstract rule base implementing common functions

    public abstract class ARule : IRule
    {
        private List<IAction> actions;
        public ARule()
        {
            actions = new List<IAction>();
        }
        public void add(IAction action)
        {
            actions.Add(action);
        }
        abstract public bool test(CSsemi.CSemiExp semi, string filename);
        public void doActions(CSsemi.CSemiExp semi, string filename)
        {
            foreach (IAction action in actions)
                action.doAction(semi, filename);
        }
        public int indexOfType(CSsemi.CSemiExp semi)
        {
            int indexCL = semi.Contains("class");
            int indexIF = semi.Contains("interface");
            int indexST = semi.Contains("struct");
            int indexEN = semi.Contains("enum");

            int index = Math.Max(indexCL, indexIF);
            index = Math.Max(index, indexST);
            index = Math.Max(index, indexEN);
            return index;
        }
    }
}

