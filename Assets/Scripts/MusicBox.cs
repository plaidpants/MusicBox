using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using AudioSynthesis.Effects;
using AudioSynthesis.Sequencer;
using AudioSynthesis.Synthesis;
using AudioSynthesis.Midi;

using UnityEngine.VR;

/*
Ideas:
size of boxes control gap between notes, bigger boxes, closer together less gap
click and drag to connect notes for no gap between notes
multiple player on same sequencer grid for "rounds"
click in space to create sequencer
click and drag in space to create different sized squencer
click in sequencer title bar to move
default title bar to name of instrument
double click in title bar to change name
use outline highlight to show selected or dragging sequencer
master control, play, pause, stop, rewind, load, save
scales
playback graphics modes, fireworks, boxes, balls, colors, bouncing balls
do a version of the pipe dreams animusic thing with blocks that shoot out of horns or something colorful
import midi
play midi
audio instructions for tutorial, offer until done once
icons of instruments, find a font? click icon to select, pop-up grid of instruments
minimize sequencer, turns into a clolored box that plays, click to open again, maybe only one open at a time to reduce grphic load
each sequencer gets a random color at creation, let user select somewhere
sequencer copy, duplicate, delete
light up boxes
use a tray model with little insets squares to place note boxes
have a background that can be changed
get insturumnet list from web site code snippet

    Use object position to get note value to play

display note icons on boxes

    have a cube mode where all the sequencers are stacked and play

    have a sequence of sequencers to organize entire music scores

    make head track clicking work with the same interface that the hand controller works so we don't need a new interface when those become available

    rotate around inside a cube, add new sequencers as segments of a polygon, wider sequencers longer segments

    drumkit needs special handling, maybe split up channel into different sequencers with different music icons

    play a little bit while selecting instrument

    make the outline box out of long thin cubes

    store note in object, use raycast to hit objects and play notes

    rotate squencer, match a piano keyboard layout
    suuport blutooth midi keyboard
*/

[RequireComponent (typeof(AudioSource))]
public class MusicBox : MonoBehaviour
{
    /*
        printnotes("majorScale");
        printnotes("minorScale");
        printnotes("chromaticScale");
        printnotes("bluesScale");
        printnotes("bluesDiminishedScale");
        printnotes("fullMinorScale");
        printnotes("harmonicMajorScale");
        printnotes("harmonicMinorScale");
        printnotes("jazzMinorScale");
        printnotes("hawaiianScale");
        printnotes("orientalScale");
        printnotes("majorMinorScale");
        printnotes("genusChromaticum");
        printnotes("dorian");
        printnotes("indian");
        printnotes("locrian");
        printnotes("lydian");
        printnotes("melodicMinor");
        printnotes("mixolydian");
        printnotes("pentatonic");
        printnotes("phrygian");
        printnotes("turkish");
        printnotes("wholeHalf");
        printnotes("wholeTone");
        printnotes("spanish");
        printnotes("pelog");
        printnotes("kumoi");
        printnotes("iwato");
        printnotes("inSen");
        printnotes("hirojoshi");
        printnotes("hungarianMinor");
        printnotes("bhairav");
        printnotes("superLocrian");
        printnotes("minorPentatonic");
    */

    public const int MIDI_MIDDLE_C = 60;
    public const int MAX_MIDI_NOTES = 56;
    public const int MAX_SCALES = 34;

