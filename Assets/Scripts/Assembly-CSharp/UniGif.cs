using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public static class UniGif
{
	public class GifTexture
	{
		public Texture2D m_texture2d;

		public float m_delaySec;

		public GifTexture(Texture2D texture2d, float delaySec)
		{
			m_texture2d = texture2d;
			m_delaySec = delaySec;
		}
	}

	private struct GifData
	{
		public byte m_sig0;

		public byte m_sig1;

		public byte m_sig2;

		public byte m_ver0;

		public byte m_ver1;

		public byte m_ver2;

		public ushort m_logicalScreenWidth;

		public ushort m_logicalScreenHeight;

		public bool m_globalColorTableFlag;

		public int m_colorResolution;

		public bool m_sortFlag;

		public int m_sizeOfGlobalColorTable;

		public byte m_bgColorIndex;

		public byte m_pixelAspectRatio;

		public List<byte[]> m_globalColorTable;

		public List<ImageBlock> m_imageBlockList;

		public List<GraphicControlExtension> m_graphicCtrlExList;

		public List<CommentExtension> m_commentExList;

		public List<PlainTextExtension> m_plainTextExList;

		public ApplicationExtension m_appEx;

		public byte m_trailer;

		public string signature
		{
			get
			{
				return new string(new char[3]
				{
					(char)m_sig0,
					(char)m_sig1,
					(char)m_sig2
				});
			}
		}

		public string version
		{
			get
			{
				return new string(new char[3]
				{
					(char)m_ver0,
					(char)m_ver1,
					(char)m_ver2
				});
			}
		}

		public void Dump()
		{
			Debug.Log("GIF Type: " + signature + "-" + version);
			Debug.Log("Image Size: " + m_logicalScreenWidth + "x" + m_logicalScreenHeight);
			Debug.Log("Animation Image Count: " + m_imageBlockList.Count);
			Debug.Log("Animation Loop Count (0 is infinite): " + m_appEx.loopCount);
			if (m_graphicCtrlExList != null && m_graphicCtrlExList.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder("Animation Delay Time (1/100sec)");
				for (int i = 0; i < m_graphicCtrlExList.Count; i++)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(m_graphicCtrlExList[i].m_delayTime);
				}
				Debug.Log(stringBuilder.ToString());
			}
			Debug.Log("Application Identifier: " + m_appEx.applicationIdentifier);
			Debug.Log("Application Authentication Code: " + m_appEx.applicationAuthenticationCode);
		}
	}

	private struct ImageBlock
	{
		public struct ImageDataBlock
		{
			public byte m_blockSize;

			public byte[] m_imageData;
		}

		public byte m_imageSeparator;

		public ushort m_imageLeftPosition;

		public ushort m_imageTopPosition;

		public ushort m_imageWidth;

		public ushort m_imageHeight;

		public bool m_localColorTableFlag;

		public bool m_interlaceFlag;

		public bool m_sortFlag;

		public int m_sizeOfLocalColorTable;

		public List<byte[]> m_localColorTable;

		public byte m_lzwMinimumCodeSize;

		public List<ImageDataBlock> m_imageDataList;
	}

	private struct GraphicControlExtension
	{
		public byte m_extensionIntroducer;

		public byte m_graphicControlLabel;

		public byte m_blockSize;

		public ushort m_disposalMethod;

		public bool m_transparentColorFlag;

		public ushort m_delayTime;

		public byte m_transparentColorIndex;

		public byte m_blockTerminator;
	}

	private struct CommentExtension
	{
		public struct CommentDataBlock
		{
			public byte m_blockSize;

			public byte[] m_commentData;
		}

		public byte m_extensionIntroducer;

		public byte m_commentLabel;

		public List<CommentDataBlock> m_commentDataList;
	}

	private struct PlainTextExtension
	{
		public struct PlainTextDataBlock
		{
			public byte m_blockSize;

			public byte[] m_plainTextData;
		}

		public byte m_extensionIntroducer;

		public byte m_plainTextLabel;

		public byte m_blockSize;

		public List<PlainTextDataBlock> m_plainTextDataList;
	}

	private struct ApplicationExtension
	{
		public struct ApplicationDataBlock
		{
			public byte m_blockSize;

			public byte[] m_applicationData;
		}

		public byte m_extensionIntroducer;

		public byte m_extensionLabel;

		public byte m_blockSize;

		public byte m_appId1;

		public byte m_appId2;

		public byte m_appId3;

		public byte m_appId4;

		public byte m_appId5;

		public byte m_appId6;

		public byte m_appId7;

		public byte m_appId8;

		public byte m_appAuthCode1;

		public byte m_appAuthCode2;

		public byte m_appAuthCode3;

		public List<ApplicationDataBlock> m_appDataList;

		public string applicationIdentifier
		{
			get
			{
				return new string(new char[8]
				{
					(char)m_appId1,
					(char)m_appId2,
					(char)m_appId3,
					(char)m_appId4,
					(char)m_appId5,
					(char)m_appId6,
					(char)m_appId7,
					(char)m_appId8
				});
			}
		}

		public string applicationAuthenticationCode
		{
			get
			{
				return new string(new char[3]
				{
					(char)m_appAuthCode1,
					(char)m_appAuthCode2,
					(char)m_appAuthCode3
				});
			}
		}

		public int loopCount
		{
			get
			{
				if (m_appDataList == null || m_appDataList.Count < 1 || m_appDataList[0].m_applicationData.Length < 3 || m_appDataList[0].m_applicationData[0] != 1)
				{
					return 0;
				}
				return BitConverter.ToUInt16(m_appDataList[0].m_applicationData, 1);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public List<GifTexture> gifTexList;

		internal void _003CGetTextureListCoroutine_003Eb__0(List<GifTexture> result)
		{
			gifTexList = result;
		}
	}

	public static IEnumerator GetTextureListCoroutine(byte[] bytes, Action<List<GifTexture>, int, int, int> callback, FilterMode filterMode = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp, bool debugLog = false)
	{
		_003C_003Ec__DisplayClass0_0 _003C_003Ec__DisplayClass0_ = new _003C_003Ec__DisplayClass0_0();
		int loopCount2 = -1;
		int width2 = 0;
		int height2 = 0;
		GifData gifData = default(GifData);
		if (!SetGifData(bytes, ref gifData, debugLog))
		{
			Debug.LogError("GIF file data set error.");
			if (callback != null)
			{
				callback(null, loopCount2, width2, height2);
			}
			yield break;
		}
		_003C_003Ec__DisplayClass0_.gifTexList = null;
		yield return DecodeTextureCoroutine(gifData, _003C_003Ec__DisplayClass0_._003CGetTextureListCoroutine_003Eb__0, filterMode, wrapMode);
		if (_003C_003Ec__DisplayClass0_.gifTexList == null || _003C_003Ec__DisplayClass0_.gifTexList.Count <= 0)
		{
			Debug.LogError("GIF texture decode error.");
			if (callback != null)
			{
				callback(null, loopCount2, width2, height2);
			}
			yield break;
		}
		loopCount2 = gifData.m_appEx.loopCount;
		width2 = gifData.m_logicalScreenWidth;
		height2 = gifData.m_logicalScreenHeight;
		if (callback != null)
		{
			callback(_003C_003Ec__DisplayClass0_.gifTexList, loopCount2, width2, height2);
		}
	}

	private static IEnumerator DecodeTextureCoroutine(GifData gifData, Action<List<GifTexture>> callback, FilterMode filterMode, TextureWrapMode wrapMode)
	{
		if (gifData.m_imageBlockList == null || gifData.m_imageBlockList.Count < 1)
		{
			yield break;
		}
		List<GifTexture> gifTexList = new List<GifTexture>(gifData.m_imageBlockList.Count);
		List<ushort> disposalMethodList = new List<ushort>(gifData.m_imageBlockList.Count);
		int imgIndex = 0;
		for (int i = 0; i < gifData.m_imageBlockList.Count; i++)
		{
			byte[] decodedData = GetDecodedData(gifData.m_imageBlockList[i]);
			GraphicControlExtension? graphicCtrlEx = GetGraphicCtrlExt(gifData, imgIndex);
			int transparentIndex = GetTransparentIndex(graphicCtrlEx);
			disposalMethodList.Add(GetDisposalMethod(graphicCtrlEx));
			Color32 bgColor;
			List<byte[]> colorTable = GetColorTableAndSetBgColor(gifData, gifData.m_imageBlockList[i], transparentIndex, out bgColor);
			yield return 0;
			bool filledTexture;
			Texture2D tex = CreateTexture2D(gifData, gifTexList, imgIndex, disposalMethodList, bgColor, filterMode, wrapMode, out filledTexture);
			yield return 0;
			int dataIndex = 0;
			for (int num = tex.height - 1; num >= 0; num--)
			{
				SetTexturePixelRow(tex, num, gifData.m_imageBlockList[i], decodedData, ref dataIndex, colorTable, bgColor, transparentIndex, filledTexture);
			}
			tex.Apply();
			yield return 0;
			float delaySec = GetDelaySec(graphicCtrlEx);
			gifTexList.Add(new GifTexture(tex, delaySec));
			imgIndex++;
			bgColor = default(Color32);
		}
		if (callback != null)
		{
			callback(gifTexList);
		}
	}

	private static byte[] GetDecodedData(ImageBlock imgBlock)
	{
		List<byte> list = new List<byte>();
		for (int i = 0; i < imgBlock.m_imageDataList.Count; i++)
		{
			for (int j = 0; j < imgBlock.m_imageDataList[i].m_imageData.Length; j++)
			{
				list.Add(imgBlock.m_imageDataList[i].m_imageData[j]);
			}
		}
		int needDataSize = imgBlock.m_imageHeight * imgBlock.m_imageWidth;
		byte[] array = DecodeGifLZW(list, imgBlock.m_lzwMinimumCodeSize, needDataSize);
		if (imgBlock.m_interlaceFlag)
		{
			array = SortInterlaceGifData(array, imgBlock.m_imageWidth);
		}
		return array;
	}

	private static List<byte[]> GetColorTableAndSetBgColor(GifData gifData, ImageBlock imgBlock, int transparentIndex, out Color32 bgColor)
	{
		List<byte[]> list = (imgBlock.m_localColorTableFlag ? imgBlock.m_localColorTable : (gifData.m_globalColorTableFlag ? gifData.m_globalColorTable : null));
		if (list != null)
		{
			byte[] array = list[gifData.m_bgColorIndex];
			bgColor = new Color32(array[0], array[1], array[2], (byte)((transparentIndex != gifData.m_bgColorIndex) ? 255u : 0u));
		}
		else
		{
			bgColor = Color.black;
		}
		return list;
	}

	private static GraphicControlExtension? GetGraphicCtrlExt(GifData gifData, int imgBlockIndex)
	{
		if (gifData.m_graphicCtrlExList != null && gifData.m_graphicCtrlExList.Count > imgBlockIndex)
		{
			return gifData.m_graphicCtrlExList[imgBlockIndex];
		}
		return null;
	}

	private static int GetTransparentIndex(GraphicControlExtension? graphicCtrlEx)
	{
		int result = -1;
		if (graphicCtrlEx.HasValue && graphicCtrlEx.Value.m_transparentColorFlag)
		{
			result = graphicCtrlEx.Value.m_transparentColorIndex;
		}
		return result;
	}

	private static float GetDelaySec(GraphicControlExtension? graphicCtrlEx)
	{
		float num = (graphicCtrlEx.HasValue ? ((float)(int)graphicCtrlEx.Value.m_delayTime / 100f) : (1f / 60f));
		if (num <= 0f)
		{
			num = 0.1f;
		}
		return num;
	}

	private static ushort GetDisposalMethod(GraphicControlExtension? graphicCtrlEx)
	{
		if (!graphicCtrlEx.HasValue)
		{
			return 2;
		}
		return graphicCtrlEx.Value.m_disposalMethod;
	}

	private static Texture2D CreateTexture2D(GifData gifData, List<GifTexture> gifTexList, int imgIndex, List<ushort> disposalMethodList, Color32 bgColor, FilterMode filterMode, TextureWrapMode wrapMode, out bool filledTexture)
	{
		filledTexture = false;
		Texture2D texture2D = new Texture2D(gifData.m_logicalScreenWidth, gifData.m_logicalScreenHeight, TextureFormat.ARGB32, false);
		texture2D.filterMode = filterMode;
		texture2D.wrapMode = wrapMode;
		ushort num = (ushort)((imgIndex > 0) ? disposalMethodList[imgIndex - 1] : 2);
		int num2 = -1;
		switch (num)
		{
		case 1:
			num2 = imgIndex - 1;
			break;
		case 2:
		{
			filledTexture = true;
			Color32[] array = new Color32[texture2D.width * texture2D.height];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = bgColor;
			}
			texture2D.SetPixels32(array);
			texture2D.Apply();
			break;
		}
		case 3:
		{
			for (int num3 = imgIndex - 1; num3 >= 0; num3--)
			{
				if (disposalMethodList[num3] == 0 || disposalMethodList[num3] == 1)
				{
					num2 = num3;
					break;
				}
			}
			break;
		}
		}
		if (num2 >= 0)
		{
			filledTexture = true;
			Color32[] pixels = gifTexList[num2].m_texture2d.GetPixels32();
			texture2D.SetPixels32(pixels);
			texture2D.Apply();
		}
		return texture2D;
	}

	private static void SetTexturePixelRow(Texture2D tex, int y, ImageBlock imgBlock, byte[] decodedData, ref int dataIndex, List<byte[]> colorTable, Color32 bgColor, int transparentIndex, bool filledTexture)
	{
		int num = tex.height - 1 - y;
		for (int i = 0; i < tex.width; i++)
		{
			int num2 = i;
			if (num < imgBlock.m_imageTopPosition || num >= imgBlock.m_imageTopPosition + imgBlock.m_imageHeight || num2 < imgBlock.m_imageLeftPosition || num2 >= imgBlock.m_imageLeftPosition + imgBlock.m_imageWidth)
			{
				if (!filledTexture)
				{
					tex.SetPixel(i, y, bgColor);
				}
				continue;
			}
			if (dataIndex >= decodedData.Length)
			{
				if (!filledTexture)
				{
					tex.SetPixel(i, y, bgColor);
					if (dataIndex == decodedData.Length)
					{
						Debug.LogError("dataIndex exceeded the size of decodedData. dataIndex:" + dataIndex + " decodedData.Length:" + decodedData.Length + " y:" + y + " x:" + i);
					}
				}
				dataIndex++;
				continue;
			}
			byte b = decodedData[dataIndex];
			if (colorTable == null || colorTable.Count <= b)
			{
				if (!filledTexture)
				{
					tex.SetPixel(i, y, bgColor);
					if (colorTable == null)
					{
						Debug.LogError("colorIndex exceeded the size of colorTable. colorTable is null. colorIndex:" + b);
					}
					else
					{
						Debug.LogError("colorIndex exceeded the size of colorTable. colorTable.Count:" + colorTable.Count + " colorIndex:" + b);
					}
				}
				dataIndex++;
			}
			else
			{
				byte[] array = colorTable[b];
				byte b2 = (byte)((transparentIndex < 0 || transparentIndex != b) ? byte.MaxValue : 0);
				if (!filledTexture || b2 != 0)
				{
					tex.SetPixel(color: new Color32(array[0], array[1], array[2], b2), x: i, y: y);
				}
				dataIndex++;
			}
		}
	}

	private static byte[] DecodeGifLZW(List<byte> compData, int lzwMinimumCodeSize, int needDataSize)
	{
		int clearCode = 0;
		int finishCode = 0;
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		int lzwCodeSize = 0;
		InitDictionary(dictionary, lzwMinimumCodeSize, out lzwCodeSize, out clearCode, out finishCode);
		BitArray bitArray = new BitArray(compData.ToArray());
		byte[] array = new byte[needDataSize];
		int num = 0;
		string text = null;
		bool flag = false;
		int num2 = 0;
		while (num2 < bitArray.Length)
		{
			if (flag)
			{
				InitDictionary(dictionary, lzwMinimumCodeSize, out lzwCodeSize, out clearCode, out finishCode);
				flag = false;
			}
			int numeral = bitArray.GetNumeral(num2, lzwCodeSize);
			string text2 = null;
			if (numeral == clearCode)
			{
				flag = true;
				num2 += lzwCodeSize;
				text = null;
				continue;
			}
			if (numeral == finishCode)
			{
				Debug.LogWarning("early stop code. bitDataIndex:" + num2 + " lzwCodeSize:" + lzwCodeSize + " key:" + numeral + " dic.Count:" + dictionary.Count);
				break;
			}
			if (dictionary.ContainsKey(numeral))
			{
				text2 = dictionary[numeral];
			}
			else
			{
				if (numeral < dictionary.Count)
				{
					Debug.LogWarning("It is strange that come here. bitDataIndex:" + num2 + " lzwCodeSize:" + lzwCodeSize + " key:" + numeral + " dic.Count:" + dictionary.Count);
					num2 += lzwCodeSize;
					continue;
				}
				if (text == null)
				{
					Debug.LogWarning("It is strange that come here. bitDataIndex:" + num2 + " lzwCodeSize:" + lzwCodeSize + " key:" + numeral + " dic.Count:" + dictionary.Count);
					num2 += lzwCodeSize;
					continue;
				}
				text2 = text + text[0];
			}
			byte[] bytes = Encoding.Unicode.GetBytes(text2);
			for (int i = 0; i < bytes.Length; i++)
			{
				if (i % 2 == 0)
				{
					array[num] = bytes[i];
					num++;
				}
			}
			if (num >= needDataSize)
			{
				break;
			}
			if (text != null)
			{
				dictionary.Add(dictionary.Count, text + text2[0]);
			}
			text = text2;
			num2 += lzwCodeSize;
			if (lzwCodeSize == 3 && dictionary.Count >= 8)
			{
				lzwCodeSize = 4;
			}
			else if (lzwCodeSize == 4 && dictionary.Count >= 16)
			{
				lzwCodeSize = 5;
			}
			else if (lzwCodeSize == 5 && dictionary.Count >= 32)
			{
				lzwCodeSize = 6;
			}
			else if (lzwCodeSize == 6 && dictionary.Count >= 64)
			{
				lzwCodeSize = 7;
			}
			else if (lzwCodeSize == 7 && dictionary.Count >= 128)
			{
				lzwCodeSize = 8;
			}
			else if (lzwCodeSize == 8 && dictionary.Count >= 256)
			{
				lzwCodeSize = 9;
			}
			else if (lzwCodeSize == 9 && dictionary.Count >= 512)
			{
				lzwCodeSize = 10;
			}
			else if (lzwCodeSize == 10 && dictionary.Count >= 1024)
			{
				lzwCodeSize = 11;
			}
			else if (lzwCodeSize == 11 && dictionary.Count >= 2048)
			{
				lzwCodeSize = 12;
			}
			else if (lzwCodeSize == 12 && dictionary.Count >= 4096 && bitArray.GetNumeral(num2, lzwCodeSize) != clearCode)
			{
				flag = true;
			}
		}
		return array;
	}

	private static void InitDictionary(Dictionary<int, string> dic, int lzwMinimumCodeSize, out int lzwCodeSize, out int clearCode, out int finishCode)
	{
		int num = (clearCode = (int)Math.Pow(2.0, lzwMinimumCodeSize));
		finishCode = clearCode + 1;
		dic.Clear();
		for (int i = 0; i < num + 2; i++)
		{
			dic.Add(i, ((char)i).ToString());
		}
		lzwCodeSize = lzwMinimumCodeSize + 1;
	}

	private static byte[] SortInterlaceGifData(byte[] decodedData, int xNum)
	{
		int num = 0;
		int num2 = 0;
		byte[] array = new byte[decodedData.Length];
		for (int i = 0; i < array.Length; i++)
		{
			if (num % 8 == 0)
			{
				array[i] = decodedData[num2];
				num2++;
			}
			if (i != 0 && i % xNum == 0)
			{
				num++;
			}
		}
		num = 0;
		for (int j = 0; j < array.Length; j++)
		{
			if (num % 8 == 4)
			{
				array[j] = decodedData[num2];
				num2++;
			}
			if (j != 0 && j % xNum == 0)
			{
				num++;
			}
		}
		num = 0;
		for (int k = 0; k < array.Length; k++)
		{
			if (num % 4 == 2)
			{
				array[k] = decodedData[num2];
				num2++;
			}
			if (k != 0 && k % xNum == 0)
			{
				num++;
			}
		}
		num = 0;
		for (int l = 0; l < array.Length; l++)
		{
			if (num % 8 != 0 && num % 8 != 4 && num % 4 != 2)
			{
				array[l] = decodedData[num2];
				num2++;
			}
			if (l != 0 && l % xNum == 0)
			{
				num++;
			}
		}
		return array;
	}

	private static bool SetGifData(byte[] gifBytes, ref GifData gifData, bool debugLog)
	{
		if (debugLog)
		{
			Debug.Log("SetGifData Start.");
		}
		if (gifBytes == null || gifBytes.Length == 0)
		{
			Debug.LogError("bytes is nothing.");
			return false;
		}
		int byteIndex = 0;
		if (!SetGifHeader(gifBytes, ref byteIndex, ref gifData))
		{
			Debug.LogError("GIF header set error.");
			return false;
		}
		if (!SetGifBlock(gifBytes, ref byteIndex, ref gifData))
		{
			Debug.LogError("GIF block set error.");
			return false;
		}
		if (debugLog)
		{
			gifData.Dump();
			Debug.Log("SetGifData Finish.");
		}
		return true;
	}

	private static bool SetGifHeader(byte[] gifBytes, ref int byteIndex, ref GifData gifData)
	{
		if (gifBytes[0] != 71 || gifBytes[1] != 73 || gifBytes[2] != 70)
		{
			Debug.LogError("This is not GIF image.");
			return false;
		}
		gifData.m_sig0 = gifBytes[0];
		gifData.m_sig1 = gifBytes[1];
		gifData.m_sig2 = gifBytes[2];
		if ((gifBytes[3] != 56 || gifBytes[4] != 55 || gifBytes[5] != 97) && (gifBytes[3] != 56 || gifBytes[4] != 57 || gifBytes[5] != 97))
		{
			Debug.LogError("GIF version error.\nSupported only GIF87a or GIF89a.");
			return false;
		}
		gifData.m_ver0 = gifBytes[3];
		gifData.m_ver1 = gifBytes[4];
		gifData.m_ver2 = gifBytes[5];
		gifData.m_logicalScreenWidth = BitConverter.ToUInt16(gifBytes, 6);
		gifData.m_logicalScreenHeight = BitConverter.ToUInt16(gifBytes, 8);
		gifData.m_globalColorTableFlag = (gifBytes[10] & 0x80) == 128;
		switch (gifBytes[10] & 0x70)
		{
		case 112:
			gifData.m_colorResolution = 8;
			break;
		case 96:
			gifData.m_colorResolution = 7;
			break;
		case 80:
			gifData.m_colorResolution = 6;
			break;
		case 64:
			gifData.m_colorResolution = 5;
			break;
		case 48:
			gifData.m_colorResolution = 4;
			break;
		case 32:
			gifData.m_colorResolution = 3;
			break;
		case 16:
			gifData.m_colorResolution = 2;
			break;
		default:
			gifData.m_colorResolution = 1;
			break;
		}
		gifData.m_sortFlag = (gifBytes[10] & 8) == 8;
		int num = (gifBytes[10] & 7) + 1;
		gifData.m_sizeOfGlobalColorTable = (int)Math.Pow(2.0, num);
		gifData.m_bgColorIndex = gifBytes[11];
		gifData.m_pixelAspectRatio = gifBytes[12];
		byteIndex = 13;
		if (gifData.m_globalColorTableFlag)
		{
			gifData.m_globalColorTable = new List<byte[]>();
			for (int i = byteIndex; i < byteIndex + gifData.m_sizeOfGlobalColorTable * 3; i += 3)
			{
				gifData.m_globalColorTable.Add(new byte[3]
				{
					gifBytes[i],
					gifBytes[i + 1],
					gifBytes[i + 2]
				});
			}
			byteIndex += gifData.m_sizeOfGlobalColorTable * 3;
		}
		return true;
	}

	private static bool SetGifBlock(byte[] gifBytes, ref int byteIndex, ref GifData gifData)
	{
		try
		{
			int num = 0;
			while (true)
			{
				int num2 = byteIndex;
				if (gifBytes[num2] == 44)
				{
					SetImageBlock(gifBytes, ref byteIndex, ref gifData);
				}
				else if (gifBytes[num2] == 33)
				{
					switch (gifBytes[num2 + 1])
					{
					case 249:
						SetGraphicControlExtension(gifBytes, ref byteIndex, ref gifData);
						break;
					case 254:
						SetCommentExtension(gifBytes, ref byteIndex, ref gifData);
						break;
					case 1:
						SetPlainTextExtension(gifBytes, ref byteIndex, ref gifData);
						break;
					case byte.MaxValue:
						SetApplicationExtension(gifBytes, ref byteIndex, ref gifData);
						break;
					}
				}
				else if (gifBytes[num2] == 59)
				{
					break;
				}
				if (num == num2)
				{
					Debug.LogError("Infinite loop error.");
					return false;
				}
				num = num2;
			}
			gifData.m_trailer = gifBytes[byteIndex];
			byteIndex++;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			return false;
		}
		return true;
	}

	private static void SetImageBlock(byte[] gifBytes, ref int byteIndex, ref GifData gifData)
	{
		ImageBlock item = default(ImageBlock);
		item.m_imageSeparator = gifBytes[byteIndex];
		byteIndex++;
		item.m_imageLeftPosition = BitConverter.ToUInt16(gifBytes, byteIndex);
		byteIndex += 2;
		item.m_imageTopPosition = BitConverter.ToUInt16(gifBytes, byteIndex);
		byteIndex += 2;
		item.m_imageWidth = BitConverter.ToUInt16(gifBytes, byteIndex);
		byteIndex += 2;
		item.m_imageHeight = BitConverter.ToUInt16(gifBytes, byteIndex);
		byteIndex += 2;
		item.m_localColorTableFlag = (gifBytes[byteIndex] & 0x80) == 128;
		item.m_interlaceFlag = (gifBytes[byteIndex] & 0x40) == 64;
		item.m_sortFlag = (gifBytes[byteIndex] & 0x20) == 32;
		int num = (gifBytes[byteIndex] & 7) + 1;
		item.m_sizeOfLocalColorTable = (int)Math.Pow(2.0, num);
		byteIndex++;
		if (item.m_localColorTableFlag)
		{
			item.m_localColorTable = new List<byte[]>();
			for (int i = byteIndex; i < byteIndex + item.m_sizeOfLocalColorTable * 3; i += 3)
			{
				item.m_localColorTable.Add(new byte[3]
				{
					gifBytes[i],
					gifBytes[i + 1],
					gifBytes[i + 2]
				});
			}
			byteIndex += item.m_sizeOfLocalColorTable * 3;
		}
		item.m_lzwMinimumCodeSize = gifBytes[byteIndex];
		byteIndex++;
		while (true)
		{
			byte b = gifBytes[byteIndex];
			byteIndex++;
			if (b == 0)
			{
				break;
			}
			ImageBlock.ImageDataBlock item2 = default(ImageBlock.ImageDataBlock);
			item2.m_blockSize = b;
			item2.m_imageData = new byte[item2.m_blockSize];
			for (int j = 0; j < item2.m_imageData.Length; j++)
			{
				item2.m_imageData[j] = gifBytes[byteIndex];
				byteIndex++;
			}
			if (item.m_imageDataList == null)
			{
				item.m_imageDataList = new List<ImageBlock.ImageDataBlock>();
			}
			item.m_imageDataList.Add(item2);
		}
		if (gifData.m_imageBlockList == null)
		{
			gifData.m_imageBlockList = new List<ImageBlock>();
		}
		gifData.m_imageBlockList.Add(item);
	}

	private static void SetGraphicControlExtension(byte[] gifBytes, ref int byteIndex, ref GifData gifData)
	{
		GraphicControlExtension item = default(GraphicControlExtension);
		item.m_extensionIntroducer = gifBytes[byteIndex];
		byteIndex++;
		item.m_graphicControlLabel = gifBytes[byteIndex];
		byteIndex++;
		item.m_blockSize = gifBytes[byteIndex];
		byteIndex++;
		switch (gifBytes[byteIndex] & 0x1C)
		{
		case 4:
			item.m_disposalMethod = 1;
			break;
		case 8:
			item.m_disposalMethod = 2;
			break;
		case 12:
			item.m_disposalMethod = 3;
			break;
		default:
			item.m_disposalMethod = 0;
			break;
		}
		item.m_transparentColorFlag = (gifBytes[byteIndex] & 1) == 1;
		byteIndex++;
		item.m_delayTime = BitConverter.ToUInt16(gifBytes, byteIndex);
		byteIndex += 2;
		item.m_transparentColorIndex = gifBytes[byteIndex];
		byteIndex++;
		item.m_blockTerminator = gifBytes[byteIndex];
		byteIndex++;
		if (gifData.m_graphicCtrlExList == null)
		{
			gifData.m_graphicCtrlExList = new List<GraphicControlExtension>();
		}
		gifData.m_graphicCtrlExList.Add(item);
	}

	private static void SetCommentExtension(byte[] gifBytes, ref int byteIndex, ref GifData gifData)
	{
		CommentExtension item = default(CommentExtension);
		item.m_extensionIntroducer = gifBytes[byteIndex];
		byteIndex++;
		item.m_commentLabel = gifBytes[byteIndex];
		byteIndex++;
		while (true)
		{
			byte b = gifBytes[byteIndex];
			byteIndex++;
			if (b == 0)
			{
				break;
			}
			CommentExtension.CommentDataBlock item2 = default(CommentExtension.CommentDataBlock);
			item2.m_blockSize = b;
			item2.m_commentData = new byte[item2.m_blockSize];
			for (int i = 0; i < item2.m_commentData.Length; i++)
			{
				item2.m_commentData[i] = gifBytes[byteIndex];
				byteIndex++;
			}
			if (item.m_commentDataList == null)
			{
				item.m_commentDataList = new List<CommentExtension.CommentDataBlock>();
			}
			item.m_commentDataList.Add(item2);
		}
		if (gifData.m_commentExList == null)
		{
			gifData.m_commentExList = new List<CommentExtension>();
		}
		gifData.m_commentExList.Add(item);
	}

	private static void SetPlainTextExtension(byte[] gifBytes, ref int byteIndex, ref GifData gifData)
	{
		PlainTextExtension item = default(PlainTextExtension);
		item.m_extensionIntroducer = gifBytes[byteIndex];
		byteIndex++;
		item.m_plainTextLabel = gifBytes[byteIndex];
		byteIndex++;
		item.m_blockSize = gifBytes[byteIndex];
		byteIndex++;
		byteIndex += 2;
		byteIndex += 2;
		byteIndex += 2;
		byteIndex += 2;
		byteIndex++;
		byteIndex++;
		byteIndex++;
		byteIndex++;
		while (true)
		{
			byte b = gifBytes[byteIndex];
			byteIndex++;
			if (b == 0)
			{
				break;
			}
			PlainTextExtension.PlainTextDataBlock item2 = default(PlainTextExtension.PlainTextDataBlock);
			item2.m_blockSize = b;
			item2.m_plainTextData = new byte[item2.m_blockSize];
			for (int i = 0; i < item2.m_plainTextData.Length; i++)
			{
				item2.m_plainTextData[i] = gifBytes[byteIndex];
				byteIndex++;
			}
			if (item.m_plainTextDataList == null)
			{
				item.m_plainTextDataList = new List<PlainTextExtension.PlainTextDataBlock>();
			}
			item.m_plainTextDataList.Add(item2);
		}
		if (gifData.m_plainTextExList == null)
		{
			gifData.m_plainTextExList = new List<PlainTextExtension>();
		}
		gifData.m_plainTextExList.Add(item);
	}

	private static void SetApplicationExtension(byte[] gifBytes, ref int byteIndex, ref GifData gifData)
	{
		gifData.m_appEx.m_extensionIntroducer = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_extensionLabel = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_blockSize = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId1 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId2 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId3 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId4 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId5 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId6 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId7 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appId8 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appAuthCode1 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appAuthCode2 = gifBytes[byteIndex];
		byteIndex++;
		gifData.m_appEx.m_appAuthCode3 = gifBytes[byteIndex];
		byteIndex++;
		while (true)
		{
			byte b = gifBytes[byteIndex];
			byteIndex++;
			if (b != 0)
			{
				ApplicationExtension.ApplicationDataBlock item = default(ApplicationExtension.ApplicationDataBlock);
				item.m_blockSize = b;
				item.m_applicationData = new byte[item.m_blockSize];
				for (int i = 0; i < item.m_applicationData.Length; i++)
				{
					item.m_applicationData[i] = gifBytes[byteIndex];
					byteIndex++;
				}
				if (gifData.m_appEx.m_appDataList == null)
				{
					gifData.m_appEx.m_appDataList = new List<ApplicationExtension.ApplicationDataBlock>();
				}
				gifData.m_appEx.m_appDataList.Add(item);
				continue;
			}
			break;
		}
	}
}
