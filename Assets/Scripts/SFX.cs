using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

//Svi zvukovi na jednom mjestu. Prefab je u Resources/SFX i sam se stvori kad se pokrene bilo koja scena.
//Poziva se iz bilo koje skripte: SFX.Play(SFX.Instance.Coins) ili SFX.PlayAt(SFX.Instance.ZombieAttack, pozicija)
public class SFX : MonoBehaviour
{
    public static SFX Instance;

    [Header("UI")]
    public AudioClip[] Click;
    public AudioClip[] OpenWindow;
    public AudioClip[] Coins;
    public AudioClip[] DrinkPotion;

    [Header("Player")]
    public AudioClip[] Footsteps;
    public AudioClip[] LightSwing;
    public AudioClip[] HeavySwing;
    public AudioClip[] PlayerHurt;
    public AudioClip[] PlayerDeath;

    [Header("Enemy")]
    public AudioClip[] EnemyHit;      //kad player pogodi neprijatelja
    public AudioClip[] ZombieAttack;
    public AudioClip[] ZombieGrowl;
    public AudioClip[] ZombieAggro;   //kad ugleda playera
    public AudioClip[] ZombieDeath;

    [Range(0, 1)] public float Volume = 1;
    public float MinDistance = 12;  //kamera je visoko pa 3D zvuk mora imati veliki min distance
    public float MaxDistance = 45;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Boot()
    {
        if (Instance != null) return;
        GameObject prefab = Resources.Load<GameObject>("SFX");
        if (prefab == null)
        {
            Debug.LogWarning("Nema Resources/SFX prefaba, nema zvuka");
            return;
        }
        GameObject go = Instantiate(prefab);
        go.name = "SFX";
        DontDestroyOnLoad(go);
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        source2D = gameObject.AddComponent<AudioSource>();
        source2D.playOnAwake = false;
        source2D.spatialBlend = 0;
    }

    private AudioSource source2D;

    //glasnoca iz settingsa (Settings sprema u PlayerPrefs)
    static float sfxVolume()
    {
        return PlayerPrefs.GetFloat("sfx", 70) / 100f * Instance.Volume;
    }

    static AudioClip random(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;
        return clips[Random.Range(0, clips.Length)];
    }

    //2D zvuk (UI)
    public static void Play(AudioClip[] clips, float volume = 1)
    {
        if (Instance == null) return;
        AudioClip clip = random(clips);
        if (clip == null) return;
        Instance.source2D.pitch = 1;
        Instance.source2D.PlayOneShot(clip, volume * sfxVolume());
    }

    //3D zvuk na poziciji (koraci, udarci, zombiji)
    public static void PlayAt(AudioClip[] clips, Vector3 position, float volume = 1, float pitchRandom = 0.08f)
    {
        if (Instance == null) return;
        AudioClip clip = random(clips);
        if (clip == null) return;
        GameObject go = new GameObject("SFX_" + clip.name);
        go.transform.position = position;
        AudioSource s = go.AddComponent<AudioSource>();
        s.clip = clip;
        s.volume = volume * sfxVolume();
        s.pitch = 1 + Random.Range(-pitchRandom, pitchRandom);
        s.spatialBlend = 0.8f;
        s.rolloffMode = AudioRolloffMode.Linear;
        s.minDistance = Instance.MinDistance;
        s.maxDistance = Instance.MaxDistance;
        s.dopplerLevel = 0;
        s.Play();
        Destroy(go, clip.length / s.pitch + 0.1f);
    }

    void Update()
    {
        //klik na bilo koji gumb/slot/slider u UI-u
        if (Mouse.current == null || EventSystem.current == null) return;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = Mouse.current.position.ReadValue();
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            if (results.Count > 0)
            {
                Selectable sel = results[0].gameObject.GetComponentInParent<Selectable>();
                if (sel != null && sel.interactable)
                {
                    Play(Click, 0.6f);
                }
            }
        }
    }
}
