using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace Team11.Menus
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        public Menu Mainmenu;
        public String gameScene;
        [SerializeField] private TMP_Dropdown serverDropdown;
        
        public bool offlineMode = false;
        
        private void Start()
        {
            if (offlineMode)
            {
                PhotonNetwork.OfflineMode = true;
            }

            if (!PhotonNetwork.IsConnected)
            {
                Mainmenu.ShowServerSelection();
            }

        }

        public void ConnectToServerByRegion()
        {
            var region = serverDropdown.options[serverDropdown.value].text;
            PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            var result = PhotonNetwork.ConnectToRegion(region);
            Debug.Log("ConnectToRegion(" + region + ") ->" + result);
            Mainmenu.Loading("Connecting to Server");
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnConnectedToMaster()
        {
            print(PhotonNetwork.GetPing());
            print(PhotonNetwork.CloudRegion);
            if (offlineMode)
            {
                PhotonNetwork.CreateRoom("Testing Room");
            }
            else
            {
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnJoinedLobby()
        {
            Mainmenu.RegionSelected();
        }
        
        public void CreateRoom(string roomID)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom(roomID,roomOptions,TypedLobby.Default);
            
        }
        
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(gameScene);
        }
    }
}
