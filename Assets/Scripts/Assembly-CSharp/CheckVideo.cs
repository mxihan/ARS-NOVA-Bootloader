using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CheckVideo : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public GameObject transition;
    public bool SkipBoot;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Reset();
        videoPlayer = GetComponent<VideoPlayer>();
        if (SkipBoot) {
            transition.SetActive(true);
        } else {
            StartCoroutine(checkState());
        }
    }

    // Update is called once per frame
    void Reset()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "state.txt");
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine("STEP 1=起動しています= ");
            sw.WriteLine("error=false");
            sw.Close();
        }
        string path1 = Path.Combine(Application.streamingAssetsPath, "install.txt");
        using (StreamWriter sw = File.CreateText(path1))
        {
            sw.WriteLine(" ");
            sw.Close();
        }
    }

    private IEnumerator checkState()
    {
        while (true)
        {
            if ((videoPlayer.frame) > 0 && (videoPlayer.isPlaying == false))
            {
                transition.SetActive(true);
                break;
            }
            yield return new WaitForSeconds(0.125f);
        }
    }
}
