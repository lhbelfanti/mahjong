using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using UnityEditor;
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
				int k = box[0] % n;
				n--;
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static List<GameObject> GetActiveChildren(this GameObject element)
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < element.transform.childCount; i++)
			{
				GameObject child = element.transform.GetChild(i).gameObject;
				if (child.activeSelf)
					list.Add(child);
			}
			return list;
		}

		public static void Init(this bool[] array, bool value)
		{
			for (int i = 0; i < array.Length; i++)
				array[i] = value;
		}

		public static void ClearConsole()
		{
#if (UNITY_EDITOR)
			var assembly = Assembly.GetAssembly(typeof(SceneView));
			var type = assembly.GetType("UnityEditor.LogEntries");
			var method = type.GetMethod("Clear");
			method.Invoke(new object(), null);
#endif
		}
	}
}
