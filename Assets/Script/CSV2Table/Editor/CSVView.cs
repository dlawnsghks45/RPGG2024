using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CSVView : EditorWindow 
{
	TextAsset csv = null;
	string[][] arr = null;

	[MenuItem("Window/CSV View")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(CSVView));
	}

	void OnGUI()
	{
		TextAsset newCsv = EditorGUILayout.ObjectField("CSV", csv, typeof(TextAsset), false) as TextAsset;
		if(newCsv != csv)
		{
			csv = newCsv;
			arr = CsvParser2.Parse(csv.text);
            
		}
        if (GUILayout.Button("Refresh") && csv != null)
        {
            arr = CsvParser2.Parse(csv.text);
        }
        if (GUILayout.Button("Save ItemDatabase") && csv != null)
        {
            string PAth = AssetDatabase.GetAssetPath(newCsv);
            //Debug.Log(PAth);
            System.IO.File.AppendAllText(PAth, arr.ToString());
            //arr = CsvParser2.Parse(csv.text);
            //Debug.Log("saved");
        }

        if (csv == null)
			return;

		if(arr == null)
			arr = CsvParser2.Parse(csv.text);

		for(int i = 0 ; i < arr.Length ; i++)
		{
			EditorGUILayout.BeginHorizontal();
			for(int j = 0 ; j < arr[i].Length ; j++)
			{
				EditorGUILayout.TextField(arr[j][i]);
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}
