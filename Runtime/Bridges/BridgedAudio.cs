using System;
using UnityEngine;
#if AKELA_FIKKIS && FIKKIS_FMOD_OK
using Fikkis.Data;
#elif AKELA_FMOD
using FMODUnity;
#endif

namespace Akela.Bridges
{
    [Serializable]
    public class BridgedAudio : IBridge
    {
#if AKELA_FIKKIS && FIKKIS_FMOD_OK
        [SerializeField] private AudioEvent _internalValue;

        public static implicit operator AudioEvent(BridgedAudio bridge) => bridge._internalValue;
#elif AKELA_FMOD
        [SerializeField] private EventReference  _internalValue;

        public static implicit operator EventReference (BridgedAudio bridge) => bridge._internalValue;
#else
        [SerializeField] private AudioClip _internalValue;

        public static implicit operator AudioClip(BridgedAudio bridge) => bridge._internalValue;
#endif
    }
}