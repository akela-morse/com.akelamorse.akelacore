using System;
using UnityEngine;
#if AKELA_FIKKIS && FIKKIS_FMOD_OK
using Fikkis.Data;
#elif AKELA_FMOD
using FMODUnity;
#else
using UnityEngine.Audio;
#endif

namespace Akela.Bridges
{
    [Serializable]
    public class BridgedAudio : IBridge
    {
#if AKELA_FIKKIS && FIKKIS_FMOD_OK
        [SerializeField] private AudioEvent _internalValue;

        public static implicit operator AudioEvent(BridgedAudio bridge) => bridge._internalValue;

        public static implicit operator bool(BridgedAudio bridge) => bridge._internalValue;
#elif AKELA_FMOD
        [SerializeField] private EventReference  _internalValue;

        public static implicit operator EventReference (BridgedAudio bridge) => bridge._internalValue;

        public static implicit operator bool(BridgedAudio bridge) => !bridge._internalValue.IsNull;
#else
        [SerializeField] private AudioResource _internalValue;

        public static implicit operator AudioResource(BridgedAudio bridge) => bridge._internalValue;

        public static implicit operator bool(BridgedAudio bridge) => bridge._internalValue;
#endif
    }
}