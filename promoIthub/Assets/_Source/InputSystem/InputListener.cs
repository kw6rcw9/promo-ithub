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
         }

         private void OnDisable()
         {
             _inputSystem.Player.Move.performed -= ReadMove;
             TeleportPlayer.LoseAction -= DisabeInput;
         }



       
        
    async void ReadMove(InputAction.CallbackContext context)
    {
        print(context.ReadValue<Vector2>());
        await _teleportPlayer.Teleport(context.ReadValue<Vector2>());
    }

    void DisabeInput()
    {
        _inputSystem.Disable();
    }
        
    }

}
