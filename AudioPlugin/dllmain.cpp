// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <windows.h>
#include <mmdeviceapi.h>
#include <endpointvolume.h>

#pragma comment(lib, "ole32.lib")

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

extern "C" __declspec(dllexport) bool IsSystemMuted() 
{
    bool isMuted = false;
   
    HRESULT hr = CoInitialize(NULL);
    if (SUCCEEDED(hr)) 
    {
        IMMDeviceEnumerator* pEnumerator = NULL;
        hr = CoCreateInstance(__uuidof(MMDeviceEnumerator), NULL, CLSCTX_ALL, __uuidof(IMMDeviceEnumerator), (void**)&pEnumerator);

        if (SUCCEEDED(hr)) 
        {
            IMMDevice* pDevice = NULL;
            hr = pEnumerator->GetDefaultAudioEndpoint(eRender, eMultimedia, &pDevice);

            if (SUCCEEDED(hr)) 
            {
                IAudioEndpointVolume* pEndpointVolume = NULL;
                hr = pDevice->Activate(__uuidof(IAudioEndpointVolume), CLSCTX_ALL, NULL, (void**)&pEndpointVolume);

                if (SUCCEEDED(hr)) 
                {
                    BOOL mute;
                    pEndpointVolume->GetMute(&mute);
					isMuted = (mute != 0); //windows BOOL -> C++ bool
                    pEndpointVolume->Release();
                }
                pDevice->Release();
            }
            pEnumerator->Release();
        }
        CoUninitialize();
    }
    return isMuted;
}