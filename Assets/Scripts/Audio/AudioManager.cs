using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
   public class AudioManager : MonoBehaviour
   {
      public AudioMixer audioMixer;
      [Space]
      public AudioMixerGroup AmbGroup;
      public AudioMixerGroup MusicTrackGroup;
      public AudioMixerGroup SfxGroup;
      [Space] 
      public AudioSource AmbienceAudioSource;
      public AudioSource SfxAudioSource;
      public AudioSource MusicAudioSource;
   
      public List<AudioClipSettings> ambiantSounds;
      public List<AudioClipSettings> musicTracks;
      public List<AudioClipSettings> sfxSounds;

      private EventsBroker eventsBroker;
   
      private void Start()
      {
         eventsBroker = FindObjectOfType<EventsBroker>();
         eventsBroker.SubscribeTo<PlaySoundEvent>(PlaySound);
         PlaySoundsOnStart();
      }

      void PlaySound(PlaySoundEvent playSoundEvent)
      {
         switch (playSoundEvent._soundType)
         {
            case SoundType.Ambience:
               PlayAmbianceSound(playSoundEvent._audioClipName);
               break;
            case SoundType.Sfx:
               PlaySfxSound(playSoundEvent._audioClipName);
               break;
            case SoundType.Music:
               PlayMusicSound(playSoundEvent._audioClipName);
               break;
            case SoundType.PlayAndWait:
               StartCoroutine(PlayAndWait(playSoundEvent._audioClipName));
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }
      }
   
      private AudioSource SetupAudio(string audioClipName, List<AudioClipSettings> audioClipSetupList)
      {
         var audioClipSetup = audioClipSetupList.FirstOrDefault(music => music.audioClip.name == audioClipName);
         if(audioClipSetup == null) throw new Exception($"Audio clip {audioClipName} is missing.");
         var audioSource = audioClipSetup.outputAudioSource;
         audioSource.clip = audioClipSetup.audioClip;
         audioSource.loop = audioClipSetup.loop;
         audioSource.playOnAwake = audioClipSetup.playOnAwake;
         audioSource.volume = 0.5f;
         return audioSource;
      }
   
      // TODO setup wait time coroutine?
      IEnumerator PlayAndWait(string audioClipName)
      {
         var audioClipSetting = sfxSounds.FirstOrDefault(music => music.audioClip.name == audioClipName);
      
         if (audioClipSetting == null) 
         {
            audioClipSetting = musicTracks.FirstOrDefault(music => music.audioClip.name == audioClipName);
         
            if (audioClipSetting == null)
            {
               audioClipSetting = ambiantSounds.FirstOrDefault(music => music.audioClip.name == audioClipName);
            }
         }
         var audioSource = audioClipSetting.outputAudioSource;
         audioSource.Play();
         yield return new WaitWhile( () => audioSource.isPlaying);
      }
   
      void PlayAmbianceSound(string audioClipName)
      {
         AmbienceAudioSource = SetupAudio(audioClipName, ambiantSounds);
         AmbienceAudioSource.Play();
      }

      void PlaySfxSound(string audioClipName)
      {
         SfxAudioSource = SetupAudio(audioClipName, sfxSounds);
         SfxAudioSource.Play();
      }

      void PlayMusicSound(string audioClipName)
      {
         MusicAudioSource = SetupAudio(audioClipName, musicTracks);
         MusicAudioSource.Play();
      }

      void PlaySoundsOnStart()
      {
         foreach (var ambSound in ambiantSounds)
         {
            if (ambSound.playOnAwake)
            {
               if (ambSound.loop) 
                  AmbienceAudioSource.loop = true;
               else
                  AmbienceAudioSource.loop = false;
               AmbienceAudioSource.clip = ambSound.audioClip;
               AmbienceAudioSource.Play();
            }
         }
      
         foreach (var musicTrack in musicTracks)
         {
            if (musicTrack.playOnAwake)
            {
               if (musicTrack.loop)
                  MusicAudioSource.loop = true;
               else
                  MusicAudioSource.loop = false;
               MusicAudioSource.clip = musicTrack.audioClip;
               MusicAudioSource.Play();
            }
         }

         foreach (var sfxSound in sfxSounds)
         {
            if (sfxSound.playOnAwake)
            {
               if (sfxSound.loop)
                  SfxAudioSource.loop = true;
               else
                  SfxAudioSource.loop = false;
               SfxAudioSource.clip = sfxSound.audioClip;
               SfxAudioSource.Play();
            }
         }
      }
   
      [Serializable]
      public class AudioClipSettings
      {
         public AudioClip audioClip;
         public AudioSource outputAudioSource;
         [Range(0.0f,1.0f)]
         public float volume;
         public bool loop;
         public bool playOnAwake;
      }
   }
}
