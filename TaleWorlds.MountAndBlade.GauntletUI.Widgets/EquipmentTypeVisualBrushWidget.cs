using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200001B RID: 27
	public class EquipmentTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000161 RID: 353 RVA: 0x00005E96 File Offset: 0x00004096
		public EquipmentTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00005EAA File Offset: 0x000040AA
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._hasVisualDetermined)
			{
				this.RegisterBrushStatesOfWidget();
				this.UpdateVisual(this.Type);
				this._hasVisualDetermined = true;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00005ED4 File Offset: 0x000040D4
		private void UpdateVisual(string type)
		{
			if (base.ContainsState(type))
			{
				this.SetState(type);
				return;
			}
			this.SetState("Invalid");
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00005EF2 File Offset: 0x000040F2
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00005EFA File Offset: 0x000040FA
		[Editor(false)]
		public string Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged<string>(value, "Type");
				}
			}
		}

		// Token: 0x040000A5 RID: 165
		private bool _hasVisualDetermined;

		// Token: 0x040000A6 RID: 166
		private string _type = "";
	}
}
