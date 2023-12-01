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

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(checkState());
    }

    // Update is called once per frame
    void Update()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "state.txt");
        using (StreamWriter sw = File.CreateText(path))
        {
            sw.WriteLine("STEP 1=起動しています=true");
            sw.WriteLine("error=false");
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
