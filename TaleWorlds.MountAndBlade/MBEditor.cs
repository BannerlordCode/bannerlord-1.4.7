using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001C5 RID: 453
	public class MBEditor
	{
		// Token: 0x06001B3F RID: 6975 RVA: 0x0005F5F8 File Offset: 0x0005D7F8
		[MBCallback(null, false)]
		internal static void SetEditorScene(Scene scene)
		{
			if (MBEditor._editorScene != null)
			{
				if (MBEditor._agentRendererSceneController != null)
				{
					MBAgentRendererSceneController.DestructAgentRendererSceneController(MBEditor._editorScene, MBEditor._agentRendererSceneController, false);
				}
				MBEditor._editorScene.ClearAll();
			}
			MBEditor._editorScene = scene;
			MBEditor._agentRendererSceneController = MBAgentRendererSceneController.CreateNewAgentRendererSceneController(MBEditor._editorScene);
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x0005F648 File Offset: 0x0005D848
		[MBCallback(null, false)]
		internal static void CloseEditorScene()
		{
			if (MBEditor._agentRendererSceneController != null)
			{
				MBAgentRendererSceneController.DestructAgentRendererSceneController(MBEditor._editorScene, MBEditor._agentRendererSceneController, false);
			}
			MBEditor._agentRendererSceneController = null;
			MBEditor._editorScene = null;
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x0005F66D File Offset: 0x0005D86D
		[MBCallback(null, false)]
		internal static void DestroyEditor(Scene scene)
		{
			MBAgentRendererSceneController.DestructAgentRendererSceneController(MBEditor._editorScene, MBEditor._agentRendererSceneController, false);
			MBEditor._editorScene.ClearAll();
			MBEditor._editorScene = null;
			MBEditor._agentRendererSceneController = null;
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001B42 RID: 6978 RVA: 0x0005F695 File Offset: 0x0005D895
		public static bool IsEditModeOn
		{
			get
			{
				return MBAPI.IMBEditor.IsEditMode();
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001B43 RID: 6979 RVA: 0x0005F6A1 File Offset: 0x0005D8A1
		public static bool EditModeEnabled
		{
			get
			{
				return MBAPI.IMBEditor.IsEditModeEnabled();
			}
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x0005F6AD File Offset: 0x0005D8AD
		public static void UpdateSceneTree(bool doNextFrame)
		{
			MBAPI.IMBEditor.UpdateSceneTree(doNextFrame);
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x0005F6BA File Offset: 0x0005D8BA
		public static bool IsEntitySelected(GameEntity entity)
		{
			return MBAPI.IMBEditor.IsEntitySelected(entity.Pointer);
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x0005F6CC File Offset: 0x0005D8CC
		public static bool IsEntitySelected(WeakGameEntity entity)
		{
			return MBAPI.IMBEditor.IsEntitySelected(entity.Pointer);
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x0005F6DF File Offset: 0x0005D8DF
		public static void RenderEditorMesh(MetaMesh mesh, MatrixFrame frame)
		{
			MBAPI.IMBEditor.RenderEditorMesh(mesh.Pointer, ref frame);
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x0005F6F3 File Offset: 0x0005D8F3
		public static void ApplyDeltaToEditorCamera(Vec3 delta)
		{
			MBAPI.IMBEditor.ApplyDeltaToEditorCamera(in delta);
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x0005F701 File Offset: 0x0005D901
		public static void EnterEditMode(SceneView sceneView, MatrixFrame initialCameraFrame, float initialCameraElevation, float initialCameraBearing)
		{
			MBAPI.IMBEditor.EnterEditMode(sceneView.Pointer, ref initialCameraFrame, initialCameraElevation, initialCameraBearing);
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x0005F717 File Offset: 0x0005D917
		public static void TickEditMode(float dt)
		{
			MBAPI.IMBEditor.TickEditMode(dt);
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x0005F724 File Offset: 0x0005D924
		public static void LeaveEditMode()
		{
			MBAPI.IMBEditor.LeaveEditMode();
			MBAgentRendererSceneController.DestructAgentRendererSceneController(MBEditor._editorScene, MBEditor._agentRendererSceneController, false);
			MBEditor._agentRendererSceneController = null;
			MBEditor._editorScene = null;
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x0005F74C File Offset: 0x0005D94C
		public static void EnterEditMissionMode(Mission mission)
		{
			MBAPI.IMBEditor.EnterEditMissionMode(mission.Pointer);
			MBEditor._isEditorMissionOn = true;
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x0005F764 File Offset: 0x0005D964
		public static void LeaveEditMissionMode()
		{
			MBAPI.IMBEditor.LeaveEditMissionMode();
			MBEditor._isEditorMissionOn = false;
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x0005F776 File Offset: 0x0005D976
		public static bool IsEditorMissionOn()
		{
			return MBEditor._isEditorMissionOn && MBEditor.IsEditModeOn;
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0005F788 File Offset: 0x0005D988
		public static void ActivateSceneEditorPresentation()
		{
			Monster.GetBoneIndexWithId = new Func<string, string, sbyte>(MBActionSet.GetBoneIndexWithId);
			Monster.GetBoneHasParentBone = new Func<string, sbyte, bool>(MBActionSet.GetBoneHasParentBone);
			MBObjectManager.Init();
			MBObjectManager.Instance.RegisterType<Monster>("Monster", "Monsters", 2U, true, false);
			MBObjectManager.Instance.LoadXML("Monsters", true);
			MBAPI.IMBEditor.ActivateSceneEditorPresentation();
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0005F7EE File Offset: 0x0005D9EE
		public static void DeactivateSceneEditorPresentation()
		{
			MBAPI.IMBEditor.DeactivateSceneEditorPresentation();
			MBObjectManager.Instance.Destroy();
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x0005F804 File Offset: 0x0005DA04
		public static void TickSceneEditorPresentation(float dt)
		{
			MBAPI.IMBEditor.TickSceneEditorPresentation(dt);
			LoadingWindow.DisableGlobalLoadingWindow();
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x0005F816 File Offset: 0x0005DA16
		public static SceneView GetEditorSceneView()
		{
			return MBAPI.IMBEditor.GetEditorSceneView();
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x0005F822 File Offset: 0x0005DA22
		public static bool HelpersEnabled()
		{
			return MBAPI.IMBEditor.HelpersEnabled();
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x0005F82E File Offset: 0x0005DA2E
		public static bool BorderHelpersEnabled()
		{
			return MBAPI.IMBEditor.BorderHelpersEnabled();
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x0005F83A File Offset: 0x0005DA3A
		public static void ZoomToPosition(Vec3 pos)
		{
			MBAPI.IMBEditor.ZoomToPosition(pos);
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x0005F847 File Offset: 0x0005DA47
		public static bool IsReplayManagerReplaying()
		{
			return MBAPI.IMBEditor.IsReplayManagerReplaying();
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0005F853 File Offset: 0x0005DA53
		public static bool IsReplayManagerRendering()
		{
			return MBAPI.IMBEditor.IsReplayManagerRendering();
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x0005F85F File Offset: 0x0005DA5F
		public static bool IsReplayManagerRecording()
		{
			return MBAPI.IMBEditor.IsReplayManagerRecording();
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x0005F86B File Offset: 0x0005DA6B
		public static void AddEditorWarning(string msg)
		{
			MBAPI.IMBEditor.AddEditorWarning(msg);
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x0005F878 File Offset: 0x0005DA78
		public static void AddEntityWarning(WeakGameEntity entityId, string msg)
		{
			MBAPI.IMBEditor.AddEntityWarning(entityId.Pointer, msg);
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x0005F88C File Offset: 0x0005DA8C
		public static void AddNavMeshWarning(Scene scene, PathFaceRecord record, string msg)
		{
			MBAPI.IMBEditor.AddNavMeshWarning(scene.Pointer, in record, msg);
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x0005F8A1 File Offset: 0x0005DAA1
		public static string GetAllPrefabsAndChildWithTag(string tag)
		{
			return MBAPI.IMBEditor.GetAllPrefabsAndChildWithTag(tag);
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x0005F8AE File Offset: 0x0005DAAE
		public static void ExitEditMode()
		{
			MBAPI.IMBEditor.ExitEditMode();
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x0005F8BC File Offset: 0x0005DABC
		public static void SetUpgradeLevelVisibility(List<string> levels)
		{
			string text = "";
			for (int i = 0; i < levels.Count - 1; i++)
			{
				text = text + levels[i] + "|";
			}
			text += levels[levels.Count - 1];
			MBAPI.IMBEditor.SetUpgradeLevelVisibility(text);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0005F915 File Offset: 0x0005DB15
		public static void SetLevelVisibility(List<string> levels)
		{
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x0005F917 File Offset: 0x0005DB17
		public static void ToggleEnableEditorPhysics()
		{
			MBAPI.IMBEditor.ToggleEnableEditorPhysics();
		}

		// Token: 0x040008FF RID: 2303
		public static Scene _editorScene;

		// Token: 0x04000900 RID: 2304
		private static MBAgentRendererSceneController _agentRendererSceneController;

		// Token: 0x04000901 RID: 2305
		public static bool _isEditorMissionOn;
	}
}
