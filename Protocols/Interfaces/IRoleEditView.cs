﻿using Protocols.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Protocols.Interfaces
{
    public interface IRoleEditView
    {
        void SetController(RoleEditController controller);
        void SetIsActiveRadioButtonGroup_Enable(bool value);
        void SetDialogResult(DialogResult dialogResult);

        string RoleName { get; set; }
        bool IsActive { get; set; }
    }
}
