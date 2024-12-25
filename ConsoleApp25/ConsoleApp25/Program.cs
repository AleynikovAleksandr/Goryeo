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

class Program
{
    static void Main(string[] args)
    {
        GraphObjectFactory colorFactory = new ColorSceneFactory();
        GraphObjectFactory bwFactory = new BlackWhiteSceneFactory();

        Scene colorScene = colorFactory.CreateScene();
        Scene bwScene = bwFactory.CreateScene();

        Point colorPoint = colorFactory.CreatePoint(2, 3);
        Line colorLine = colorFactory.CreateLine(colorPoint, colorFactory.CreatePoint(5, 7));
        Circle colorCircle = colorFactory.CreateCircle(colorPoint, 4);

        colorPoint.AddToScene(colorScene);
        colorLine.AddToScene(colorScene);
        colorCircle.AddToScene(colorScene);

        Point bwPoint = bwFactory.CreatePoint(1, 1);
        Line bwLine = bwFactory.CreateLine(bwPoint, bwFactory.CreatePoint(3, 4));
        Circle bwCircle = bwFactory.CreateCircle(bwPoint, 2);

        bwPoint.AddToScene(bwScene);
        bwLine.AddToScene(bwScene);
        bwCircle.AddToScene(bwScene);

        colorScene.Draw();
        bwScene.Draw();

        Console.ReadLine();
    }
}
