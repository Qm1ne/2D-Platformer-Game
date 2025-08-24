using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Button StartButton;
    [SerializeField] public Button ExitButton;
    void Awake()
    {
        StartButton.onClick.AddListener(PlayGame);
        ExitButton.onClick.AddListener(Application.Quit);

    }

    private void PlayGame()
    {
        SceneManager.LoadScene("level1");
        SoundManager.Instance.PlaySound(Sounds.buttonClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
