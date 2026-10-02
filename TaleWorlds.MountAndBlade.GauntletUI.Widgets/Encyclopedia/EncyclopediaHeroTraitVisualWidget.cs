using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Encyclopedia
{
	// Token: 0x02000159 RID: 345
	public class EncyclopediaHeroTraitVisualWidget : Widget
	{
		// Token: 0x0600124F RID: 4687 RVA: 0x0003276B File Offset: 0x0003096B
		public EncyclopediaHeroTraitVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00032774 File Offset: 0x00030974
		private void SetVisual(string traitCode, int value)
		{
			if (!string.IsNullOrEmpty(traitCode))
			{
				string text = string.Concat(new object[]
				{
					"SPGeneral\\SPTraits\\",
					traitCode.ToLower(),
					"_",
					value
				});
				base.Sprite = base.Context.SpriteData.GetSprite(text);
				base.Sprite = base.Context.SpriteData.GetSprite(text);
				if (value < 0)
				{
					base.Color = new Color(0.738f, 0.113f, 0.113f, 1f);
					return;
				}
				base.Color = new Color(0.992f, 0.75f, 0.33f, 1f);
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06001251 RID: 4689 RVA: 0x0003282B File Offset: 0x00030A2B
		// (set) Token: 0x06001252 RID: 4690 RVA: 0x00032833 File Offset: 0x00030A33
		[Editor(false)]
		public string TraitId
		{
			get
			{
				return this._traitId;
			}
			set
			{
				if (this._traitId != value)
				{
					this._traitId = value;
					base.OnPropertyChanged<string>(value, "TraitId");
					this.SetVisual(value, this.TraitValue);
				}
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06001253 RID: 4691 RVA: 0x00032863 File Offset: 0x00030A63
		// (set) Token: 0x06001254 RID: 4692 RVA: 0x0003286B File Offset: 0x00030A6B
		[Editor(false)]
		public int TraitValue
		{
			get
			{
				return this._traitValue;
			}
			set
			{
				if (this._traitValue != value)
				{
					this._traitValue = value;
					base.OnPropertyChanged(value, "TraitValue");
					this.SetVisual(this.TraitId, value);
				}
			}
		}

		// Token: 0x04000852 RID: 2130
		private string _traitId;

		// Token: 0x04000853 RID: 2131
		private int _traitValue;
	}
}
