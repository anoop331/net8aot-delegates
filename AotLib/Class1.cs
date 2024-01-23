using System.Runtime.InteropServices;

namespace AotLib;

public struct ReturnValue{
    public int success;
    public int totalSeconds;
    public IntPtr message;
    
}

public static class NativeEntryPoints
{

    [UnmanagedCallersOnly(EntryPoint = "add")]
    public static int Add(int x, int y)
    {
        return x + y;
    }
    
    // public static unsafe delegate* unmanaged[Cdecl]<int, void> CppCallback;

    [UnmanagedCallersOnly(EntryPoint = "longRunningProcess")]
    public static unsafe int RunLongRunningProcess(int input, delegate* unmanaged[Cdecl]<int, void> callback, delegate* unmanaged[Cdecl]<ReturnValue, void> doneCallBack)
    {

        LongRunningOperationAsync(input,ProgressCallBack).ContinueWith(x => {
            Console.WriteLine($"NativeEntryPoint.SwdlAsync->Done callback");
            var result = x.Result;
            doneCallBack(result);
        });

         void ProgressCallBack(int x)
        {
            callback(x);
        }

        return 0;

        // Task.Run(() => {
        //      for(var i=0; i < input; i++){
        //     var percentage = (int)(i * 100/ input);
        //     callback(percentage);
        //    Thread.Sleep(2000);
        // }

        // Console.WriteLine($"AotLib: All done, calling the native call back");

        // var result = new ReturnValue{
        //     success = 1,
        //     totalSeconds = 300
        // };

        
        // doneCallBack(result);
        
        // Console.WriteLine($"AotLib: Done with calling the native call back");

        // }) ;      

        // return 0;

    }


    private static async Task<ReturnValue> LongRunningOperationAsync(int input, Action<int> progressCallBack)
    {
         
            for(var i=0; i < input; i++)
            {
                var percentage = (int)(i * 100/ input);
                progressCallBack(percentage);
                await Task.Delay(2000);    
            }            

            Console.WriteLine($"AotLib: All done, calling the native call back");

            var result = new ReturnValue{
                success = 1,
                totalSeconds = 300,
                message = Marshal.StringToHGlobalAnsi("all good")
            };           
            
            
            Console.WriteLine($"AotLib: Done with calling the native call back");

            return result;
            
    }

}
