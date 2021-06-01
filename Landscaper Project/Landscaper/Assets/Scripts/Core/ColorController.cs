using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landscaper.Core
{
	public class ColorController : MonoBehaviour
	{
		[Header("Component References")]
		[SerializeField] private Transform _targetModel = null;

		[Header("Color Controller Properties")]
		public Color InitialColor;
		public Color HighlightColor;
		public Color SelectedColor;

		private Renderer objectRenderer;
		private bool selected = false;

		private void Awake()
		{
			if (_targetModel.TryGetComponent<Renderer>(out Renderer renderer))
			{
				InitialColor = renderer.material.color;
				objectRenderer = renderer;
			}
		}

		public void OnPointerClick()
		{
			if (objectRenderer) SetColor(SelectedColor);
			selected = true;
		}
		public void OnPointerUnClick()
		{
			if (objectRenderer) SetColor(InitialColor);
			selected = false;
		}

		public void OnPointerEnter()
		{
			if (objectRenderer && selected == false) SetColor(HighlightColor);
		}

		public void OnPointerExit()
		{

			if (objectRenderer && selected == false) SetColor(InitialColor);
		}

		public void SetColor(Color color)
		{
			if (objectRenderer)  objectRenderer.material.color = color;
		}
	}
}