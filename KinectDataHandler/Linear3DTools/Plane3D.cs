using System;

namespace KinectDataHandler.Linear3DTools
{
    [Serializable]
    public class Plane3D : ISurface3D
    {
        private readonly double _a;
        private readonly double _b;
        private readonly double _c;
        private readonly double _d;

        public Plane3D(double a, double b, double c, double d)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
        }

        public Plane3D(IVector3D a, IVector3D b, IPoint3D c) : this(a.Cross(b), c)
        {
        }

        public Plane3D(IVector3D n, IPoint3D b)
        {
            _a = n.X;
            _b = n.Y;
            _c = n.Z;
            _d = -(_a * b.X + _b * b.Y + _c * b.Z);
        }

        public Plane3D(IPoint3D a, IPoint3D b, IPoint3D c)
            : this(Vector3D.FromPoints(a, b), Vector3D.FromPoints(a, c), a)
        {
        }

        public bool IsOn(IPoint3D p)
        {
            return _a * p.X + _b * p.Y + _c * p.Z + _d == 0;
        }

        public IVector3D Normal(IPoint3D point)
        {
            return Normal();
        }

        public IVector3D Normal() => new Vector3D(_a, _b, _c);

        public ISurface3D Tangent(IPoint3D point)
        {
            return this;
        }

        protected bool Equals(Plane3D other)
        {
            return _a.Equals(other._a) && _b.Equals(other._b) && _c.Equals(other._c) && _d.Equals(other._d);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _a.GetHashCode();
                hashCode = (hashCode * 397) ^ _b.GetHashCode();
                hashCode = (hashCode * 397) ^ _c.GetHashCode();
                hashCode = (hashCode * 397) ^ _d.GetHashCode();
                return hashCode;
            }
        }
    }
}