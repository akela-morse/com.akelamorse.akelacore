using UnityEngine;

namespace Akela.Optimisations
{
    public interface ICullingElement
    {
        void InitialState(bool visible, int distanceBand);
        void IndexChanged(int newIndex);
        void StateChanged(CullingGroupEvent data);
    }
}
