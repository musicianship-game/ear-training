using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILessonPlanSliders : MonoBehaviour {
	private GridLayoutGroup content;
	public Slider prefabSlider;
	public List<float> distribution;

	void Start()
	{
		content = transform.GetComponentInChildren<GridLayoutGroup>();
	}

	public void Activate()
	{
		int numberOfSliders = Scale.Distribution.Count;
		for (int i = 0; i < numberOfSliders; i++)
		{
			Slider slider = Instantiate(prefabSlider, content.transform);
			TextMeshProUGUI label = slider.transform.GetComponentInChildren<TextMeshProUGUI>();
			slider.value = Scale.Distribution[i];
			slider.gameObject.GetComponent<UISlider>().index = i;
			label.text = Scale.NoteNames[i];
			distribution.Add(slider.value);
			slider.onValueChanged.AddListener(delegate {
				UpdateDistribution(slider.value, slider);
			});
		}
	}

	public void UpdateDistribution(float value, Slider slider)
	{
		int index = slider.gameObject.GetComponent<UISlider>().index;
		Debug.Log(index + ", " + value);
		distribution[index] = value;
	}
}
