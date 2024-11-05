using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialPageManager : MonoBehaviour
{
    public void BackToHome()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
