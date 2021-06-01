using Landscaper.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Landscaper.UI
{
	public class TileDisplay : MonoBehaviour
	{
		[Header("Component References")]
		[SerializeReference] private TextMeshProUGUI _tileCoordinatesText = null;
		[SerializeReference] private TextMeshProUGUI _tileStateText = null;

		private Tile Tile;

		private void FixedUpdate()
		{
			if (Tile) Init(Tile);
		}

		public TileDisplay Init(Tile tile)
		{
			Tile = tile;

			_tileCoordinatesText.text = tile.TileCoordinates.ToString();
			_tileStateText.text = tile.TileType.ToString();

			//_tileStateText.text = tile.Selected.ToString();

			return this;
		}
	}
}