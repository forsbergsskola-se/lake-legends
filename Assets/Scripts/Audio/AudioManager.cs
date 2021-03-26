using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
         SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
      }

      private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
      {
         CheckSoundsToPlayOnSceneChange(ambiantSounds, AmbienceAudioSource, arg0.name);
         CheckSoundsToPlayOnSceneChange(sfxSounds, SfxAudioSource, arg0.name);
         CheckSoundsToPlayOnSceneChange(musicTracks, MusicAudioSource, arg0.name);
      }

      public void PlaySound(PlaySoundEvent playSoundEvent)
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
      
      public IEnumerator PlaySoundEnumerator(PlaySoundEvent playSoundEvent)
      {
         switch (playSoundEvent._soundType)
         {
            case SoundType.PlayAndWait:
               yield return PlayAndWait(playSoundEvent._audioClipName);
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }
      }
   
      private AudioSource SetupAudio(string audioClipName, List<AudioClipSettings> audioClipSetupList)
      {
         var audioClipSetup = audioClipSetupList.FirstOrDefault(music => music.eventName == audioClipName);
         if(audioClipSetup == null) throw new Exception($"Audio clip {audioClipName} is missing.");
         var audioSource = audioClipSetup.outputAudioSource;
         audioSource.clip = audioClipSetup.audioClip;
         audioSource.loop = audioClipSetup.loop;
         audioSource.playOnAwake = false;
         audioSource.volume = 0.5f;
         return audioSource;
      }
      
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
         audioSource.clip = audioClipSetting.audioClip;
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

      void CheckSoundsToPlayOnSceneChange(List<AudioClipSettings> audioClipSettings, AudioSource audioSource, string sceneName)
      {
         foreach (var audioClipSetting in audioClipSettings)
         {
            if (audioClipSetting.scenesToPlayIn.Find(sceneToPlayIn => sceneToPlayIn.Contains(sceneName)) == sceneName)
            {
               if (audioClipSetting.loop)
                  audioSource.loop = true;
               else
                  audioSource.loop = false;
               if (audioClipSetting.playOnSceneChange)
               {
                  audioSource.clip = audioClipSetting.audioClip;
                  audioSource.Play();
               }
            }
            else if(audioClipSetting.audioClip == audioSource.clip)
            {
               audioSource.Stop();
            }
         }
      }
   
      [Serializable]
      public class AudioClipSettings
      {
         public string eventName;
         public AudioClip audioClip;
         public AudioSource outputAudioSource;
         [Range(0.0f,1.0f)]
         public float volume;
         public bool loop;
         public bool playOnSceneChange;
         public List<string> scenesToPlayIn;
      }
   }
}
