using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterLoader : MonoBehaviour
{
    public GameObject boyPrefab;
    public GameObject girlPrefab;
    public Transform player1Slot;
    public Transform player2Slot;
    public TMP_Text player1NameText;
    public TMP_Text player2NameText;

    [SerializeField] private Camera mainCamera;

    private GameObject player1Character;
    private GameObject player2Character;

    public GameObject magicalLinkPrefab; // Reference to the magical link prefab
    private float linkDistance = 100.0f; // Distance within which the link appears
    public GameObject currentLink; // To keep track of the instantiated link

    // Start is called before the first frame update
    void Start()
    {
        List<string> playerNames = PlayerData.instance.playerName;
        List<int> playerSpriteIndices = PlayerData.instance.playerSpriteIndex;

        mainCamera = Camera.main;

        // Instantiate characters based on player selection
        if (playerSpriteIndices.Count >= 2)
        {
            // Load Player 1 character
            player1Character = InstantiateCharacter(playerSpriteIndices[0]);
            player1Character.transform.SetParent(player1Slot, false);
            player1Character.transform.localPosition = Vector3.zero;

            // Debug logs for Player 1
            // Debug.Log("Player 1 instantiated at: " + player1Character.transform.position);
            // Debug.Log("Player 1 parent slot: " + player1Slot.name);

            // Load Player 2 character
            player2Character = InstantiateCharacter(playerSpriteIndices[1]);
            player2Character.transform.SetParent(player2Slot, false);
            player2Character.transform.localPosition = Vector3.zero;

            // Debug logs for Player 2
            // Debug.Log("Player 2 instantiated at: " + player2Character.transform.position);
            // Debug.Log("Player 2 parent slot: " + player2Slot.name);

            // Set player names in the UI
            player1NameText.text = playerNames[0];
            player2NameText.text = playerNames[1];

            // Set sorting layers and order in layer for player characters
            SetSortingLayerAndOrder(player1Character, "Player", 1);
            SetSortingLayerAndOrder(player2Character, "Player", 1);
        }

        // Set sorting layer and order for background
        GameObject background = GameObject.Find("Background");
        if (background != null)
        {
            SetSortingLayerAndOrder(background, "Background", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerNamePositions();
        UpdateLinkBetweenPlayers();
    }

   private void UpdatePlayerNamePositions()
{
    if (player1Character != null && player2Character != null)
    {
        // Offset above character based on height (adjust as needed)
        float offsetY = 15.5f;

        // Set Player 1 name position
        Vector3 player1WorldPos = player1Character.transform.position + Vector3.up * offsetY;
        player1NameText.transform.position = mainCamera.WorldToScreenPoint(player1WorldPos);

        // Set Player 2 name position
        Vector3 player2WorldPos = player2Character.transform.position + Vector3.up * offsetY;
        player2NameText.transform.position = mainCamera.WorldToScreenPoint(player2WorldPos);
    }
}

    // Helper method to instantiate the correct prefab based on index
    private GameObject InstantiateCharacter(int index)
    {
        GameObject characterPrefab;
        switch (index)
        {
            case 0:
                characterPrefab = boyPrefab;
                break;
            case 1:
                characterPrefab = girlPrefab;
                break;
            default:
                Debug.LogWarning("Invalid character index, defaulting to Boy");
                characterPrefab = boyPrefab;
                break;
        }

        // Instantiate the character at the origin
        GameObject character = Instantiate(characterPrefab);
        character.transform.position = Vector3.zero;

        return character;
    }

    // Helper method to set sorting layer and order in layer
    private void SetSortingLayerAndOrder(GameObject obj, string sortingLayerName, int orderInLayer)
    {
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = orderInLayer;
        }
    }

    // Method to update the link between players
    private void UpdateLinkBetweenPlayers()
    {
        if (player1Character != null && player2Character != null)
        {
            float distance = Vector3.Distance(player1Character.transform.position, player2Character.transform.position);
            //Debug.Log("Distance between players: " + distance); // Log the distance

            if (distance <= linkDistance)
            {
                // If the link does not exist, create it
                if (currentLink == null)
                {
                    currentLink = Instantiate(magicalLinkPrefab);
                    //Debug.Log("Link created!"); // Log when the link is created
                }
                UpdateLinkPosition(distance); // Update the position and scale of the link
            }
            else
            {
                // Destroy the link if players are too far apart
                if (currentLink != null)
                {
                   // Debug.Log("Link destroyed!"); // Log when the link is destroyed
                    Destroy(currentLink);
                    currentLink = null; // Reset currentLink to null
                }
            }
        }
    }

    private void UpdateLinkPosition(float distance)
    {
        if (currentLink != null)
        {
            // Cap the distance to a maximum of 50
            float adjustedDistance = Mathf.Min(distance, 100.0f);

            // Calculate the direction and midpoint between the players
            Vector3 direction = (player2Character.transform.position - player1Character.transform.position).normalized;
            Vector3 midpoint = (player1Character.transform.position + player2Character.transform.position) / 2;

            // Set the link's position at the midpoint
            currentLink.transform.position = midpoint;

            // Scale the link along the x-axis based on the adjusted distance
            currentLink.transform.localScale = new Vector3(adjustedDistance, currentLink.transform.localScale.y, currentLink.transform.localScale.z);

            // Rotate the link to align exactly from Player 1 to Player 2
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentLink.transform.rotation = Quaternion.Euler(0, 0, angle);

            //Debug.Log("Link positioned and rotated with adjusted distance: " + adjustedDistance);
        }
    }
}
