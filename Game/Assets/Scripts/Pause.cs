using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject losePanel;

    private void Start()
    {

    }

    public void showPausePanel()
    {
        if (!pausePanel.activeSelf && !losePanel.activeSelf)
        {
            pausePanel.SetActive(true);

        }
        else
        {
            pausePanel.SetActive(false);

        }
        pauseStarter();
    }

    private void pauseStarter()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}
