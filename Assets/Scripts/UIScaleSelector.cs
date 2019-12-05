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
	private string csvFrequenciesFilename;
	private string csvDistributionFilename;

	private DirectoryInfo[] notationDirInfos;
	private DirectoryInfo[] scaleDirInfos;

	private void Awake() {
		notationsDir = @"Assets/Scales";
		csvFrequenciesFilename = @"fundamental_frequencies.csv";
		csvDistributionFilename = @"distribution.csv";

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

	public void AcceptChanges() {
		// Read the CSV
		string scaleDir = scaleDirInfos[scaleDropdown.value - 1].FullName;
		string csvFrequenciesPath = Path.Combine(scaleDir, csvFrequenciesFilename);
		string csvDistributionPath = Path.Combine(scaleDir, csvDistributionFilename);
		ParsedCSV frequencyCSV = ReadCSV(csvFrequenciesPath);
		ParsedCSV distributionCSV = ReadCSV(csvDistributionPath);
		Scale.NoteNames = frequencyCSV.names;
		Scale.Frequencies = frequencyCSV.values;
		Scale.ScaleDegrees = frequencyCSV.itemsPerLine;
		Scale.Alterations = frequencyCSV.linePairs;
		Scale.Distribution = distributionCSV.values;
		Scale.NormalizeDistribution();
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
