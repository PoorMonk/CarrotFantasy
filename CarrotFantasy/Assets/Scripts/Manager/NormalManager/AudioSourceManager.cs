using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager 
{
    public AudioSource[] audioSources; // 0:播放BG；1：播放特效
    private bool m_playEffectMusic = true;
    private bool m_playBGMusic = true;
	
    public AudioSourceManager()
    {
        audioSources = GameManager.Instance.GetComponents<AudioSource>();
    }

    // 切换BG音乐
    public void PlayBGMusic(AudioClip clip)
    {
        if (audioSources[0].isPlaying || audioSources[0].clip != clip)
        {
            audioSources[0].clip = clip;
            audioSources[0].Play();
        }
    }

    public void PlayEffectMusic(AudioClip clip)
    {
        if (m_playEffectMusic)
        {
            audioSources[1].PlayOneShot(clip);
        }
    }

    public void CloseBGMusic()
    {
        audioSources[0].Stop();
    }

    public void OpenBGMusic()
    {
        audioSources[0].Play();
    }

    public void CloseOrOpenBGMusic()
    {
        m_playBGMusic = !m_playBGMusic;
        if (m_playBGMusic)
        {
            OpenBGMusic();
        }
        else
        {
            CloseBGMusic();
        }
    }

    public void CloseOrOpenEffectMusic()
    {
        m_playEffectMusic = !m_playEffectMusic;
    }

    public void PlayButtonAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.m_factoryManager.m_audioClipFactory.GetSingleResource("Main/Button"));
    }

    public void PlayPagingAudioClip()
    {
        PlayEffectMusic(GameManager.Instance.m_factoryManager.m_audioClipFactory.GetSingleResource("Main/Paging"));
    }
}
