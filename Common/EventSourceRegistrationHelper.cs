using System;

using System.IO;
using System.Reflection;

namespace Common
{
	public static class EventSourceRegistrationHelper
	{
		private const string EventRegister = "eventRegister.exe";

		public static void Register()
		{
			string currentDir = Environment.CurrentDirectory;
			string mainFileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
			Environment.CurrentDirectory = Path.GetDirectoryName(mainFileName);
			try
			{
				if (!File.Exists(EventRegister))
				{
					using (var stream = Assembly.GetExecutingAssembly()
						.GetManifestResourceStream("Common." + EventRegister))
					{
						if (stream != null)
							using (var reader = new BinaryReader(stream))
							{
								File.WriteAllBytes(EventRegister, reader.ReadBytes((int)stream.Length));
							}
					}
				}
				var psi = new System.Diagnostics.ProcessStartInfo(EventRegister, Path.GetFileName(mainFileName))
				{
					CreateNoWindow = true,
					LoadUserProfile = false,
					ErrorDialog = false,
					RedirectStandardError = true,
					RedirectStandardOutput = true,
					UseShellExecute = false,
					WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
				};
				using (var info = System.Diagnostics.Process.Start(psi))
				{
					if (info == null) throw new Exception("Failed to register Event Providers (Failed to launch exe)");

					info.WaitForExit();

					if (info.ExitCode != 0)
					{
						throw new Exception($"Failed to register Event Providers (Exitcode:{info.ExitCode})");
					}

					//string error = info.StandardError.ReadToEnd();
					//string result = info.StandardOutput.ReadToEnd();
				}
			}
			finally
			{
				Environment.CurrentDirectory = currentDir;
			}
		}
	}
}