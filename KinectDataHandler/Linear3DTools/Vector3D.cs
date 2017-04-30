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
        public Vector3D(double x, double y, double z)
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
            var xpart = (Vector3D)((Y * vec.Z - Z * vec.Y) * XUnit3D);
            var ypart = (Vector3D)((Z * vec.X - X * vec.Z) * YUnit3D);
            var zpart = (Vector3D)((X * vec.Y - Y * vec.X) * ZUnit3D);

            return (Vector3D)(xpart + ypart) + zpart;
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

        public override bool Equals(object other)
        {
            var i = other as IVector3D;
            return X.Equals(i?.X) && Y.Equals(i?.Y) && Z.Equals(i?.Z);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
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

        public IVector3D RotateX(double angle)
        {
            return new Vector3D(X, Y * Math.Cos(angle) - Z * Math.Sin(angle), Y * Math.Sin(angle) + Z * Math.Cos(angle));
        }

        public IVector3D RotateY(double angle)
        {
            return new Vector3D(X * Math.Cos(angle) + Z * Math.Sin(angle), Y, - X * Math.Sin(angle) + Z * Math.Cos(angle));
        }

        public IVector3D RotateZ(double angle)
        {
            return new Vector3D(X * Math.Cos(angle) - Y * Math.Sin(angle), X * Math.Sin(angle) + Y * Math.Cos(angle), Z);
        }

        public IVector3D Rotate(double rx, double ry, double rz)
        {
            return RotateX(rx).RotateY(ry).RotateZ(rz);
        }
    }
}
