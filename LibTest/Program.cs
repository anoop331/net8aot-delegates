﻿using System.Runtime.InteropServices;

namespace LibTest
{

    public struct ReturnValue{
        public int success;
        public int totalSeconds;
        public IntPtr message;
        
    }

    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("$Starting up the DLL interop");

            var callback = new ProgressCallBackkDelegate(OnProgressChange);
            var doneCallBack = new DoneCallBackDelegate(onDoneCallBack);
            var result = longRunningProcess(10,callback,doneCallBack);  

            Console.WriteLine("Press any key to exit the app");
            Console.ReadLine();          
            
        }

        public static void onDoneCallBack(ReturnValue value)
        {
            Console.WriteLine($"Swdl done and it is : {(value.success == 1 ? "Successful":"Failure")} and it took {value.totalSeconds} seconds with message {Marshal.PtrToStringAnsi(value.message)}");
        }


        public static void OnProgressChange(int value)
        {
            Console.WriteLine($"The progress is : {value} %");
        }


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DoneCallBackDelegate(ReturnValue value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ProgressCallBackkDelegate(int value);


        [DllImport("aotlib.so")]
        public static extern int add(int x, int y);

        [DllImport("aotlib.so")]
        public static extern int longRunningProcess(int x, ProgressCallBackkDelegate  callback, DoneCallBackDelegate doneCallBack);
    }
}
