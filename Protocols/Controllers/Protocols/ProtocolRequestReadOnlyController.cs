﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toxikon.ProtocolManager.Controllers.Templates;
using Toxikon.ProtocolManager.Interfaces.Protocols;
using Toxikon.ProtocolManager.Models;
using Toxikon.ProtocolManager.Models.Reports;
using Toxikon.ProtocolManager.Queries;
using Toxikon.ProtocolManager.Views.Templates;

namespace Toxikon.ProtocolManager.Controllers.Protocols
{
    public class ProtocolRequestReadOnlyController
    {
        private IProtocolRequestReadOnlyView view;
        private ProtocolRequest request;
        private IList templates;
        private SponsorContact sponsor;
        private LoginInfo loginInfo;
        private RequestFormController requestFormController;
        private string SelectOneMessage = "Please select one title and try it again.";

        public ProtocolRequestReadOnlyController(IProtocolRequestReadOnlyView view)
        {
            this.view = view;
            this.view.SetController(this);
            loginInfo = LoginInfo.GetInstance();
            this.templates = new ArrayList();
            this.requestFormController = new RequestFormController(this.view.GetRequestForm);
            requestFormController.ClearForm();
        }

        public void LoadView(ProtocolRequest protocolRequest)
        {
            this.request = protocolRequest;
            this.sponsor = protocolRequest.Contact;
            this.requestFormController.LoadView(this.request);
            this.RefreshTitleListView();
        }

        public void ClearView()
        {
            this.requestFormController.ClearForm();
            this.view.ClearView();
        }

        private void AddTemplatesToView()
        {
            foreach (ProtocolTemplate item in this.templates)
            {
                this.view.AddTitleToView(item);
            }
        }

        private void RefreshTitleListView()
        {
            this.templates.Clear();
            this.view.ClearProtocolTitleListView();
            LoadRequestTemplates();
            AddTemplatesToView();
            this.view.SetListViewAutoResizeColumns();
        }

        private void LoadRequestTemplates()
        {
            this.templates = QProtocolRequestTemplates.SelectItems(this.request.ID);
        }

        private ProtocolTemplate GetSelectedTitleFromView()
        {
            int selectedIndex = Convert.ToInt32(this.view.SelectedTitleIndexes[0]);
            ProtocolTemplate title = this.templates[selectedIndex] as ProtocolTemplate;
            return title;
        }

        public void ViewEventsButtonClicked()
        {
            if (this.view.SelectedTitleIndexes.Count == 1)
            {
                ShowSelectedTitleEvents();
            }
            else
            {
                MessageBox.Show(this.SelectOneMessage);
            }
        }

        private void ShowSelectedTitleEvents()
        {
            ProtocolTemplate title = GetSelectedTitleFromView();
            IList events = QProtocolActivities.SelectItems(this.request.ID, title.TemplateID);
            IList columns = new ArrayList() { "Date", "User", "Event" };
            TemplatesController.ShowReadOnlyListViewForm(columns, events, view.ParentControl);
        }

        public void ViewCommentsButtonClicked()
        {
            if (this.view.SelectedTitleIndexes.Count == 1)
            {
                ShowSelectedTitleComments();
            }
            else
            {
                MessageBox.Show(this.SelectOneMessage);
            }
        }

        private void ShowSelectedTitleComments()
        {
            ProtocolTemplate title = GetSelectedTitleFromView();
            IList comments = QProtocolComments.SelectItems(title);
            IList columns = new ArrayList() { "Date", "User", "Comments" };
            TemplatesController.ShowReadOnlyListViewForm(columns, comments, view.ParentControl);
        }

        public void DownloadRequestReportButtonClicked()
        {
            if (this.request != null)
            {
                CreateAndDownloadSelectedTitleReport();
            }
            else
            {
                MessageBox.Show("Please select a request.");
            }
        }

        private void CreateAndDownloadSelectedTitleReport()
        {
            ProtocolRequestReport reportTemplate = new ProtocolRequestReport(this.request);
            reportTemplate.Create();
            MessageBox.Show("Download Complete!");
        }

        public void OpenFileButtonClicked()
        {
            if (this.view.SelectedTitleIndexes.Count == 1)
            {
                OpenSelectedTitleFile();   
            }
            else
            {
                MessageBox.Show(this.SelectOneMessage);
            }
        }

        private void OpenSelectedTitleFile()
        {
            ProtocolTemplate title = GetSelectedTitleFromView();
            if (title.FilePath != String.Empty && File.Exists(title.FilePath))
            {
                System.Diagnostics.Process.Start(title.FilePath);
            }
            else
            {
                MessageBox.Show("File does not exist.");
            }
        }
    }
}
