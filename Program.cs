using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
class Program
{
    static void Main()
    {
        Console.WriteLine("Введите целое положительное число N (3 ≤  N ≤  10):");
        int N = ReadInt(3, 10);

        List<PointF> points = new List<PointF>();

        for (int i = 0; i < N; i++)
        {
            Console.WriteLine($"Введите координаты точки {i + 1} (X Y):");
            string[] input = Console.ReadLine().Split(' ');
            if (input.Length != 2 || !float.TryParse(input[0], out float x) || !float.TryParse(input[1], out float y))
            {
                Console.WriteLine("Неверный формат ввода. Введите координаты вещественных чисел через пробел.");
                i--;
                continue;
            }
            points.Add(new PointF(x, y));
        }

        List<PointF> convexHull = CalculateConvexHull(points);
        float area = CalculateArea(convexHull);

        Console.WriteLine("Координаты точек выпуклой оболочки:");
        foreach (var point in convexHull)
        {
            Console.WriteLine($"({point.X}, {point.Y})");
        }

        Console.WriteLine($"Площадь фигуры, образованной выпуклой оболочкой: {area}");
    }

    static List<PointF> CalculateConvexHull(List<PointF> points)
    {
        if (points.Count < 3)
        {
            return points;
        }

        List<PointF> convexHull = new List<PointF>();

        PointF pivot = points[0];

        foreach (var point in points)
        {
            if (point.Y < pivot.Y || (point.Y == pivot.Y && point.X < pivot.X))
            {
                pivot = point;
            }
        }

        convexHull.Add(pivot);
        points.Remove(pivot);

        points.Sort((a, b) =>
        {
            float angleA = (float)Math.Atan2(a.Y - pivot.Y, a.X - pivot.X);
            float angleB = (float)Math.Atan2(b.Y - pivot.Y, b.X - pivot.X);
            if (angleA < angleB) return -1;
            if (angleA > angleB) return 1;
            return Distance(a, pivot).CompareTo(Distance(b, pivot));
        });

        convexHull.Add(points[0]);

        for (int i = 1; i < points.Count;)
        {
            int j = convexHull.Count - 1;
            var current = convexHull[j];
            var prev = convexHull[j - 1];
            var next = points[i];

            float crossProduct = CrossProduct(prev, current, next);

            if (crossProduct < 0)
            {
                convexHull.RemoveAt(j);
            }
            else
            {
                convexHull.Add(next);
                i++;
            }
        }

        return convexHull;
    }

    static float CalculateArea(List<PointF> convexHull)
    {
        float area = 0;
        int n = convexHull.Count;
        for (int i = 0; i < n; i++)
        {
            PointF current = convexHull[i];
            PointF next = convexHull[(i + 1) % n];
            area += current.X * next.Y - current.Y * next.X;
        }
        return Math.Abs(area) / 2;
    }

    static float Distance(PointF a, PointF b)
    {
        float dx = a.X - b.X;
        float dy = a.Y - b.Y;
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    static float CrossProduct(PointF a, PointF b, PointF c)
    {
        return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
    }

    static int ReadInt(int min, int max)
    {
        int value;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out value) && value >= min && value <= max)
            {
                break;
            }
            else
            {
                Console.WriteLine($"Неверные данные. Введите целое положительное число в диапазоне ({min}, {max}).");
            }
        }
        return value;
    }
}
