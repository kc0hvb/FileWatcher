using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                string sSource = ConfigurationManager.AppSettings["PAK_Location"];

            }
            catch (Exception ex)
            {
                System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + $"Error Log {DateTime.Today}.txt");
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + $"Error Log {DateTime.Today}.txt"))
                {
                    file.WriteLine(ex);
                }
            }
                                  
        }

        protected override void OnStop()
        {
            this.Stop();
        }
    }
}
