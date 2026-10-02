using System;
using Helpers;
using SandBox.BoardGames.MissionLogics;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.BoardGames.AI
{
	// Token: 0x02000104 RID: 260
	public abstract class BoardGameAIBase
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x0005F30C File Offset: 0x0005D50C
		public BoardGameAIBase.AIState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x0005F316 File Offset: 0x0005D516
		// (set) Token: 0x06000D0B RID: 3339 RVA: 0x0005F31E File Offset: 0x0005D51E
		public Move RecentMoveCalculated { get; private set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x0005F327 File Offset: 0x0005D527
		public bool AbortRequested
		{
			get
			{
				return this.State == BoardGameAIBase.AIState.AbortRequested;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000D0D RID: 3341 RVA: 0x0005F332 File Offset: 0x0005D532
		// (set) Token: 0x06000D0E RID: 3342 RVA: 0x0005F33A File Offset: 0x0005D53A
		private protected BoardGameHelper.AIDifficulty Difficulty { protected get; private set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x0005F343 File Offset: 0x0005D543
		// (set) Token: 0x06000D10 RID: 3344 RVA: 0x0005F34B File Offset: 0x0005D54B
		private protected MissionBoardGameLogic BoardGameHandler { protected get; private set; }

		// Token: 0x06000D11 RID: 3345 RVA: 0x0005F354 File Offset: 0x0005D554
		protected BoardGameAIBase(BoardGameHelper.AIDifficulty difficulty, MissionBoardGameLogic boardGameHandler)
		{
			this._stateLock = new object();
			this.Difficulty = difficulty;
			this.BoardGameHandler = boardGameHandler;
			this.Initialize();
			this._aiTask = AsyncTask.CreateWithDelegate(new ManagedDelegate
			{
				Instance = new ManagedDelegate.DelegateDefinition(this.UpdateThinkingAboutMoveOnSeparateThread)
			}, true);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0005F3AB File Offset: 0x0005D5AB
		public virtual Move CalculatePreMovementStageMove()
		{
			Debug.FailedAssert("CalculatePreMovementStageMove is not implemented for " + this.BoardGameHandler.CurrentBoardGame, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\BoardGames\\AI\\BoardGameAIBase.cs", "CalculatePreMovementStageMove", 64);
			return Move.Invalid;
		}

		// Token: 0x06000D13 RID: 3347
		public abstract Move CalculateMovementStageMove();

		// Token: 0x06000D14 RID: 3348
		protected abstract void InitializeDifficulty();

		// Token: 0x06000D15 RID: 3349 RVA: 0x0005F3DD File Offset: 0x0005D5DD
		public virtual bool WantsToForfeit()
		{
			return false;
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0005F3E0 File Offset: 0x0005D5E0
		public virtual void OnSetGameOver()
		{
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				BoardGameAIBase.AIState state = this.State;
				if (state != BoardGameAIBase.AIState.ReadyToRun)
				{
					if (state == BoardGameAIBase.AIState.Running)
					{
						this._state = BoardGameAIBase.AIState.AbortRequested;
					}
				}
				else
				{
					this._state = BoardGameAIBase.AIState.AbortRequested;
				}
			}
			this._aiTask.Wait();
			this.Reset();
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0005F450 File Offset: 0x0005D650
		public virtual void Initialize()
		{
			this.Reset();
			this.InitializeDifficulty();
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0005F45E File Offset: 0x0005D65E
		public void SetDifficulty(BoardGameHelper.AIDifficulty difficulty)
		{
			this.Difficulty = difficulty;
			this.InitializeDifficulty();
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0005F46D File Offset: 0x0005D66D
		public float HowLongDidAIThinkAboutMove()
		{
			return this._aiDecisionTimer;
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0005F478 File Offset: 0x0005D678
		public void UpdateThinkingAboutMove(float dt)
		{
			this._aiDecisionTimer += dt;
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				if (this.State == BoardGameAIBase.AIState.NeedsToRun)
				{
					this._state = BoardGameAIBase.AIState.ReadyToRun;
					this._aiTask.Invoke();
				}
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0005F4DC File Offset: 0x0005D6DC
		private void UpdateThinkingAboutMoveOnSeparateThread()
		{
			if (this.BoardGameHandler.Board.InPreMovementStage)
			{
				this.CalculatePreMovementStageOnSeparateThread();
				return;
			}
			this.CalculateMovementStageMoveOnSeparateThread();
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0005F4FD File Offset: 0x0005D6FD
		public void ResetThinking()
		{
			this._aiDecisionTimer = 0f;
			this._state = BoardGameAIBase.AIState.NeedsToRun;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0005F513 File Offset: 0x0005D713
		public bool CanMakeMove()
		{
			return this.State == BoardGameAIBase.AIState.Done && this._aiDecisionTimer >= 1.5f;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0005F530 File Offset: 0x0005D730
		private void Reset()
		{
			this.RecentMoveCalculated = Move.Invalid;
			this.MayForfeit = true;
			this.ResetThinking();
			this.MaxDepth = 0;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0005F554 File Offset: 0x0005D754
		private void CalculatePreMovementStageOnSeparateThread()
		{
			if (this.OnBeginSeparateThread())
			{
				Move move = this.CalculatePreMovementStageMove();
				this.OnExitSeparateThread(move);
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0005F578 File Offset: 0x0005D778
		private void CalculateMovementStageMoveOnSeparateThread()
		{
			if (this.OnBeginSeparateThread())
			{
				Move move = this.CalculateMovementStageMove();
				this.OnExitSeparateThread(move);
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0005F59C File Offset: 0x0005D79C
		private bool OnBeginSeparateThread()
		{
			bool flag = false;
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				if (this.AbortRequested)
				{
					this._state = BoardGameAIBase.AIState.Aborted;
					flag = true;
				}
				else
				{
					this._state = BoardGameAIBase.AIState.Running;
				}
			}
			return !flag;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0005F5FC File Offset: 0x0005D7FC
		private void OnExitSeparateThread(Move calculatedMove)
		{
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				if (this.AbortRequested)
				{
					this._state = BoardGameAIBase.AIState.Aborted;
					this.RecentMoveCalculated = Move.Invalid;
				}
				else
				{
					this._state = BoardGameAIBase.AIState.Done;
					this.RecentMoveCalculated = calculatedMove;
				}
			}
		}

		// Token: 0x040005A9 RID: 1449
		private const float AIDecisionDuration = 1.5f;

		// Token: 0x040005AA RID: 1450
		protected bool MayForfeit;

		// Token: 0x040005AB RID: 1451
		protected int MaxDepth;

		// Token: 0x040005AC RID: 1452
		private float _aiDecisionTimer;

		// Token: 0x040005AD RID: 1453
		private readonly ITask _aiTask;

		// Token: 0x040005AE RID: 1454
		private readonly object _stateLock;

		// Token: 0x040005AF RID: 1455
		private volatile BoardGameAIBase.AIState _state;

		// Token: 0x0200022C RID: 556
		public enum AIState
		{
			// Token: 0x040009D2 RID: 2514
			NeedsToRun,
			// Token: 0x040009D3 RID: 2515
			ReadyToRun,
			// Token: 0x040009D4 RID: 2516
			Running,
			// Token: 0x040009D5 RID: 2517
			AbortRequested,
			// Token: 0x040009D6 RID: 2518
			Aborted,
			// Token: 0x040009D7 RID: 2519
			Done
		}
	}
}
