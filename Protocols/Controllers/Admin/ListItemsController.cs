﻿using Toxikon.ProtocolManager.Interfaces.Admin;
using Toxikon.ProtocolManager.Models;
using Toxikon.ProtocolManager.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toxikon.ProtocolManager.Controllers.Admin
{
    public class ListItemsController
    {
        IListItemsView view;
        IList listNames;
        string selectedListName;

        public ListItemsController(IListItemsView view)
        {
            this.view = view;
            this.view.SetController(this);
            listNames = new ArrayList() { ListNames.Guidelines, ListNames.Compliance, ListNames.ProtocolType};
            this.selectedListName = "";
        }

        public void LoadView()
        {
            foreach(string listName in listNames)
            {
                this.view.AddListNameToView(listName);
            }
        }

        public void ListNameComboBoxSelectedIndexChanged(int selectedIndex)
        {
            if(selectedIndex > -1 && selectedIndex < this.listNames.Count)
            {
                this.selectedListName = listNames[selectedIndex] as string;
                LoadSelectedListNameItems();
            }
        }

        private void LoadSelectedListNameItems()
        {
            view.ClearListItems();
            IList listItems = QListItems.GetListItems(this.selectedListName);
            AddListItemsToView(listItems);
        }

        private void AddListItemsToView(IList items)
        {
            foreach(ListItem item in items)
            {
                view.AddListItemToView(item.ItemName);
            }
        }

        public void AddButtonClicked()
        {
            if(this.selectedListName != "" && view.ItemName.Trim() != "")
            {
                ListItem item = new ListItem();
                item.ListName = this.selectedListName;
                item.ItemName = view.ItemName;
                item.IsActive = true;

                LoginInfo loginInfo = LoginInfo.GetInstance();
                QListItems.InsertListItem(item, loginInfo.UserName);
                view.ClearNewItemTextBox();
                LoadSelectedListNameItems();
            }
        }
    }
}
