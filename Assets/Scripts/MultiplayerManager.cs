using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    [SerializeField]
    private List<Color> _colorOptions = new List<Color>();

    private static int _currentColorIndex = 0;
    private Renderer[] _allPlayerRenderers;

    // Start is called before the first frame update
    void Start()
    {
        // Get all the Renderer components attached to this GameObject
        _allPlayerRenderers = GetComponentsInChildren<Renderer>();

        // Set the default color for all player renderers
        int colorIndex = _currentColorIndex % _colorOptions.Count;
        for (int i = 0; i < _allPlayerRenderers.Length; i++)
        {
            _allPlayerRenderers[i].material.color = _colorOptions[colorIndex];
            HitstunSystem hitstunSystem = _allPlayerRenderers[i].GetComponent<HitstunSystem>();
            if (hitstunSystem != null)
            {
                hitstunSystem._defaultColor = _colorOptions[colorIndex];
            }
        }

        // Increment the current color index for the next instance of MultiplayerManager
        _currentColorIndex++;
    }
}
