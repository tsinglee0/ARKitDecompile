using System;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	[CustomEditor(typeof(CloudRecoAbstractBehaviour), true)]
	public class CloudRecoEditor : Editor
	{
		private SerializedProperty mAccessKey;

		private SerializedProperty mSecretKey;

		private void OnEnable()
		{
			this.mAccessKey = base.serializedObject.FindProperty("AccessKey");
			this.mSecretKey = base.serializedObject.FindProperty("SecretKey");
		}

		public override void OnInspectorGUI()
		{
			using (base.serializedObject.Edit())
			{
				EditorGUILayout.HelpBox("The Access and Secret Keys are used to authenticate your app with the Cloud Reco service, and to identify which Cloud database is queried. Do not share your keys with untrusted 3rd parties and take appropriate steps to protect them within your application code. If you based your app on the Cloud Reco sample, be sure to replace the sample keys with your own.", MessageType.Info);
				EditorGUILayout.PropertyField(this.mAccessKey, new GUIContent("Access Key"), new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(this.mSecretKey, new GUIContent("Secret Key"), new GUILayoutOption[0]);
			}
		}

		public void OnSceneGUI()
		{
			Component arg_30_0 = (CloudRecoAbstractBehaviour)base.target;
			GUIStyle expr_10 = new GUIStyle();
			expr_10.alignment = TextAnchor.LowerRight;
			expr_10.fontSize = 18;
			expr_10.normal.textColor = Color.white;
			GUIStyle gUIStyle = expr_10;
			Handles.Label(arg_30_0.transform.position, "Cloud\nRecognition", gUIStyle);
		}
	}
}
