namespace WutheringWavesFrameUnlocker___WWFU
{
	public class FileCheckResult
	{
		public bool IsFileExist { get; private set; }
		public string Message { get; private set; }
		public string FilePath { get; private set; }
		public string FolderPath { get; private set; }

		public FileCheckResult()
		{
			this.IsFileExist = false;
			this.Message = "File does not exist.";
			this.FilePath = string.Empty;
			this.FolderPath = string.Empty;
		}

		public FileCheckResult(string fileType, string fileLocation) : this()
		{
			bool CHECKK = File.Exists(fileLocation);
			this.IsFileExist = CHECKK;
			this.Message = string.Format(@"{0} {1}", fileType, CHECKK ? "File exists." : "File does not exist.");
			this.FilePath = CHECKK ? fileLocation : string.Empty;
			this.FolderPath = CHECKK ? this.GetDirectoryName(fileLocation) : string.Empty;
		}

		public FileCheckResult(string fileLocation) : this()
		{
			bool CHECKK = File.Exists(fileLocation);
			this.IsFileExist = CHECKK;
			this.Message = string.Format(@"{0}", CHECKK ? "File exists." : "File does not exist.");
			this.FilePath = CHECKK ? fileLocation : string.Empty;
			this.FolderPath = CHECKK ? this.GetDirectoryName(fileLocation) : string.Empty;
		}



		public override string ToString() => $"{this.Message}, {this.IsFileExist}";

		private string GetDirectoryName(string df)
		{
			string? GDN = Path.GetDirectoryName(df);
			if (GDN is not null)
			{
				return GDN;
			}
			else
			{
				return string.Empty;
			}
		}
	}
}
