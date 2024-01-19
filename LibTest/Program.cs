using System.Runtime.InteropServices;

namespace LibTest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("$Starting up the DLL interop");

            var callback = new ProgressCallBackkDelegate(OnProgressChange);
            var result = longRunningProcess(10,callback);            
            
        }

        public static void OnProgressChange(int value)
        {
            Console.WriteLine($"The progress is : {value} %");
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ProgressCallBackkDelegate(int value);


        [DllImport("aotlib.so")]
        public static extern int add(int x, int y);

        [DllImport("aotlib.so")]
        public static extern int longRunningProcess(int x, ProgressCallBackkDelegate  callback);
    }
}
