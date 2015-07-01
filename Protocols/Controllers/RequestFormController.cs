﻿using Toxikon.ProtocolManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toxikon.ProtocolManager.Models;
using Toxikon.ProtocolManager.Views.Templates;
using Toxikon.ProtocolManager.Controllers.Templates;
using System.Windows.Forms;
using System.Collections;
using Toxikon.ProtocolManager.Queries;
using System.Diagnostics;

namespace Toxikon.ProtocolManager.Controllers
{
    public class RequestFormController
    {
        protected IRequestForm view;
        protected ProtocolRequest request;
        LoginInfo loginInfo;

        enum OptionFields { Guidelines, Compliance, ProtocolType, AssignedTo, Contact };

        public RequestFormController(IRequestForm view)
        {
            this.view = view;
            this.view.SetController(this);
            this.request = new ProtocolRequest();
            loginInfo = LoginInfo.GetInstance();
        }

        public void LoadView(ProtocolRequest protocolRequest)
        {
            this.request = protocolRequest;
            UpdateViewWithRequestValues();
        }

        protected void UpdateViewWithRequestValues()
        {
            this.view.RequestedBy = request.RequestedBy;
            this.view.RequestedDate = request.RequestedDate.ToString("MM/dd/yyyy");
            this.view.Guidelines = request.Guidelines;
            this.view.Compliance = request.Compliance;
            this.view.ProtocolType = request.ProtocolType;
            this.view.BillTo = request.BillTo;
            this.view.SendVia = request.SendVia;
            this.view.DueDate = request.DueDate;
            this.view.Comments = request.Comments;
            this.view.AssignedTo = request.AssignedTo;

            UpdateViewWithSponsorContact();
        }

        protected void UpdateViewWithSponsorContact()
        {
            this.view.ContactName = request.Contact.ContactName;
            this.view.SponsorName = request.Contact.SponsorName;
            this.view.Email = request.Contact.Email;
            this.view.Address = request.Contact.Address;
            this.view.City = request.Contact.City;
            this.view.State = request.Contact.State;
            this.view.ZipCode = request.Contact.ZipCode;
            this.view.PhoneNumber = request.Contact.PhoneNumber;
            this.view.FaxNumber = request.Contact.FaxNumber;
            this.view.PONumber = request.Contact.PONumber;
        }

        public void UpdateRequestWithViewValues()
        {
            this.request.DueDate = this.view.DueDate;
            this.request.SendVia = this.view.SendVia;
            this.request.BillTo = this.view.BillTo;
            this.request.Comments = this.view.Comments;
        }

        public void GuidelinesButtonClicked()
        {
            IList items = QListItems.SelectItems(OptionFields.Guidelines.ToString());
            List<string> selectedItems = TemplatesController.ShowCheckBoxOptionsForm(items, view.ParentControl);
            if (selectedItems.Count != 0)
            {
                string itemsString = String.Join(", ", selectedItems);
                this.request.Guidelines = itemsString;
                this.view.Guidelines = itemsString;
            }
        }

        public void ComplianceButtonClicked()
        {
            IList items = QListItems.SelectItems(OptionFields.Compliance.ToString());
            Item selectedItem = TemplatesController.ShowListBoxOptionsForm(items, view.ParentControl);
            if(selectedItem.Name != "")
            {
                this.request.Compliance = selectedItem.Value;
                this.view.Compliance = selectedItem.Value;
            }
        }

        public void ProtocolTypeButtonClicked()
        {
            IList items = QListItems.SelectItems(OptionFields.ProtocolType.ToString());
            Item selectedItem = TemplatesController.ShowListBoxOptionsForm(items, view.ParentControl);
            if (selectedItem.Value != "")
            {
                this.request.ProtocolType = selectedItem.Value;
                this.view.ProtocolType = selectedItem.Value;
            }
        }

        public void AssignedToButtonClicked()
        {
            IList items = QUsers.SelectUsersByRoleID(UserRoles.DocControl);
            Item selectedItem = TemplatesController.ShowListBoxOptionsForm(items, view.ParentControl);
            if (selectedItem.Value != "")
            {
                this.request.AssignedTo = selectedItem.Value;
                this.view.AssignedTo = selectedItem.Text;
            }
        }

        public void ChangeContactButtonClicked()
        {
            IList items = QMatrix.GetSponsorContacts_NameAndCodeOnly(this.request.Contact.SponsorName);
            Item selectedItem = TemplatesController.ShowListBoxOptionsForm(items, view.ParentControl);
            if (selectedItem.Value != "")
            {
                this.request.SetContact(selectedItem.Value);
                UpdateViewWithSponsorContact();
            }
        }

        public void SubmitRequest()
        {
            this.request.RequestStatus = RequestStatuses.New;
            this.request.ID = QProtocolRequests.InsertItem(this.request, loginInfo.UserName);
            if (this.request.Titles.Count > 0)
            {
                QProtocolTitles.InsertItem(this.request, loginInfo.UserName);
            }
            QProtocolActivities.InsertItem(this.request, loginInfo.UserName);
        }

        public void UpdateRequest()
        {
            QProtocolRequests.UpdateItem(this.request, loginInfo.UserName);
        }

        public void ClearForm()
        {
            this.request.Refresh();
            this.view.ClearView();
        }
    }
}