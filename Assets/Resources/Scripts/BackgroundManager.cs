using UnityEngine;
using System.Collections;

public class BackgroundManager : MonoBehaviour
{

    public int rowNum;
    public int colNum;

    public static BackgroundManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        LoadSounds();

        CreateBG();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateBG()
    {
        GameObject bgPrefab = Resources.Load<GameObject>("Prefabs/BGTile");
        GameObject parent = GameObject.Find("Background");
        Vector2 size = bgPrefab.GetComponent<SpriteRenderer>().bounds.size;

        float startX = (size.x * colNum * -.5f) + size.x/2;
        float startY = (size.y * rowNum *.5f) - size.y/2;
        float currX = startX;
        float currY = startY;

        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                GameObject bg = ((GameObject)Instantiate(bgPrefab, Vector2.zero, Quaternion.identity));
                bg.transform.position = new Vector2(currX, currY);
                bg.transform.SetParent(parent.transform);
                currX += size.x;
            }
            currY -= size.y;
            currX = startX;
        }
    }

    AudioClip gunSound = null;
    GameObject gunSoundSource;
    AudioSource ss;

    AudioClip hurtSound = null;
    GameObject hurtSoundSource;
    AudioSource ss2;

    AudioClip fhurtSound = null;
    GameObject fhurtSoundSource;
    AudioSource ss3;

    AudioClip explodeSound = null;
    GameObject explodeSoundSource;
    AudioSource ss4;
    AudioClip spawnSound1 = null;
    AudioClip spawnSound2 = null;
    AudioClip spawnSound3 = null;
    AudioClip spawnSound4 = null;
    AudioClip spawnSound5 = null;
    AudioClip spawnSound6 = null;
    GameObject spawnSoundSource;
    AudioSource ss5;
    AudioClip gunSound2 = null;
    GameObject gunSoundSource2;
    AudioSource ss6;
    private void LoadSounds()
    {
        if (gunSound == null)
        {
            gunSoundSource = new GameObject();
            gunSoundSource.name = "gunSoundSource";
            ss = gunSoundSource.AddComponent<AudioSource>();
            gunSound = Resources.Load("Sounds/MachineGun1") as AudioClip;
            ss.clip = gunSound;
            ss.loop = false;
            ss.rolloffMode = AudioRolloffMode.Linear;
            ss.volume = Util.SFXVolume;
        }

        if (hurtSound == null)
        {
            hurtSoundSource = new GameObject();
            hurtSoundSource.name = "hurtSoundSource3";
            ss2 = hurtSoundSource.AddComponent<AudioSource>();
            hurtSound = Resources.Load("Sounds/DinoAttack") as AudioClip;
            ss2.clip = hurtSound;
            ss2.loop = false;
            ss2.rolloffMode = AudioRolloffMode.Linear;
            ss2.volume = Util.SFXVolume;
        }

        if (fhurtSound == null)
        {
            fhurtSoundSource = new GameObject();
            fhurtSoundSource.name = "fhurtSoundSource";
            ss3 = fhurtSoundSource.AddComponent<AudioSource>();
            fhurtSound = Resources.Load("Sounds/DinoHurt") as AudioClip;
            ss3.clip = hurtSound;
            ss3.loop = false;
            ss3.rolloffMode = AudioRolloffMode.Linear;
            ss3.volume = Util.SFXVolume;
        }
        if (explodeSound == null)
        {
            explodeSoundSource = new GameObject();
            explodeSoundSource.name = "explodeSoundSource";
            ss4 = explodeSoundSource.AddComponent<AudioSource>();
            explodeSound = Resources.Load("Sounds/Explosion5") as AudioClip;
            ss4.clip = explodeSound;
            ss4.loop = false;
            ss4.rolloffMode = AudioRolloffMode.Linear;
            ss4.volume = Util.SFXVolume;
        }
        if (spawnSound1 == null)
        {
            spawnSoundSource = new GameObject();
            spawnSoundSource.name = "spawnSoundSource";
            ss5 = spawnSoundSource.AddComponent<AudioSource>();
            spawnSound1 = Resources.Load("Sounds/DinoScream1") as AudioClip;
            spawnSound2 = Resources.Load("Sounds/DinoScream3") as AudioClip;
            spawnSound3 = Resources.Load("Sounds/DinoScream2") as AudioClip;
            spawnSound4 = Resources.Load("Sounds/DinoScream4") as AudioClip;
            spawnSound5 = Resources.Load("Sounds/DinoScream5") as AudioClip;
            spawnSound6 = Resources.Load("Sounds/DinoScream6") as AudioClip;

            ss5.loop = false;
            ss5.rolloffMode = AudioRolloffMode.Linear;
            ss5.volume = Util.SFXVolume;
        }

        if (gunSound2 == null)
        {
            gunSoundSource2 = new GameObject();
            gunSoundSource2.name = "gunSoundSource2";
            ss6 = gunSoundSource2.AddComponent<AudioSource>();
            gunSound2 = Resources.Load("Sounds/Gunshot1") as AudioClip;
            ss6.clip = gunSound;
            ss6.loop = false;
            ss6.rolloffMode = AudioRolloffMode.Linear;
            ss6.volume = Util.SFXVolume;
        }
    }

    public void PlaySound(int sound)
    {
        switch (sound)
        {
            case 0:
                if (!ss.isPlaying)
                {
                    ss.Play();
                }
                break;
            case 1:
                if (!ss2.isPlaying)
                {
                    ss2.Play();
                }
                break;
            case 2:
                if (!ss3.isPlaying)
                {
                    ss3.Play();
                }
                break;
            case 3:
                if (!ss4.isPlaying)
                {
                    ss4.Play();
                }
                break;
            case 4:
                ss5.clip = spawnSound1;
                ss5.Play();
                break;
            case 5:
                ss5.clip = spawnSound2;
                ss5.Play();
                break;
            case 6:
                ss5.clip = spawnSound3;
                ss5.Play();
                break;
            case 7:
                ss5.clip = spawnSound4;
                ss5.Play();
                break;
            case 8:
                ss5.clip = spawnSound5;
                ss5.Play();
                break;
            case 9:
                ss5.clip = spawnSound6;
                ss5.Play();
                break;
            case 10:
                if (!ss6.isPlaying)
                {
                    ss6.Play();
                }
                break;
        }

    }
}
