using System;
using System.Collections.Generic;
using System.Linq;
using EventManagement;
using Events;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
   public AudioMixer audioMixer;
   public AudioMixerGroup AmbGroup;
   public AudioMixerGroup MusicTrackGroup;
   public AudioMixerGroup SfxGroup;
   
   public List<AudioClip> ambianceSounds;
   public List<AudioClip> musicTracks;
   public List<AudioClip> sfxSounds;

   private EventsBroker eventsBroker;
   
   private void Start()
   {
      eventsBroker = FindObjectOfType<EventsBroker>();
      eventsBroker.SubscribeTo<PlaySoundEvent>(PlaySound);
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
         default:
            throw new ArgumentOutOfRangeException();
      }
   }
   
   /*  TODO setup wait time coroutine?
   IEnumerator PlayAndWait(string audioClipName)
   {
      
      yield return new WaitWhile( () => audioSource.isPlaying);
   }*/
   
   void PlayAmbianceSound(string audioClipName)
   {
      var audioSource = SetupAudioSource(audioClipName, ambianceSounds);
      audioSource.outputAudioMixerGroup = AmbGroup;
      audioSource.loop = true;
      audioSource.playOnAwake = false;
      audioSource.Play();
   }

   void PlaySfxSound(string audioClipName)
   {
      //TODO fix values
      var audioSource = SetupAudioSource(audioClipName, sfxSounds);
      audioSource.outputAudioMixerGroup = SfxGroup;
      audioSource.loop = false;
      audioSource.playOnAwake = false;
      audioSource.Play();
   }

   void PlayMusicSound(string audioClipName)
   {
      var audioSource = SetupAudioSource(audioClipName, musicTracks);
      audioSource.outputAudioMixerGroup = MusicTrackGroup;
      audioSource.loop = true;
      audioSource.playOnAwake = false;
      audioSource.Play();
   }

   private AudioSource SetupAudioSource(string audioClipName, List<AudioClip> soundTypeList)
   {
      var audioSource = gameObject.AddComponent<AudioSource>();
      var audioClip = soundTypeList.FirstOrDefault(music => music.name == audioClipName);
      if(audioClip == null) throw new Exception("Could not find audio clip with name: " + audioClipName);
      audioSource.clip = audioClip;
      audioSource.volume = 0.5f;
      return audioSource;
   }
}
