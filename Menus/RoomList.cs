using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Team11.Menus
{
    public class RoomList : MonoBehaviourPunCallbacks
    {
        
        private List<RoomInfo> _roomList = new List<RoomInfo>();
        private Dictionary<string,RoomListItem> _roomListItemDict = new Dictionary<string, RoomListItem>();

        [SerializeField] private Transform Content;
        [SerializeField] private RoomListItem RoomListItemPrefab;

        [SerializeField] private TextMeshProUGUI logText;
        
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            logText.text+= "OnRoomListUpdate called";
            roomList.ForEach(room =>
                {
                    if(room.RemovedFromList)
                    {
                        Debug.Log("Room " + room.Name +" Removed");
                        logText.text += "Room " + room.Name + " Removed";;
                        RemoveFromList(room);
                    }
                    else if(_roomListItemDict.ContainsKey(room.Name))
                    {
                        Debug.Log("Room " + room.Name +" Updated");
                        logText.text += "Room " + room.Name + " Updated";;
                        UpdateList(room);
                    }
                    else
                    {
                        AddToList(room);
                        Debug.Log("Room " + room.Name + " Added");
                        logText.text += "Room " + room.Name + " Added";;
                    }
                }
            );
        }
        
        private void AddToList(RoomInfo room)
        {
            _roomList.Add(room);
            var listItem = Instantiate(RoomListItemPrefab, Content);
            listItem.Initialize(room);
            _roomListItemDict.Add(room.Name, listItem);
            UpdateList(room);
        }

        private void UpdateList(RoomInfo room)
        {
            if(room.MaxPlayers == room.PlayerCount)
            {
                _roomListItemDict[room.Name].gameObject.SetActive(false);
            }
            else
            {
                _roomListItemDict[room.Name].gameObject.SetActive(true);
            }
        }
        
        private void RemoveFromList(RoomInfo room)
        {
            _roomList.Remove(room);
            if (!_roomListItemDict.ContainsKey(room.Name)) return;
            _roomListItemDict[room.Name].Destroy();
            _roomListItemDict.Remove(room.Name);
        }
    }
}