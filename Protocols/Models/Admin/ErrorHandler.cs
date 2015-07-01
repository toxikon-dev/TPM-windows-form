﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Toxikon.ProtocolManager.Models
{
    public class ErrorHandler
    {
        private static string logFolder = @"\\toxx\toxxShared\TPMErrorLogs\";
        /*Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                          "\\TPMErrorLogFiles\\";*/

        public static void SendEmailToSupport(Exception catchedException)
        {
            try
            {
                LoginInfo loginInfo = LoginInfo.GetInstance();

                Outlook.Application outlookApp = new Outlook.Application();
                Outlook._MailItem mailItem = (Outlook.MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);

                Outlook.Recipient recipient = (Outlook.Recipient)mailItem.Recipients.Add("Bichngoc.McCulley@toxikon.com");
                recipient.Resolve();

                mailItem.Subject = loginInfo.UserName + "---ERROR: " + catchedException.Message;
                mailItem.Body = catchedException.ToString();

                mailItem.Save();
                mailItem.Display();

                recipient = null;
                mailItem = null;
                outlookApp = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void CreateLogFile(string formName, string controlName, Exception e)
        {
            try
            {
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }
                LoginInfo loginInfo = LoginInfo.GetInstance();
                string logDateTime = DateTime.Now.ToString("yyyyMMdd-HHmmss-");

                StreamWriter log = new StreamWriter(logFolder + logDateTime + loginInfo.UserName + ".txt");
                log.WriteLine("Date Time: " + DateTime.Now);
                log.WriteLine("Form Name: " + formName);
                log.WriteLine("Control Name: " + controlName);
                log.WriteLine(e.ToString());

                log.Close();

                SendEmailToSupport(e);
            }
            catch(UnauthorizedAccessException accessEx)
            {
                MessageBox.Show("ErrorHandler: " + accessEx.Message);
            }
        }
    }
}