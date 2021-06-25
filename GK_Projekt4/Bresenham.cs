using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_Projekt4
{
    //Poniższa klasa zawiera zaimplementowane metody MidpointLine do rysowania linii algorytmem Bresenhama.
    //Implementacja wzorowana na tej wykładowej.
    static class Bresenham
    {
        private static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }
        //Metoda ogólna do rysowania linii.
        public static void MidpointLine(DirectBitmap bitmap, int x1, int y1, double z1, int x2, int y2, double z2,
            double[,] zBuffer, Color color)
        {
            if (Math.Abs(x2 - x1) >= Math.Abs(y2 - y1))
                //jeśli nachylenie linii jest między -45 i 45 stopni
            {
                if (x1 > x2)
                    //to bez znaczenia, z którego punktu rysujemy do którego, a mi
                    //x1 <= x2 ułatwiło implementację
                {
                    Swap(ref x1, ref x2);
                    Swap(ref y1, ref y2);
                    Swap(ref z1, ref z2);
                }
                if (zBuffer == null)
                    MidpointFlatLine(bitmap, x1, y1, x2, y2, color);
                else
                    MidpointFlatLine(bitmap, x1, y1, z1, x2, y2, z2, zBuffer, color);
            }
            else
                //w przeciwnym przypadku wywołujemy odpowiednią funkcję dla linii "stromych"
            {
                if (y1 > y2)
                {
                    Swap(ref x1, ref x2);
                    Swap(ref y1, ref y2);
                    Swap(ref z1, ref z2);
                }
                if (zBuffer == null)
                    MidpointSteepLine(bitmap, x1, y1, x2, y2, color);
                else
                    MidpointSteepLine(bitmap, x1, y1, z1, x2, y2, z2, zBuffer, color);
            }
        }
        //Poniższa funkcja służy do wyrysowania linii, jeśli jej nachylenie jest między -45 i 45 stopni
        //(lub -45+180 a 45+180 stopni).
        //Jest to po prostu implementacja ze slajdów rozszerzona o warunek, czy nachylenie jest w górę,
        //czy w dół (dy > 0).
        //UWAGA! x1 i x2 muszą spełniać x1 <= x2
        private static void MidpointFlatLine(DirectBitmap bitmap, int x1, int y1, int x2, int y2, 
            Color color)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            //poniżej policzyłem ręcznie odpowiednie wartości dla dy <= 0 
            //(te dla dy > 0 są ze slajdów)
            int d = dy > 0 ? 2 * dy - dx : 2 * dy + dx;
            int incrD1 = dy > 0 ? 2 * dy : 2 * (dy + dx);
            int incrD2 = dy > 0 ? 2 * (dy - dx) : 2 * dy;
            int incrY1 = dy > 0 ? 0 : -1;
            int incrY2 = dy > 0 ? 1 : 0;
            int x = x1;
            int y = y1;
            PutPixel(bitmap, x, y, color);
            while (x < x2)
            {
                x++;
                if (d < 0)
                    //tutaj gdy y1 < y2 idziemy do E, gdy y1 >= y2 idziemy do SE
                {
                    d += incrD1;
                    y += incrY1;
                }
                else
                {
                    //tutaj gdy y1 < y2 idziemy do NE, gdy y1 >= y2 to idziemy do E
                    d += incrD2;
                    y += incrY2;
                }
                PutPixel(bitmap, x, y, color);
            }
        }

        //Poniższa funkcja służy do wyrysowania linii, jeśli jej nachylenie jest między 45 a 135 stopni
        //(lub 45+180 a 135+180 stopni).
        //Poniższa funkcja jest zrobiona symetrycznie na wzór MidpointFlatLine.
        //Symetrycznie, to znaczy zamieniłem wystąpienia dx z wystąpieniami dy, 
        //zmieniłem warunek w whilu x na warunek z y, zamieniłem inkrementowane zmienne 
        //x i y miejscami (oraz zmieniłem nazwy zmiennych incrY1 i incrY2 na incrX1 i incrX2).
        //W poniższej funkcji z tego powodu nie ma komentarzy (są w funkcji MidpointFlatLine).
        //Ogólnie, mógłbym oczywiście w powyższej funkcji zrobić wszystko ale wydaje mi się,
        //że bardzo utrudniłoby to sprawdzanie i czytelność.
        private static void MidpointSteepLine(DirectBitmap bitmap, int x1, int y1, int x2, int y2,
            Color color)
        {
            int dy = y2 - y1;
            int dx = x2 - x1;
            int d = dx > 0 ? 2 * dx - dy : 2 * dx + dy;
            int incrD1 = dx > 0 ? 2 * dx : 2 * (dx + dy);
            int incrD2 = dx > 0 ? 2 * (dx - dy) : 2 * dx;
            int incrX1 = dx > 0 ? 0 : -1;
            int incrX2 = dx > 0 ? 1 : 0;
            int x = x1;
            int y = y1;
            PutPixel(bitmap, x, y, color);
            while (y < y2)
            {
                y++;
                if (d < 0)
                {
                    d += incrD1;
                    x += incrX1;
                }
                else
                {
                    d += incrD2;
                    x += incrX2;
                }
                PutPixel(bitmap, x, y, color);
            }
        }

        //Dwie poniższe funkcje to nieco zmodyfikowane funkcje powyższe, różnią się tym, że uwzględniają zBufor.

        //Poniższa funkcja służy do wyrysowania linii, jeśli jej nachylenie jest między -45 i 45 stopni
        //(lub -45+180 a 45+180 stopni).
        //Jest to po prostu implementacja ze slajdów rozszerzona o warunek, czy nachylenie jest w górę,
        //czy w dół (dy > 0).
        //UWAGA! x1 i x2 muszą spełniać x1 <= x2
        private static void MidpointFlatLine(DirectBitmap bitmap, int x1, int y1, double z1, int x2, int y2,
            double z2, double[,] zBuffer, Color color)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            //poniżej policzyłem ręcznie odpowiednie wartości dla dy <= 0 
            //(te dla dy > 0 są ze slajdów)
            int d = dy > 0 ? 2 * dy - dx : 2 * dy + dx;
            int incrD1 = dy > 0 ? 2 * dy : 2 * (dy + dx);
            int incrD2 = dy > 0 ? 2 * (dy - dx) : 2 * dy;
            int incrY1 = dy > 0 ? 0 : -1;
            int incrY2 = dy > 0 ? 1 : 0;
            double incrZ = (z2 - z1) / dx;
            int x = x1;
            int y = y1;
            double z = z1;
            if (z <= zBuffer[x, y])
            {
                zBuffer[x, y] = z;
                PutPixel(bitmap, x, y, color);
            }
            while (x < x2)
            {
                x++;
                z += incrZ;
                if (d < 0)
                //tutaj gdy y1 < y2 idziemy do E, gdy y1 >= y2 idziemy do SE
                {
                    d += incrD1;
                    y += incrY1;
                }
                else
                {
                    //tutaj gdy y1 < y2 idziemy do NE, gdy y1 >= y2 to idziemy do E
                    d += incrD2;
                    y += incrY2;
                }
                if (z <= zBuffer[x, y])
                {
                    zBuffer[x, y] = z;
                    PutPixel(bitmap, x, y, color);
                }
            }
        }

        //Poniższa funkcja służy do wyrysowania linii, jeśli jej nachylenie jest między 45 a 135 stopni
        //(lub 45+180 a 135+180 stopni).
        //Poniższa funkcja jest zrobiona symetrycznie na wzór MidpointFlatLine.
        //Symetrycznie, to znaczy zamieniłem wystąpienia dx z wystąpieniami dy, 
        //zmieniłem warunek w whilu x na warunek z y, zamieniłem inkrementowane zmienne 
        //x i y miejscami (oraz zmieniłem nazwy zmiennych incrY1 i incrY2 na incrX1 i incrX2).
        //W poniższej funkcji z tego powodu nie ma komentarzy (są w funkcji MidpointFlatLine).
        //Ogólnie, mógłbym oczywiście w powyższej funkcji zrobić wszystko ale wydaje mi się,
        //że bardzo utrudniłoby to sprawdzanie i czytelność.
        private static void MidpointSteepLine(DirectBitmap bitmap, int x1, int y1, double z1, int x2, int y2,
            double z2, double[,] zBuffer, Color color)
        {
            int dy = y2 - y1;
            int dx = x2 - x1;
            int d = dx > 0 ? 2 * dx - dy : 2 * dx + dy;
            int incrD1 = dx > 0 ? 2 * dx : 2 * (dx + dy);
            int incrD2 = dx > 0 ? 2 * (dx - dy) : 2 * dx;
            int incrX1 = dx > 0 ? 0 : -1;
            int incrX2 = dx > 0 ? 1 : 0;
            double incrZ = (z2 - z1) / dy;
            int x = x1;
            int y = y1;
            double z = z1;
            if (z <= zBuffer[x, y])
            {
                zBuffer[x, y] = z;
                PutPixel(bitmap, x, y, color);
            }
            while (y < y2)
            {
                y++;
                z += incrZ;
                if (d < 0)
                {
                    d += incrD1;
                    x += incrX1;
                }
                else
                {
                    d += incrD2;
                    x += incrX2;
                }
                if (z <= zBuffer[x,y])
                {
                    zBuffer[x, y] = z;
                    PutPixel(bitmap, x, y, color);
                }
            }
        }
        private static void PutPixel(DirectBitmap bitmap, int x, int y, Color color)
        {
            if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height)
                bitmap.SetPixel(x, y, color);
        }
    }
}
