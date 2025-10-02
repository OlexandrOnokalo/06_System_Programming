using System;
using System.Text;


namespace _03_Threads_01
{
    using System;
    using System.Threading;

    class Program
    {
        static void PrintNumbers(object rangeObj)
        {
            Tuple<int, int> range = (Tuple<int, int>)rangeObj;
            for (int i = range.Item1; i <= range.Item2; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(100); 
            }
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Write("Введіть початок діапазону: ");
            int start = int.Parse(Console.ReadLine());

            Console.Write("Введіть кінець діапазону: ");
            int end = int.Parse(Console.ReadLine());

            Tuple<int, int> range = new Tuple<int, int>(start, end);

            Console.WriteLine();
            Console.WriteLine("Натискайте Enter для запуску нових потоків...");
            Console.WriteLine("(Закрийте консоль, коли захочете завершити програму.)");

            while (true)
            {
                Console.ReadLine();
                Thread t = new Thread(PrintNumbers);
                t.Start(range);
            }
        }
    }


}
