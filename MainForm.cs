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
				//textBox1.Text = string.Format("설치 경로 : {0}\r\nDB 경로 : {1}", Execution.FolderPath, DataBase.FilePath);
				textBox1.Text = string.Format("설치 경로 : {0}", Execution.FolderPath);
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
						Logger.WriteLog(LogType.Error, "db에서 셋팅값을 읽어오지 못했습니다..", true);
						MessageBox.Show("db에서 셋팅값을 읽어오지 못했습니다..", "ERROR");
						return;
					}
					Settings = JObject.Parse(jsonString);
					Logger.WriteLog(LogType.Info, "게임 설정 불러오기 완료!", true);
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
					Logger.WriteLog(LogType.Info, string.Format("기존에 설정된 주사율은 {0}Hz 입니다.", KeyCustomFrameRate), true);
				}
				Logger.WriteLog(LogType.Info, "라디오버튼, 저장버튼 활성화..", true);
				//라디오버튼, 저장버튼 활성화
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
				Logger.WriteLog(LogType.Info, "레지스트리에서 명조 설치 디렉터리를 조회중입니다..", true);
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

		//레지스트리에서 
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
								Logger.WriteLog(LogType.Info, "레지스트리에서 명조 설치 디렉터리를 찾았습니다.", true);
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
				Logger.WriteLog(LogType.Info, "자동으로 명조 설치 디렉터리를 찾지 못했습니다.\r\n수동으로 Wuthering Waves.exe 파일을 선택해 주세요.", true);
				MessageBox.Show("자동으로 명조 설치 디렉터리를 찾지 못했습니다.\r\n수동으로 Wuthering Waves.exe 파일을 선택해 주세요.");
			}
			using (openFileDialog1 = new OpenFileDialog())
			{
				openFileDialog1.Title = "Wuthering Waves.exe 파일 선택";
				openFileDialog1.Filter = "명조 실행파일 (Wuthering Waves.exe)|*.exe";
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
							//textBox1.Text = string.Format("설치 경로 : {0}\r\nDB 경로 : {1}", Execution.FolderPath, DataBase.FilePath);
							textBox1.Text = string.Format("설치 경로 : {0}", Execution.FolderPath);
							GetGameDBFileData();
						}
					}
				}
				else
				{
					if (!Execution.IsFileExist)
					{
						Logger.WriteLog(LogType.Info, "명조 설치 디렉터리에서\r\nWuthering Waves.exe 파일을 선택해 주세요.", true);
						MessageBox.Show("명조 설치 디렉터리에서\r\nWuthering Waves.exe 파일을 선택해 주세요.");
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

						Logger.WriteLog(LogType.Info, string.Format(@"주사율 {0}Hz 가 정상적으로 반영되었습니다.", hz), true);
						MessageBox.Show(string.Format(@"주사율 {0}Hz 가 정상적으로 반영되었습니다.", hz));
						SavedHzLabelMsg(hz);
					};
				}
			}
			catch (System.Data.SQLite.SQLiteException msg)
			{

				//msg.ErrorCode
				if (msg.ErrorCode == 8)
				{
					Logger.WriteLog(LogType.Error, string.Format("{0}\r\n{1}", "게임이 실행되어 있어 설정을 저장하지 못했습니다.", msg.Message), true);
					MessageBox.Show(string.Format("{0}\r\n{1}", "게임이 실행되어 있어 설정을 저장하지 못했습니다.", msg.Message));
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
				label1.Text = string.Format("현재 저장된 주사율:{0}Hz", wasSavedHz.ToString());
			}
			else
			{
				label1.Text = string.Format("현재 저장된 주사율:{0}Hz", wasSavedHz.ToString());
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
				Logger.WriteLog(LogType.Info, string.Format("주사율 {0}Hz 를 선택하였습니다.", Convert.ToInt32(sender.Tag)), true);
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
			//라디오버튼, 저장버튼 비활성화
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
			//로그창 보이게
			this.Size = ExpandSize;
			timer1.Start();
		}

		private void toOriginSize()
		{
			//로그창 안보이게
			this.Size = originSize;
			timer1.Stop();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("log 파일의 모든 내용을 초기화 합니다.", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
				if (MessageBox.Show(string.Format("선택하신 {0}Hz 로 저장 후 종료하시겠습니까?", nowSelectedHz), "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
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

