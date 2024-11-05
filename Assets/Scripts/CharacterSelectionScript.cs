using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CharacterSelectionScript : MonoBehaviour
{
    public List<GameObject> sprites = new List<GameObject>();
    public TMP_InputField InputField;
    public int spriteIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        setSprite();
    }
    public void Next()
    {
        spriteIndex++;
        if (spriteIndex >= sprites.Count) spriteIndex = 0;
        setSprite();
    }
    public void Prev()
    {
        spriteIndex--;
        if (spriteIndex < 0) spriteIndex = 1;
        setSprite();
    }
    public void setSprite()
    {
        int counter = 0;
        foreach (GameObject sprite in sprites)
        {
            sprite.SetActive(false);
            if (counter == spriteIndex) sprite.SetActive(true);
            counter++;
        }
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}