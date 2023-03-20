using System.Collections.Generic;
using System.Linq;
using Team11.Interactions;
using UnityEditor;
using UnityEngine;

namespace Team11.Editors
{
    public class LevelWindow : EditorWindow
    {
        private LevelWindowData _windowData;
    
        private SerializedObject _so;
        private SerializedProperty _propPrefabs;
        private SerializedProperty _propOrientToNormal;

        private PlaceableGroup _placeableGroup;
        private Transform _currentParent;
        private Vector2 _scrollPos;
        private bool _canPlacePrefab;
    
        [MenuItem("Tools/Leveler")]
        private static void CreateWindow() => 
            GetWindow<LevelWindow>("Leveler").Show();

        private void OnEnable()
        {
            if (_windowData == null)
            {
                _windowData = AssetDatabase.LoadAssetAtPath<LevelWindowData>("Assets/WindowData.asset");
                if(_windowData != null)
                {
                    InitSo();
                    SceneView.duringSceneGui += DuringSceneGUI;
                    return;
                }

                _windowData = CreateInstance<LevelWindowData>();
                AssetDatabase.CreateAsset(_windowData, "Assets/WindowData.asset");
                AssetDatabase.Refresh();
            }

            InitSo();
            SceneView.duringSceneGui += DuringSceneGUI;
        }

        private void OnDisable() => SceneView.duringSceneGui -= DuringSceneGUI;

        private void InitSo()
        {
            _so = new SerializedObject(_windowData);
            _propPrefabs = _so.FindProperty(nameof(_windowData.prefabs));
            _propOrientToNormal = _so.FindProperty(nameof(_windowData.orientToNormal));
        
            InitGroups();
        }

        private void InitGroups()
        {
            var objs = FindObjectsOfType<PlaceableObject>().ToList();
            _placeableGroup = new PlaceableGroup();
            foreach (var placeableObject in objs)
                _placeableGroup.AddObject(placeableObject);
        }

        private void DuringSceneGUI(SceneView scene)
        {
            var camTf = scene.camera;
        
            if(Event.current.type == EventType.MouseMove)
                scene.Repaint();
        
            //var ray = new Ray(camTf.transform.position, camTf.transform.forward);
            var mousePos = new Vector3(Event.current.mousePosition.x, Screen.height - Event.current.mousePosition.y);   
            var ray = camTf.ScreenPointToRay(mousePos);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, 25))
            {
                Handles.color = Color.black;
                Handles.DrawSolidDisc(hitInfo.point, hitInfo.normal, .2f);
            
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C)
                {
                    Event.current.Use();
                    _canPlacePrefab = true;
                }
            
                if (_canPlacePrefab)
                {
                    _canPlacePrefab = false;
                    PlacePrefab(hitInfo);
                }
            }
        }

        private void PlacePrefab(RaycastHit hit)
        {
            if(_windowData.prefabs == null) return;
            var prefab = _windowData.prefabs[_windowData.selectedPrefabIndex];
            if(prefab == null) return;

            var go = (PlaceableObject) PrefabUtility.InstantiatePrefab(prefab);
            go.transform.position = hit.point;
            if (_windowData.orientToNormal) 
                go.transform.rotation = Quaternion.LookRotation(hit.normal);
            if(_currentParent != null)
                go.transform.SetParent(_currentParent);
            _placeableGroup.AddObject(go);
        
            Undo.RegisterCreatedObjectUndo(go.gameObject, "spawn prefab");
        }

        private void OnGUI()
        {
            _so.Update();
        
            EditorGUILayout.PropertyField(_propPrefabs);
            EditorGUILayout.PropertyField(_propOrientToNormal);
            _currentParent = (Transform) EditorGUILayout.ObjectField("Parent", _currentParent, typeof(Transform), true);
            EditorGUILayout.LabelField("Choose a Prefab from the List and Press C to Place");
            DrawPrefabSelectors();
        
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.ExpandWidth(true));
            DrawObjectsList();
            EditorGUILayout.EndScrollView();

            _so.ApplyModifiedProperties();
            Repaint();
        
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C)
            {
                Event.current.Use();
                _canPlacePrefab = true;
            }
        }

        private void DrawPrefabSelectors()
        {
            EditorGUILayout.Space();
            if(_windowData.prefabs == null) return;
            using (new EditorGUILayout.HorizontalScope())
            {
                foreach (var prefab in _windowData.prefabs)
                {
                    if (prefab == null) continue;
                    GUIStyle style = new(GUI.skin.GetStyle("Button"));
                    if (_windowData.selectedPrefabIndex == _windowData.prefabs.IndexOf(prefab)) 
                        style.fontStyle = FontStyle.BoldAndItalic;

                    if (GUILayout.Button(prefab.name, style))
                        _windowData.selectedPrefabIndex = _windowData.prefabs.IndexOf(prefab);
                }
            }
        }

        private void DrawObjectsList()
        {
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            DrawObjectsGroup(_placeableGroup.NoParentObjects);

            int i = 0;
            foreach (var group in _placeableGroup.Groups)
            {
                if(group.Key == null || group.Value.Count == 0) continue;
            
                EditorGUILayout.Space();
                _placeableGroup.ShowList[i] = EditorGUILayout.Foldout(_placeableGroup.ShowList[i], group.Key.gameObject.name);
                if(_placeableGroup.ShowList[i])
                    DrawObjectsGroup(group.Value);
                i++;
            }
        }

        private void DrawObjectsGroup(List<PlaceableObject> objectList)
        {
            var objToRemove = new List<PlaceableObject>();
            foreach (var obj in objectList)
            {
                if (obj == null)
                {
                    objToRemove.Add(obj);
                    continue;
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    DrawObjectDetail(obj);
                }
            }

            foreach (var obj in objToRemove)
            {
                _placeableGroup.RemoveObject(obj);
            }
        }

        private static void DrawObjectDetail(PlaceableObject obj)
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField(obj, obj.GetType());
            GUI.enabled = true;
            if (GUILayout.Button("Delete"))
            {
                Undo.DestroyObjectImmediate(obj.gameObject);
            }
            if (GUILayout.Button("Select")) Selection.activeGameObject = obj.gameObject;
            if (GUILayout.Button("Focus"))
            {
                Selection.activeGameObject = obj.gameObject;
                SceneView.FrameLastActiveSceneView();
            }
        }
    }
}