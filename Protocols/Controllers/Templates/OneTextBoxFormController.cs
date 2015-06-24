﻿using Toxikon.ProtocolManager.Interfaces.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Toxikon.ProtocolManager.Controllers.Templates
{
    public class OneTextBoxFormController
    {
        IOneTextBoxFormView view;
        public string TextBoxLabel { get; private set; }
        public string TextBoxValue { get; set; }

        public OneTextBoxFormController(IOneTextBoxFormView view, string textBoxlabel)
        {
            this.view = view;
            this.view.SetController(this);
            this.TextBoxLabel = textBoxlabel;
            this.TextBoxValue = "";
        }

        public void LoadView()
        {
            this.view.TextBoxLabel = this.TextBoxLabel;
            this.view.TextBoxValue = this.TextBoxValue;
        }

        public void SubmitButtonClicked()
        {
            this.TextBoxValue = this.view.TextBoxValue;
            view.SetDialogResult(DialogResult.OK);
        }
    }
}
