using UnityEngine;
using UnityEngine.InputSystem;

namespace Project
{
    public class GameController : MonoBehaviour
    {
        //  Inspector
        [Header("Dependencies")]
        [SerializeField] private Character _character;

        //  Methods
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void OnMove(InputValue inputValue) => _character.OnMove(inputValue);
        public void OnLook(InputValue inputValue) => _character.OnLook(inputValue);
        public void OnJump(InputValue inputValue) => _character.OnJump(inputValue);
    }
}
