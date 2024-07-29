using System;
using PlayerSystem.TeleportSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace InputSystem
{
    public class InputListener : MonoBehaviour
    {
         private PlayerInput _inputSystem;
         [Inject] private TeleportPlayer _teleportPlayer;

         private void OnEnable()
         {
             TeleportPlayer.LoseAction += DisabeInput;
         }
         

         private void Awake()
         {
             _inputSystem = new PlayerInput();
             _inputSystem.Enable();
             _inputSystem.Player.Move.performed +=ReadMove;
             _inputSystem.Player.Jump.performed += Jump;
         }

         private void OnDisable()
         {
             _inputSystem.Player.Move.performed -= ReadMove;
             _inputSystem.Player.Jump.performed -= Jump;
             TeleportPlayer.LoseAction -= DisabeInput;
         }



         void Jump(InputAction.CallbackContext context)
         {
             _teleportPlayer.Jump();
         }
        
    async void ReadMove(InputAction.CallbackContext context)
    {
        await _teleportPlayer.Teleport(context.ReadValue<Vector2>());
    }

    void DisabeInput()
    {
        _inputSystem.Disable();
    }
        
    }

}
