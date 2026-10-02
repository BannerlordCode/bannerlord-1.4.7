using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000CE RID: 206
	public class MultiplayerClassLoadoutTroopInfoBrushWidget : BrushWidget
	{
		// Token: 0x06000AB2 RID: 2738 RVA: 0x0001DFB4 File Offset: 0x0001C1B4
		public MultiplayerClassLoadoutTroopInfoBrushWidget(UIContext context)
			: base(context)
		{
			this.SetAlpha(this.DefaultAlpha);
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0001DFD4 File Offset: 0x0001C1D4
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			this.SetAlpha(1f);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0001DFE7 File Offset: 0x0001C1E7
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			this.SetAlpha(this.DefaultAlpha);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0001DFFB File Offset: 0x0001C1FB
		public override void OnBrushChanged()
		{
			base.OnBrushChanged();
			this.SetAlpha(this.DefaultAlpha);
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0001E00F File Offset: 0x0001C20F
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x0001E017 File Offset: 0x0001C217
		[Editor(false)]
		public float DefaultAlpha
		{
			get
			{
				return this._defaultAlpha;
			}
			set
			{
				if (value != this._defaultAlpha)
				{
					this._defaultAlpha = value;
					base.OnPropertyChanged(value, "DefaultAlpha");
				}
			}
		}

		// Token: 0x040004E0 RID: 1248
		private float _defaultAlpha = 0.7f;
	}
}
