using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Ambient music manager that handles playing the background music tracks.
/// </summary>

public class AmbientMusicManager : MonoBehaviour
{

    [Header("----- Music Settings -----")]
    [Tooltip("List of ambient music tracks to play")]
    [SerializeField] List<AudioClip> musicTracks = new List<AudioClip>();

    [Tooltip("Volume of background music")]
    [SerializeField][Range(0f, 1f)] float musicVolume = 0.5f;

    [Tooltip("Should the music tracks play in order?")]
    [SerializeField] bool randomizeOrder = false;

    [Tooltip("Should there be silence in between tracks?")]
    [SerializeField] bool silenceBetweenTracks = false;

    [Tooltip("Minimum amount of silence between tracks (in seconds)")]
    [SerializeField][Range(0f, 30f)] float minSilenceDuration = 5f;

    [Tooltip("Maximum amount of silence between tracks (in seconds)")]
    [SerializeField][Range(0f, 30f)] float maxSilenceDuration = 10f;

    [Tooltip("Fade duration when transitioning between tracks (in seconds)")]
    [SerializeField][Range(0f, 10f)] float crossfadeDuration = 3f;

    [Header("----- Scene Transition -----")]
    [Tooltip("Duration of fade out when leaving scene")]
    [SerializeField][Range(0f, 5f)] float sceneTransitionFadeOut = 1.5f;

    private AudioSource musicSource1;
    private AudioSource musicSource2;
    private int currentTrackIndex = 0;
    private bool isSource1Active = true;
    private Coroutine musicCoroutine;


    private void Awake()
    {
        // Set up audio for crossfading
        musicSource1 = gameObject.AddComponent<AudioSource>();
        musicSource2 = gameObject.AddComponent<AudioSource>();

        SetupAudioSource(musicSource1);
        SetupAudioSource(musicSource2);

        // Scene change events
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Start playing music if tracks are assigned
        if (musicTracks.Count > 0)
        {
            if (randomizeOrder)
            {
                ShuffleMusicTracks();
            }
            StartMusicSequence();
        }
    }

    void SetupAudioSource(AudioSource source)
    {
        source.loop = false;
        source.playOnAwake = false;
        source.volume = 0;
        source.spatialBlend = 0;
    }

    void ShuffleMusicTracks()
    {
        for (int i = musicTracks.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            AudioClip temp = musicTracks[i];
            musicTracks[i] = musicTracks[j];
            musicTracks[j] = temp;
        }
    }

    void StartMusicSequence()
    { 
        if(musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine);
        }

        musicCoroutine = StartCoroutine(PlayMusicSequence());
    }

    IEnumerator PlayMusicSequence()
    {
        while (true)
        { 
            // Play the current music track
            AudioClip currentTrack = musicTracks[currentTrackIndex];
            AudioSource activeSource = isSource1Active ? musicSource1 : musicSource2;
            AudioSource inactiveSource = isSource1Active ? musicSource2 : musicSource1;

            // Stop the inactive source
            inactiveSource.Stop();

            // Set up and the current track
            activeSource.clip = currentTrack;
            activeSource.Play();
            
            // Fade in
            yield return StartCoroutine(FadeAudioSource(activeSource, 0, musicVolume, crossfadeDuration));

            // Wait for the track to finish
            float trackDuration = currentTrack.length;
            float timeRemaining = trackDuration - crossfadeDuration;

            // Wait until near the end of the track
            if(timeRemaining > 0)
            {
                yield return new WaitForSeconds(timeRemaining);
            }

            // Move to the next track
            currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Count;

            // Have silence between tracks (optional)
            if (silenceBetweenTracks)
            {
                // Fade Out
                yield return StartCoroutine(FadeAudioSource(activeSource, musicVolume, 0, crossfadeDuration));

                // Wait in silence
                float silenceDuration = Random.Range(minSilenceDuration, maxSilenceDuration);
                yield return new WaitForSeconds(silenceDuration);
            }

            // Switch active source for next track
            isSource1Active = !isSource1Active;
        }
    }

    IEnumerator FadeAudioSource(AudioSource source, float startVolume, float targetVolume, float duration)
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        source.volume = targetVolume;
    }

    // This is called when entering a new scene
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Start playing music for this scene
        if(musicTracks.Count > 0)
        {
            if (randomizeOrder)
            {
                ShuffleMusicTracks();
            }
            StartMusicSequence();
        }
    }

    // This is called when leaving a scene
    void OnSceneUnloaded(Scene scene)
    {
        // Fade out music and stop playback
        StopAllCoroutines();
        StartCoroutine(FadeOutMusic());
    }

    IEnumerator FadeOutMusic()
    {
        // Fade out both audio sources
        StartCoroutine(FadeAudioSource(musicSource1, musicSource1.volume, 0, sceneTransitionFadeOut));
        yield return StartCoroutine(FadeAudioSource(musicSource2, musicSource2.volume, 0, sceneTransitionFadeOut));

        // Stop both audio sources
        musicSource1.Stop();
        musicSource2.Stop();
    }

    public void StopMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusic());
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        AudioSource activeSource = isSource1Active ? musicSource1 : musicSource2;
        StartCoroutine(FadeAudioSource(activeSource, activeSource.volume, musicVolume, 1.0f));
    }
}
