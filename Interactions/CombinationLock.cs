using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Team11.Interactions
{
    [RequireComponent(typeof(PhotonView))]
    public class CombinationLock : MonoBehaviourPunCallbacks
    {
        [SerializeField] private List<Lever> levers;
        public UnityEvent OnUnlock;
        
        private List<Lever> _currentCombination = new();

        private void Start()
        {
            foreach (var lever in levers)
            {
                lever.OnPress.AddListener(() => UpdateCombination(lever));
            }
        }

        private void UpdateCombination(Lever lever)
        {
            AddLever(lever);

            if(_currentCombination.Count != levers.Count) return;
            if (CheckCombination())
            {
                Unlocked();
            }
            else
            {
                RevertLevers();
            }
        }
        
        private void AddLever(Lever lever)
        {
            _currentCombination.Add(lever);
        }

        private void Unlocked()
        {
            OnUnlock?.Invoke();
            foreach (var l in levers)
            {
                l.OnPress.RemoveAllListeners();
            }
        }

        private void RevertLevers()
        {
            _currentCombination.Clear();
            foreach (var lever in levers)
            {
                lever.Revert();
            }
        }

        private bool CheckCombination()
        {
            for (int i = 0; i < _currentCombination.Count; i++)
            {
                if (_currentCombination[i] != levers[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}