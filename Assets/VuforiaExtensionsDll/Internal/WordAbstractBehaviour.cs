using System;
using UnityEngine;

namespace Vuforia
{
	public abstract class WordAbstractBehaviour : TrackableBehaviour
	{
		[HideInInspector, SerializeField]
		private WordTemplateMode mMode;

		[HideInInspector, SerializeField]
		private string mSpecificWord;

		private Word mWord;

		public Word Word
		{
			get
			{
				return this.mWord;
			}
		}

		public bool IsTemplateMode
		{
			get
			{
				return this.mMode == WordTemplateMode.Template;
			}
		}

		internal bool IsSpecificWordMode
		{
			get
			{
				return this.mMode == WordTemplateMode.SpecificWord;
			}
		}

		public string SpecificWord
		{
			get
			{
				return this.mSpecificWord;
			}
		}

		protected override void InternalUnregisterTrackable()
		{
			this.mTrackable = (this.mWord = null);
		}

		internal unsafe void InitializeWord(Word word)
		{
			this.mWord = word;
			this.mTrackable = word;
			this.mTrackableName = word.StringValue;
            Vector2 arg_4D_0 = word.Size;
			Vector3 vector = Vector3.one;
			MeshFilter component = base.GetComponent<MeshFilter>();
			if (component != null)
			{
				vector = component.sharedMesh.bounds.size;
			}
			float num = arg_4D_0.y / vector.z;
			base.transform.localScale = new Vector3(num, num, num);
		}

		private void OnValidate()
		{
			if (this.IsSpecificWordMode && this.mSpecificWord.Length == 0)
			{
				Debug.LogWarning("Empty string used as word: This trackable and its augmentation will never be selected at runtime.");
			}
		}
	}
}