    public int[,] notescales = new int[MAX_SCALES, MAX_MIDI_NOTES] {
  {
    107, 105, 103, 101, 100, 98, 96, 95, 93, 91, 89, 88, 86, 84, 83, 81, 79, 77, 76, 74, 72, 71, 69, 67, 65, 64, 62, 60, 59, 57, 55, 53, 52, 50, 48, 47, 45, 43, 41, 40, 38, 36, 35, 33, 31, 29, 28, 26, 24, 23, 21, 19, 17, 16, 14, 12                }
  ,
  {
    106, 104, 103, 101, 99, 98, 96, 94, 92, 91, 89, 87, 86, 84, 82, 80, 79, 77, 75, 74, 72, 70, 68, 67, 65, 63, 62, 60, 58, 56, 55, 53, 51, 50, 48, 46, 44, 43, 41, 39, 38, 36, 34, 32, 31, 29, 27, 26, 24, 22, 20, 19, 17, 15, 14, 12                }
  ,
  {
    87, 86, 85, 84, 83, 82, 81, 80, 79, 78, 77, 76, 75, 74, 73, 72, 71, 70, 69, 68, 67, 66, 65, 64, 63, 62, 61, 60, 59, 58, 57, 56, 55, 54, 53, 52, 51, 50, 49, 48, 47, 46, 45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32                }
  ,
  {
    114, 113, 111, 108, 106, 103, 102, 101, 99, 96, 94, 91, 90, 89, 87, 84, 82, 79, 78, 77, 75, 72, 70, 67, 66, 65, 63, 60, 58, 55, 54, 53, 51, 48, 46, 43, 42, 41, 39, 36, 34, 31, 30, 29, 27, 24, 22, 19, 18, 17, 15, 12, 10, 7, 6, 5                }
  ,
  {
    100, 99, 97, 96, 94, 93, 91, 90, 88, 87, 85, 84, 82, 81, 79, 78, 76, 75, 73, 72, 70, 69, 67, 66, 64, 63, 61, 60, 58, 57, 55, 54, 52, 51, 49, 48, 46, 45, 43, 42, 40, 39, 37, 36, 34, 33, 31, 30, 28, 27, 25, 24, 22, 21, 19, 18                }
  ,
  {
    96, 95, 94, 93, 92, 91, 89, 87, 86, 84, 83, 82, 81, 80, 79, 77, 75, 74, 72, 71, 70, 69, 68, 67, 65, 63, 62, 60, 59, 58, 57, 56, 55, 53, 51, 50, 48, 47, 46, 45, 44, 43, 41, 39, 38, 36, 35, 34, 33, 32, 31, 29, 27, 26, 24, 23                }
  ,
  {
    107, 104, 103, 101, 100, 98, 96, 95, 92, 91, 89, 88, 86, 84, 83, 80, 79, 77, 76, 74, 72, 71, 68, 67, 65, 64, 62, 60, 59, 56, 55, 53, 52, 50, 48, 47, 44, 43, 41, 40, 38, 36, 35, 32, 31, 29, 28, 26, 24, 23, 20, 19, 17, 16, 14, 12                }
  ,
  {
    107, 104, 103, 101, 99, 98, 96, 95, 92, 91, 89, 87, 86, 84, 83, 80, 79, 77, 75, 74, 72, 71, 68, 67, 65, 63, 62, 60, 59, 56, 55, 53, 51, 50, 48, 47, 44, 43, 41, 39, 38, 36, 35, 32, 31, 29, 27, 26, 24, 23, 20, 19, 17, 15, 14, 12                }
  ,
  {
    107, 105, 103, 101, 99, 98, 96, 95, 93, 91, 89, 87, 86, 84, 83, 81, 79, 77, 75, 74, 72, 71, 69, 67, 65, 63, 62, 60, 59, 57, 55, 53, 51, 50, 48, 47, 45, 43, 41, 39, 38, 36, 35, 33, 31, 29, 27, 26, 24, 23, 21, 19, 17, 15, 14, 12                }
  ,
  {
    115, 111, 110, 108, 107, 105, 103, 99, 98, 96, 95, 93, 91, 87, 86, 84, 83, 81, 79, 75, 74, 72, 71, 69, 67, 63, 62, 60, 59, 57, 55, 51, 50, 48, 47, 45, 43, 39, 38, 36, 35, 33, 31, 27, 26, 24, 23, 21, 19, 15, 14, 12, 11, 9, 7, 3                }
  ,
  {
    106, 105, 102, 101, 100, 97, 96, 94, 93, 90, 89, 88, 85, 84, 82, 81, 78, 77, 76, 73, 72, 70, 69, 66, 65, 64, 61, 60, 58, 57, 54, 53, 52, 49, 48, 46, 45, 42, 41, 40, 37, 36, 34, 33, 30, 29, 28, 25, 24, 22, 21, 18, 17, 16, 13, 12                }
  ,
  {
    106, 103, 102, 101, 100, 98, 96, 94, 91, 90, 89, 88, 86, 84, 82, 79, 78, 77, 76, 74, 72, 70, 67, 66, 65, 64, 62, 60, 58, 55, 54, 53, 52, 50, 48, 46, 43, 42, 41, 40, 38, 36, 34, 31, 30, 29, 28, 26, 24, 22, 19, 18, 17, 16, 14, 12                }
  ,
  {
    96, 95, 93, 92, 91, 89, 88, 87, 85, 84, 83, 81, 80, 79, 77, 76, 75, 73, 72, 71, 69, 68, 67, 65, 64, 63, 61, 60, 59, 57, 56, 55, 53, 52, 51, 49, 48, 47, 45, 44, 43, 41, 40, 39, 37, 36, 35, 33, 32, 31, 29, 28, 27, 25, 24, 23                }
  ,
  {
    106, 105, 103, 101, 99, 98, 96, 94, 93, 91, 89, 87, 86, 84, 82, 81, 79, 77, 75, 74, 72, 70, 69, 67, 65, 63, 62, 60, 58, 57, 55, 53, 51, 50, 48, 46, 45, 43, 41, 39, 38, 36, 34, 33, 31, 29, 27, 26, 24, 22, 21, 19, 17, 15, 14, 12                }
  ,
  {
    106, 104, 101, 100, 97, 97, 96, 94, 92, 89, 88, 85, 85, 84, 82, 80, 77, 76, 73, 73, 72, 70, 68, 65, 64, 61, 61, 60, 58, 56, 53, 52, 49, 49, 48, 46, 44, 41, 40, 37, 37, 36, 34, 32, 29, 28, 25, 25, 24, 22, 20, 17, 16, 13, 13, 12                }
  ,
  {
    106, 104, 102, 101, 99, 97, 96, 94, 92, 90, 89, 87, 85, 84, 82, 80, 78, 77, 75, 73, 72, 70, 68, 66, 65, 63, 61, 60, 58, 56, 54, 53, 51, 49, 48, 46, 44, 42, 41, 39, 37, 36, 34, 32, 30, 29, 27, 25, 24, 22, 20, 18, 17, 15, 13, 12                }
  ,
  {
    106, 105, 103, 102, 100, 98, 96, 94, 93, 91, 90, 88, 86, 84, 82, 81, 79, 78, 76, 74, 72, 70, 69, 67, 66, 64, 62, 60, 58, 57, 55, 54, 52, 50, 48, 46, 45, 43, 42, 40, 38, 36, 34, 33, 31, 30, 28, 26, 24, 22, 21, 19, 18, 16, 14, 12                }
  ,
  {
    96, 95, 94, 93, 92, 91, 89, 87, 86, 84, 83, 82, 81, 80, 79, 77, 75, 74, 72, 71, 70, 69, 68, 67, 65, 63, 62, 60, 59, 58, 57, 56, 55, 53, 51, 50, 48, 47, 46, 45, 44, 43, 41, 39, 38, 36, 35, 34, 33, 32, 31, 29, 27, 26, 24, 23                }
  ,
  {
    106, 105, 103, 101, 100, 98, 96, 94, 93, 91, 89, 88, 86, 84, 82, 81, 79, 77, 76, 74, 72, 70, 69, 67, 65, 64, 62, 60, 58, 57, 55, 53, 52, 50, 48, 46, 45, 43, 41, 40, 38, 36, 34, 33, 31, 29, 28, 26, 24, 22, 21, 19, 17, 16, 14, 12                }
  ,
  {
    124, 122, 120, 117, 115, 112, 110, 108, 105, 103, 100, 98, 96, 93, 91, 88, 86, 84, 81, 79, 76, 74, 72, 69, 67, 64, 62, 60, 57, 55, 52, 50, 48, 45, 43, 40, 38, 36, 33, 31, 28, 26, 24, 21, 19, 16, 14, 12, 9, 7, 4, 2, 0, 253, 251, 248                }
  ,
  {
    106, 104, 103, 101, 99, 97, 96, 94, 92, 91, 89, 87, 85, 84, 82, 80, 79, 77, 75, 73, 72, 70, 68, 67, 65, 63, 61, 60, 58, 56, 55, 53, 51, 49, 48, 46, 44, 43, 41, 39, 37, 36, 34, 32, 31, 29, 27, 25, 24, 22, 20, 19, 17, 15, 13, 12                }
  ,
  {
    107, 106, 103, 101, 99, 97, 96, 95, 94, 91, 89, 87, 85, 84, 83, 82, 79, 77, 75, 73, 72, 71, 70, 67, 65, 63, 61, 60, 59, 58, 55, 53, 51, 49, 48, 47, 46, 43, 41, 39, 37, 36, 35, 34, 31, 29, 27, 25, 24, 23, 22, 19, 17, 15, 13, 12                }
  ,
  {
    96, 95, 94, 93, 91, 89, 88, 87, 86, 84, 83, 82, 81, 79, 77, 76, 75, 74, 72, 71, 70, 69, 67, 65, 64, 63, 62, 60, 59, 58, 57, 55, 53, 52, 51, 50, 48, 47, 46, 45, 43, 41, 40, 39, 38, 36, 35, 34, 33, 31, 29, 28, 27, 26, 24, 23                }
  ,
  {
    114, 112, 110, 108, 106, 104, 102, 100, 98, 96, 94, 92, 90, 88, 86, 84, 82, 80, 78, 76, 74, 72, 70, 68, 66, 64, 62, 60, 58, 56, 54, 52, 50, 48, 46, 44, 42, 40, 38, 36, 34, 32, 30, 28, 26, 24, 22, 20, 18, 16, 14, 12, 10, 8, 6, 4                }
  ,
  {
    100, 99, 97, 96, 94, 92, 90, 89, 88, 87, 85, 84, 82, 80, 78, 77, 76, 75, 73, 72, 70, 68, 66, 65, 64, 63, 61, 60, 58, 56, 54, 53, 52, 51, 49, 48, 46, 44, 42, 41, 40, 39, 37, 36, 34, 32, 30, 29, 28, 27, 25, 24, 22, 20, 18, 17                }
  ,
  {
    112, 111, 109, 108, 104, 103, 100, 99, 97, 96, 92, 91, 88, 87, 85, 84, 80, 79, 76, 75, 73, 72, 68, 67, 64, 63, 61, 60, 56, 55, 52, 51, 49, 48, 44, 43, 40, 39, 37, 36, 32, 31, 28, 27, 25, 24, 20, 19, 16, 15, 13, 12, 8, 7, 4, 3                }
  ,
  {
    123, 122, 120, 117, 115, 111, 110, 108, 105, 103, 99, 98, 96, 93, 91, 87, 86, 84, 81, 79, 75, 74, 72, 69, 67, 63, 62, 60, 57, 55, 51, 50, 48, 45, 43, 39, 38, 36, 33, 31, 27, 26, 24, 21, 19, 15, 14, 12, 9, 7, 3, 2, 0, 253, 251, 247                }
  ,
  {
    125, 121, 120, 118, 114, 113, 109, 108, 106, 102, 101, 97, 96, 94, 90, 89, 85, 84, 82, 78, 77, 73, 72, 70, 66, 65, 61, 60, 58, 54, 53, 49, 48, 46, 42, 41, 37, 36, 34, 30, 29, 25, 24, 22, 18, 17, 13, 12, 10, 6, 5, 1, 0, 254, 250, 249                }
  ,
  {
    125, 121, 120, 118, 115, 113, 109, 108, 106, 103, 101, 97, 96, 94, 91, 89, 85, 84, 82, 79, 77, 73, 72, 70, 67, 65, 61, 60, 58, 55, 53, 49, 48, 46, 43, 41, 37, 36, 34, 31, 29, 25, 24, 22, 19, 17, 13, 12, 10, 7, 5, 1, 0, 254, 251, 249                }
  ,
  {
    123, 122, 120, 116, 115, 111, 110, 108, 104, 103, 99, 98, 96, 92, 91, 87, 86, 84, 80, 79, 75, 74, 72, 68, 67, 63, 62, 60, 56, 55, 51, 50, 48, 44, 43, 39, 38, 36, 32, 31, 27, 26, 24, 20, 19, 15, 14, 12, 8, 7, 3, 2, 0, 252, 251, 247                }
  ,
  {
    107, 104, 103, 102, 99, 98, 96, 95, 92, 91, 90, 87, 86, 84, 83, 80, 79, 78, 75, 74, 72, 71, 68, 67, 66, 63, 62, 60, 59, 56, 55, 54, 51, 50, 48, 47, 44, 43, 42, 39, 38, 36, 35, 32, 31, 30, 27, 26, 24, 23, 20, 19, 18, 15, 14, 12                }
  ,
  {
    107, 104, 103, 101, 100, 97, 96, 95, 92, 91, 89, 88, 85, 84, 83, 80, 79, 77, 76, 73, 72, 71, 68, 67, 65, 64, 61, 60, 59, 56, 55, 53, 52, 49, 48, 47, 44, 43, 41, 40, 37, 36, 35, 32, 31, 29, 28, 25, 24, 23, 20, 19, 17, 16, 13, 12                }
  ,
  {
    106, 104, 102, 100, 99, 97, 96, 94, 92, 90, 88, 87, 85, 84, 82, 80, 78, 76, 75, 73, 72, 70, 68, 66, 64, 63, 61, 60, 58, 56, 54, 52, 51, 49, 48, 46, 44, 42, 40, 39, 37, 36, 34, 32, 30, 28, 27, 25, 24, 22, 20, 18, 16, 15, 13, 12                }
  ,
  {
    125, 123, 120, 118, 115, 113, 111, 108, 106, 103, 101, 99, 96, 94, 91, 89, 87, 84, 82, 79, 77, 75, 72, 70, 67, 65, 63, 60, 58, 55, 53, 51, 48, 46, 43, 41, 39, 36, 34, 31, 29, 27, 24, 22, 19, 17, 15, 12, 10, 7, 5, 3, 0, 254, 251, 249                }
};

