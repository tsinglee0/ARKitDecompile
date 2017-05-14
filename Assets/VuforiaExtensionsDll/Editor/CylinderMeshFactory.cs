using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vuforia.EditorClasses
{
	internal class CylinderMeshFactory
	{
		private readonly float mTopRadius;

		private readonly float mBottomRadius;

		private readonly float mSideLength;

		private readonly float mSmallRadius;

		private readonly float mBigRadius;

		private readonly bool mFlip;

		private readonly float mSinTheta;

		private readonly float mUMax;

		private readonly float mVSmall;

		private readonly float mSidelengthBig;

		private readonly float mSidelengthSmall;

		private List<Vector3> mPositions;

		private List<Vector3> mNormals;

		private List<Vector2> mUVs;

		public static Mesh CreateCylinderMesh(float sideLength, float topDiameter, float bottomDiameter, int numPerimeterVertices, bool hasTopGeometry, bool hasBottomGeometry, bool insideMaterial = false)
		{
			return CylinderMeshFactory.CreateCylinderMesh(null, sideLength, topDiameter, bottomDiameter, numPerimeterVertices, hasTopGeometry, hasBottomGeometry, insideMaterial);
		}

		public static Mesh CreateCylinderMesh(Mesh oldMesh, float sideLength, float topDiameter, float bottomDiameter, int numPerimeterVertices, bool hasTopGeometry, bool hasBottomGeometry, bool insideMaterial = false)
		{
			return new CylinderMeshFactory(sideLength, topDiameter, bottomDiameter).CreateCylinderMesh(oldMesh, numPerimeterVertices, hasTopGeometry, hasBottomGeometry, insideMaterial);
		}

		private CylinderMeshFactory(float sideLength, float topDiameter, float bottomDiameter)
		{
			this.mSideLength = sideLength;
			this.mTopRadius = topDiameter * 0.5f;
			this.mBottomRadius = bottomDiameter * 0.5f;
			this.mFlip = (this.mTopRadius >= this.mBottomRadius);
			if (this.mFlip)
			{
				this.mBigRadius = this.mTopRadius;
				this.mSmallRadius = this.mBottomRadius;
			}
			else
			{
				this.mBigRadius = this.mBottomRadius;
				this.mSmallRadius = this.mTopRadius;
			}
			if (this.mBigRadius - this.mSmallRadius >= sideLength)
			{
				this.mSinTheta = 1f;
			}
			else
			{
				this.mSinTheta = (this.mBigRadius - this.mSmallRadius) / sideLength;
			}
			this.mSidelengthSmall = (this.IsCylinder() ? 0f : (sideLength * this.mSmallRadius / (this.mBigRadius - this.mSmallRadius)));
			this.mSidelengthBig = this.mSidelengthSmall + sideLength;
			float num = 3.14159274f * this.mSinTheta;
			bool flag = (double)num < 1.5707963267948966;
			this.mUMax = (this.IsCylinder() ? (3.14159274f * this.mBigRadius) : (flag ? (this.mSidelengthBig * Mathf.Sin(num)) : this.mSidelengthBig));
			this.mVSmall = Mathf.Cos(num) * (flag ? this.mSidelengthSmall : this.mSidelengthBig);
		}

		private Mesh CreateCylinderMesh(Mesh oldMesh, int numPerimeterVertices, bool hasTopGeometry, bool hasBottomGeometry, bool insideMaterial)
		{
			this.mPositions = new List<Vector3>();
			this.mNormals = new List<Vector3>();
			this.mUVs = new List<Vector2>();
			float arg_46_0 = this.ComputeHeight(this.mSideLength);
			List<Vector3> list = CylinderMeshFactory.CreatePerimeterPositions(0f, this.mBottomRadius, numPerimeterVertices);
			List<Vector3> list2 = CylinderMeshFactory.CreatePerimeterPositions(arg_46_0, this.mTopRadius, numPerimeterVertices);
			List<int> list3 = this.AddBodyTriangles(list, list2);
			int num = 1;
			List<int> list4 = null;
			if (hasBottomGeometry && this.mBottomRadius > 0f)
			{
				list4 = this.AddSealingTriangles(list, false);
				num++;
			}
			List<int> list5 = null;
			if (hasTopGeometry && this.mTopRadius > 0f)
			{
				list5 = this.AddSealingTriangles(list2, true);
				num++;
			}
			Mesh mesh = oldMesh ?? new Mesh();
			mesh.vertices = this.mPositions.ToArray();
			mesh.normals = this.mNormals.ToArray();
			mesh.uv = this.mUVs.ToArray();
			mesh.subMeshCount = insideMaterial ? (2 * num) : num;
			int[] source = null;
			int[] array = null;
			int[] array2 = null;
			if (insideMaterial)
			{
				source = list3.Skip(list3.Count / 2).ToArray<int>();
				list3 = list3.Take(list3.Count / 2).ToList<int>();
				if (list4 != null)
				{
					array2 = list4.Take(list4.Count / 2).ToArray<int>();
					list4 = list4.Skip(list4.Count / 2).ToList<int>();
				}
				if (list5 != null)
				{
					array = list5.Skip(list5.Count / 2).ToArray<int>();
					list5 = list5.Take(list5.Count / 2).ToList<int>();
				}
			}
			mesh.SetTriangles(list3.ToArray(), 0);
			if (list4 != null)
			{
				mesh.SetTriangles(list4.ToArray(), 1);
			}
			if (list5 != null)
			{
				mesh.SetTriangles(list5.ToArray(), num - 1);
			}
			if (insideMaterial)
			{
				mesh.SetTriangles(source.ToArray<int>(), num);
				if (array2 != null)
				{
					mesh.SetTriangles(array2, num + 1);
				}
				if (array != null)
				{
					mesh.SetTriangles(array, num + num - 1);
				}
			}
			return mesh;
		}

		private List<int> AddBodyTriangles(List<Vector3> bottomPerimeterVertices, List<Vector3> topPerimeterVertices)
		{
			int count = bottomPerimeterVertices.Count;
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < count; i++)
			{
				Vector3 vector = bottomPerimeterVertices[i];
				Vector3 vector2 = topPerimeterVertices[i];
				Vector3 vector3 = new Vector3(vector.x + vector2.x, 0f, vector.z + vector2.z);
				vector3.Normalize();
				Vector3 vector4 = Vector3.Cross(Vector3.up, vector3) * -1f;
				Vector3 vector5 = vector2 - vector;
				Vector3 item = Quaternion.AngleAxis(Vector3.Angle(Vector3.up, vector5), vector4) * vector3;
				list.Add(item);
			}
			float num = -3.14159274f;
			float num2 = 6.28318548f / (float)count;
			for (int j = 0; j <= count; j++)
			{
				int index = j % count;
				this.mPositions.Add(bottomPerimeterVertices[index]);
				this.mNormals.Add(list[index]);
				this.mUVs.Add(this.ConvertToUVCoordinates(num, 0f));
				num += num2;
			}
			num = -3.14159274f;
			for (int k = 0; k <= count; k++)
			{
				int index2 = k % count;
				this.mPositions.Add(topPerimeterVertices[index2]);
				this.mNormals.Add(list[index2]);
				this.mUVs.Add(this.ConvertToUVCoordinates(num, this.mSideLength));
				num += num2;
			}
			int count2 = this.mPositions.Count;
			for (int l = 0; l < this.mPositions.Count; l++)
			{
				this.mNormals.Add(-this.mNormals[l]);
			}
			this.mPositions.AddRange(this.mPositions);
			this.mUVs.AddRange(this.mUVs);
			List<int> list2 = new List<int>();
			for (int m = 1; m <= count; m++)
			{
				list2.AddRange(new int[]
				{
					m - 1,
					m,
					m + count
				});
				list2.AddRange(new int[]
				{
					m + count + 1,
					m + count,
					m
				});
			}
			for (int n = 1; n <= count; n++)
			{
				list2.AddRange(new int[]
				{
					count2 + n - 1,
					count2 + n + count,
					count2 + n
				});
				list2.AddRange(new int[]
				{
					count2 + n + count + 1,
					count2 + n,
					count2 + n + count
				});
			}
			return list2;
		}

		private List<int> AddSealingTriangles(List<Vector3> perimeterVertices, bool isTop)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < 2; i++)
			{
				bool flag = i == 0;
				Vector3 item = flag ? Vector3.up : Vector3.down;
				int count = this.mPositions.Count;
				this.mPositions.Add(new Vector3(0f, perimeterVertices[0].y, 0f));
				int count2 = this.mPositions.Count;
				int num = count2 + perimeterVertices.Count - 1;
				this.mPositions.AddRange(perimeterVertices);
				for (int j = 0; j <= perimeterVertices.Count; j++)
				{
					this.mNormals.Add(item);
				}
				this.mUVs.AddRange(CylinderMeshFactory.CreatePerimeterUVCoordinates(perimeterVertices.Count, isTop));
				for (int k = count2; k < num; k++)
				{
					List<int> arg_ED_0 = list;
					IEnumerable<int> arg_ED_1;
					if (!flag)
					{
						int[] expr_C3 = new int[3];
						expr_C3[0] = count;
						expr_C3[1] = k + 1;
						arg_ED_1 = expr_C3;
						expr_C3[2] = k;
					}
					else
					{
						int[] expr_DC = new int[3];
						expr_DC[0] = count;
						expr_DC[1] = k;
						arg_ED_1 = expr_DC;
						expr_DC[2] = k + 1;
					}
					arg_ED_0.AddRange(arg_ED_1);
				}
				List<int> arg_12E_0 = list;
				IEnumerable<int> arg_12E_1;
				if (!flag)
				{
					int[] expr_108 = new int[3];
					expr_108[0] = count;
					expr_108[1] = count2;
					arg_12E_1 = expr_108;
					expr_108[2] = num;
				}
				else
				{
					int[] expr_11F = new int[3];
					expr_11F[0] = count;
					expr_11F[1] = num;
					arg_12E_1 = expr_11F;
					expr_11F[2] = count2;
				}
				arg_12E_0.AddRange(arg_12E_1);
			}
			return list;
		}

		private static List<Vector3> CreatePerimeterPositions(float height, float radius, int numPerimeterVertices)
		{
			List<Vector3> list = new List<Vector3>();
			float num = -1.57079637f;
			float num2 = 6.28318548f / (float)numPerimeterVertices;
			for (int i = 0; i < numPerimeterVertices; i++)
			{
				list.Add(new Vector3(radius * Mathf.Sin(num), height, radius * Mathf.Cos(num)));
				num += num2;
			}
			return list;
		}

		private static List<Vector2> CreatePerimeterUVCoordinates(int numPerimeterVertices, bool isTop)
		{
			List<Vector2> list = new List<Vector2>();
			list.Add(new Vector2(0.5f, 0.5f));
			float num = -1.57079637f;
			float num2 = 6.28318548f / (float)numPerimeterVertices;
			for (int i = 0; i < numPerimeterVertices; i++)
			{
				float num3 = Mathf.Cos(num) * 0.5f + 0.5f;
				float num4 = Mathf.Sin(num) * 0.5f + 0.5f;
				num3 = 1f - num3;
				if (!isTop)
				{
					num4 = 1f - num4;
				}
				list.Add(new Vector2(num3, num4));
				num += num2;
			}
			return list;
		}

		private Vector2 ConvertToUVCoordinates(float angleInRadians, float slantedYPos)
		{
			if (this.mFlip)
			{
				angleInRadians = -angleInRadians;
				slantedYPos = this.mSidelengthSmall + slantedYPos;
			}
			else
			{
				slantedYPos = this.mSidelengthBig - slantedYPos;
			}
			float num;
			float num2;
			if (this.IsCylinder())
			{
				num = angleInRadians * this.mBigRadius;
				num2 = slantedYPos;
			}
			else
			{
				num = slantedYPos * Mathf.Sin(angleInRadians * this.mSinTheta);
				num2 = slantedYPos * Mathf.Cos(angleInRadians * this.mSinTheta);
			}
			num = num / (2f * this.mUMax) + 0.5f;
			num2 = (num2 - this.mVSmall) / (this.mSidelengthBig - this.mVSmall);
			if (this.mFlip)
			{
				num2 = 1f - num2;
				num = 1f - num;
			}
			return new Vector2(num, num2);
		}

		private bool IsCylinder()
		{
			return (double)Math.Abs(this.mBigRadius - this.mSmallRadius) < 1E-05;
		}

		private float ComputeHeight(float sideLength)
		{
			return Mathf.Sqrt(sideLength * sideLength - (this.mBigRadius - this.mSmallRadius) * (this.mBigRadius - this.mSmallRadius));
		}
	}
}
