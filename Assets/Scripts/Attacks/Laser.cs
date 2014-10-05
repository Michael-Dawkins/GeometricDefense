using UnityEngine;
using System.Collections;

public class Laser : Projectile {

	float shootingTime;
	float timeBeforeDeletion = 1f;

	void Start () {
		shootingTime = Time.time;
	}
	
	void Update () {
		if ((shootingTime + timeBeforeDeletion) < Time.time){
			Destroy(this);
		} else {
			transform.localPosition = new Vector3(transform.localPosition.x + speed * Time.deltaTime, transform.localPosition.y, 0);
		}
	}

	public override void OnEnemyHit(){}

	public void SetColor(Color colorToApply){
		SpriteRenderer centerRenderer = GetComponent<SpriteRenderer>();
		SpriteRenderer glowRenderer = transform.Find("Glow").GetComponent<SpriteRenderer>();

		float h, s, v;
		GDUtils.ColorToHSV(colorToApply, out h, out s, out v);
		colorToApply = GDUtils.ColorFromHSV(h, 1f, v);
		GDUtils.ColorToHSV(colorToApply, out h, out s, out v);
		Color lightColor = GDUtils.ColorFromHSV(h,0.25f,v);

		centerRenderer.color = lightColor;
		glowRenderer.color = colorToApply;
	}
}
