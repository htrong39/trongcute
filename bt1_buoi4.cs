using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        //Câu 1
        int[] arr = { 1, 3, 5, 7, 9, 11, 15, 18, 20, 25 };
        int target = 15;
        //SeqSearch
        MeasureTime(() =>
        {
            int result = SequentialSearch(arr, target);
            Console.WriteLine(result != -1 ? $"Tim thay tai {result}" : "Khong tim thay");
        }, "Sequential Search");
        //RecuSearch
        MeasureTime(() =>
        {
            int result = RecursiveSearch(arr, target, 0);
            Console.WriteLine(result != -1 ? $"Tim thay tai {result}" : "Khong tim thay");
        }, "Recursive Search");
        //SenSearch
        MeasureTime(() =>
        {
            int result = SentinelSearch(arr, target);
            Console.WriteLine(result != -1 ? $"Tim thay tai {result}" : "Khong tim thay");
        }, "Sentinel Search");
        //BinSearch
        MeasureTime(() =>
        {
            int result = BinarySearch(arr, target);
            Console.WriteLine(result != -1 ? $"Tim thay tai {result}" : "Khong tim thay");
        }, "Binary Search");
    }
    //Hàm đo thời gian
    static void MeasureTime(Action searchMethod, string searchName)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        searchMethod();
        stopwatch.Stop();
        Console.WriteLine($"{searchName} thoi gian thuc hien: {stopwatch.ElapsedTicks} ticks\n");
    }
    //SeqSearch
    static int SequentialSearch(int[] arr, int target)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == target)
                return i;
        }
        return -1;
    }
    //RecuSearch
    static int RecursiveSearch(int[] arr, int target, int index)
    {
        if (index >= arr.Length) return -1;
        if (arr[index] == target) return index;
        return RecursiveSearch(arr, target, index + 1);
    }
    //SenSearch
    static int SentinelSearch(int[] arr, int target)
    {
        int last = arr[arr.Length - 1];
        arr[arr.Length - 1] = target;
        int i = 0;
        while (arr[i] != target) i++;
        arr[arr.Length - 1] = last;
        return (i < arr.Length - 1 || arr[arr.Length - 1] == target) ? i : -1;
    }
    //BinSearch
    static int BinarySearch(int[] arr, int target)
    {
        int left = 0, right = arr.Length - 1;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (arr[mid] == target)
                return mid;
            if (arr[mid] < target)
                left = mid + 1;
            else
                right = mid - 1;
        }
        return -1;
    }
    class SinhVien
    {
        public int Id { get; set; }
        public string HoTen { get; set; }
        public double DiemTB { get; set; }
        public SinhVien(int id, string hoten, double diemtb)
        {
            Id = id;
            HoTen = hoten;
            DiemTB = diemtb;
        }
    }
}


