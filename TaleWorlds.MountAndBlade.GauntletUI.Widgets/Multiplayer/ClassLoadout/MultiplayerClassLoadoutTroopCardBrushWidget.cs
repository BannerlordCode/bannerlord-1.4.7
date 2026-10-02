using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000CD RID: 205
	public class MultiplayerClassLoadoutTroopCardBrushWidget : BrushWidget
	{
		// Token: 0x06000AA8 RID: 2728 RVA: 0x0001DE8A File Offset: 0x0001C08A
		public MultiplayerClassLoadoutTroopCardBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x0001DE94 File Offset: 0x0001C094
		private void OnCultureIDUpdated()
		{
			if (this.CultureID != null)
			{
				this.SetState(this.CultureID);
				BrushWidget border = this.Border;
				if (border != null)
				{
					border.SetState(this.CultureID);
				}
				BrushWidget classBorder = this.ClassBorder;
				if (classBorder != null)
				{
					classBorder.SetState(this.CultureID);
				}
				BrushWidget classFrame = this.ClassFrame;
				if (classFrame == null)
				{
					return;
				}
				classFrame.SetState(this.CultureID);
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06000AAA RID: 2730 RVA: 0x0001DEF9 File Offset: 0x0001C0F9
		// (set) Token: 0x06000AAB RID: 2731 RVA: 0x0001DF01 File Offset: 0x0001C101
		[Editor(false)]
		public string CultureID
		{
			get
			{
				return this._cultureID;
			}
			set
			{
				if (value != this._cultureID)
				{
					this._cultureID = value;
					base.OnPropertyChanged<string>(value, "CultureID");
					this.OnCultureIDUpdated();
				}
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x0001DF2A File Offset: 0x0001C12A
		// (set) Token: 0x06000AAD RID: 2733 RVA: 0x0001DF32 File Offset: 0x0001C132
		[Editor(false)]
		public BrushWidget Border
		{
			get
			{
				return this._border;
			}
			set
			{
				if (value != this._border)
				{
					this._border = value;
					base.OnPropertyChanged<BrushWidget>(value, "Border");
					this.OnCultureIDUpdated();
				}
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x0001DF56 File Offset: 0x0001C156
		// (set) Token: 0x06000AAF RID: 2735 RVA: 0x0001DF5E File Offset: 0x0001C15E
		[Editor(false)]
		public BrushWidget ClassBorder
		{
			get
			{
				return this._classBorder;
			}
			set
			{
				if (value != this._classBorder)
				{
					this._classBorder = value;
					base.OnPropertyChanged<BrushWidget>(value, "ClassBorder");
					this.OnCultureIDUpdated();
				}
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0001DF82 File Offset: 0x0001C182
		// (set) Token: 0x06000AB1 RID: 2737 RVA: 0x0001DF8A File Offset: 0x0001C18A
		[Editor(false)]
		public BrushWidget ClassFrame
		{
			get
			{
				return this._classFrame;
			}
			set
			{
				if (value != this._classFrame)
				{
					this._classFrame = value;
					base.OnPropertyChanged<BrushWidget>(value, "ClassFrame");
					base.OnPropertyChanged<BrushWidget>(value, "ClassFrame");
				}
			}
		}

		// Token: 0x040004DC RID: 1244
		private string _cultureID;

		// Token: 0x040004DD RID: 1245
		private BrushWidget _border;

		// Token: 0x040004DE RID: 1246
		private BrushWidget _classBorder;

		// Token: 0x040004DF RID: 1247
		private BrushWidget _classFrame;
	}
}
