using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using VouchReputationSystem.Classes;

using Mathematics.NL;
using System.Reflection;
using Analytics.Nonlinear;

namespace VouchReputationSystem
{
    static class Program
    {
        //Setups up the linear algebra library
        private static void LinearAlgebraSetup()
        {
            string[] req = new string[] { "Analytics.Real", "Analytics.Derivatives" };

            int n = req.Length;
            for (int i = 0; i < n; i++)
            {
                string sname = req[i];
                try
                {
                    Assembly.Load(sname);
                }
                catch (Exception)
                {
                }
            }
        }

        // The main entry point for the application.
        [STAThread]
        static void Main()
        {
            LinearAlgebraSetup();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DiagramForm Diagram = new DiagramForm(CreateNetwork());
            Application.Run(Diagram);
        }
      
        static Network CreateNetwork()
        {
            Account adam = new Account("Adam");

            //The bottom part---------------------

            Account noah = new Account("Noah");
            Account jacob = new Account("Jacob");
            Account simon = new Account("Simon");

            adam.VouchFor(noah);
            noah.VouchFor(adam);

            adam.VouchFor(jacob);
            jacob.VouchFor(adam);

            noah.VouchFor(jacob);
            jacob.VouchFor(noah);

            noah.VouchAgainst(simon);
            jacob.VouchAgainst(simon);

            //------------------------------------

            //The right part----------------------

            Account eli = new Account("Eli");
            Account peter = new Account("Peter");
            Account joseph = new Account("Joseph");
            Account john = new Account("John");

            adam.VouchFor(eli);
            eli.VouchFor(adam);

            eli.VouchAgainst(peter);

            joseph.VouchFor(eli);
            eli.VouchFor(joseph);

            joseph.VouchFor(john);
            john.VouchFor(joseph);

            //------------------------------------
            Account abel = new Account("Abel");
            Account eve = new Account("Eve");
            Account cain = new Account("Cain");

            abel.VouchFor(adam);
            eve.VouchFor(adam);
            cain.VouchFor(adam);

            adam.VouchFor(abel);
            adam.VouchFor(eve);
            adam.VouchFor(cain);

            abel.VouchFor(cain);
            cain.VouchFor(abel);

            eve.VouchAgainst(cain);
            abel.VouchAgainst(eve);
            
            //------------------------------------

            //Creates the networks through Adam's perspective
            Network _network = new Network(adam);
            _network.PrintAllNodes();
            return _network;
        }
    }
}