    /// <summary>
    /// General MIDI instrument, used in Program Change messages.
    /// </summary>
    /// <remarks>
    /// <para>The MIDI protocol defines a Program Change message, which can be used to switch a
    /// device among "presets".  The General MIDI specification further standardizes those presets
    /// into the specific instruments in this enum.  General-MIDI-compliant devices will
    /// have these particular instruments; non-GM devices may have other instruments.</para>
    /// <para>MIDI instruments are one-indexed in the spec, but they're zero-indexed in code, so
    /// we have them zero-indexed here.</para>
    /// <para>This enum has extension methods, such as <see cref="InstrumentExtensionMethods.Name"/>
    /// and <see cref="InstrumentExtensionMethods.IsValid"/>, defined in
    /// <see cref="InstrumentExtensionMethods"/>.</para>
    /// </remarks>
    public enum Instrument
    {
        // Piano Family:

        /// <summary>General MIDI instrument 0 ("Acoustic Grand Piano").</summary>
        AcousticGrandPiano = 0,
        /// <summary>General MIDI instrument 1 ("Bright Acoustic Piano").</summary>
        BrightAcousticPiano = 1,
        /// <summary>General MIDI instrument 2 ("Electric Grand Piano").</summary>
        ElectricGrandPiano = 2,
        /// <summary>General MIDI instrument 3 ("Honky Tonk Piano").</summary>
        HonkyTonkPiano = 3,
        /// <summary>General MIDI instrument 4 ("Electric Piano 1").</summary>
        ElectricPiano1 = 4,
        /// <summary>General MIDI instrument 5 ("Electric Piano 2").</summary>
        ElectricPiano2 = 5,
        /// <summary>General MIDI instrument 6 ("Harpsichord").</summary>
        Harpsichord = 6,
        /// <summary>General MIDI instrument 7 ("Clavinet").</summary>
        Clavinet = 7,

