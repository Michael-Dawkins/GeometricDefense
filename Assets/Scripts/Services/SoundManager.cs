using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {

    public static string BUTTON_CLICK = "ButtonClick";
    public static string CIRCLE_ION_CHARGE = "CircleIonCharge";
    public static string CIRCLE_SHOOT = "CircleShoot";
    public static string ENEMY_TO_BASE = "EnemyToBase";
    public static string ION_CHARGE_SQUARE = "IonChargeSquare";
    public static string ION_CHARGING = "IonCharging";
    public static string SQUARE_ATTACK_START = "SquareAttackStart";
    public static string SQUARE_HEALING = "SquareHealing";
    public static string TOWER_UPGRADE = "TowerUpgrade";
    public static string TRIANGLE_ION_CHARGE = "TriangleIonCharge";
    public static string TRIANGLE_SHOOT = "TriangleShoot";
    Dictionary<string, float> volumes;
    Dictionary<string, AudioClip> audioClips;

    public static SoundManager instance;

    void Awake() {
        instance = this;
    }

	void Start () {
        gameObject.AddComponent<AudioSource>();
        audioClips = new Dictionary<string, AudioClip>();
        volumes = new Dictionary<string, float>();
        volumes.Add(BUTTON_CLICK, 0.4f);
        volumes.Add(CIRCLE_ION_CHARGE, 0.4f);
        volumes.Add(CIRCLE_SHOOT, 0.4f);
        volumes.Add(ENEMY_TO_BASE, 0.4f);
        volumes.Add(ION_CHARGE_SQUARE, 0.4f);
        volumes.Add(ION_CHARGING, 0.4f);
        volumes.Add(SQUARE_ATTACK_START, 0.4f);
        volumes.Add(SQUARE_HEALING, 0.4f);
        volumes.Add(TOWER_UPGRADE, 0.4f);
        volumes.Add(TRIANGLE_ION_CHARGE, 0.4f);
        volumes.Add(TRIANGLE_SHOOT, 0.4f);
    }

    public void PlaySound(string soundKey) {
        AudioClip clipToPlay;
        if (!audioClips.ContainsKey(soundKey)) {
            audioClips[soundKey] = Instantiate(Resources.Load(soundKey)) as AudioClip;
        } 
        clipToPlay = audioClips[soundKey];
        audio.PlayOneShot(clipToPlay, volumes[soundKey]);
    }
	
}
