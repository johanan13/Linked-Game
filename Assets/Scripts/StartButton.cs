using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class StartButton: MonoBehaviour
{
    [SerializeField] List<CharacterSelectionScript> characterSelections = new List<CharacterSelectionScript>();
    [SerializeField] TextMeshProUGUI ErrorText;
    public void StartGame()
    {
        foreach (CharacterSelectionScript charS in characterSelections)
        {
            if (string.IsNullOrEmpty(charS.InputField.text))
            {
                ErrorText.text = "Players must input names";
                StartCoroutine(HideErrorMessage());
                return;
            }
        }
        foreach (CharacterSelectionScript charS in characterSelections)
        {
            PlayerData.instance.playerName.Add(charS.InputField.text);
            PlayerData.instance.playerSpriteIndex.Add(charS.spriteIndex);
        }
        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator HideErrorMessage()
    {
        yield return new WaitForSeconds(3);
        ErrorText.text = "";
    }


}