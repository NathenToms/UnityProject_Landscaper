using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Landscaper.UI
{
	public class NotificationDisplay : MonoBehaviour
	{
		public static NotificationDisplay Instance;

		[SerializeField] private TextMeshProUGUI _notificationTitleText = null;
		[SerializeField] private TextMeshProUGUI _notificationSubText = null;
		[SerializeField] private Transform _notificationBoxAnchor = null;

		public bool InAnimation = false;

		private bool waitingForClick = false;
		private bool wasClicked = false;

		private void Awake()
		{
			if (Instance)
			{
				Debug.LogError("There can't be 2 'Notification Displays' in the scene!", this);
				Destroy(gameObject);
			}
			else
			{
				Instance = this;
			}

			DontDestroyOnLoad(gameObject);
		}

		private void Update()
		{
			if (waitingForClick)
			{
				if (Input.GetKeyDown(KeyCode.Mouse0))
				{
					waitingForClick = false;
					wasClicked = true;
				}
			}
		}

		public static void ShowMessage(string messageTitle, string messagesubText)
		{
			if (Instance == null) { Debug.Log("There is no 'NotificationDisplay' in the scene!"); }
			else
			{
				Instance.ShowNewMessage(messageTitle, messagesubText);
			}
		}

		public void ShowNewMessage(string messageTitle, string messagesubText) => StartCoroutine(CoShowMessage(messageTitle, messagesubText));
		IEnumerator CoShowMessage(string messageTitle, string messagesubText)
		{
			while (InAnimation) yield return null;
			InAnimation = true;

			bool wait = true;

			_notificationTitleText.text = messageTitle;
			_notificationSubText.text = messagesubText;

			Tween.Instance.EaseOut_Transform_QuartY(_notificationBoxAnchor, Screen.height, 0, 0.5f, 0, () => { wait = false; });

			while (wait) yield return null;



			yield return new WaitForSecondsRealtime(0.5f);

			wait = true;
			Tween.Instance.EaseIn_Transform_QuartY(_notificationBoxAnchor, 0, -Screen.height , 0.5f, 0, () => { wait = false; });
			while (wait) yield return null;

			InAnimation = false;
		}
	}
}