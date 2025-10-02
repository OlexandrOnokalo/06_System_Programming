using System;
using System.IO;
using System.Threading;


namespace _03_Threads_02
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            int[] numbers = new int[100];
            Random rnd = new Random();
            for (int i = 0; i < numbers.Length; i++)
                numbers[i] = rnd.Next(0, 1000);

            
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "numbers_and_results.txt");
            if (File.Exists(filePath))
                File.Delete(filePath);

            Console.WriteLine("Масив згенеровано.");

            
            Thread tMax = new Thread(() =>
            {
                int max = numbers[0];
                for (int i = 1; i < numbers.Length; i++)
                    if (numbers[i] > max) max = numbers[i];
                Console.WriteLine($"Максимум = {max}");
            });

            Thread tMin = new Thread(() =>
            {
                int min = numbers[0];
                for (int i = 1; i < numbers.Length; i++)
                    if (numbers[i] < min) min = numbers[i];
                Console.WriteLine($"Мінімум = {min}");
            });

            Thread tAvg = new Thread(() =>
            {
                long sum = 0;
                for (int i = 0; i < numbers.Length; i++)
                    sum += numbers[i];
                double avg = (double)sum / numbers.Length;
                Console.WriteLine($"Середнє = {avg:F2}");
            });

            
            Thread tWriteNumbers = new Thread(() =>
            {
                using (StreamWriter sw = new StreamWriter(filePath, append: true))
                {
                    sw.WriteLine("---- Numbers ----");
                    for (int i = 0; i < numbers.Length; i++)
                        sw.WriteLine(numbers[i]);
                    sw.WriteLine();
                }
                Console.WriteLine("Масив записано у файл.");
            });

            
            Thread tComputeAndWrite = new Thread(() =>
            {
                int max = numbers[0], min = numbers[0];
                long sum = numbers[0];
                for (int i = 1; i < numbers.Length; i++)
                {
                    if (numbers[i] > max) max = numbers[i];
                    if (numbers[i] < min) min = numbers[i];
                    sum += numbers[i];
                }
                double avg = (double)sum / numbers.Length;

                using (StreamWriter sw = new StreamWriter(filePath, append: true))
                {
                    sw.WriteLine("---- Results ----");
                    sw.WriteLine($"Max: {max}");
                    sw.WriteLine($"Min: {min}");
                    sw.WriteLine($"Avg: {avg:F2}");
                }
                Console.WriteLine("Результати записано у файл.");
            });


            tMax.Start();
            tMin.Start();
            tAvg.Start();

            tMax.Join();
            tMin.Join();
            tAvg.Join();

            tWriteNumbers.Start();
            tWriteNumbers.Join();

            tComputeAndWrite.Start();
            tComputeAndWrite.Join();

            Console.WriteLine("Усі потоки завершили роботу. Дані збережено у файл.");
            Console.ReadLine();
        }
    }


}
