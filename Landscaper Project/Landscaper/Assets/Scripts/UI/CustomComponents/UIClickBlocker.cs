using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Landscaper.UI.CustomComponents
{
	public class UIClickBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public static Transform HoveredTarget;

		public void OnPointerEnter(PointerEventData eventData)
		{
			HoveredTarget = transform;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (HoveredTarget == transform) {
				HoveredTarget = null;
			}
		}

		private void OnDisable()
		{
			if (HoveredTarget == transform) {
				HoveredTarget = null;
			}
		}
	}
}