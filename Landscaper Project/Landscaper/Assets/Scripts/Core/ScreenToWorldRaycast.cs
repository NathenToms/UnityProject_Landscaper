using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landscaper.Core
{
	public class ScreenToWorldRaycast : MonoBehaviour, IRayProvider
	{
		public bool Raycast(out RaycastHit raycastHit)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out raycastHit)) {
				return true;
			}

			return false;
		}
	}
}