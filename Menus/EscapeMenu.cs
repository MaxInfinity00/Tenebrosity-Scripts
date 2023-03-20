using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.AI;

namespace Team11.Menus
{
    public class EscapeMenu : MonoBehaviourPunCallbacks
    {
        public TextMeshProUGUI MessageText;

        [SerializeField] private GameObject EscapeMenuObj;
        [SerializeField] private GameObject SettingsMenuObj;
        [SerializeField] private SettingsMenu settingsMenu;
        [SerializeField] private string MainMenuSceneName = "Proto Main Menu";
        
        private InputScheme _inputs;
        private PlayerInput _playerInput;
        private MenuState _menuState;
    
        public static event Action<MenuState> OnMenuStateChanged; 
        
        private void Start()
        {
            _inputs = new();
            _inputs.UI.Menu.performed += Menu;
            _inputs.UI.Menu.Enable();
            settingsMenu.SetPath();
            settingsMenu.Load();
        }
        
        private void OnDestroy()
        {
            _inputs.UI.Menu.performed -= Menu;
            _inputs.UI.Menu.Disable();
        }
        
        public void SetPlayerInput(PlayerInput playerInput)
        {
            _playerInput = playerInput;
            if(_menuState != MenuState.Playing)
            {
                _playerInput.enabled = false;
            }
            MessageText.gameObject.SetActive(false);
        }

        public void BackFromSettings()
        {
            Menu(new InputAction.CallbackContext());
        }

        private void Menu(InputAction.CallbackContext _)
        {
            if (_menuState == MenuState.Playing)
            {
                _menuState = MenuState.EscapeMenu;
                EscapeMenuObj.SetActive(true);
                if(_playerInput != null)
                {
                    _playerInput.enabled = false;
                }
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (_menuState == MenuState.EscapeMenu)
            {
                _menuState = MenuState.Playing;
                EscapeMenuObj.SetActive(false);
                if(_playerInput != null)
                {
                    _playerInput.enabled = true;
                }
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                
            }
            else if (_menuState == MenuState.SettingsMenu)
            {
                _menuState = MenuState.EscapeMenu;
                EscapeMenuObj.SetActive(true);
                SettingsMenuObj.SetActive(false);
            }
            OnMenuStateChanged?.Invoke(_menuState);
        }
        
        public void Resume()
        {
            Menu(new InputAction.CallbackContext());
        }
        
        public void CallResetLevel()
        {
            photonView.RPC(nameof(ResetLevel), RpcTarget.All);
        }
        
        [PunRPC]
        public void ResetLevel() {
            FMOD.ChannelGroup mcg;
            FMODUnity.RuntimeManager.CoreSystem.getMasterChannelGroup(out mcg);
            mcg.stop();
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
        }

        public void OpenSettings()
        {
            _menuState = MenuState.SettingsMenu;
            EscapeMenuObj.SetActive(false);
            SettingsMenuObj.SetActive(true);
        }
        
        public void Quit()
        {
            FMOD.ChannelGroup mcg;
            FMODUnity.RuntimeManager.CoreSystem.getMasterChannelGroup(out mcg);
            mcg.stop();
            PhotonNetwork.LeaveRoom(false);
            PhotonNetwork.LoadLevel(MainMenuSceneName);
        }
        
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            MessageText.text = "Other player left the game";
            MessageText.gameObject.SetActive(true);
            Invoke(nameof(Quit), 5f);
        }
        
        public void DeactivateMenusAndPlayers()
        {
            
            while (_menuState!= MenuState.Playing)
            {
                Menu(new InputAction.CallbackContext());
            }
            _inputs.UI.Menu.performed -= Menu;
            _inputs.UI.Menu.Disable();
            if(_playerInput != null)
            {
                _playerInput.enabled = false;
            }
        }

    }

    public enum MenuState
    {
        Playing,
        EscapeMenu,
        SettingsMenu
    }
}