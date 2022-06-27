//using UnityEngine;
//using UnityEditor;


//[CustomEditor(typeof(MeshFilter))]
//public class FixMeshRotationEditor : Editor {

//    GameObject targetTransform;

//    public override void OnInspectorGUI() {

//        if (GUILayout.Button("Rotate X")) {

//            MeshFilter meshFilter = (MeshFilter)target;
//            Mesh mesh = meshFilter.sharedMesh;
//            Vector3[] vertices = mesh.vertices;
//            Vector3[] newVertices = new Vector3[vertices.Length];
//            Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);

//            for (int i = 0; i < vertices.Length; i++)
//                newVertices[i] = rotation * vertices[i];

//            mesh.vertices = newVertices;
//            mesh.RecalculateNormals();
//            mesh.RecalculateBounds();
//            EditorUtility.SetDirty(meshFilter);

//        }

//        if (GUILayout.Button("Rotate Y")) {

//            MeshFilter meshFilter = (MeshFilter)target;
//            Mesh mesh = meshFilter.sharedMesh;
//            Vector3[] vertices = mesh.vertices;
//            Vector3[] newVertices = new Vector3[vertices.Length];
//            Quaternion rotation = Quaternion.Euler(0f, -90f, 0f);

//            for (int i = 0; i < vertices.Length; i++)
//                newVertices[i] = rotation * vertices[i];

//            mesh.vertices = newVertices;
//            mesh.RecalculateNormals();
//            mesh.RecalculateBounds();
//            EditorUtility.SetDirty(meshFilter);

//        }

//        if (GUILayout.Button("Rotate Z")) {

//            MeshFilter meshFilter = (MeshFilter)target;
//            Mesh mesh = meshFilter.sharedMesh;
//            Vector3[] vertices = mesh.vertices;
//            Vector3[] newVertices = new Vector3[vertices.Length];
//            Quaternion rotation = Quaternion.Euler(0f, 0f, -90f);

//            for (int i = 0; i < vertices.Length; i++)
//                newVertices[i] = rotation * vertices[i];

//            mesh.vertices = newVertices;
//            mesh.RecalculateNormals();
//            mesh.RecalculateBounds();
//            EditorUtility.SetDirty(meshFilter);

//        }

//        targetTransform = (GameObject)EditorGUILayout.ObjectField("Target Transform", targetTransform, typeof(GameObject));

//        if (GUILayout.Button("Reset Axis")) {

//            targetTransform.transform.localRotation = Quaternion.identity;

//        }

//    }

//}