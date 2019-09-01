using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;

public class UIScaleSelector : MonoBehaviour {
	private Text notationText;
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
		csvFilename = @"values.csv";
		notationText = transform.Find("NotationsText").GetComponent<Text>();
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
		foreach (DirectoryInfo dir in notationDirInfos)
        {
			notationNames.Add(dir.Name);
        }
		notationDropdown.ClearOptions();
		notationDropdown.AddOptions(notationNames);
	}

	public void NotationChanged(int value) {
		scaleDropdown.ClearOptions();
		if (value == 0)
		{
			scaleDropdown.interactable = false;
			acceptButton.interactable = false;
		}
		else
		{
			scaleDropdown.interactable = true;
			DirectoryInfo notationRoot = notationDirInfos[value - 1];
			scaleDirInfos = notationRoot.GetDirectories();
			List<string> scaleNames = new List<string>();
			scaleNames.Add("None");
			foreach (DirectoryInfo dir in scaleDirInfos)
        	{
				scaleNames.Add(dir.Name);
        	}
			scaleDropdown.AddOptions(scaleNames);
			scaleDropdown.value = 0;
		}
	}

	public void ScaleChanged(int value) {
		if (value == 0)
		{
			acceptButton.interactable = false;
		}
		else
		{
			acceptButton.interactable = true;
		}
	}

	public void AcceptChanges() {
		// Read the CSV
		List<string> NoteNames = new List<string>();
		List<string> Frequencies = new List<string>();
		string scaleDir = scaleDirInfos[scaleDropdown.value - 1].FullName;
		string csvPath = Path.Combine(scaleDir, csvFilename);
		using(StreamReader reader = new StreamReader(csvPath))
    	{
			int lineType = 0;
			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine();
				var values = line.Split(',');
				foreach (string value in values)
				{
					bool isFrequency = lineType % 2 == 0 ? false : true;
					if (isFrequency) {
						Frequencies.Add(value);
					}
					else{
						NoteNames.Add(value);
					}
				}
			}
    	}
		foreach (string note in NoteNames)
		{
			Debug.Log(note);
		}
		foreach (string frequency in Frequencies)
		{
			Debug.Log(frequency);
		}
	}

	public void CancelChanges() {
		notationDropdown.value = 0;
		scaleDropdown.value = 0;
		notationDropdown.ClearOptions();
		scaleDropdown.ClearOptions();
		gameObject.SetActive(false);
	}
}
