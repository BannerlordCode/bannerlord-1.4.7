using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000185 RID: 389
	public class SkillPointsContainerListPanel : ListPanel
	{
		// Token: 0x06001436 RID: 5174 RVA: 0x00037117 File Offset: 0x00035317
		public SkillPointsContainerListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x00037120 File Offset: 0x00035320
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			for (int i = 0; i < base.ChildCount; i++)
			{
				if (!this._initialized)
				{
					base.GetChild(i).RegisterBrushStatesOfWidget();
				}
				bool flag = this.CurrentFocusLevel >= i + 1;
				base.GetChild(i).SetState(flag ? "Full" : "Empty");
			}
			this._initialized = true;
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06001438 RID: 5176 RVA: 0x0003718A File Offset: 0x0003538A
		// (set) Token: 0x06001439 RID: 5177 RVA: 0x00037192 File Offset: 0x00035392
		public int CurrentFocusLevel
		{
			get
			{
				return this._currentFocusLevel;
			}
			set
			{
				if (this._currentFocusLevel != value)
				{
					this._currentFocusLevel = value;
					base.OnPropertyChanged(value, "CurrentFocusLevel");
				}
			}
		}

		// Token: 0x0400092C RID: 2348
		private bool _initialized;

		// Token: 0x0400092D RID: 2349
		private int _currentFocusLevel;
	}
}
