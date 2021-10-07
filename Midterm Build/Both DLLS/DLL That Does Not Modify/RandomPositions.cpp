//Game Engine Design and Implementation INFR 3310U Midterm Quiz
//Ame Gilham 100741352 - Developer Role
//Random position generator 

#define RandomPos_API __declspec(dllexport)
#include <iostream> 

extern "C" {
	void RandomPos_API someOtherFunction() {
		printf("This is the one that does not do the randomization");
	}
}