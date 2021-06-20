using Analytics.Nonlinear;
using Mathematics.NL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VouchReputationSystem.Classes.ReputationFunctions
{
    abstract class ReputationFunction
    {
        public Network network;
        public AccountNode observerNode;
        public List<AccountNode> allNodes = new List<AccountNode>();

        public ReputationFunction(Network _network)
        {
            this.network = _network;
            this.observerNode = _network.observerNode;
            this.allNodes = _network.allNodes;
        }

        public List<AccountNode> GetReputationList(List<AccountNode> _nodeList)
        {
            List<AccountNode> nodesWithoutObserver = new List<AccountNode>(_nodeList);
            nodesWithoutObserver.Remove(observerNode);

            List<string> variables = new List<string>();
            List<string> functions = new List<string>();
            foreach (AccountNode _node in nodesWithoutObserver)
            {
                variables.Add(_node.name);
                functions.Add(GetEquation(_node));

                Console.WriteLine(_node.name);
            }

            //This only happens if there are not bounds in the network
            if (variables.Count == 0)
                return allNodes;

            double[] reputationArray = LinearSystemSolver(variables.ToArray(), functions.ToArray());

            for (int i = 0; i < reputationArray.Length;i++)
            {
                nodesWithoutObserver[i].reputation = reputationArray[i];
            }

            nodesWithoutObserver.Add(observerNode);

            return nodesWithoutObserver;
        }

        public abstract string GetEquation(AccountNode _node);

        public double[] LinearSystemSolver(string[] variables, string[] functions)
        {
            // creating nonlinear system instance - Analytical System
            NonlinearSystem system = new AnalyticalSystem(variables, functions);

            double[] x0;
            SolverOptions options;
            double[] result;
            SolutionResult actual;

            // creating nonlinear solver instance - Newton-Raphson solver.
            NonlinearSolver solver = new NewtonRaphsonSolver();

            x0 = new double[] { 0.2, 0.2 }; // initial guess for variable values
            options = new SolverOptions() // options for solving nonlinear problem
            {
                MaxIterationCount = 100,
                SolutionPrecision = 1e-5
            };

            result = null;
            // solving the system
            actual = solver.Solve(system, x0, options, ref result);

            return result;
        }
    }
}
