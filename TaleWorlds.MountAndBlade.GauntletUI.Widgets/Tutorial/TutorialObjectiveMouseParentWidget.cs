using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004D RID: 77
	public class TutorialObjectiveMouseParentWidget : Widget
	{
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0000D234 File Offset: 0x0000B434
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x0000D23C File Offset: 0x0000B43C
		public BrushWidget MouseBodyWidget { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0000D245 File Offset: 0x0000B445
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x0000D24D File Offset: 0x0000B44D
		public BrushWidget MouseLeftClickWidget { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000438 RID: 1080 RVA: 0x0000D256 File Offset: 0x0000B456
		// (set) Token: 0x06000439 RID: 1081 RVA: 0x0000D25E File Offset: 0x0000B45E
		public BrushWidget MouseRightClickWidget { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x0000D267 File Offset: 0x0000B467
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x0000D26F File Offset: 0x0000B46F
		public BrushWidget MouseMiddleClickWidget { get; set; }

		// Token: 0x0600043C RID: 1084 RVA: 0x0000D278 File Offset: 0x0000B478
		public TutorialObjectiveMouseParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000D284 File Offset: 0x0000B484
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this._animationsSet)
			{
				if (this.MouseBodyWidget == null || this.MouseLeftClickWidget == null || this.MouseRightClickWidget == null || this.MouseMiddleClickWidget == null)
				{
					return;
				}
				this._animationsSet = true;
				BrushAnimation animation2 = this.MouseLeftClickWidget.Brush.GetAnimation("BlinkAnimation");
				using (IEnumerator<BrushAnimation> enumerator = this.MouseLeftClickWidget.Brush.GetAnimations().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BrushAnimation animation3 = enumerator.Current;
						if (animation3 != animation2)
						{
							Action<BrushAnimationProperty> <>9__0;
							foreach (BrushLayerAnimation brushLayerAnimation in animation2.GetLayerAnimations())
							{
								List<BrushAnimationProperty> list = brushLayerAnimation.Collections.ToList<BrushAnimationProperty>();
								Action<BrushAnimationProperty> action;
								if ((action = <>9__0) == null)
								{
									action = (<>9__0 = delegate(BrushAnimationProperty x)
									{
										animation3.AddAnimationProperty(x);
									});
								}
								list.ForEach(action);
							}
						}
					}
				}
				using (IEnumerator<BrushAnimation> enumerator = this.MouseRightClickWidget.Brush.GetAnimations().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BrushAnimation animation4 = enumerator.Current;
						if (animation4 != animation2)
						{
							Action<BrushAnimationProperty> <>9__1;
							foreach (BrushLayerAnimation brushLayerAnimation2 in animation2.GetLayerAnimations())
							{
								List<BrushAnimationProperty> list2 = brushLayerAnimation2.Collections.ToList<BrushAnimationProperty>();
								Action<BrushAnimationProperty> action2;
								if ((action2 = <>9__1) == null)
								{
									action2 = (<>9__1 = delegate(BrushAnimationProperty x)
									{
										animation4.AddAnimationProperty(x);
									});
								}
								list2.ForEach(action2);
							}
						}
					}
				}
				using (IEnumerator<BrushAnimation> enumerator = this.MouseMiddleClickWidget.Brush.GetAnimations().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BrushAnimation animation = enumerator.Current;
						if (animation != animation2)
						{
							Action<BrushAnimationProperty> <>9__2;
							foreach (BrushLayerAnimation brushLayerAnimation3 in animation2.GetLayerAnimations())
							{
								List<BrushAnimationProperty> list3 = brushLayerAnimation3.Collections.ToList<BrushAnimationProperty>();
								Action<BrushAnimationProperty> action3;
								if ((action3 = <>9__2) == null)
								{
									action3 = (<>9__2 = delegate(BrushAnimationProperty x)
									{
										animation.AddAnimationProperty(x);
									});
								}
								list3.ForEach(action3);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000D51C File Offset: 0x0000B71C
		private void DecideMovement()
		{
			switch (this.MovementType)
			{
			case 1:
				this.MouseBodyWidget.SetState("Left");
				return;
			case 2:
				this.MouseBodyWidget.SetState("Right");
				return;
			case 3:
				this.MouseBodyWidget.SetState("Up");
				return;
			case 4:
				this.MouseBodyWidget.SetState("Down");
				return;
			default:
				this.MouseBodyWidget.SetState("Default");
				return;
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000D5A0 File Offset: 0x0000B7A0
		private void DecideClick()
		{
			string keyId = this.KeyId;
			if (keyId == "mouse_left_click")
			{
				this.MouseLeftClickWidget.IsVisible = true;
				this.MouseMiddleClickWidget.IsVisible = false;
				this.MouseRightClickWidget.IsVisible = false;
				return;
			}
			if (keyId == "mouse_middle_click")
			{
				this.MouseLeftClickWidget.IsVisible = false;
				this.MouseMiddleClickWidget.IsVisible = true;
				this.MouseRightClickWidget.IsVisible = false;
				return;
			}
			if (!(keyId == "mouse_right_click"))
			{
				this.MouseLeftClickWidget.IsVisible = false;
				this.MouseMiddleClickWidget.IsVisible = false;
				this.MouseRightClickWidget.IsVisible = false;
				return;
			}
			this.MouseLeftClickWidget.IsVisible = false;
			this.MouseMiddleClickWidget.IsVisible = false;
			this.MouseRightClickWidget.IsVisible = true;
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0000D670 File Offset: 0x0000B870
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x0000D678 File Offset: 0x0000B878
		[Editor(false)]
		public string KeyId
		{
			get
			{
				return this._keyId;
			}
			set
			{
				if (value != this._keyId)
				{
					this._keyId = value;
					this.DecideClick();
					base.OnPropertyChanged<string>(value, "KeyId");
				}
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x0000D6A1 File Offset: 0x0000B8A1
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x0000D6A9 File Offset: 0x0000B8A9
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
					this.DecideMovement();
					base.OnPropertyChanged(value, "MovementType");
				}
			}
		}

		// Token: 0x040001C8 RID: 456
		private bool _animationsSet;

		// Token: 0x040001C9 RID: 457
		private string _keyId;

		// Token: 0x040001CA RID: 458
		private int _movementType;

		// Token: 0x020001A7 RID: 423
		public enum MovementTypes
		{
			// Token: 0x040009C7 RID: 2503
			None,
			// Token: 0x040009C8 RID: 2504
			MoveLeft,
			// Token: 0x040009C9 RID: 2505
			MoveRight,
			// Token: 0x040009CA RID: 2506
			MoveUp,
			// Token: 0x040009CB RID: 2507
			MoveDown
		}
	}
}
