using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x02000176 RID: 374
	public class EncyclopediaManager
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06001B59 RID: 7001 RVA: 0x0008DA04 File Offset: 0x0008BC04
		// (set) Token: 0x06001B5A RID: 7002 RVA: 0x0008DA0C File Offset: 0x0008BC0C
		public IViewDataTracker ViewDataTracker { get; private set; }

		// Token: 0x06001B5B RID: 7003 RVA: 0x0008DA18 File Offset: 0x0008BC18
		public void CreateEncyclopediaPages()
		{
			this._pages = new Dictionary<Type, EncyclopediaPage>();
			this.ViewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			List<Type> list = new List<Type>();
			List<Assembly> list2 = new List<Assembly>();
			Assembly assembly = typeof(EncyclopediaModelBase).Assembly;
			list2.Add(assembly);
			foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
			{
				AssemblyName[] referencedAssemblies = assembly2.GetReferencedAssemblies();
				for (int j = 0; j < referencedAssemblies.Length; j++)
				{
					if (referencedAssemblies[j].ToString() == assembly.GetName().ToString())
					{
						list2.Add(assembly2);
						break;
					}
				}
			}
			foreach (Assembly assembly3 in list2)
			{
				list.AddRange(assembly3.GetTypesSafe(null));
			}
			foreach (Type type in list)
			{
				if (typeof(EncyclopediaPage).IsAssignableFrom(type))
				{
					object[] array = type.GetCustomAttributesSafe(typeof(OverrideEncyclopediaModel), false);
					for (int i = 0; i < array.Length; i++)
					{
						OverrideEncyclopediaModel overrideEncyclopediaModel = array[i] as OverrideEncyclopediaModel;
						if (overrideEncyclopediaModel != null)
						{
							EncyclopediaPage encyclopediaPage = Activator.CreateInstance(type) as EncyclopediaPage;
							foreach (Type type2 in overrideEncyclopediaModel.PageTargetTypes)
							{
								this._pages.Add(type2, encyclopediaPage);
							}
						}
					}
				}
			}
			foreach (Type type3 in list)
			{
				if (typeof(EncyclopediaPage).IsAssignableFrom(type3))
				{
					object[] array = type3.GetCustomAttributesSafe(typeof(EncyclopediaModel), false);
					for (int i = 0; i < array.Length; i++)
					{
						EncyclopediaModel encyclopediaModel = array[i] as EncyclopediaModel;
						if (encyclopediaModel != null)
						{
							EncyclopediaPage encyclopediaPage2 = Activator.CreateInstance(type3) as EncyclopediaPage;
							foreach (Type type4 in encyclopediaModel.PageTargetTypes)
							{
								if (!this._pages.ContainsKey(type4))
								{
									this._pages.Add(type4, encyclopediaPage2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x0008DCB4 File Offset: 0x0008BEB4
		public IEnumerable<EncyclopediaPage> GetEncyclopediaPages()
		{
			return this._pages.Values.Distinct<EncyclopediaPage>();
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x0008DCC6 File Offset: 0x0008BEC6
		public EncyclopediaPage GetPageOf(Type type)
		{
			return this._pages[type];
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x0008DCD4 File Offset: 0x0008BED4
		public string GetIdentifier(Type type)
		{
			return this._pages[type].GetIdentifier(type);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0008DCE8 File Offset: 0x0008BEE8
		public void GoToLink(string pageType, string stringID)
		{
			if (this._executeLink == null || string.IsNullOrEmpty(pageType))
			{
				return;
			}
			if (pageType == "Home" || pageType == "LastPage")
			{
				this._executeLink(pageType, null);
				return;
			}
			if (pageType == "ListPage")
			{
				EncyclopediaPage encyclopediaPage = Campaign.Current.EncyclopediaManager.GetEncyclopediaPages().FirstOrDefault<EncyclopediaPage>((EncyclopediaPage e) => e.HasIdentifier(stringID));
				this._executeLink(pageType, encyclopediaPage);
				return;
			}
			EncyclopediaPage encyclopediaPage2 = Campaign.Current.EncyclopediaManager.GetEncyclopediaPages().FirstOrDefault<EncyclopediaPage>((EncyclopediaPage e) => e.HasIdentifier(pageType));
			MBObjectBase @object = encyclopediaPage2.GetObject(pageType, stringID);
			if (encyclopediaPage2 != null && encyclopediaPage2.IsValidEncyclopediaItem(@object))
			{
				this._executeLink(pageType, @object);
			}
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x0008DDF4 File Offset: 0x0008BFF4
		public void GoToLink(string link)
		{
			int num = link.IndexOf('-');
			if (num > 0)
			{
				string text = link.Substring(0, num);
				string text2 = link.Substring(num + 1);
				this.GoToLink(text, text2);
				return;
			}
			Debug.FailedAssert("Failed to resolve encyclopedia link: " + link, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\EncyclopediaManager.cs", "GoToLink", 165);
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x0008DE49 File Offset: 0x0008C049
		public void SetLinkCallback(Action<string, object> ExecuteLink)
		{
			this._executeLink = ExecuteLink;
		}

		// Token: 0x0400093D RID: 2365
		private Dictionary<Type, EncyclopediaPage> _pages;

		// Token: 0x0400093F RID: 2367
		public const string HOME_ID = "Home";

		// Token: 0x04000940 RID: 2368
		public const string LIST_PAGE_ID = "ListPage";

		// Token: 0x04000941 RID: 2369
		public const string LAST_PAGE_ID = "LastPage";

		// Token: 0x04000942 RID: 2370
		private Action<string, object> _executeLink;
	}
}
