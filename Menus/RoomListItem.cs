using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Team11.Menus
{
    public class RoomListItem : MonoBehaviour
    {
        private RoomInfo _roomInfo;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;
        
        public void Initialize(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;
            _text.text = roomInfo.Name;
            _button.onClick.AddListener(() =>
                {
                    FindObjectOfType<Menu>().Loading("Joining Room");
                    PhotonNetwork.JoinRoom(roomInfo.Name);
                }
            );
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}