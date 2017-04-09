using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class PlaySequencer : MonoBehaviour {

    public Sequencer sequencer;
    public VRInteractiveItem m_InteractiveItem;
    public MusicBox synth;
    public int row;
    public int column;
    public bool set;
    public int instrument;
    public TextMesh text_mesh;

    public Color HighlightColor;
    public float HighlightColorAppliedTime;
    public Color NormalColor;
    public GameObject highlightObject;
    public float HighlightColorTime = 0.25f;

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
        if (sequencer.playing)
        {
            sequencer.StopPlaying();
            text_mesh.text = "";
        }
        else
        {
            sequencer.StartPlaying();
            text_mesh.text = "";
        }

        highlightObject.gameObject.GetComponent<Renderer>().material.color = HighlightColor;
        HighlightColorAppliedTime = Time.time;

        // change to stop icon
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
