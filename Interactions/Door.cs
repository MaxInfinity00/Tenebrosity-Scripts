using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Team11.Interactions
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private TorchContainer[] connectedContainers;
        [SerializeField] private PressurePlate[] connectedPressurePlates;
        [SerializeField] private CombinationLock[] connectedCombinationLocks;
        [SerializeField] private KeyCollection[] connectedKeyCollections;
        [SerializeField] private bool oneTimeOpen;
        public UnityEvent OnOpen;
        public UnityEvent OnClose;
        public bool openState;
        public int openRequirement;
        [ReadOnly] public int currentCount;
        public Animator doorAnimator;

        private bool _openedForTheFirstTime;

        private void Start()
        {
            foreach (var container in connectedContainers)
            {
                container.OnTorchPlaced.AddListener(ConditionProgress);
                container.OnTorchRemoved.AddListener(ConditionRegress);
            }

            foreach (var pressurePlate in connectedPressurePlates)
            {
                pressurePlate.OnPress.AddListener(ConditionProgress);
                pressurePlate.OnRelease.AddListener(ConditionRegress);
            }

            foreach (var combinationLock in connectedCombinationLocks)
            {
                combinationLock.OnUnlock.AddListener(ConditionProgress);
            }
            
            foreach (var keyCollection in connectedKeyCollections)
            {
                keyCollection.OnUnlock.AddListener(ConditionProgress);
            }

            if (openState)
            {
                OpenDoor();
            }
        }

        public void ConditionProgress()
        {
            currentCount++;
            if (currentCount >= openRequirement && !openState)
            {
                OpenDoor();
            }
        }

        public void ConditionRegress()
        {
            currentCount--;
            if (currentCount < openRequirement)
            {
                CloseDoor();
            }
        }

        public void OpenDoor()
        {
            _openedForTheFirstTime = true;
            openState = true;
            // Play animation
            doorAnimator.SetBool("open", openState);
            OnOpen?.Invoke();
        }

        public void CloseDoor()
        {
            if (_openedForTheFirstTime && oneTimeOpen) return;
            openState = false;
            //PlayAnimation
            doorAnimator.SetBool("open", openState);
            OnClose?.Invoke();
        }
    }
}