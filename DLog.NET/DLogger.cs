using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DLog.NET.Extenstions;
using DLog.NET.Models;

namespace DLog.NET
{
    public class DLogger
    {
        private List<DLogMessage> logEntries;

        private List<TextBox> targetTextBoxes;
        private List<FileInfo> targetFiles;
        private List<NotifyIcon> targetNotifyIcons;
        private List<ProgressBar> targetProgressBars;
        private List<ToolStripProgressBar> targetToolStripProgressBars; 

        public List<NotifyIcon> TargetNotifyIcons
        {
            get { return targetNotifyIcons; }
        }

        public List<FileInfo> TargetFiles
        {
            get { return targetFiles; }
        }

        public List<TextBox> TargetTextBoxes
        {
            get { return targetTextBoxes; }
        }

        public List<ProgressBar> TargetProgressBars
        {
            get { return targetProgressBars; }
        }

        public List<ToolStripProgressBar> TargetToolStripProgressBars
        {
            get { return targetToolStripProgressBars; }
        } 

        /// <summary>
        /// Instanciates Log class
        /// </summary>
        public DLogger()
        {
            logEntries = new List<DLogMessage>();
        }


        /// <summary>
        /// Adds TextBox control for log output target control list
        /// </summary>
        /// <param name="textBox">TextBox to add</param>
        public void AddTargetTextBox(TextBox textBox)
        {
            if (targetTextBoxes == null)
                targetTextBoxes = new List<TextBox>();
            targetTextBoxes.Add(textBox);
        }

        /// <summary>
        /// Adds file by path for log output target file list
        /// </summary>
        /// <param name="path">Path of the file</param>
        public void AddTargetFile(string path)
        {
            if (targetFiles == null)
                targetFiles = new List<FileInfo>();
            FileInfo newTargetFile = new FileInfo(path);
            if (newTargetFile.Directory != null && !newTargetFile.Directory.Exists)
                Directory.CreateDirectory(newTargetFile.Directory.FullName);
            if (!newTargetFile.Exists)
                File.CreateText(newTargetFile.FullName);
            targetFiles.Add(newTargetFile);
        }

        /// <summary>
        /// Adds NotifyIcon control for log output 
        /// </summary>
        /// <param name="notifyIcon">NotifyIcon to add</param>
        public void AddTargetNotifyIcon(NotifyIcon notifyIcon)
        {
            if (targetNotifyIcons == null)
                targetNotifyIcons = new List<NotifyIcon>();
            targetNotifyIcons.Add(notifyIcon);
        }

        /// <summary>
        /// Adds file by FileInfo type variable for log output target file list
        /// </summary>
        /// <param name="file">FileInfo type variable</param>
        public void AddTargetFile(FileInfo file)
        {
            AddTargetFile(file.FullName);
        }

        public void AddProgressBar(ProgressBar progressBar)
        {
            if (targetProgressBars == null)
                targetProgressBars = new List<ProgressBar>();
            targetProgressBars.Add(progressBar);
        }

        public void AddToolStripProgressBar(ToolStripProgressBar progressBar)
        {
            if (targetToolStripProgressBars == null)
                targetToolStripProgressBars = new List<ToolStripProgressBar>();
            targetToolStripProgressBars.Add(progressBar);
        }

        /// <summary>
        /// Outputs log to controls and files (if previously set)
        /// </summary>
        /// <param name="message">Log message</param>
        public void Write(string message, int progress = -1)
        {
            DLogMessage msg = new DLogMessage(message);
            if (logEntries == null)
                logEntries = new List<DLogMessage>();
            logEntries.Add(msg);

            foreach (var entry in logEntries)
            {
                if (targetTextBoxes != null)
                {
                    foreach (Control control in targetTextBoxes)
                    {
                        Control myControl = control;
                        DLogMessage myLogEntry = entry;
                        control.InvokeIfRequired(delegate
                        {
                            myControl.Text += myLogEntry.GetFormatted() + Environment.NewLine;
                            if (myControl is TextBox)
                            {
                                ((TextBox)myControl).SelectionStart = ((TextBox)myControl).TextLength;
                                ((TextBox)myControl).ScrollToCaret();
                            }
                        });
                    }
                }
                if (targetFiles != null)
                {
                    foreach (FileInfo targetFile in targetFiles)
                    {
                        if (!targetFile.Exists)
                        {
                            using (StreamWriter sw = targetFile.CreateText())
                            {
                                sw.WriteLine("Log started");
                            }
                        }

                        using (StreamWriter sw = targetFile.AppendText())
                        {
                            sw.WriteLine(entry.GetFormatted());
                        }
                    }
                }
                if (targetNotifyIcons != null)
                {
                    foreach (NotifyIcon notifyIcon in targetNotifyIcons)
                    {
                        ToolTipIcon icon;
                        notifyIcon.BalloonTipText = entry.GetFormatted();
                        notifyIcon.Visible = true;
                        if (entry.Message.ToLower().Contains("error"))
                            icon = ToolTipIcon.Error;
                        else
                            icon = ToolTipIcon.Info;

                        notifyIcon.ShowBalloonTip(1, "Information", entry.Message, icon);
                    }
                }

                if (progress > -1)
                {
                    if (targetProgressBars != null)
                    {
                        foreach (ProgressBar progressBar in targetProgressBars)
                        {
                            progressBar.InvokeIfRequired(delegate
                            {
                                progressBar.Value = progress;
                            });
                        }
                    }

                    if (targetToolStripProgressBars != null)
                    {
                        foreach (ToolStripProgressBar toolStripProgressBar in targetToolStripProgressBars)
                        {
                            toolStripProgressBar.Value = progress;
                        }
                    }

                }

            }
            logEntries = new List<DLogMessage>();
        }


    }
}
