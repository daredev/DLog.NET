using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DLog.NET.Extenstions;
using DLog.NET.Models;

namespace DLog.NET
{
    public class DLogger
    {
        private List<DLogMessage> logEntries;

        private List<TextBox> targetTextBoxes = new List<TextBox>();
        private List<FileInfo> targetFiles = new List<FileInfo>();
        private List<NotifyIcon> targetNotifyIcons = new List<NotifyIcon>();
        private List<ProgressBar> targetProgressBars = new List<ProgressBar>();
        private List<ToolStripProgressBar> targetToolStripProgressBars = new List<ToolStripProgressBar>();

        private SqlConnection connection = new SqlConnection();
        private string tableName;
        private string usernameColumn;
        private string messageColumn;
        private string eventDateColumn;
        private string username;

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

        public bool IsConsoleUsed = false;
        public bool IsDatabaseUsed = false;
        public TextWriter ConsoleOut = null;


        /// <summary>
        /// Instanciates Log class
        /// </summary>
        public DLogger()
        {
            logEntries = new List<DLogMessage>();
        }


        /// <summary>
        /// Adds support for outputting to console out
        /// </summary>
        /// <param name="consoleOut">Console.Out from Console Application code</param>
        public void AddConsoleOut(TextWriter consoleOut)
        {
            if (consoleOut != null)
            {
                IsConsoleUsed = true;
                ConsoleOut = consoleOut;
            }
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
        /// Removes TextBox control from log output
        /// </summary>
        /// <param name="textBox">TextBox to remove</param>
        public void RemoveTargetTextBox(TextBox textBox)
        {
            if (targetTextBoxes.Contains(textBox))
                targetTextBoxes.Remove(textBox);
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
            {
                StreamWriter sw = File.CreateText(newTargetFile.FullName);
                sw.Flush();
                sw.Close();
            }
            bool exists = false;

            foreach (FileInfo targetFile in targetFiles)
            {
                if (targetFile.FullName == path)
                    exists = true;
            }

            if (!exists)
                targetFiles.Add(newTargetFile);
        }

        /// <summary>
        /// Removes file from log output
        /// </summary>
        /// <param name="path">Log file path</param>
        public void RemoveTargetFile(string path)
        {
            foreach (FileInfo targetFile in targetFiles.ToList())
            {
                if (targetFile.FullName == path)
                    targetFiles.Remove(targetFile);
            }
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
        /// Removes Notify Icon control from log output
        /// </summary>
        /// <param name="notifyIcon">Notify Icon control to remove</param>
        public void RemoveTargetNotifyIcon(NotifyIcon notifyIcon)
        {
            if (targetNotifyIcons.Contains(notifyIcon))
                targetNotifyIcons.Remove(notifyIcon);
        }

        /// <summary>
        /// Adds file by FileInfo type variable for log output target file list
        /// </summary>
        /// <param name="file">FileInfo type variable</param>
        public void AddTargetFile(FileInfo file)
        {
            AddTargetFile(file.FullName);
        }

        /// <summary>
        /// Adds Progress Bar control for log output
        /// </summary>
        /// <param name="progressBar">Progress Bar control to add</param>
        public void AddProgressBar(ProgressBar progressBar)
        {
            if (targetProgressBars == null)
                targetProgressBars = new List<ProgressBar>();
            targetProgressBars.Add(progressBar);
        }

        /// <summary>
        /// Removes Progress Bar control from log output
        /// </summary>
        /// <param name="progressBar">Progress Bar control to remove</param>
        public void RemoveTargetProgressBar(ProgressBar progressBar)
        {
            if (targetProgressBars.Contains(progressBar))
                targetProgressBars.Remove(progressBar);
        }

        /// <summary>
        /// Adds ToolStripProgressBar control for log output
        /// </summary>
        /// <param name="progressBar">ToolStripProgressBar control to add</param>
        public void AddToolStripProgressBar(ToolStripProgressBar progressBar)
        {
            if (targetToolStripProgressBars == null)
                targetToolStripProgressBars = new List<ToolStripProgressBar>();
            targetToolStripProgressBars.Add(progressBar);
        }

        /// <summary>
        /// Removes ToolStripProgressBar control from log output
        /// </summary>
        /// <param name="progressBar">ToolStripProgressBar control to remove</param>
        public void RemoveTargetProgressBar(ToolStripProgressBar progressBar)
        {
            if (targetToolStripProgressBars.Contains(progressBar))
                targetToolStripProgressBars.Remove(progressBar);
        }


        /* Experimental */

        public void AddDatabase(SqlConnection connection, string tableName, string usernameColumn, string messageColumn,
            string eventDateColumn, string username)
        {
            this.connection = connection;
            this.tableName = tableName;
            this.usernameColumn = usernameColumn;
            this.messageColumn = messageColumn;
            this.eventDateColumn = eventDateColumn;
            this.username = username;
            try
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                    IsDatabaseUsed = true;
                connection.Close();
            }
            catch (Exception)
            {
                IsDatabaseUsed = false;
            }
        }


        /// <summary>
        /// Outputs log to each target objects
        /// </summary>
        /// <param name="message">Log message</param>
        /// <param name="progress">(Optional) Progress value (for ProgressBar type controls)</param>
        public void Write(string message, int progress = -1)
        {
            List<DLogMessage> m_logEntries;
            DLogMessage msg = new DLogMessage(message);
            if (logEntries == null)
                m_logEntries = new List<DLogMessage>();
            else
                m_logEntries = logEntries;

            m_logEntries.Add(msg);

            foreach (var entry in m_logEntries.AsEnumerable())
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
                    object lockObject = new object();

                    foreach (FileInfo targetFile in targetFiles)
                    {
                        lock (lockObject)
                        {
                            if (!targetFile.Exists)
                            {
                                using (
                                    var fs = File.Open(targetFile.FullName, FileMode.Create, FileAccess.Write,
                                        FileShare.ReadWrite))
                                {
                                    using (StreamWriter sw = new StreamWriter(fs))
                                    {
                                        sw.AutoFlush = true;
                                        sw.WriteLine("Log started");
                                        sw.Flush();
                                    }
                                }
                            }
                            else
                            {
                                using (var fs = File.Open(targetFile.FullName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                                {
                                    using (StreamWriter sw = new StreamWriter(fs))
                                    {
                                        sw.AutoFlush = true;
                                        sw.WriteLine(entry.GetFormatted());
                                        sw.Flush();
                                    }
                                }
                            }
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
                            ProgressBar bar = progressBar;
                            bar.InvokeIfRequired(delegate
                            {
                                bar.Value = progress;
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
                if (IsConsoleUsed && ConsoleOut != null)
                {
                    ConsoleOut.WriteLine(entry.GetFormatted());
                }
                if (IsDatabaseUsed)
                {
                    if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(usernameColumn) &&
                        !string.IsNullOrEmpty(messageColumn) && !string.IsNullOrEmpty(eventDateColumn) && !string.IsNullOrEmpty(username))
                    {
                        string logInsertCommandTemplate = "INSERT INTO [{0}] ([{1}], [{2}], [{3}]) VALUES ('{4}','{5}','{6}')";
                        string logInsertCommand = string.Format(logInsertCommandTemplate, tableName, usernameColumn, messageColumn, eventDateColumn, username,entry.Message, DateTime.Now);
                        SqlCommand sqlCmd = new SqlCommand(logInsertCommand, connection);
                        sqlCmd.CommandType = CommandType.Text;
                        if (connection.State != ConnectionState.Open)
                            connection.Open();
                        sqlCmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            logEntries = new List<DLogMessage>();
        }


    }
}
