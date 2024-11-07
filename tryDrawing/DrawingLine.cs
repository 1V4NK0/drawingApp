using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core;

namespace tryDrawing
{
	
        public class DrawingLine : IDrawingLine
        {
            public double X { get; set; }
            public double Y { get; set; }
        int IDrawingLine.Granularity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        Color IDrawingLine.LineColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        float IDrawingLine.LineWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        ObservableCollection<PointF> IDrawingLine.Points { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IDrawingLine.ShouldSmoothPathWhenDrawn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        ValueTask<Stream> IDrawingLine.GetImageStream(double imageSizeWidth, double imageSizeHeight, Paint background, CancellationToken token)
        {
            throw new NotImplementedException();
        }
        // Other properties and methods
    }

    
}

