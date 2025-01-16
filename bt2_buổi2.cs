using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRỌNG_CU_TE
{
    internal class Program
    {
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
        public static int TimMax(List<int> list) => list.Max();
        public static int TimMin(List<int> list) => list.Min();
        public static double TinhTrungBinh(List<int> list) => list.Average();
        public static void Main(string[] args)
        {
            Console .OutputEncoding = Encoding.UTF8;
            List<int> diemSo = TaoDanhSach(10);
            Console.WriteLine("Danh sách điểm: " + string.Join(", ", diemSo));
            Console.WriteLine("Điểm trung bình: " + TinhTrungBinh(diemSo));
            Console.WriteLine("Điểm cao nhất: " + TimMax(diemSo));
            Console.WriteLine("Điểm thấp nhất: " + TimMin(diemSo));
        }
    }
}
