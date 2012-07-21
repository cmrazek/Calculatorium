using System;
using System.Collections.Generic;
using System.Windows.Forms;
//using System.Runtime.InteropServices;
using System.Threading;

namespace Calc
{
	static class Program
	{
		private const string k_guid = "{1345A991-7C01-4a74-A09E-F828A8E3613D}";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			try
			{
				bool createdNew = false;
				using (Semaphore sem = new Semaphore(0, 1, "Global\\" + k_guid, out createdNew))
				{
					if (createdNew)
					{
						ThreadPool.QueueUserWorkItem((WaitCallback)(state => SemProc(sem)));

						Globals.form = new CalcForm();
						Application.Run(Globals.form);
					}
					sem.Release();
				}
			}
			catch (Exception)
			{ }
		}

		static void SemProc(Semaphore s)
		{
			try
			{
				if (s != null)
				{
					while (s.WaitOne())
					{
						Globals.form.AppActivate();
					}
				}
			}
			catch (Exception)
			{ }
		}
	}
}
