using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool preProcess = true;

    [SerializeField] private List<double> beatTimes = new List<double>();

    // private void Start()
    // {
    //     if (audioSource == null)
    //     {
    //         Debug.LogError("No AudioSource assigned.");
    //         return;
    //     }
    //
    //     // Nếu bạn tiền xử lý
    //     if (preProcess)
    //     {
    //         // Gọi hàm preprocess của beat‑mapping
    //         BeatMapper.PreprocessAudioClip(audioSource.clip, (List<double> times) =>
    //         {
    //             beatTimes = times;
    //             Debug.Log("Detected beat count: " + beatTimes.Count);
    //
    //             // Khi cần, bạn có thể start audio
    //             audioSource.Play();
    //         });
    //     }
    //     else
    //     {
    //         // Hoặc realtime detection — subscribe callback
    //         BeatMapper.OnBeat += OnBeatDetected;
    //         audioSource.Play();
    //     }
    // }

    private void OnBeatDetected(double dspTime)
    {
        beatTimes.Add(dspTime);
        Debug.Log("Beat at dspTime: " + dspTime);
    }

    // Ví dụ: hàm để trả ra danh sách beat
    public List<double> GetBeats()
    {
        return beatTimes;
    }
}