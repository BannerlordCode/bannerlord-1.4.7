using System;
using System.Text.RegularExpressions;

namespace TaleWorlds.Library
{
	// Token: 0x0200009F RID: 159
	public class UniqueSceneId
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x0001390C File Offset: 0x00011B0C
		public string UniqueToken { get; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00013914 File Offset: 0x00011B14
		public string Revision { get; }

		// Token: 0x0600059C RID: 1436 RVA: 0x0001391C File Offset: 0x00011B1C
		public UniqueSceneId(string uniqueToken, string revision)
		{
			if (uniqueToken == null)
			{
				throw new ArgumentNullException("uniqueToken");
			}
			this.UniqueToken = uniqueToken;
			if (revision == null)
			{
				throw new ArgumentNullException("revision");
			}
			this.Revision = revision;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00013950 File Offset: 0x00011B50
		public string Serialize()
		{
			return string.Format(":ut[{0}]{1}:rev[{2}]{3}", new object[]
			{
				this.UniqueToken.Length,
				this.UniqueToken,
				this.Revision.Length,
				this.Revision
			});
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x000139A8 File Offset: 0x00011BA8
		public static bool TryParse(string uniqueMapId, out UniqueSceneId identifiers)
		{
			identifiers = null;
			if (uniqueMapId == null)
			{
				return false;
			}
			Match match = UniqueSceneId.IdentifierPattern.Value.Match(uniqueMapId);
			if (match.Success)
			{
				identifiers = new UniqueSceneId(match.Groups[1].Value, match.Groups[2].Value);
				return true;
			}
			return false;
		}

		// Token: 0x040001B5 RID: 437
		private static readonly Lazy<Regex> IdentifierPattern = new Lazy<Regex>(() => new Regex("^:ut\\[\\d+\\](.*):rev\\[\\d+\\](.*)$", RegexOptions.Compiled));
	}
}
