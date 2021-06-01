using Landscaper.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landscaper.Players
{
	public class PlayerResourceManager : MonoBehaviour
	{
		public int MovePoints = 0;
		public int MaxMovePoints = 0;

		private void Awake()
		{
			MovePoints = MaxMovePoints;
		}

		public void ResetPoints()
		{
			MovePoints = MaxMovePoints;
		}
	}
}