using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace Team11.Menus
{
    public class Menu : MonoBehaviour
    {
        public ConnectToServer connectToServer;
        public GameObject ServerSelection;
        public GameObject MainMenu;
        public GameObject CreateARoomMenu;
        public GameObject JoinARoomMenu;
        public GameObject SettingsMenu;
        public GameObject LoadingPage;
        public GameObject CreditsPage;
        public TextMeshProUGUI LoadingText;
        public TMP_InputField RoomNameInputField;
        
        private InputScheme _inputs;
        private MainMenuState _mainMenuState = MainMenuState.Loading;
        

        private void Start()
        {
            SettingsMenu.GetComponent<SettingsMenu>().Load();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _inputs = new();
            _inputs.UI.Menu.performed += Back;
            _inputs.UI.Menu.Enable();
        }
        
        private void OnDestroy()
        {
            _inputs.UI.Menu.performed -= Back;
            _inputs.UI.Menu.Disable();
        }

        public void BackBtnCall()
        {
            Back(new InputAction.CallbackContext());
        }

        public void ShowServerSelection()
        {
            LoadingPage.SetActive(false);
            ServerSelection.SetActive(true);
            _mainMenuState = MainMenuState.ServerSelection;
        }
        
        public void Back(InputAction.CallbackContext _)
        {
            switch (_mainMenuState)
            {
                case MainMenuState.MainMenu:
                    connectToServer.Disconnect();
                    MainMenu.SetActive(false);
                    ServerSelection.SetActive(true);
                    break;
                case MainMenuState.CreateARoom:
                    CreateARoomMenu.SetActive(false);
                    MainMenu.SetActive(true);
                    _mainMenuState = MainMenuState.MainMenu;
                    break;
                case MainMenuState.JoinARoom:
                    JoinARoomMenu.SetActive(false);
                    MainMenu.SetActive(true);
                    _mainMenuState = MainMenuState.MainMenu;
                    break;
                case MainMenuState.SettingsMenu:
                    SettingsMenu.SetActive(false);
                    MainMenu.SetActive(true);
                    _mainMenuState = MainMenuState.MainMenu;
                    break;
                case MainMenuState.Credits:
                    CreditsPage.SetActive(false);
                    MainMenu.SetActive(true);
                    _mainMenuState = MainMenuState.MainMenu;
                    break;
            }
        }
        
        public void RegionSelected()
        {
            ServerSelection.SetActive(false);
            LoadingPage.SetActive(false);
            MainMenu.SetActive(true);
            _mainMenuState = MainMenuState.MainMenu;
        }

        public void GoToCreateARoom()
        {
            MainMenu.SetActive(false);
            CreateARoomMenu.SetActive(true);
            _mainMenuState = MainMenuState.CreateARoom;
        }
        
        public void GoToJoinARoom()
        {
            MainMenu.SetActive(false);
            JoinARoomMenu.SetActive(true);
            _mainMenuState = MainMenuState.JoinARoom;
        }
        
        public void GoToSettings()
        {
            MainMenu.SetActive(false);
            SettingsMenu.SetActive(true);
            _mainMenuState = MainMenuState.SettingsMenu;
        }
        
        public void GoToCredits()
        {
            MainMenu.SetActive(false);
            CreditsPage.SetActive(true);
            _mainMenuState = MainMenuState.Credits;
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void CreateRoom()
        {
            string roomName = RoomNameInputField.text;
            if (string.IsNullOrEmpty(roomName)) return;
            Loading("Creating Room");
            connectToServer.CreateRoom(RoomNameInputField.text);
        }

        public void Loading(string message)
        {
            switch (_mainMenuState)
            {
                case MainMenuState.ServerSelection:
                    ServerSelection.SetActive(false);
                    break;
                case MainMenuState.MainMenu:
                    MainMenu.SetActive(false);
                    break;
                case MainMenuState.CreateARoom:
                    CreateARoomMenu.SetActive(false);
                    break;
                case MainMenuState.JoinARoom:
                    JoinARoomMenu.SetActive(false);
                    break;
                case MainMenuState.SettingsMenu:
                    SettingsMenu.SetActive(false);
                    break;
            }
            LoadingPage.SetActive(true);
            LoadingText.text = message;
            _mainMenuState = MainMenuState.Loading;
        }
    }
    
    public enum MainMenuState
    {
        ServerSelection,
        MainMenu,
        CreateARoom,
        JoinARoom,
        SettingsMenu,
        Loading,
        Credits
    }
}