        // Chromatic Percussion Family:

        /// <summary>General MIDI instrument 8 ("Celesta").</summary>
        Celesta = 8,
        /// <summary>General MIDI instrument 9 ("Glockenspiel").</summary>
        Glockenspiel = 9,
        /// <summary>General MIDI instrument 10 ("Music Box").</summary>
        MusicBox = 10,
        /// <summary>General MIDI instrument 11 ("Vibraphone").</summary>
        Vibraphone = 11,
        /// <summary>General MIDI instrument 12 ("Marimba").</summary>
        Marimba = 12,
        /// <summary>General MIDI instrument 13 ("Xylophone").</summary>
        Xylophone = 13,
        /// <summary>General MIDI instrument 14 ("Tubular Bells").</summary>
        TubularBells = 14,
        /// <summary>General MIDI instrument 15 ("Dulcimer").</summary>
        Dulcimer = 15,

        // Organ Family:

        /// <summary>General MIDI instrument 16 ("Drawbar Organ").</summary>
        DrawbarOrgan = 16,
        /// <summary>General MIDI instrument 17 ("Percussive Organ").</summary>
        PercussiveOrgan = 17,
        /// <summary>General MIDI instrument 18 ("Rock Organ").</summary>
        RockOrgan = 18,
        /// <summary>General MIDI instrument 19 ("Church Organ").</summary>
        ChurchOrgan = 19,
        /// <summary>General MIDI instrument 20 ("Reed Organ").</summary>
        ReedOrgan = 20,
        /// <summary>General MIDI instrument 21 ("Accordion").</summary>
        Accordion = 21,
        /// <summary>General MIDI instrument 22 ("Harmonica").</summary>
        Harmonica = 22,
        /// <summary>General MIDI instrument 23 ("Tango Accordion").</summary>
        TangoAccordion = 23,

