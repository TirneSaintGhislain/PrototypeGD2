using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    [SerializeField]
    private List<Material> _materialOptions = new List<Material>(); // List of materials for each player

    private List<GameObject> _allPlayers = new List<GameObject>();

    [SerializeField]
    private GameObject playerModel;
    [HideInInspector]
    public int _playerIndex;

    // Start is called before the first frame update
    void Awake()
    {

        // Makes a list of all the players in the game
        _allPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        _playerIndex = _allPlayers.Count -1;

        // Check if the player index is within the bounds of the material options
        if (_playerIndex >= 0 && _playerIndex < _materialOptions.Count)
        {
            // Iterate through all child GameObjects and change the material for each renderer
            foreach (Transform childTransform in playerModel.transform)
            {
                Renderer childRenderer = childTransform.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    childRenderer.material = _materialOptions[_playerIndex];
                }
            }

            // Also set the default material in HitstunSystem
            Material defaultMaterial = _materialOptions[_playerIndex];
            GetComponent<HitstunSystem>()._defaultMaterial = defaultMaterial;
        }
        else
        {
            Debug.LogWarning("Player index out of bounds for material options.");
        }
    }
}
