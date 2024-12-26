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
public class Triangle
{
    private double x1, y1, x2, y2, x3, y3;

    public Triangle(double x1, double y1, double x2, double y2, double x3, double y3)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        this.x3 = x3;
        this.y3 = y3;
    }

    public void Render()
    {
        Console.WriteLine($"Рисуем треугольник с вершинами ({x1}, {y1}), ({x2}, {y2}), ({x3}, {y3})");
    }

    public double X1 => x1;
    public double Y1 => y1;
    public double X2 => x2;
    public double Y2 => y2;
    public double X3 => x3;
    public double Y3 => y3;

}
public class TriangleAdapter : GraphObject
{
    private Triangle triangle;

    public TriangleAdapter(Triangle triangle)
    {
        this.triangle = triangle;
    }

    public override void Draw()
    {
        triangle.Render();
    }

    public override object Clone()
    {
        return new TriangleAdapter(
            new Triangle(triangle.X1, triangle.Y1, triangle.X2, triangle.Y2, triangle.X3, triangle.Y3)
        );
    }

}

public class CompositeGraphObject : GraphObject
{
    private List<GraphObject> children = new List<GraphObject>();

    public void Add(GraphObject graphObject)
    {
        children.Add(graphObject);
    }

    public void Remove(GraphObject graphObject)
    {
        children.Remove(graphObject);
    }

    public override void Draw()
    {
        Console.WriteLine("Рисуем композитный элемент:");
        foreach (var child in children)
        {
            child.Draw();
        }
    }
    public override object Clone()
    {
        var compositeClone = new CompositeGraphObject();
        foreach (var child in children)
        {
            compositeClone.Add((GraphObject)child.Clone());
        }
        return compositeClone;
    }
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

        Triangle triangle = new Triangle(0, 0, 3, 0, 1.5, 2.5);
        TriangleAdapter triangleAdapter = new TriangleAdapter(triangle);

        CompositeGraphObject composite = new CompositeGraphObject();
        composite.Add(point1);
        composite.Add(line);
        composite.Add(circle);

        composite.AddToScene(scene);
        return scene;
    }
}
public abstract class Decorator : GraphObject
{
    protected GraphObject graphObject;

    public Decorator(GraphObject graphObject)
    {
        this.graphObject = graphObject;
    }

    public override void Draw()
    {
        graphObject.Draw();
    }

    public override object Clone()
    {
        return graphObject.Clone();  
    }
}

public class ColorDecorator : Decorator
{
    private string fillColor;

    public ColorDecorator(GraphObject graphObject, string fillColor) : base(graphObject)
    {
        this.fillColor = fillColor;
    }

    public override void Draw()
    {
        graphObject.Draw();
        Console.WriteLine($"Закрашиваем объект цветом: {fillColor}");
    }

    public string FillColor => fillColor;
}

public class SceneFacade
{
    private Scene scene;
    private GraphObjectFactory factory;

    public SceneFacade(GraphObjectFactory factory)
    {
        this.factory = factory;
        this.scene = factory.CreateScene();
    }

    public void AddPoint(double x, double y, string color = null)
    {
        GraphObject point = factory.CreatePoint(x, y);
        if (color != null)
        {
            point = new ColorDecorator(point, color);
        }
        point.AddToScene(scene);
    }

    public void AddLine(double startX, double startY, double endX, double endY, string color = null)
    {
        Point start = factory.CreatePoint(startX, startY);
        Point end = factory.CreatePoint(endX, endY);
        GraphObject line = factory.CreateLine(start, end);

        if (color != null)
        {
            line = new ColorDecorator(line, color);
        }
        line.AddToScene(scene);
    }

    public void AddCircle(double centerX, double centerY, double radius, string color = null)
    {
        Point center = factory.CreatePoint(centerX, centerY);
        GraphObject circle = factory.CreateCircle(center, radius);

        if (color != null)
        {
            circle = new ColorDecorator(circle, color);
        }
        circle.AddToScene(scene);
    }

    public void AddTriangle(double x1, double y1, double x2, double y2, double x3, double y3, string color = null)
    {
        Triangle triangle = new Triangle(x1, y1, x2, y2, x3, y3);
        GraphObject triangleAdapter = new TriangleAdapter(triangle);

        if (color != null)
        {
            triangleAdapter = new ColorDecorator(triangleAdapter, color);
        }
        triangleAdapter.AddToScene(scene);
    }

    public void DrawScene()
    {
        scene.Draw();
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

        Triangle triangle = new Triangle(0, 0, 3, 0, 1.5, 2.5);
        TriangleAdapter triangleAdapter = new TriangleAdapter(triangle);

        CompositeGraphObject composite = new CompositeGraphObject();

        GraphObject coloredPoint = new ColorDecorator(point1, "Красный");
        GraphObject coloredLine = new ColorDecorator(line, "Синий");
        GraphObject coloredCircle = new ColorDecorator(circle, "Зеленый");
        GraphObject coloredTriangle = new ColorDecorator(triangleAdapter, "Желтый");

        composite.Add(coloredPoint);
        composite.Add(coloredLine);
        composite.Add(coloredCircle);
        composite.Add(coloredTriangle);

        composite.Draw();
        Console.WriteLine();
        Console.WriteLine("Фасад");
        SceneFacade facade = new SceneFacade(factory);

        facade.AddPoint(1, 1, "Красный");
        facade.AddLine(0, 0, 3, 3, "Синий");
        facade.AddCircle(5, 5, 2, "Зеленый");
        facade.AddTriangle(0, 0, 4, 0, 2, 3, "Желтый");

        facade.DrawScene();
        Console.WriteLine();


        long memoryBefore = GC.GetTotalMemory(true);

        Scene scene = factory.CreateScene();

        long memoryAfter = GC.GetTotalMemory(true);

        Console.WriteLine($"Память, занимаемая сценой: {memoryAfter - memoryBefore} байт");
    }
}

class Program
{
    static void Main(string[] args)
    {
        GraphObjectFactory colorFactory = new ColorSceneFactory();

        TestSceneBuilder testBuilder = new TestSceneBuilder(colorFactory);
        Scene colorScene = testBuilder.BuildTestScene();

        MemorySceneBuilder memoryBuilder = new MemorySceneBuilder(colorFactory);
        memoryBuilder.BuildAndCalculateMemory();

        colorScene.Draw();

        Console.ReadLine();
    }
}
