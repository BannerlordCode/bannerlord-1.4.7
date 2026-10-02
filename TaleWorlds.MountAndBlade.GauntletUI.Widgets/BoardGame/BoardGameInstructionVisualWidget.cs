using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.BoardGame
{
	// Token: 0x0200018D RID: 397
	public class BoardGameInstructionVisualWidget : Widget
	{
		// Token: 0x06001488 RID: 5256 RVA: 0x00037DE4 File Offset: 0x00035FE4
		public BoardGameInstructionVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x00037DF0 File Offset: 0x00035FF0
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.Sprite == null)
			{
				int siblingIndex = base.ParentWidget.ParentWidget.GetSiblingIndex();
				if (!string.IsNullOrEmpty(this.GameType))
				{
					base.Sprite = base.Context.SpriteData.GetSprite(this.GameType + siblingIndex);
				}
			}
			if (base.Sprite != null)
			{
				base.SuggestedWidth = (float)base.Sprite.Width * 0.5f;
				base.SuggestedHeight = (float)base.Sprite.Height * 0.5f;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x00037E89 File Offset: 0x00036089
		// (set) Token: 0x0600148B RID: 5259 RVA: 0x00037E91 File Offset: 0x00036091
		[Editor(false)]
		public string GameType
		{
			get
			{
				return this._gameType;
			}
			set
			{
				if (this._gameType != value)
				{
					this._gameType = value;
				}
			}
		}

		// Token: 0x04000950 RID: 2384
		private const float ScaleCoeff = 0.5f;

		// Token: 0x04000951 RID: 2385
		private string _gameType;
	}
}
