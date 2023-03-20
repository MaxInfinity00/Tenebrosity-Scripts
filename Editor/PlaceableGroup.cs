using System.Collections.Generic;
using Team11.Interactions;
using UnityEditor;
using UnityEngine;

namespace Team11.Editors
{
    public class PlaceableGroup
    {
        public readonly Dictionary<Transform, List<PlaceableObject>> Groups;
        public readonly List<PlaceableObject> NoParentObjects;
        public readonly List<bool> ShowList;

        public PlaceableGroup()
        {
            NoParentObjects = new List<PlaceableObject>();
            Groups = new Dictionary<Transform, List<PlaceableObject>>();
            ShowList = new List<bool>();
        }

        public void AddObject(PlaceableObject obj)
        {
            if(!PrefabUtility.IsOutermostPrefabInstanceRoot(obj.gameObject)) return;
            if (obj.transform.parent == null)
            {
                NoParentObjects.Add(obj);
                return;
            }

            var parent = PrefabUtility.GetOutermostPrefabInstanceRoot(obj).transform.parent;
            if (parent == null)
                parent = obj.transform.root;
            if (Groups.ContainsKey(parent))
            {
                Groups[parent].Add(obj);
            }
            else
            {
                Groups.Add(parent, new List<PlaceableObject>());
                ShowList.Add(true);
                Groups[parent].Add(obj);
            }
        }

        public void RemoveObject(PlaceableObject obj)
        {
            if (NoParentObjects.Contains(obj))
            {
                NoParentObjects.Remove(obj);
                return;
            }

            foreach (var parent in Groups.Keys)
            {
                if (!Groups[parent].Contains(obj)) continue;
                Groups[parent].Remove(obj);
                return;
            }
        }
    }
}