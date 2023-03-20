using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Team11.Players;
using UnityEngine.UI;

namespace Team11.Menus
{
    public class Setup : MonoBehaviourPunCallbacks
    {
        public PlayerBase P1;
        public PlayerBase P2;
        
        [SerializeField] private Image Crosshair;
        [SerializeField] private PlayerSettings playerSettings;
        
        public UnityEvent OnStartPlaying;
        
        private void Start()
        {
            if (!PhotonNetwork.OfflineMode && PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                if (!PhotonNetwork.IsMasterClient)
                {
                    SelectP2();
                    photonView.RPC(nameof(SelectP1), RpcTarget.OthersBuffered);
                }
            }
            OnJoinedRoom();
        }

        public override void OnJoinedRoom()
        {
            if (PhotonNetwork.OfflineMode || !PhotonNetwork.IsConnected)
            {
                SelectP1();
            }
        }

        [PunRPC]
        public void SelectP1()
        {
            P1.Select(playerSettings);
            SetupPlay();
        }
        
        [PunRPC]
        public void SelectP2()
        {
            P2.Select(playerSettings);
            SetupPlay();
        }

        private void SetupPlay()
        {
            ChangeCursorState(true);
            Crosshair.enabled = true;
            OnStartPlaying?.Invoke();
        }

        public void SetCamera(CinemachineVirtualCamera cam) => 
            playerSettings.vcam = cam;

        private void ChangeCursorState(bool state)
        {
            Cursor.visible = !state;
            Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
    
    [System.Serializable]
    public class PlayerSettings{
        public VignetteIntensity vignette;
        public CinemachineVirtualCamera vcam;
        public EscapeMenu escapemenu;
    }
}