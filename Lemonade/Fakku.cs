using System;
using System.Drawing;
using CC_Functions.Commandline.TUI;

namespace Lemonade
{
    public static class EllipseDrawer
    {
        private static PointF GetEllipsePointFromX(float x, float a, float b) =>
            //(x/a)^2 + (y/b)^2 = 1
            //(y/b)^2 = 1 - (x/a)^2
            //y/b = -sqrt(1 - (x/a)^2)  --Neg root for upper portion of the plane
            //y = b*-sqrt(1 - (x/a)^2)
            new PointF(x, b * -(float)Math.Sqrt(1 - x * x / a / a));

        public static void DrawEllipse(this Pixel[,] target, Rectangle area, Pixel basePixel)
        {
            int p0 = target.GetLength(0);
            int p1 = target.GetLength(1);
            bool[,] tmp = new bool[p0, p1];
            DrawEllipse(tmp, new Rectangle(area.Y, area.X, area.Height, area.Width));
            for (int i = 0; i < p0; i++)
            for (int j = 0; j < p1; j++)
                if (tmp[i, j])
                    target[i, j] = basePixel;
        }

        private static void DrawEllipse(bool[,] pixels, Rectangle area)
        {
            // Get the size of the matrix
            int matrixWidth = pixels.GetLength(0);
            int matrixHeight = pixels.GetLength(1);
            int offsetY = area.Top;
            int offsetX = area.Left;
            // Figure out how big the ellipse is
            float ellipseWidth = area.Width;
            float ellipseHeight = area.Height;
            // Figure out the radiuses of the ellipses
            float radiusX = ellipseWidth / 2;
            float radiusY = ellipseHeight / 2;
            //Keep track of the previous y position
            int prevY = 0;
            bool firstRun = true;
            // Loop through the points in the matrix
            for (int x = 0; x <= radiusX; ++x)
            {
                int xPos = x + offsetX;
                int rxPos = (int)ellipseWidth - x - 1 + offsetX;
                if (xPos < 0 || rxPos < xPos || xPos >= matrixWidth) continue;
                PointF pointOnEllipseBoundCorrespondingToXMatrixPosition = GetEllipsePointFromX(x - radiusX, radiusX, radiusY);
                int y = (int) Math.Floor(pointOnEllipseBoundCorrespondingToXMatrixPosition.Y + (int)radiusY);
                int yPos = y + offsetY;
                int ryPos = (int)ellipseHeight - y - 1 + offsetY;
                if (yPos >= 0)
                {
                    if (xPos < matrixWidth && yPos < matrixHeight) pixels[xPos, yPos] = true;
                    if(xPos < matrixWidth && ryPos > -1 && ryPos < matrixHeight) pixels[xPos, ryPos] = true;
                    if (rxPos < matrixWidth)
                    {
                        if (yPos < matrixHeight) pixels[rxPos, yPos] = true;
                        if (ryPos > -1 && ryPos < matrixHeight) pixels[rxPos, ryPos] = true;
                    }
                }
                //While there's a >1 jump in y, fill in the gap (assumes that this is not the first time we've tracked y, x != 0)
                for (int j = prevY - 1; !firstRun && j > y - 1 && y > 0; --j)
                {
                    int jPos = j + offsetY;
                    int rjPos = (int)ellipseHeight - j - 1 + offsetY;
                    if(jPos == rjPos - 1) continue;
                    if(jPos > -1 && jPos < matrixHeight) pixels[xPos, jPos] = true;
                    if(rjPos > -1 && rjPos < matrixHeight) pixels[xPos, rjPos] = true;
                    if (rxPos < matrixWidth)
                    {
                        if(jPos > -1 && jPos < matrixHeight) pixels[rxPos, jPos] = true;
                        if(rjPos > -1 && rjPos < matrixHeight) pixels[rxPos, rjPos] = true;
                    }
                }
                firstRun = false;
                prevY = y;
                float countTarget = radiusY - y;
                for (int count = 0; count < countTarget; ++count)
                {
                    ++yPos;
                    --ryPos;
                    // Set all four points in the matrix we just learned about
                    //  also, make the indication that for the rest of this row, we need to fill the body of the ellipse
                    if(yPos > -1 && yPos < matrixHeight) pixels[xPos, yPos] = true;
                    if(ryPos > -1 && ryPos < matrixHeight) pixels[xPos, ryPos] = true;
                    if (rxPos < matrixWidth)
                    {
                        if(yPos > -1 && yPos < matrixHeight) pixels[rxPos, yPos] = true;
                        if(ryPos > -1 && ryPos < matrixHeight) pixels[rxPos, ryPos] = true;
                    }
                }
            }
        }
    }
}