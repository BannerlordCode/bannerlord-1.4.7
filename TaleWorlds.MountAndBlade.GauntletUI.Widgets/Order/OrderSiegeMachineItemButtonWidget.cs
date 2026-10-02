using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Order
{
	// Token: 0x02000074 RID: 116
	public class OrderSiegeMachineItemButtonWidget : ButtonWidget
	{
		// Token: 0x06000633 RID: 1587 RVA: 0x0001235F File Offset: 0x0001055F
		public OrderSiegeMachineItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00012376 File Offset: 0x00010576
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isVisualsDirty)
			{
				this.MachineIconWidgetChanged();
				this.UpdateRemainingCount();
				this.UpdateMachineIcon();
				this._isVisualsDirty = false;
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000123A0 File Offset: 0x000105A0
		private void MachineIconWidgetChanged()
		{
			if (this.MachineIconWidget == null)
			{
				return;
			}
			this.MachineIconWidget.RegisterBrushStatesOfWidget();
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000123B8 File Offset: 0x000105B8
		private void UpdateMachineIcon()
		{
			if (this.MachineIconWidget == null)
			{
				return;
			}
			this._isRemainingCountVisible = true;
			string machineClass = this.MachineClass;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(machineClass);
			if (num <= 1056090379U)
			{
				if (num <= 390431385U)
				{
					if (num <= 6339497U)
					{
						if (num != 0U)
						{
							if (num == 6339497U)
							{
								if (machineClass == "ladder")
								{
									this.MachineIconWidget.SetState("Ladder");
									return;
								}
							}
						}
						else if (machineClass != null)
						{
						}
					}
					else if (num != 354578048U)
					{
						if (num == 390431385U)
						{
							if (machineClass == "bricole")
							{
								this.MachineIconWidget.SetState("Bricole");
								return;
							}
						}
					}
					else if (machineClass == "Mangonel")
					{
						this.MachineIconWidget.SetState("Mangonel");
						return;
					}
				}
				else if (num <= 729368230U)
				{
					if (num != 616782878U)
					{
						if (num == 729368230U)
						{
							if (machineClass == "siege_tower_level1")
							{
								this.MachineIconWidget.SetState("SiegeTower");
								return;
							}
						}
					}
					else if (machineClass == "improved_ram")
					{
						this.MachineIconWidget.SetState("ImprovedRam");
						return;
					}
				}
				else if (num != 808481256U)
				{
					if (num == 1056090379U)
					{
						if (machineClass == "preparations")
						{
							this.MachineIconWidget.SetState("Preparations");
							return;
						}
					}
				}
				else if (machineClass == "fire_ballista")
				{
					this.MachineIconWidget.SetState("FireBallista");
					return;
				}
			}
			else if (num <= 1839032341U)
			{
				if (num <= 1748194790U)
				{
					if (num != 1241455715U)
					{
						if (num == 1748194790U)
						{
							if (machineClass == "fire_catapult")
							{
								this.MachineIconWidget.SetState("FireCatapult");
								return;
							}
						}
					}
					else if (machineClass == "ram")
					{
						this.MachineIconWidget.SetState("Ram");
						return;
					}
				}
				else if (num != 1820818168U)
				{
					if (num == 1839032341U)
					{
						if (machineClass == "trebuchet")
						{
							this.MachineIconWidget.SetState("Trebuchet");
							return;
						}
					}
				}
				else if (machineClass == "fire_onager")
				{
					this.MachineIconWidget.SetState("FireOnager");
					return;
				}
			}
			else if (num <= 1898442385U)
			{
				if (num != 1844264380U)
				{
					if (num == 1898442385U)
					{
						if (machineClass == "catapult")
						{
							this.MachineIconWidget.SetState("Catapult");
							return;
						}
					}
				}
				else if (machineClass == "FireMangonel")
				{
					this.MachineIconWidget.SetState("FireMangonel");
					return;
				}
			}
			else if (num != 2166136261U)
			{
				if (num != 2806198843U)
				{
					if (num == 4036530155U)
					{
						if (machineClass == "ballista")
						{
							this.MachineIconWidget.SetState("Ballista");
							return;
						}
					}
				}
				else if (machineClass == "onager")
				{
					this.MachineIconWidget.SetState("Onager");
					return;
				}
			}
			else if (machineClass != null && machineClass.Length != 0)
			{
			}
			this.MachineIconWidget.SetState("None");
			this._isRemainingCountVisible = false;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00012768 File Offset: 0x00010968
		private void UpdateRemainingCount()
		{
			if (this.RemainingCountWidget == null)
			{
				return;
			}
			base.IsDisabled = this.RemainingCount == 0;
			this.RemainingCountWidget.IsVisible = this._isRemainingCountVisible;
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x00012793 File Offset: 0x00010993
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x0001279B File Offset: 0x0001099B
		[Editor(false)]
		public int RemainingCount
		{
			get
			{
				return this._remainingCount;
			}
			set
			{
				if (this._remainingCount != value)
				{
					this._remainingCount = value;
					base.OnPropertyChanged(value, "RemainingCount");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x000127C0 File Offset: 0x000109C0
		// (set) Token: 0x0600063B RID: 1595 RVA: 0x000127C8 File Offset: 0x000109C8
		[Editor(false)]
		public TextWidget RemainingCountWidget
		{
			get
			{
				return this._remainingCountWidget;
			}
			set
			{
				if (this._remainingCountWidget != value)
				{
					this._remainingCountWidget = value;
					base.OnPropertyChanged<TextWidget>(value, "RemainingCountWidget");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x000127ED File Offset: 0x000109ED
		// (set) Token: 0x0600063D RID: 1597 RVA: 0x000127F5 File Offset: 0x000109F5
		[Editor(false)]
		public string MachineClass
		{
			get
			{
				return this._machineClass;
			}
			set
			{
				if (this._machineClass != value)
				{
					this._machineClass = value;
					base.OnPropertyChanged<string>(value, "MachineClass");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x0001281F File Offset: 0x00010A1F
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x00012827 File Offset: 0x00010A27
		[Editor(false)]
		public Widget MachineIconWidget
		{
			get
			{
				return this._machineIconWidget;
			}
			set
			{
				if (this._machineIconWidget != value)
				{
					this._machineIconWidget = value;
					base.OnPropertyChanged<Widget>(value, "MachineIconWidget");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x040002AA RID: 682
		private bool _isRemainingCountVisible = true;

		// Token: 0x040002AB RID: 683
		private bool _isVisualsDirty = true;

		// Token: 0x040002AC RID: 684
		private int _remainingCount;

		// Token: 0x040002AD RID: 685
		private TextWidget _remainingCountWidget;

		// Token: 0x040002AE RID: 686
		private string _machineClass;

		// Token: 0x040002AF RID: 687
		private Widget _machineIconWidget;
	}
}
