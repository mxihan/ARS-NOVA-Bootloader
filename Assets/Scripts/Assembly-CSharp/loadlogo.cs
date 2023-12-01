using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class loadlogo : MonoBehaviour
{
	public Image imageComponent;

	private void Start()
	{
		StartCoroutine(LoadImage());
	}

	private IEnumerator LoadImage()
	{
		string text = Path.Combine(Application.streamingAssetsPath, "logo.png");
		if (text.Contains("://") || text.Contains(":///"))
		{
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(text);
			yield return www.SendWebRequest();
			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
				yield break;
			}
			Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
			imageComponent.sprite = sprite;
		}
		else
		{
			byte[] data = File.ReadAllBytes(text);
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
			Sprite sprite2 = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
			imageComponent.sprite = sprite2;
		}
	}
}
