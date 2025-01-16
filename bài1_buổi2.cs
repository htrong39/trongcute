using System;
using System.Collections.Generic;
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
            Array diemSo = TaoMang(10);
            Console.WriteLine("Danh sách điểm: ");
            foreach (int diem in diemSo)
            {
                Console.Write(diem + " ");
            }
            Console.WriteLine("\nĐiểm trung bình: " + TinhTrungBinh(diemSo));
            Console.WriteLine("Điểm cao nhất: " + TimMax(diemSo));
            Console.WriteLine("Điểm thấp nhất: " + TimMin(diemSo));
        }

    }
}
