using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startHandle : MonoBehaviour
{
	public string batchFileName = "game.bat";
	public bool showWindow = false;

	private void Start()
	{
		ProcessStartInfo startInfo = new ProcessStartInfo();
		startInfo.FileName = "powershell.exe";
		startInfo.UseShellExecute = true;
		startInfo.CreateNoWindow = showWindow;
		startInfo.WorkingDirectory = Application.streamingAssetsPath;
		if (showWindow)
        {
			startInfo.WindowStyle = ProcessWindowStyle.Normal;
		}
		else
        {
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
		}
		startInfo.Arguments = "-File \"" + Application.streamingAssetsPath + "/" + batchFileName + "\"";
		Process process = new Process();
		process.StartInfo = startInfo;
		process.Start();
	}
}
