using Photon.Pun;

namespace Team11.Menus
{
    public class OfflineTester : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.OfflineMode = true;
            }
        }
        
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.CreateRoom("Testing Room");
        }
    }
}