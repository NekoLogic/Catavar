using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PanelMaker : MonoBehaviour
{
	public Canvas canvas;
	public SkinnedMeshRenderer mesh;
	public GameObject panelPrefab;
	public GameObject sliderPrefab;
	public GameObject sliderContainerPrefab;
	public float itemHeight = 50f;
	public List<PanelSetup> setupList = new List<PanelSetup> ();

	private Transform _content;
	private GameObject _currentPanel;
	private Button _closeButton;

	// Use this for initialization
	void Start ()
	{
		//CreatePanel (0);
	}

	private void BuildSliders (List<SliderSetup> setups)
	{
		float h = 30f;
		foreach (SliderSetup setup in setups)
		{
			RectTransform rt = SetupSliders (setup);

			rt.SetInsetAndSizeFromParentEdge (RectTransform.Edge.Top, h, 40);
			h += itemHeight;
		}
	}

	public void CreatePanel (int index)
	{
		ClearPanel ();

		GameObject panel = Instantiate (panelPrefab, canvas.transform);
		Text title = panel.transform.Find ("Title").GetComponent<Text> ();
		title.text = setupList[index].label;
		panel.name = setupList[index].label + " Panel";
		_content = panel.transform.Find ("Viewport/Content");
		_closeButton = panel.GetComponentInChildren<Button> ();
		_closeButton.onClick.AddListener (ClearPanel);
		List<SliderSetup> setups = setupList[index].setups;
		BuildSliders (setups);
		_currentPanel = panel;
	}

	public void ClearPanel ()
	{
		if (_currentPanel == null) return;
		_closeButton.onClick.RemoveAllListeners ();
		_closeButton = null;
		_content = null;
		Destroy (_currentPanel);
		_currentPanel = null;
	}

	private RectTransform SetupSliders (SliderSetup setup)
	{
		GameObject containerGO = Instantiate (sliderContainerPrefab, _content);
		containerGO.name = setup.label + " Slider";
		Text label = containerGO.transform.Find ("Label").GetComponent<Text> ();
		label.text = setup.label;

		Transform sliderHolder = containerGO.transform.Find ("SliderHolder");
		AddSlider (setup.targetA, sliderHolder);
		AddSlider (setup.targetB, sliderHolder);

		BlendShapeMixer[] mixers = containerGO.GetComponentsInChildren<BlendShapeMixer> ();
		Slider[] sliders = containerGO.GetComponentsInChildren<Slider> ();

		return containerGO.transform as RectTransform;
	}

	private void AddSlider (BlendTargetData targetData, Transform sliderHolder)
	{
		if (targetData.targets.Count == 0) return;
		GameObject sliderGO = Instantiate (sliderPrefab, sliderHolder);
		sliderGO.name = targetData.ToString () + " Slider";

		BlendShapeMixer mixer = sliderGO.GetComponent<BlendShapeMixer> ();
		mixer.targets = targetData.targets;
		mixer.mesh = mesh;

		Slider slider = sliderGO.GetComponent<Slider> ();
		slider.value = mixer.GetSliderValueForTargets ();
	}
}

[System.Serializable]
public class PanelSetup
{
	public string label;
	public List<SliderSetup> setups;
}

[System.Serializable]
public class SliderSetup
{
	public string label;
	public BlendTargetData targetA;
	public BlendTargetData targetB;
}

[System.Serializable]
public class BlendTargetData
{
	public List<BlendTarget> targets;
	public float balance = 0.5f;

	override public string ToString ()
	{
		return String.Join ("-", targets.Select (w => w.ToString ()).ToArray ());
	}
}