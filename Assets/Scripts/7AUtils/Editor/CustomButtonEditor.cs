using UnityEditor;

[CustomEditor(typeof(CustomButton))]
public class MenuButtonEditor : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {
        CustomButton targetMenuButton = (CustomButton)target;

        targetMenuButton.scaleOnPressed = EditorGUILayout.Toggle("Squeeze on pressed", targetMenuButton.scaleOnPressed);
        
		targetMenuButton.isRepeatable = EditorGUILayout.Toggle("Repeatable", targetMenuButton.isRepeatable);

		if(targetMenuButton.isRepeatable)
		{
			targetMenuButton.startRepeatTime = EditorGUILayout.FloatField("Start repeat time", targetMenuButton.startRepeatTime);
			targetMenuButton.repeatTime = EditorGUILayout.FloatField("Repeat time interval", targetMenuButton.repeatTime);
		}


		// Show default inspector property editor
		//DrawDefaultInspector();
		base.OnInspectorGUI();
    }
}