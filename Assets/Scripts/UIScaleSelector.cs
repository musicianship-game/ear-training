using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System.Linq;

public class UIScaleSelector : MonoBehaviour {
	private Dropdown notationDropdown;
	private Dropdown scaleDropdown;
	private Dropdown lessonplanDropdown;
	private UILessonPlanSliders lessonplanSliders;
	private Button acceptButton;
	private Button cancelButton;
	private string notationsDir;
	private string lessonplanDir;
	private string csvFrequenciesFilename;
	// private string csvDistributionFilename;

	private DirectoryInfo[] notationDirInfos;
	private DirectoryInfo[] scaleDirInfos;
	private FileInfo[] lessonplanInfos;

	private void Awake() {
		notationsDir = @"Assets/Scales";
		lessonplanDir = @"Assets/LessonPlans";
		csvFrequenciesFilename = @"fundamental_frequencies.csv";
		// Notation
		notationDropdown = transform.Find("NotationsDropdown").GetComponent<Dropdown>();
		notationDropdown.ClearOptions();
		// Scale
		scaleDropdown = transform.Find("ScalesDropdown").GetComponent<Dropdown>();
		scaleDropdown.interactable = false;
		scaleDropdown.ClearOptions();
		// Lesson plan
		lessonplanDropdown = transform.Find("LessonPlanDropdown").GetComponent<Dropdown>();
		lessonplanDropdown.interactable = false;
		lessonplanDropdown.ClearOptions();
		lessonplanSliders = transform.Find("LessonPlanSliders").GetComponent<UILessonPlanSliders>();
		// Accept button
		acceptButton = transform.Find("AcceptButton").GetComponent<Button>();
		acceptButton.interactable = false;
		// Cancel button
		cancelButton = transform.Find("CancelButton").GetComponent<Button>();
		// Listeners
		notationDropdown.onValueChanged.AddListener(NotationChanged);
		scaleDropdown.onValueChanged.AddListener(ScaleChanged);
		lessonplanDropdown.onValueChanged.AddListener(LessonPlanChanged);
		acceptButton.onClick.AddListener(AcceptChanges);
		cancelButton.onClick.AddListener(CancelChanges);
	}

	public void Activate() {
		// Filling the scale notations dropdown
		DirectoryInfo root = new DirectoryInfo(notationsDir);
        notationDirInfos = root.GetDirectories();
        List<string> notationNames = new List<string>();
		notationNames.Add("None");
		foreach (DirectoryInfo dir in notationDirInfos) notationNames.Add(dir.Name);
		notationDropdown.ClearOptions();
		notationDropdown.AddOptions(notationNames);
		// Filling the lesson plan dropdown
		root = new DirectoryInfo(lessonplanDir);
		lessonplanInfos = root.GetFiles("*.csv");
		List<string> lessonplanNames = new List<string>();
		lessonplanNames.Add("None");
		foreach (FileInfo file in lessonplanInfos) lessonplanNames.Add(file.Name);
		lessonplanDropdown.ClearOptions();
		lessonplanDropdown.AddOptions(lessonplanNames);
	}

	public void NotationChanged(int value) {
		bool notationSelected = (value != 0);
		scaleDropdown.ClearOptions();
		scaleDropdown.interactable = notationSelected;
		lessonplanDropdown.value = 0;
		lessonplanDropdown.interactable = false;
		acceptButton.interactable = false;
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
		bool scaleSelected = (value != 0);
		lessonplanDropdown.interactable = scaleSelected;
	}

	public void LessonPlanChanged(int value) {
		bool lessonplanSelected = (value != 0);
		if (lessonplanSelected) {
			string lessonplanFile = lessonplanInfos[value - 1].FullName;
			ParsedCSV lessonplanCSV = ReadCSV(lessonplanFile);
			Scale.LessonPlanNames = lessonplanCSV.names;
			Scale.Distribution = lessonplanCSV.values;
			lessonplanSliders.Activate();
			acceptButton.interactable = true;
		}
	}

	public class ParsedCSV
	{
		public List<string> names;
		public List<float> values;
		public int itemsPerLine;
		public int linePairs;

		public ParsedCSV() {
			names = new List<string>();
			values = new List<float>();
			itemsPerLine = 0;
			linePairs = 0;
		}
	}

	private ParsedCSV ReadCSV(string csvPath)
	{
		const int NAMES = 0;
		const int VALUES = 1;
		ParsedCSV csv = new ParsedCSV();
		using(StreamReader reader = new StreamReader(csvPath))
    	{
            int lineNumber = 0;
			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine();
				var tokens = line.Split(',');
				if (csv.itemsPerLine == 0) csv.itemsPerLine = tokens.Length;
				int lineType = lineNumber % 2 == 0 ? NAMES : VALUES;
				foreach (string token in tokens)
				{
					if (lineType == VALUES) {
						float t = float.Parse(token);
						csv.values.Add(t);
					}
					else {
						csv.names.Add(token);
					}
				}
				if (lineType == VALUES) csv.linePairs++;
				lineNumber++;
			}
    	}
		return csv;
	}

	private bool IsValidDistribution()
	{
		float p = 0f;
		for (int i = 0; i < lessonplanSliders.distribution.Count; i++)
		{
			p += lessonplanSliders.distribution[i];
		}
		return (p > 0.00001f);
	}

	public void AcceptChanges() {
		// If the sliders end up making an invalid distribution, don't store any changes
		if (!IsValidDistribution()) return;
		// Read CSV
		string scaleDir = scaleDirInfos[scaleDropdown.value - 1].FullName;
		string csvFrequenciesPath = Path.Combine(scaleDir, csvFrequenciesFilename);
		ParsedCSV frequencyCSV = ReadCSV(csvFrequenciesPath);
		Scale.NoteNames = frequencyCSV.names;
		Scale.Frequencies = frequencyCSV.values;
		Scale.ScaleDegrees = frequencyCSV.itemsPerLine;
		Scale.Alterations = frequencyCSV.linePairs;
		Scale.Distribution = lessonplanSliders.distribution;
		Scale.UpdateDistribution(Scale.Distribution);
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
