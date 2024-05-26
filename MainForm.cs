using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Entity;
using System.Data.SQLite;

namespace WutheringWavesFrameUnlocker___WWFU
{
	public partial class MainForm : Form
	{
		static string db_file_location_byLauncher = @"Wuthering Waves Game\Client\Saved\LocalStorage\LocalStorage.db";
		static string db_file_location_byGameClient = @"Client\Saved\LocalStorage\LocalStorage.db";
		static string game_execution_file_location_byLauncher = @"Wuthering Waves Game\Wuthering Waves.exe";
		//static string game_execution_file_location_byGameClient = @"Wuthering Waves.exe";
		string dbFileFullLocation = string.Empty;
		string executionFileFullLocation = string.Empty;

		FileCheckResult Execution = new FileCheckResult();
		FileCheckResult DataBase = new FileCheckResult();

		SQLiteConnection _conn;
		private static JObject Settings;

		static private int wasSavedHz = 0;
		static private int nowSelectedHz = 0;

		public MainForm()
		{
			InitializeComponent();
			textBox1.ReadOnly = true;
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			Logger.WriteLog(LogType.Info, "WWFU Program Launched.", true);


			string? InstallPathResult = checkWWLauncherInstalled("Wuthering Waves");
			if (InstallPathResult != null && InstallPathResult != string.Empty)
			{
				SuccessfullyGetDBFileLocation(InstallPathResult);
			}
			else
			{
				FindWutheringWavesExecuteFile(true);
			}
		}

		private void SuccessfullyGetDBFileLocation(string? InstallPath)
		{
			executionFileFullLocation = string.Format(@"{0}\{1}", InstallPath, game_execution_file_location_byLauncher);
			dbFileFullLocation = string.Format(@"{0}\{1}", InstallPath, db_file_location_byLauncher);
			Execution = new FileCheckResult("Execution", executionFileFullLocation);
			DataBase = new FileCheckResult("DataBase", dbFileFullLocation);

			if (Execution.IsFileExist == true && DataBase.IsFileExist == true)
			{
				//textBox1.Text = string.Format("��ġ ��� : {0}\r\nDB ��� : {1}", Execution.FolderPath, DataBase.FilePath);
				textBox1.Text = string.Format("��ġ ��� : {0}", Execution.FolderPath);
				GetGameDBFileData();
			}
			else
			{
				FindWutheringWavesExecuteFile(true);
			}
		}

		private void GetGameDBFileData()
		{
			try
			{
				_conn = new SQLiteConnection(string.Format(@"Data Source={0};", DataBase.FilePath));
				_conn.Open();
				using (SQLiteCommand comm = _conn.CreateCommand())
				{
					comm.CommandText = @"SELECT value FROM LocalStorage WHERE key = 'GameQualitySetting'";
					var jsonString = comm.ExecuteScalar()?.ToString();
					if (jsonString == null)
					{
						Logger.WriteLog(LogType.Error, "db���� ���ð��� �о���� ���߽��ϴ�..", true);
						MessageBox.Show("db���� ���ð��� �о���� ���߽��ϴ�..", "ERROR");
						return;
					}
					Settings = JObject.Parse(jsonString);
					Logger.WriteLog(LogType.Info, "���� ���� �ҷ����� �Ϸ�!", true);
					int KeyCustomFrameRate = Convert.ToInt32(Settings["KeyCustomFrameRate"]);
					//"KeyCustomFrameRate": 60
					switch (KeyCustomFrameRate)
					{
						case 30: radioButton1.Checked = true; break;
						case 60: radioButton2.Checked = true; break;
						case 100: radioButton3.Checked = true; break;
						case 120: radioButton4.Checked = true; break;
						case 144: radioButton5.Checked = true; break;
						default:
							radioButton2.Checked = true;
							break;
					}
					wasSavedHz = KeyCustomFrameRate;
					SavedHzLabelMsg(KeyCustomFrameRate.ToString());
					Logger.WriteLog(LogType.Info, string.Format("������ ������ �ֻ����� {0}Hz �Դϴ�.", KeyCustomFrameRate), true);
				}
				Logger.WriteLog(LogType.Info, "������ư, �����ư Ȱ��ȭ..", true);
				//������ư, �����ư Ȱ��ȭ
				button2.Enabled = true;
				radioButton1.Enabled = true;
				radioButton2.Enabled = true;
				radioButton3.Enabled = true;
				radioButton4.Enabled = true;
				radioButton5.Enabled = true;
			}
			catch (Exception ex)
			{
				Logger.WriteLog(LogType.Info, ex.Message, true);
			}
		}

