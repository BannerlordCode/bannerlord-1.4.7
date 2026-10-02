using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200003C RID: 60
	public class ScoreboardAnimatedTextWidget : TextWidget
	{
		// Token: 0x06000388 RID: 904 RVA: 0x0000B594 File Offset: 0x00009794
		public ScoreboardAnimatedTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000B59D File Offset: 0x0000979D
		private void HandleValueChanged(int value)
		{
			base.Text = ((!this.ShowZero && value == 0) ? "" : value.ToString());
			base.BrushRenderer.RestartAnimation();
			base.RegisterUpdateBrushes();
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000B5CF File Offset: 0x000097CF
		// (set) Token: 0x0600038B RID: 907 RVA: 0x0000B5D7 File Offset: 0x000097D7
		[Editor(false)]
		public int ValueAsInt
		{
			get
			{
				return this._valueAsInt;
			}
			set
			{
				if (value != this._valueAsInt)
				{
					this._valueAsInt = value;
					base.OnPropertyChanged(value, "ValueAsInt");
					this.HandleValueChanged(value);
				}
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0000B5FC File Offset: 0x000097FC
		// (set) Token: 0x0600038D RID: 909 RVA: 0x0000B604 File Offset: 0x00009804
		[Editor(false)]
		public bool ShowZero
		{
			get
			{
				return this._showZero;
			}
			set
			{
				if (this._showZero != value)
				{
					this._showZero = value;
					base.OnPropertyChanged(value, "ShowZero");
					this.HandleValueChanged(this._valueAsInt);
				}
			}
		}

		// Token: 0x04000176 RID: 374
		private bool _showZero;

		// Token: 0x04000177 RID: 375
		private int _valueAsInt;
	}
}
