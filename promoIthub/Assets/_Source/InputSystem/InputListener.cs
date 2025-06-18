using System;
using PlayerSystem.TeleportSystem;
using SoundSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace InputSystem
{
    public class InputListener : MonoBehaviour
    {
        private PlayerInput _inputSystem;
        [Inject] private TeleportPlayer _teleportPlayer;
        [Inject] private Animator _animator;

        private void OnEnable()
        {
            TeleportPlayer.LoseAction += DisabeInput;
        }


        private void Awake()
        {
            _inputSystem = new PlayerInput();
            _inputSystem.Player.Move.performed += ReadMove;
            _inputSystem.Player.MobileMove.performed += HandleTapScreen;

        }

        private void OnDisable()
        {
            _inputSystem.Player.Move.performed -= ReadMove;
            TeleportPlayer.LoseAction -= DisabeInput;
        }



        void Jump(InputAction.CallbackContext context)
        {
            _teleportPlayer.Jump();
        }

        async void ReadMove(InputAction.CallbackContext context)
        {
            SoundManager.Instance.Play("Jump");
            await _teleportPlayer.Teleport(context.ReadValue<Vector2>());
        }

        void DisabeInput()
        {
            _inputSystem.Disable();
            _animator.SetTrigger("Fall");
        }

        public void EnableInput()
        {
            _inputSystem.Enable();

        }

        private async void HandleTapScreen(InputAction.CallbackContext context)
        {
           
            Vector2 screenPosition = Pointer.current.position.ReadValue();

            if (screenPosition.x < Screen.width / 2f)
                await _teleportPlayer.Teleport(Vector2.left);
            else
                await _teleportPlayer.Teleport(Vector2.right);

            SoundManager.Instance.Play("Jump");
        }
    }

}
