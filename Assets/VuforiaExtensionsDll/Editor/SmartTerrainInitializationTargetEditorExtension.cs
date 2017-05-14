using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal static class SmartTerrainInitializationTargetEditorExtension
	{
		private enum TransformHandle
		{
			NONE,
			TRANSLATION,
			ROTATION
		}

		private static SmartTerrainInitializationTargetEditorExtension.TransformHandle sTransformHandle;

		internal static void DrawInspectorForInitializationTarget(SerializedDataSetTrackable serializedDst, bool sizeOnlyKnownAtRuntime)
		{
			if (serializedDst.InitializeSmartTerrain)
			{
				GUILayout.Box("", new GUILayoutOption[]
				{
					GUILayout.ExpandWidth(true),
					GUILayout.Height(1f)
				});
			}
			bool flag = SceneManager.Instance.ExtendedTrackingEnabledOnATarget();
			if (flag)
			{
				EditorGUILayout.HelpBox("Smart Terrain cannot be enabled at the same time as Extended Tracking.", MessageType.Info);
			}
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			bool enabled = GUI.enabled;
			GUI.enabled = enabled && !flag;
			bool initializeSmartTerrain = serializedDst.InitializeSmartTerrain;
			EditorGUILayout.PropertyField(serializedDst.InitializeSmartTerrainProperty, new GUIContent("Enable Smart Terrain"), new GUILayoutOption[0]);
			GUI.enabled = enabled;
			bool flag2 = false;
			if (initializeSmartTerrain != serializedDst.InitializeSmartTerrain && serializedDst.InitializeSmartTerrain && serializedDst.ReconstructionToInitialize == null)
			{
				ReconstructionFromTargetAbstractBehaviour[] array = (ReconstructionFromTargetAbstractBehaviour[])UnityEngine.Object.FindObjectsOfType(typeof(ReconstructionFromTargetAbstractBehaviour));
				if (array.Length != 0)
				{
					serializedDst.ReconstructionToInitialize = array[0];
					SmartTerrainInitializationTargetEditorExtension.InitOccluderBoundsIfUnset(serializedDst);
				}
				else
				{
					flag2 = true;
				}
			}
			if (serializedDst.InitializeSmartTerrain)
			{
				if (GUILayout.Button("NEW", new GUILayoutOption[0]))
				{
					flag2 = true;
				}
				if (flag2)
				{
                    UnityEngine.Object @object = AssetDatabase.LoadAssetAtPath("Assets/Vuforia/Prefabs/SmartTerrain/SmartTerrain.prefab", typeof(ReconstructionAbstractBehaviour));
					if (@object != null)
					{
						ReconstructionAbstractBehaviour reconstructionAbstractBehaviour = PrefabUtility.InstantiatePrefab(@object) as ReconstructionAbstractBehaviour;
						if (reconstructionAbstractBehaviour != null)
						{
							ReconstructionFromTargetAbstractBehaviour component = reconstructionAbstractBehaviour.GetComponent<ReconstructionFromTargetAbstractBehaviour>();
							if (component != null)
							{
								GameObject gameObject = serializedDst.GetGameObjects()[0];
								float x = gameObject.transform.lossyScale.x;
								reconstructionAbstractBehaviour.ScaleEditorMeshesByFactor(x);
								reconstructionAbstractBehaviour.ScaleEditorPropPositionsByFactor(x);
								reconstructionAbstractBehaviour.gameObject.name = "SmartTerrain_" + gameObject.name;
								reconstructionAbstractBehaviour.gameObject.transform.position = gameObject.transform.position;
								reconstructionAbstractBehaviour.gameObject.transform.rotation = gameObject.transform.rotation;
								serializedDst.ReconstructionToInitialize = component;
								SmartTerrainInitializationTargetEditorExtension.InitOccluderBoundsIfUnset(serializedDst);
								ReconstructionEditor.EditorConfigureTarget(reconstructionAbstractBehaviour, null);
							}
						}
					}
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
				if (serializedDst.ReconstructionToInitialize != null)
				{
					EditorGUILayout.HelpBox("Press 'NEW' to instantiate a new Smart Terrain prefab in the scene.", MessageType.None);
				}
				ReconstructionFromTargetAbstractBehaviour reconstructionFromTargetAbstractBehaviour = (ReconstructionFromTargetAbstractBehaviour)EditorGUILayout.ObjectField("Smart Terrain Instance", serializedDst.ReconstructionToInitialize, typeof(ReconstructionFromTargetAbstractBehaviour), true, new GUILayoutOption[0]);
				if (reconstructionFromTargetAbstractBehaviour != serializedDst.ReconstructionToInitialize)
				{
					serializedDst.ReconstructionToInitialize = reconstructionFromTargetAbstractBehaviour;
					if (reconstructionFromTargetAbstractBehaviour != null)
					{
						SmartTerrainInitializationTargetEditorExtension.InitOccluderBoundsIfUnset(serializedDst);
					}
				}
				if (!(reconstructionFromTargetAbstractBehaviour != null))
				{
					serializedDst.InitializeSmartTerrain = false;
					return;
				}
				if (!sizeOnlyKnownAtRuntime || !serializedDst.AutoSetOccluderFromTargetSize)
				{
					EditorGUILayout.HelpBox("Change these values to the size of the object you initialize from, e.g. if there is a border around your target.\n The stage inside the occluder box will be ignored during scanning", MessageType.None);
					serializedDst.SmartTerrainOccluderBoundsMin = EditorGUILayout.Vector3Field("Occluder Bounds Min", serializedDst.SmartTerrainOccluderBoundsMin, new GUILayoutOption[0]);
					serializedDst.SmartTerrainOccluderBoundsMax = EditorGUILayout.Vector3Field("Occluder Bounds Max", serializedDst.SmartTerrainOccluderBoundsMax, new GUILayoutOption[0]);
					if (sizeOnlyKnownAtRuntime)
					{
						EditorGUILayout.HelpBox("Check this box to automatically set the occluder box depending on the cloud target size at runtime.", MessageType.None);
						EditorGUILayout.PropertyField(serializedDst.AutoSetOccluderFromTargetSizeProperty, new GUIContent("Automatically to cloud target size"), new GUILayoutOption[0]);
					}
					else
					{
						EditorGUILayout.Space();
						if (GUILayout.Button("Reset occluder bounds to target size", new GUILayoutOption[0]))
						{
							SmartTerrainInitializationTargetEditorExtension.SetOccluderBoundsDefaultValues(serializedDst);
						}
						EditorGUILayout.Space();
					}
					EditorGUILayout.HelpBox("Use the surface offset below if the primary surface is not co-planar with the initialization target", MessageType.None);
					bool isSmartTerrainOccluderOffset = serializedDst.IsSmartTerrainOccluderOffset;
					EditorGUILayout.PropertyField(serializedDst.IsSmartTerrainOccluderOffsetProperty, new GUIContent("Enable Offset Surface"), new GUILayoutOption[0]);
					if (isSmartTerrainOccluderOffset != serializedDst.IsSmartTerrainOccluderOffset && serializedDst.IsSmartTerrainOccluderOffset)
					{
						Quaternion smartTerrainOccluderRotation = serializedDst.SmartTerrainOccluderRotation;
						if (smartTerrainOccluderRotation.x == 0f && smartTerrainOccluderRotation.y == 0f && smartTerrainOccluderRotation.z == 0f && smartTerrainOccluderRotation.w == 0f)
						{
							serializedDst.SmartTerrainOccluderRotation = Quaternion.identity;
						}
					}
					if (serializedDst.IsSmartTerrainOccluderOffset)
					{
						if (SmartTerrainInitializationTargetEditorExtension.sTransformHandle != SmartTerrainInitializationTargetEditorExtension.TransformHandle.NONE && Tools.current != Tool.None && Tools.current != Tool.View)
						{
							SmartTerrainInitializationTargetEditorExtension.sTransformHandle = SmartTerrainInitializationTargetEditorExtension.TransformHandle.NONE;
						}
						EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
						serializedDst.SmartTerrainOccluderOffset = EditorGUILayout.Vector3Field("Translation", serializedDst.SmartTerrainOccluderOffset, new GUILayoutOption[0]);
						if (GUILayout.Button(((SmartTerrainInitializationTargetEditorExtension.sTransformHandle == SmartTerrainInitializationTargetEditorExtension.TransformHandle.TRANSLATION) ? "Hide" : "Show") + " Gizmo", new GUILayoutOption[0]))
						{
							if (SmartTerrainInitializationTargetEditorExtension.sTransformHandle != SmartTerrainInitializationTargetEditorExtension.TransformHandle.TRANSLATION)
							{
								Tools.current = 0;
								SmartTerrainInitializationTargetEditorExtension.sTransformHandle = SmartTerrainInitializationTargetEditorExtension.TransformHandle.TRANSLATION;
							}
							else
							{
								SmartTerrainInitializationTargetEditorExtension.sTransformHandle = SmartTerrainInitializationTargetEditorExtension.TransformHandle.NONE;
							}
						}
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
						Vector3 eulerAngles = serializedDst.SmartTerrainOccluderRotation.eulerAngles;
						Vector3 vector = EditorGUILayout.Vector3Field("Rotation", eulerAngles, new GUILayoutOption[0]);
						if (vector != eulerAngles)
						{
							serializedDst.SmartTerrainOccluderRotation = Quaternion.Euler(vector);
						}
						if (GUILayout.Button(((SmartTerrainInitializationTargetEditorExtension.sTransformHandle == SmartTerrainInitializationTargetEditorExtension.TransformHandle.ROTATION) ? "Hide" : "Show") + " Gizmo", new GUILayoutOption[0]))
						{
							if (SmartTerrainInitializationTargetEditorExtension.sTransformHandle != SmartTerrainInitializationTargetEditorExtension.TransformHandle.ROTATION)
							{
								Tools.current = 0;
								SmartTerrainInitializationTargetEditorExtension.sTransformHandle = SmartTerrainInitializationTargetEditorExtension.TransformHandle.ROTATION;
							}
							else
							{
								SmartTerrainInitializationTargetEditorExtension.sTransformHandle = SmartTerrainInitializationTargetEditorExtension.TransformHandle.NONE;
							}
						}
						EditorGUILayout.EndHorizontal();
						return;
					}
				}
			}
			else
			{
				EditorGUILayout.EndHorizontal();
			}
		}

		internal static void DrawSceneGUIForInitializationTarget(SerializedDataSetTrackable serializedDst, bool sizeOnlyKnownAtRuntime)
		{
			if (SmartTerrainInitializationTargetEditorExtension.sTransformHandle != SmartTerrainInitializationTargetEditorExtension.TransformHandle.NONE && Tools.current != Tool.None && Tools.current != Tool.View)
			{
				SmartTerrainInitializationTargetEditorExtension.sTransformHandle = SmartTerrainInitializationTargetEditorExtension.TransformHandle.NONE;
			}
			using (serializedDst.Edit())
			{
				if (serializedDst.InitializeSmartTerrain && serializedDst.ReconstructionToInitialize != null && (!sizeOnlyKnownAtRuntime || !serializedDst.AutoSetOccluderFromTargetSize) && serializedDst.IsSmartTerrainOccluderOffset)
				{
					using (List<GameObject>.Enumerator enumerator = serializedDst.GetGameObjects().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Transform transform = enumerator.Current.transform;
							if (SmartTerrainInitializationTargetEditorExtension.sTransformHandle == SmartTerrainInitializationTargetEditorExtension.TransformHandle.TRANSLATION)
							{
								Handles.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
								Vector3 smartTerrainOccluderOffset = serializedDst.SmartTerrainOccluderOffset;
								Vector3 vector = Handles.PositionHandle(smartTerrainOccluderOffset, serializedDst.SmartTerrainOccluderRotation);
								if (vector != smartTerrainOccluderOffset)
								{
									serializedDst.SmartTerrainOccluderOffset = vector;
								}
							}
							if (SmartTerrainInitializationTargetEditorExtension.sTransformHandle == SmartTerrainInitializationTargetEditorExtension.TransformHandle.ROTATION)
							{
								Vector3 vector2 = transform.rotation * serializedDst.SmartTerrainOccluderOffset;
								Handles.matrix = Matrix4x4.TRS(transform.position + vector2, transform.rotation, Vector3.one);
								Quaternion smartTerrainOccluderRotation = serializedDst.SmartTerrainOccluderRotation;
								Quaternion quaternion = Handles.RotationHandle(smartTerrainOccluderRotation, Vector3.zero);
								if (quaternion != smartTerrainOccluderRotation)
								{
									serializedDst.SmartTerrainOccluderRotation = quaternion;
								}
							}
							Handles.matrix = Matrix4x4.identity;
						}
					}
				}
			}
		}

		private static void InitOccluderBoundsIfUnset(SerializedDataSetTrackable serializedDst)
		{
			if (serializedDst.SmartTerrainOccluderBoundsMin == Vector3.zero && serializedDst.SmartTerrainOccluderBoundsMax == Vector3.zero)
			{
				SmartTerrainInitializationTargetEditorExtension.SetOccluderBoundsDefaultValues(serializedDst);
			}
		}

		private static void SetOccluderBoundsDefaultValues(SerializedDataSetTrackable serializedDst)
		{
			serializedDst.SetDefaultOccluderBounds();
		}
	}
}
