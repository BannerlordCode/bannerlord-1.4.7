using System;
using System.Numerics;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Quest
{
	// Token: 0x02000060 RID: 96
	public class QuestStageItemWidget : Widget
	{
		// Token: 0x0600053A RID: 1338 RVA: 0x0000FE3E File Offset: 0x0000E03E
		public QuestStageItemWidget(UIContext context)
			: base(context)
		{
			this._firstFrame = true;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000FE50 File Offset: 0x0000E050
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			this._previousHoverBegan = this._hoverBegan;
			if (!this._firstFrame && this.IsNew)
			{
				bool flag = this.IsMouseOverWidget();
				if (flag && !this._hoverBegan)
				{
					this._hoverBegan = true;
				}
				else if (!flag && this._hoverBegan)
				{
					this._hoverBegan = false;
				}
			}
			this._firstFrame = false;
			if (this._previousHoverBegan && !this._hoverBegan)
			{
				base.EventFired("ResetGlow", Array.Empty<object>());
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000FED4 File Offset: 0x0000E0D4
		private bool IsMouseOverWidget()
		{
			Vector2 globalPosition = base.GlobalPosition;
			return this.IsBetween(base.EventManager.MousePosition.X, globalPosition.X, globalPosition.X + base.Size.X) && this.IsBetween(base.EventManager.MousePosition.Y, globalPosition.Y, globalPosition.Y + base.Size.Y);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000FF48 File Offset: 0x0000E148
		private bool IsBetween(float number, float min, float max)
		{
			return number >= min && number <= max;
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0000FF57 File Offset: 0x0000E157
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x0000FF5F File Offset: 0x0000E15F
		[Editor(false)]
		public bool IsNew
		{
			get
			{
				return this._isNew;
			}
			set
			{
				if (this._isNew != value)
				{
					this._isNew = value;
					base.OnPropertyChanged(value, "IsNew");
				}
			}
		}

		// Token: 0x04000240 RID: 576
		private bool _firstFrame;

		// Token: 0x04000241 RID: 577
		private bool _previousHoverBegan;

		// Token: 0x04000242 RID: 578
		private bool _hoverBegan;

		// Token: 0x04000243 RID: 579
		private bool _isNew;
	}
}
