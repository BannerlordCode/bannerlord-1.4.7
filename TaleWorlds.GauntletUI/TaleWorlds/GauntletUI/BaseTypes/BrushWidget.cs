using System;
using System.Numerics;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000052 RID: 82
	public class BrushWidget : Widget
	{
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x00017164 File Offset: 0x00015364
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x000171DE File Offset: 0x000153DE
		[Editor(false)]
		public Brush Brush
		{
			get
			{
				if (this._originalBrush == null)
				{
					this._originalBrush = base.Context.DefaultBrush;
					this._clonedBrush = this._originalBrush.Clone();
					this.BrushRenderer.Brush = this.ReadOnlyBrush;
				}
				else if (this._clonedBrush == null)
				{
					this._clonedBrush = this._originalBrush.Clone();
					this.BrushRenderer.Brush = this.ReadOnlyBrush;
				}
				return this._clonedBrush;
			}
			set
			{
				if (this._originalBrush != value)
				{
					this._originalBrush = value;
					this._clonedBrush = null;
					this.OnBrushChanged();
					base.OnPropertyChanged<Brush>(value, "Brush");
				}
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00017209 File Offset: 0x00015409
		public Brush ReadOnlyBrush
		{
			get
			{
				if (this._clonedBrush != null)
				{
					return this._clonedBrush;
				}
				if (this._originalBrush == null)
				{
					this._originalBrush = base.Context.DefaultBrush;
				}
				return this._originalBrush;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00017239 File Offset: 0x00015439
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x00017255 File Offset: 0x00015455
		[Editor(false)]
		public new Sprite Sprite
		{
			get
			{
				return this.ReadOnlyBrush.DefaultStyle.GetLayer("Default").Sprite;
			}
			set
			{
				this.Brush.DefaultStyle.GetLayer("Default").Sprite = value;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00017272 File Offset: 0x00015472
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x0001727A File Offset: 0x0001547A
		public BrushRenderer BrushRenderer { get; private set; }

		// Token: 0x06000585 RID: 1413 RVA: 0x00017283 File Offset: 0x00015483
		public BrushWidget(UIContext context)
			: base(context)
		{
			this.BrushRenderer = new BrushRenderer();
			base.EventFire += this.BrushWidget_EventFire;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x000172AC File Offset: 0x000154AC
		private void BrushWidget_EventFire(Widget arg1, string eventName, object[] arg3)
		{
			if (this.ReadOnlyBrush != null)
			{
				AudioProperty eventAudioProperty = this.ReadOnlyBrush.SoundProperties.GetEventAudioProperty(eventName);
				if (eventAudioProperty != null && eventAudioProperty.AudioName != null && !eventAudioProperty.AudioName.Equals(""))
				{
					base.EventManager.Context.TwoDimensionContext.PlaySound(eventAudioProperty.AudioName);
				}
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001730B File Offset: 0x0001550B
		public override void UpdateBrushes(float dt)
		{
			this.UpdateBrushRendererInternal(dt);
			if (!this.IsBrushUpdateNeeded())
			{
				this.UnRegisterUpdateBrushes();
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00017322 File Offset: 0x00015522
		protected bool IsBrushUpdateNeeded()
		{
			Brush brush = this.Brush;
			return base.IsVisible && this.BrushRenderer.IsUpdateNeeded() && this.AreaRect.IsCollide(in base.EventManager.AreaRectangle);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00017358 File Offset: 0x00015558
		protected void UpdateBrushRendererInternal(float dt)
		{
			UIContext context = base.Context;
			bool flag;
			if (context == null)
			{
				flag = null != null;
			}
			else
			{
				TwoDimensionContext twoDimensionContext = context.TwoDimensionContext;
				flag = ((twoDimensionContext != null) ? twoDimensionContext.Platform : null) != null;
			}
			if (!flag)
			{
				Debug.FailedAssert("Trying to update brush renderer after context or platform is finalized", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\BaseTypes\\BrushWidget.cs", "UpdateBrushRendererInternal", 129);
				return;
			}
			this.BrushRenderer.ForcePixelPerfectPlacement = base.ForcePixelPerfectRenderPlacement;
			this.BrushRenderer.UseLocalTimer = !base.UseGlobalTimeForAnimation;
			this.BrushRenderer.Brush = this.ReadOnlyBrush;
			this.BrushRenderer.CurrentState = base.CurrentState;
			this.BrushRenderer.Update(base.EventManager.LocalFrameNumber, base.Context.TwoDimensionContext.Platform.ApplicationTime, dt);
			if (base.RestartAnimationFirstFrame && !this._animRestarted)
			{
				base.EventManager.AddLateUpdateAction(this, delegate(float _dt)
				{
					if (base.RestartAnimationFirstFrame)
					{
						this.BrushRenderer.RestartAnimation();
					}
				}, 5);
				this._animRestarted = true;
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00017444 File Offset: 0x00015644
		public override void SetState(string stateName)
		{
			if (base.CurrentState != stateName)
			{
				if (base.EventManager != null && this.ReadOnlyBrush != null)
				{
					AudioProperty stateAudioProperty = this.ReadOnlyBrush.SoundProperties.GetStateAudioProperty(stateName);
					if (stateAudioProperty != null)
					{
						if (stateAudioProperty.AudioName != null && !stateAudioProperty.AudioName.Equals(""))
						{
							base.EventManager.Context.TwoDimensionContext.PlaySound(stateAudioProperty.AudioName);
						}
						else
						{
							Debug.FailedAssert(string.Concat(new string[] { "Widget with id \"", base.Id, "\" has a sound having no audioName for event \"", stateName, "\"!" }), "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\GauntletUI\\TaleWorlds.GauntletUI\\BaseTypes\\BrushWidget.cs", "SetState", 169);
						}
					}
				}
				this.RegisterUpdateBrushes();
			}
			base.SetState(stateName);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00017515 File Offset: 0x00015715
		protected override void RefreshState()
		{
			base.RefreshState();
			this.RegisterUpdateBrushes();
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00017524 File Offset: 0x00015724
		protected override void OnRender(TwoDimensionContext twoDimensionContext, TwoDimensionDrawContext drawContext)
		{
			if (this.IsBrushUpdateNeeded() && base.EventManager.LocalFrameNumber != this.BrushRenderer.LastUpdatedFrameNumber)
			{
				this.RegisterUpdateBrushes();
				this.UpdateBrushRendererInternal(base.EventManager.CachedDt);
			}
			this.BrushRenderer.Render(drawContext, in this.AreaRect, base._scaleToUse, base.Context.ContextAlpha, default(Vector2), default(Vector2));
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001759D File Offset: 0x0001579D
		protected override void OnConnectedToRoot()
		{
			base.OnConnectedToRoot();
			this.BrushRenderer.SetSeed(this._seed);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x000175B8 File Offset: 0x000157B8
		public override void UpdateAnimationPropertiesSubTask(float alphaFactor)
		{
			this.Brush.GlobalAlphaFactor = alphaFactor;
			foreach (Widget widget in base.Children)
			{
				widget.UpdateAnimationPropertiesSubTask(alphaFactor);
			}
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00017618 File Offset: 0x00015818
		public virtual void OnBrushChanged()
		{
			this.RegisterUpdateBrushes();
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00017620 File Offset: 0x00015820
		protected void RegisterUpdateBrushes()
		{
			base.EventManager.RegisterWidgetForEvent(WidgetContainer.ContainerType.UpdateBrushes, this);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001762F File Offset: 0x0001582F
		protected void UnRegisterUpdateBrushes()
		{
			base.EventManager.UnRegisterWidgetForEvent(WidgetContainer.ContainerType.UpdateBrushes, this);
		}

		// Token: 0x040002A1 RID: 673
		private Brush _originalBrush;

		// Token: 0x040002A2 RID: 674
		private Brush _clonedBrush;

		// Token: 0x040002A4 RID: 676
		private bool _animRestarted;
	}
}
