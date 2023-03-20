using Cinemachine;
using StarterAssets;
using UnityEngine;

namespace Team11.Menus
{
    public class SettingsApplier : MonoBehaviour
    {
        private CinemachineVirtualCamera _camera;
        
        private void Awake()
        {
            _camera = FindObjectOfType<CinemachineVirtualCamera>();
            SettingsMenu.OnApplySettings += ApplySettings;
        }

        private void OnDestroy()
        {
            SettingsMenu.OnApplySettings -= ApplySettings;
        }

        private void ApplySettings(SettingsData data)
        {
            SetScreenSettings(data);
            if (_camera != null) 
                _camera.m_Lens.FieldOfView = data.fov;
            StarterAssetsInputs.Sensitivity = data.sensitivity;
        }

        private void SetScreenSettings(SettingsData data)
        {
            Screen.fullScreen = data.fullScreen;
            Screen.SetResolution(data.resolution.x, data.resolution.y, data.fullScreen);
            Application.targetFrameRate = GetFrameTarget(data.frameLimiter);
            QualitySettings.vSyncCount = data.vsync ? 1 : 0;
            print(data.resolution.ToString());
        }

        private bool IsSameResolution(Vector2Int currentRes)
        {
            return Screen.currentResolution.width == currentRes.x && Screen.currentResolution.height == currentRes.y;
        }

        private int GetFrameTarget(FrameOptions frameOptions)
        {
            return frameOptions switch
            {
                FrameOptions.Thirty => 30,
                FrameOptions.Sixty => 60,
                FrameOptions.OneTwenty => 120,
                _ => 120
            };
        }
    }
}