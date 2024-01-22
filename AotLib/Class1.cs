using System.Runtime.InteropServices;

namespace AotLib;

public struct ReturnValue{
    public int success;
    public int totalSeconds;
    
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

        Task.Run(() => {
             for(var i=0; i < input; i++){
            var percentage = (int)(i * 100/ input);
            callback(percentage);
           Thread.Sleep(2000);
        }

        Console.WriteLine($"AotLib: All done, calling the native call back");

        var result = new ReturnValue{
            success = 1,
            totalSeconds = 300
        };

        
        doneCallBack(result);
        
        Console.WriteLine($"AotLib: Done with calling the native call back");

        }) ;      

        return 0;

    }

}
