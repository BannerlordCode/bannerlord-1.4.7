using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000028 RID: 40
	public class InitialMenuAnimControllerWidget : Widget
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000799E File Offset: 0x00005B9E
		// (set) Token: 0x06000213 RID: 531 RVA: 0x000079A6 File Offset: 0x00005BA6
		public bool IsAnimEnabled { get; set; }

		// Token: 0x06000214 RID: 532 RVA: 0x000079AF File Offset: 0x00005BAF
		public InitialMenuAnimControllerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000079B8 File Offset: 0x00005BB8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.IsAnimEnabled)
			{
				if (!this._isInitialized)
				{
					Widget optionsList = this.OptionsList;
					bool flag;
					if (optionsList == null)
					{
						flag = false;
					}
					else
					{
						List<Widget> children = optionsList.Children;
						int? num = ((children != null) ? new int?(children.Count) : null);
						int num2 = 0;
						flag = (num.GetValueOrDefault() > num2) & (num != null);
					}
					if (flag)
					{
						this.OptionsList.Children.ForEach(delegate(Widget x)
						{
							x.SetGlobalAlphaRecursively(0f);
						});
						this._totalOptionCount = this.OptionsList.Children.Count;
						this._isInitialized = true;
					}
				}
				if (this._isInitialized && !this._isFinalized && this.OptionsList != null)
				{
					this._timer += dt;
					if (this._timer >= this.InitialWaitTime + (float)this._currentOptionIndex * this.WaitTimeBetweenOptions)
					{
						Widget child = this.OptionsList.GetChild(this._currentOptionIndex);
						if (child != null)
						{
							child.SetState("Activated");
						}
						this._currentOptionIndex++;
					}
					for (int i = 0; i < this._currentOptionIndex; i++)
					{
						float num3 = this.InitialWaitTime + this.WaitTimeBetweenOptions * (float)i;
						float num4 = num3 + this.OptionFadeInTime;
						Widget child2 = this.OptionsList.GetChild(i);
						if (this._timer < num4)
						{
							float num5 = MathF.Clamp((this._timer - num3) / (num4 - num3), 0f, 1f);
							if (child2 != null)
							{
								child2.SetGlobalAlphaRecursively(num5);
							}
						}
						else if (child2 != null)
						{
							child2.SetGlobalAlphaRecursively(1f);
						}
					}
					this._isFinalized = this._timer > this.InitialWaitTime + this.WaitTimeBetweenOptions * (float)(this._totalOptionCount - 1) + this.OptionFadeInTime;
				}
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00007B9F File Offset: 0x00005D9F
		// (set) Token: 0x06000217 RID: 535 RVA: 0x00007BA7 File Offset: 0x00005DA7
		[Editor(false)]
		public Widget OptionsList
		{
			get
			{
				return this._optionsList;
			}
			set
			{
				if (this._optionsList != value)
				{
					this._optionsList = value;
					base.OnPropertyChanged<Widget>(value, "OptionsList");
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00007BC5 File Offset: 0x00005DC5
		// (set) Token: 0x06000219 RID: 537 RVA: 0x00007BCD File Offset: 0x00005DCD
		[Editor(false)]
		public float InitialWaitTime
		{
			get
			{
				return this._initialWaitTime;
			}
			set
			{
				if (this._initialWaitTime != value)
				{
					this._initialWaitTime = value;
					base.OnPropertyChanged(value, "InitialWaitTime");
				}
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00007BEB File Offset: 0x00005DEB
		// (set) Token: 0x0600021B RID: 539 RVA: 0x00007BF3 File Offset: 0x00005DF3
		[Editor(false)]
		public float WaitTimeBetweenOptions
		{
			get
			{
				return this._waitTimeBetweenOptions;
			}
			set
			{
				if (this._waitTimeBetweenOptions != value)
				{
					this._waitTimeBetweenOptions = value;
					base.OnPropertyChanged(value, "WaitTimeBetweenOptions");
				}
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00007C11 File Offset: 0x00005E11
		// (set) Token: 0x0600021D RID: 541 RVA: 0x00007C19 File Offset: 0x00005E19
		[Editor(false)]
		public float OptionFadeInTime
		{
			get
			{
				return this._optionFadeInTime;
			}
			set
			{
				if (this._optionFadeInTime != value)
				{
					this._optionFadeInTime = value;
					base.OnPropertyChanged(value, "OptionFadeInTime");
				}
			}
		}

		// Token: 0x040000F9 RID: 249
		private bool _isInitialized;

		// Token: 0x040000FA RID: 250
		private bool _isFinalized;

		// Token: 0x040000FB RID: 251
		private int _currentOptionIndex;

		// Token: 0x040000FC RID: 252
		private int _totalOptionCount;

		// Token: 0x040000FD RID: 253
		private float _timer;

		// Token: 0x040000FE RID: 254
		private Widget _optionsList;

		// Token: 0x040000FF RID: 255
		private float _initialWaitTime;

		// Token: 0x04000100 RID: 256
		private float _waitTimeBetweenOptions;

		// Token: 0x04000101 RID: 257
		private float _optionFadeInTime;
	}
}
