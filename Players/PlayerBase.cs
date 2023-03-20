using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Photon.Pun;
using Team11.DebugConsole;
using Team11.Health;
using Team11.Menus;

namespace Team11.Players
{
    public class PlayerBase : MonoBehaviourPunCallbacks
    {
        [SerializeField] private List<Behaviour> ToEnable;
        [FormerlySerializedAs("Controller")] [SerializeField] private CharacterController controller;
        [FormerlySerializedAs("PlayerCameraRoot")] [SerializeField] private Transform playerCameraRoot;
        [SerializeField] private GameObject playerModel;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerHealth health;

        public void Select(PlayerSettings playerSettings)
        {
            foreach (var behaviour in ToEnable)
            {
                behaviour.enabled = true;
            }

            controller.enabled = true;

            playerSettings.vcam.Follow = playerCameraRoot;
            playerSettings.vignette.SetPlayerHealth(health);
            playerSettings.escapemenu.SetPlayerInput(playerInput);
            playerSettings.escapemenu.MessageText.gameObject.SetActive(false);

            photonView.RequestOwnership();

            playerModel.layer = LayerMask.NameToLayer("Invisible");

            //Cheats
            DebugCommand revive = new DebugCommand("revive", "Revive the player", "revive", () => { health.Revive(); });
            DebugController.instance.commandList.Add(revive);
            DebugCommand<bool> invincible = new DebugCommand<bool>("invincible", "Make the player invincible",
                "invincible <true/false>", (bool value) =>
                {
                    Debug.Log(value);
                    health.Invincibility(value);
                }
            );
            DebugController.instance.commandList.Add(invincible);
        }
    }
}