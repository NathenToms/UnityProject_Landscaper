using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landscaper.Pieces
{
	[System.Serializable]
	public class MoveParameters
	{
		public Vector2Int Direction;

		// How Far does this move go
		public float Distance = 1;

		// Does this move need to previous move in the chain to work
		public bool AbsoluteMove;
	}
}