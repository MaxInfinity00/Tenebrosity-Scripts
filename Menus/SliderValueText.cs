using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Time11.Menus
{
    [RequireComponent(typeof(TMP_Text))]
    public class SliderValueText : MonoBehaviour
    {
        [SerializeField] private int multiplier = 1;
        [SerializeField] private string format = "0.0";
        private void Start()
        {
            var text = GetComponent<TMP_Text>();
            var slider = GetComponentInParent<Slider>();
            if(slider == null) return;
            text.text = (slider.value * multiplier).ToString(format);
            slider.onValueChanged.AddListener(value =>
                {
                    text.text = (value * multiplier).ToString(format);
                }
            );
        }
    }
}
