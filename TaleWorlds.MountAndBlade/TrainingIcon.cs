using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000368 RID: 872
	public class TrainingIcon : UsableMachine
	{
		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06003201 RID: 12801 RVA: 0x000CBE1B File Offset: 0x000CA01B
		// (set) Token: 0x06003202 RID: 12802 RVA: 0x000CBE23 File Offset: 0x000CA023
		public bool Focused { get; private set; }

		// Token: 0x06003203 RID: 12803 RVA: 0x000CBE2C File Offset: 0x000CA02C
		protected internal override void OnInit()
		{
			base.OnInit();
			this._markerBeam = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity.GetFirstChildEntityWithTag("highlight_beam"));
			this._weaponIcons = (from x in TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(base.GameEntity).GetChildren()
				where !x.GetScriptComponents().Any<ScriptComponentBehavior>() && x != this._markerBeam
				select x).ToList<GameEntity>();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003204 RID: 12804 RVA: 0x000CBE95 File Offset: 0x000CA095
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06003205 RID: 12805 RVA: 0x000CBEA0 File Offset: 0x000CA0A0
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._markerBeam != null)
			{
				if (MathF.Abs(this._markerAlpha - this._targetMarkerAlpha) > dt * 110f)
				{
					this._markerAlpha += dt * 110f * (float)MathF.Sign(this._targetMarkerAlpha - this._markerAlpha);
					this._markerBeam.GetChild(0).GetFirstMesh().SetVectorArgument(this._markerAlpha, 1f, 0.49f, 11.65f);
				}
				else
				{
					this._markerAlpha = this._targetMarkerAlpha;
					if (this._targetMarkerAlpha == 0f)
					{
						GameEntity markerBeam = this._markerBeam;
						if (markerBeam != null)
						{
							markerBeam.SetVisibilityExcludeParents(false);
						}
					}
				}
			}
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				if (standingPoint.HasUser)
				{
					Agent userAgent = standingPoint.UserAgent;
					ActionIndexCache currentAction = userAgent.GetCurrentAction(0);
					ActionIndexCache currentAction2 = userAgent.GetCurrentAction(1);
					if (!(currentAction2 == ActionIndexCache.act_none) || (!(currentAction == ActionIndexCache.act_pickup_middle_begin) && !(currentAction == ActionIndexCache.act_pickup_middle_begin_left_stance)))
					{
						if (currentAction2 == ActionIndexCache.act_none && (currentAction == ActionIndexCache.act_pickup_middle_end || currentAction == ActionIndexCache.act_pickup_middle_end_left_stance))
						{
							this._activated = true;
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
						else
						{
							if (!(currentAction2 != ActionIndexCache.act_none))
							{
								Agent agent = userAgent;
								int num = 0;
								ActionIndexCache actionIndexCache = (userAgent.GetIsLeftStance() ? ActionIndexCache.act_pickup_middle_begin_left_stance : ActionIndexCache.act_pickup_middle_begin);
								if (agent.SetActionChannel(num, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
								{
									continue;
								}
							}
							userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
						}
					}
				}
			}
		}

		// Token: 0x06003206 RID: 12806 RVA: 0x000CC098 File Offset: 0x000CA298
		public void SetMarked(bool highlight)
		{
			if (!highlight)
			{
				this._targetMarkerAlpha = 0f;
				return;
			}
			this._targetMarkerAlpha = 75f;
			this._markerBeam.GetChild(0).GetFirstMesh().SetVectorArgument(this._markerAlpha, 1f, 0.49f, 11.65f);
			GameEntity markerBeam = this._markerBeam;
			if (markerBeam == null)
			{
				return;
			}
			markerBeam.SetVisibilityExcludeParents(true);
		}

		// Token: 0x06003207 RID: 12807 RVA: 0x000CC0FB File Offset: 0x000CA2FB
		public bool GetIsActivated()
		{
			bool activated = this._activated;
			this._activated = false;
			return activated;
		}

		// Token: 0x06003208 RID: 12808 RVA: 0x000CC10A File Offset: 0x000CA30A
		public string GetTrainingSubTypeTag()
		{
			return this._trainingSubTypeTag;
		}

		// Token: 0x06003209 RID: 12809 RVA: 0x000CC114 File Offset: 0x000CA314
		public void DisableIcon()
		{
			foreach (GameEntity gameEntity in this._weaponIcons)
			{
				gameEntity.SetVisibilityExcludeParents(false);
			}
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x000CC168 File Offset: 0x000CA368
		public void EnableIcon()
		{
			foreach (GameEntity gameEntity in this._weaponIcons)
			{
				gameEntity.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x000CC1BC File Offset: 0x000CA3BC
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			TextObject textObject = new TextObject("{=!}{TRAINING_TYPE}", null);
			textObject.SetTextVariable("TRAINING_TYPE", GameTexts.FindText("str_tutorial_" + this._descriptionTextOfIcon, null));
			return textObject;
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x000CC1EB File Offset: 0x000CA3EB
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject = null)
		{
			TextObject textObject = new TextObject("{=wY1qP2qj}{KEY} Select", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x0600320D RID: 12813 RVA: 0x000CC21A File Offset: 0x000CA41A
		public override void OnFocusGain(Agent userAgent)
		{
			base.OnFocusGain(userAgent);
			this.Focused = true;
		}

		// Token: 0x0600320E RID: 12814 RVA: 0x000CC22A File Offset: 0x000CA42A
		public override void OnFocusLose(Agent userAgent)
		{
			base.OnFocusLose(userAgent);
			this.Focused = false;
		}

		// Token: 0x0400152E RID: 5422
		private const string HighlightBeamTag = "highlight_beam";

		// Token: 0x0400152F RID: 5423
		private const float MarkerAlphaChangeAmount = 110f;

		// Token: 0x04001531 RID: 5425
		private bool _activated;

		// Token: 0x04001532 RID: 5426
		private float _markerAlpha;

		// Token: 0x04001533 RID: 5427
		private float _targetMarkerAlpha;

		// Token: 0x04001534 RID: 5428
		private List<GameEntity> _weaponIcons = new List<GameEntity>();

		// Token: 0x04001535 RID: 5429
		private GameEntity _markerBeam;

		// Token: 0x04001536 RID: 5430
		[EditableScriptComponentVariable(true, "")]
		private string _descriptionTextOfIcon = "";

		// Token: 0x04001537 RID: 5431
		[EditableScriptComponentVariable(true, "")]
		private string _trainingSubTypeTag = "";
	}
}
