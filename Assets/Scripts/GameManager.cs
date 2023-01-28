using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void ResetGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
