using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
public class LevelCloser : MonoBehaviour
{
    [SerializeField] private Image Menu;

    [SerializeField] private PlayerControl playerControl;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;
    private void CloseLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Extract the level number
        int currentLevel = int.Parse(currentSceneName.Replace("Level", ""));

        // Calculate next level
        int nextLevel = currentLevel + 1;

        // Check if next level exists (assuming we have 3 levels)
        if (nextLevel <= 3)
        {
            Debug.Log($"Loading Level{nextLevel}...");
            SceneManager.LoadScene($"Level{nextLevel}");
        }
        else
        {
            // Handle game completion (e.g., load end screen or return to menu)
            Debug.Log("Game Completed!");
            // Optionally load a completion scene
            // SceneManager.LoadScene("GameComplete");
        }
    }
    private void Death()
    {
        // Logic to reset the level, e.g., reload the current scene or reset player position
        Debug.Log("showing  Menu...");
        if (playerControl.IsDead)
        {
            SoundManager.Instance.PlaySound(Sounds.PlayerDeath);
            StartCoroutine(ShowMenuWithDelay());
        }
    }
    private IEnumerator ShowMenuWithDelay()
    {
        Debug.Log("Waiting to show menu...");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Showing Menu...");
        Menu.gameObject.SetActive(true);
        
    }
    void LateUpdate()
    {
        Death();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControl>() != null)
        {
            // Optionally, you can trigger the level close immediately when the player enters a specific area
            if (playerControl.HaveKey)
            {
                CloseLevel();
            }
            else
            {
                Debug.Log("You need a key to proceed to the next level!");
                CameraShake(10f, 0.2f);
            }
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
#if UNITY_EDITOR
        // Stop Play mode in the Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application in a build
        Application.Quit();
#endif
    }
    public void CameraShake(float intensity, float duration)
    {
        virtualCamera.GetComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
        StartCoroutine(ResetCameraShakeAfterDelay(duration));
    }

    private string ResetCameraShakeAfterDelay(float duration)
    {
        throw new NotImplementedException();
    }
}
