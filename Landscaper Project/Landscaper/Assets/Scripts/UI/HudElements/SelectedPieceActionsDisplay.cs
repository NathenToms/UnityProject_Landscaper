using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Landscaper.Pieces;
using Landscaper.Core;


namespace Landscaper.UI
{
	public class SelectedPieceActionsDisplay : MonoBehaviour
	{
		[SerializeField] private RectTransform _pieceActionButton_Upgrade = null;
		[SerializeField] private RectTransform _pieceActionButton_OpenShip_Tower = null;
		[SerializeField] private RectTransform _pieceActionButton_OpenShip_Factory = null;
		[SerializeField] private RectTransform _pieceActionButton_MoveChunk_Up = null;
		[SerializeField] private RectTransform _pieceActionButton_MoveChunk_Down = null;

		public GameManager GameManager;

		public Piece Target;

		public void Init(Piece piece)
		{
			TurnOffAllButtons();

			if (piece == null) return;

			Target = piece;

			//
			if (piece.Abilities.CanUpgrade)
			{
				_pieceActionButton_Upgrade.gameObject.SetActive(true);
				Tween.Instance.EaseOut_Scale_QuartY(_pieceActionButton_Upgrade, 0, 1, 0.25f);
			}

			if (piece.Abilities.CanOpenTowerShop)
			{
				_pieceActionButton_OpenShip_Tower.gameObject.SetActive(true);
				Tween.Instance.EaseOut_Scale_QuartY(_pieceActionButton_OpenShip_Tower, 0, 1, 0.25f);
			}
			if (piece.Abilities.CanOpenFactoryShop)
			{
				_pieceActionButton_OpenShip_Factory.gameObject.SetActive(true);
				Tween.Instance.EaseOut_Scale_QuartY(_pieceActionButton_OpenShip_Factory, 0, 1, 0.25f);
			}

			if (piece.Abilities.CanMoveChunk)
			{
				_pieceActionButton_MoveChunk_Up.gameObject.SetActive(true);
				Tween.Instance.EaseOut_Scale_QuartY(_pieceActionButton_MoveChunk_Up, 0, 1, 0.25f);
			}					
			if (piece.Abilities.CanMoveChunk)
			{
				_pieceActionButton_MoveChunk_Down.gameObject.SetActive(true);
				Tween.Instance.EaseOut_Scale_QuartY(_pieceActionButton_MoveChunk_Down, 0, 1, 0.25f);
			}
		}

		public void TurnOffAllButtons()
		{
			_pieceActionButton_Upgrade.gameObject.SetActive(false);
			_pieceActionButton_OpenShip_Tower.gameObject.SetActive(false);
			_pieceActionButton_OpenShip_Factory.gameObject.SetActive(false);
			_pieceActionButton_MoveChunk_Up.gameObject.SetActive(false);
			_pieceActionButton_MoveChunk_Down.gameObject.SetActive(false);
		}

		public void UpgradeUnit()
		{
			if (Target) GameManager.LandscaperBoard.UpgradePiece(FindObjectOfType<Player>(), Target);
		}

		public void ChangeChunkHeight(bool up)
		{
			if (Target) GameManager.LandscaperBoard.ChangeChunkHeight(FindObjectOfType<Player>(), Target, up ? 1 : -1);
		}

		public void OpenFactoryShop()
		{
			if (Target) GameManager.OpenFactoryShop();
		}

		public void OpenTowerShop()
		{
			if (Target) GameManager.OpenTowerShop();
		}
	}
}