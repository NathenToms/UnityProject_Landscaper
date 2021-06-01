using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Landscaper.UI.CustomComponents
{
	public class SlidingMenu : MonoBehaviour
	{
		public Transform MenuAnchor = null;

		[Range(-1, 1)] public int DirectionX = 0;
		[Range(-1, 1)] public int DirectionY = 0;
		[Range(-1, 1)] public int DirectionZ = 0;

		[Header("KeyBinds")]
		public KeyCode OpenKey;

		[Header("Sliding Menu")]
		public float Speed = 0.5f;
		public bool Open = false;

		private bool canOpen = true;

		void Update()
		{
			if (!Input.GetKeyDown(OpenKey)) return;

			if (!Open) { OpenMenu(); } else { CloseMenu(); }
		}

		public void OpenMenu()
		{
			if (!canOpen) return;

			gameObject.SetActive(true);

			canOpen = false;

			Tween.Instance.EaseOut_Transform_QuartX(MenuAnchor, Screen.width * DirectionX, 0, Speed);
			Tween.Instance.EaseOut_Transform_QuartY(MenuAnchor, Screen.width * DirectionY, 0, Speed);
			Tween.Instance.EaseOut_Transform_QuartZ(MenuAnchor, Screen.width * DirectionZ, 0, Speed, 0, () => { canOpen = true; });

			Open = true;
		}

		public void CloseMenu()
		{
			if (!canOpen) return;

			canOpen = false;

			Tween.Instance.EaseOut_Transform_QuartX(MenuAnchor, 0, Screen.width * DirectionX, Speed);
			Tween.Instance.EaseOut_Transform_QuartY(MenuAnchor, 0, Screen.width * DirectionY, Speed);
			Tween.Instance.EaseOut_Transform_QuartZ(MenuAnchor, 0, Screen.width * DirectionZ, Speed, 0, () => {
				canOpen = true;
			});

			Open = false;
		}
	}
}