        // Guitar Family:

        /// <summary>General MIDI instrument 24 ("Acoustic Guitar (nylon)").</summary>
        AcousticGuitarNylon = 24,
        /// <summary>General MIDI instrument 25 ("Acoustic Guitar (steel)").</summary>
        AcousticGuitarSteel = 25,
        /// <summary>General MIDI instrument 26 ("Electric Guitar (jazz)").</summary>
        ElectricGuitarJazz = 26,
        /// <summary>General MIDI instrument 27 ("Electric Guitar (clean)").</summary>
        ElectricGuitarClean = 27,
        /// <summary>General MIDI instrument 28 ("Electric Guitar (muted)").</summary>
        ElectricGuitarMuted = 28,
        /// <summary>General MIDI instrument 29 ("Overdriven Guitar").</summary>
        OverdrivenGuitar = 29,
        /// <summary>General MIDI instrument 30 ("Distortion Guitar").</summary>
        DistortionGuitar = 30,
        /// <summary>General MIDI instrument 31 ("Guitar Harmonics").</summary>
        GuitarHarmonics = 31,

        // Bass Family:

        /// <summary>General MIDI instrument 32 ("Acoustic Bass").</summary>
        AcousticBass = 32,
        /// <summary>General MIDI instrument 33 ("Electric Bass (finger)").</summary>
        ElectricBassFinger = 33,
        /// <summary>General MIDI instrument 34 ("Electric Bass (pick)").</summary>
        ElectricBassPick = 34,
        /// <summary>General MIDI instrument 35 ("Fretless Bass").</summary>
        FretlessBass = 35,
        /// <summary>General MIDI instrument 36 ("Slap Bass 1").</summary>
        SlapBass1 = 36,
        /// <summary>General MIDI instrument 37 ("Slap Bass 2").</summary>
        SlapBass2 = 37,
        /// <summary>General MIDI instrument 38 ("Synth Bass 1").</summary>
        SynthBass1 = 38,
        /// <summary>General MIDI instrument 39("Synth Bass 2").</summary>
        SynthBass2 = 39,

        // Strings Family:

