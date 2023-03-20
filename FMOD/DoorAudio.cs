using UnityEngine;
using FMODUnity;
using Team11.Interactions;

namespace Team11.Audio
{
    public class DoorAudio : MonoBehaviour
    {
        [SerializeField] private Door _door;
        [SerializeField] private string openSoundPath;
        [SerializeField] private string closeSoundPath;
        private FMOD.Studio.EventInstance doorSound;

        /*
        private void Update()
        {
            if (_door.openState)
            {
                //RuntimeManager.PlayOneShot(closeSoundPath);
                doorSound = RuntimeManager.CreateInstance(closeSoundPath);
                doorSound.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
                doorSound.start();
                doorSound.release();
            }
            else
            {
                //RuntimeManager.PlayOneShot(openSoundPath);
                doorSound = RuntimeManager.CreateInstance(openSoundPath);
                doorSound.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
                doorSound.start();
                doorSound.release();
            }
        }
        */
    }
}

