using Cinemachine;
using Team11.Menus;
using Team11.Players;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Team11.Editor
{
    public class ToolsWindow : EditorWindow
    {
        [MenuItem("Tools/Tools Window")]
        private static void ShowWindow()
        {
            var window = GetWindow<ToolsWindow>();
            window.titleContent = new GUIContent("Tools");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Setup Scene"))
            {
                SetupScene();
            }
        }

        private void SetupScene()
        {
            var gameUi = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Game UI.prefab");
            var mainCam = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MainCamera.prefab");
            var cineCam = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlayerFollowCamera.prefab");
            var player1 = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Litomancer.prefab");
            var player2 = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Polarizer.prefab");
            var offlineTest = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Offline Tester.prefab");
            
            GameObject gameUiGo = PrefabUtility.InstantiatePrefab(gameUi) as GameObject; 
            PrefabUtility.InstantiatePrefab(mainCam);
            GameObject cineCamGo = PrefabUtility.InstantiatePrefab(cineCam) as GameObject;
            GameObject player1Go = PrefabUtility.InstantiatePrefab(player1) as GameObject;
            GameObject player2Go = PrefabUtility.InstantiatePrefab(player2) as GameObject;
            PrefabUtility.InstantiatePrefab(offlineTest);
            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();

            Setup setup = gameUiGo.GetComponentInChildren<Setup>();
            // setup.GetAllPickups();
            setup.P1 = player1Go.GetComponent<PlayerBase>();
            setup.P2 = player2Go.GetComponent<PlayerBase>();
            setup.SetCamera(cineCamGo.GetComponent<CinemachineVirtualCamera>());
        }
    }
}