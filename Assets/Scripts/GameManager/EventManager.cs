using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void EncounterEvent(string tag);
    public static event EncounterEvent OnEncounter;

    public static void TriggerEncounter(string tag)
    {
        OnEncounter?.Invoke(tag);
    }
}

