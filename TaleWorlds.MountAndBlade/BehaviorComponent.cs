using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200010F RID: 271
	public abstract class BehaviorComponent
	{
		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0001CA81 File Offset: 0x0001AC81
		// (set) Token: 0x06000DBC RID: 3516 RVA: 0x0001CA89 File Offset: 0x0001AC89
		public Formation Formation { get; private set; }

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x0001CA92 File Offset: 0x0001AC92
		// (set) Token: 0x06000DBE RID: 3518 RVA: 0x0001CA9A File Offset: 0x0001AC9A
		public float BehaviorCoherence { get; set; }

		// Token: 0x06000DBF RID: 3519 RVA: 0x0001CAA4 File Offset: 0x0001ACA4
		protected BehaviorComponent(Formation formation)
		{
			this.Formation = formation;
			this.PreserveExpireTime = 0f;
			this._navmeshlessTargetPenaltyTime = new Timer(Mission.Current.CurrentTime, 50f, true);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0001CAFA File Offset: 0x0001ACFA
		protected BehaviorComponent()
		{
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0001CB18 File Offset: 0x0001AD18
		private void InformSergeantPlayer()
		{
			if (Mission.Current.MainAgent != null && this.Formation.Team.GeneralAgent != null && !this.Formation.Team.IsPlayerGeneral && this.Formation.Team.IsPlayerSergeant && this.Formation.PlayerOwner == Agent.Main)
			{
				TextObject behaviorString = this.GetBehaviorString();
				MBTextManager.SetTextVariable("BEHAVIOR", behaviorString, false);
				MBTextManager.SetTextVariable("PLAYER_NAME", Mission.Current.MainAgent.NameTextObject, false);
				MBTextManager.SetTextVariable("TEAM_LEADER", this.Formation.Team.GeneralAgent.NameTextObject, false);
				MBInformationManager.AddQuickInformation(new TextObject("{=L91XKoMD}{TEAM_LEADER}: {PLAYER_NAME}, {BEHAVIOR}", null), 4000, this.Formation.Team.GeneralAgent.Character, null, "");
			}
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0001CC02 File Offset: 0x0001AE02
		protected virtual void OnBehaviorActivatedAux()
		{
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0001CC04 File Offset: 0x0001AE04
		internal void OnBehaviorActivated()
		{
			if (!this.Formation.Team.IsPlayerGeneral && !this.Formation.Team.IsPlayerSergeant && this.Formation.IsPlayerTroopInFormation && Mission.Current.MainAgent != null)
			{
				TextObject textObject = new TextObject(this.ToString().Replace("MBModule.Behavior", ""), null);
				MBTextManager.SetTextVariable("BEHAVIOUR_NAME_BEGIN", textObject, false);
				textObject = GameTexts.FindText("str_formation_ai_soldier_instruction_text", null);
				MBInformationManager.AddQuickInformation(textObject, 2000, Mission.Current.MainAgent.Character, null, "");
			}
			if (!GameNetwork.IsMultiplayer)
			{
				this.InformSergeantPlayer();
				this._lastPlayerInformTime = Mission.Current.CurrentTime;
			}
			if (this.Formation.IsAIControlled)
			{
				this.OnBehaviorActivatedAux();
			}
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0001CCD2 File Offset: 0x0001AED2
		public virtual void OnBehaviorCanceled()
		{
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0001CCD4 File Offset: 0x0001AED4
		public virtual void OnLostAIControl()
		{
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x0001CCD6 File Offset: 0x0001AED6
		public virtual void OnAgentRemoved(Agent agent)
		{
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0001CCD8 File Offset: 0x0001AED8
		public void RemindSergeantPlayer()
		{
			float currentTime = Mission.Current.CurrentTime;
			if (this == this.Formation.AI.ActiveBehavior && this._lastPlayerInformTime + 60f < currentTime)
			{
				this.InformSergeantPlayer();
				this._lastPlayerInformTime = currentTime;
			}
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0001CD1F File Offset: 0x0001AF1F
		public virtual void TickOccasionally()
		{
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x0001CD24 File Offset: 0x0001AF24
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x0001CDB0 File Offset: 0x0001AFB0
		public virtual float NavmeshlessTargetPositionPenalty
		{
			get
			{
				if (this._navmeshlessTargetPositionPenalty == 1f)
				{
					return 1f;
				}
				this._navmeshlessTargetPenaltyTime.Check(Mission.Current.CurrentTime);
				float num = this._navmeshlessTargetPenaltyTime.ElapsedTime();
				if (num >= 10f)
				{
					this._navmeshlessTargetPositionPenalty = 1f;
					return 1f;
				}
				if (num <= 5f)
				{
					return this._navmeshlessTargetPositionPenalty;
				}
				return MBMath.Lerp(this._navmeshlessTargetPositionPenalty, 1f, (num - 5f) / 5f, 1E-05f);
			}
			set
			{
				this._navmeshlessTargetPenaltyTime.Reset(Mission.Current.CurrentTime);
				this._navmeshlessTargetPositionPenalty = value;
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0001CDCE File Offset: 0x0001AFCE
		public float GetAIWeight()
		{
			return this.GetAiWeight() * this.NavmeshlessTargetPositionPenalty;
		}

		// Token: 0x06000DCC RID: 3532
		protected abstract float GetAiWeight();

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x0001CDDD File Offset: 0x0001AFDD
		// (set) Token: 0x06000DCE RID: 3534 RVA: 0x0001CDE5 File Offset: 0x0001AFE5
		public MovementOrder CurrentOrder
		{
			get
			{
				return this._currentOrder;
			}
			protected set
			{
				this._currentOrder = value;
				this.IsCurrentOrderChanged = true;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x0001CDF5 File Offset: 0x0001AFF5
		// (set) Token: 0x06000DD0 RID: 3536 RVA: 0x0001CDFD File Offset: 0x0001AFFD
		public float PreserveExpireTime { get; set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x0001CE06 File Offset: 0x0001B006
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x0001CE0E File Offset: 0x0001B00E
		public float WeightFactor { get; set; }

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0001CE17 File Offset: 0x0001B017
		public virtual void ResetBehavior()
		{
			this.WeightFactor = 0f;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0001CE24 File Offset: 0x0001B024
		public virtual TextObject GetBehaviorString()
		{
			string name = base.GetType().Name;
			return GameTexts.FindText("str_formation_ai_sergeant_instruction_behavior_text", name);
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0001CE48 File Offset: 0x0001B048
		public virtual void OnValidBehaviorSideChanged()
		{
			this._behaviorSide = this.Formation.AI.Side;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0001CE60 File Offset: 0x0001B060
		protected virtual void CalculateCurrentOrder()
		{
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0001CE64 File Offset: 0x0001B064
		public void PrecalculateMovementOrder()
		{
			this.CalculateCurrentOrder();
			this.CurrentOrder.GetPosition(this.Formation);
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0001CE8C File Offset: 0x0001B08C
		public override bool Equals(object obj)
		{
			return base.GetType() == obj.GetType();
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0001CE9F File Offset: 0x0001B09F
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0001CEA7 File Offset: 0x0001B0A7
		public virtual void OnDeploymentFinished()
		{
		}

		// Token: 0x04000334 RID: 820
		protected FormationAI.BehaviorSide _behaviorSide;

		// Token: 0x04000335 RID: 821
		protected const float FormArrangementDistanceToOrderPosition = 10f;

		// Token: 0x04000336 RID: 822
		private const float _playerInformCooldown = 60f;

		// Token: 0x04000337 RID: 823
		protected float _lastPlayerInformTime;

		// Token: 0x04000338 RID: 824
		private Timer _navmeshlessTargetPenaltyTime;

		// Token: 0x04000339 RID: 825
		private float _navmeshlessTargetPositionPenalty = 1f;

		// Token: 0x0400033A RID: 826
		public bool IsCurrentOrderChanged;

		// Token: 0x0400033B RID: 827
		private MovementOrder _currentOrder;

		// Token: 0x0400033C RID: 828
		protected FacingOrder CurrentFacingOrder = FacingOrder.FacingOrderLookAtEnemy;
	}
}
