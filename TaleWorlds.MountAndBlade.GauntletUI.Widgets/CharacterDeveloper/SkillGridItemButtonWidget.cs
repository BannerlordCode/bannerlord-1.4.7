using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x02000184 RID: 388
	public class SkillGridItemButtonWidget : ButtonWidget
	{
		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x0600142A RID: 5162 RVA: 0x0003700E File Offset: 0x0003520E
		// (set) Token: 0x0600142B RID: 5163 RVA: 0x00037016 File Offset: 0x00035216
		public Brush CannotLearnBrush { get; set; }

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x0600142C RID: 5164 RVA: 0x0003701F File Offset: 0x0003521F
		// (set) Token: 0x0600142D RID: 5165 RVA: 0x00037027 File Offset: 0x00035227
		public Brush CanLearnBrush { get; set; }

		// Token: 0x0600142E RID: 5166 RVA: 0x00037030 File Offset: 0x00035230
		public SkillGridItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x00037040 File Offset: 0x00035240
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			Widget focusLevelWidget = this.FocusLevelWidget;
			if (focusLevelWidget != null)
			{
				focusLevelWidget.SetState(this.CurrentFocusLevel.ToString());
			}
			if (this._isVisualsDirty)
			{
				base.Brush = (this.CanLearnSkill ? this.CanLearnBrush : this.CannotLearnBrush);
				this._isVisualsDirty = false;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06001430 RID: 5168 RVA: 0x0003709E File Offset: 0x0003529E
		// (set) Token: 0x06001431 RID: 5169 RVA: 0x000370A6 File Offset: 0x000352A6
		public Widget FocusLevelWidget
		{
			get
			{
				return this._focusLevelWidget;
			}
			set
			{
				if (this._focusLevelWidget != value)
				{
					this._focusLevelWidget = value;
					base.OnPropertyChanged<Widget>(value, "FocusLevelWidget");
				}
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06001432 RID: 5170 RVA: 0x000370C4 File Offset: 0x000352C4
		// (set) Token: 0x06001433 RID: 5171 RVA: 0x000370CC File Offset: 0x000352CC
		public bool CanLearnSkill
		{
			get
			{
				return this._canLearnSkill;
			}
			set
			{
				if (this._canLearnSkill != value)
				{
					this._canLearnSkill = value;
					base.OnPropertyChanged(value, "CanLearnSkill");
					this._isVisualsDirty = true;
				}
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x000370F1 File Offset: 0x000352F1
		// (set) Token: 0x06001435 RID: 5173 RVA: 0x000370F9 File Offset: 0x000352F9
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

		// Token: 0x04000928 RID: 2344
		private bool _isVisualsDirty = true;

		// Token: 0x04000929 RID: 2345
		private Widget _focusLevelWidget;

		// Token: 0x0400092A RID: 2346
		private int _currentFocusLevel;

		// Token: 0x0400092B RID: 2347
		private bool _canLearnSkill;
	}
}
