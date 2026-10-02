using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004C RID: 76
	public class TutorialObjectiveItemWidget : Widget
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x0000D15A File Offset: 0x0000B35A
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x0000D162 File Offset: 0x0000B362
		public InputKeyVisualWidget KeyPressWidget { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0000D16B File Offset: 0x0000B36B
		// (set) Token: 0x0600042B RID: 1067 RVA: 0x0000D173 File Offset: 0x0000B373
		public TutorialObjectiveMouseParentWidget MouseMoveWidget { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x0000D17C File Offset: 0x0000B37C
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x0000D184 File Offset: 0x0000B384
		public TutorialObjectiveStickParentWidget StickMoveWidget { get; set; }

		// Token: 0x0600042E RID: 1070 RVA: 0x0000D18D File Offset: 0x0000B38D
		public TutorialObjectiveItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000D198 File Offset: 0x0000B398
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			this.KeyPressWidget.IsVisible = this.InputType == 1;
			this.MouseMoveWidget.IsVisible = this.InputType == 0;
			this.StickMoveWidget.IsVisible = this.InputType == 2;
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x0000D1E8 File Offset: 0x0000B3E8
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
		[Editor(false)]
		public int MovementType
		{
			get
			{
				return this._movementType;
			}
			set
			{
				if (value != this._movementType)
				{
					this._movementType = value;
					base.OnPropertyChanged(value, "MovementType");
				}
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0000D20E File Offset: 0x0000B40E
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x0000D216 File Offset: 0x0000B416
		[Editor(false)]
		public int InputType
		{
			get
			{
				return this._inputType;
			}
			set
			{
				if (value != this._inputType)
				{
					this._inputType = value;
					base.OnPropertyChanged(value, "InputType");
				}
			}
		}

		// Token: 0x040001C2 RID: 450
		private int _movementType;

		// Token: 0x040001C3 RID: 451
		private int _inputType;

		// Token: 0x020001A6 RID: 422
		public enum InputTypes
		{
			// Token: 0x040009C3 RID: 2499
			MouseAndClick,
			// Token: 0x040009C4 RID: 2500
			Key,
			// Token: 0x040009C5 RID: 2501
			ControllerStick
		}
	}
}
