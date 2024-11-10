using System;

namespace Akela.Signals
{
	[Serializable]
	public struct SignalType : IEquatable<SignalType>
	{
		public string type;

        public SignalType(string type)
        {
            this.type = type;
        }

        public override readonly int GetHashCode() => HashCode.Combine(type);

		public override readonly bool Equals(object obj) => obj is SignalType type && Equals(type);

		public readonly bool Equals(SignalType other) => type == other.type;

		public static bool operator ==(SignalType left, SignalType right) => left.Equals(right);

		public static bool operator !=(SignalType left, SignalType right) => !(left == right);

		public static implicit operator SignalType(string s) => new(s);
		public static implicit operator string(SignalType e) => e.type;
	}
}
