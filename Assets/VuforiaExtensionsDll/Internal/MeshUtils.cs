using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
	internal class MeshUtils
	{
		public static Mesh UpdateMesh(VuforiaManagerImpl.MeshData meshData, Mesh oldMesh, bool setNormalsUpwards, bool swapYZ)
		{
			if (meshData.numVertexValues == 0 || meshData.numTriangleIndices == 0)
			{
				return null;
			}
			if (oldMesh == null)
			{
				oldMesh = new Mesh();
			}
			else
			{
				oldMesh.Clear();
			}
			MeshUtils.CopyPositions(meshData.positionsArray, meshData.numVertexValues, oldMesh, swapYZ);
			MeshUtils.CopyTriangles(meshData.triangleIdxArray, meshData.numTriangleIndices, oldMesh);
			if (meshData.hasNormals == 1)
			{
				MeshUtils.CopyNormals(meshData.normalsArray, meshData.numVertexValues, oldMesh, swapYZ);
			}
			else if (setNormalsUpwards)
			{
				Vector3[] array = new Vector3[meshData.numVertexValues / 3];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = Vector3.up;
				}
				oldMesh.normals = array;
			}
			else
			{
				oldMesh.RecalculateNormals();
			}
			if (meshData.hasTexCoords == 1)
			{
				MeshUtils.CopyTexCoords(meshData.texCoordsArray, meshData.numVertexValues * 2 / 3, oldMesh);
			}
			else
			{
				Vector2[] array2 = new Vector2[meshData.numVertexValues / 3];
				for (int j = 0; j < meshData.numVertexValues / 3; j++)
				{
					array2[j] = new Vector2(0.5f, 0.5f);
				}
				oldMesh.uv = array2;
			}
			return oldMesh;
		}

		private static void CopyPositions(IntPtr positionsArray, int numVertexValues, Mesh mesh, bool swapYZ)
		{
			Vector3[] array = new Vector3[numVertexValues / 3];
			float[] array2 = new float[numVertexValues];
			Marshal.Copy(positionsArray, array2, 0, numVertexValues);
			int num = 0;
			int num2 = swapYZ ? 2 : 1;
			int num3 = swapYZ ? 1 : 2;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Vector3(array2[num], array2[num + num2], array2[num + num3]);
				num += 3;
			}
			mesh.vertices = array;
		}

		private static void CopyNormals(IntPtr normalsArray, int numVertexValues, Mesh mesh, bool swapYZ)
		{
			Vector3[] array = new Vector3[numVertexValues / 3];
			float[] array2 = new float[numVertexValues];
			Marshal.Copy(normalsArray, array2, 0, numVertexValues);
			int num = 0;
			int num2 = swapYZ ? 2 : 1;
			int num3 = swapYZ ? 1 : 2;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Vector3(array2[num], array2[num + num2], array2[num + num3]);
				num += 3;
			}
			mesh.normals = array;
		}

		private static void CopyTexCoords(IntPtr texCoordsArray, int numTexCoordValues, Mesh mesh)
		{
			Vector2[] array = new Vector2[numTexCoordValues / 2];
			float[] array2 = new float[numTexCoordValues];
			Marshal.Copy(texCoordsArray, array2, 0, numTexCoordValues);
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Vector3(array2[num], array2[num + 1]);
				num += 2;
			}
			mesh.uv = array;
		}

		private static void CopyTriangles(IntPtr triangleIdxArray, int numTriangleIndices, Mesh mesh)
		{
			int[] array = new int[numTriangleIndices];
			byte[] array2 = new byte[numTriangleIndices * 2];
			Marshal.Copy(triangleIdxArray, array2, 0, numTriangleIndices * 2);
			for (int i = 0; i < numTriangleIndices; i += 3)
			{
				int num = i * 2;
				array[i] = (int)BitConverter.ToUInt16(array2, num);
				array[i + 1] = (int)BitConverter.ToUInt16(array2, num + 4);
				array[i + 2] = (int)BitConverter.ToUInt16(array2, num + 2);
			}
			mesh.triangles = array;
		}
	}
}
