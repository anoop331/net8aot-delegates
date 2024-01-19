#include <iostream>
#include <dlfcn.h>
#include <unistd.h>

#define symLoad dlsym



int add(int x, int y);
int longRunningProcess();

extern "C" void progressCallBack(int value)
{
    std::cout << "The progress is: " << value << std::endl;
    if(value == 100){
         std::cout << "All done!" << std::endl;
    }
}



int main(int argc, char** argv)
{
    // std::cout << "Starting up" << std::endl;
    // auto result = add(2,8);
    // std::cout << "Sum of 2 and 8 is " << result <<  std::endl;

    std::cout << "Starting up" << std::endl;
    auto result = longRunningProcess();
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

int longRunningProcess(){
    auto handle =  dlopen("./aotlib.so", RTLD_LAZY);
    if (!handle) {
        std::cerr << "Failed to load the library" << std::endl;
        return -1;
    }

    // Get the function address
    typedef int (*LongRunningProcessFunc)(int, void (*)(int));
    auto longRunningProcessFuncHandle = (LongRunningProcessFunc)dlsym(handle, "longRunningProcess");

    const char* dlsym_error = dlerror();

    if (dlsym_error) {
        std::cerr << "Cannot load symbol 'CallCppDelegate': " << dlsym_error << std::endl;
        dlclose(handle);
        return -1;
    }

    // Call the function with a C++ function as a delegate
    auto result = longRunningProcessFuncHandle(10,progressCallBack);

    dlclose(handle);
    return result;

}