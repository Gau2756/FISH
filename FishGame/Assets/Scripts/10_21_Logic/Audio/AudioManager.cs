public class AudioManager : MonoBehaviour 
{
    public static AudioManager Instance;
    
    [Header("Music")]
    public AudioClip gameplayMusic;
    public AudioClip gameOverMusic;
    
    [Header("SFX")]
    public AudioClip buttonPressEP;
    public AudioClip buttonPressLobbyist;
    public AudioClip starCollect;
    public AudioClip bagelCollect;
    public AudioClip fishDeath;
    public AudioClip outOfEnergy;
    
    private AudioSource musicSource;
    private AudioSource sfxSource;
    
    void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else 
        {
            Destroy(gameObject);
        }
        
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        
        musicSource.loop = true;
    }
    
    public void PlayMusic(AudioClip clip) 
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
    
    public void PlaySFX(AudioClip clip) 
    {
        sfxSource.PlayOneShot(clip);
    }
}