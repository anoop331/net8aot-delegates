using System.Runtime.InteropServices;

namespace AotLib;

public static class NativeEntryPoints
{

    [UnmanagedCallersOnly(EntryPoint = "add")]
    public static int Add(int x, int y)
    {
        return x + y;
    }

    // [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    // public delegate void CppCallback(int value);

    public static unsafe delegate* unmanaged[Cdecl]<int, void> CppCallback;

    [UnmanagedCallersOnly(EntryPoint = "CallCppDelegate")]
    public static unsafe void CallCppDelegate(delegate* unmanaged[Cdecl]<int, void> cppCallback)
    {
        if (cppCallback != null)
        {
            cppCallback(100); // Example argument
        }
    }

}