        /// <summary>General MIDI instrument 40 ("Violin").</summary>
        Violin = 40,
        /// <summary>General MIDI instrument 41 ("Viola").</summary>
        Viola = 41,
        /// <summary>General MIDI instrument 42 ("Cello").</summary>
        Cello = 42,
        /// <summary>General MIDI instrument 43 ("Contrabass").</summary>
        Contrabass = 43,
        /// <summary>General MIDI instrument 44 ("Tremolo Strings").</summary>
        TremoloStrings = 44,
        /// <summary>General MIDI instrument 45 ("Pizzicato Strings").</summary>
        PizzicatoStrings = 45,
        /// <summary>General MIDI instrument 46 ("Orchestral Harp").</summary>
        OrchestralHarp = 46,
        /// <summary>General MIDI instrument 47 ("Timpani").</summary>
        Timpani = 47,

        // Ensemble Family:

        /// <summary>General MIDI instrument 48 ("String Ensemble 1").</summary>
        StringEnsemble1 = 48,
        /// <summary>General MIDI instrument 49 ("String Ensemble 2").</summary>
        StringEnsemble2 = 49,
        /// <summary>General MIDI instrument 50 ("Synth Strings 1").</summary>
        SynthStrings1 = 50,
        /// <summary>General MIDI instrument 51 ("Synth Strings 2").</summary>
        SynthStrings2 = 51,
        /// <summary>General MIDI instrument 52 ("Choir Aahs").</summary>
        ChoirAahs = 52,
        /// <summary>General MIDI instrument 53 ("Voice oohs").</summary>
        VoiceOohs = 53,
        /// <summary>General MIDI instrument 54 ("Synth Voice").</summary>
        SynthVoice = 54,
        /// <summary>General MIDI instrument 55 ("Orchestra Hit").</summary>
        OrchestraHit = 55,

        // Brass Family:

        /// <summary>General MIDI instrument 56 ("Trumpet").</summary>
        Trumpet = 56,
        /// <summary>General MIDI instrument 57 ("Trombone").</summary>
        Trombone = 57,
        /// <summary>General MIDI instrument 58 ("Tuba").</summary>
        Tuba = 58,
        /// <summary>General MIDI instrument 59 ("Muted Trumpet").</summary>
        MutedTrumpet = 59,
        /// <summary>General MIDI instrument 60 ("French Horn").</summary>
        FrenchHorn = 60,
        /// <summary>General MIDI instrument 61 ("Brass Section").</summary>
        BrassSection = 61,
        /// <summary>General MIDI instrument 62 ("Synth Brass 1").</summary>
        SynthBrass1 = 62,
        /// <summary>General MIDI instrument 63 ("Synth Brass 2").</summary>
        SynthBrass2 = 63,

        // Reed Family:

        /// <summary>General MIDI instrument 64 ("Soprano Sax").</summary>
        SopranoSax = 64,
        /// <summary>General MIDI instrument 65 ("Alto Sax").</summary>
        AltoSax = 65,
        /// <summary>General MIDI instrument 66 ("Tenor Sax").</summary>
        TenorSax = 66,
        /// <summary>General MIDI instrument 67 ("Baritone Sax").</summary>
        BaritoneSax = 67,
        /// <summary>General MIDI instrument 68 ("Oboe").</summary>
        Oboe = 68,
        /// <summary>General MIDI instrument 69 ("English Horn").</summary>
        EnglishHorn = 69,
        /// <summary>General MIDI instrument 70 ("Bassoon").</summary>
        Bassoon = 70,
        /// <summary>General MIDI instrument 71 ("Clarinet").</summary>
        Clarinet = 71,

        // Pipe Family:

        /// <summary>General MIDI instrument 72 ("Piccolo").</summary>
        Piccolo = 72,
        /// <summary>General MIDI instrument 73 ("Flute").</summary>
        Flute = 73,
        /// <summary>General MIDI instrument 74 ("Recorder").</summary>
        Recorder = 74,
        /// <summary>General MIDI instrument 75 ("PanFlute").</summary>
        PanFlute = 75,
        /// <summary>General MIDI instrument 76 ("Blown Bottle").</summary>
        BlownBottle = 76,
        /// <summary>General MIDI instrument 77 ("Shakuhachi").</summary>
        Shakuhachi = 77,
        /// <summary>General MIDI instrument 78 ("Whistle").</summary>
        Whistle = 78,
        /// <summary>General MIDI instrument 79 ("Ocarina").</summary>
        Ocarina = 79,

        // Synth Lead Family:

        /// <summary>General MIDI instrument 80 ("Lead 1 (square)").</summary>
        Lead1Square = 80,
        /// <summary>General MIDI instrument 81 ("Lead 2 (sawtooth)").</summary>
        Lead2Sawtooth = 81,
        /// <summary>General MIDI instrument 82 ("Lead 3 (calliope)").</summary>
        Lead3Calliope = 82,
        /// <summary>General MIDI instrument 83 ("Lead 4 (chiff)").</summary>
        Lead4Chiff = 83,
        /// <summary>General MIDI instrument 84 ("Lead 5 (charang)").</summary>
        Lead5Charang = 84,
        /// <summary>General MIDI instrument 85 ("Lead 6 (voice)").</summary>
        Lead6Voice = 85,
        /// <summary>General MIDI instrument 86 ("Lead 7 (fifths)").</summary>
        Lead7Fifths = 86,
        /// <summary>General MIDI instrument 87 ("Lead 8 (bass + lead)").</summary>
        Lead8BassPlusLead = 87,

