﻿using Toxikon.ProtocolManager.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toxikon.ProtocolManager.Models
{
    public class ProtocolRequest
    {
        public int ID { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedDate { get; set; }
        public SponsorContact Contact { get; private set; }
        public string Guidelines { get; set; }
        public string Compliance { get; set; }
        public string ProtocolType { get; set; }
        public List<ProtocolTitle> Titles { get; private set; }
        public DateTime DueDate { get; set; }
        public string SendVia { get; set; }
        public string BillTo { get; set; }
        public string Comments { get; set; }
        public string AssignedTo { get; set; }

        public string RequestStatus { get; set; }
        public bool IsActive { get; set; }

        public ProtocolRequest()
        {
            Refresh();
        }

        public void Refresh()
        {
            this.ID = 0;
            RequestedBy = "";
            RequestedDate = DateTime.Now;           
            this.Contact = new Models.SponsorContact();
            Guidelines = "";
            Compliance = "";
            ProtocolType = "";
            Titles = new List<ProtocolTitle>() { };
            DueDate = DateTime.Now;
            BillTo = "Toxikon";
            Comments = "";
            this.AssignedTo = "";
            this.IsActive = true;
        }

        public void SetContact(SponsorContact contact)
        {
            this.Contact.SponsorCode = contact.SponsorCode;
            this.Contact.ContactCode = contact.ContactCode;
            this.Contact.SponsorName = contact.SponsorName;
            this.Contact.ContactName = contact.ContactName;
            this.Contact.Address = contact.Address;
            this.Contact.City = contact.City;
            this.Contact.State = contact.State;
            this.Contact.ZipCode = contact.ZipCode;
            this.Contact.PhoneNumber = contact.PhoneNumber;
            this.Contact.FaxNumber = contact.FaxNumber;
            this.Contact.Email = contact.Email;
        }

        public void SetContact(string contactCode)
        {
            this.Contact.ContactCode = contactCode;
            this.Contact = QMatrix.GetSponsorByContactCode(contactCode);
        }

        public void SetTitles(List<string> titleDescriptions)
        {
            foreach(string item in titleDescriptions)
            {
                ProtocolTitle protocolTitle = new ProtocolTitle();
                protocolTitle.Description = item;
                this.Titles.Add(protocolTitle);
            }
        }

        public void SetTitles(List<ProtocolTitle> items)
        {
            this.Titles = new List<ProtocolTitle>(items);
        }

        public void RefreshProtocolTitles()
        {
            this.Titles.Clear();
            this.Titles = (List<ProtocolTitle>)QProtocolTitles.SelectItems(this.ID);
        }
       
        public void CloseRequest()
        {
            this.RequestStatus = RequestStatuses.Closed;
            this.IsActive = false;
            LoginInfo loginInfo = LoginInfo.GetInstance();
            QProtocolRequests.UpdateRequestStatus(this, loginInfo.UserName);
        }
    }
}