using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] musicTracks; // Array to hold music tracks for each scene
    private AudioSource audioSource;
    private static AudioManager instance;

    void Awake()
    {
        // Ensure that only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the audio source to loop
        audioSource.loop = true;
    }

    void Start()
    {
        PlayMusicForCurrentScene();
    }

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        PlayMusicForCurrentScene();
    }

    void PlayMusicForCurrentScene()
    {
        int sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex < musicTracks.Length && musicTracks[sceneIndex] != null)
        {
            if (audioSource.clip != musicTracks[sceneIndex])
            {
                audioSource.Stop(); // Stop the previous music track
                audioSource.clip = musicTracks[sceneIndex];
                audioSource.Play();
            }
        }
    }
}