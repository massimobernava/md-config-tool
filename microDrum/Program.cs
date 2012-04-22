using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace microDrum
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ExceptionLogger logger = new ExceptionLogger();
            logger.NotificationType = NotificationType.Inform;
            logger.IncludeModule = false;
            logger.AddLogger(new TextFileLogger());

            Application.Run(new MainForm());
        }
    }
}
