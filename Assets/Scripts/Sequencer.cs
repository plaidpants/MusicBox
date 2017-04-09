using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

public class Sequencer : MonoBehaviour {

    public NoteInteractive[,] notes;
    public MusicBox synth;
    public GameObject grid;
    public GameObject notePrefab;

    public float beats_per_minute = 120f;
    public bool playing = false;
    public int playing_column = 0;
    public float last_play_time = 0f;
    public int instrument = 0;
    public int scale = 0;
    public int velocity = 100;
    public int note_offset = 28;
    public bool slur_notes = false;

    // Use this for initialization
    void Start () {
        notes = new NoteInteractive[8, 8];

        for (int column = 0; column < 8; column++)
        {
            for (int row = 0; row < 8; row++)
            {
                Vector3 pos = new Vector3((column)*2, (7-row)*2, 0);
                GameObject note = Instantiate(notePrefab, pos, grid.transform.rotation) as GameObject;
                note.transform.SetParent(grid.transform);
                note.transform.localPosition = pos;
                NoteInteractive  noteInteractive = note.GetComponent<NoteInteractive>();
                noteInteractive.row = row;
                noteInteractive.set = false;
                noteInteractive.synth = synth;
                noteInteractive.sequencer = this;
                notes[row, column] = noteInteractive;
            }
        }
    }

    public void StartPlaying()
    {
        Sequencer[] seqs = FindObjectsOfType(typeof(Sequencer)) as Sequencer[];
        foreach (Sequencer seq in seqs)
        {
            seq.playing_column = 7;
            seq.playing = true;
        }


    }

    public void StopPlaying()
    {
        synth.AllNotesOff();

        Sequencer[] seqs = FindObjectsOfType(typeof(Sequencer)) as Sequencer[];
        foreach (Sequencer seq in seqs)
        {
            seq.playing = false;
            for (int row = 0; row < 8; row++)
            {
                seq.notes[row, playing_column].UnHighlight();
            }
        }
        
    }

    // Update is called once per frame
    void Update () {
        float time = Time.time;
        int next_playing_column;

	    if (playing && last_play_time + 60f / beats_per_minute < time)
        {
            next_playing_column = playing_column + 1;
            
            if (next_playing_column > 7)
            {
                next_playing_column = 0;
            }

            for (int row = 0; row < 8; row++)
            {
                if ((notes[row, playing_column].set && !notes[row, next_playing_column].set && slur_notes) || (notes[row, playing_column].set  && !slur_notes))
                {
                    synth.NoteOff(1, synth.notescales[scale, row + note_offset]);
                }

                notes[row, playing_column].UnHighlight();
            }

            for (int row = 0; row < 8; row++)
            {
                if ((!notes[row, playing_column].set && notes[row, next_playing_column].set && slur_notes) || (notes[row, next_playing_column].set && !slur_notes))
                {
                    synth.NoteOn(1, synth.notescales[scale, row + note_offset], velocity);
                }

                notes[row, next_playing_column].Highlight();
            }

            last_play_time = time;
            playing_column = next_playing_column;
        }
    }
}
