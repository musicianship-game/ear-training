using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryTeller : MonoBehaviour {

    public SpriteRenderer sunset_pic;
    public TextMeshProUGUI title_text;
    public TextMeshProUGUI scene_text;
    public float fade_time = 10.0f;
    private float start_time;
    private float end_time;
    private Color color;

    // Use this for initialization
    void Start () {
        start_time = Time.time;
        end_time = start_time + fade_time;
        color = sunset_pic.color;
        color.a = 1.0f;
        sunset_pic.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > end_time)
        {
            color.a = 0.0f;
        }
        else
        {
            color.a = (end_time - Time.time) / fade_time;
            sunset_pic.color = color;
        }
    }
}
