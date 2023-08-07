using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    [SerializeField]
    private AudioClip _lightAttack;
    [SerializeField]
    private AudioClip _heavyAttack;
    [SerializeField]
    private AudioClip _dashAttack;
    [SerializeField]
    private AudioClip _hurtSound;
    [SerializeField]
    private AudioClip _evolvedSound;
    [SerializeField]
    private AudioClip _devolvedSound;

    //The time before a clip can be freely interrupted
    [SerializeField]
    private float _interruptionTime;
    private float _interruptionTimer;

    public void PlayLightAttack()
    {
        PlayAudio(_lightAttack);
    }

    public void PlayHeavyAttack()
    {
        PlayAudio(_heavyAttack);
    }

    public void PlayDashAttack()
    {
        PlayAudio(_dashAttack);
    }

    public void PlayHurtSound()
    {
        PlayAudio(_hurtSound);
    }

    public void PlayEvolvedSound()
    {
        PlayAudio(_evolvedSound);
    }

    public void PlayDevolvedSound()
    {
        PlayAudio(_devolvedSound);
    }

    private void Update()
    {
        //This allows the script to freely interrupt a sound effect if i'ts had the chance to player for a while
        if (GetComponent<AudioSource>().isPlaying)
        {
            _interruptionTimer += Time.deltaTime;
        }
        if (_interruptionTimer > _interruptionTime)
        {
            GetComponent<AudioSource>().Stop();
            _interruptionTimer = 0;
        }
    }

    private void PlayAudio(AudioClip audioClip)
    {
        if (audioClip == _evolvedSound)
        {
            GetComponent<AudioSource>().clip = audioClip;
            GetComponent<AudioSource>().Play();
        }
        if (!GetComponent<AudioSource>().isPlaying || audioClip == _hurtSound)
        {
            GetComponent<AudioSource>().clip = audioClip;
            GetComponent<AudioSource>().Play();
        }
    }
}
