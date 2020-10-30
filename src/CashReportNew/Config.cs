using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nwuram.Framework.Project;
using Nwuram.Framework.Logging;
using Nwuram.Framework.Settings.Connection;
using System.Windows.Forms;

namespace CashReportNew
{
    class Config
    {
        public static Procedures hCntMainKasReal { get; set; } //осн. коннект
        //public static Procedures hCntMainDbase1 { get; set; } //осн. коннект
        //public static Procedures hCntVVOdbase1 { get; set; } //доп. коннект
        public static Procedures hCntVVOKasReal { get; set; } //доп. коннект

        public static void DoOnUIThread(MethodInvoker d, Form _this)
        {
            if (_this.InvokeRequired) { _this.Invoke(d); } else { d(); }
        }
    }
}