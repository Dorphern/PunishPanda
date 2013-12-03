using UnityEngine;
using UnityEditor;
using System.Collections;

class ParticleSystemColorField : EditorWindow 
{
	public Color startColor = Color.red;
	[MenuItem("Examples/Particle System Color Change")]

	static void Init() 
	{
		ParticleSystemColorField window = GetWindow(typeof(ParticleSystemColorField)) as ParticleSystemColorField;
		window.position = new Rect(0,0,170,60);
		window.Show();
	}
	void OnGUI() 
	{
		startColor = EditorGUI.ColorField(new Rect(3,3,position.width - 6, 15), "Start Color:", startColor);
	
		if(GUI.Button(new Rect(3,50,position.width-6, 30),"Change colors!"))
			ChangeColor();
	}
  
	void ChangeColor() 
	{
		if(Selection.activeGameObject)
		{
			foreach(GameObject t in Selection.gameObjects)
			{
				if(t.particleSystem)
				{
					t.particleSystem.startColor = startColor;
					
					SerializedObject so = new SerializedObject(t.particleSystem);
					so.FindProperty("InitialModule.startColor.minMaxState").intValue = 0;
					so.ApplyModifiedProperties();
				}
			}
		}
	}
}