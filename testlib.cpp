#include <iostream>
#include <dlfcn.h>
#include <unistd.h>

#define symLoad dlsym



int add(int x, int y);
void callDelegate();

extern "C" void MyCppFunction(int value)
{
    std::cout << "Called from C#: " << value << std::endl;
}

typedef void (*CallCppDelegateFunction)(void (*)(int));

int main(int argc, char** argv)
{
    // std::cout << "Starting up" << std::endl;
    // auto result = add(2,8);
    // std::cout << "Sum of 2 and 8 is " << result <<  std::endl;

    std::cout << "Starting up" << std::endl;
    callDelegate();
    std::cout << "Done with calling the delegate "  <<  std::endl;

    
}

int add(int x, int y)
{   
    std::cout << "Loading lib" << std::endl;
    auto handle = dlopen("./aotlib.so", RTLD_LAZY);
    typedef int (*addFunc)(int,int);
    addFunc addFuncHandle = (addFunc)symLoad(handle, "add");    
    std::cout << "Calling add" << std::endl;
    auto result = addFuncHandle(x,y);    
    return result;
}

void callDelegate(){
    auto handle =  dlopen("./aotlib.so", RTLD_LAZY);
    if (!handle) {
        std::cerr << "Failed to load the library" << std::endl;
        return;
    }

    // Get the function address
    auto callCppDelegate = (CallCppDelegateFunction)dlsym(handle, "CallCppDelegate");

    const char* dlsym_error = dlerror();

    if (dlsym_error) {
        std::cerr << "Cannot load symbol 'CallCppDelegate': " << dlsym_error << std::endl;
        dlclose(handle);
        return;
    }

    // Call the function with a C++ function as a delegate
    callCppDelegate(MyCppFunction);

    dlclose(handle);
    return;

}