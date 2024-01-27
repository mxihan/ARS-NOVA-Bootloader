using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CheckStart : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoPlayerObject;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Reset();
        if (File.Exists(Path.Combine("C:\\SEGA\\system\\BIOS_ENABLE")))
        {

            Debug.Log("BIOS Enabled");
            videoPlayerObject.SetActive(true);
            StartCoroutine(checkState());
        }
        else
        {
            Debug.Log("BIOS Skipped");
            transition();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void Reset()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "current_config.txt");
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine("haruna=false");
            sw.WriteLine("sp_en=found_cvt");
            sw.Close();
        }
        string path1 = Path.Combine(Application.streamingAssetsPath, "state.txt");
        using (StreamWriter sw = File.CreateText(path1))
        {
            sw.WriteLine("STEP 1=Boot=false");
            sw.WriteLine("error=false");
            sw.Close();
        }
        string path2 = Path.Combine(Application.streamingAssetsPath, "install.txt");
        using (StreamWriter sw = File.CreateText(path2))
        {
            sw.WriteLine("");
            sw.Close();
        }
    }

    private IEnumerator checkState()
    {
        while (true)
        {
            if ((videoPlayer.frame) > 0 && (videoPlayer.isPlaying == false))
            {
                Debug.Log("BIOS End");
                transition();
                break;
            }
            yield return new WaitForSeconds(0.125f);
        }
    }

    private void transition()
    {
        if (File.Exists(Path.Combine("C:\\SEGA\\update\\system_update.ps1")))
        {
            Debug.Log("System Update Found");
            SceneManager.LoadScene("LevelUpdate");
        }
        else if (File.Exists(Path.Combine("C:\\SEGA\\system\\PLATFORM_INSTALLED")))
        {
            Debug.Log("Ready to Boot");
            SceneManager.LoadScene("LevelNormal");
        }
        else
        {
            Debug.Log("Platform Installer Triggred");
            SceneManager.LoadScene("LevelPre");
        }
    }
}