        // Synth Pad Family:

        /// <summary>General MIDI instrument 88 ("Pad 1 (new age)").</summary>
        Pad1NewAge = 88,
        /// <summary>General MIDI instrument 89 ("Pad 2 (warm)").</summary>
        Pad2Warm = 89,
        /// <summary>General MIDI instrument 90 ("Pad 3 (polysynth)").</summary>
        Pad3Polysynth = 90,
        /// <summary>General MIDI instrument 91 ("Pad 4 (choir)").</summary>
        Pad4Choir = 91,
        /// <summary>General MIDI instrument 92 ("Pad 5 (bowed)").</summary>
        Pad5Bowed = 92,
        /// <summary>General MIDI instrument 93 ("Pad 6 (metallic)").</summary>
        Pad6Metallic = 93,
        /// <summary>General MIDI instrument 94 ("Pad 7 (halo)").</summary>
        Pad7Halo = 94,
        /// <summary>General MIDI instrument 95 ("Pad 8 (sweep)").</summary>
        Pad8Sweep = 95,

        // Synth Effects Family:

        /// <summary>General MIDI instrument 96 ("FX 1 (rain)").</summary>
        FX1Rain = 96,
        /// <summary>General MIDI instrument 97 ("FX 2 (soundtrack)").</summary>
        FX2Soundtrack = 97,
        /// <summary>General MIDI instrument 98 ("FX 3 (crystal)").</summary>
        FX3Crystal = 98,
        /// <summary>General MIDI instrument 99 ("FX 4 (atmosphere)").</summary>
        FX4Atmosphere = 99,
        /// <summary>General MIDI instrument 100 ("FX 5 (brightness)").</summary>
        FX5Brightness = 100,
        /// <summary>General MIDI instrument 101 ("FX 6 (goblins)").</summary>
        FX6Goblins = 101,
        /// <summary>General MIDI instrument 102 ("FX 7 (echoes)").</summary>
        FX7Echoes = 102,
        /// <summary>General MIDI instrument 103 ("FX 8 (sci-fi)").</summary>
        FX8SciFi = 103,

        // Ethnic Family:

        /// <summary>General MIDI instrument 104 ("Sitar").</summary>
        Sitar = 104,
        /// <summary>General MIDI instrument 105 ("Banjo").</summary>
        Banjo = 105,
        /// <summary>General MIDI instrument 106 ("Shamisen").</summary>
        Shamisen = 106,
        /// <summary>General MIDI instrument 107 ("Koto").</summary>
        Koto = 107,
        /// <summary>General MIDI instrument 108 ("Kalimba").</summary>
        Kalimba = 108,
        /// <summary>General MIDI instrument 109 ("Bagpipe").</summary>
        Bagpipe = 109,
        /// <summary>General MIDI instrument 110 ("Fiddle").</summary>
        Fiddle = 110,
        /// <summary>General MIDI instrument 111 ("Shanai").</summary>
        Shanai = 111,

        // Percussive Family:

        /// <summary>General MIDI instrument 112 ("Tinkle Bell").</summary>
        TinkleBell = 112,
        /// <summary>General MIDI instrument 113 (Agogo"").</summary>
        Agogo = 113,
        /// <summary>General MIDI instrument 114 ("Steel Drums").</summary>
        SteelDrums = 114,
        /// <summary>General MIDI instrument 115 ("Woodblock").</summary>
        Woodblock = 115,
        /// <summary>General MIDI instrument 116 ("Taiko Drum").</summary>
        TaikoDrum = 116,
        /// <summary>General MIDI instrument 117 ("Melodic Tom").</summary>
        MelodicTom = 117,
        /// <summary>General MIDI instrument 118 ("Synth Drum").</summary>
        SynthDrum = 118,
        /// <summary>General MIDI instrument 119 ("Reverse Cymbal").</summary>
        ReverseCymbal = 119,

        // Sound Effects Family:

        /// <summary>General MIDI instrument 120 ("Guitar Fret Noise").</summary>
        GuitarFretNoise = 120,
        /// <summary>General MIDI instrument 121 ("Breath Noise").</summary>
        BreathNoise = 121,
        /// <summary>General MIDI instrument 122 ("Seashore").</summary>
        Seashore = 122,
        /// <summary>General MIDI instrument 123 ("Bird Tweet").</summary>
        BirdTweet = 123,
        /// <summary>General MIDI instrument 124 ("Telephone Ring").</summary>
        TelephoneRing = 124,
        /// <summary>General MIDI instrument 125 ("Helicopter").</summary>
        Helicopter = 125,
        /// <summary>General MIDI instrument 126 ("Applause").</summary>
        Applause = 126,
        /// <summary>General MIDI instrument 127 ("Gunshot").</summary>
        Gunshot = 127
    };

