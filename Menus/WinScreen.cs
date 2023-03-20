using UnityEngine;
using UnityEngine.UI;

namespace Team11.Menus
{
    public class WinScreen : MonoBehaviour
    {
        public GameObject WinScreenObj;
        public Image FadeImage;
        public GameObject CreditsObj;
        public GameObject QuitButton;
        public float CreditsYLocation;
        public float TimeToWhite;
        public float TimeToGreen;
        public float ScrollTime;


        [ContextMenu("Roll Credits")]
        public void OnWin()
        {
            WinScreenObj.SetActive(true);
            LeanTween.value(gameObject, new Color(1, 1, 1, 0), Color.white, TimeToWhite).setOnUpdate(
                (value) => { FadeImage.color = value; }
            ).setOnComplete(
                () =>
                {
                    LeanTween.value(gameObject, Color.white, new Color(0.17f, 0.33f, 0.34f, 1), TimeToGreen).setOnUpdate(
                        (value) => { FadeImage.color = value; }
                    ).setOnComplete(
                        () =>
                        {
                            QuitButton.SetActive(true);
                            Cursor.lockState = CursorLockMode.None;
                            Cursor.visible = true;
                            LeanTween.moveLocalY(CreditsObj, CreditsYLocation, ScrollTime);
                        }
                    );
                }
            );
        }
    }
}