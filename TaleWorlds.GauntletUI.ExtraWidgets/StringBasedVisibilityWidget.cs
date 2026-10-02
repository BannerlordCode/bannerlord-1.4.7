using System;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.GauntletUI.ExtraWidgets
{
	// Token: 0x02000014 RID: 20
	public class StringBasedVisibilityWidget : Widget
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00006B1D File Offset: 0x00004D1D
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00006B25 File Offset: 0x00004D25
		public StringBasedVisibilityWidget.WatchTypes WatchType { get; set; }

		// Token: 0x06000124 RID: 292 RVA: 0x00006B2E File Offset: 0x00004D2E
		public StringBasedVisibilityWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00006B37 File Offset: 0x00004D37
		// (set) Token: 0x06000126 RID: 294 RVA: 0x00006B40 File Offset: 0x00004D40
		[Editor(false)]
		public string FirstString
		{
			get
			{
				return this._firstString;
			}
			set
			{
				if (this._firstString != value)
				{
					this._firstString = value;
					base.OnPropertyChanged<string>(value, "FirstString");
					StringBasedVisibilityWidget.WatchTypes watchType = this.WatchType;
					if (watchType == StringBasedVisibilityWidget.WatchTypes.Equal)
					{
						base.IsVisible = string.Equals(value, this.SecondString, StringComparison.OrdinalIgnoreCase);
						return;
					}
					if (watchType != StringBasedVisibilityWidget.WatchTypes.NotEqual)
					{
						return;
					}
					base.IsVisible = !string.Equals(value, this.SecondString, StringComparison.OrdinalIgnoreCase);
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00006BA7 File Offset: 0x00004DA7
		// (set) Token: 0x06000128 RID: 296 RVA: 0x00006BB0 File Offset: 0x00004DB0
		[Editor(false)]
		public string SecondString
		{
			get
			{
				return this._secondString;
			}
			set
			{
				if (this._secondString != value)
				{
					this._secondString = value;
					base.OnPropertyChanged<string>(value, "SecondString");
					StringBasedVisibilityWidget.WatchTypes watchType = this.WatchType;
					if (watchType == StringBasedVisibilityWidget.WatchTypes.Equal)
					{
						base.IsVisible = string.Equals(value, this.FirstString, StringComparison.OrdinalIgnoreCase);
						return;
					}
					if (watchType != StringBasedVisibilityWidget.WatchTypes.NotEqual)
					{
						return;
					}
					base.IsVisible = !string.Equals(value, this.FirstString, StringComparison.OrdinalIgnoreCase);
				}
			}
		}

		// Token: 0x04000088 RID: 136
		private string _firstString;

		// Token: 0x04000089 RID: 137
		private string _secondString;

		// Token: 0x0200001E RID: 30
		public enum WatchTypes
		{
			// Token: 0x040000C6 RID: 198
			Equal,
			// Token: 0x040000C7 RID: 199
			NotEqual
		}
	}
}
