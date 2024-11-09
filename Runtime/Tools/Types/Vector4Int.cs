using System;
using UnityEngine;

namespace Akela.Tools
{
	[Serializable]
	public struct Vector4Int
	{
		public int x;
		public int y;
		public int z;
		public int w;

        public Vector4Int(int x, int y)
        {
            this.x = x;
			this.y = y;
			this.z = 0;
			this.w = 0;
        }

		public Vector4Int(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = 0;
		}

		public Vector4Int(int x, int y, int z, int w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public static implicit operator Vector4(Vector4Int v) => new(v.x, v.y, v.z, v.w);
	}
}
