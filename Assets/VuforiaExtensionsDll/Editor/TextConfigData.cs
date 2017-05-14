using System;
using System.Collections.Generic;

namespace Vuforia.EditorClasses
{
	internal class TextConfigData
	{
		public struct DictionaryData
		{
			public string BinaryFile;
		}

		public struct WordListData
		{
			public string TextFile;
		}

		private Dictionary<string, TextConfigData.DictionaryData> mDictionaries;

		private Dictionary<string, TextConfigData.WordListData> mWordLists;

		public int NumDictionaries
		{
			get
			{
				return this.mDictionaries.Count;
			}
		}

		public int NumWordLists
		{
			get
			{
				return this.mWordLists.Count;
			}
		}

		public TextConfigData()
		{
			this.mDictionaries = new Dictionary<string, TextConfigData.DictionaryData>();
			this.mWordLists = new Dictionary<string, TextConfigData.WordListData>();
		}

		public void SetDictionaryData(TextConfigData.DictionaryData data, string name)
		{
			this.mDictionaries[name] = data;
		}

		public void SetWordListData(TextConfigData.WordListData data, string name)
		{
			this.mWordLists[name] = data;
		}

		public TextConfigData.DictionaryData GetDictionaryData(string name)
		{
			return this.mDictionaries[name];
		}

		public TextConfigData.WordListData GetWordListData(string name)
		{
			return this.mWordLists[name];
		}

		public void CopyDictionaryNames(string[] arrayToFill, int index)
		{
			this.mDictionaries.Keys.CopyTo(arrayToFill, index);
		}

		public void CopyDictionaryNamesAndFiles(string[] namesArray, string[] filesArray, int index)
		{
			foreach (KeyValuePair<string, TextConfigData.DictionaryData> current in this.mDictionaries)
			{
				namesArray[index] = current.Key;
				filesArray[index] = current.Value.BinaryFile;
				index++;
			}
		}

		public void CopyWordListNames(string[] arrayToFill, int index)
		{
			this.mWordLists.Keys.CopyTo(arrayToFill, index);
		}

		public void CopyWordListNamesAndFiles(string[] namesArray, string[] filesArray, int index)
		{
			foreach (KeyValuePair<string, TextConfigData.WordListData> current in this.mWordLists)
			{
				namesArray[index] = current.Key;
				filesArray[index] = current.Value.TextFile;
				index++;
			}
		}
	}
}
