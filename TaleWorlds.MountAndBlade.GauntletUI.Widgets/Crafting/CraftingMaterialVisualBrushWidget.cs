using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x02000166 RID: 358
	public class CraftingMaterialVisualBrushWidget : BrushWidget
	{
		// Token: 0x060012E9 RID: 4841 RVA: 0x00033CB3 File Offset: 0x00031EB3
		public CraftingMaterialVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00033CC3 File Offset: 0x00031EC3
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._visualDirty)
			{
				this.UpdateVisual();
				this._visualDirty = false;
			}
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x00033CE4 File Offset: 0x00031EE4
		private void UpdateVisual()
		{
			this.RegisterBrushStatesOfWidget();
			string text = this.MaterialType;
			if (this.IsBig)
			{
				text += "Big";
			}
			this.SetState(text);
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060012EC RID: 4844 RVA: 0x00033D19 File Offset: 0x00031F19
		// (set) Token: 0x060012ED RID: 4845 RVA: 0x00033D21 File Offset: 0x00031F21
		public string MaterialType
		{
			get
			{
				return this._materialType;
			}
			set
			{
				if (this._materialType != value)
				{
					this._materialType = value;
					this._visualDirty = true;
				}
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x00033D3F File Offset: 0x00031F3F
		// (set) Token: 0x060012EF RID: 4847 RVA: 0x00033D47 File Offset: 0x00031F47
		public bool IsBig
		{
			get
			{
				return this._isBig;
			}
			set
			{
				if (this._isBig != value)
				{
					this._isBig = value;
					this._visualDirty = true;
				}
			}
		}

		// Token: 0x04000896 RID: 2198
		private bool _visualDirty = true;

		// Token: 0x04000897 RID: 2199
		private string _materialType;

		// Token: 0x04000898 RID: 2200
		private bool _isBig;
	}
}
