using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VouchReputationSystem.Classes;

namespace VouchReputationSystem
{
    static class Program
    {
        // The main entry point for the application.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DiagramForm Diagram = new DiagramForm(CreateNetwork());
            Application.Run(Diagram);
        }
        static Network CreateNetwork()
        {
            AccountChain adam = new AccountChain("Adam");

            //The bottom part---------------------

            AccountChain noah = new AccountChain("Noah");
            AccountChain jacob = new AccountChain("Jacob");
            AccountChain simon = new AccountChain("Simon");

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

            AccountChain eli = new AccountChain("Eli");
            AccountChain peter = new AccountChain("Peter");
            AccountChain joseph = new AccountChain("Joseph");
            AccountChain john = new AccountChain("John");

            adam.VouchFor(eli);
            eli.VouchFor(adam);

            peter.VouchFor(eli);
            eli.VouchFor(peter);

            joseph.VouchFor(eli);
            eli.VouchFor(joseph);

            joseph.VouchFor(john);
            john.VouchFor(joseph);

            //------------------------------------

            AccountChain abel = new AccountChain("Abel");
            AccountChain eve = new AccountChain("Eve");
            AccountChain cain = new AccountChain("Cain");

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

            //Creates the networks through Adam's perspective
            Network _network = new Network(adam);
            _network.PrintAllNodes();
            return _network;
        }
    }
}
