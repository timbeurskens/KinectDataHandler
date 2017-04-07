using System;
using System.Globalization;

namespace KinectDataHandler.Linear3DTools
{
    /// <summary>
    /// Immutable class representing a 3D vector
    /// Properties: X, Y, Z
    /// Methods:
    /// </summary>
    public class Vector3D : IVector3D
    {
        public static readonly Vector3D Origin3D = new Vector3D(0, 0, 0);
        public static readonly Vector3D XUnit3D = new Vector3D(1, 0, 0);
        public static readonly Vector3D YUnit3D = new Vector3D(0, 1, 0);
        public static readonly Vector3D ZUnit3D = new Vector3D(0, 0, 1);
        public static readonly Vector3D Unit3D = new Vector3D(1, 1, 1);

        public double X { get; }
        public double Y { get; }
        public double Z { get; }

        /// <summary>
        /// Vector constructor
        /// </summary>
        /// <param name="z">X</param>
        /// <param name="y">Y</param>
        /// <param name="x">Z</param>
        public Vector3D(double z, double y, double x)
        {
            Z = z;
            Y = y;
            X = x;
        }

        /// <summary>
        /// Vector constructor
        /// </summary>
        public Vector3D()
        {
            Z = Origin3D.X;
            Y = Origin3D.Y;
            X = Origin3D.Z;
        }

        /// <summary>
        /// Vector constructor
        /// </summary>
        public Vector3D(double unit)
        {
            var scaledUnitVector = Unit3D * unit;
            Z = scaledUnitVector.X;
            Y = scaledUnitVector.X;
            X = scaledUnitVector.X;
        }

        /// <summary>
        /// Adds a vector to the current vector and returns the resulting vector
        /// </summary>
        /// <param name="vec">Vector to be added</param>
        /// <returns>New vector V: V == this + vec</returns>
        public IVector3D Add(IVector3D vec)
        {
            return new Vector3D(X + vec.X, Y + vec.Y, Z + vec.Z);
        }

        /// <summary>
        /// Multiplies the current vector with the scaling vector
        /// </summary>
        /// <param name="scalar">Scaling vector</param>
        /// <returns>New vector V: V == this \times scalar</returns>
        public IVector3D Scale(IVector3D scalar)
        {
            return new Vector3D(X * scalar.X, Y * scalar.Y, Z * scalar.Z);
        }

        /// <summary>
        /// Scales the vector by a constant value
        /// </summary>
        /// <param name="scalar">Value to multiply with</param>
        /// <returns>New vector V: V == this \times scalar</returns>
        public IVector3D Scale(double scalar)
        {
            return Scale(FromConstant(scalar));
        }

