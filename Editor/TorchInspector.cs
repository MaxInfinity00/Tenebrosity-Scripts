using Team11.Interactions;
using UnityEditor;
using UnityEngine;

namespace Team11.Editor
{
    [CustomEditor(typeof(Torch))]
    public class TorchInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var t = target as Torch;
            base.OnInspectorGUI();
            if (GUILayout.Button("Update Range"))
            {
                t.UpdateRange();
            }
        }
    }
}