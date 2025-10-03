using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _04_Thread_Sync
{
    class Stat
    {
        public static int Letters { get; set; }
        public static int Digits { get; set; }

        

    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Stat statistic = new Stat();

            string[] files = Directory.GetFiles(@$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Test");

            Thread[] threads = new Thread[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine("\nНазва файлу: "+files[i]);
                string text = File.ReadAllText(files[i]);

                threads[i] = new Thread(() => TextAnalyse(text));
                threads[i].Start();
                Console.WriteLine("\nВміст файлу: "+text);
            }


            for (int i = 0; i < files.Length; i++)
            {
                threads[i].Join();
            }

            Console.WriteLine("\nКількість цифр: " + Stat.Digits+"\nКількість букв: "+Stat.Letters);


        }

        static void TextAnalyse(object text)
        {
            string input = text as string; 


            foreach (char ch in input)
            {
                if (char.IsLetter(ch)) Stat.Letters++;
                else if (char.IsDigit(ch)) Stat.Digits++;
            }
        }

    }
}