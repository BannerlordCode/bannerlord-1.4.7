using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x0200010A RID: 266
	public class DevelopmentNameTextWidget : TextWidget
	{
		// Token: 0x06000E32 RID: 3634 RVA: 0x00027248 File Offset: 0x00025448
		public DevelopmentNameTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00027263 File Offset: 0x00025463
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this.IsInQueue)
			{
				this.SetState(base.ParentWidget.CurrentState);
			}
			else
			{
				this.SetState("Selected");
			}
			this.HandleAnim(dt);
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x0002729C File Offset: 0x0002549C
		private void HandleAnim(float dt)
		{
			switch (this._currentState)
			{
			case DevelopmentNameTextWidget.AnimState.Start:
				this._currentAlphaTarget = 0f;
				this._currentState = DevelopmentNameTextWidget.AnimState.DownName;
				break;
			case DevelopmentNameTextWidget.AnimState.DownName:
				if ((double)base.ReadOnlyBrush.TextAlphaFactor < 0.01)
				{
					this._currentAlphaTarget = 1f;
					base.Text = this.MaxText;
					this._currentState = DevelopmentNameTextWidget.AnimState.UpMax;
				}
				break;
			case DevelopmentNameTextWidget.AnimState.UpMax:
				if ((double)base.ReadOnlyBrush.TextAlphaFactor > 0.99)
				{
					this._currentAlphaTarget = 0f;
					this._currentState = DevelopmentNameTextWidget.AnimState.StayMax;
					this._stayMaxTotalTime = 0f;
				}
				break;
			case DevelopmentNameTextWidget.AnimState.StayMax:
				this._stayMaxTotalTime += dt;
				if (this._stayMaxTotalTime >= this.MaxTextStayTime)
				{
					this._currentAlphaTarget = 0f;
					this._currentState = DevelopmentNameTextWidget.AnimState.DownMax;
				}
				break;
			case DevelopmentNameTextWidget.AnimState.DownMax:
				if ((double)base.ReadOnlyBrush.TextAlphaFactor < 0.01)
				{
					this._currentAlphaTarget = 1f;
					this._currentState = DevelopmentNameTextWidget.AnimState.UpName;
					base.Text = this.NameText;
				}
				break;
			case DevelopmentNameTextWidget.AnimState.UpName:
				if ((double)base.ReadOnlyBrush.TextAlphaFactor > 0.99)
				{
					this._currentState = DevelopmentNameTextWidget.AnimState.Idle;
					base.Text = this.NameText;
				}
				break;
			}
			if (this._currentState != DevelopmentNameTextWidget.AnimState.Idle && this._currentState != DevelopmentNameTextWidget.AnimState.StayMax)
			{
				base.Brush.TextAlphaFactor = Mathf.Lerp(base.ReadOnlyBrush.TextAlphaFactor, this._currentAlphaTarget, dt * 15f);
			}
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00027434 File Offset: 0x00025634
		public void StartMaxTextAnimation()
		{
			DevelopmentNameTextWidget.AnimState currentState = this._currentState;
			if (currentState > DevelopmentNameTextWidget.AnimState.StayMax)
			{
				int num = currentState - DevelopmentNameTextWidget.AnimState.DownMax;
				this._currentState = DevelopmentNameTextWidget.AnimState.Start;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x00027459 File Offset: 0x00025659
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x00027461 File Offset: 0x00025661
		[Editor(false)]
		public string MaxText
		{
			get
			{
				return this._maxText;
			}
			set
			{
				if (this._maxText != value)
				{
					this._maxText = value;
					base.OnPropertyChanged<string>(value, "MaxText");
				}
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x00027484 File Offset: 0x00025684
		// (set) Token: 0x06000E39 RID: 3641 RVA: 0x0002748C File Offset: 0x0002568C
		[Editor(false)]
		public float MaxTextStayTime
		{
			get
			{
				return this._maxTextStayTime;
			}
			set
			{
				if (this._maxTextStayTime != value)
				{
					this._maxTextStayTime = value;
					base.OnPropertyChanged(value, "MaxTextStayTime");
				}
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x000274AA File Offset: 0x000256AA
		// (set) Token: 0x06000E3B RID: 3643 RVA: 0x000274B2 File Offset: 0x000256B2
		[Editor(false)]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (this._nameText != value)
				{
					this._nameText = value;
					base.OnPropertyChanged<string>(value, "NameText");
					base.Text = this.NameText;
				}
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x000274E1 File Offset: 0x000256E1
		// (set) Token: 0x06000E3D RID: 3645 RVA: 0x000274E9 File Offset: 0x000256E9
		[Editor(false)]
		public bool IsInQueue
		{
			get
			{
				return this._isInQueue;
			}
			set
			{
				if (this._isInQueue != value)
				{
					this._isInQueue = value;
					base.OnPropertyChanged(value, "IsInQueue");
				}
			}
		}

		// Token: 0x0400066F RID: 1647
		private float _currentAlphaTarget;

		// Token: 0x04000670 RID: 1648
		private float _stayMaxTotalTime;

		// Token: 0x04000671 RID: 1649
		private DevelopmentNameTextWidget.AnimState _currentState = DevelopmentNameTextWidget.AnimState.Idle;

		// Token: 0x04000672 RID: 1650
		private float _maxTextStayTime = 1f;

		// Token: 0x04000673 RID: 1651
		private bool _isInQueue;

		// Token: 0x04000674 RID: 1652
		private string _maxText;

		// Token: 0x04000675 RID: 1653
		private string _nameText;

		// Token: 0x020001C6 RID: 454
		public enum AnimState
		{
			// Token: 0x04000A1A RID: 2586
			Start,
			// Token: 0x04000A1B RID: 2587
			DownName,
			// Token: 0x04000A1C RID: 2588
			UpMax,
			// Token: 0x04000A1D RID: 2589
			StayMax,
			// Token: 0x04000A1E RID: 2590
			DownMax,
			// Token: 0x04000A1F RID: 2591
			UpName,
			// Token: 0x04000A20 RID: 2592
			Idle
		}
	}
}
