using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Landscaper.Players;
using Landscaper.Tiles;
using Landscaper.Core;
using Landscaper.UI;
using System;
using CustomAudio;

namespace Landscaper.Pieces
{
	public class PieceActions : MonoBehaviour
	{
		// Public Members
		public Piece Piece;

		public float AnimationDuration = 0.25f;

		public bool InAction = false;


		// Private Members
		private float counter = 0.0f;

		public Animator Animator { get { return Piece.Animator; } }

		// Piece Actions
		// 1) Move
		// 2) Attack
		// 3) Upgrade

		// 4) Moves List Display
		// 5) Moves List Highlight


		private void Awake() => Piece = GetComponent<Piece>();

		#region Public Methods

		public void MovePiece(Tile targetTile, WorldState worldState) => StartCoroutine(CoMovePiece(targetTile, worldState));

		public void TakePiece(Tile targetTile, WorldState worldState)
		{
			StartCoroutine(CoDestroyPiece(targetTile, worldState));
		}

		#endregion

		#region Private Methods

		void OccupyNewArea(Piece movingPiece, Tile targetTile, WorldState worldState)
		{
			var pieceDictionary = worldState.PieceDictionary;
			var coordinatesRequired = movingPiece.CoordinatesRequired;


			// Remove ourself from the current tile dictionary
			foreach (Vector2Int coordinateRequired in coordinatesRequired)
				pieceDictionary.Remove(movingPiece.ParentTile.TileCoordinates + coordinateRequired);

			// Change our parent to the new tile
			movingPiece.transform.SetParent(targetTile.ChildAnchor, true);

			// Add ourself back to the tile dictionary 
			foreach (Vector2Int coordinateRequired in coordinatesRequired)
				pieceDictionary.Add(movingPiece.ParentTile.TileCoordinates + coordinateRequired, movingPiece);
		}

		void PlayThudSound()
		{
			AudioManager.PlayClip(AudioClipName.Thud);
		}

		void StartMovingAnimation()
		{
			if (Piece.Animator)
			{
				Piece.Animator.SetBool("Moving", true);

				// By doing this we make the animation slow down when moving further..
				Piece.Animator.speed = 1 / AnimationDuration;
			}
		}

		void StopMovingAnimation()
		{
			if (Piece.Animator)
			{
				// Reset the speed of the animator
				if (Piece.Animator) Piece.Animator.speed = 1;

				if (Piece.Animator) Piece.Animator.SetBool("Moving", false);
				if (Piece.Animator) Piece.Animator.SetBool("Selected", false);
			}
		}

		void ResetGameState()
		{
			var LandscaperBoard = FindObjectOfType<LandscaperBoard>();

			// UnClick the parent tile at the new location
			Piece.ParentTile.OnPointerUnClick(LandscaperBoard.WorldState);
		}

		#endregion

		#region Coroutines

		IEnumerator CoDestroyPiece(Tile target, WorldState worldState)
		{
			// Remove the target from the world
			Piece targetPiece = worldState.PieceDictionary[target.TileCoordinates];

			// Remove the target from the world
			foreach (Vector2Int coordinateRequired in targetPiece.CoordinatesRequired)
			{
				Vector2Int coordinates = targetPiece.ParentTile.TileCoordinates + coordinateRequired;
				worldState.PieceDictionary.Remove(coordinates);
			}

			Destroy(targetPiece.gameObject);

			// Reset the game state 
			ResetGameState();

			yield return CoMovePiece(target, worldState);
		}

		IEnumerator CoMovePiece(Tile target, WorldState worldState)
		{
			// If this piece is currently in an action we want to wait for it to end
			// When it does end we immediately tell this piece that we are n a new action
			while (InAction) yield return null;
			InAction = true;


			// Get the starting and ending positions for this piece
			Vector3 oldPos = transform.position, newPos = target.transform.position;


			// Tell the animator that we are going to start moving
			StartMovingAnimation();


			// Occupy new tile (or set of tiles)
			// This clears where we currently are from the worldState
			// and occupies a new set of coordinators
			OccupyNewArea(Piece, target, worldState);


			// ANIMATION
			//
			// Next we want to play the lerping animation
			// I unity the animation and PieceActions.AnimationDuration should match up
			counter = 0;
			while (counter < AnimationDuration)
			{
				// Increment the timer
				counter += Time.deltaTime;

				// Lerp to the final location
				transform.position = Vector3.Lerp(oldPos, newPos, counter / AnimationDuration);
				yield return null;
			}


			// PLay the Think sound when we get to our destination
			PlayThudSound();


			// Reset the game state 
			ResetGameState();


			// Stop and Moving animation and reset the animators speed
			StopMovingAnimation();
			InAction = false;
		}

		#endregion
	}
}