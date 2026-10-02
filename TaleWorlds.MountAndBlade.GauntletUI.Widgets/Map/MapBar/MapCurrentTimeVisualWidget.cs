using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x0200012C RID: 300
	public class MapCurrentTimeVisualWidget : Widget
	{
		// Token: 0x06000FAD RID: 4013 RVA: 0x0002B420 File Offset: 0x00029620
		public MapCurrentTimeVisualWidget(UIContext context)
			: base(context)
		{
			base.AddState("Disabled");
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x0002B434 File Offset: 0x00029634
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.IsDisabled)
			{
				this.SetState("Disabled");
				return;
			}
			this.SetState("Default");
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			switch (this.CurrentTimeState)
			{
			case 0:
			case 6:
				flag3 = true;
				break;
			case 1:
			case 3:
				flag = true;
				break;
			case 2:
			case 4:
			case 5:
				flag2 = true;
				break;
			}
			this.PlayButton.IsSelected = flag;
			this.FastForwardButton.IsSelected = flag2;
			this.PauseButton.IsSelected = flag3;
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x0002B4C6 File Offset: 0x000296C6
		// (set) Token: 0x06000FB0 RID: 4016 RVA: 0x0002B4CE File Offset: 0x000296CE
		[Editor(false)]
		public int CurrentTimeState
		{
			get
			{
				return this._currenTimeState;
			}
			set
			{
				if (this._currenTimeState != value)
				{
					this._currenTimeState = value;
					base.OnPropertyChanged(value, "CurrentTimeState");
				}
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0002B4EC File Offset: 0x000296EC
		// (set) Token: 0x06000FB2 RID: 4018 RVA: 0x0002B4F4 File Offset: 0x000296F4
		[Editor(false)]
		public ButtonWidget FastForwardButton
		{
			get
			{
				return this._fastForwardButton;
			}
			set
			{
				if (this._fastForwardButton != value)
				{
					this._fastForwardButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "FastForwardButton");
				}
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x0002B512 File Offset: 0x00029712
		// (set) Token: 0x06000FB4 RID: 4020 RVA: 0x0002B51A File Offset: 0x0002971A
		[Editor(false)]
		public ButtonWidget PlayButton
		{
			get
			{
				return this._playButton;
			}
			set
			{
				if (this._playButton != value)
				{
					this._playButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "PlayButton");
				}
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0002B538 File Offset: 0x00029738
		// (set) Token: 0x06000FB6 RID: 4022 RVA: 0x0002B540 File Offset: 0x00029740
		[Editor(false)]
		public ButtonWidget PauseButton
		{
			get
			{
				return this._pauseButton;
			}
			set
			{
				if (this._pauseButton != value)
				{
					this._pauseButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "PauseButton");
				}
			}
		}

		// Token: 0x0400071F RID: 1823
		private int _currenTimeState;

		// Token: 0x04000720 RID: 1824
		private ButtonWidget _fastForwardButton;

		// Token: 0x04000721 RID: 1825
		private ButtonWidget _playButton;

		// Token: 0x04000722 RID: 1826
		private ButtonWidget _pauseButton;
	}
}
