using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class FindMissingScripts : EditorWindow
{
	[MenuItem("Window/FindMissingScripts")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(FindMissingScripts));
	}

	public void OnGUI()
	{
		if (GUILayout.Button("Find Missing Scripts in selected prefabs"))
			FindInSelected();

		if (GUILayout.Button("Remove Missing Scripts in selected prefabs"))
			Remove();

	}

	[MenuItem("Window/RemoveMissingScripts")]
	private static void FindAndRemoveMissingInSelected() {

		// EditorUtility.CollectDeepHierarchy does not include inactive children
		var deeperSelection = Selection.gameObjects.SelectMany(go => go.GetComponentsInChildren<Transform>(true))
			.Select(t => t.gameObject);
		var prefabs = new HashSet<Object>();
		int compCount = 0;
		int goCount = 0;
		foreach (var go in deeperSelection) {
			int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
			if (count > 0) {
				if (PrefabUtility.IsPartOfAnyPrefab(go)) {
					RecursivePrefabSource(go, prefabs, ref compCount, ref goCount);
					count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);
					// if count == 0 the missing scripts has been removed from prefabs
					if (count == 0)
						continue;
					// if not the missing scripts must be prefab overrides on this instance
				}

				Undo.RegisterCompleteObjectUndo(go, "Remove missing scripts");
				GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
				compCount += count;
				goCount++;
			}
		}

		Debug.Log($"Found and removed {compCount} missing scripts from {goCount} GameObjects");

	}

	private static void RecursivePrefabSource(GameObject instance, HashSet<Object> prefabs, ref int compCount,
	ref int goCount) {
		var source = PrefabUtility.GetCorrespondingObjectFromSource(instance);
		// Only visit if source is valid, and hasn't been visited before
		if (source == null || !prefabs.Add(source))
			return;

		// go deep before removing, to differantiate local overrides from missing in source
		RecursivePrefabSource(source, prefabs, ref compCount, ref goCount);

		int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(source);
		if (count > 0) {
			Undo.RegisterCompleteObjectUndo(source, "Remove missing scripts");
			GameObjectUtility.RemoveMonoBehavioursWithMissingScript(source);
			compCount += count;
			goCount++;
		}
	}

	private static void Remove() {

		for (int i = 0; i < Selection.gameObjects.Length; i++) {

			var gameObject = Selection.gameObjects[i];

			// We must use the GetComponents array to actually detect missing components
			var components = gameObject.GetComponents<Component>();

			// Create a serialized object so that we can edit the component list
			var serializedObject = new SerializedObject(gameObject);
			// Find the component list property
			var prop = serializedObject.FindProperty("m_Component");

			// Track how many components we've removed
			int r = 0;

			// Iterate over all components
			for (int j = 0; j < components.Length; j++) {
				// Check if the ref is null
				if (components[j] == null) {
					// If so, remove from the serialized component array
					prop.DeleteArrayElementAtIndex(j - r);
					// Increment removed count
					r++;
				}
			}

			// Apply our changes to the game object
			serializedObject.ApplyModifiedProperties();
		}

	}

	private static void FindInSelected()
	{
		GameObject[] go = Selection.gameObjects;
		int go_count = 0, components_count = 0, missing_count = 0;
		foreach (GameObject g in go)
		{
			go_count++;
			Component[] components = g.GetComponents<Component>();
			for (int i = 0; i < components.Length; i++)
			{
				components_count++;
				if (components[i] == null)
				{
					missing_count++;
					string s = g.name;
					Transform t = g.transform;
					while (t.parent != null) 
					{
						s = t.parent.name +"/"+s;
						t = t.parent;
					}
					Debug.Log (s + " has an empty script attached in position: " + i, g);
				}
			}
		}

		Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
	}
}