#include "FaceDetection.h"

extern "C" int AndroidInfo()
{
	return FaceDetection::GetTemplateInfo();
}