using PlayerSystem.TeleportSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputListener : MonoBehaviour
    {
         private PlayerInput _inputSystem;
         [SerializeField] private TeleportPlayer _teleportPlayer;

         private void Awake()
         {
             _inputSystem = new PlayerInput();
             _inputSystem.Enable();
             _inputSystem.Player.Move.performed +=ReadMove;
         }

         private void OnDisable()
         {
             _inputSystem.Player.Move.performed -= ReadMove;
         }



       
        
    void ReadMove(InputAction.CallbackContext context)
    {
        _teleportPlayer.Teleport(context.ReadValue<Vector2>());
    }
        
    }

}
