using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox
{
	// Token: 0x020000DB RID: 219
	public class MultiplayerItemTestMissionController : MissionLogic
	{
		// Token: 0x060008F1 RID: 2289 RVA: 0x0000F0B4 File Offset: 0x0000D2B4
		public MultiplayerItemTestMissionController(BasicCultureObject culture)
		{
			this._culture = culture;
			if (!MultiplayerItemTestMissionController._initializeFlag)
			{
				Game.Current.ObjectManager.LoadXML("MPCharacters", false);
				MultiplayerItemTestMissionController._initializeFlag = true;
			}
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0000F125 File Offset: 0x0000D325
		public override void AfterStart()
		{
			this.GetAllTroops();
			this.SpawnMainAgent();
			this.SpawnMultiplayerTroops();
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0000F13C File Offset: 0x0000D33C
		private void SpawnMultiplayerTroops()
		{
			foreach (BasicCharacterObject basicCharacterObject in this._troops)
			{
				Vec3 vec;
				Vec2 vec2;
				this.GetNextSpawnFrame(out vec, out vec2);
				foreach (Equipment equipment in basicCharacterObject.BattleEquipments)
				{
					base.Mission.SpawnAgent(new AgentBuildData(new BasicBattleAgentOrigin(basicCharacterObject)).Equipment(equipment).InitialPosition(in vec).InitialDirection(in vec2), false);
					vec += new Vec3(0f, 2f, 0f, -1f);
				}
				foreach (Equipment equipment2 in basicCharacterObject.CivilianEquipments)
				{
					base.Mission.SpawnAgent(new AgentBuildData(new BasicBattleAgentOrigin(basicCharacterObject)).Equipment(equipment2).InitialPosition(in vec).InitialDirection(in vec2), false);
					vec += new Vec3(0f, 2f, 0f, -1f);
				}
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0000F2CC File Offset: 0x0000D4CC
		private void GetNextSpawnFrame(out Vec3 position, out Vec2 direction)
		{
			this._coordinate += new Vec3(3f, 0f, 0f, -1f);
			if (this._coordinate.x > (float)this._mapHorizontalEndCoordinate)
			{
				this._coordinate.x = 3f;
				this._coordinate.y = this._coordinate.y + 3f;
			}
			position = this._coordinate;
			direction = new Vec2(0f, -1f);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0000F35C File Offset: 0x0000D55C
		private XmlDocument LoadXmlFile(string path)
		{
			Debug.Print("opening " + path, 0, Debug.DebugColor.White, 17592186044416UL);
			XmlDocument xmlDocument = new XmlDocument();
			string text = new StreamReader(path).ReadToEnd();
			xmlDocument.LoadXml(text);
			return xmlDocument;
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0000F3A0 File Offset: 0x0000D5A0
		private void SpawnMainAgent()
		{
			if (this.mainAgent == null || this.mainAgent.State != AgentState.Active)
			{
				BasicCharacterObject @object = Game.Current.ObjectManager.GetObject<BasicCharacterObject>("main_hero");
				Mission mission = base.Mission;
				AgentBuildData agentBuildData = new AgentBuildData(new BasicBattleAgentOrigin(@object)).Team(base.Mission.DefenderTeam);
				Vec3 vec = new Vec3(200f + (float)MBRandom.RandomInt(15), 200f + (float)MBRandom.RandomInt(15), 1f, -1f);
				this.mainAgent = mission.SpawnAgent(agentBuildData.InitialPosition(in vec).InitialDirection(in Vec2.Forward).Controller(AgentControllerType.Player), false);
			}
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0000F44C File Offset: 0x0000D64C
		private void GetAllTroops()
		{
			foreach (object obj in this.LoadXmlFile(BasePath.Name + "/Modules/Native/ModuleData/mpcharacters.xml").DocumentElement.SelectNodes("NPCCharacter"))
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlAttributeCollection attributes = xmlNode.Attributes;
				if (((attributes != null) ? attributes["occupation"] : null) != null && xmlNode.Attributes["occupation"].InnerText == "Soldier")
				{
					string innerText = xmlNode.Attributes["id"].InnerText;
					BasicCharacterObject @object = Game.Current.ObjectManager.GetObject<BasicCharacterObject>(innerText);
					if (@object != null && @object.Culture == this._culture)
					{
						this._troops.Add(@object);
					}
				}
			}
		}

		// Token: 0x0400020E RID: 526
		private Agent mainAgent;

		// Token: 0x0400020F RID: 527
		private BasicCultureObject _culture;

		// Token: 0x04000210 RID: 528
		private List<BasicCharacterObject> _troops = new List<BasicCharacterObject>();

		// Token: 0x04000211 RID: 529
		private const float HorizontalGap = 3f;

		// Token: 0x04000212 RID: 530
		private const float VerticalGap = 3f;

		// Token: 0x04000213 RID: 531
		private Vec3 _coordinate = new Vec3(200f, 200f, 0f, -1f);

		// Token: 0x04000214 RID: 532
		private int _mapHorizontalEndCoordinate = 800;

		// Token: 0x04000215 RID: 533
		private static bool _initializeFlag;
	}
}
