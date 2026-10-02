using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000BD RID: 189
	public class TauntCircleActionSelectorWidget : CircleActionSelectorWidget
	{
		// Token: 0x060009DC RID: 2524 RVA: 0x0001B89E File Offset: 0x00019A9E
		public TauntCircleActionSelectorWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0001B8B8 File Offset: 0x00019AB8
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._currentSelectedIndex != -1)
			{
				Widget child = base.GetChild(this._currentSelectedIndex);
				object obj;
				if (child == null)
				{
					obj = null;
				}
				else
				{
					obj = child.Children.FirstOrDefault<Widget>((Widget c) => c is ButtonWidget);
				}
				ButtonWidget buttonWidget = obj as ButtonWidget;
				Widget widget = ((buttonWidget != null) ? buttonWidget.FindChild("InputKeyContainer", true) : null);
				if (widget != null && !widget.IsVisible)
				{
					base.EventManager.HoveredWidget = buttonWidget;
				}
			}
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0001B944 File Offset: 0x00019B44
		protected override void OnSelectedIndexChanged(int selectedIndex)
		{
			if (this._currentSelectedIndex == selectedIndex)
			{
				return;
			}
			this._currentSelectedIndex = selectedIndex;
			bool flag = false;
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				ButtonWidget buttonWidget = child.Children.FirstOrDefault<Widget>((Widget c) => c is ButtonWidget) as ButtonWidget;
				if (child.GamepadNavigationIndex != -1 && buttonWidget != null)
				{
					bool flag2 = buttonWidget.IsEnabled && this._currentSelectedIndex == i;
					child.DoNotAcceptNavigation = !flag2;
					if (flag2)
					{
						this.SetCurrentNavigationTarget(child);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				this.SetCurrentNavigationTarget(this.FallbackNavigationWidget);
			}
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0001B9F6 File Offset: 0x00019BF6
		private void SetCurrentNavigationTarget(Widget target)
		{
			if (this._tauntSlotNavigationTrialCount == -1)
			{
				this._currentNavigationTarget = target;
				this._tauntSlotNavigationTrialCount = 0;
				base.EventManager.AddLateUpdateAction(this, new Action<float>(this.NavigationUpdate), 1);
			}
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0001BA28 File Offset: 0x00019C28
		private void NavigationUpdate(float dt)
		{
			if (this._currentNavigationTarget != null)
			{
				if (GauntletGamepadNavigationManager.Instance.TryNavigateTo(this._currentNavigationTarget))
				{
					this._currentNavigationTarget = null;
					this._tauntSlotNavigationTrialCount = -1;
					return;
				}
				if (this._tauntSlotNavigationTrialCount < 5)
				{
					this._tauntSlotNavigationTrialCount++;
					base.EventManager.AddLateUpdateAction(this, new Action<float>(this.NavigationUpdate), 1);
					return;
				}
				this._tauntSlotNavigationTrialCount = -1;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060009E1 RID: 2529 RVA: 0x0001BA96 File Offset: 0x00019C96
		// (set) Token: 0x060009E2 RID: 2530 RVA: 0x0001BA9E File Offset: 0x00019C9E
		public Widget FallbackNavigationWidget
		{
			get
			{
				return this._fallbackNavigationWidget;
			}
			set
			{
				if (value != this._fallbackNavigationWidget)
				{
					this._fallbackNavigationWidget = value;
					base.OnPropertyChanged<Widget>(value, "FallbackNavigationWidget");
				}
			}
		}

		// Token: 0x04000473 RID: 1139
		private Widget _currentNavigationTarget;

		// Token: 0x04000474 RID: 1140
		private int _currentSelectedIndex = -1;

		// Token: 0x04000475 RID: 1141
		private int _tauntSlotNavigationTrialCount = -1;

		// Token: 0x04000476 RID: 1142
		private Widget _fallbackNavigationWidget;
	}
}
