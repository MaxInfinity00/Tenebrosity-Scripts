using UnityEngine;

namespace Team11.Menus
{
    public class SoundVolumeApplier : MonoBehaviour
    {
        private SettingsData _currentSettings;
        private bool _initSet = true;
        private MenuState _currentState;
        
        private void Start()
        {
            SettingsMenu.OnApplySettings += ApplySettings;
            EscapeMenu.OnMenuStateChanged += ChangeSoundLevels;
        }

        private void OnDestroy()
        {
            SettingsMenu.OnApplySettings -= ApplySettings;
            EscapeMenu.OnMenuStateChanged -= ChangeSoundLevels;
        }

        private void ApplySettings(SettingsData data)
        {
            _currentSettings = new SettingsData(data);
            ChangeSoundLevels(_currentState);
        }

        private void ChangeSoundLevels(MenuState state)
        {
            _currentState = state;
            switch (state)
            {
                case MenuState.Playing:
                    SetVolumes();
                    break;
                case MenuState.SettingsMenu:
                case MenuState.EscapeMenu:
                    FadeVolumes();
                    break;
            }
        }

        private void SetVolumes()
        {
            _initSet = false;
            FMOD.Studio.Bus master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
            FMOD.Studio.Bus ambience = FMODUnity.RuntimeManager.GetBus("bus:/Master/Amb");
            FMOD.Studio.Bus sfx = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
            master.setVolume(_currentSettings.masterVolume);
            ambience.setVolume(_currentSettings.ambienceVolume);
            sfx.setVolume(_currentSettings.soundEffectVolume);
        }

        private void FadeVolumes()
        {
            FMOD.Studio.Bus master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
            FMOD.Studio.Bus ambience = FMODUnity.RuntimeManager.GetBus("bus:/Master/Amb");
            FMOD.Studio.Bus sfx = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
            master.setVolume(_currentSettings.masterVolume);
            ambience.setVolume(.3f);
            sfx.setVolume(0);
        }
    }
}