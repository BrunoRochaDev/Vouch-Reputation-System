using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Analytics;
using Mathematics.NL;
using Analytics.Formulae;
using Analytics.Syntactic;

namespace Analytics.Nonlinear
{
    /// <summary>
    /// Nonlinear System of Analytical Equations.
    /// </summary>
    public class AnalyticalSystem: ConstructedSystem
    {
        #region Analytical data members
        protected Translator translator;
        protected string[] functions;
		protected string[,] dfunctions;
        protected Formula[] formulae;
        protected Formula[,] fderivatives;
        #endregion Analytical data members

        /// <summary>
        /// Creates translator and adds Real variables to it.
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        protected bool AssignVariables(string[] variables)
        {
            if (variables == null || variables.Length == 0) return false;

            translator = new Translator();
            int l = variables.Length;
            for (int i = 0; i < l; i++)
            {
                if (!translator.Add(variables[i], (double)0.0))
                {
                    throw new InvalidNameException(variables[i]);
                }
            }

            return true;
        }

        /// <summary>
        /// Creates formulae for equations and their derivatives.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        protected bool AssignFunctions(string[] f)
        {
            if (f == null || f.Length == 0) return false;

            int l = f.Length;
            functions = new string[l];
            formulae = new Formula[l];
            for (int i = 0; i < l; i++)
            {
                if (!translator.CheckSyntax(f[i]))
                {
                    functions = null;
                    formulae = null;
                    return false;
                }

                functions[i] = f[i];
                formulae[i] = translator.BuildFormula(functions[i]);
                if (formulae[i].ResultType != typeof(double))
                {
                    Type t = formulae[i].ResultType;
                    functions = null;
                    formulae = null;
                    throw new WrongArgumentException("Function must return real value.", typeof(double), t);
                }
            }

			dfunctions = new string[l, l];
            fderivatives = new Formula[l,l];
            try
            {
                for (int i = 0; i < l; i++)
                {
                    for (int j = 0; j < l; j++)
                    {
						dfunctions[i, j] = translator.Derivative(functions[i], translator.Variables[j].Name);
						fderivatives[i, j] = translator.BuildFormula(dfunctions[i, j]);
                    }
                }
            }
            catch (Exception)
            {
                // if some derivative coud not be calculated -
                // the system does not support Jacobian.
                fderivatives = null;
            }

            return true;
        }

        /// <summary>
        /// Assigns variable values.
        /// </summary>
        /// <param name="values"></param>
        protected void AssignVariableValues(double[] values)
        {
            int l = values.Length;
            for (int i = 0; i < l; i++)
            {
                translator.Variables[i].Value = values[i];
            }
        }

        /// <summary>
        /// Calculates equation result for current variable values.
        /// </summary>
        /// <param name="i">Equation number</param>
        /// <returns></returns>
        protected double Equation(int i)
        {
            return (double)formulae[i].Calculate();
        }

        /// <summary>
        /// Calculates derivative result for current variable values.
        /// </summary>
        /// <param name="i">Equation number</param>
        /// <param name="j">Variable number</param>
        /// <returns></returns>
        protected double Derivative(int i, int j)
        {
            return (double)fderivatives[i,j].Calculate();
        }

        /// <summary>
        /// Creates equation (and derivative) delegates
        /// based on the created formulae objects.
        /// </summary>
        protected void CreateEquations()
        {
            int l = formulae.Length;
            equations = new Equation[l];
            for (int i = 0; i < l; i++)
            {
                // DO NOT remove temp variable,
                // using 'i' directly leads to incorrect lambda.
                int temp = i;
                        
                equations[i] = (double[] x) =>
                    {
                        AssignVariableValues(x);
                        return Equation(temp);
                    };
            }

            // if formulae of derivatives are not assigned -
            // system will not support Jacobian.
            if (fderivatives != null)
            {
                derivatives = new Derivative[l];
                for (int i = 0; i < l; i++)
                {
                    // DO NOT remove temp variable,
                    // using 'i' directly leads to incorrect lambda.
                    int tempi = i;

                    derivatives[i] = (int j, double[] x) =>
                    {
                        AssignVariableValues(x);
                        return Derivative(tempi, j);
                    };
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="variables">Variable names, the system must be resolved of.</param>
        /// <param name="f">System Equation Functions</param>
        public AnalyticalSystem(string[] variables, string[] f)
        {
            if (variables.Length != f.Length)
            {
                throw new WrongArgumentException("Variable count must be equal to equation count.", variables.Length, f.Length);    
            }

            if (!AssignVariables(variables))
            {
                return;
            }

            if (!AssignFunctions(f))
            {
                return;
            }

            CreateEquations();
        }

		/// <summary>
		/// Analytical equation system type description.
		/// </summary>
		/// <returns></returns>
		protected override string GetSystemType()
		{
			return "Analytical equations system";
		}

		/// <summary>
		/// Prints all data.
		/// </summary>
		/// <returns></returns>
	    public override string Print()
	    {
		    string result = base.Print();

		    int l = Dimension;
		    
			string s = string.Empty;
		    for (int i = 0; i < l; i++)
		    {
			    s += Environment.NewLine + ExpressionBuilder.BuildArrayIndexes(new List<string> {i.ToString()}) + "=" + functions[i];
		    }
		    result += Environment.NewLine + "Equations:" + s;

		    if (DerivativeSupported)
		    {
			    s = string.Empty;
			    for (int i = 0; i < l; i++)
			    {
				    for (int j = 0; j < l; j++)
				    {
					    s += Environment.NewLine +
					         ExpressionBuilder.BuildArrayIndexes(new List<string> {i.ToString(), translator.Variables[j].Name}) + "=" + dfunctions[i, j];
				    }
			    }
			    result += Environment.NewLine + "Jacobian:" + s;
		    }

		    return result;
	    }
    }
}
