namespace Events
{
    public class PlaySoundEvent
    {
        public readonly SoundType _soundType;
        public readonly string _audioClipName;

        public PlaySoundEvent(SoundType soundType, string audioClipName)
        {
            _soundType = soundType;
            _audioClipName = audioClipName;
        }
    }

    public enum SoundType
    {
        Ambience,
        Sfx,
        Music,
        PlayAndWait
    }
}