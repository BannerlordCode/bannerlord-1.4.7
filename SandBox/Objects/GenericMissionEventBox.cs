using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.Objects
{
	// Token: 0x02000037 RID: 55
	public class GenericMissionEventBox : VolumeBox
	{
		// Token: 0x060001F2 RID: 498 RVA: 0x0000C9F8 File Offset: 0x0000ABF8
		protected override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(ScriptComponentBehavior.TickRequirement.Tick);
			using (IEnumerator<ScriptComponentBehavior> enumerator = base.GameEntity.GetScriptComponents().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GenericMissionEventScript genericMissionEventScript;
					if ((genericMissionEventScript = enumerator.Current as GenericMissionEventScript) != null)
					{
						this._genericMissionEvents.Add(genericMissionEventScript);
					}
				}
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000CA68 File Offset: 0x0000AC68
		protected override void OnTick(float dt)
		{
			bool flag = true;
			using (List<GenericMissionEventScript>.Enumerator enumerator = this._genericMissionEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsDisabled)
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				bool flag2 = false;
				foreach (Agent agent in Mission.Current.Agents)
				{
					if (agent.AgentVisuals.IsValid() && agent.AgentVisuals.GetEntity().Tags.Any<string>((string x) => !string.IsNullOrEmpty(x) && this.ActivatorAgentTags.Contains(x)) && base.IsPointIn(agent.Position))
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					foreach (GenericMissionEventScript genericMissionEventScript in this._genericMissionEvents)
					{
						if (!genericMissionEventScript.IsDisabled)
						{
							Game.Current.EventManager.TriggerEvent<GenericMissionEvent>(new GenericMissionEvent(genericMissionEventScript.EventId, genericMissionEventScript.Parameter));
						}
					}
				}
			}
		}

		// Token: 0x040000B8 RID: 184
		public string ActivatorAgentTags;

		// Token: 0x040000B9 RID: 185
		private List<GenericMissionEventScript> _genericMissionEvents = new List<GenericMissionEventScript>();
	}
}
