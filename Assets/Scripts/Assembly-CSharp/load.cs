using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class load : MonoBehaviour
{
	public Text displayText;

	public Text stepText;

	public Text modelText;

	private string configFilePath;
	private string statusFilePath;

	public GameObject boot;

	public bool ContinueRunning = true;
	public string errorSceneName = "error";

	private void Start()
	{
		configFilePath = Path.Combine(Application.streamingAssetsPath, "config.txt");
		statusFilePath = Path.Combine(Application.streamingAssetsPath, "state.txt");
		string[] array = File.ReadAllLines(configFilePath);
		DisplayText();
		StartCoroutine(UpdateDisplay());
	}

	private void DisplayText()
	{
		string[] array = File.ReadAllLines(statusFilePath);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text.StartsWith("Model"))
			{
				string[] array3 = text.Split('=');
				string text5 = array3[0];
				string text2 = array3[1];
				modelText.text = text2;
			}
			if (text.StartsWith("STEP "))
			{
				string[] array4 = text.Split('=');
				string text3 = array4[0];
				string text4 = array4[1];
				displayText.text = text4;
				stepText.text = text3;
			}
		}
	}

	private IEnumerator UpdateDisplay()
	{
		while (ContinueRunning)
		{
			// Use a StreamReader to read the file
			try
			{
				using (StreamReader reader = new StreamReader(statusFilePath))
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						if (line.StartsWith("STEP "))
						{
							string[] array4 = line.Split('=');
							string text3 = array4[0];
							string text4 = array4[1];
							displayText.text = text4;
							stepText.text = text3;
							bool result = true;
							if (!(array4.Length > 2 && bool.TryParse(array4[2], out result) && result == true))
							{
								boot.SetActive(true);
							}
						}
						else if (line.StartsWith("error"))
						{
							string[] array5 = line.Split('=');
							if (array5[1] == "true")
							{
								LoadErrorScene();
							}
						}
					}
				}
			}
			catch
			{
				Debug.LogError("File Locked");
			}

			yield return new WaitForSeconds(0.125f); // Wait for one second before reading the file again
		}
	}
	private void LoadErrorScene()
	{
		SceneManager.LoadScene(errorSceneName);
	}
}
