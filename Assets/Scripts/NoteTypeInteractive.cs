using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class NoteTypeInteractive : MonoBehaviour {
    public VRInteractiveItem m_InteractiveItem;
    public Sequencer sequencer;
    public MusicBox synth;
    public TextMesh text_mesh;
    public TextMesh slur_mesh;

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
        if (sequencer.beats_per_minute == 480f)
        {
            sequencer.slur_notes = !sequencer.slur_notes;
            sequencer.beats_per_minute = 30f;
            if (sequencer.slur_notes)
            {
                text_mesh.text = "w w";
                slur_mesh.text = "U";
            }
            else
            {
                text_mesh.text = "w";
                slur_mesh.text = "";
            }
        }
        else if (sequencer.beats_per_minute == 30f)
        {
            sequencer.beats_per_minute = 60f;
            if (sequencer.slur_notes)
            {
                text_mesh.text = "h h";
            }
            else
            {
                text_mesh.text = "h";
            }
        }
        else if (sequencer.beats_per_minute == 60f)
        {
            sequencer.beats_per_minute = 120f;
            if (sequencer.slur_notes)
            {
                text_mesh.text = "q q";
            }
            else
            {
                text_mesh.text = "q";
            }
        }
        else if (sequencer.beats_per_minute == 120f)
        {
            sequencer.beats_per_minute = 240f;
            if (sequencer.slur_notes)
            {
                text_mesh.text = "e e";
            }
            else
            {
                text_mesh.text = "e";
            }
        }
        else if (sequencer.beats_per_minute == 240f)
        {
            sequencer.beats_per_minute = 480f;
            if (sequencer.slur_notes)
            {
                text_mesh.text = "s s";
            }
            else
            {
                text_mesh.text = "s";
            }
        }
        else
        {
            sequencer.beats_per_minute = 30f;
            text_mesh.text = "d";
            sequencer.slur_notes = false;
        }
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
	
	}
}
