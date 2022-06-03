using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFromMic : MonoBehaviour
{
    public int sampleWindow = 128;
    private AudioClip microphoneClip;

    // Start is called before the first frame update
    void Start()
    {
        MicrophoneToAudioClip();
    }

    public void MicrophoneToAudioClip()
    {
        // get the frst microphone in device list
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 2, AudioSettings.outputSampleRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
