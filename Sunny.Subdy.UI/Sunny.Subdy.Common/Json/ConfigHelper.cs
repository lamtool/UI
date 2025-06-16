using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Logs;
using Sunny.UI;

namespace Sunny.Subdy.Common.Json
{
    public class ConfigHelper
    {
        JObject jConfig = new();
        Form form;
        UserControl uc;
        string configFile;
        List<DataGridView> dgvs = new();
        List<Control> excepts = new();
        Action? action = null;
        Action? actionClosing = null;
        bool exists = true;
        public ConfigHelper(Form form, string configFilename, List<Control>? excepts = null, Action? action = null, Action? actionClosing = null, bool exists = true)
        {
            this.form = form;
            this.exists = exists;
            if (excepts != null)
            {
                this.excepts = excepts;
            }
            this.action = action;
            this.actionClosing = actionClosing;
            FileHelper.CreateFolder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"configs"));
            configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"configs\\{configFilename}.json");
            if (File.Exists(configFile))
            {
                try
                {
                    jConfig = JObject.Parse(File.ReadAllText(configFile));
                }
                catch
                {
                    jConfig = new JObject();
                }
            }
            else
            {
                jConfig = new JObject();
            }
            form.Load += FormLoad;
            form.FormClosing += FormClosing;
            this.exists = exists;
        }
        public ConfigHelper(Form form, string jConfig)
        {
            this.form = form;
            if (excepts != null)
            {
                this.excepts = excepts;
            }

            if (!string.IsNullOrEmpty(jConfig))
            {
                try
                {
                    this.jConfig = JObject.Parse(jConfig);
                }
                catch
                {
                    this.jConfig = new JObject();
                }
            }
            else
            {
                this.jConfig = new JObject();
            }
            form.Load += FormLoad;
        }
        private void ReadFile()
        {
            if (File.Exists(configFile))
            {
                try
                {
                    jConfig = JObject.Parse(File.ReadAllText(configFile));
                }
                catch
                {
                    jConfig = new JObject();
                }
            }
            else
            {
                jConfig = new JObject();
            }
        }
        public ConfigHelper(UserControl uc, string configFilename, List<Control>? excepts = null, Action? action = null, Action? actionClosing = null, bool exists = false)
        {
            this.uc = uc;
            if (excepts != null)
            {
                this.excepts = excepts;
            }
            this.action = action;
            this.actionClosing = actionClosing;
            FileHelper.CreateFolder(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"configs"));
            configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"configs\\{configFilename}.json");
            if (File.Exists(configFile))
            {
                try
                {
                    jConfig = JObject.Parse(File.ReadAllText(configFile));
                }
                catch
                {
                    jConfig = new JObject();
                }
            }
            else
            {
                jConfig = new JObject();
            }

            uc.Load += UserControlLoad;
            // this.uc.ParentChanged += FormClosing;
            this.exists = exists;
        }
        private IConfigurableControl? GetAdapterFromSender(object sender)
        {
            return sender switch
            {
                CheckBox cb => new CheckBoxGetterAdapter(cb),
                TextBox tb => new TextBoxGetterAdapter(tb),
                ComboBox cb => new ComboBoxGetterAdapter(cb),
                UIComboBox uicb => new UIComboBoxGetterAdapter(uicb),
                UITextBox uitb => new UITextBoxGetterAdapter(uitb),
                NumericUpDown num => new NumericUpDownGetterAdapter(num),
                RadioButton rb => new RadioButtonGetterAdapter(rb),
                _ => null
            };
        }
        private void ValueChanged(object sender, EventArgs e)
        {
            ReadFile();
            try
            {
                var adapter = GetAdapterFromSender(sender);
                if (adapter != null && !string.IsNullOrWhiteSpace(adapter.Name))
                {
                    jConfig[adapter.Name] = JToken.FromObject(adapter.GetValue());
                    File.WriteAllText(configFile, jConfig.ToString());
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }
        public void UserControlLoad(object sender, EventArgs e)
        {
            List<Control> controls = ControlHelper.GetControls(uc);
            LoadControls(controls);
        }
        public void FormLoad(object sender, EventArgs e)
        {
            List<Control> controls = ControlHelper.GetControls(form);
            LoadControls(controls);
        }
        private IControlAdapter? CreateAdapter(Control control)
        {
            return control switch
            {
                TextBox tb => new TextBoxBinderAdapter(tb),
                UITextBox uitb => new UITextBoxBinderAdapter(uitb),
                CheckBox cb => new CheckBoxBinderAdapter(cb),
                UICheckBox uicb => new UICheckBoxBinderAdapter(uicb),
                ComboBox cb => new ComboBoxBinderAdapter(cb),
                UIComboBox uicb => new UIComboBoxBinderAdapter(uicb),
                RadioButton rb => new RadioButtonBinderAdapter(rb),
                NumericUpDown nud => new NumericUpDownBinderAdapter(nud),
                _ => null
            };
        }
        void LoadControls(List<Control> controls)
        {
            foreach (var control in controls)
            {
                if (control is DataGridView dgv)
                {
                    dgv.CurrentCellDirtyStateChanged += dgvCurrentCellDirtyStateChanged;
                    dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
            }

            foreach (var control in controls)
            {
                try
                {
                    if (string.IsNullOrEmpty(control.Name) || excepts.Contains(control))
                        continue;

                    var adapter = CreateAdapter(control);
                    if (adapter == null)
                        continue;

                    if (jConfig[adapter.Name] != null)
                    {
                        adapter.LoadValue(jConfig[adapter.Name]);
                    }

                    adapter.BindEvent(ValueChanged);
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                }
            }

            action?.Invoke();
        }
        public void dgvCurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).IsCurrentCellDirty)
            {
                ((DataGridView)sender).CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        public void FormClosing(object sender, EventArgs e)
        {
            try
            {
                List<Control> controls = form != null
                    ? ControlHelper.GetControls(form)
                    : uc != null ? ControlHelper.GetControls(uc) : new List<Control>();

                foreach (Control control in controls)
                {
                    try
                    {
                        IConfigurableControl? adapter = GetAdapterFromSender(control);
                        if (adapter != null && !string.IsNullOrWhiteSpace(adapter.Name))
                        {
                            object? value = adapter.GetValue();
                            if (value != null)
                            {
                                jConfig[adapter.Name] = JToken.FromObject(value);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.Error(ex);
                    }
                }

                File.WriteAllText(configFile, jConfig.ToString());

                actionClosing?.Invoke();

                if (exists)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }
       
        /// <summary>
        /// Thêm giá trị vào thuộc tính trong đối tượng JObject.
        /// </summary>
        /// <param name="key">Khóa (key) của thuộc tính cần thêm giá trị.</param>
        /// <param name="value">Giá trị cần thêm.</param>
        public void AddValue(string key, object value)
        {
            try
            {
                this.jConfig[key] = JToken.FromObject(value.ToString());
            }
            catch
            {
            }
        }
        public string GetJsonString()
        {
            List<Control> controls = new List<Control>();
            if (form != null)
            {
                controls = ControlHelper.GetControls(form);
            }
            else if (uc != null)
            {
                controls = ControlHelper.GetControls(uc);
            }
            foreach (Control control in controls)
            {
                try
                {
                    if (control is TextBox textBox)
                    {
                        if (control.Name != "")
                        {
                            jConfig[((TextBox)control).Name] = JToken.FromObject(((TextBox)control).Text);
                        }
                    }
                    else if (control is NumericUpDown numericUpDown)
                    {
                        if (((NumericUpDown)control).Name != "")
                        {
                            jConfig[((NumericUpDown)control).Name] = JToken.FromObject(((NumericUpDown)control).Value);
                        }
                    }
                    else if (control is TextBox TextBox)
                    {
                        if (((TextBox)control).Name != "")
                        {
                            jConfig[((TextBox)control).Name] = JToken.FromObject(((TextBox)control).Text);
                        }
                    }
                    else if (control is CheckBox checkBox)
                    {
                        if (((CheckBox)control).Name != "")
                        {
                            jConfig[((CheckBox)control).Name] = JToken.FromObject(((CheckBox)control).Checked);
                        }
                    }
                    else if (control is RadioButton radioButton)
                    {
                        if (((RadioButton)control).Name != "")
                        {
                            jConfig[((RadioButton)control).Name] = JToken.FromObject(((RadioButton)control).Checked);
                        }
                    }
                    else if (control is ComboBox)
                    {
                        if (((ComboBox)control).Name != "")
                        {
                            jConfig[((ComboBox)control).Name] = JToken.FromObject(((ComboBox)control).SelectedIndex);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                }
            }
            string result = "";
            try
            {
                result = this.jConfig.ToString().Replace("\r\n", "");
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
