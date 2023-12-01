using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UniGifImage : MonoBehaviour
{
	public enum State
	{
		None = 0,
		Loading = 1,
		Ready = 2,
		Playing = 3,
		Pause = 4
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass33_0
	{
		public UniGifImage _003C_003E4__this;

		public bool autoPlay;

		internal void _003CSetGifFromUrlCoroutine_003Eb__0(List<UniGif.GifTexture> gifTexList, int loopCount, int width, int height)
		{
			if (gifTexList != null)
			{
				_003C_003E4__this.m_gifTextureList = gifTexList;
				_003C_003E4__this.loopCount = loopCount;
				_003C_003E4__this.width = width;
				_003C_003E4__this.height = height;
				_003C_003E4__this.nowState = State.Ready;
				_003C_003E4__this.m_imgAspectCtrl.FixAspectRatio(width, height);
				if (_003C_003E4__this.m_rotateOnLoading)
				{
					_003C_003E4__this.transform.localEulerAngles = Vector3.zero;
				}
				if (autoPlay)
				{
					_003C_003E4__this.Play();
				}
			}
			else
			{
				Debug.LogError("Gif texture get error.");
				_003C_003E4__this.nowState = State.None;
			}
		}
	}

	[SerializeField]
	private RawImage m_rawImage;

	[SerializeField]
	private UniGifImageAspectController m_imgAspectCtrl;

	[SerializeField]
	private FilterMode m_filterMode;

	[SerializeField]
	private TextureWrapMode m_wrapMode = TextureWrapMode.Clamp;

	[SerializeField]
	private bool m_loadOnStart;

	[SerializeField]
	private string m_loadOnStartUrl;

	[SerializeField]
	private bool m_rotateOnLoading;

	[SerializeField]
	private bool m_outputDebugLog;

	private List<UniGif.GifTexture> m_gifTextureList;

	private float m_delayTime;

	private int m_gifTextureIndex;

	private int m_nowLoopCount;

	public State nowState { get; private set; }

	public int loopCount { get; private set; }

	public int width { get; private set; }

	public int height { get; private set; }

	private void Start()
	{
		if (m_rawImage == null)
		{
			m_rawImage = GetComponent<RawImage>();
		}
		if (m_loadOnStart)
		{
			SetGifFromUrl(m_loadOnStartUrl);
		}
	}

	private void OnDestroy()
	{
		Clear();
	}

	private void Update()
	{
		switch (nowState)
		{
		case State.Loading:
			if (m_rotateOnLoading)
			{
				base.transform.Rotate(0f, 0f, 30f * Time.deltaTime, Space.Self);
			}
			break;
		case State.Playing:
			if (m_rawImage == null || m_gifTextureList == null || m_gifTextureList.Count <= 0 || m_delayTime > Time.time)
			{
				break;
			}
			m_gifTextureIndex++;
			if (m_gifTextureIndex >= m_gifTextureList.Count)
			{
				m_gifTextureIndex = 0;
				if (loopCount > 0)
				{
					m_nowLoopCount++;
					if (m_nowLoopCount >= loopCount)
					{
						Stop();
						break;
					}
				}
			}
			m_rawImage.texture = m_gifTextureList[m_gifTextureIndex].m_texture2d;
			m_delayTime = Time.time + m_gifTextureList[m_gifTextureIndex].m_delaySec;
			break;
		case State.None:
		case State.Ready:
		case State.Pause:
			break;
		}
	}

	public void SetGifFromUrl(string url, bool autoPlay = true)
	{
		StartCoroutine(SetGifFromUrlCoroutine(url, autoPlay));
	}

	public IEnumerator SetGifFromUrlCoroutine(string url, bool autoPlay = true)
	{
		_003C_003Ec__DisplayClass33_0 _003C_003Ec__DisplayClass33_ = new _003C_003Ec__DisplayClass33_0();
		_003C_003Ec__DisplayClass33_._003C_003E4__this = this;
		_003C_003Ec__DisplayClass33_.autoPlay = autoPlay;
		if (string.IsNullOrEmpty(url))
		{
			Debug.LogError("URL is nothing.");
			yield break;
		}
		if (nowState == State.Loading)
		{
			Debug.LogWarning("Already loading.");
			yield break;
		}
		nowState = State.Loading;
		string url2 = ((!url.StartsWith("http")) ? Path.Combine("file:///" + Application.streamingAssetsPath, url) : url);
		using (WWW www = new WWW(url2))
		{
			yield return www;
			if (!string.IsNullOrEmpty(www.error))
			{
				Debug.LogError("File load error.\n" + www.error);
				nowState = State.None;
				yield break;
			}
			Clear();
			nowState = State.Loading;
			yield return StartCoroutine(UniGif.GetTextureListCoroutine(www.bytes, _003C_003Ec__DisplayClass33_._003CSetGifFromUrlCoroutine_003Eb__0, m_filterMode, m_wrapMode, m_outputDebugLog));
		}
	}

	public void Clear()
	{
		if (m_rawImage != null)
		{
			m_rawImage.texture = null;
		}
		if (m_gifTextureList != null)
		{
			for (int i = 0; i < m_gifTextureList.Count; i++)
			{
				if (m_gifTextureList[i] != null)
				{
					if (m_gifTextureList[i].m_texture2d != null)
					{
						Object.Destroy(m_gifTextureList[i].m_texture2d);
						m_gifTextureList[i].m_texture2d = null;
					}
					m_gifTextureList[i] = null;
				}
			}
			m_gifTextureList.Clear();
			m_gifTextureList = null;
		}
		nowState = State.None;
	}

	public void Play()
	{
		if (nowState != State.Ready)
		{
			Debug.LogWarning("State is not READY.");
			return;
		}
		if (m_rawImage == null || m_gifTextureList == null || m_gifTextureList.Count <= 0)
		{
			Debug.LogError("Raw Image or GIF Texture is nothing.");
			return;
		}
		nowState = State.Playing;
		m_rawImage.texture = m_gifTextureList[0].m_texture2d;
		m_delayTime = Time.time + m_gifTextureList[0].m_delaySec;
		m_gifTextureIndex = 0;
		m_nowLoopCount = 0;
	}

	public void Stop()
	{
		if (nowState != State.Playing && nowState != State.Pause)
		{
			Debug.LogWarning("State is not Playing and Pause.");
		}
		else
		{
			nowState = State.Ready;
		}
	}

	public void Pause()
	{
		if (nowState != State.Playing)
		{
			Debug.LogWarning("State is not Playing.");
		}
		else
		{
			nowState = State.Pause;
		}
	}

	public void Resume()
	{
		if (nowState != State.Pause)
		{
			Debug.LogWarning("State is not Pause.");
		}
		else
		{
			nowState = State.Playing;
		}
	}
}
