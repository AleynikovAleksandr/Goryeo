using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class GraphObject : ICloneable
{
    public abstract void Draw();
    public abstract object Clone();

    public void AddToScene()
    {
        Scene.Instance.AddObject(this);
    }
}

public class Point : GraphObject
{
    private double x;
    private double y;

    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
        AddToScene();
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
        AddToScene();
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
        AddToScene();
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

public class Scene
{
    private List<GraphObject> objects = new List<GraphObject>();
    private static Scene instance;
    public event Action<GraphObject> ObjectAdded;

    private Scene() { }

    public static Scene Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Scene();
            }
            return instance;
        }
    }

    public void AddObject(GraphObject graphObject)
    {
        objects.Add(graphObject);
       
        ObjectAdded?.Invoke(graphObject);
    }

    public void Draw()
    {
        foreach (var obj in objects)
        {
            obj.Draw();
        }
    }
}

public class GraphObjectFactory
{
    public static Point CreatePoint(double x, double y)
    {
        return new Point(x, y);
    }

    public static Line CreateLine(Point start, Point end)
    {
        return new Line(start, end);
    }

    public static Circle CreateCircle(Point center, double radius)
    {
        return new Circle(center, radius);
    }
}

class Program
{
    static void Main(string[] args)
    {
       
        Point point1 = GraphObjectFactory.CreatePoint(1, 1);
        Point point2 = GraphObjectFactory.CreatePoint(4, 5);
        Line line = GraphObjectFactory.CreateLine(point1, point2);
        Circle circle = GraphObjectFactory.CreateCircle(point1, 3.5);

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

        Scene scene = Scene.Instance;
        scene.AddObject(point1);
        scene.AddObject(line);
        scene.AddObject(circle);

        Console.WriteLine();
        Console.WriteLine("Рисуем все объекты на сцене:");
        scene.Draw();

        Console.ReadLine();
    }
}
