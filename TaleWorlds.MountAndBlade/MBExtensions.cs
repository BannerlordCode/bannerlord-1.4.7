using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200013F RID: 319
	public static class MBExtensions
	{
		// Token: 0x06000FD0 RID: 4048 RVA: 0x0002C070 File Offset: 0x0002A270
		private static Vec2 GetGlobalOrganicDirectionAux(ColumnFormation columnFormation, int depthCount = -1)
		{
			IEnumerable<Agent> unitsAtVanguardFile = columnFormation.GetUnitsAtVanguardFile<Agent>();
			Vec2 vec = Vec2.Zero;
			int num = 0;
			Agent agent = null;
			foreach (Agent agent2 in unitsAtVanguardFile)
			{
				if (agent != null)
				{
					Vec2 vec2 = (agent.Position - agent2.Position).AsVec2.Normalized();
					vec += vec2;
					num++;
				}
				agent = agent2;
				if (depthCount > 0 && num >= depthCount)
				{
					break;
				}
			}
			if (num == 0)
			{
				return Vec2.Invalid;
			}
			return vec * (1f / (float)num);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0002C11C File Offset: 0x0002A31C
		public static Vec2 GetGlobalOrganicDirection(this ColumnFormation columnFormation)
		{
			return MBExtensions.GetGlobalOrganicDirectionAux(columnFormation, -1);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0002C125 File Offset: 0x0002A325
		public static Vec2 GetGlobalHeadDirection(this ColumnFormation columnFormation)
		{
			return MBExtensions.GetGlobalOrganicDirectionAux(columnFormation, 3);
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0002C12E File Offset: 0x0002A32E
		public static IEnumerable<T> FindAllWithType<T>(this IEnumerable<GameEntity> entities) where T : ScriptComponentBehavior
		{
			return entities.SelectMany<GameEntity, T>((GameEntity e) => e.GetScriptComponents<T>());
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x0002C158 File Offset: 0x0002A358
		public static IEnumerable<T> FindAllWithType<T>(this IEnumerable<MissionObject> missionObjects) where T : MissionObject
		{
			return from e in missionObjects
				where e != null && e is T
				select e as T;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x0002C1B0 File Offset: 0x0002A3B0
		public static List<GameEntity> FindAllWithCompatibleType(this IEnumerable<GameEntity> sceneProps, params Type[] types)
		{
			List<GameEntity> list = new List<GameEntity>();
			foreach (GameEntity gameEntity in sceneProps)
			{
				foreach (ScriptComponentBehavior scriptComponentBehavior in gameEntity.GetScriptComponents())
				{
					Type type = scriptComponentBehavior.GetType();
					for (int i = 0; i < types.Length; i++)
					{
						if (types[i].IsAssignableFrom(type))
						{
							list.Add(gameEntity);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0002C260 File Offset: 0x0002A460
		public static List<MissionObject> FindAllWithCompatibleType(this IEnumerable<MissionObject> missionObjects, params Type[] types)
		{
			List<MissionObject> list = new List<MissionObject>();
			foreach (MissionObject missionObject in missionObjects)
			{
				if (missionObject != null)
				{
					Type type = missionObject.GetType();
					for (int i = 0; i < types.Length; i++)
					{
						if (types[i].IsAssignableFrom(type))
						{
							list.Add(missionObject);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0002C2DC File Offset: 0x0002A4DC
		private static void CollectScriptComponentsIncludingChildrenAux<T>(GameEntity entity, MBList<T> list) where T : ScriptComponentBehavior
		{
			IEnumerable<T> scriptComponents = entity.GetScriptComponents<T>();
			list.AddRange(scriptComponents);
			foreach (GameEntity gameEntity in entity.GetChildren())
			{
				MBExtensions.CollectScriptComponentsIncludingChildrenAux<T>(gameEntity, list);
			}
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0002C338 File Offset: 0x0002A538
		private static void CollectScriptComponentsIncludingChildrenAux<T>(WeakGameEntity entity, MBList<T> list) where T : ScriptComponentBehavior
		{
			list.AddRange(entity.GetScriptComponents<T>());
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				MBExtensions.CollectScriptComponentsIncludingChildrenAux<T>(weakGameEntity, list);
			}
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0002C394 File Offset: 0x0002A594
		public static MBList<T> CollectScriptComponentsIncludingChildrenRecursive<T>(this GameEntity entity) where T : ScriptComponentBehavior
		{
			MBList<T> mblist = new MBList<T>();
			MBExtensions.CollectScriptComponentsIncludingChildrenAux<T>(entity, mblist);
			return mblist;
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0002C3B0 File Offset: 0x0002A5B0
		public static MBList<T> CollectScriptComponentsIncludingChildrenRecursive<T>(this WeakGameEntity entity) where T : ScriptComponentBehavior
		{
			MBList<T> mblist = new MBList<T>();
			MBExtensions.CollectScriptComponentsIncludingChildrenAux<T>(entity, mblist);
			return mblist;
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0002C3CC File Offset: 0x0002A5CC
		public static List<T> CollectScriptComponentsWithTagIncludingChildrenRecursive<T>(this GameEntity entity, string tag) where T : ScriptComponentBehavior
		{
			List<T> list = new List<T>();
			foreach (GameEntity gameEntity in entity.GetChildren())
			{
				if (gameEntity.HasTag(tag))
				{
					IEnumerable<T> scriptComponents = gameEntity.GetScriptComponents<T>();
					list.AddRange(scriptComponents);
				}
				if (gameEntity.ChildCount > 0)
				{
					list.AddRange(gameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<T>(tag));
				}
			}
			return list;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0002C448 File Offset: 0x0002A648
		public static List<T> CollectScriptComponentsWithTagIncludingChildrenRecursive<T>(this WeakGameEntity entity, string tag) where T : ScriptComponentBehavior
		{
			List<T> list = new List<T>();
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				if (weakGameEntity.HasTag(tag))
				{
					list.AddRange(weakGameEntity.GetScriptComponents<T>());
				}
				if (weakGameEntity.ChildCount > 0)
				{
					list.AddRange(weakGameEntity.CollectScriptComponentsWithTagIncludingChildrenRecursive<T>(tag));
				}
			}
			return list;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0002C4C4 File Offset: 0x0002A6C4
		public static List<GameEntity> CollectChildrenEntitiesWithTag(this GameEntity entity, string tag)
		{
			List<GameEntity> list = new List<GameEntity>();
			foreach (GameEntity gameEntity in entity.GetChildren())
			{
				if (gameEntity.HasTag(tag))
				{
					list.Add(gameEntity);
				}
				if (gameEntity.ChildCount > 0)
				{
					list.AddRange(gameEntity.CollectChildrenEntitiesWithTag(tag));
				}
			}
			return list;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0002C538 File Offset: 0x0002A738
		public static List<WeakGameEntity> CollectChildrenEntitiesWithTag(this WeakGameEntity entity, string tag)
		{
			List<WeakGameEntity> list = new List<WeakGameEntity>();
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				if (weakGameEntity.HasTag(tag))
				{
					list.Add(weakGameEntity);
				}
				if (weakGameEntity.ChildCount > 0)
				{
					list.AddRange(weakGameEntity.CollectChildrenEntitiesWithTag(tag));
				}
			}
			return list;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0002C5B0 File Offset: 0x0002A7B0
		public static WeakGameEntity GetFirstChildEntityWithName(this WeakGameEntity entity, string name)
		{
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				if (weakGameEntity.Name == name)
				{
					return weakGameEntity;
				}
			}
			return WeakGameEntity.Invalid;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0002C614 File Offset: 0x0002A814
		public static T GetFirstScriptInFamilyDescending<T>(this GameEntity entity) where T : ScriptComponentBehavior
		{
			T t = entity.GetFirstScriptOfType<T>();
			if (t != null)
			{
				return t;
			}
			foreach (GameEntity gameEntity in entity.GetChildren())
			{
				t = gameEntity.GetFirstScriptInFamilyDescending<T>();
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0002C688 File Offset: 0x0002A888
		public static T GetFirstScriptInFamilyDescending<T>(this WeakGameEntity entity) where T : ScriptComponentBehavior
		{
			T t = entity.GetFirstScriptOfType<T>();
			if (t != null)
			{
				return t;
			}
			foreach (WeakGameEntity weakGameEntity in entity.GetChildren())
			{
				t = weakGameEntity.GetFirstScriptInFamilyDescending<T>();
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0002C700 File Offset: 0x0002A900
		public static TSource ElementAtOrValue<TSource>(this IEnumerable<TSource> source, int index, TSource value)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (index >= 0)
			{
				IList<TSource> list = source as IList<TSource>;
				if (list == null)
				{
					using (IEnumerator<TSource> enumerator = source.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (index == 0)
							{
								return enumerator.Current;
							}
							index--;
						}
					}
					return value;
				}
				if (index < list.Count)
				{
					return list[index];
				}
			}
			return value;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0002C77C File Offset: 0x0002A97C
		public static bool IsOpponentOf(this BattleSideEnum s, BattleSideEnum side)
		{
			return (s == BattleSideEnum.Attacker && side == BattleSideEnum.Defender) || (s == BattleSideEnum.Defender && side == BattleSideEnum.Attacker);
		}
	}
}
