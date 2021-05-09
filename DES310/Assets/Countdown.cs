using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{

    public RawImage countdown;
    private float time;
    public float startTime;
    private int textureCounter = 0;
    public CameraHandler cameraHandler;
    public List<Texture> textures;
    private GameManager manager;

    // Start is called before the first frame update
    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    private void Update()
    {
        if (manager.state == MatchState.countdown)
        {
            cameraHandler.ChangeCameraPosMatch();
            time -= Time.deltaTime;
            countdown.texture = textures[textureCounter];
            countdown.gameObject.SetActive(true);

            if (time <= 0)
            {
                time = 0;
                manager.state = MatchState.setUp;
            }
            else if (time <= 1)
            {
                textureCounter = 3;
            }
            else if (time <= 2)
            {
                textureCounter = 2;
            }
            else if(time <= 3)
            {
                textureCounter = 1;
            }
            else
            {
                textureCounter = 0;
            }

        }
        else
        {
            time = startTime;
            countdown.gameObject.SetActive(false);
        }
    }
    //private void CountdownState()
    //{
    //    cameraHandler.ChangeCameraPosMatch();
    //    StartCoroutine(CountdownToStart());
    //}

    //IEnumerator CountdownToStart()
    //{
    //    while(time > 0)
    //    {
    //        countdown.texture = textures[textureCount];

    //        yield return new WaitForSeconds(1f);

    //        time--;
    //        textureCount++;
    //    }

    //    countdown.gameObject.SetActive(false);
    //    manager.state = MatchState.setUp;

    //}
}
