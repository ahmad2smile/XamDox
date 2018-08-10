#include "pch.h"
#include "FaceDetection.UWP.h"
#include "FaceDetection.h"

using namespace Platform;

int FaceDetection_UWP::FaceDetectionBridge::MeaningOfLife()
{
	return FaceDetection::GetTemplateInfo();
}