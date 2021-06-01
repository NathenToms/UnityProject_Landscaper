using Landscaper.Core;

namespace Landscaper.Core
{
	public interface ISelectable
	{
		void OnPointerClick(WorldState worldState);
		void OnPointerEnter(WorldState worldState);
		void OnPointerExit(WorldState worldState);
		void OnPointerUnClick(WorldState worldState);
	}
}