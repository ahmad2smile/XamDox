#include "FaceDetection.h"

#define PLATFORM_ANDROID 0
#define PLATFORM_IOS 1

int FaceDetection::GetTemplateInfo()
{
#if PLATFORM == PLATFORM_IOS
	const auto info = 10;
#elif PLATFORM == PLATFORM_ANDROID
	const int info = 11;
#else
	const int info = 12;
#endif

	return info;
}