        public double Dot(IVector3D vec)
        {
            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public IVector3D Cross(IVector3D vec)
        {
            return (Vector3D)(XUnit3D * (Y * vec.Z - Z * vec.Y)) - (Vector3D)(YUnit3D * (X * vec.Z - Z * vec.X)) + (Vector3D)(ZUnit3D * (X * vec.Y - Y * vec.X));
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public double Angle(IVector3D vec)
        {
            var dot = this * vec;
            var len = Length() * vec.Length();
            return Math.Acos(dot / len);
        }

        /// <summary>
        /// Creates a new vector with constant parameters c
        /// </summary>
        /// <param name="c">Constant for the new vector</param>
        /// <returns>New vector V: V == (c, c, c)</returns>
        public static Vector3D FromConstant(double c)
        {
            return new Vector3D(c, c, c);
        }

        /// <summary>
        /// Creates the vector v from p1 to p2
        /// </summary>
        /// <param name="p1">point 1</param>
        /// <param name="p2">point 2</param>
        /// <returns>vector v</returns>
        public static Vector3D FromPoints(IPoint3D p1, IPoint3D p2)
        {
            return new Vector3D(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
        }

        /// <summary>
        /// Creates a vector v from origin to p
        /// </summary>
        /// <param name="p">point p</param>
        /// <returns>vector v</returns>
        public static Vector3D FromPoints(IPoint3D p)
        {
            return FromPoints(Origin3D, p);
        }

        public static IVector3D operator +(Vector3D a, IVector3D b)
        {
            return a.Add(b);
        }

        public static IVector3D operator +(Vector3D a, Vector3D b)
        {
            return a.Add(b);
        }

        public static IVector3D operator +(IVector3D a, Vector3D b)
        {
            return a.Add(b);
        }

        public static IVector3D operator -(IVector3D a, Vector3D b)
        {
            return a.Add(b.Scale(-1));
        }

        public static IVector3D operator -(Vector3D a, IVector3D b)
        {
            return a.Add(b.Scale(-1));
        }

        public static IVector3D operator -(Vector3D a, Vector3D b)
        {
            return a.Add(b.Scale(-1));
        }

        public static IVector3D operator *(Vector3D a, double b)
        {
            return a.Scale(b);
        }

        public static IVector3D operator *(double a, Vector3D b)
        {
            return b.Scale(a);
        }

        public static double operator *(Vector3D a, IVector3D b)
        {
            return a.Dot(b);
        }

        public static double operator *(IVector3D a, Vector3D b)
        {
            return a.Dot(b);
        }

        public static double operator *(Vector3D a, Vector3D b)
        {
            return a.Dot(b);
        }

        public override string ToString()
        {
            var nfi = new NumberFormatInfo {NumberDecimalSeparator = "."};
            return $"({X.ToString(nfi)}, {Y.ToString(nfi)}, {Z.ToString(nfi)})";
        }

        IPoint3D IPoint3D.Add(IVector3D vec)
        {
            return Add(vec);
        }

        public IVector3D Invert()
        {
            return Scale(-1);
        }
    }

    internal class LineSeg3D : ICurve3D
    {
        public Vector3D Point1 { get; }
        public Vector3D Point2 { get; }

        public LineSeg3D(Vector3D point1, Vector3D point2)
        {
            Point1 = point1;
            Point2 = point2;
        }

        public bool IsOn(IPoint3D point)
        {
            throw new NotImplementedException();
        }

        public ISurface3D Normal(IPoint3D point)
        {
            throw new NotImplementedException();
        }

        public IVector3D Tangent(IPoint3D point)
        {
            throw new NotImplementedException();
        }
    }

    public interface ISurface3D
    {
        /// <summary>
        /// Returns whether point is on the surface
        /// </summary>
        /// <param name="point">The point to check</param>
        /// <returns>True if point is on the surface, false otherwise</returns>
        bool IsOn(IPoint3D point);

        /// <summary>
        /// Returns the normal vector of the surface at point
        /// </summary>
        /// <param name="point">The point to get the normal vector from</param>
        /// <returns>IVector3D v such that v is perpendicular to the surface at point</returns>
        IVector3D Normal(IPoint3D point);

        /// <summary>
        /// Return a tangent surface at point
        /// </summary>
        /// <param name="point">The point to calculate from</param>
        /// <returns>A surface such that the normal of that surface is always equal to the normal of this at point</returns>
        ISurface3D Tangent(IPoint3D point);
    }

    public interface ICurve3D
    {
        bool IsOn(IPoint3D point);
        ISurface3D Normal(IPoint3D point);
        IVector3D Tangent(IPoint3D point);
    }

    public interface IPoint3D
    {
        double X { get; }
        double Y { get; }
        double Z { get; }

        IPoint3D Add(IVector3D vec);
    }

    public interface IVector3D : IPoint3D
    {
        double Length();
        new IVector3D Add(IVector3D vec);
        IVector3D Scale(double scalar);
        IVector3D Scale(IVector3D scalar);
        IVector3D Invert();
        double Dot(IVector3D vec);
        IVector3D Cross(IVector3D vec);
    }

    internal abstract class Surface3D : ISurface3D
    {
        public bool IsOn(IPoint3D point)
        {
            throw new NotImplementedException();
        }

        public IVector3D Normal(IPoint3D point)
        {
            throw new NotImplementedException();
        }

        public ISurface3D Tangent(IPoint3D point)
        {
            throw new NotImplementedException();
        }
    }

    internal class Plane3D : Surface3D
    {
        
    }
}
