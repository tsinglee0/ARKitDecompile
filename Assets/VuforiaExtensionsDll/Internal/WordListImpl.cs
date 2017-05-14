using System;
using System.Runtime.InteropServices;

namespace Vuforia
{
	internal class WordListImpl : WordList
	{
		public override bool LoadWordListFile(string relativePath)
		{
			return this.LoadWordListFile(relativePath, VuforiaUnity.StorageType.STORAGE_APPRESOURCE);
		}

		public override bool LoadWordListFile(string path, VuforiaUnity.StorageType storageType)
		{
			path = VuforiaRuntimeUtilities.GetStoragePath(path, storageType);
			return VuforiaWrapper.Instance.WordListLoadWordList(path, (int)storageType) == 1;
		}

		public override int AddWordsFromFile(string relativePath)
		{
			return this.AddWordsFromFile(relativePath, VuforiaUnity.StorageType.STORAGE_APPRESOURCE);
		}

		public override int AddWordsFromFile(string path, VuforiaUnity.StorageType storageType)
		{
			path = VuforiaRuntimeUtilities.GetStoragePath(path, storageType);
			return VuforiaWrapper.Instance.WordListAddWordsFromFile(path, (int)storageType);
		}

		public override bool AddWord(string word)
		{
			IntPtr intPtr = Marshal.StringToHGlobalUni(word);
			bool arg_1B_0 = VuforiaWrapper.Instance.WordListAddWordU(intPtr) == 1;
			Marshal.FreeHGlobal(intPtr);
			return arg_1B_0;
		}

		public override bool RemoveWord(string word)
		{
			IntPtr intPtr = Marshal.StringToHGlobalUni(word);
			bool arg_1B_0 = VuforiaWrapper.Instance.WordListRemoveWordU(intPtr) == 1;
			Marshal.FreeHGlobal(intPtr);
			return arg_1B_0;
		}

		public override bool ContainsWord(string word)
		{
			IntPtr intPtr = Marshal.StringToHGlobalUni(word);
			bool arg_1B_0 = VuforiaWrapper.Instance.WordListContainsWordU(intPtr) == 1;
			Marshal.FreeHGlobal(intPtr);
			return arg_1B_0;
		}

		public override bool UnloadAllLists()
		{
			return VuforiaWrapper.Instance.WordListUnloadAllLists() == 1;
		}

		public override WordFilterMode GetFilterMode()
		{
			return (WordFilterMode)VuforiaWrapper.Instance.WordListGetFilterMode();
		}

		public override bool SetFilterMode(WordFilterMode mode)
		{
			return VuforiaWrapper.Instance.WordListSetFilterMode((int)mode) == 1;
		}

		public override bool LoadFilterListFile(string relativePath)
		{
			return this.LoadFilterListFile(relativePath, VuforiaUnity.StorageType.STORAGE_APPRESOURCE);
		}

		public override bool LoadFilterListFile(string path, VuforiaUnity.StorageType storageType)
		{
			path = VuforiaRuntimeUtilities.GetStoragePath(path, storageType);
			return VuforiaWrapper.Instance.WordListLoadFilterList(path, (int)storageType) == 1;
		}

		public override bool AddWordToFilterList(string word)
		{
			IntPtr intPtr = Marshal.StringToHGlobalUni(word);
			bool arg_1B_0 = VuforiaWrapper.Instance.WordListAddWordToFilterListU(intPtr) == 1;
			Marshal.FreeHGlobal(intPtr);
			return arg_1B_0;
		}

		public override bool RemoveWordFromFilterList(string word)
		{
			IntPtr intPtr = Marshal.StringToHGlobalUni(word);
			bool arg_1B_0 = VuforiaWrapper.Instance.WordListRemoveWordFromFilterListU(intPtr) == 1;
			Marshal.FreeHGlobal(intPtr);
			return arg_1B_0;
		}

		public override bool ClearFilterList()
		{
			return VuforiaWrapper.Instance.WordListClearFilterList() == 1;
		}

		public override int GetFilterListWordCount()
		{
			return VuforiaWrapper.Instance.WordListGetFilterListWordCount();
		}

		public override string GetFilterListWord(int index)
		{
			return Marshal.PtrToStringUni(VuforiaWrapper.Instance.WordListGetFilterListWordU(index));
		}
	}
}
