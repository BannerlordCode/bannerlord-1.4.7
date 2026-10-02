using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000092 RID: 146
	public class MultiplayerScoreboardEndOfBattlePanelWidget : Widget
	{
		// Token: 0x060007F2 RID: 2034 RVA: 0x00017164 File Offset: 0x00015364
		public MultiplayerScoreboardEndOfBattlePanelWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00017178 File Offset: 0x00015378
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._isFinished || !this._isStarted)
			{
				return;
			}
			this._timePassed += dt;
			if (this._timePassed >= this.SecondDelay)
			{
				this._isFinished = true;
				this.SetState("Opened");
				base.Context.TwoDimensionContext.PlaySound(this._openedSoundEvent);
				return;
			}
			if (this._timePassed >= this.FirstDelay && !this._isPreStateFinished)
			{
				this._isPreStateFinished = true;
				this.SetState("PreOpened");
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001720A File Offset: 0x0001540A
		public void StartAnimation()
		{
			this._isStarted = true;
			this._isFinished = false;
			this._isPreStateFinished = false;
			this._timePassed = 0f;
			base.AddState("PreOpened");
			base.AddState("Opened");
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00017242 File Offset: 0x00015442
		private void Reset()
		{
			this._isStarted = false;
			this._isPreStateFinished = false;
			this._isFinished = false;
			this.SetState("Default");
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00017264 File Offset: 0x00015464
		private void AvailableUpdated()
		{
			if (this.IsAvailable)
			{
				this.StartAnimation();
				return;
			}
			this.Reset();
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x0001727B File Offset: 0x0001547B
		// (set) Token: 0x060007F8 RID: 2040 RVA: 0x00017283 File Offset: 0x00015483
		[Editor(false)]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (value != this._isAvailable)
				{
					this._isAvailable = value;
					base.OnPropertyChanged(value, "IsAvailable");
					this.AvailableUpdated();
				}
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x000172A7 File Offset: 0x000154A7
		// (set) Token: 0x060007FA RID: 2042 RVA: 0x000172AF File Offset: 0x000154AF
		[Editor(false)]
		public float FirstDelay
		{
			get
			{
				return this._firstDelay;
			}
			set
			{
				if (value != this._firstDelay)
				{
					this._firstDelay = value;
					base.OnPropertyChanged(value, "FirstDelay");
				}
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x000172CD File Offset: 0x000154CD
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x000172D5 File Offset: 0x000154D5
		[Editor(false)]
		public float SecondDelay
		{
			get
			{
				return this._secondDelay;
			}
			set
			{
				if (value != this._secondDelay)
				{
					this._secondDelay = value;
					base.OnPropertyChanged(value, "SecondDelay");
				}
			}
		}

		// Token: 0x04000388 RID: 904
		private bool _isStarted;

		// Token: 0x04000389 RID: 905
		private bool _isPreStateFinished;

		// Token: 0x0400038A RID: 906
		private bool _isFinished;

		// Token: 0x0400038B RID: 907
		private float _timePassed;

		// Token: 0x0400038C RID: 908
		private string _openedSoundEvent = "panels/scoreboard_flags";

		// Token: 0x0400038D RID: 909
		private bool _isAvailable;

		// Token: 0x0400038E RID: 910
		private float _firstDelay;

		// Token: 0x0400038F RID: 911
		private float _secondDelay;
	}
}
