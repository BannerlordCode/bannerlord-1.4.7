using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard
{
	// Token: 0x02000055 RID: 85
	public class ScoreboardBattleRewardsWidget : Widget
	{
		// Token: 0x060004A1 RID: 1185 RVA: 0x0000E8F9 File Offset: 0x0000CAF9
		public ScoreboardBattleRewardsWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000E918 File Offset: 0x0000CB18
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._isAnimationActive)
			{
				this.UpdateAnimation(dt);
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0000E930 File Offset: 0x0000CB30
		public void StartAnimation()
		{
			this._isAnimationActive = true;
			this._animationTimePassed = 0f;
			this._animationLastItemIndex = -1;
			this.ItemContainer.SetState("Opened");
			for (int i = 0; i < this.ItemContainer.ChildCount; i++)
			{
				Widget child = this.ItemContainer.GetChild(i);
				child.IsVisible = false;
				child.AddState("Opened");
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000E99C File Offset: 0x0000CB9C
		public void Reset()
		{
			for (int i = 0; i < this.ItemContainer.ChildCount; i++)
			{
				this.ItemContainer.GetChild(i).IsVisible = false;
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0000E9D4 File Offset: 0x0000CBD4
		private void UpdateAnimation(float dt)
		{
			if (this._animationTimePassed >= this.AnimationDelay + this.AnimationInterval * (float)this.ItemContainer.ChildCount)
			{
				return;
			}
			if (this._animationTimePassed >= this.AnimationDelay)
			{
				int num = MathF.Floor((this._animationTimePassed - this.AnimationDelay) / this.AnimationInterval);
				if (num != this._animationLastItemIndex && num < this.ItemContainer.ChildCount)
				{
					for (int i = this._animationLastItemIndex + 1; i <= num; i++)
					{
						Widget child = this.ItemContainer.GetChild(i);
						child.IsVisible = true;
						child.SetState("Opened");
					}
				}
			}
			this._animationTimePassed += dt;
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0000EA81 File Offset: 0x0000CC81
		// (set) Token: 0x060004A7 RID: 1191 RVA: 0x0000EA89 File Offset: 0x0000CC89
		[Editor(false)]
		public float AnimationDelay
		{
			get
			{
				return this._animationDelay;
			}
			set
			{
				if (this._animationDelay != value)
				{
					this._animationDelay = value;
					base.OnPropertyChanged(value, "AnimationDelay");
				}
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0000EAA7 File Offset: 0x0000CCA7
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x0000EAAF File Offset: 0x0000CCAF
		[Editor(false)]
		public float AnimationInterval
		{
			get
			{
				return this._animationInterval;
			}
			set
			{
				if (this._animationInterval != value)
				{
					this._animationInterval = value;
					base.OnPropertyChanged(value, "AnimationInterval");
				}
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x0000EACD File Offset: 0x0000CCCD
		// (set) Token: 0x060004AB RID: 1195 RVA: 0x0000EAD5 File Offset: 0x0000CCD5
		[Editor(false)]
		public Widget ItemContainer
		{
			get
			{
				return this._itemContainer;
			}
			set
			{
				if (this._itemContainer != value)
				{
					this._itemContainer = value;
					base.OnPropertyChanged<Widget>(value, "ItemContainer");
				}
			}
		}

		// Token: 0x040001FB RID: 507
		private bool _isAnimationActive;

		// Token: 0x040001FC RID: 508
		private float _animationTimePassed;

		// Token: 0x040001FD RID: 509
		private int _animationLastItemIndex;

		// Token: 0x040001FE RID: 510
		private float _animationDelay = 1f;

		// Token: 0x040001FF RID: 511
		private float _animationInterval = 0.25f;

		// Token: 0x04000200 RID: 512
		private Widget _itemContainer;
	}
}
