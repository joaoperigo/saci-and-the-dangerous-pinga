using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
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

    public string note;

    void Start()
    {
        _samples = new float[QSamples];
        _spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;
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
        // C
        if (PitchValue >= 65.41 && PitchValue <= 69.30 ||
            PitchValue >= 130.82 && PitchValue <= 138.60 ||
            PitchValue >= 261.64 && PitchValue <= 277.20 ||
            PitchValue >= 523.28 && PitchValue <= 554.40 ||
            PitchValue >= 1046.56 && PitchValue <= 1108.80)
            note = "C";
        if (PitchValue >= 69.30 && PitchValue <= 73.42 ||
            PitchValue >= 138.60 && PitchValue <= 146.84 ||
            PitchValue >= 277.20 && PitchValue <= 293.68 ||
            PitchValue >= 554.40 && PitchValue <= 587.36 ||
            PitchValue >= 1108.80 && PitchValue <= 1244.48)
            note = "C#";
        else if (PitchValue >= 73.42 && PitchValue <= 77.78 ||
            PitchValue >= 146.84 && PitchValue <= 155.56 ||
            PitchValue >= 293.68 && PitchValue <= 311.12 ||
            PitchValue >= 587.36 && PitchValue <= 622.24 ||
            PitchValue >= 1244.48 && PitchValue <= 1318.56)
            note = "D";
        else if (PitchValue >= 73.42 && PitchValue <= 77.78 ||
            PitchValue >= 146.84 && PitchValue <= 155.56 ||
            PitchValue >= 293.68 && PitchValue <= 311.12 ||
            PitchValue >= 587.36 && PitchValue <= 622.24 ||
            PitchValue >= 1244.48 && PitchValue <= 1318.56)
            note = "D#";
        else if (PitchValue >= 82.41 && PitchValue <= 87.31 ||
            PitchValue >= 164.82 && PitchValue <= 174.62 ||
            PitchValue >= 329.64 && PitchValue <= 349.24 ||
            PitchValue >= 659.28 && PitchValue <= 698.48 ||
            PitchValue >= 1318.56 && PitchValue <= 1396.96)
            note = "E";
        else if (PitchValue >= 87.31 && PitchValue <= 92.50 ||
            PitchValue >= 174.62 && PitchValue <= 185.00 ||
            PitchValue >= 349.24 && PitchValue <= 370.00 ||
            PitchValue >= 698.48 && PitchValue <= 740.00 ||
            PitchValue >= 1396.96 && PitchValue <= 1480.00)
            note = "F";
        else if (PitchValue >= 92.50 && PitchValue <= 98.00 ||
            PitchValue >= 185.00 && PitchValue <= 196.00 ||
            PitchValue >= 370.00 && PitchValue <= 392.00 ||
            PitchValue >= 740.00 && PitchValue <= 784.00 ||
            PitchValue >= 1480.00 && PitchValue <= 1568.00)
            note = "F#";
        else if (PitchValue >= 98.00 && PitchValue <= 103.83 ||
            PitchValue >= 196.00 && PitchValue <= 207.66 ||
            PitchValue >= 392.00 && PitchValue <= 415.32 ||
            PitchValue >= 784.00 && PitchValue <= 830.64 ||
            PitchValue >= 1568.00 && PitchValue <= 1661.28)
            note = "G";
        else if (PitchValue >= 103.83 && PitchValue <= 55.00 ||
            PitchValue >= 207.66 && PitchValue <= 110.00 ||
            PitchValue >= 415.32 && PitchValue <= 220.00 ||
            PitchValue >= 830.64 && PitchValue <= 440.00 ||
            PitchValue >= 1661.28 && PitchValue <= 880.00)
            note = "G#";
        else if (PitchValue >= 55.00 && PitchValue <= 58.27 ||
            PitchValue >= 110.00 && PitchValue <= 116.54 ||
            PitchValue >= 220.00 && PitchValue <= 233.08 ||
            PitchValue >= 440.00 && PitchValue <= 466.16 ||
            PitchValue >= 880.00 && PitchValue <= 932.32)
            note = "A";
        else if (PitchValue >= 58.27 && PitchValue <= 61.74 ||
            PitchValue >= 116.54 && PitchValue <= 123.48 ||
            PitchValue >= 233.08 && PitchValue <= 246.96 ||
            PitchValue >= 466.16 && PitchValue <= 493.92 ||
            PitchValue >= 932.32 && PitchValue <= 987.84)
            note = "A#";
        else if (PitchValue >= 61.74 && PitchValue <= 87.31 ||
            PitchValue >= 123.48 && PitchValue <= 174.62 ||
            PitchValue >= 246.96 && PitchValue <= 349.24 ||
            PitchValue >= 493.92 && PitchValue <= 698.48 ||
            PitchValue >= 987.84 && PitchValue <= 1046.56)
            note = "B";

        myTextElement.text = note;
    }
}