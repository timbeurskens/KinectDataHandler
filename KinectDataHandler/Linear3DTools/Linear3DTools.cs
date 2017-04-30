using System;

namespace KinectDataHandler.Linear3DTools
{

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

    internal class Line3D : ICurve3D
    {
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
        IVector3D RotateX(double angle);
        IVector3D RotateY(double angle);
        IVector3D RotateZ(double angle);
        IVector3D Rotate(double rx, double ry, double rz);
        double Angle(IVector3D vec);
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
}
