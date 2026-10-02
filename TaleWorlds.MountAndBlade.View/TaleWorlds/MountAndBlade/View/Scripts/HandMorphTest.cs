using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000065 RID: 101
	public class HandMorphTest : ScriptComponentBehavior
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x0001D688 File Offset: 0x0001B888
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x0001D690 File Offset: 0x0001B890
		public uint ClothColor1 { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003DA RID: 986 RVA: 0x0001D699 File Offset: 0x0001B899
		// (set) Token: 0x060003DB RID: 987 RVA: 0x0001D6A1 File Offset: 0x0001B8A1
		public uint ClothColor2 { get; private set; }

		// Token: 0x060003DC RID: 988 RVA: 0x0001D6AC File Offset: 0x0001B8AC
		protected override void OnInit()
		{
			base.OnInit();
			this.ClothColor1 = uint.MaxValue;
			this.ClothColor2 = uint.MaxValue;
			if (this._agentVisuals == null && !this._characterSpawned)
			{
				this.SpawnCharacter();
				this._characterSpawned = true;
			}
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			this._agentVisuals.GetVisuals().SetFrame(ref globalFrame);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001D70B File Offset: 0x0001B90B
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
			if (Game.Current == null)
			{
				this._editorGameManager = new EditorGameManager();
			}
			this.ClothColor1 = uint.MaxValue;
			this.ClothColor2 = uint.MaxValue;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001D734 File Offset: 0x0001B934
		protected override void OnEditorTick(float dt)
		{
			if (!this._isFinished && this._editorGameManager != null)
			{
				this._isFinished = !this._editorGameManager.DoLoadingForGameManager();
			}
			if (Game.Current != null && this._agentVisuals == null && !this._characterSpawned)
			{
				this.SpawnCharacter();
				this._characterSpawned = true;
			}
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			this._agentVisuals.GetVisuals().SetFrame(ref globalFrame);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001D7AC File Offset: 0x0001B9AC
		public void SpawnCharacter()
		{
			CharacterCode characterCode = CharacterCode.CreateFrom(MBObjectManager.Instance.GetObject<BasicCharacterObject>("facgen_template_test_char_0"));
			this.InitWithCharacter(characterCode);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0001D7D5 File Offset: 0x0001B9D5
		public void Reset()
		{
			AgentVisuals agentVisuals = this._agentVisuals;
			if (agentVisuals == null)
			{
				return;
			}
			agentVisuals.Reset();
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001D7E8 File Offset: 0x0001B9E8
		public void InitWithCharacter(CharacterCode characterCode)
		{
			this.Reset();
			MatrixFrame frame = base.GameEntity.GetFrame();
			frame.rotation.s.z = 0f;
			frame.rotation.f.z = 0f;
			frame.rotation.s.Normalize();
			frame.rotation.f.Normalize();
			frame.rotation.u = Vec3.CrossProduct(frame.rotation.s, frame.rotation.f);
			characterCode.BodyProperties = new BodyProperties(new DynamicBodyProperties(20f, 0f, 0f), characterCode.BodyProperties.StaticProperties);
			Monster baseMonsterFromRace = FaceGen.GetBaseMonsterFromRace(characterCode.Race);
			this._agentVisuals = AgentVisuals.Create(new AgentVisualsData().Equipment(characterCode.CalculateEquipment()).BodyProperties(characterCode.BodyProperties).Race(characterCode.Race)
				.SkeletonType(characterCode.IsFemale ? SkeletonType.Female : SkeletonType.Male)
				.ActionSet(MBGlobals.GetActionSetWithSuffix(baseMonsterFromRace, characterCode.IsFemale, "_facegen"))
				.ActionCode(in this.act_visual_test_morph_animation)
				.Scene(base.GameEntity.Scene)
				.Monster(baseMonsterFromRace)
				.PrepareImmediately(true)
				.UseMorphAnims(true)
				.ClothColor1(this.ClothColor1)
				.ClothColor2(this.ClothColor2)
				.Frame(frame), "HandMorphTest", false, false, false);
			this._agentVisuals.SetAction(in this.act_defend_up_fist_active, 1f, true);
			MatrixFrame matrixFrame = frame;
			this._agentVisuals.GetVisuals().GetSkeleton().TickAnimationsAndForceUpdate(1f, matrixFrame, true);
			this._agentVisuals.GetVisuals().SetFrame(ref matrixFrame);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001D9AA File Offset: 0x0001BBAA
		protected override void OnRemoved(int removeReason)
		{
			base.OnRemoved(removeReason);
			this._agentVisuals.Reset();
		}

		// Token: 0x04000241 RID: 577
		private const bool CreateFaceImmediately = true;

		// Token: 0x04000242 RID: 578
		private readonly ActionIndexCache act_defend_up_fist_active = ActionIndexCache.Create("act_defend_up_fist_active");

		// Token: 0x04000243 RID: 579
		private readonly ActionIndexCache act_visual_test_morph_animation = ActionIndexCache.Create("act_visual_test_morph_animation");

		// Token: 0x04000246 RID: 582
		private MBGameManager _editorGameManager;

		// Token: 0x04000247 RID: 583
		private bool _isFinished;

		// Token: 0x04000248 RID: 584
		private bool _characterSpawned;

		// Token: 0x04000249 RID: 585
		private AgentVisuals _agentVisuals;
	}
}
