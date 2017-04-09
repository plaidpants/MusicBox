using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class ScaleInteractive : MonoBehaviour {
    public VRInteractiveItem m_InteractiveItem;
    public Sequencer sequencer;
    public MusicBox synth;
    public TextMesh text_mesh;
    public Color HighlightColor;
    public float HighlightColorAppliedTime;
    public float HighlightColorTime = 0.25f;
    public Color NormalColor;
    public GameObject highlightObject;

    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
        m_InteractiveItem.OnClick += HandleClick;
        m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
    }

    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
        m_InteractiveItem.OnClick -= HandleClick;
        m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
    }

    //Handle the Over event
    private void HandleOver()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    //Handle the Out event
    private void HandleOut()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    //Handle the Click event
    private void HandleClick()
    {
        sequencer.scale++;
        if (sequencer.scale > 33)
        {
            sequencer.scale = 0;
        }

        if (sequencer.playing)
        {
            sequencer.StopPlaying();
            sequencer.StartPlaying();
        }

        highlightObject.gameObject.GetComponent<Renderer>().material.color = HighlightColor;
        HighlightColorAppliedTime = Time.time;
    }

    //Handle the DoubleClick event
    private void HandleDoubleClick()
    {
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if ((HighlightColorAppliedTime != 0f) && (Time.time > HighlightColorAppliedTime + HighlightColorTime))
        {
            highlightObject.gameObject.GetComponent<Renderer>().material.color = NormalColor;
            HighlightColorAppliedTime = 0f;
        }
    }
}
