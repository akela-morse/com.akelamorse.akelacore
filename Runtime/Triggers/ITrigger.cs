using System;

namespace Akela.Triggers
{
    public interface ITrigger
    {
        bool IsActive { get; }

        void AddListener(Action callback, TriggerEventType eventType = TriggerEventType.OnBecomeActive);
    }

    public enum TriggerEventType
    {
        OnBecomeActive,
        OnBecomeInactive
    }
}