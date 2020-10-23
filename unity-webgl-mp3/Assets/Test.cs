using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Button playMusic;
    public Slider slider;
    private NLayer.MpegFile mp3MpgFile;
    private long readLen;
    private bool isPlay;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        playMusic.onClick.AddListener(() =>
        {
            PlayMusic();
        });
    }
    void PlayMusic()
    {
        byte[] buff = File.ReadAllBytes("music/刘昊霖 - 儿时.mp3");
        text.text = "刘昊霖 - 儿时";
        mp3MpgFile = new NLayer.MpegFile(new MemoryStream(buff));
        AudioSource audios = gameObject.GetComponent<AudioSource>();
        if (null == audios)
        {
            audios = gameObject.AddComponent<AudioSource>();
        }
        audios.clip = AudioClip.Create("clip", (int)mp3MpgFile.Length / 8, mp3MpgFile.Channels,
            mp3MpgFile.SampleRate, true, MusicBack);
        audios.loop = false;
        audios.volume = 1;
        audios.Play();
        isPlay = true;
    }
    void MusicBack(float[] data)
    {
        if (mp3MpgFile.ReadSamples(data, 0, data.Length) == 0)
        {
            isPlay = false;
            Debug.Log("播放完毕");
            mp3MpgFile.Dispose();
            mp3MpgFile = null;
        }
        readLen += data.LongLength;
    }
    private void Update()
    {
        if (isPlay)
        {
            slider.value = readLen / (float)mp3MpgFile.Length;
        }
    }
}
    