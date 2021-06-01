using Landscaper.Chunks;
using Landscaper.Core;
using Landscaper.Pieces;
using Landscaper.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	public static List<EnemyBehaviour> NewEnemies = new List<EnemyBehaviour>();

	public LandscaperBoard LandscaperBoard;
	public Piece Piece;

	public int Range = 3;

	private void Awake()
	{
		LandscaperBoard = FindObjectOfType<LandscaperBoard>();
	}

	public virtual IEnumerator Act()
	{
		var tiles = LandscaperBoard.GetSurroundingTiles(Piece.ParentTile.TileCoordinates, Range);

		// Loop through all the tiles in this pieces range
		foreach (Tile tile in tiles)
		{
			//If the tile has a piece on it AND it is the players piece
			if (LandscaperBoard.PieceDictionary.ContainsKey(tile.TileCoordinates)) {
				if (LandscaperBoard.PieceDictionary[tile.TileCoordinates].OwnerID == 0)
				{
					var attackOptions = GetAttackOptions(Piece);

					if (attackOptions.Count > 0)
					{
						LandscaperBoard.CapturePiece(null, Piece, attackOptions[0]);

						yield return new WaitForSeconds(0.2f);
					}
					else
					{
						// Get the move options
						var moveOptions = GetMoveOptions(Piece);

						var targetPiece = LandscaperBoard.PieceDictionary[tile.TileCoordinates];
						var targetTile = GetClosestTileToTarget(targetPiece, moveOptions);

						// If we found a good tile to move to
						if (targetTile)
						{
							Chunk targetChunk = targetTile.GetComponentInParent<Chunk>();
							Chunk currentChunk = Piece.GetComponentInParent<Chunk>();

							if (Mathf.Abs(targetChunk.Height - currentChunk.Height) > Piece.JumpRange)
							{
								if (Piece.Abilities.CanMoveChunk == true)
								{
									currentChunk.SetHeight(currentChunk.Height + (currentChunk.Height > targetChunk.Height ? -1 : 1));

									yield return new WaitForSeconds(0.2f);
								}
								else
								{
									// LandscaperBoard.MovePiece(null, enemy.Piece, targetTile);
								}
							}
							else
							{
								LandscaperBoard.MovePiece(null, Piece, targetTile);

								yield return new WaitForSeconds(0.2f);
							}
						}
					}
				}
			}
			else
			{
				// Wait for target and do nothing
			}
		}

	}

	#region Utility Methods

	List<Tile> GetMoveOptions(Piece piece)
	{
		List<Tile> tiles = new List<Tile>();

		// Loop through all of the given pieces moves
		foreach (MoveParameters move in piece.Moves)
		{
			for (int step = 0; step < move.Distance; step++)
			{
				// Calculate the next position to check
				Vector2Int coordinates = piece.ParentTile.TileCoordinates + (move.Direction * (step + 1));

				// If that tile is real
				if (LandscaperBoard.TileDictionary.ContainsKey(coordinates)) 		 {
					if (LandscaperBoard.PieceDictionary.ContainsKey(coordinates))
					{
						if (LandscaperBoard.PieceDictionary[coordinates].OwnerID == piece.OwnerID)
						{
							// This is one of us
						}
						else
						{
							tiles.Add(LandscaperBoard.TileDictionary[coordinates]);
						}
					}
					else
					{
						tiles.Add(LandscaperBoard.TileDictionary[coordinates]);
					}
				}
			}
		}

		return tiles;
	}
	
	List<Tile> GetAttackOptions(Piece piece)
	{
		List<Tile> tiles = new List<Tile>();

		// Loop through all of the given pieces moves
		foreach (MoveParameters move in piece.Moves) {
			for (int step = 0; step < move.Distance; step++)
			{
				// Calculate the next position to check
				Vector2Int coordinates = piece.ParentTile.TileCoordinates + (move.Direction * (step + 1));

				// If that tile is real
				if (LandscaperBoard.TileDictionary.ContainsKey(coordinates)) {
					if (LandscaperBoard.PieceDictionary.ContainsKey(coordinates))
					{
						if (LandscaperBoard.PieceDictionary[coordinates].OwnerID == piece.OwnerID)
						{
							// This is one of us
						}
						else
						{
							tiles.Add(LandscaperBoard.TileDictionary[coordinates]);
						}
					}
				}
			}
		}

		return tiles;
	}

	Tile GetClosestTileToTarget(Piece enemy, List<Tile> tiles)
	{
		Tile targetTile = null;

		foreach (Tile tile in tiles)
		{
			if (targetTile == null) targetTile = tile;

			float distToTarget = Vector2.Distance(tile.TileCoordinates, enemy.ParentTile.TileCoordinates);
			float distToBeat = Vector2.Distance(targetTile.TileCoordinates, enemy.ParentTile.TileCoordinates);

			if (distToTarget < distToBeat)
			{
				targetTile = tile;
			}
		}

		return targetTile;
	}

	#endregion
}
