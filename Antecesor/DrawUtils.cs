using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antecesor
{
    internal class DrawUtils
    {
        public static GraphicsPath CreateRoundRect(float x, float y, float width, float height, float radius)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(x + width - radius * 2f, y, radius * 2f, radius * 2f, 270f, 90f);
            graphicsPath.AddArc(x + width - radius * 2f, y + height - radius * 2f, radius * 2f, radius * 2f, 0f, 90f);
            graphicsPath.AddArc(x, y + height - radius * 2f, radius * 2f, radius * 2f, 90f, 90f);
            graphicsPath.AddArc(x, y, radius * 2f, radius * 2f, 180f, 90f);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        public static GraphicsPath CreateRoundRect(Rectangle rect, float radius)
        {
            return CreateRoundRect(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public static GraphicsPath CreateRoundRect(RectangleF rect, float radius)
        {
            return CreateRoundRect(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }

        public static Color BlendColor(Color backgroundColor, Color frontColor, double blend)
        {
            double num = blend / 255.0;
            double num2 = 1.0 - num;
            int red = (int)((double)(int)backgroundColor.R * num2 + (double)(int)frontColor.R * num);
            int green = (int)((double)(int)backgroundColor.G * num2 + (double)(int)frontColor.G * num);
            int blue = (int)((double)(int)backgroundColor.B * num2 + (double)(int)frontColor.B * num);
            return Color.FromArgb(red, green, blue);
        }

        public static Color BlendColor(Color backgroundColor, Color frontColor)
        {
            return BlendColor(backgroundColor, frontColor, (int)frontColor.A);
        }

        public static void DrawSquareShadow(Graphics g, Rectangle bounds)
        {
            using SolidBrush brush = new SolidBrush(Color.FromArgb(12, 0, 0, 0));
            GraphicsPath path = CreateRoundRect(new RectangleF((float)bounds.X - 3.5f, (float)bounds.Y - 1.5f, bounds.Width + 6, bounds.Height + 6), 8f);
            g.FillPath(brush, path);
            path = CreateRoundRect(new RectangleF((float)bounds.X - 2.5f, (float)bounds.Y - 1.5f, bounds.Width + 4, bounds.Height + 4), 6f);
            g.FillPath(brush, path);
            path = CreateRoundRect(new RectangleF((float)bounds.X - 1.5f, (float)bounds.Y - 0.5f, bounds.Width + 2, bounds.Height + 2), 4f);
            g.FillPath(brush, path);
            path = CreateRoundRect(new RectangleF((float)bounds.X - 0.5f, (float)bounds.Y + 1.5f, bounds.Width, bounds.Height), 4f);
            g.FillPath(brush, path);
            path = CreateRoundRect(new RectangleF((float)bounds.X - 0.5f, (float)bounds.Y + 2.5f, bounds.Width, bounds.Height), 4f);
            g.FillPath(brush, path);
            path.Dispose();
        }

        public static void DrawRoundShadow(Graphics g, Rectangle bounds)
        {
            using SolidBrush brush = new SolidBrush(Color.FromArgb(12, 0, 0, 0));
            g.FillEllipse(brush, new Rectangle(bounds.X - 2, bounds.Y - 1, bounds.Width + 4, bounds.Height + 6));
            g.FillEllipse(brush, new Rectangle(bounds.X - 1, bounds.Y - 1, bounds.Width + 2, bounds.Height + 4));
            g.FillEllipse(brush, new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height + 2));
            g.FillEllipse(brush, new Rectangle(bounds.X, bounds.Y + 2, bounds.Width, bounds.Height));
            g.FillEllipse(brush, new Rectangle(bounds.X, bounds.Y + 1, bounds.Width, bounds.Height));
        }

    }
}
