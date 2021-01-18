using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Utils
{
	public static class Utils
	{
		public static void Shuffle<T>(this IList<T> list)
		{
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			int n = list.Count;
			while (n > 1)
			{
				byte[] box = new byte[1];
				do provider.GetBytes(box);
				while (!(box[0] < n * (Byte.MaxValue / n)));
				int k = (box[0] % n);
				n--;
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static void ToInts(this Vector3 v, out int x, out int y, out int z)
		{
			x = (int) v.x;
			y = (int) v.y;
			z = (int) v.z;
		}
	}
}
