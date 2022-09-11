using UnityEngine.InputSystem;
using Game.Services;
using UnityEngine;
using System;

namespace Game.Input
{
    public sealed class InputService : MonoBehaviour, IInputService
    {
        public event Action<PlayerInputsData> OnReadPlayerInputs;

        [Header("Input System")] 
        private PlayerInputs _playerInputs;
        private PlayerInputs.LandMapActions _landActions;
        private PlayerInputsData _playerInputsData;

        [Header("Inputs")] 
        private bool _executeLightAttack;
        private bool _executeHeavyAttack;
        private bool _executeDefend;
        private bool _executeDodge;

        private void Awake()
        {
            ServiceLocator.RegisterService<IInputService>(this);

            DontDestroyOnLoad(gameObject);

            InitializePlayerInputs();

            EnablePlayerInputs();

            SubscribeEvents();
        }

        private void OnDestroy()
        {
            ServiceLocator.DeregisterService<IInputService>();

            DisablePlayerInputs();

            UnsubscribeEvents();
        }

        private void Update()
        {
            UpdatePlayerInputsData();

            OnReadPlayerInputs?.Invoke(_playerInputsData);

            ResetInputs();
        }

        private void SubscribeEvents()
        {
            _landActions.LightAttack.performed += HandleLightAttack;
            _landActions.HeavyAttack.performed += HandleHeavyAttack;
            _landActions.Defend.performed += HandleDefend;
            _landActions.Dodge.performed += HandleDodge;
        }

        private void UnsubscribeEvents()
        {
            _landActions.LightAttack.performed -= HandleLightAttack;
            _landActions.HeavyAttack.performed -= HandleHeavyAttack;
            _landActions.Defend.performed -= HandleDefend;
            _landActions.Dodge.performed -= HandleDodge;
        }

        private void InitializePlayerInputs()
        {
            _playerInputs = new PlayerInputs();

            _landActions = _playerInputs.LandMap;
        }

        private void EnablePlayerInputs()
        {
            _playerInputs.Enable();
        }

        private void DisablePlayerInputs()
        {
            _playerInputs.Disable();
        }

        private void ResetInputs()
        {
            _executeLightAttack = false;
            _executeHeavyAttack = false;
            _executeDefend = false;
            _executeDodge = false;
        }

        private void UpdatePlayerInputsData()
        {
            _playerInputsData.ExecuteLightAttack = _executeLightAttack;
            _playerInputsData.ExecuteHeavyAttack = _executeHeavyAttack;
            _playerInputsData.ExecuteDefend = _executeDefend;
            _playerInputsData.ExecuteDodge = _executeDodge;
        }

        private void HandleLightAttack(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                {
                    _executeLightAttack = true;
                    break;
                }
                case InputActionPhase.Canceled:
                {
                    _executeLightAttack = false;
                    break;
                }
            }
        }
        
        private void HandleHeavyAttack(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                {
                    _executeHeavyAttack = true;
                    break;
                }
                case InputActionPhase.Canceled:
                {
                    _executeHeavyAttack = false;
                    break;
                }
            }
        }
        
        private void HandleDefend(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                {
                    _executeDefend = true;
                    break;
                }
                case InputActionPhase.Canceled:
                {
                    _executeDefend = false;
                    break;
                }
            }
        }
        
        private void HandleDodge(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                {
                    _executeDodge = true;
                    break;
                }
                case InputActionPhase.Canceled:
                {
                    _executeDodge = false;
                    break;
                }
            }
        }
    }
}
