using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tutorial
{
	// Token: 0x0200004E RID: 78
	public class TutorialObjectiveStickParentWidget : TextWidget
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x0000D6CD File Offset: 0x0000B8CD
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x0000D6D5 File Offset: 0x0000B8D5
		public Widget StickMiddle { get; set; }

		// Token: 0x06000446 RID: 1094 RVA: 0x0000D6E0 File Offset: 0x0000B8E0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._animQueue.Count > 0)
			{
				base.ParentWidget.ParentWidget.AlphaFactor = 0.5f;
				this._animQueue.Peek().ForEach(delegate(TutorialObjectiveStickParentWidget.StickAnimStage a)
				{
					a.Tick(dt);
				});
				if (this._animQueue.Peek().All<TutorialObjectiveStickParentWidget.StickAnimStage>((TutorialObjectiveStickParentWidget.StickAnimStage a) => a.IsCompleted))
				{
					this._animQueue.Dequeue();
					return;
				}
			}
			else
			{
				this.UpdateAnimQueue();
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000D789 File Offset: 0x0000B989
		public TutorialObjectiveStickParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000D79D File Offset: 0x0000B99D
		private void ResetAnim()
		{
			base.PositionXOffset = 0f;
			base.PositionYOffset = 0f;
			this.SetGlobalAlphaRecursively(0f);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000D7C0 File Offset: 0x0000B9C0
		private void UpdateAnimQueue()
		{
			this.ResetAnim();
			switch (this.MovementType)
			{
			case 1:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(-20f, 0f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(1f) });
				return;
			case 2:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(20f, 0f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(1f) });
				return;
			case 3:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(0f, -20f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(1f) });
				return;
			case 4:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(0f, 20f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(1f) });
				return;
			case 5:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(-20f, 0f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(2f) });
				return;
			case 6:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(20f, 0f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(2f) });
				return;
			case 7:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(0f, -20f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(2f) });
				return;
			case 8:
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this, true),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage>
				{
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateMovementStage(0.15f, new Vec2(0f, 20f), this.StickMiddle),
					TutorialObjectiveStickParentWidget.StickAnimStage.CreateFadeInStage(0.15f, this.StickMiddle, false)
				});
				this._animQueue.Enqueue(new List<TutorialObjectiveStickParentWidget.StickAnimStage> { TutorialObjectiveStickParentWidget.StickAnimStage.CreateStayStage(2f) });
				return;
			default:
				return;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0000DD32 File Offset: 0x0000BF32
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0000DD3A File Offset: 0x0000BF3A
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

		// Token: 0x040001CB RID: 459
		private const float LongStayTime = 1f;

		// Token: 0x040001CC RID: 460
		private const float ShortStayTime = 0.1f;

		// Token: 0x040001CD RID: 461
		private const float FadeInTime = 0.15f;

		// Token: 0x040001CE RID: 462
		private const float FadeOutTime = 0.15f;

		// Token: 0x040001CF RID: 463
		private const float SingleMovementDirection = 20f;

		// Token: 0x040001D0 RID: 464
		private const float MovementTime = 0.15f;

		// Token: 0x040001D1 RID: 465
		private const float ParentActiveAlpha = 0.5f;

		// Token: 0x040001D3 RID: 467
		private Queue<List<TutorialObjectiveStickParentWidget.StickAnimStage>> _animQueue = new Queue<List<TutorialObjectiveStickParentWidget.StickAnimStage>>();

		// Token: 0x040001D4 RID: 468
		private int _movementType;

		// Token: 0x020001AB RID: 427
		public class StickAnimStage
		{
			// Token: 0x17000761 RID: 1889
			// (get) Token: 0x06001508 RID: 5384 RVA: 0x0003972C File Offset: 0x0003792C
			// (set) Token: 0x06001509 RID: 5385 RVA: 0x00039734 File Offset: 0x00037934
			public bool IsCompleted { get; private set; }

			// Token: 0x17000762 RID: 1890
			// (get) Token: 0x0600150A RID: 5386 RVA: 0x0003973D File Offset: 0x0003793D
			// (set) Token: 0x0600150B RID: 5387 RVA: 0x00039745 File Offset: 0x00037945
			public float AnimTime { get; private set; }

			// Token: 0x17000763 RID: 1891
			// (get) Token: 0x0600150C RID: 5388 RVA: 0x0003974E File Offset: 0x0003794E
			// (set) Token: 0x0600150D RID: 5389 RVA: 0x00039756 File Offset: 0x00037956
			public Vec2 Direction { get; private set; }

			// Token: 0x17000764 RID: 1892
			// (get) Token: 0x0600150E RID: 5390 RVA: 0x0003975F File Offset: 0x0003795F
			// (set) Token: 0x0600150F RID: 5391 RVA: 0x00039767 File Offset: 0x00037967
			public TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes AnimType { get; private set; }

			// Token: 0x17000765 RID: 1893
			// (get) Token: 0x06001510 RID: 5392 RVA: 0x00039770 File Offset: 0x00037970
			// (set) Token: 0x06001511 RID: 5393 RVA: 0x00039778 File Offset: 0x00037978
			public Widget WidgetToManipulate { get; private set; }

			// Token: 0x06001512 RID: 5394 RVA: 0x00039781 File Offset: 0x00037981
			private StickAnimStage()
			{
			}

			// Token: 0x06001513 RID: 5395 RVA: 0x00039789 File Offset: 0x00037989
			internal static TutorialObjectiveStickParentWidget.StickAnimStage CreateMovementStage(float movementTime, Vec2 direction, Widget widgetToManipulate)
			{
				return new TutorialObjectiveStickParentWidget.StickAnimStage
				{
					AnimTime = movementTime,
					Direction = direction,
					AnimType = TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.Movement,
					WidgetToManipulate = widgetToManipulate
				};
			}

			// Token: 0x06001514 RID: 5396 RVA: 0x000397AC File Offset: 0x000379AC
			internal static TutorialObjectiveStickParentWidget.StickAnimStage CreateFadeInStage(float fadeInTime, Widget widgetToManipulate, bool isGlobal)
			{
				return new TutorialObjectiveStickParentWidget.StickAnimStage
				{
					AnimTime = fadeInTime,
					AnimType = (isGlobal ? TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeInGlobal : TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeInLocal),
					WidgetToManipulate = widgetToManipulate
				};
			}

			// Token: 0x06001515 RID: 5397 RVA: 0x000397CE File Offset: 0x000379CE
			internal static TutorialObjectiveStickParentWidget.StickAnimStage CreateStayStage(float stayTime)
			{
				return new TutorialObjectiveStickParentWidget.StickAnimStage
				{
					AnimTime = stayTime,
					AnimType = TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.Stay,
					WidgetToManipulate = null
				};
			}

			// Token: 0x06001516 RID: 5398 RVA: 0x000397EC File Offset: 0x000379EC
			public void Tick(float dt)
			{
				float num = MathF.Clamp(this._totalTime / this.AnimTime, 0f, 1f);
				switch (this.AnimType)
				{
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.Movement:
					this.WidgetToManipulate.PositionXOffset = ((this.Direction.X != 0f) ? MathF.Lerp(0f, this.Direction.X, num, 1E-05f) : 0f);
					this.WidgetToManipulate.PositionYOffset = ((this.Direction.Y != 0f) ? MathF.Lerp(0f, this.Direction.Y, num, 1E-05f) : 0f);
					this.IsCompleted = this._totalTime > this.AnimTime;
					break;
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeInLocal:
					this.WidgetToManipulate.AlphaFactor = num;
					this.IsCompleted = this.WidgetToManipulate.AlphaFactor > 0.98f;
					break;
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeOutLocal:
					this.WidgetToManipulate.AlphaFactor = 1f - num;
					this.IsCompleted = this.WidgetToManipulate.AlphaFactor < 0.02f;
					break;
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeInGlobal:
					this.WidgetToManipulate.SetGlobalAlphaRecursively(num);
					this.IsCompleted = this.WidgetToManipulate.AlphaFactor > 0.98f;
					break;
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.FadeOutGlobal:
					this.WidgetToManipulate.SetGlobalAlphaRecursively(1f - num);
					this.IsCompleted = this.WidgetToManipulate.AlphaFactor < 0.02f;
					break;
				case TutorialObjectiveStickParentWidget.StickAnimStage.AnimTypes.Stay:
					this.IsCompleted = this._totalTime > this.AnimTime;
					break;
				}
				this._totalTime += dt;
			}

			// Token: 0x040009D7 RID: 2519
			private float _totalTime;

			// Token: 0x020001DA RID: 474
			public enum AnimTypes
			{
				// Token: 0x04000A6C RID: 2668
				Movement,
				// Token: 0x04000A6D RID: 2669
				FadeInLocal,
				// Token: 0x04000A6E RID: 2670
				FadeOutLocal,
				// Token: 0x04000A6F RID: 2671
				FadeInGlobal,
				// Token: 0x04000A70 RID: 2672
				FadeOutGlobal,
				// Token: 0x04000A71 RID: 2673
				Stay
			}
		}
	}
}
