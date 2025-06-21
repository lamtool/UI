using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.View.Forms.Actions;
using Sunny.UI;

namespace Sunny.Subdy.UI.View.Forms
{
    public partial class fListActionFacebook : Form
    {
    
        private Script _script;
        private ScriptActionContext _scriptActionContext;
        public fListActionFacebook(Script script)
        {
            InitializeComponent();
            List<Control> controls = ControlHelper.GetControls(this);
            foreach (Control control in controls)
            {
                if (control is UIButton)
                {
                    UIButton clickedButton = (UIButton)control;
                    clickedButton.Click += buttonGetText_Click;
                }
            }
            _script = script;
            _scriptActionContext = new ScriptActionContext();
           
        }
        private void buttonGetText_Click(object sender, EventArgs e)
        {
            if (sender is UIButton)
            {
                UIButton clickedButton = (UIButton)sender;
                if (clickedButton == null) { return; }
                string text = clickedButton.Name.Trim().Replace("btn_", "");
                if (string.IsNullOrEmpty(text)) { return; }
                OpenForm(text);

            }
        }
        private void OpenForm(string text)
        {
            string config = string.Empty;
            string name = string.Empty;
            switch (text)
            {
                case TypeAction.FB_SpamXu:
                    {
                        fAction_SpamXu form = new fAction_SpamXu(TypeAction.GetNameAction(text), string.Empty);
                        form.ShowDialog();
                        config = form._Json;
                        name = form._Name.Trim();
                        break;
                    }
            }
            if (string.IsNullOrEmpty(config))
            {
                return;
            }
            ScriptAction action = new ScriptAction
            {
                Id = Guid.NewGuid(),
                Name = name,
                ScriptId = _script.Id,
                Json = config,
                Type = text
            };
            if (!_scriptActionContext.Add(action))
            {
                CommonMethod.ShowMessageError("Lỗi khi thêm hành động vào kịch bản", "Lỗi");
                return;
            }
            _script.Config = _script.Config + "|" + action.Id.ToString();
            new ScriptContext().Update(_script);
            Close();
        }
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
