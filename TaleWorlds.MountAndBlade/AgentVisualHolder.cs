using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002C9 RID: 713
	public class AgentVisualHolder : IAgentVisual
	{
		// Token: 0x0600292D RID: 10541 RVA: 0x0009ACFC File Offset: 0x00098EFC
		public AgentVisualHolder(MatrixFrame frame, Equipment equipment, string name, BodyProperties bodyProperties)
		{
			this.SetFrame(ref frame);
			this._equipment = equipment;
			this._characterObjectStringID = name;
			this._bodyProperties = bodyProperties;
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x0009AD22 File Offset: 0x00098F22
		public void SetAction(in ActionIndexCache actionName, float startProgress = 0f, bool forceFaceMorphRestart = true)
		{
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x0009AD24 File Offset: 0x00098F24
		public GameEntity GetEntity()
		{
			return null;
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x0009AD27 File Offset: 0x00098F27
		public MBAgentVisuals GetVisuals()
		{
			return null;
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x0009AD2A File Offset: 0x00098F2A
		public void SetFrame(ref MatrixFrame frame)
		{
			this._frame = frame;
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x0009AD38 File Offset: 0x00098F38
		public MatrixFrame GetFrame()
		{
			return this._frame;
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x0009AD40 File Offset: 0x00098F40
		public BodyProperties GetBodyProperties()
		{
			return this._bodyProperties;
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x0009AD48 File Offset: 0x00098F48
		public void SetBodyProperties(BodyProperties bodyProperties)
		{
			this._bodyProperties = bodyProperties;
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x0009AD51 File Offset: 0x00098F51
		public bool GetIsFemale()
		{
			return false;
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x0009AD54 File Offset: 0x00098F54
		public string GetCharacterObjectID()
		{
			return this._characterObjectStringID;
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x0009AD5C File Offset: 0x00098F5C
		public void SetCharacterObjectID(string id)
		{
			this._characterObjectStringID = id;
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x0009AD65 File Offset: 0x00098F65
		public Equipment GetEquipment()
		{
			return this._equipment;
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x0009AD6D File Offset: 0x00098F6D
		public void RefreshWithNewEquipment(Equipment equipment)
		{
			this._equipment = equipment;
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x0009AD76 File Offset: 0x00098F76
		public void SetClothingColors(uint color1, uint color2)
		{
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x0009AD78 File Offset: 0x00098F78
		public void GetClothingColors(out uint color1, out uint color2)
		{
			color1 = uint.MaxValue;
			color2 = uint.MaxValue;
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x0009AD80 File Offset: 0x00098F80
		public AgentVisualsData GetCopyAgentVisualsData()
		{
			return null;
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x0009AD83 File Offset: 0x00098F83
		public void Refresh(bool needBatchedVersionForWeaponMeshes, AgentVisualsData data, bool forceUseFaceCache = false)
		{
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x0009AD85 File Offset: 0x00098F85
		void IAgentVisual.SetAction(in ActionIndexCache actionName, float startProgress, bool forceFaceMorphRestart)
		{
			this.SetAction(in actionName, startProgress, forceFaceMorphRestart);
		}

		// Token: 0x04000FC8 RID: 4040
		private MatrixFrame _frame;

		// Token: 0x04000FC9 RID: 4041
		private Equipment _equipment;

		// Token: 0x04000FCA RID: 4042
		private string _characterObjectStringID;

		// Token: 0x04000FCB RID: 4043
		private BodyProperties _bodyProperties;
	}
}
