using System;
using UnityEngine.InputSystem;


namespace Input
{
    public class InputService
    {
        private Keyboard keyboard;
        public event Action<char> OnTextInput;
        private InputActions inputActions;
        public event Action OnBackspacePressed;
        public event Action OnBackSpaceHold;
        public event Action OnBackspaceReleased;
        public event Action OnEnterPressed;
        

        public InputService()
        {
            inputActions = new InputActions();
            inputActions.Typewriter.Enable();
            inputActions.Typewriter.BackspacePressed.performed += BackSpacePressed;
            inputActions.Typewriter.BackspaceHold.performed += BackSpaceHold;
            inputActions.Typewriter.BackspaceHold.canceled += BackSpaceReleased;
            inputActions.Typewriter.EnterPressed.performed += EnterPressed;
        }


        public void EnableReadingTextInput()
        {
            keyboard = Keyboard.current;
            keyboard.onTextInput += TextInput;
        }


        private void BackSpacePressed(InputAction.CallbackContext callbackContext)
        {
            OnBackspacePressed?.Invoke();
        }


        private void BackSpaceHold(InputAction.CallbackContext callbackContext)
        {
            OnBackSpaceHold?.Invoke();
        }
        
        private void BackSpaceReleased(InputAction.CallbackContext callbackContext)
        {
            OnBackspaceReleased?.Invoke();
        }


        private void EnterPressed(InputAction.CallbackContext callbackContext)
        {
            OnEnterPressed?.Invoke();       
        }
        

        private void TextInput(char character)
        {
            OnTextInput?.Invoke(character);
        }

        
        public void DisableReadingTextInput()
        {
            keyboard.onTextInput -= TextInput;
        }
    }
}
