// A Force-Directed Diagram Layout Algorithm
// Bradley Smith - 2015/02/28

using System;
using System.Drawing;

namespace VouchReputationSystem.Classes
{
	/// <summary>
	/// Represents a 2-dimensional vector where each component is a <see cref="System.Double"/>.
	/// </summary>
	public struct Vector
	{

		/// <summary>
		/// Gets or sets the X component of the vector.
		/// </summary>
		public double X { get; set; }
		/// <summary>
		/// Gets or sets the Y component of the vector.
		/// </summary>
		public double Y { get; set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="Vector"/> structure with the specified magnitude and direction.
		/// </summary>
		/// <param name="magnitude"></param>
		/// <param name="direction"></param>
		public Vector(double magnitude, double direction) : this()
		{
			X = magnitude * Math.Cos((Math.PI / 180.0) * direction);
			Y = magnitude * Math.Sin((Math.PI / 180.0) * direction);
		}

		/// <summary>
		/// Adds two vectors together.
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public static Vector operator +(Vector a, Vector b)
		{
			Vector result = new Vector();
			result.X = a.X + b.X;
			result.Y = a.Y + b.Y;
			return result;
		}

		/// <summary>
		/// Multiplies a vector by a scalar value.
		/// </summary>
		/// <param name="vector"></param>
		/// <param name="multiplier"></param>
		/// <returns></returns>
		public static Vector operator *(Vector vector, double multiplier)
		{
			Vector result = new Vector();
			result.X = vector.X * multiplier;
			result.Y = vector.Y * multiplier;
			return result;
		}

		/// <summary>
		/// Returns a <see cref="System.Drawing.Point"/> that is equivalent to the vector.
		/// </summary>
		/// <returns></returns>
		public Point ToPoint()
		{
			return new Point((int)X, (int)Y);
		}

		/// <summary>
		/// Returns a string representation of the vector.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return String.Format("({0}, {1})", X.ToString("N2"), Y.ToString("N2"));
		}
	}
}