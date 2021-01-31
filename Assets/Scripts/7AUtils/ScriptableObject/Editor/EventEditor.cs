// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent))]
public class EventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        GameEvent e = target as GameEvent;
        if (GUILayout.Button("Raise"))
            e.Raise();
    }
}

[CustomEditor(typeof(IntEvent))]
public class IntEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        IntEvent e = target as IntEvent;

        if (GUILayout.Button("Raise"))
            e.Raise(e.testRaiseData);
    }
}

[CustomEditor(typeof(BoolEvent))]
public class BoolEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        BoolEvent e = target as BoolEvent;

        if (GUILayout.Button("Raise"))
            e.Raise(e.testRaiseData);
    }
}

[CustomEditor(typeof(FloatEvent))]
public class FloatEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        FloatEvent e = target as FloatEvent;

        if (GUILayout.Button("Raise"))
            e.Raise(e.testRaiseData);
    }
}

[CustomEditor(typeof(StringEvent))]
public class StringEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;

        StringEvent e = target as StringEvent;

        if (GUILayout.Button("Raise"))
            e.Raise(e.testRaiseData);
    }
}

//public class GenericEventEditor<T, V> : Editor where T : GameEvent<V>
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        GUI.enabled = Application.isPlaying;

//        T e = target as T;

//        Debug.Log(e);

//        if (GUILayout.Button("Raise"))
//            e.Raise(e.testRaiseData);
//    }
//}

//[CustomEditor(typeof(IntEvent))]
//public class IntEventEditor : GenericEventEditor<GameEvent<IntEvent>, IntEvent> {

//}