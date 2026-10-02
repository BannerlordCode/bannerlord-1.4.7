using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x0200017E RID: 382
	public class CharacterDeveloperPerksContainerWidget : Widget
	{
		// Token: 0x060013D8 RID: 5080 RVA: 0x00035FB9 File Offset: 0x000341B9
		public CharacterDeveloperPerksContainerWidget(UIContext context)
			: base(context)
		{
			this._perkWidgets = new List<PerkItemButtonWidget>();
			this._navigationScopes = new List<GamepadNavigationScope>();
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00035FE0 File Offset: 0x000341E0
		private void RefreshScopes()
		{
			foreach (GamepadNavigationScope gamepadNavigationScope in this._navigationScopes)
			{
				base.GamepadNavigationContext.RemoveNavigationScope(gamepadNavigationScope);
			}
			this._navigationScopes.Clear();
			GamepadNavigationScope gamepadNavigationScope2 = this.BuildNewScope(this.FirstScopeID);
			this._navigationScopes.Add(gamepadNavigationScope2);
			base.GamepadNavigationContext.AddNavigationScope(gamepadNavigationScope2, true);
			int num = -1;
			if (this._perkWidgets.Count > 0)
			{
				num = this._perkWidgets[0].AlternativeType;
			}
			for (int i = 0; i < this._perkWidgets.Count; i++)
			{
				if (this._perkWidgets[i].AlternativeType == 0 || num == 0)
				{
					GamepadNavigationScope gamepadNavigationScope3 = this.BuildNewScope("Scope-" + i);
					this._navigationScopes.Add(gamepadNavigationScope3);
					base.GamepadNavigationContext.AddNavigationScope(gamepadNavigationScope3, true);
				}
				this._perkWidgets[i].GamepadNavigationIndex = 0;
				this._navigationScopes[this._navigationScopes.Count - 1].AddWidget(this._perkWidgets[i]);
				num = this._perkWidgets[i].AlternativeType;
			}
			for (int j = 0; j < this._navigationScopes.Count; j++)
			{
				List<Widget> list = this._navigationScopes[j].NavigatableWidgets.ToList<Widget>();
				list = list.OrderBy<Widget, int>((Widget w) => ((PerkItemButtonWidget)w).AlternativeType).ToList<Widget>();
				this._navigationScopes[j].ClearNavigatableWidgets();
				for (int k = 0; k < list.Count; k++)
				{
					list[k].GamepadNavigationIndex = k;
					this._navigationScopes[j].AddWidget(list[k]);
				}
				if (this._navigationScopes[j].NavigatableWidgets.Count > 1)
				{
					this._navigationScopes[j].AlternateMovementStepSize = MathF.Round((float)this._navigationScopes[j].NavigatableWidgets.Count / 2f);
					this._navigationScopes[j].AlternateScopeMovements = GamepadNavigationTypes.Vertical;
				}
				this._navigationScopes[j].DownNavigationScopeID = this.DownScopeID;
				this._navigationScopes[j].UpNavigationScopeID = this.UpScopeID;
				if (j == 0)
				{
					this._navigationScopes[j].LeftNavigationScopeID = this.LeftScopeID;
					if (this._navigationScopes.Count > 1)
					{
						this._navigationScopes[j].RightNavigationScopeID = this._navigationScopes[j + 1].ScopeID;
					}
				}
				else if (j == this._navigationScopes.Count - 1)
				{
					if (this._navigationScopes.Count > 1)
					{
						this._navigationScopes[j].LeftNavigationScopeID = this._navigationScopes[j - 1].ScopeID;
					}
					this._navigationScopes[j].RightNavigationScopeID = this.RightScopeID;
				}
				else if (j > 0 && j < this._navigationScopes.Count - 1)
				{
					this._navigationScopes[j].LeftNavigationScopeID = this._navigationScopes[j - 1].ScopeID;
					this._navigationScopes[j].RightNavigationScopeID = this._navigationScopes[j + 1].ScopeID;
				}
			}
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x000363AC File Offset: 0x000345AC
		protected override void OnLateUpdate(float dt)
		{
			if (!this._initialized || this._lastPerkCount != this._perkWidgets.Count)
			{
				this.RefreshScopes();
				this._initialized = true;
				this._lastPerkCount = this._perkWidgets.Count;
			}
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x000363E7 File Offset: 0x000345E7
		private GamepadNavigationScope BuildNewScope(string scopeID)
		{
			return new GamepadNavigationScope
			{
				ScopeID = scopeID,
				ParentWidget = this,
				ScopeMovements = GamepadNavigationTypes.Horizontal,
				DoNotAutomaticallyFindChildren = true
			};
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0003640C File Offset: 0x0003460C
		protected override void OnChildAdded(Widget child)
		{
			PerkItemButtonWidget perkItemButtonWidget;
			if ((perkItemButtonWidget = child as PerkItemButtonWidget) != null)
			{
				this._perkWidgets.Add(perkItemButtonWidget);
				this._initialized = false;
			}
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x00036438 File Offset: 0x00034638
		protected override void OnBeforeChildRemoved(Widget child)
		{
			PerkItemButtonWidget perkItemButtonWidget;
			if ((perkItemButtonWidget = child as PerkItemButtonWidget) != null)
			{
				this._perkWidgets.Remove(perkItemButtonWidget);
				this._initialized = false;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x00036463 File Offset: 0x00034663
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x0003646B File Offset: 0x0003466B
		public string LeftScopeID { get; set; }

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x00036474 File Offset: 0x00034674
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x0003647C File Offset: 0x0003467C
		public string RightScopeID { get; set; }

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x00036485 File Offset: 0x00034685
		// (set) Token: 0x060013E3 RID: 5091 RVA: 0x0003648D File Offset: 0x0003468D
		public string DownScopeID { get; set; }

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x00036496 File Offset: 0x00034696
		// (set) Token: 0x060013E5 RID: 5093 RVA: 0x0003649E File Offset: 0x0003469E
		public string UpScopeID { get; set; }

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060013E6 RID: 5094 RVA: 0x000364A7 File Offset: 0x000346A7
		// (set) Token: 0x060013E7 RID: 5095 RVA: 0x000364AF File Offset: 0x000346AF
		public string FirstScopeID { get; set; }

		// Token: 0x040008FF RID: 2303
		private List<GamepadNavigationScope> _navigationScopes;

		// Token: 0x04000900 RID: 2304
		private List<PerkItemButtonWidget> _perkWidgets;

		// Token: 0x04000901 RID: 2305
		private bool _initialized;

		// Token: 0x04000902 RID: 2306
		private int _lastPerkCount = -1;
	}
}
