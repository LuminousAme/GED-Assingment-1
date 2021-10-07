//Game Engine Design and Implementation INFR 3310U Midterm Quiz
//Ame Gilham 100741352 - Developer Role
//Random position generator 

#define RandomPos_API __declspec(dllexport)
#include <random>
#include <time.h>

extern "C" {
	void RandomPos_API runOnLaunch() {
		srand(time(NULL));
	}

	float RandomPos_API RandFloat(float min, float max) {
		float random = (float)(rand()) / (float)(RAND_MAX); //get a random float in the 0f-1f range

		//convert to range
		float randomInRange = (random * (max - min)) + min; //convert it to the min-max range
		//handle if the range is zero
		if (max - min == 0.0f) randomInRange = min; //if the range is zero, min and max are the same, so we can just say it's the minimum
		//return converted number
		return randomInRange;
	}
}