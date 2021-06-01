using UnityEngine;

namespace Landscaper.Core
{
	public interface IRayProvider
	{
		bool Raycast(out RaycastHit raycastHit);
	}
}