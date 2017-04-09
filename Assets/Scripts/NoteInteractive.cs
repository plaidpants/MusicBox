using UnityEngine;
using VRStandardAssets.Utils;

    // This script is a simple example of how an interactive item can
    // be used to change things on gameobjects by handling events.
    public class NoteInteractive : MonoBehaviour
    {
        public VRInteractiveItem m_InteractiveItem;
        public MusicBox synth;
        public int row;
        public bool set;
    public Sequencer sequencer;
    public GameObject setObject;
    public GameObject unsetObject;
    public Color HighlightColor;
    public Color NormalColor;

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

    public void Highlight()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        setObject.gameObject.GetComponent<Renderer>().material.color = HighlightColor;
        unsetObject.gameObject.GetComponent<Renderer>().material.color = HighlightColor;
    }
    public void UnHighlight()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        setObject.gameObject.GetComponent<Renderer>().material.color = NormalColor;
        unsetObject.gameObject.GetComponent<Renderer>().material.color = NormalColor;
    }

    //Handle the Over event
    private void HandleOver()
        {
        Highlight();
        }

        //Handle the Out event
        private void HandleOut()
        {
        UnHighlight();
        }


        //Handle the Click event
        private void HandleClick()
        {
            synth.NoteOn(1, synth.notescales[sequencer.scale, row + 28], sequencer.velocity);
            synth.NoteOff(1, synth.notescales[sequencer.scale, row + 28]);
            set = !set;

            if (set)
            {
                setObject.gameObject.SetActive(true);
                unsetObject.gameObject.SetActive(false);
            }
            else
            {
                setObject.gameObject.SetActive(false);
                unsetObject.gameObject.SetActive(true);
            }
        }


        //Handle the DoubleClick event
        private void HandleDoubleClick()
        {
        }
}