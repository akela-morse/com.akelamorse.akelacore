using UnityEngine;

namespace Akela.Optimisations
{
	public interface ICullingElement
	{
		void IndexChanged(int newIndex);
		void StateChanged(CullingGroupEvent data);
	}
}
