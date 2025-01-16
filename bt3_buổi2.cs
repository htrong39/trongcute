using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRỌNG_CU_TE
{
    internal class Program
    {
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
        public static int TimMax(ArrayList list)
        {
            int max = (int)list[0];
            foreach (int value in list)
            {
                if (value > max) max = value;
            }
            return max;
        }
        public static int TimMin(ArrayList list)
        {
            int min = (int)list[0];
            foreach (int value in list)
            {
                if (value < min) min = value;
            }
            return min;
        }
        public static double TinhTrungBinh(ArrayList list)
        {
            int sum = 0;
            foreach (int value in list)
            {
                sum += value;
            }
            return (double)sum / list.Count;
        }

        public static void Main(string[] args)
        {
            Console .OutputEncoding = Encoding.UTF8;
            ArrayList diemSo = TaoArrayList(10);
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
