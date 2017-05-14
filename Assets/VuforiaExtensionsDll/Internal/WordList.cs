using System;

namespace Vuforia
{
	public abstract class WordList
	{
		public abstract bool LoadWordListFile(string relativePath);

		public abstract bool LoadWordListFile(string relativePath, VuforiaUnity.StorageType storageType);

		public abstract int AddWordsFromFile(string relativePath);

		public abstract int AddWordsFromFile(string path, VuforiaUnity.StorageType storageType);

		public abstract bool AddWord(string word);

		public abstract bool RemoveWord(string word);

		public abstract bool ContainsWord(string word);

		public abstract bool UnloadAllLists();

		public abstract WordFilterMode GetFilterMode();

		public abstract bool SetFilterMode(WordFilterMode mode);

		public abstract bool LoadFilterListFile(string relativePath);

		public abstract bool LoadFilterListFile(string path, VuforiaUnity.StorageType storageType);

		public abstract bool AddWordToFilterList(string word);

		public abstract bool RemoveWordFromFilterList(string word);

		public abstract bool ClearFilterList();

		public abstract int GetFilterListWordCount();

		public abstract string GetFilterListWord(int index);
	}
}