    //Public
    //Check the Midi's file folder for different songs
    public string midiFilePath;
	public string bankFilePath;

	//Private 
	private float[] sampleBuffer;
	private float gain = 1f;
	//private MidiFileSequencer midiSequencer;
	private Synthesizer midiStreamSynthesizer;

    public GameObject sequencerPrefab;

    public class MidiNote
	{
		public int channel;
        public int note;
        public int velocity;
        public bool onoff;
	};
	
	// Awake is called when the script instance
	// is being loaded.
	void Awake ()
	{
        int bufferSize;
        int numBuffers;
        AudioSettings.GetDSPBufferSize(out bufferSize, out numBuffers);
		midiStreamSynthesizer = new Synthesizer(AudioSettings.outputSampleRate, 2, 32, 16, 40);
		midiStreamSynthesizer.LoadBank(bankFilePath);
        midiStreamSynthesizer.MasterVolume = 3f;

        //sampleBuffer = new float[midiStreamSynthesizer.BufferSize];
        sampleBuffer = new float[2048];

        //midiSequencer = new MidiFileSequencer (midiStreamSynthesizer);
        //MidiFile myfile = new MidiFile(midiFilePath);
        //midiSequencer.LoadMidi (myfile);
    }

    // Start is called just before any of the
    // Update methods is called the first time.
    void Start ()
	{
        //midiSequencer.Play();
    }

    // Update is called every frame, if the
    // MonoBehaviour is enabled.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12) || Input.GetButton("Fire3"))
        {
            UnityEngine.VR.InputTracking.Recenter();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit) == false)
            {
                // start position where we are currently looking
                Quaternion quaternion = InputTracking.GetLocalRotation(VRNode.CenterEye);

                Vector3 euler = quaternion.eulerAngles;

                float positionAngle = euler.y * Mathf.Deg2Rad;
                float radius = 20f;

                // the X & Y position for this angle are calculated using Sin & Cos
                float x = Mathf.Sin(positionAngle) * radius;
                float y = Mathf.Cos(positionAngle) * radius;
                Vector3 pos = new Vector3(x, 0, y) + transform.position;
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, transform.position - pos) * Quaternion.Euler(0, 180, 0);

                GameObject sequencer = Instantiate(sequencerPrefab, pos, rot) as GameObject;
                sequencer.GetComponent<Sequencer>().synth = this;
            }
        }
    }

    // See http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour.OnAudioFilterRead.html for reference code
    //	If OnAudioFilterRead is implemented, Unity will insert a custom filter into the audio DSP chain.
    //
    //	The filter is inserted in the same order as the MonoBehaviour script is shown in the inspector. 	
    //	OnAudioFilterRead is called everytime a chunk of audio is routed thru the filter (this happens frequently, every ~20ms depending on the samplerate and platform). 
    //	The audio data is an array of floats ranging from [-1.0f;1.0f] and contains audio from the previous filter in the chain or the AudioClip on the AudioSource. 
    //	If this is the first filter in the chain and a clip isn't attached to the audio source this filter will be 'played'. 
    //	That way you can use the filter as the audio clip, procedurally generating audio.
    //
    //	If OnAudioFilterRead is implemented a VU meter will show up in the inspector showing the outgoing samples level. 
    //	The process time of the filter is also measured and the spent milliseconds will show up next to the VU Meter 
    //	(it turns red if the filter is taking up too much time, so the mixer will starv audio data). 
    //	Also note, that OnAudioFilterRead is called on a different thread from the main thread (namely the audio thread) 
    //	so calling into many Unity functions from this function is not allowed ( a warning will show up ). 	
    private void OnAudioFilterRead (float[] data, int channels)
	{
        //midiSequencer.FillMidiEventQueue();

        //This uses the Unity specific float method we added to get the buffer
        midiStreamSynthesizer.GetNext (sampleBuffer);
			
		for (int i = 0; i < data.Length; i++) {
			data [i] = sampleBuffer [i] * gain;
		}
	}

    public void NoteOn(int channel, int note, int velocity)
    {
        midiStreamSynthesizer.NoteOn(channel, note, velocity);
    }

    public void NoteOff(int channel, int note)
    {
        midiStreamSynthesizer.NoteOff(channel, note);
    }

    public void ChangeInstrument(int channel, int program)
    {
        midiStreamSynthesizer.ProcessMidiMessage(channel, 0xC0, program, 0x00);
    }

    public void AllNotesOff()
    {
        midiStreamSynthesizer.NoteOffAll(true);
    }
}
