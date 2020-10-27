using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Nwuram.Framework.Project;
using Nwuram.Framework.Logging;
using Nwuram.Framework.Settings.Connection;

namespace CashReportNew
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //try
            //{
                if (Project.FillSettings(args))
                {
                    Logging.Init(ConnectionSettings.GetServer(), ConnectionSettings.GetDatabase(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);
                    Config.hCntMainKasReal = new Procedures(ConnectionSettings.GetServer(), ConnectionSettings.GetDatabase(), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);
                    //Config.hCntMainDbase1 = new Procedures(ConnectionSettings.GetServer("2"), ConnectionSettings.GetDatabase("2"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);
                    Config.hCntVVOKasReal = new Procedures(ConnectionSettings.GetServer("2"), ConnectionSettings.GetDatabase("2"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);
                    //Config.hCntVVOdbase1 = new Procedures(ConnectionSettings.GetServer("4"), ConnectionSettings.GetDatabase("4"), ConnectionSettings.GetUsername(), ConnectionSettings.GetPassword(), ConnectionSettings.ProgramName);

                    LogStartProgram();

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());

                    LogStopProgram();
                }
            //}
            //catch {
            //    string g = "";
            //    foreach (var n in args)
            //        g += " " + n;
            //    MessageBox.Show("Ошибка входа в программу" ); }
        }

        static void LogStartProgram()
        {
            Logging.StartFirstLevel(1);
            Logging.Comment("Вход в программу");
            Logging.StopFirstLevel();
        }

        static void LogStopProgram()
        {
            Logging.StartFirstLevel(2);
            Logging.Comment("Выход из программы");
            Logging.StopFirstLevel();
        }
    }
}
