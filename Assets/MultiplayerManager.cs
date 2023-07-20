using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    [SerializeField]
    private List<Color> _colorOptions = new List<Color>();

    [SerializeField]
    private List<LayerMask> _hurtboxLayers = new List<LayerMask>();

    [SerializeField]
    private int _playerAmount;

    private List<GameObject> _allPlayers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //Makes a list of all the players in the game
        _allPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        int playerIndex = _allPlayers.Count - 1;

        GetComponent<Renderer>().material.color = _colorOptions[playerIndex];

        string layerName = "Player" + (playerIndex + 1);
        gameObject.layer = LayerMask.NameToLayer(layerName);
        
    }
}
