using System;
using System.Diagnostics;
using System.Security;
using System.ServiceProcess;
namespace Demo.WindowsService
{
	partial class HttpJobRunnerInstaller
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private ServiceInstaller _httpJobServiceInstaller;
		private ServiceProcessInstaller _httpJobServiceProcessInstaller;
		private String _serviceName = "HttpJobRunner";

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			_httpJobServiceInstaller = new ServiceInstaller();
			_httpJobServiceProcessInstaller = new ServiceProcessInstaller();

			_httpJobServiceInstaller.Description = "HttpJob runner service which carries out a series of HTTP calls";
			_httpJobServiceInstaller.DisplayName = "HttpJob automatic runner";
			_httpJobServiceInstaller.ServiceName = _serviceName;
			_httpJobServiceInstaller.StartType = ServiceStartMode.Automatic;				

			_httpJobServiceProcessInstaller.Account = ServiceAccount.LocalSystem;

			Installers.AddRange(new System.Configuration.Install.Installer[]
				{
					_httpJobServiceInstaller
					, _httpJobServiceProcessInstaller
				});
		}

		protected override void OnAfterInstall(System.Collections.IDictionary savedState)
		{
			base.OnAfterInstall(savedState);
			using (var serviceController = new ServiceController(_httpJobServiceInstaller.ServiceName, Environment.MachineName))
			{
				serviceController.Start();
			}
		}

		protected override void OnCommitted(System.Collections.IDictionary savedState)
		{
			base.OnCommitted(savedState);
			SetRecoveryOptions();
		}	

		private void SetRecoveryOptions()
		{
			int exitCode;
			using (Process process = new Process())
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo();
				processStartInfo.FileName = "sc";
				processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				processStartInfo.Arguments = string.Format("failure \"{0}\" reset= 0 actions= restart/1000", _serviceName);
				process.Start();
				process.WaitForExit();
				exitCode = process.ExitCode;
			}

			if (exitCode != 0)
			{
				throw new InvalidOperationException(string.Format("sc failure setting process exited with code {0}", exitCode));
			}
		}

		#endregion
	}
}