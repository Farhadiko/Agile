using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Duzbucaqlinin enini daxil edin");
            int nummber1 = int.Parse(Console.ReadLine());
            Console.WriteLine("Duzbucaqlinin uzunlugunu daxil edin");
            int number2 = int.Parse(Console.ReadLine());
            int area = nummber1 * number2;
            Console.WriteLine("Duzbucaqlinin sahesi");
            int perimeter = 2 * (nummber1 + number2);
            Console.WriteLine(area);
            Console.WriteLine("Duzbucaqlinin Perimetri");
            Console.WriteLine(perimeter);
            Console.ReadLine();
        }
    }
}
