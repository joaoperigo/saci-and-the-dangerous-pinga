using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // So we can use List<>
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class GetHertzFromAudio : MonoBehaviour
{
    public float RmsValue;
    public float DbValue;
    public float PitchValue;

    private const int QSamples = 1024;
    private const float RefValue = 0.1f;
    private const float Threshold = 0.02f;

    float[] _samples;
    private float[] _spectrum;
    private float _fSample;

    // int floatToInt;
    //[SerializeField]
    //public Text txtAudio;

    public string microphone;
    private AudioSource audioSource;
    public int audioSampleRate = 44100;
    private List<string> options = new List<string>();

    void Start()
    {
        _samples = new float[QSamples];
        _spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;

        audioSource = GetComponent<AudioSource>();
        foreach (string device in Microphone.devices)
        {
            if (microphone == null)
            {
                //set default mic to first mic found.
                microphone = device;
            }
            options.Add(device);
        }
        UpdateMicrophone();
    }

    void UpdateMicrophone()
    {
        audioSource.Stop();
        //Start recording to audioclip from the mic
        audioSource.clip = Microphone.Start(microphone, true, 10, audioSampleRate);
        audioSource.loop = true;
        // Mute the sound with an Audio Mixer group becuase we don't want the player to hear it
        Debug.Log(Microphone.IsRecording(microphone).ToString());

        if (Microphone.IsRecording(microphone))
        { //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
            while (!(Microphone.GetPosition(microphone) > 0))
            {
            } // Wait until the recording has started. 

            Debug.Log("recording started with " + microphone);

            // Start playing the audio source
            audioSource.Play();
            // audioSource.volume = 0.001f;
        }
        else
        {
            //microphone doesn't work for some reason

            Debug.Log(microphone + " doesn't work!");
        }
    }

    void Update()
    {
        AnalyzeSound();
    }

    [SerializeField] private TextMeshProUGUI myTextElement;
    void AnalyzeSound()
    {
        GetComponent<AudioSource>().GetOutputData(_samples, 0); // fill array with samples
        int i;
        float sum = 0;
        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // sum squared samples
        }
        RmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average
        DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // calculate dB
        if (DbValue < -160) DbValue = -160; // clamp it to -160dB min
                                            // get sound spectrum
        GetComponent<AudioSource>().GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < QSamples; i++)
        { // find max 
            if (!(_spectrum[i] > maxV) || !(_spectrum[i] > Threshold))
                continue;

            maxV = _spectrum[i];
            maxN = i; // maxN is the index of max
        }
        float freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < QSamples - 1)
        { // interpolate index using neighbours
            var dL = _spectrum[maxN - 1] / _spectrum[maxN];
            var dR = _spectrum[maxN + 1] / _spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        PitchValue = freqN * (_fSample / 2) / QSamples; // convert index to frequency
        //Debug.Log(PitchValue);
        //floatToInt = (int)PitchValue;
        //Debug.Log(floatToInt);
        // txt.text = PitchValue.ToString();
        //txtAudio.text = "Asd";
        myTextElement.text = PitchValue.ToString();
    }
}