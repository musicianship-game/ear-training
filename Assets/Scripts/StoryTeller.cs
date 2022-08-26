using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StoryTeller : MonoBehaviour {

    public TextMeshProUGUI title_text;
    private float title_fade_end;
    private float title_start_time;
    private float text_fade_end;
    public float title_fade_duration = 3.0f;
    private float text_duration;
    private Color title_color;
    private bool title_fade = false;
    public TextMeshProUGUI scene_text;
    private Color scene_text_color;
    private float start_time;
    public SpriteRenderer sunset_pic;
    public float sky_fade_time = 20.0f;
    private float sky_end_time;
    private Color sky_color;
    private SceneText current_scene_text = null;
    private Queue<SceneText> texts;
    private bool initialization_complete = false;
    private float title_stay_dur;

    public class SceneText
    {
        public float start_time;
        public float end_time;
        public float duration;
        public string text;
        public SceneText(float start, float end, string t)
        {
            start_time = start;
            end_time = end;
            duration = end_time - start_time;
            text = t;
        }
        public bool IsActive(float current_time)
        {
            return HasStarted(current_time) && !HasEnded(current_time) ? true : false;
        }
        public bool HasStarted(float current_time)
        {
            return current_time >= start_time ? true : false;
        }

        public bool HasEnded(float current_time)
        {
            return current_time > end_time ? true : false;
        }
    }

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
        texts = new Queue<SceneText>();
        texts.Enqueue(new SceneText(1.0f, 5.0f, "When night falls..."));
        texts.Enqueue(new SceneText(6.0f, 11.0f, "...ghosts begin to haunt the village."));
        texts.Enqueue(new SceneText(12.0f, 16.0f, "They can be freed from this place..."));
        texts.Enqueue(new SceneText(17.0f, 20.0f, "...through Music!"));
        title_start_time = 21.0f;
        title_stay_dur = 5.0f;
        initialization_complete = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }
        // Sky stuff
        if (Time.time < sky_end_time)
        {
            sky_color.a = (sky_end_time - Time.time) / sky_fade_time;
            sunset_pic.color = sky_color;
        }
        else {
            sky_color.a = 0.0f;
            sunset_pic.color = sky_color;
        }
        // Title stuff
        if (title_fade)
        {
            if (Time.time < title_fade_end)
            { 
                title_color.a = 1.0f - (title_fade_end - Time.time) / title_fade_duration;
                title_text.color = title_color;
            }
            else
            {
                title_color.a = 1.0f;
                title_text.color = title_color;
            }
        }

        if (texts.Count > 0 && texts.Peek().HasStarted(Time.time))
        {
            current_scene_text = texts.Dequeue();
            WriteSceneText(current_scene_text.text, current_scene_text.duration);
        }
        if (current_scene_text != null && !current_scene_text.IsActive(Time.time))
        {
            current_scene_text = null;
            Debug.Log("Killing a text!");
        }

        // Other text stuff
        if (Time.time < text_fade_end)
        {
            scene_text_color.a = TextFadeFunc();
            scene_text.color = scene_text_color;
        }
        else
        {
            scene_text_color.a = 0.0f;
            scene_text.color = scene_text_color;
        }

        if (initialization_complete && !title_fade && Time.time >= title_start_time + start_time)
        {
            FadeInTitle();
        }

        if (initialization_complete && title_fade && Time.time > title_fade_end + title_stay_dur)
        {
            SceneManager.LoadScene(1);
        }
    }

    public float TextFadeFunc()
    {
        float normalized_fade = (text_fade_end - Time.time) / text_duration;
        float centered_fade = Mathf.Abs(normalized_fade - 0.5f);
        float fade_envelope = Mathf.Min(((0.5f - centered_fade) * 10.0f), 1.0f);
        return fade_envelope;
    }
    
    public void WriteSceneText(string text, float duration)
    {
        scene_text.text = text;
        scene_text_color.a = 0.0f;
        scene_text.color = scene_text_color;
        text_duration = duration;
        text_fade_end = Time.time + duration;
    }

    public void FadeInTitle()
    {
        title_fade = true;
        title_fade_end = Time.time + title_fade_duration;
    }
}
