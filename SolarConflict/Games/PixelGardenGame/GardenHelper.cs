using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaintPlay
{
    class GardenHelper
    {



        static double GetY(double x)
        {
            return Math.Sin(x / 10.0) + Math.Cos(x / 15.0) + Math.Atan(Math.Cos(x / 32.0)) + 2 + 3.5;
        }
        
        static void VerLine(int xPos, int yMin, int yMax, GPixel[,] grid) //add color
        {
            if(yMax>yMin)
            {
                for (int i = yMin; i <= yMax; i++)
			    {
			        grid[xPos,i].type = PixelType.Sand;
                    grid[xPos,i].value = (int)((double)(i-yMin)/ (double)(yMax-yMin)*254.0)+1;
			    }
            }
        }

        public static void MakeGround(int xMin, int xMax, int yMax, GPixel[,] grid)
        {
            for (int i = xMin; i <= xMax; i++)
            {
                VerLine(i, yMax -40 + (int)(GetY(i)*3), yMax, grid);
                //VerLine(i, (int)GetY(i + 1000, 10), yMax, grid);
            }             
        }


        public static void Line(int x1, int y1, int x2, int y2, GPixel color, GPixel[,] grid)
        {
            int deltaX = x2 - x1;
            int deltaY = y2 - y1;
            int n = Math.Max( Math.Abs(deltaX), Math.Abs(deltaY));

            if (n > 0)
            {
                float dx = (float)deltaX / n;
                float dy = (float)deltaY / n;

                for (int i = 0; i <= n; i++)
                {
                    int x =(int)Math.Round(x1 + i * dx);
                    int y = (int)Math.Round(y1 + i * dy);
                    if(x>0 && x<grid.GetLength(0)-1 && y>0 && y<grid.GetLength(1))
                        grid[x, y] = color;
                }
            }
            else
            {
                if (x1 > 0 && x1 < grid.GetLength(0) - 1 && y1 > 0 && y1 < grid.GetLength(1))
                    grid[x1, y1] = color;
            }
        }

        /*
         * procedure line(x1,y1,x2,y2:integer; color:byte;ad:word);

    function sgn(a:real):integer;
    begin
         if a>0 then sgn:=+1;
         if a<0 then sgn:=-1;
         if a=0 then sgn:=0;
    end;

var u,s,v,d1x,d1y,d2x,d2y,m,n:real;
    i:integer;
begin
     u:= x2 - x1;
     v:= y2 - y1;
     d1x:= SGN(u);
     d1y:= SGN(v);
     d2x:= SGN(u);
     d2y:= 0;
     m:= ABS(u);
     n := ABS(v);
     IF NOT (M>N) then
     BEGIN
          d2x := 0 ;
          d2y := SGN(v);
          m := ABS(v);
          n := ABS(u);
     END;
     s := INT(m / 2);
     FOR i := 0 TO round(m) DO
     BEGIN
          limpixel(x1,y1,color,ad);
          s := s + n;
          IF not (s<m) THEN
          BEGIN
               s := s - m;
               x1:=x1+round(d1x);
               y1:=y1+ round(d1y);
          END
          ELSE
          BEGIN
               x1:=x1+round(d2x);
               y1:=y1+round(d2y);
          END;
     end;
END;
         */
    }
}
