using UnityEngine;

[ExecuteInEditMode]
public class UniGifImageAspectController : MonoBehaviour
{
	public int m_originalWidth;

	public int m_originalHeight;

	public bool m_fixOnUpdate;

	private Vector2 m_lastSize = Vector2.zero;

	private Vector2 m_newSize = Vector2.zero;

	private RectTransform m_rectTransform;

	public RectTransform rectTransform
	{
		get
		{
			if (!(m_rectTransform != null))
			{
				return m_rectTransform = GetComponent<RectTransform>();
			}
			return m_rectTransform;
		}
	}

	private void Update()
	{
		if (m_fixOnUpdate)
		{
			FixAspectRatio();
		}
	}

	public void FixAspectRatio(int originalWidth = -1, int originalHeight = -1)
	{
		bool flag = false;
		if (originalWidth > 0 && originalHeight > 0)
		{
			m_originalWidth = originalWidth;
			m_originalHeight = originalHeight;
			flag = true;
		}
		if (m_originalWidth <= 0 || m_originalHeight <= 0)
		{
			return;
		}
		bool flag2;
		if (flag || m_lastSize.x != rectTransform.sizeDelta.x)
		{
			flag2 = true;
		}
		else
		{
			if (m_lastSize.y == rectTransform.sizeDelta.y)
			{
				return;
			}
			flag2 = false;
		}
		if (flag2)
		{
			float num = rectTransform.sizeDelta.x / (float)m_originalWidth;
			m_newSize.Set(rectTransform.sizeDelta.x, (float)m_originalHeight * num);
		}
		else
		{
			float num2 = rectTransform.sizeDelta.y / (float)m_originalHeight;
			m_newSize.Set((float)m_originalWidth * num2, rectTransform.sizeDelta.y);
		}
		rectTransform.sizeDelta = m_newSize;
		m_lastSize = rectTransform.sizeDelta;
	}
}
