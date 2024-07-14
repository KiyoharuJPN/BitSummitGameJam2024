using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct AudioData
{
    public string name;
    public AudioClip audioClip;
}


public class SoundManager : MonoBehaviour
{
    // １つだけにするため静的インスタンスにする
    public static SoundManager instance;
    // あらゆるBGMとSE
    public AudioData[] BgmDatas, SeDatas;
    // 音を鳴らす用AudioSource
    [SerializeField]
    AudioSource BGM_Source, SE_Source;
    //別名(name)をキーとした管理用Dictionary
    Dictionary<string, AudioData> BGMDictionary = new Dictionary<string, AudioData>(), SEDictionary = new Dictionary<string, AudioData>();


    private void Awake()
    {
        // インスタンスを作る
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(gameObject.GetComponentInParent<Transform>().gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Dictionaryに音を追加する（あとはDictionaryにある音だけならせられる）
        foreach (var bgm  in BgmDatas)
        {
            BGMDictionary.Add(bgm.name, bgm);
        }
        foreach(var se in SeDatas)
        {
            SEDictionary.Add(se.name, se);
        }
    }


    // 内部関数



    // 外部関数
    // 音声を流す
    public void PlayBGM(string bgmname)
    {
        if(BGMDictionary.TryGetValue(bgmname, out var bgm))
        {
            if (BGM_Source.isPlaying) BGM_Source.Stop();
            BGM_Source.clip = bgm.audioClip;
            BGM_Source.Play();
        }
        else
        {
            Debug.Log("想定外の値が入力された");
        }
    }
    public void PlaySE(string sename)
    {
        if (SEDictionary.TryGetValue(sename, out var bgm))
        {
            SE_Source.clip = bgm.audioClip;
            SE_Source.Play();
        }
        else
        {
            Debug.Log("想定外の値が入力された");
        }
    }
    // 音を止める
    public void StopBGM()
    {
        BGM_Source.Stop();
    }
    //public void StopSE(string sename)
    //{

    //}
    // 音を一時停止
    public void PauseBGM()
    {
        BGM_Source.Pause();
    }
    //public void PauseSE()
    //{

    //}
    // 音を再開する
    public void UnpauseBGM()
    {
        BGM_Source.UnPause();
    }
    //public void UnpauseSE()
    //{

    //}
    // 音量調整をする
    public void SetBGMVolume(float volume)
    {
        BGM_Source.volume = volume;
    }
    public void SetSEVolume(float volume)
    {
        SE_Source.volume = volume;
    }


}






















//using System.Collections.Generic;
//using UnityEngine;

//public class SoundManager : MonoBehaviour
//{
//    public static SoundManager Instance;

//    [System.Serializable]
//    public class Sound
//    {
//        public string name;
//        public AudioClip clip;
//        [Range(0f, 1f)]
//        public float volume = 1f;
//        public bool loop = false;
//    }

//    public Sound[] sounds;

//    private Dictionary<string, AudioSource> soundDictionary = new Dictionary<string, AudioSource>();

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }

//        foreach (var sound in sounds)
//        {
//            AudioSource source = gameObject.AddComponent<AudioSource>();
//            source.clip = sound.clip;
//            source.volume = sound.volume;
//            source.loop = sound.loop;
//            soundDictionary[sound.name] = source;
//        }
//    }

//    public void Play(string name)
//    {
//        if (soundDictionary.TryGetValue(name, out var source))
//        {
//            source.Play();
//        }
//        else
//        {
//            Debug.LogWarning($"Sound {name} not found!");
//        }
//    }

//    public void Pause(string name)
//    {
//        if (soundDictionary.TryGetValue(name, out var source))
//        {
//            source.Pause();
//        }
//        else
//        {
//            Debug.LogWarning($"Sound {name} not found!");
//        }
//    }

//    public void Stop(string name)
//    {
//        if (soundDictionary.TryGetValue(name, out var source))
//        {
//            source.Stop();
//        }
//        else
//        {
//            Debug.LogWarning($"Sound {name} not found!");
//        }
//    }

//    public void SetLoop(string name, bool loop)
//    {
//        if (soundDictionary.TryGetValue(name, out var source))
//        {
//            source.loop = loop;
//        }
//        else
//        {
//            Debug.LogWarning($"Sound {name} not found!");
//        }
//    }

//    public void SetVolume(string name, float volume)
//    {
//        if (soundDictionary.TryGetValue(name, out var source))
//        {
//            source.volume = volume;
//        }
//        else
//        {
//            Debug.LogWarning($"Sound {name} not found!");
//        }
//    }

//    public void ChangeMusic(AudioClip newMusic)
//    {
//        foreach (var sound in soundDictionary)
//        {
//            if (sound.Value.loop)
//            {
//                sound.Value.clip = newMusic;
//                sound.Value.Play();
//                break;
//            }
//        }
//    }
//}




