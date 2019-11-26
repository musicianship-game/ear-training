using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class UIScaleSelector : MonoBehaviour {
	private Dropdown notationDropdown;
	private Dropdown scaleDropdown;
	private Button acceptButton;
	private Button cancelButton;
	private string notationsDir;
	private string csvFilename;

	private DirectoryInfo[] notationDirInfos;
	private DirectoryInfo[] scaleDirInfos;

	private void Awake() {
		notationsDir = @"Assets/Scales";
		csvFilename = @"fundamental_frequencies.csv";
		notationDropdown = transform.Find("NotationsDropdown").GetComponent<Dropdown>();
		notationDropdown.ClearOptions();
		scaleDropdown = transform.Find("ScalesDropdown").GetComponent<Dropdown>();
		scaleDropdown.interactable = false;
		scaleDropdown.ClearOptions();
		acceptButton = transform.Find("AcceptButton").GetComponent<Button>();
		acceptButton.interactable = false;
		cancelButton = transform.Find("CancelButton").GetComponent<Button>();
		notationDropdown.onValueChanged.AddListener(NotationChanged);
		scaleDropdown.onValueChanged.AddListener(ScaleChanged);
		acceptButton.onClick.AddListener(AcceptChanges);
		cancelButton.onClick.AddListener(CancelChanges);
	}

	public void Activate() {
		DirectoryInfo root = new DirectoryInfo(notationsDir);
        notationDirInfos = root.GetDirectories();
        List<string> notationNames = new List<string>();
		notationNames.Add("None");
		foreach (DirectoryInfo dir in notationDirInfos) notationNames.Add(dir.Name);
		notationDropdown.ClearOptions();
		notationDropdown.AddOptions(notationNames);
	}

	public void NotationChanged(int value) {
		scaleDropdown.ClearOptions();
		bool notationSelected = (value != 0);
		acceptButton.interactable = false;
		scaleDropdown.interactable = notationSelected;
		if (notationSelected)
		{
			DirectoryInfo notationRoot = notationDirInfos[value - 1];
			scaleDirInfos = notationRoot.GetDirectories();
			List<string> scaleNames = new List<string>();
			scaleNames.Add("None");
			foreach (DirectoryInfo dir in scaleDirInfos) scaleNames.Add(dir.Name);
			scaleDropdown.AddOptions(scaleNames);
			scaleDropdown.value = 0;
		}
	}

	public void ScaleChanged(int value) {
		bool scaleSelected = value != 0;
        acceptButton.interactable = scaleSelected;
	}

	public void AcceptChanges() {
		// Read the CSV
		List<string> NoteNames = new List<string>();
		List<float> Frequencies = new List<float>();
		int scaleDegrees = 0;
		int alterations = 0;
		string scaleDir = scaleDirInfos[scaleDropdown.value - 1].FullName;
		string csvPath = Path.Combine(scaleDir, csvFilename);
		using(StreamReader reader = new StreamReader(csvPath))
    	{
			int lineType = 0;
			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine();
				var values = line.Split(',');
				if (scaleDegrees == 0) scaleDegrees = values.Length;
				bool isFrequency = lineType % 2 == 0 ? false : true;
				// if (isFrequency) Frequencies.Clear();
				// NoteNames.Clear();
				foreach (string value in values)
				{
					if (isFrequency) {
						float freq = float.Parse(value);
						Frequencies.Add(freq);
					}
					else{
						NoteNames.Add(value);
					}
				}
				if (isFrequency) alterations++;
				lineType++;
			}
    	}
		Scale.NoteNames = NoteNames;
		Scale.Frequencies = Frequencies;
		Scale.ScaleDegrees = scaleDegrees;
		Scale.Alterations = alterations;
		gameObject.SetActive(false);
	}

	public void CancelChanges() {
		notationDropdown.value = 0;
		scaleDropdown.value = 0;
		notationDropdown.ClearOptions();
		scaleDropdown.ClearOptions();
		gameObject.SetActive(false);
	}
}
