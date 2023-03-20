using System.Collections.Generic;
using Team11.Interactions;
using UnityEngine;

namespace Team11.Editors
{
    public class LevelWindowData : ScriptableObject
    {
        public List<PlaceableObject> prefabs;
        public int selectedPrefabIndex;
        public bool orientToNormal;
    }
}