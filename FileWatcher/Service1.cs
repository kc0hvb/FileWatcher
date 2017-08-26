using System;
using System.Threading;
using System.IO;
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
                Thread thread = new Thread(processFileWatcher);
                thread.Start();
                //Task.Factory.StartNew(() => processFileWatcher());
            }
            catch (Exception ex)
            {
                errorLogCreation(ex);    
            }

        }

        public void processFileWatcher()
        {
            ServiceController sc = new ServiceController("FileModWatcher");
            sc.WaitForStatus(ServiceControllerStatus.Running);
            while (true)
            {
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    // Start the service if the current status is stopped.
                    try
                    {
                        // Start the service, and wait until its status is "Running".
                        sc.Start();


                    }
                    catch (InvalidOperationException ex)
                    {
                        errorLogCreation(ex);
                    }
                }
                string sSource = ConfigurationManager.AppSettings["PAK_Location"];
                string sTarget = ConfigurationManager.AppSettings["PAK_Target_Location"];
                //string[] files = System.IO.Directory.GetFiles(sSource.ToString, "*.pak", SearchOption.AllDirectories);

                string[] fileEntries = System.IO.Directory.GetFiles(sSource, "*.*", System.IO.SearchOption.AllDirectories);

                if (!Directory.Exists(sTarget))
                {
                    Directory.CreateDirectory(sTarget);
                }

                foreach (string fileName in fileEntries)
                {
                    if (fileName.Contains(".pak"))
                    {
                        string sFileName = Path.GetFileName(fileName);
                        string sFileNameDest = sTarget + '\\' + sFileName;
                        bool sFileNameDestExist = File.Exists(sFileNameDest);
                        if (sFileNameDestExist)
                        {
                            FileInfo fFileInfoSource = new FileInfo(fileName);
                            FileInfo fFileInfoDest = new FileInfo(sFileNameDest);
                            if (fFileInfoSource.LastWriteTimeUtc > fFileInfoDest.LastWriteTimeUtc)
                            {
                                File.Copy(fileName, sFileNameDest, true);
                            }
                        }
                        else
                        {
                            File.Copy(fileName, sFileNameDest, true);
                        }
                    }
                }

                Thread.Sleep(Int32.Parse(ConfigurationManager.AppSettings["Sleep_Time"]));
                return;
            }
        }

        public void errorLogCreation(Exception ex)
        {
            string sErrorFilePath = AppDomain.CurrentDomain.BaseDirectory + $"Error Log {DateTime.Today.Millisecond}.txt";
            using (StreamWriter file =
            new StreamWriter(sErrorFilePath))
            {
                file.WriteLine(ex);
            }
            OnStop();
        }

        protected override void OnStop()
        {
            base.OnStop();
            CanShutdown = true;
        }

    }
}
