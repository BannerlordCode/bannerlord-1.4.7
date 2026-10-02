using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000076 RID: 118
	public class GameText
	{
		// Token: 0x170002CD RID: 717
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x0001B048 File Offset: 0x00019248
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x0001B050 File Offset: 0x00019250
		public string Id { get; private set; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x0001B059 File Offset: 0x00019259
		public IEnumerable<GameText.GameTextVariation> Variations
		{
			get
			{
				return this._variationList;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x0001B061 File Offset: 0x00019261
		public TextObject DefaultText
		{
			get
			{
				if (this._variationList != null && this._variationList.Count > 0)
				{
					return this._variationList[0].Text;
				}
				return null;
			}
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0001B08C File Offset: 0x0001928C
		internal GameText()
		{
			this._variationList = new List<GameText.GameTextVariation>();
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0001B09F File Offset: 0x0001929F
		internal GameText(string id)
		{
			this.Id = id;
			this._variationList = new List<GameText.GameTextVariation>();
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001B0BC File Offset: 0x000192BC
		internal TextObject GetVariation(string variationId)
		{
			foreach (GameText.GameTextVariation gameTextVariation in this._variationList)
			{
				if (gameTextVariation.Id.Equals(variationId))
				{
					return gameTextVariation.Text;
				}
			}
			return null;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001B124 File Offset: 0x00019324
		public void AddVariationWithId(string variationId, TextObject text, List<GameTextManager.ChoiceTag> choiceTags)
		{
			foreach (GameText.GameTextVariation gameTextVariation in this._variationList)
			{
				if (gameTextVariation.Id.Equals(variationId) && gameTextVariation.Text.ToString().Equals(text.ToString()))
				{
					return;
				}
			}
			this._variationList.Add(new GameText.GameTextVariation(variationId, text, choiceTags));
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001B1AC File Offset: 0x000193AC
		public void SetVariationWithId(string variationId, TextObject text, List<GameTextManager.ChoiceTag> choiceTags)
		{
			for (int i = 0; i < this._variationList.Count; i++)
			{
				if (this._variationList[i].Id.Equals(variationId))
				{
					this._variationList[i] = new GameText.GameTextVariation(variationId, text, choiceTags);
					return;
				}
			}
			this._variationList.Add(new GameText.GameTextVariation(variationId, text, choiceTags));
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001B210 File Offset: 0x00019410
		public void AddVariation(string text, params object[] propertiesAndWeights)
		{
			List<GameTextManager.ChoiceTag> list = new List<GameTextManager.ChoiceTag>();
			for (int i = 0; i < propertiesAndWeights.Length; i += 2)
			{
				string text2 = (string)propertiesAndWeights[i];
				int num = Convert.ToInt32(propertiesAndWeights[i + 1]);
				list.Add(new GameTextManager.ChoiceTag(text2, num));
			}
			this.AddVariationWithId("", new TextObject(text, null), list);
		}

		// Token: 0x0400041E RID: 1054
		private readonly List<GameText.GameTextVariation> _variationList;

		// Token: 0x02000119 RID: 281
		public struct GameTextVariation
		{
			// Token: 0x06000BF9 RID: 3065 RVA: 0x000265EB File Offset: 0x000247EB
			internal GameTextVariation(string id, TextObject text, List<GameTextManager.ChoiceTag> choiceTags)
			{
				this.Id = id;
				this.Text = text;
				this.Tags = choiceTags.ToArray();
			}

			// Token: 0x0400079C RID: 1948
			public readonly string Id;

			// Token: 0x0400079D RID: 1949
			public readonly TextObject Text;

			// Token: 0x0400079E RID: 1950
			public readonly GameTextManager.ChoiceTag[] Tags;
		}
	}
}
