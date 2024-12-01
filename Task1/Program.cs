using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите размер массива-");
            int n = Convert.ToInt32(Console.ReadLine());

            Func<object, int[]> func1 = new Func<object, int[]>(GetArray);
            Task<int[]> task1 = new Task<int[]> (func1, n);

            Func<Task<int[]>, Tuple<int, int[]>> func2 = new Func<Task<int[]>, Tuple<int, int[]>>(SummArray);
            Task<Tuple<int, int[]>> task2 = task1.ContinueWith(func2);

            Func<Task<Tuple<int, int[]>>, int> func3 = new Func<Task<Tuple<int, int[]>>, int>(MaxArray);
            Task<int> task3 = task2.ContinueWith(func3);

            task1.Start();
            task1.Wait();
            Console.Write($"Массив: ");
            foreach (int a in task1.Result)
            {
                Console.Write($"{a} ");
            }

            Console.WriteLine($"\nСумма чисел: {task2.Result.Item1}");
            Console.WriteLine($"Максимальное число: {task3.Result}");

            Console.ReadKey();


        }

        static int[] GetArray(object a)
        {
            int n = (int)a;
            int[] array = new int[n];
            Random random = new Random();
            for (int i = 0; i < n; i++)
            { 
            array[i] = random.Next(1,100);
            }
            return array;
        }

        static Tuple<int, int[]> SummArray(Task<int[]> task1)
        {
            int[] array = task1.Result;
        int count = 0;
            for (int i = 0; i < array.Count(); i++)
            {
                count += array[i];
            }
            return Tuple.Create(count, array);
        }

        static int MaxArray(Task<Tuple<int, int[]>> task2)
        {
            int[] array = task2.Result.Item2;
            int max = 0;
            for (int i = 0; i < array.Count(); i++)
            {
                if (array[i] > max && array.Length>0)
                {
                    max = array[i];
                }
            }
            return max;
        }
    }
}
