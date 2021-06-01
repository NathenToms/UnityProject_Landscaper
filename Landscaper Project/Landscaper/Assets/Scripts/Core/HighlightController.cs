using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Landscaper.Core
{
	public enum HighlightType
	{
		Move,
		HighlightedMove,
		Highlighted,
		Self,
		Ally,
		Attack,
		Selected,
		PlacementOption,
	}

	public class HighlightController : MonoBehaviour
	{
		#region Static Members

		public static int HIGHLIGHTED = 2;
		public static int SELECTED = 3;

		public static int MOVE = 4;
		public static int HIGHLIGHTED_MOVE = 5;
		public static int SELF = 6;
		public static int ALLY = 5;

		public static int ATTACK = 4;

		public static int PLACEMENT_OPTION = 10;
		public static int RESET = 99;

		#endregion

		// List of color that we load into a dictionary to reference
		[SerializeField] private List<HighlightColors> highlightPresets = new List<HighlightColors>();


		// Public Members	
		// List of models we want to color
		public List<Renderer> Renderers = new List<Renderer>();

		// Only values over the current priority can clear this
		public int CurrentPriority = 0;


		public Dictionary<HighlightType, Color> HighlightDictionary;


		// Private Members
		private Color initialColor;

		private void Awake()
		{
			// For now.. Ill just use the first renderer color as the base color
			if (Renderers.Count > 0) {
				initialColor = Renderers[0].material.color;
			}

			HighlightDictionary = new Dictionary<HighlightType, Color>();
			BuildDictionary();
		}

		void BuildDictionary()
		{
			foreach (HighlightColors preset in highlightPresets)
			{
				if (!HighlightDictionary.ContainsKey(preset.HighlightType)) {
					 HighlightDictionary.Add(preset.HighlightType, preset.Color);
				}
			}
		}

		public void SetColor(HighlightType type, int colorPriority, bool isFaded = false)
		{
			if (HighlightDictionary.ContainsKey(type) == false) {
				Debug.LogError("THIS IS NOT A VALID HIGHLIGHT! " + type.ToString());
			}

			if (colorPriority > CurrentPriority) {
				CurrentPriority = colorPriority;

				foreach (Renderer renderers in Renderers) {

					if (isFaded)
					{
						renderers.material.color = (HighlightDictionary[type] + initialColor) / 2f;
					}
					else
					{
						renderers.material.color = HighlightDictionary[type];
					}
				}
			}
		}

		public void Clear(int clearPriority)
		{
			if (clearPriority >= CurrentPriority)
			{
				foreach (Renderer renderers in Renderers) {
					renderers.material.color = initialColor;
				}

				CurrentPriority = 0;
			}
		}
	}

	[System.Serializable]
	public class HighlightColors
	{
		public string HighlighName;

		// The Type of highlight this color maps to
		public HighlightType HighlightType;

		// The Color of this highlight
		public Color Color;
	}
}