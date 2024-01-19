using System.Runtime.InteropServices;

namespace AotLib;

public static class NativeEntryPoints
{

    [UnmanagedCallersOnly(EntryPoint = "add")]
    public static int Add(int x, int y)
    {
        return x + y;
    }
    
    // public static unsafe delegate* unmanaged[Cdecl]<int, void> CppCallback;

    [UnmanagedCallersOnly(EntryPoint = "longRunningProcess")]
    public static unsafe int RunLongRunningProcess(int input, delegate* unmanaged[Cdecl]<int, void> callback)
    {

        for(var i=0; i < input; i++){
            var percentage = (int)(i * 100/ input);
            callback(percentage);
            System.Threading.Thread.Sleep(2000);
        }

        return 0;
    }

}
