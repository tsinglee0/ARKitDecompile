using System;

namespace Vuforia
{
	public interface ITextRecoEventHandler
	{
		void OnInitialized();

		void OnWordDetected(WordResult word);

		void OnWordLost(Word word);
	}
}
