using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayNote : MonoBehaviour
{
    // public AudioSource source;
    public MicrophoneInput detector;

    //public float loudnessSensibility = 100;
    //public float threshold = 0.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    [SerializeField] private TextMeshProUGUI myTextElement;
    // Update is called once per frame
    void Update()
    {
        // float loudness = detector.GetLoudnessFromAudioClip(source.timeSamples, source.clip) * loudnessSensibility;
        float loudness = detector.GetFundamentalFrequency();

        //if (loudness < threshold)
        //    loudness = 0;

        // lerp value minscale to maxscale
        myTextElement.text = loudness.ToString();
        // Debug.Log(loudness);
    }
}