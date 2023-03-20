using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Team11.Interactions;
using UnityEngine;
using UnityEngine.Events;

public class Blockification : PlaceableObject, IContainer
{
    [SerializeField] private GameObject movableObjectPrefab;
    [SerializeField] private Transform spawnPoint;
    public UnityEvent OnTorchPlaced;

    [PunRPC]
    public void InstantiateMovableObject()
    {
        PhotonNetwork.Instantiate(movableObjectPrefab.name,spawnPoint.position,Quaternion.identity);
    }

    [PunRPC]
    public void DestroyTorch(int pickupId)
    {
        // PhotonNetwork.GetPhotonView(pickupId).RequestOwnership();
        PhotonNetwork.Destroy(PhotonNetwork.GetPhotonView(pickupId).gameObject);
        
    }
    
    [ContextMenu("PhotonDestroy")]
    public void PhotonDestroy()
    {
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
    }

    public bool PutIn(IPickup pickup) 
    {

        OnTorchPlaced.Invoke();
        // photonView.RPC(nameof(DestroyTorch), RpcTarget.MasterClient, ((Torch)pickup).photonView.ViewID);
        DestroyTorch(((Torch)pickup).photonView.ViewID);
        photonView.RPC(nameof(InstantiateMovableObject), RpcTarget.MasterClient);
        return true;
    }

    public IPickup TakeOut() 
    {
        return null;
    }
}
