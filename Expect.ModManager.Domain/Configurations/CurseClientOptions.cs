namespace Expect.ModManager.Domain.Configurations
{
	public class CurseClientOptions
	{
		public string ApiKey { get; set; }
        public Endpoints Endpoints { get; set; }
	}

    public class Endpoints
    {
        public string SearchMods {get;set;}
        public string GetMod {get;set;}
        public string GetMods {get;set;}
        public string GetModDescription {get;set;}
        public string GetCategories {get;set;}
        public string GetMinecraftVersions {get;set;}
        public string GetMinecraftModLoaders {get;set;}
        public string GetModFile {get;set;}
        public string GetModFiles {get;set;}
        public string GetFiles {get;set;}
        public string GetModFileDownloadUrl {get;set;}
	}
}
