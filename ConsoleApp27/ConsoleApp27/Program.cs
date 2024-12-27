using System;
using System.Collections.Generic;

namespace ConsoleApp
{
   
    public class EventRequest
    {
        private bool handled = false;

        public bool IsHandled => handled;

        public void SetHandled(bool value) => handled = value;

        public void Consume() => SetHandled(true);
    }

   
    public interface Handler
    {
        void SetNextHandler(Handler next);
        void Handle(EventRequest request);
    }

   
    public delegate void PressListener(object sender, EventRequest e);

    public abstract class UIComponent : Handler
    {
        private Handler nextHandler;
        private readonly List<PressListener> listeners = new List<PressListener>();

        public abstract bool Draw(int line);
        public abstract int GetHeight();
        public abstract int GetWidth();

        public void SetNextHandler(Handler next) => nextHandler = next;

        public void AddPressListener(PressListener listener) => listeners.Add(listener);

        public void RemovePressListener(PressListener listener) => listeners.Remove(listener);

        public void Handle(EventRequest request)
        {
            foreach (var listener in listeners)
            {
                if (request.IsHandled) return;
                listener(this, request);
            }

            nextHandler?.Handle(request);
        }
    }

    public class Label : UIComponent
    {
        private readonly string text;

        public Label(string text) => this.text = text;

        public override bool Draw(int line)
        {
            if (line == 0)
            {
                Console.Write($" {text} ");
                return true;
            }
            return false;
        }

        public override int GetHeight() => 1;

        public override int GetWidth() => text.Length + 2;
    }

    public class Button : UIComponent
    {
        private const char BUTTON_FRAME = '*';
        private readonly string text;

        public Button(string text) => this.text = text;

        private void PrintBorder()
        {
            for (int i = 0; i < text.Length; i++)
                Console.Write(BUTTON_FRAME);
        }

        public override bool Draw(int line)
        {
            if (line == 0 || line == 2)
            {
                Console.Write(BUTTON_FRAME);
                PrintBorder();
                Console.Write(BUTTON_FRAME);
                return true;
            }

            if (line == 1)
            {
                Console.Write(BUTTON_FRAME);
                Console.Write(text);
                Console.Write(BUTTON_FRAME);
                return true;
            }

            return false;
        }

        public override int GetHeight() => 2;

        public override int GetWidth() => text.Length + 2;

        public void Press()
        {
            Console.WriteLine("Button pressed");
            Handle(new EventRequest());
        }
    }

    public class CompositeControl : UIComponent
    {
        private const char COMPOSITE_FRAME = '+';
        protected readonly List<UIComponent> children = new List<UIComponent>();

        public CompositeControl Add(UIComponent component)
        {
            children.Add(component);
            component.SetNextHandler(this);
            return this;
        }

        public void Remove(UIComponent component) => children.Remove(component);

        public override bool Draw(int line)
        {
            if (line == 0 || line == GetHeight() - 1)
            {
                PrintBorder();
                return true;
            }

            if (line > 0 && line < GetHeight() - 1)
            {
                Console.Write(COMPOSITE_FRAME);
                foreach (var child in children)
                {
                    if (!child.Draw(line - 1))
                    {
                        Console.Write(new string(' ', child.GetWidth()));
                    }
                }
                Console.Write(COMPOSITE_FRAME);
                return true;
            }

            return false;
        }

        public void Draw()
        {
            for (int i = 0; i < GetHeight(); i++)
            {
                Draw(i);
                Console.WriteLine();
            }
        }

        private void PrintBorder() => Console.WriteLine(new string(COMPOSITE_FRAME, GetWidth()));

        public override int GetHeight()
        {
            int totalHeight = 0;
            foreach (var child in children)
            {
                totalHeight = Math.Max(totalHeight, child.GetHeight());
            }
            return totalHeight + 2; 
        }

        public override int GetWidth()
        {
            int totalWidth = 0;
            foreach (var child in children)
            {
                totalWidth += child.GetWidth(); 
            }
            return totalWidth + 2;
        }
    }

    public class MainWindow : CompositeControl
    {
        public override bool Draw(int line)
        {
            bool result = base.Draw(line);
            
            if (result)
            {
                Console.WriteLine("");
            }
            return result;
        }
    }


    class Program
    {
        static void Main()
        {
            var mainWin = new MainWindow();

            var frame1 = new CompositeControl();
            var frame2 = new CompositeControl();

            frame1.Add(new Label("Login"))
                  .Add(new Button("OK"));

            frame2.Add(new Label("Password"))
                  .Add(new Button("Verify"));

            var printButton = new Button("Print");
            mainWin.Add(frame1)
                   .Add(frame2)
                   .Add(new CompositeControl().Add(printButton));

            mainWin.AddPressListener((sender, e) =>
            {
                Console.WriteLine("MainWin handler");
            });

            printButton.AddPressListener((sender, e) =>
            {
                Console.WriteLine("Button press first handler");
            });

            mainWin.Draw();

            printButton.AddPressListener((sender, e) =>
            {
                Console.WriteLine("Button press second handler");
            });

            printButton.Press();
            Console.ReadLine();
        }
    }
}
