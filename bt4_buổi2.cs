using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRỌNG_CU_TE
{
    internal class Program
    {
        public static Array TaoMang(int len)
        {
            Array arr = Array.CreateInstance(typeof(int), len);
            Random rand = new Random(100);
            for (int i = 0; i < len; i++)
            {
                arr.SetValue(rand.Next(1, 100), i);
            }
            return arr;
        }
        public static List<int> TaoDanhSach(int len)
        {
            List<int> list = new List<int>();
            Random rand = new Random(100);
            for (int i = 0; i < len; i++)
            {
                list.Add(rand.Next(1, 100));
            }
            return list;
        }

        public static ArrayList TaoArrayList(int len)
        {
            ArrayList list = new ArrayList();
            Random rand = new Random(100);
            for (int i = 0; i < len; i++)
            {
                list.Add(rand.Next(1, 100));
            }
            return list;
        }
        public static int TimMax(Array arr)
        {
            int max = (int)arr.GetValue(0);
            foreach (int value in arr)
            {
                if (value > max) max = value;
            }
            return max;
        }
        public static int TimMin(Array arr)
        {
            int min = (int)arr.GetValue(0);
            foreach (int value in arr)
            {
                if (value < min) min = value;
            }
            return min;
        }
        public static double TinhTrungBinh(Array arr)
        {
            int sum = 0;
            foreach (int value in arr)
            {
                sum += value;
            }
            return (double)sum / arr.Length;
        }

        public static void Main(string[] args)
        {
            Console .OutputEncoding = Encoding.UTF8;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Array diemSoArray = TaoMang(1000000);
            sw.Stop();
            Console.WriteLine("Thời gian với Array: " + sw.ElapsedMilliseconds + " ms");
            sw.Restart();
            List<int> diemSoList = TaoDanhSach(1000000);
            sw.Stop();
            Console.WriteLine("Thời gian với List: " + sw.ElapsedMilliseconds + " ms");
            sw.Restart();
            ArrayList diemSoArrayList = TaoArrayList(1000000);
            sw.Stop();
            Console.WriteLine("Thời gian với ArrayList: " + sw.ElapsedMilliseconds + " ms");
          
        }
    }
}
