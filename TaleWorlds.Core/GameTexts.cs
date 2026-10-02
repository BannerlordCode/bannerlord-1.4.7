using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000078 RID: 120
	public static class GameTexts
	{
		// Token: 0x0600083E RID: 2110 RVA: 0x0001B7BC File Offset: 0x000199BC
		public static void Initialize(GameTextManager gameTextManager)
		{
			GameTexts._gameTextManager = gameTextManager;
			GameTexts.InitializeGlobalTags();
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001B7C9 File Offset: 0x000199C9
		public static TextObject FindText(string id, string variation = null)
		{
			return GameTexts._gameTextManager.FindText(id, variation);
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0001B7D7 File Offset: 0x000199D7
		public static bool TryGetText(string id, out TextObject textObject, string variation = null)
		{
			return GameTexts._gameTextManager.TryGetText(id, variation, out textObject);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0001B7E6 File Offset: 0x000199E6
		public static IEnumerable<TextObject> FindAllTextVariations(string id)
		{
			return GameTexts._gameTextManager.FindAllTextVariations(id);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0001B7F3 File Offset: 0x000199F3
		public static void SetVariable(string variableName, string content)
		{
			MBTextManager.SetTextVariable(variableName, content, false);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0001B7FD File Offset: 0x000199FD
		public static void SetVariable(string variableName, float content)
		{
			MBTextManager.SetTextVariable(variableName, content, 2);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0001B807 File Offset: 0x00019A07
		public static void SetVariable(string variableName, int content)
		{
			MBTextManager.SetTextVariable(variableName, content);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0001B810 File Offset: 0x00019A10
		public static void SetVariable(string variableName, TextObject content)
		{
			MBTextManager.SetTextVariable(variableName, content, false);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001B81A File Offset: 0x00019A1A
		public static void ClearInstance()
		{
			GameTexts._gameTextManager = null;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001B822 File Offset: 0x00019A22
		public static GameTexts.GameTextHelper AddGameTextWithVariation(string id)
		{
			return new GameTexts.GameTextHelper(id);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001B82A File Offset: 0x00019A2A
		private static void InitializeGlobalTags()
		{
			GameTexts.SetVariable("newline", "\n");
		}

		// Token: 0x04000420 RID: 1056
		private static GameTextManager _gameTextManager;

		// Token: 0x0200011C RID: 284
		public class GameTextHelper
		{
			// Token: 0x06000C0A RID: 3082 RVA: 0x00026807 File Offset: 0x00024A07
			public GameTextHelper(string id)
			{
				this._id = id;
			}

			// Token: 0x06000C0B RID: 3083 RVA: 0x00026816 File Offset: 0x00024A16
			public GameTexts.GameTextHelper Variation(string text, params object[] propertiesAndWeights)
			{
				GameTexts._gameTextManager.AddGameText(this._id).AddVariation(text, propertiesAndWeights);
				return this;
			}

			// Token: 0x06000C0C RID: 3084 RVA: 0x00026830 File Offset: 0x00024A30
			public static TextObject MergeTextObjectsWithComma(List<TextObject> textObjects, bool includeAnd)
			{
				return GameTexts.GameTextHelper.MergeTextObjectsWithSymbol(textObjects, new TextObject("{=kfdxjIad}, ", null), includeAnd ? new TextObject("{=eob9goyW} and ", null) : null);
			}

			// Token: 0x06000C0D RID: 3085 RVA: 0x00026854 File Offset: 0x00024A54
			public static TextObject MergeTextObjectsWithSymbol(List<TextObject> textObjects, TextObject symbol, TextObject lastSymbol = null)
			{
				int count = textObjects.Count;
				TextObject textObject;
				if (count == 0)
				{
					textObject = TextObject.GetEmpty();
				}
				else if (count == 1)
				{
					textObject = textObjects[0];
				}
				else
				{
					string text = "{=!}";
					for (int i = 0; i < textObjects.Count - 2; i++)
					{
						text = string.Concat(new object[] { text, "{VAR_", i, "}{SYMBOL}" });
					}
					text = string.Concat(new object[]
					{
						text,
						"{VAR_",
						textObjects.Count - 2,
						"}{LAST_SYMBOL}{VAR_",
						textObjects.Count - 1,
						"}"
					});
					textObject = new TextObject(text, null);
					for (int j = 0; j < textObjects.Count; j++)
					{
						textObject.SetTextVariable("VAR_" + j, textObjects[j]);
					}
					textObject.SetTextVariable("SYMBOL", symbol);
					textObject.SetTextVariable("LAST_SYMBOL", lastSymbol ?? symbol);
				}
				return textObject;
			}

			// Token: 0x040007A9 RID: 1961
			private string _id;
		}
	}
}
