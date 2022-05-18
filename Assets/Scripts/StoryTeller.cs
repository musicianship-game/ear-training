using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryTeller : MonoBehaviour {

    public TextMeshProUGUI title_text;
    private float title_fade_end;
    public float title_fade_duration = 3.0f;
    private Color title_color;
    private bool title_fade = false;
    public TextMeshProUGUI scene_text;
    private Color scene_text_color;
    private float scene_text_end_time = 0.0f;
    private float start_time;
    public SpriteRenderer sunset_pic;
    public float sky_fade_time = 20.0f;
    private float sky_end_time;
    private Color sky_color;

    // Use this for initialization
    void Start () {
        start_time = Time.time;
        sky_end_time = start_time + sky_fade_time;
        sky_color = sunset_pic.color;
        sky_color.a = 1.0f;
        sunset_pic.color = sky_color;
        scene_text_color = scene_text.color;
        scene_text_color.a = 0.0f;
        scene_text.color = scene_text_color;
        title_color = title_text.color;
        title_color.a = 0.0f;
        title_text.color = title_color;
        FadeInTitle();
        WriteSceneText("Once upon a time..", 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }
        if (Time.time > sky_end_time)
        {
            sky_color.a = 0.0f;
            sunset_pic.color = sky_color;
        }
        else
        {
            sky_color.a = (sky_end_time - Time.time) / sky_fade_time;
            sunset_pic.color = sky_color;
        }
        if (Time.time > scene_text_end_time)
        {
            scene_text_color.a = 0.0f;
            scene_text.color = scene_text_color;
        }
        if (title_fade && Time.time < title_fade_end)
        {
            title_color.a = 1.0f - (title_fade_end - Time.time) / title_fade_duration;
            title_text.color = title_color;
        }
        else if (title_fade && Time.time >= title_fade_end)
        {
            title_color.a = 1.0f;
            title_text.color = title_color;
        }
    }

    public void WriteSceneText(string text, float duration)
    {
        scene_text.text = text;
        scene_text_color.a = 1.0f;
        scene_text.color = scene_text_color;
        scene_text_end_time = Time.time + duration;
    }

    public void FadeInTitle()
    {
        title_fade = true;
        title_fade_end = Time.time + title_fade_duration;
    }
}
