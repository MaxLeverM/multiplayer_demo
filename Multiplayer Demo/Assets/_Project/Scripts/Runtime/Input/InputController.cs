using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
	public class InputController : MonoBehaviour
	{
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		
		private IControllable _controllerTarget;

		public void SetTarget(IControllable target)
		{
			_controllerTarget = target;
		}
		
		public void OnMove(InputValue value) => _controllerTarget?.Move(value.Get<Vector2>());
		public void OnLook(InputValue value) => _controllerTarget?.Look(value.Get<Vector2>());
		public void OnJump(InputValue value) => _controllerTarget?.Jump(value.isPressed);
		public void OnSprint(InputValue value) => _controllerTarget?.Sprint(value.isPressed);

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}