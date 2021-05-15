using System;
using System.Collections.Generic;

namespace VouchReputationSystem
{
    class Program
    {
        static void Main(string[] args)
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

            //Creates the networks through Adam's perspective
            Network _network = new Network(adam);
            _network.GetNodeWithAccount(simon).PrintImmediateVouches();
            _network.PrintAllNodes();
        }
    }
}
