using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp25
{
  
    public abstract class GraphObject : ICloneable
    {
        public abstract void Draw();

        public abstract object Clone();
    }

  
    public class Point : GraphObject
    {
        private double x;
        private double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

      
        public override void Draw()
        {
            Console.WriteLine($"Рисуем точку в координатах ({x}, {y})");
        }

       
        public override object Clone()
        {
            return new Point(this.x, this.y);
        }

       
        public double X => x;
        public double Y => y;
    }

   
    public class Line : GraphObject
    {
        private Point start;
        private Point end;

       
        public Line(Point start, Point end)
        {
            this.start = start;
            this.end = end;
        }

       
        public override void Draw()
        {
            Console.WriteLine($"Рисуем линию от ({start.X}, {start.Y}) до ({end.X}, {end.Y})");
        }

       
        public override object Clone()
        {
           
            return new Line((Point)this.start.Clone(), (Point)this.end.Clone());
        }

        
        public Point Start => start;
        public Point End => end;
    }

    
    public class Circle : GraphObject
    {
        private Point center;
        private double radius;

        
        public Circle(Point center, double radius)
        {
            this.center = center;
            this.radius = radius;
        }

       
        public override void Draw()
        {
            Console.WriteLine($"Рисуем круг с центром в ({center.X}, {center.Y}) и радиусом {radius}");
        }

       
        public override object Clone()
        {
           
            return new Circle((Point)this.center.Clone(), this.radius);
        }

        
        public Point Center => center;
        public double Radius => radius;
    }

    
    class Program
    {
        static void Main(string[] args)
        {
           
            Point point1 = new Point(1, 1);
            Point point2 = new Point(4, 5);

            Line line = new Line(point1, point2);
            Circle circle = new Circle(point1, 3.5);

            
            Console.WriteLine("Оригинальные объекты:");
            point1.Draw();
            line.Draw();
            circle.Draw();

            
            Point pointClone = (Point)point1.Clone();
            Line lineClone = (Line)line.Clone();
            Circle circleClone = (Circle)circle.Clone();

            Console.WriteLine("\nКлонированные объекты:");
            pointClone.Draw();
            lineClone.Draw();
            circleClone.Draw();
            Console.ReadLine();
        }
    }
}
