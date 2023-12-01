using System.Collections;
using UnityEngine;

public static class UniGifExtension
{
	public static int GetNumeral(this BitArray array, int startIndex, int bitLength)
	{
		BitArray bitArray = new BitArray(bitLength);
		for (int i = 0; i < bitLength; i++)
		{
			if (array.Length <= startIndex + i)
			{
				bitArray[i] = false;
				continue;
			}
			bool value = array.Get(startIndex + i);
			bitArray[i] = value;
		}
		return bitArray.ToNumeral();
	}

	public static int ToNumeral(this BitArray array)
	{
		if (array == null)
		{
			Debug.LogError("array is nothing.");
			return 0;
		}
		if (array.Length > 32)
		{
			Debug.LogError("must be at most 32 bits long.");
			return 0;
		}
		int[] array2 = new int[1];
		array.CopyTo(array2, 0);
		return array2[0];
	}
}
