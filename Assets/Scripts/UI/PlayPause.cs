﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (Image))]
public class PlayPause : MonoBehaviour {

    public static PlayPause instance;
    public Sprite playSprite;
    public Sprite pauseSprite;
    private Image image;
    public delegate void OnPauseListener();
    private List<OnPauseListener> callbacks = new List<OnPauseListener>();

    void Awake() {
        instance = this;
    }

	void Start () {
        image = GetComponent<Image>();
        UpdateState();
        NotifyPauseListeners();
	}

    public void TogglePlayPause() {
        TimeScaleManager.instance.isPlaying = !TimeScaleManager.instance.isPlaying;
        if (TimeScaleManager.instance.isPlaying) {
            Play();
        } else {
            Pause();
        }
        UpdateState();
        NotifyPauseListeners();
    }

    void Play() {
        switch (TimeScaleManager.instance.timeScale) {
            case TimeScaleManager.TimeScale.ONE:
                Time.timeScale = 1f;
                break;
            case TimeScaleManager.TimeScale.TWO:
                Time.timeScale = 2f;
                break;
            case TimeScaleManager.TimeScale.THREE:
                Time.timeScale = 3f;
                break;
        }
        Spawners.instance.SpawnerList[0].StartNextWave();
    }

    void Pause() {
        Time.timeScale = 0f;
    }

    void UpdateState() {
        if (Time.timeScale == 0f) {
            image.sprite = playSprite;
        } else {
            image.sprite = pauseSprite;
        }
    }

    public void AddPauseListener(OnPauseListener callback) {
        callbacks.Add(callback);
    }

    public void RemovePauseListener(OnPauseListener callback) {
        callbacks.Remove(callback);
    }

    public void NotifyPauseListeners() {
        foreach (OnPauseListener callback in callbacks) {
            callback();
        }
    }

}
