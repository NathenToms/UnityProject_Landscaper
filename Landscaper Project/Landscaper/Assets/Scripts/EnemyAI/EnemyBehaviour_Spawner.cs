using Landscaper.Pieces;
using Landscaper.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour_Spawner : EnemyBehaviour
{
	int spawnCounter;
	public override IEnumerator Act()
	{
		yield return new WaitForSeconds(0.2f);
		spawnCounter++;

		if (spawnCounter > 3)
		{
			var tiles = GetSurroundingTiles(Piece);

			if (tiles.Count > 0)
			{
				foreach (Tile tile in tiles)
				{
					if (LandscaperBoard.PieceDictionary.ContainsKey(tile.TileCoordinates) == false)
					{
						LandscaperBoard.PlacePiece(null, PieceType.Enemy, tile.TileCoordinates, 1, out Piece piece);

						NewEnemies.Add(piece.GetComponent<EnemyBehaviour>());

						spawnCounter = 0;

						break;
					}
				}
			}
		}
	}

	List<Tile> GetSurroundingTiles(Piece piece)
	{
		return LandscaperBoard.GetSurroundingTiles(piece.ParentTile.TileCoordinates, 2);
	}
}
