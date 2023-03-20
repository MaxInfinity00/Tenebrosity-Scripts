using System;
using System.Collections.Generic;
using Photon.Pun;
using Team11.Players;
using UnityEngine;
using UnityEngine.Events;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PhotonView))]
    public class KeyCollection : MonoBehaviourPunCallbacks
    {
        [SerializeField] private List<Key> keys;

        private int _currentKeyCount;
        
        public UnityEvent OnUnlock;

        private int _playersInTrigger;

        private void Start()
        {
            foreach (var key in keys)
            {
                key.OnCollected.AddListener(KeyCollected);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerBase>() == null) return;
            int objectID = other.GetComponent<PhotonView>().ViewID;
            if (!PhotonNetwork.GetPhotonView(objectID).IsMine) return;
            photonView.RPC(nameof(UpdatePlayerCount), RpcTarget.All, 1);
            if(_currentKeyCount >= keys.Count && _playersInTrigger == 2)
                OnUnlock?.Invoke();
        }

        private void OnTriggerExit(Collider other) {
            if(other.GetComponent<PlayerBase>() == null) return;
            int objectID = other.GetComponent<PhotonView>().ViewID;
            if (!PhotonNetwork.GetPhotonView(objectID).IsMine) return;
            photonView.RPC(nameof(UpdatePlayerCount), RpcTarget.All, -1);
            // if(_currentKeyCount >= keys.Count)
            //     OnUnlock?.Invoke();
        }

        [PunRPC]
        private void UpdatePlayerCount(int count)
        {
            _playersInTrigger += count;
            if(_currentKeyCount >= keys.Count && _playersInTrigger == 2)
                OnUnlock?.Invoke();
        }

        private void KeyCollected()
        {
            photonView.RPC(nameof(AddKey), RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void AddKey()
        {
            _currentKeyCount++;
        }
    }
}