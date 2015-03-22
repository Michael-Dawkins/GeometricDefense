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

    public AudioClip ButtonClick;
    public AudioClip CircleIonCharge;
    public AudioClip CircleShoot;
    public AudioClip EnemyToBase;
    public AudioClip IonChargeSquare;
    public AudioClip IonCharging;
    public AudioClip SquareAttackStart;
    public AudioClip SquareHealing;
    public AudioClip TowerUpgrade;
    public AudioClip TriangleIonCharge;
    public AudioClip TriangleShoot;


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

        audioClips.Add(BUTTON_CLICK, ButtonClick);
        audioClips.Add(CIRCLE_ION_CHARGE, CircleIonCharge);
        audioClips.Add(CIRCLE_SHOOT, CircleShoot);
        audioClips.Add(ENEMY_TO_BASE, EnemyToBase);
        audioClips.Add(ION_CHARGE_SQUARE, IonChargeSquare);
        audioClips.Add(ION_CHARGING, IonCharging);
        audioClips.Add(SQUARE_ATTACK_START, SquareAttackStart);
        audioClips.Add(SQUARE_HEALING, SquareHealing);
        audioClips.Add(TOWER_UPGRADE, TowerUpgrade);
        audioClips.Add(TRIANGLE_ION_CHARGE, TriangleIonCharge);
        audioClips.Add(TRIANGLE_SHOOT, TriangleShoot);  

    }

    public void PlaySound(string soundKey) {
        GetComponent<AudioSource>().PlayOneShot(audioClips[soundKey], volumes[soundKey]);
    }
	
}
