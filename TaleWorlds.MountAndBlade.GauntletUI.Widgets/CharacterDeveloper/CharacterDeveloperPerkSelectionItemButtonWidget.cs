using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterDeveloper
{
	// Token: 0x0200017F RID: 383
	public class CharacterDeveloperPerkSelectionItemButtonWidget : ButtonWidget
	{
		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x000364B8 File Offset: 0x000346B8
		// (set) Token: 0x060013E9 RID: 5097 RVA: 0x000364C0 File Offset: 0x000346C0
		public Widget PerkSelectionIndicatorWidget { get; set; }

		// Token: 0x060013EA RID: 5098 RVA: 0x000364C9 File Offset: 0x000346C9
		public CharacterDeveloperPerkSelectionItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x000364D4 File Offset: 0x000346D4
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.PerkSelectionIndicatorWidget != null)
			{
				if (base.ParentWidget.ChildCount == 1)
				{
					this.PerkSelectionIndicatorWidget.VerticalAlignment = VerticalAlignment.Center;
					return;
				}
				this.PerkSelectionIndicatorWidget.VerticalAlignment = ((base.GetSiblingIndex() % 2 == 0) ? VerticalAlignment.Bottom : VerticalAlignment.Top);
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00036524 File Offset: 0x00034724
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x0003652C File Offset: 0x0003472C
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
		}
	}
}
