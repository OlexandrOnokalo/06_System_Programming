using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _05_Tasks
{
    class Program
    {
        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }



        static void Main(string[] args)
        {

            Console.OutputEncoding = Encoding.UTF8;

            Task task = Task.Run(() => Console.WriteLine("1 Task" + DateTime.Now));
            Task task2 = Task.Run(() => Console.WriteLine("2 Task" + DateTime.Now));
            Task task3 = Task.Run(() => Console.WriteLine("3 Task" + DateTime.Now));

            Task task4 = new Task(() => Console.WriteLine("4 Task" + DateTime.Now));
            Task task5 = new Task(() => Console.WriteLine("5 Task" + DateTime.Now));
            Task task6 = new Task(() => Console.WriteLine("6 Task" + DateTime.Now));
            task4.Start();
            task5.Start();
            task6.Start();

            Task task7 = Task.Factory.StartNew(() => Console.WriteLine("7 Task" + DateTime.Now));
            Task task8 = Task.Factory.StartNew(() => Console.WriteLine("8 Task" + DateTime.Now));
            Task task9 = Task.Factory.StartNew(() => Console.WriteLine("9 Task" + DateTime.Now));

            Console.ReadLine();
            Console.WriteLine("Exercise 2 ");
            Task task10 = Task.Run(() =>
            {
                for (int number = 2; number <= 1000; number++)
                {
                    if (IsPrime(number))
                    {
                        Console.WriteLine(number);
                    }
                }
            });

            task10.Wait();

            Console.ReadLine();
            Console.WriteLine("Exercise 3 ");

            Console.Write("Введіть початок діапазону: ");
            int start = int.Parse(Console.ReadLine())!;

            Console.Write("Введіть кінець діапазону: ");
            int end = int.Parse(Console.ReadLine())!;

            int primeCount = 0;

            Task task11 = Task.Run(() =>
            {
                for (int number = start; number <= end; number++)
                {
                    if (IsPrime(number))
                    {
                        Console.WriteLine(number);
                        primeCount++;
                    }
                }
            });

            task11.Wait();

            Console.WriteLine($"\nКількість простих чисел у діапазоні [{start}; {end}] = {primeCount}");

            Console.ReadLine();
            Console.WriteLine("Exercise 4 ");


            var rnd = new Random();
            int length = 20;
            int[] numbers = new int[length];

            for (int i = 0; i < length; i++)
                numbers[i] = rnd.Next(1, 101); 

            Console.WriteLine("Масив:");
            Console.WriteLine(string.Join(", ", numbers));

            int min = 0, max = 0, sum = 0;
            double avg = 0;


            Task[] tasks = new Task[4];

            tasks[0] = Task.Run(() =>
            {
                min = numbers.Min();
            });

            tasks[1] = Task.Run(() =>
            {
                max = numbers.Max();
            });

            tasks[2] = Task.Run(() =>
            {
                sum = numbers.Sum();
            });

            tasks[3] = Task.Run(() =>
            {
                avg = numbers.Average();
            });


            Task.WaitAll(tasks);

            Console.WriteLine("\nРезультати обчислень:");
            Console.WriteLine($"Мінімум: {min}");
            Console.WriteLine($"Максимум: {max}");
            Console.WriteLine($"Сума: {sum}");
            Console.WriteLine($"Середнє арифметичне: {avg:F2}");


            Console.ReadLine();
            Console.WriteLine("Exercise 5 ");


            Task<int[]> generateTask = Task.Run(() =>
            {
                var rnd = new Random();
                int length = 20;
                int[] arr = new int[length];
                for (int i = 0; i < length; i++)
                    arr[i] = rnd.Next(0, 15);
                Console.WriteLine("Згенерований масив:");
                Console.WriteLine(string.Join(", ", arr));
                return arr;
            });

            Task<int[]> removeDuplicatesTask = generateTask.ContinueWith(prev =>
            {
                int[] distinct = prev.Result.Distinct().ToArray();
                Console.WriteLine("\nПісля видалення дублікатів:");
                Console.WriteLine(string.Join(", ", distinct));
                return distinct;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            Task<int[]> sortTask = removeDuplicatesTask.ContinueWith(prev =>
            {
                int[] arr = prev.Result;
                Array.Sort(arr);
                Console.WriteLine("\nВідсортований масив:");
                Console.WriteLine(string.Join(", ", arr));
                return arr;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            Task searchTask = sortTask.ContinueWith(prev =>
            {
                int[] sorted = prev.Result;
                Console.WriteLine("\nВведіть число для бінарного пошуку:");
                int value = int.Parse(Console.ReadLine()); 
                int index = Array.BinarySearch(sorted, value);
                if (index >= 0)
                    Console.WriteLine($"Значення {value} знайдено за індексом {index}.");
                else
                    Console.WriteLine($"Значення {value} не знайдено.");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            
            searchTask.Wait();

            Console.ReadKey();



        }
    }
}