// Status: Completed
using System;

namespace Project1
{
    public class V2
    {
        public float X, Y;

        public V2(double x, float y)
        {
            this.X = (float)x;
            this.Y = (float)y;
        }

        public V2(float x, float y)
        {
            this.X = (float)x;
            this.Y = (float)y;
        }

        public static V2 operator -(V2 v)
        {
            return new V2(-v.X, -v.Y);
        }

        public static V2 operator +(V2 a, V2 b)
        {
            return new V2(a.X + b.X, a.Y + b.Y);
        }

        public static V2 operator +(V2 a, float b)
        {
            return new V2(a.X + b, a.Y + b);
        }

        public static V2 operator -(V2 a, V2 b)
        {
            return new V2(a.X - b.X, a.Y - b.Y);
        }

        public static V2 operator -(V2 a, float b)
        {
            return new V2(a.X - b, a.Y - b);
        }

        public static V2 operator *(V2 a, float b)
        {
            return new V2(a.X * b, a.Y * b);
        }

        public static V2 operator *(float b, V2 a)
        {
            return new V2(a.X * b, a.Y * b);
        }

        public static V2 operator /(V2 a, float b)
        {
            return new V2(a.X / b, a.Y / b);
        }

        public static V2 NormalizeVector(V2 vector)
        {
            float length = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (length == 0)
                return vector; // Return the original vector if it's a zero vector.

            float x = vector.X / length;
            float y = vector.Y / length;
            return new V2(x, y);
        }

        public float MagSqr()
        {
            return (X * X) + (Y * Y);
        }

        public float Length()
        {
            return (float)Math.Sqrt(MagSqr());
        }

        public float Distance(V2 a)
        {
            return (float)Math.Sqrt((X - a.X) * (X - a.X) + (Y - a.Y) * (Y - a.Y));
        }

    }
}
