using System;
using System.Collections.Generic;

public abstract class GraphObject : ICloneable
{
    public abstract void Draw();
    public abstract object Clone();

    public void AddToScene(Scene scene)
    {
        scene.AddObject(this);
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

public class Scene
{
    private List<GraphObject> objects = new List<GraphObject>();
    public string SceneType { get; }

    public Scene(string sceneType)
    {
        SceneType = sceneType;
    }

    public void AddObject(GraphObject graphObject)
    {
        objects.Add(graphObject);
    }

    public void Draw()
    {
        Console.WriteLine($"Рисуем {SceneType} сцену:");
        foreach (var obj in objects)
        {
            obj.Draw();
        }
    }
}

public abstract class GraphObjectFactory
{
    public abstract Point CreatePoint(double x, double y);
    public abstract Line CreateLine(Point start, Point end);
    public abstract Circle CreateCircle(Point center, double radius);
    public abstract Scene CreateScene();
}

public class ColorSceneFactory : GraphObjectFactory
{
    public override Point CreatePoint(double x, double y)
    {
        return new Point(x, y);
    }

    public override Line CreateLine(Point start, Point end)
    {
        return new Line(start, end);
    }

    public override Circle CreateCircle(Point center, double radius)
    {
        return new Circle(center, radius);
    }

    public override Scene CreateScene()
    {
        return new Scene("Цветная");
    }
}

public class BlackWhiteSceneFactory : GraphObjectFactory
{
    public override Point CreatePoint(double x, double y)
    {
        return new Point(x, y);
    }

    public override Line CreateLine(Point start, Point end)
    {
        return new Line(start, end);
    }

    public override Circle CreateCircle(Point center, double radius)
    {
        return new Circle(center, radius);
    }

    public override Scene CreateScene()
    {
        return new Scene("Черно-белая");
    }
}

public class TestSceneBuilder
{
    private GraphObjectFactory factory;

    public TestSceneBuilder(GraphObjectFactory factory)
    {
        this.factory = factory;
    }

    public Scene BuildTestScene()
    {
        Scene scene = factory.CreateScene();

        
        Point point1 = factory.CreatePoint(1, 1);
        Point point2 = factory.CreatePoint(4, 5);
        Line line = factory.CreateLine(point1, point2);
        Circle circle = factory.CreateCircle(point1, 3);

        
        point1.AddToScene(scene);
        point2.AddToScene(scene);
        line.AddToScene(scene);
        circle.AddToScene(scene);

        return scene;
    }
}


public class MemorySceneBuilder
{
    private GraphObjectFactory factory;

    public MemorySceneBuilder(GraphObjectFactory factory)
    {
        this.factory = factory;
    }

    public void BuildAndCalculateMemory()
    {
        
        Point point1 = factory.CreatePoint(1, 1);
        Point point2 = factory.CreatePoint(4, 5);
        Line line = factory.CreateLine(point1, point2);
        Circle circle = factory.CreateCircle(point1, 3);

        
        long memoryBefore = GC.GetTotalMemory(true);

        
        Scene scene = factory.CreateScene();
        point1.AddToScene(scene);
        point2.AddToScene(scene);
        line.AddToScene(scene);
        circle.AddToScene(scene);

        
        long memoryAfter = GC.GetTotalMemory(true);

        
        Console.WriteLine($"Память, занимаемая сценой: {memoryAfter - memoryBefore} байт");
    }
}

class Program
{
    static void Main(string[] args)
    {
        GraphObjectFactory colorFactory = new ColorSceneFactory();
        GraphObjectFactory bwFactory = new BlackWhiteSceneFactory();

        
        TestSceneBuilder testBuilder = new TestSceneBuilder(colorFactory);
        Scene colorScene = testBuilder.BuildTestScene();

       
        MemorySceneBuilder memoryBuilder = new MemorySceneBuilder(colorFactory);
        memoryBuilder.BuildAndCalculateMemory();

        
        colorScene.Draw();

        Console.ReadLine();
    }
}