		public static string? checkWWLauncherInstalled(string findByName)
		{
			string? InstallPath = string.Empty;
			string UninstallregistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
			string UninstallregistryKey2 = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
			try
			{
				Logger.WriteLog(LogType.Info, "������Ʈ������ ���� ��ġ ���͸��� ��ȸ���Դϴ�..", true);
				InstallPath = findTargetAppFromLocalMachine(findByName, UninstallregistryKey);
				if (InstallPath == string.Empty)
				{
					InstallPath = findTargetAppFromLocalMachine(findByName, UninstallregistryKey2);
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog(LogType.Info, ex.Message, true);
			}
			return InstallPath;
		}

		//������Ʈ������ 
		private static string? findTargetAppFromLocalMachine(string findByName, string registryKey)
		{
			string? result_ = string.Empty;

			//64 bits computer
			RegistryKey basekey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
			var key = basekey.OpenSubKey(registryKey);
			if (key != null)
			{
				result_ = FindingMachine(findByName, key);
			}
			else
			{
				//32bit
				basekey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
				key = basekey.OpenSubKey(registryKey);
				if (key != null)
				{
					result_ = FindingMachine(findByName, key);
				}
			}
			return result_;
		}

		private static string? FindingMachine(string findByName, RegistryKey key)
		{
			string? return_ = string.Empty;
			if (key != null)
			{
				string? displayName;
				foreach (var subkey in key.GetSubKeyNames().Select(keyName => key.OpenSubKey(keyName)))
				{
					if (subkey != null)
					{
						displayName = subkey.GetValue("DisplayName") as string;
						if (displayName != null && displayName.Contains(findByName))
						{
							return_ = subkey.GetValue("InstallLocation") as string;
							return_ = subkey.GetValue("InstallPath") as string;
							if (return_ != string.Empty && return_ != null)
							{
								Logger.WriteLog(LogType.Info, "������Ʈ������ ���� ��ġ ���͸��� ã�ҽ��ϴ�.", true);
								break;
							}
						}
					}
				}
				key.Close();
			}
			return return_;
		}


		private void button1_Click(object sender, EventArgs e)
		{
			FindWutheringWavesExecuteFile(false);
		}

		private void FindWutheringWavesExecuteFile(bool isAutoExecuted)
		{
			if (isAutoExecuted)
			{
				Logger.WriteLog(LogType.Info, "�ڵ����� ���� ��ġ ���͸��� ã�� ���߽��ϴ�.\r\n�������� Wuthering Waves.exe ������ ������ �ּ���.", true);
				MessageBox.Show("�ڵ����� ���� ��ġ ���͸��� ã�� ���߽��ϴ�.\r\n�������� Wuthering Waves.exe ������ ������ �ּ���.");
			}
			using (openFileDialog1 = new OpenFileDialog())
			{
				openFileDialog1.Title = "Wuthering Waves.exe ���� ����";
				openFileDialog1.Filter = "���� �������� (Wuthering Waves.exe)|*.exe";
				openFileDialog1.Multiselect = false;
				openFileDialog1.CheckFileExists = true;
				openFileDialog1.CheckPathExists = true;
				DialogResult result = openFileDialog1.ShowDialog();
				if (result == DialogResult.OK)
				{
					Execution = new FileCheckResult("Execution", openFileDialog1.FileName);
					if (Execution.IsFileExist)
					{
						DataBase = new FileCheckResult("DataBase", string.Format(@"{0}\{1}", Execution.FolderPath, db_file_location_byGameClient));
						if (DataBase.IsFileExist)
						{
							//textBox1.Text = string.Format("��ġ ��� : {0}\r\nDB ��� : {1}", Execution.FolderPath, DataBase.FilePath);
							textBox1.Text = string.Format("��ġ ��� : {0}", Execution.FolderPath);
							GetGameDBFileData();
						}
					}
				}
				else
				{
					if (!Execution.IsFileExist)
					{
						Logger.WriteLog(LogType.Info, "���� ��ġ ���͸�����\r\nWuthering Waves.exe ������ ������ �ּ���.", true);
						MessageBox.Show("���� ��ġ ���͸�����\r\nWuthering Waves.exe ������ ������ �ּ���.");
					}
				}
			}
		}


		private void button2_Click(object sender, EventArgs e)
		{
			SelectedHzSaveToDBFile();
		}

		private void SelectedHzSaveToDBFile()
		{
			try
			{
				using (SQLiteCommand comm = _conn.CreateCommand())
				{
					comm.CommandText = $@"UPDATE LocalStorage SET value = @value WHERE key = 'GameQualitySetting'";
					comm.Parameters.Add(
					new SQLiteParameter()
					{
						ParameterName = "@value",
						Value = JsonConvert.SerializeObject(Settings, Newtonsoft.Json.Formatting.None)
					});

					if (comm.ExecuteNonQuery() == 1)
					{
						string? hz = Convert.ToString(Settings["KeyCustomFrameRate"]);

						Logger.WriteLog(LogType.Info, string.Format(@"�ֻ��� {0}Hz �� ���������� �ݿ��Ǿ����ϴ�.", hz), true);
						MessageBox.Show(string.Format(@"�ֻ��� {0}Hz �� ���������� �ݿ��Ǿ����ϴ�.", hz));
						SavedHzLabelMsg(hz);
					};
				}
			}
			catch (System.Data.SQLite.SQLiteException msg)
			{

				//msg.ErrorCode
				if (msg.ErrorCode == 8)
				{
					Logger.WriteLog(LogType.Error, string.Format("{0}\r\n{1}", "������ ����Ǿ� �־� ������ �������� ���߽��ϴ�.", msg.Message), true);
					MessageBox.Show(string.Format("{0}\r\n{1}", "������ ����Ǿ� �־� ������ �������� ���߽��ϴ�.", msg.Message));
				}
				else
				{
					Logger.WriteLog(LogType.Error, msg.Message, true);
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLog(LogType.Error, ex.Message, true);
				MessageBox.Show(ex.Message);
			}
		}

		private void SavedHzLabelMsg(string? hz)
		{
			wasSavedHz = Convert.ToInt32(hz);
			if (wasSavedHz == 0)
			{
				label1.Text = string.Format("���� ����� �ֻ���:{0}Hz", wasSavedHz.ToString());
			}
			else
			{
				label1.Text = string.Format("���� ����� �ֻ���:{0}Hz", wasSavedHz.ToString());
			}
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			SetHzToSetting((RadioButton)sender, e);
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			SetHzToSetting((RadioButton)sender, e);
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			SetHzToSetting((RadioButton)sender, e);
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			SetHzToSetting((RadioButton)sender, e);
		}

		private void radioButton5_CheckedChanged(object sender, EventArgs e)
		{
			SetHzToSetting((RadioButton)sender, e);
		}

		private static void SetHzToSetting(RadioButton sender, EventArgs e)
		{
			if (sender.Checked)
			{
				Settings["KeyCustomFrameRate"] = Convert.ToInt32(sender.Tag);
				nowSelectedHz = Convert.ToInt32(sender.Tag);
				Logger.WriteLog(LogType.Info, string.Format("�ֻ��� {0}Hz �� �����Ͽ����ϴ�.", Convert.ToInt32(sender.Tag)), true);
			}
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			using (FileStream Steam = File.Open(Logger.filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (StreamReader Reader = new StreamReader(Steam))
				{
					while (!Reader.EndOfStream)
					{
						textBox2.Text = Reader.ReadToEnd();
						break;
					}
				}
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			//������ư, �����ư ��Ȱ��ȭ
			button2.Enabled = false;
			radioButton1.Enabled = false;
			radioButton2.Enabled = false;
			radioButton3.Enabled = false;
			radioButton4.Enabled = false;
			radioButton5.Enabled = false;
			toOriginSize();
		}


		private static Size originSize = new Size(606, 335);
		private static Size ExpandSize = new Size(606, 476);

		private void button3_Click(object sender, EventArgs e)
		{
			if (this.Size == originSize)
			{
				toExpandSize();
			}
			else
			{
				toOriginSize();
			}

		}

		private void toExpandSize()
		{
			//�α�â ���̰�
			this.Size = ExpandSize;
			timer1.Start();
		}

		private void toOriginSize()
		{
			//�α�â �Ⱥ��̰�
			this.Size = originSize;
			timer1.Stop();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("log ������ ��� ������ �ʱ�ȭ �մϴ�.", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				Logger.FlushLogFile();
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (wasSavedHz == 0 || nowSelectedHz == wasSavedHz)
			{
				Logger.WriteLog(LogType.Info, "WWFU Program Exit..", true);
				e.Cancel = false;
			}
			else
			{
				e.Cancel = true;
				if (MessageBox.Show(string.Format("�����Ͻ� {0}Hz �� ���� �� �����Ͻðڽ��ϱ�?", nowSelectedHz), "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					SelectedHzSaveToDBFile();
					SavedHzLabelMsg(nowSelectedHz.ToString());
					Logger.WriteLog(LogType.Info, "WWFU Program Exit..", true);
					e.Cancel = false;
				}
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			System.Windows.Forms.Application.Exit();
		}
	}
}

