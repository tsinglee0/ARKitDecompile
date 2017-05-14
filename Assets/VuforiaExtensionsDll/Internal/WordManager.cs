using System;
using System.Collections.Generic;

namespace Vuforia
{
	public abstract class WordManager
	{
		public abstract IEnumerable<WordResult> GetActiveWordResults();

		public abstract IEnumerable<WordResult> GetNewWords();

		public abstract IEnumerable<Word> GetLostWords();

		public abstract bool TryGetWordBehaviour(Word word, out WordAbstractBehaviour behaviour);

		public abstract IEnumerable<WordAbstractBehaviour> GetTrackableBehaviours();

		public abstract void DestroyWordBehaviour(WordAbstractBehaviour behaviour, bool destroyGameObject = true);
	}
}