////[System.Serializable]
////public class SoundData
////{
////    public string name;
////    public AudioClip audioClip;
////}

////[SerializeField]
////private SoundData[] soundDatas;

//////AudioSource（スピーカー）を同時に鳴らしたい音の数だけ用意
////private AudioSource[] audioSourceList = new AudioSource[20];

//////別名(name)をキーとした管理用Dictionary
////private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();

////private void Awake()
////{
////    //auidioSourceList配列の数だけAudioSourceを自分自身に生成して配列に格納
////    for (var i = 0; i < audioSourceList.Length; ++i)
////    {
////        audioSourceList[i] = gameObject.AddComponent<AudioSource>();
////    }

////    //soundDictionaryにセット
////    foreach (var soundData in soundDatas)
////    {
////        soundDictionary.Add(soundData.name, soundData);
////    }
////}

//////未使用のAudioSourceの取得 全て使用中の場合はnullを返却
////private AudioSource GetUnusedAudioSource()
////{
////    for (var i = 0; i < audioSourceList.Length; ++i)
////    {
////        if (audioSourceList[i].isPlaying == false) return audioSourceList[i];
////    }

////    return null; //未使用のAudioSourceは見つかりませんでした
////}

//////指定されたAudioClipを未使用のAudioSourceで再生
////public void Play(AudioClip clip)
////{
////    var audioSource = GetUnusedAudioSource();
////    if (audioSource == null) return; //再生できませんでした
////    audioSource.clip = clip;
////    audioSource.Play();
////}

//////指定された別名で登録されたAudioClipを再生
////public void Play(string name)
////{
////    if (soundDictionary.TryGetValue(name, out var soundData)) //管理用Dictionary から、別名で探索
////    {
////        Play(soundData.audioClip); //見つかったら、再生
////    }
////    else
////    {
////        Debug.LogWarning($"その別名は登録されていません:{name}");
////    }
////}
















////using System.Collections;
////using UnityEngine;
////using DG.Tweening;
////using UnityEngine.Audio;

/////// <summary>
/////// 音源管理クラス
/////// </summary>
////public class SoundManager : MonoBehaviour
////{
////    public static SoundManager instance;

////    // BGM管理
////    public enum BGM_Type
////    {
////        // BGM用の列挙子をゲームに合わせて登録

////        SILENCE = 999,        // 無音状態をBGMとして作成したい場合には追加しておく。それ以外は不要
////    }

////    // SE管理
////    public enum SE_Type
////    {
////        // SE用の列挙子をゲームに合わせて登録
////    }

////    // クロスフェード時間
////    public const float CROSS_FADE_TIME = 1.0f;

////    // ボリューム関連
////    public float BGM_Volume = 0.1f;
////    public float SE_Volume = 0.2f;
////    public bool Mute = false;

////    // === AudioClip ===
////    public AudioClip[] BGM_Clips;
////    public AudioClip[] SE_Clips;

////    // SE用AudioMixer  未使用
////    public AudioMixer audioMixer;


////    // === AudioSource ===
////    private AudioSource[] BGM_Sources = new AudioSource[2];
////    private AudioSource[] SE_Sources = new AudioSource[16];

////    private bool isCrossFading;

////    private int currentBgmIndex = 999;

////    void Awake()
////    {
////        // シングルトンかつ、シーン遷移しても破棄されないようにする
////        if (instance == null)
////        {
////            instance = this;
////            DontDestroyOnLoad(gameObject);
////        }
////        else
////        {
////            Destroy(gameObject);
////        }

////        // BGM用 AudioSource追加
////        BGM_Sources[0] = gameObject.AddComponent<AudioSource>();
////        BGM_Sources[1] = gameObject.AddComponent<AudioSource>();

////        // SE用 AudioSource追加
////        for (int i = 0; i < SE_Sources.Length; i++)
////        {
////            SE_Sources[i] = gameObject.AddComponent<AudioSource>();
////        }
////    }

////    void Update()
////    {
////        // ボリューム設定
////        if (!isCrossFading)
////        {
////            BGM_Sources[0].volume = BGM_Volume;
////            BGM_Sources[1].volume = BGM_Volume;
////        }

////        foreach (AudioSource source in SE_Sources)
////        {
////            source.volume = SE_Volume;
////        }
////    }

////    /// <summary>
////    /// BGM再生
////    /// </summary>
////    /// <param name="bgmType"></param>
////    /// <param name="loopFlg"></param>
////    public void PlayBGM(BGM_Type bgmType, bool loopFlg = true)
////    {
////        // BGMなしの状態にする場合            
////        if ((int)bgmType == 999)
////        {
////            StopBGM();
////            return;
////        }

////        int index = (int)bgmType;
////        currentBgmIndex = index;

