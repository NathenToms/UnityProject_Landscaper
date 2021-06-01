using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

using Landscaper.Pieces;


namespace Landscaper.UI.CustomComponents
{
	public class GamePieceShopButton : MonoBehaviour, IPointerClickHandler
	{
		[SerializeField] private TextMeshProUGUI _pieceNameText = null;
		[SerializeField] private TextMeshProUGUI _pieceQuantText = null;
		[SerializeField] private Piece _piecePrefab = null;

		private PiecePicker piecePicker;

		public GamePieceShopButton Init(PiecePicker picker, int pieceCount, string name)
		{
			piecePicker = picker;

			_pieceNameText.text = name;
			_pieceQuantText.text = pieceCount.ToString();

			if (pieceCount == 0) gameObject.SetActive(false);

			return this;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			piecePicker.OnPick(_piecePrefab);
		}
	}
}