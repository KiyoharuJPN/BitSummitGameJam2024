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
    // �P�����ɂ��邽�ߐÓI�C���X�^���X�ɂ���
    public static SoundManager instance;
    // ������BGM��SE
    public AudioData[] BgmDatas, SeDatas;
    // ����炷�pAudioSource
    [SerializeField]
    AudioSource BGM_Source, SE_Source;
    //�ʖ�(name)���L�[�Ƃ����Ǘ��pDictionary
    Dictionary<string, AudioData> BGMDictionary = new Dictionary<string, AudioData>(), SEDictionary = new Dictionary<string, AudioData>();


    private void Awake()
    {
        // �C���X�^���X�����
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

        // Dictionary�ɉ���ǉ�����i���Ƃ�Dictionary�ɂ��鉹�����Ȃ点����j
        foreach (var bgm  in BgmDatas)
        {
            BGMDictionary.Add(bgm.name, bgm);
        }
        foreach(var se in SeDatas)
        {
            SEDictionary.Add(se.name, se);
        }
    }


    // �����֐�



    // �O���֐�
    // �����𗬂�
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
            Debug.Log("�z��O�̒l�����͂��ꂽ");
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
            Debug.Log("�z��O�̒l�����͂��ꂽ");
        }
    }
    // �����~�߂�
    public void StopBGM()
    {
        BGM_Source.Stop();
    }
    //public void StopSE(string sename)
    //{

    //}
    // �����ꎞ��~
    public void PauseBGM()
    {
        BGM_Source.Pause();
    }
    //public void PauseSE()
    //{

    //}
    // �����ĊJ����
    public void UnpauseBGM()
    {
        BGM_Source.UnPause();
    }
    //public void UnpauseSE()
    //{

    //}
    // ���ʒ���������
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

//////AudioSource�i�X�s�[�J�[�j�𓯎��ɖ炵�������̐������p��
////private AudioSource[] audioSourceList = new AudioSource[20];

//////�ʖ�(name)���L�[�Ƃ����Ǘ��pDictionary
////private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();

////private void Awake()
////{
////    //auidioSourceList�z��̐�����AudioSource���������g�ɐ������Ĕz��Ɋi�[
////    for (var i = 0; i < audioSourceList.Length; ++i)
////    {
////        audioSourceList[i] = gameObject.AddComponent<AudioSource>();
////    }

////    //soundDictionary�ɃZ�b�g
////    foreach (var soundData in soundDatas)
////    {
////        soundDictionary.Add(soundData.name, soundData);
////    }
////}

//////���g�p��AudioSource�̎擾 �S�Ďg�p���̏ꍇ��null��ԋp
////private AudioSource GetUnusedAudioSource()
////{
////    for (var i = 0; i < audioSourceList.Length; ++i)
////    {
////        if (audioSourceList[i].isPlaying == false) return audioSourceList[i];
////    }

////    return null; //���g�p��AudioSource�͌�����܂���ł���
////}

//////�w�肳�ꂽAudioClip�𖢎g�p��AudioSource�ōĐ�
////public void Play(AudioClip clip)
////{
////    var audioSource = GetUnusedAudioSource();
////    if (audioSource == null) return; //�Đ��ł��܂���ł���
////    audioSource.clip = clip;
////    audioSource.Play();
////}

//////�w�肳�ꂽ�ʖ��œo�^���ꂽAudioClip���Đ�
////public void Play(string name)
////{
////    if (soundDictionary.TryGetValue(name, out var soundData)) //�Ǘ��pDictionary ����A�ʖ��ŒT��
////    {
////        Play(soundData.audioClip); //����������A�Đ�
////    }
////    else
////    {
////        Debug.LogWarning($"���̕ʖ��͓o�^����Ă��܂���:{name}");
////    }
////}
















////using System.Collections;
////using UnityEngine;
////using DG.Tweening;
////using UnityEngine.Audio;

/////// <summary>
/////// �����Ǘ��N���X
/////// </summary>
////public class SoundManager : MonoBehaviour
////{
////    public static SoundManager instance;

////    // BGM�Ǘ�
////    public enum BGM_Type
////    {
////        // BGM�p�̗񋓎q���Q�[���ɍ��킹�ēo�^

////        SILENCE = 999,        // ������Ԃ�BGM�Ƃ��č쐬�������ꍇ�ɂ͒ǉ����Ă����B����ȊO�͕s�v
////    }

////    // SE�Ǘ�
////    public enum SE_Type
////    {
////        // SE�p�̗񋓎q���Q�[���ɍ��킹�ēo�^
////    }

////    // �N���X�t�F�[�h����
////    public const float CROSS_FADE_TIME = 1.0f;

////    // �{�����[���֘A
////    public float BGM_Volume = 0.1f;
////    public float SE_Volume = 0.2f;
////    public bool Mute = false;

////    // === AudioClip ===
////    public AudioClip[] BGM_Clips;
////    public AudioClip[] SE_Clips;

////    // SE�pAudioMixer  ���g�p
////    public AudioMixer audioMixer;


