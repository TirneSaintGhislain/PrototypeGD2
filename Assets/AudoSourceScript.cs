using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudoSourceScript : MonoBehaviour
{
    public float _destroyTime;
    public AudioClip _audioClip;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().clip = _audioClip;
        GetComponent<AudioSource>().Play();
        StartCoroutine(DestroyAudioSource());
    }

    private IEnumerator DestroyAudioSource()
    {
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }
}
