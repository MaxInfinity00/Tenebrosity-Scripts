using UnityEngine;

namespace Team11.Menus
{
    [System.Serializable]
    public class SettingsData
    {
        public float masterVolume = 1;
        public float soundEffectVolume = 1;
        public float ambienceVolume = 1;
        public float fov = 90;
        public float sensitivity = 1;
        public float brightness = 5;
        public bool vsync = true;
        public bool fullScreen = true;
        public Vector2Int resolution = new(1920, 1080);
        public FrameOptions frameLimiter = FrameOptions.OneTwenty;

        #region Properties

        public float MasterVolume
        {
            get => masterVolume;
            set => masterVolume = value;
        }

        public float SoundEffectVolume
        {
            get => soundEffectVolume;
            set => soundEffectVolume = value;
        }

        public float AmbienceVolume
        {
            get => ambienceVolume;
            set => ambienceVolume = value;
        }

        public float FOV
        {
            get => fov;
            set => fov = value;
        }

        public float Sensitivity
        {
            get => sensitivity;
            set => sensitivity = value;
        }
        
        public bool Vsync
        {
            get => vsync;
            set => vsync = value;
        }

        public Vector2Int Resolution1
        {
            get => resolution;
            set => resolution = value;
        }

        public FrameOptions FrameLimiter
        {
            get => frameLimiter;
            set => frameLimiter = value;
        }

        public float Brightness
        {
            get => brightness;
            set => brightness = value;
        }

        public bool FullScreen
        {
            get => fullScreen;
            set => fullScreen = value;
        }

        #endregion

        public SettingsData() { }

        public SettingsData(SettingsData data)
        {
            masterVolume = data.masterVolume;
            soundEffectVolume = data.soundEffectVolume;
            ambienceVolume = data.ambienceVolume;
            fov = data.fov;
            sensitivity = data.sensitivity;
            vsync = data.vsync;
            fullScreen = data.fullScreen;
            resolution = data.resolution;
            frameLimiter = data.frameLimiter;
            brightness = data.brightness;
        }

        public void SetValue<T>(string variableName, T value)
        {
            var propertyInfo = GetType().GetProperty(variableName);
            if (propertyInfo != null)
            {
                var setter = propertyInfo.SetMethod;
                setter.Invoke(this, new object[] { value });
            }
        }
    }

    public enum FrameOptions
    {
        Thirty,
        Sixty,
        OneTwenty
    }
}