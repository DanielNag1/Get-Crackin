using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class SoundEngine : ScriptableObject
{
    /// <summary>
    /// Handles audio clips in audio sources.
    /// </summary>

    private static SoundEngine instance;
    public static SoundEngine Instance
    {
        get
        {
            if (instance == null)
            {
                instance = CreateInstance<SoundEngine>();
            }
            return instance;
        }
    }

    public List<AudioSource> audioSources;
    [SerializeField] private int musicTrackCount;
    [SerializeField] private int concurrentSFXPlaying;
    public List<Tuple<AudioSource, string /*trackPath*/, int /*priority*/, double /*requestSentTime*/>> requestList = new List<Tuple<AudioSource, string, int, double>>(); //requests sent from audioHandlers get placed here
    public List<float> volumeScales = new List<float>();
    /// <summary>
    /// Call this to send a request to play a sound.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="trackPath"></param>
    /// <param name="priority"></param>
    /// <param name="requestSentTime"></param>
    public void RequestSFX(AudioSource source, string trackPath, int priority, double requestSentTime,float volumeScale)
    {
        Tuple<AudioSource, string, int, double> tempTuple = new Tuple<AudioSource, string, int, double>(source, trackPath, priority, requestSentTime);
        requestList.Add(tempTuple);
        volumeScales.Add(volumeScale);
    }

    public void Update()
    {
        CheckRequestList();
    }

    private void CheckRequestList()
    {
        if (requestList.Count != 0)
        {
            //remove timed out requests
            for (int i = requestList.Count; i <= 0; i--)
            {
                if (Time.fixedTime - requestList[i].Item4 < 0.5d)
                {
                    requestList.RemoveAt(i);
                    volumeScales.RemoveAt(i);
                }
            }

            //sort the list by priority
            //requestList.Sort((x, y) => x.Item3.CompareTo(y.Item3));

            concurrentSFXPlaying = 0;

            //get currently playing sounds count, add playing list
            foreach (var SFX in requestList)
            {
                if (SFX.Item1.isPlaying)
                {
                    concurrentSFXPlaying++;
                }
            }

            requestList.Reverse();
            volumeScales.Reverse();

            //play requests up to limit of simultaneous sounds
            for (int i = requestList.Count - 1; i >= 0; i--)
            {
                LoadAudioClip(requestList[i], volumeScales[i]);
                requestList.RemoveAt(i);
                volumeScales.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Play a new sound, Item1=AudioSource, Item2 = string trackPath, Item3 = int Priority, Item4 = double RequestSentTime
    /// </summary>
    /// <param name="request"></param>
    private void LoadAudioClip(Tuple<AudioSource, string, int, double> request, float volumeScale)
    {
        request.Item1.clip = Resources.Load<AudioClip>(request.Item2);
        PlaySoundClip(request,volumeScale);
    }

    private void PlaySoundClip(Tuple<AudioSource, string, int, double> request, float volumeScale)
    {
        request.Item1.PlayOneShot(request.Item1.clip,volumeScale);
    }


    /// <summary>
    /// Plays the audio clip on the selected audio source.
    /// </summary>
    /// <param name="soundID">The ordinal of the audio source in the array.</param>
    public void Play(int soundID)
    {
        if (!audioSources[soundID].isPlaying)
        {
            //If a clip is not already playing, play it.
            audioSources[soundID].PlayOneShot(audioSources[soundID].clip);
        }

        //Checks if the audio clip selected is music.
        if (soundID < musicTrackCount)
        {
            for (int i = 0; i < musicTrackCount; i++)
            {
                if (audioSources[i] != null)
                {
                    //Checks if there is other music playing that is not the current one.
                    if (audioSources[i].isPlaying && i != soundID)
                    {
                        //Stop music that is playing to be able to play another music clip.
                        audioSources[i].Stop();
                    }
                }
            }
        }
    }
}


/*
 ------------------------------------
 Example of usage provided bellow
 ------------------------------------

    //Add to global variables of the movement script

        private float walkingSFXtimer = 0;
        [SerializeField]private float WalkingSFXCooldownTime;
        [SerializeField]private List<string> SoundPaths;

    //Add as a part of movement script

    private void WalkingSFX(float verticalAxis, float horizontalAxis)
    {

        // until timer is above cooldown keep incrementing timer
        if (walkingSFXtimer < WalkingSFXCooldownTime) 
        {
            walkingSFXtimer += Time.deltaTime;
        }
        
        //timer is above cooldown, Start checking if we are moving
        else 
        {
            //if we are moving we play a sound, and set the timer to 0
            if (verticalAxis != 0 || h != horizontalAxis) 
            {
                SoundEngine.Instance.RequestSFX(transform.root.GetComponent<AudioSource>(),SoundPaths[Random.Range(0,SoundPaths.Count)],1,Time.fixedTime);
                walkingSFXtimer = 0;
            }
        }
    }
      
*/
