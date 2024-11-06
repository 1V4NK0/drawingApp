using System;
namespace tryDrawing
{
	public class DrawingLine
	{
		public List<Point> Points { get; set; } = new List<Point>();
		public Color Color { get; set; }
		public float Width { get; set; }

		public DrawingLine(List<Point> points, Color Color, float Width) 
		{
			this.Points = points;
			this.Color = Color;
			this.Width = Width;
		}
	}
}

