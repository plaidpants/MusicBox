using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class LoudInteractive : MonoBehaviour {
    public Sequencer sequencer;
    public VRInteractiveItem m_InteractiveItem;
    public MusicBox synth;
    public TextMesh text_mesh;

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
        if (sequencer.velocity == 126)
        {
            sequencer.velocity = 16;
            text_mesh.text = "ppp";
        }
        else if (sequencer.velocity == 16)
        {
            sequencer.velocity = 33;
            text_mesh.text = "pp";
        }
        else if (sequencer.velocity == 33)
        {
            sequencer.velocity = 49;
            text_mesh.text = "p";
        }
        else if (sequencer.velocity == 49)
        {
            sequencer.velocity = 64;
            text_mesh.text = "mp";
        }
        else if (sequencer.velocity == 64)
        {
            sequencer.velocity = 80;
            text_mesh.text = "mf";
        }
        else if (sequencer.velocity == 80)
        {
            sequencer.velocity = 96;
            text_mesh.text = "f";
        }
        else if (sequencer.velocity == 96)
        {
            sequencer.velocity = 112;
            text_mesh.text = "ff";
        }
        else if (sequencer.velocity == 112)
        {
            sequencer.velocity = 126;
            text_mesh.text = "fff";
        }
        else
        {
            sequencer.velocity = 96;
            text_mesh.text = "f";
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
