using System;
using PlayerSystem.TeleportSystem;
using UnityEngine;

namespace PlayerSystem.InputSystem
{
    public class InputListener : MonoBehaviour
    {
         private PlayerInput _inputSystem;
         [SerializeField] private TeleportPlayer _teleportPlayer;

         private void Awake()
         {
             _inputSystem = new PlayerInput();
             _inputSystem.Enable();
         }

    
        

         void Update()
        {
            ReadMove();
        }

        void ReadMove()
        {
            /*var value = _inputSystem.Player.Move.ReadValue<Vector2>();
            if (value != new Vector2(0, 0))
            {
                Debug.Log(value);
                _teleportPlayer.Teleport(value);
                
            }*/

            if (Input.GetKeyDown(KeyCode.A))
            {
                _teleportPlayer.Teleport(new Vector2(-1,0));
                Debug.Log("Pressed A");
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _teleportPlayer.Teleport(new Vector2(1,0));
                Debug.Log("Pressed D");
            }
        }
    }
}
