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

    private List<GameObject> _allPlayers;

    // Start is called before the first frame update
    void Awake()
    {
        //Makes a list of all the players in the game
        _allPlayers.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        int playerIndex = _allPlayers.Count - 1;

        GetComponent<Renderer>().material.color = _colorOptions[playerIndex];
        gameObject.layer = _hurtboxLayers[playerIndex];
    }
}