////    // === AudioSource ===
////    private AudioSource[] BGM_Sources = new AudioSource[2];
////    private AudioSource[] SE_Sources = new AudioSource[16];

////    private bool isCrossFading;

////    private int currentBgmIndex = 999;

////    void Awake()
////    {
////        // �V���O���g�����A�V�[���J�ڂ��Ă��j������Ȃ��悤�ɂ���
////        if (instance == null)
////        {
////            instance = this;
////            DontDestroyOnLoad(gameObject);
////        }
////        else
////        {
////            Destroy(gameObject);
////        }

////        // BGM�p AudioSource�ǉ�
////        BGM_Sources[0] = gameObject.AddComponent<AudioSource>();
////        BGM_Sources[1] = gameObject.AddComponent<AudioSource>();

////        // SE�p AudioSource�ǉ�
////        for (int i = 0; i < SE_Sources.Length; i++)
////        {
////            SE_Sources[i] = gameObject.AddComponent<AudioSource>();
////        }
////    }

////    void Update()
////    {
////        // �{�����[���ݒ�
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
////    /// BGM�Đ�
////    /// </summary>
////    /// <param name="bgmType"></param>
////    /// <param name="loopFlg"></param>
////    public void PlayBGM(BGM_Type bgmType, bool loopFlg = true)
////    {
////        // BGM�Ȃ��̏�Ԃɂ���ꍇ            
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

////        // ����BGM�̏ꍇ�͉������Ȃ�
////        if (BGM_Sources[0].clip != null && BGM_Sources[0].clip == BGM_Clips[index])
////        {
////            return;
////        }
////        else if (BGM_Sources[1].clip != null && BGM_Sources[1].clip == BGM_Clips[index])
////        {
////            return;
////        }

////        // �t�F�[�h��BGM�J�n
////        if (BGM_Sources[0].clip == null && BGM_Sources[1].clip == null)
////        {
////            BGM_Sources[0].loop = loopFlg;
////            BGM_Sources[0].clip = BGM_Clips[index];
////            BGM_Sources[0].Play();
////        }
////        else
////        {
////            // �N���X�t�F�[�h����
////            StartCoroutine(CrossFadeChangeBMG(index, loopFlg));
////        }
////    }

////    /// <summary>
////    /// BGM�̃N���X�t�F�[�h����
////    /// </summary>
////    /// <param name="index">AudioClip�̔ԍ�</param>
////    /// <param name="loopFlg">���[�v�ݒ�B���[�v���Ȃ��ꍇ����false�w��</param>
////    /// <returns></returns>
////    private IEnumerator CrossFadeChangeBMG(int index, bool loopFlg)
////    {
////        isCrossFading = true;
////        if (BGM_Sources[0].clip != null)
////        {
////            // [0]���Đ�����Ă���ꍇ�A[0]�̉��ʂ����X�ɉ����āA[1]��V�����ȂƂ��čĐ�
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
////            // [1]���Đ�����Ă���ꍇ�A[1]�̉��ʂ����X�ɉ����āA[0]��V�����ȂƂ��čĐ�
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
////    /// BGM���S��~
////    /// </summary>
////    public void StopBGM()
////    {
////        BGM_Sources[0].Stop();
////        BGM_Sources[1].Stop();
////        BGM_Sources[0].clip = null;
////        BGM_Sources[1].clip = null;
////    }

////    /// <summary>
////    /// SE�Đ�
////    /// </summary>
////    /// <param name="seType"></param>
////    public void PlaySE(SE_Type seType)
////    {
////        int index = (int)seType;
////        if (index < 0 || SE_Clips.Length <= index)
////        {
////            return;
////        }

////        // �Đ����ł͂Ȃ�AudioSource��������SE��炷
////        foreach (AudioSource source in SE_Sources)
////        {

////            // �Đ����� AudioSource �̏ꍇ�ɂ͎��̃��[�v�����ֈڂ�
////            if (source.isPlaying)
////            {
////                continue;
////            }

////            // �Đ����łȂ� AudioSource �� Clip ���Z�b�g���� SE ��炷
////            source.clip = SE_Clips[index];
////            source.Play();
////            break;
////        }
////    }

////    /// <summary>
////    /// SE��~
////    /// </summary>
////    public void StopSE()
////    {
////        // �S�Ă�SE�p��AudioSource���~����
////        foreach (AudioSource source in SE_Sources)
////        {
////            source.Stop();
////            source.clip = null;
////        }
////    }

////    /// <summary>
////    /// BGM�ꎞ��~
////    /// </summary>
////    public void MuteBGM()
////    {
////        BGM_Sources[0].Stop();
////        BGM_Sources[1].Stop();
////    }

////    /// <summary>
////    /// �ꎞ��~��������BGM���Đ�(�ĊJ)
////    /// </summary>
////    public void ResumeBGM()
////    {
////        BGM_Sources[0].Play();
////        BGM_Sources[1].Play();
////    }


////    ////* ���g�p *////


////    /// <summary>
////    /// AudioMixer�ݒ�
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