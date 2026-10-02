using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard
{
	// Token: 0x0200005A RID: 90
	public class ScoreboardSkillItemHoverToggleWidget : HoverToggleWidget
	{
		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x0000F5D8 File Offset: 0x0000D7D8
		// (set) Token: 0x060004F0 RID: 1264 RVA: 0x0000F5E0 File Offset: 0x0000D7E0
		public ScoreboardGainedSkillsListPanel SkillsShowWidget { get; set; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0000F5E9 File Offset: 0x0000D7E9
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x0000F5F1 File Offset: 0x0000D7F1
		public ListPanel GainedSkillsList { get; set; }

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000F5FA File Offset: 0x0000D7FA
		public ScoreboardSkillItemHoverToggleWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000F603 File Offset: 0x0000D803
		public List<Widget> GetAllSkillWidgets()
		{
			return this.GainedSkillsList.Children;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000F610 File Offset: 0x0000D810
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (base.IsOverWidget && !this._isHoverBeginHandled)
			{
				this.SkillsShowWidget.SetCurrentUnit(this);
				this._isHoverBeginHandled = true;
				this._isHoverEndHandled = true;
				return;
			}
			if (!base.IsOverWidget && this._isHoverEndHandled)
			{
				this.SkillsShowWidget.SetCurrentUnit(null);
				this._isHoverEndHandled = false;
				this._isHoverBeginHandled = false;
			}
		}

		// Token: 0x04000220 RID: 544
		private bool _isHoverEndHandled;

		// Token: 0x04000221 RID: 545
		private bool _isHoverBeginHandled;
	}
}