////        if (index < 0 || BGM_Clips.Length <= index)
////        {
////            return;
////        }

////        // 同じBGMの場合は何もしない
////        if (BGM_Sources[0].clip != null && BGM_Sources[0].clip == BGM_Clips[index])
////        {
////            return;
////        }
////        else if (BGM_Sources[1].clip != null && BGM_Sources[1].clip == BGM_Clips[index])
////        {
////            return;
////        }

////        // フェードでBGM開始
////        if (BGM_Sources[0].clip == null && BGM_Sources[1].clip == null)
////        {
////            BGM_Sources[0].loop = loopFlg;
////            BGM_Sources[0].clip = BGM_Clips[index];
////            BGM_Sources[0].Play();
////        }
////        else
////        {
////            // クロスフェード処理
////            StartCoroutine(CrossFadeChangeBMG(index, loopFlg));
////        }
////    }

////    /// <summary>
////    /// BGMのクロスフェード処理
////    /// </summary>
////    /// <param name="index">AudioClipの番号</param>
////    /// <param name="loopFlg">ループ設定。ループしない場合だけfalse指定</param>
////    /// <returns></returns>
////    private IEnumerator CrossFadeChangeBMG(int index, bool loopFlg)
////    {
////        isCrossFading = true;
////        if (BGM_Sources[0].clip != null)
////        {
////            // [0]が再生されている場合、[0]の音量を徐々に下げて、[1]を新しい曲として再生
////            BGM_Sources[1].volume = 0;
////            BGM_Sources[1].clip = BGM_Clips[index];
////            BGM_Sources[1].loop = loopFlg;
////            BGM_Sources[1].Play();
////            BGM_Sources[1].DOFade(1.0f, CROSS_FADE_TIME).SetEase(Ease.Linear);
////            BGM_Sources[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

////            yield return new WaitForSeconds(CROSS_FADE_TIME);
////            BGM_Sources[0].Stop();
////            BGM_Sources[0].clip = null;
////        }
////        else
////        {
////            // [1]が再生されている場合、[1]の音量を徐々に下げて、[0]を新しい曲として再生
////            BGM_Sources[0].volume = 0;
////            BGM_Sources[0].clip = BGM_Clips[index];
////            BGM_Sources[0].loop = loopFlg;
////            BGM_Sources[0].Play();
////            BGM_Sources[0].DOFade(1.0f, CROSS_FADE_TIME).SetEase(Ease.Linear);
////            BGM_Sources[1].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);

////            yield return new WaitForSeconds(CROSS_FADE_TIME);
////            BGM_Sources[1].Stop();
////            BGM_Sources[1].clip = null;
////        }
////        isCrossFading = false;
////    }

////    /// <summary>
////    /// BGM完全停止
////    /// </summary>
////    public void StopBGM()
////    {
////        BGM_Sources[0].Stop();
////        BGM_Sources[1].Stop();
////        BGM_Sources[0].clip = null;
////        BGM_Sources[1].clip = null;
////    }

////    /// <summary>
////    /// SE再生
////    /// </summary>
////    /// <param name="seType"></param>
////    public void PlaySE(SE_Type seType)
////    {
////        int index = (int)seType;
////        if (index < 0 || SE_Clips.Length <= index)
////        {
////            return;
////        }

////        // 再生中ではないAudioSourceをつかってSEを鳴らす
////        foreach (AudioSource source in SE_Sources)
////        {

////            // 再生中の AudioSource の場合には次のループ処理へ移る
////            if (source.isPlaying)
////            {
////                continue;
////            }

////            // 再生中でない AudioSource に Clip をセットして SE を鳴らす
////            source.clip = SE_Clips[index];
////            source.Play();
////            break;
////        }
////    }

////    /// <summary>
////    /// SE停止
////    /// </summary>
////    public void StopSE()
////    {
////        // 全てのSE用のAudioSourceを停止する
////        foreach (AudioSource source in SE_Sources)
////        {
////            source.Stop();
////            source.clip = null;
////        }
////    }

////    /// <summary>
////    /// BGM一時停止
////    /// </summary>
////    public void MuteBGM()
////    {
////        BGM_Sources[0].Stop();
////        BGM_Sources[1].Stop();
////    }

////    /// <summary>
////    /// 一時停止した同じBGMを再生(再開)
////    /// </summary>
////    public void ResumeBGM()
////    {
////        BGM_Sources[0].Play();
////        BGM_Sources[1].Play();
////    }


////    ////* 未使用 *////


////    /// <summary>
////    /// AudioMixer設定
////    /// </summary>
////    /// <param name="vol"></param>
////    public void SetAudioMixerVolume(float vol)
////    {
////        if (vol == 0)
////        {
////            audioMixer.SetFloat("volumeSE", -80);
////        }
////        else
////        {
////            audioMixer.SetFloat("volumeSE", 0);
////        }
////    }
////}