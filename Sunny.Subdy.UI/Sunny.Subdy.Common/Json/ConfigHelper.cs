using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Logs;
using Sunny.UI;

namespace Sunny.Subdy.Common.Json
{
    public class ConfigHelper
    {
        private readonly JObject jConfig = new();
        private readonly Form? form;
        private readonly UserControl? uc;
        private readonly string? configFile;
        private readonly List<Control> excepts;
        private readonly Action? onLoadAction;
        private readonly Action? onCloseAction;
        private readonly bool shouldExit;

        public ConfigHelper(Form form, string configFilename, List<Control>? excepts = null, Action? onLoad = null, Action? onClose = null, bool shouldExit = true)
        {
            this.form = form;
            this.excepts = excepts ?? new();
            this.onLoadAction = onLoad;
            this.onCloseAction = onClose;
            this.shouldExit = shouldExit;

            configFile = InitConfigFile(configFilename);
            LoadConfigFromFile();

            form.Load += ControlLoad;
            form.FormClosing += ControlClosing;
        }
        public ConfigHelper(Form form, string jsonString, bool isJsonString,  Action? onLoad = null, Action? onClose = null)
        {
            this.form = form;
            this.excepts = excepts ?? new();
            this.onLoadAction = onLoad;
            this.onCloseAction = onClose;
            this.shouldExit = false;
            LoadConfigFromFile(jsonString);

            form.Load += ControlLoad;
            form.FormClosing += ControlClosing;
        }
        public ConfigHelper(UserControl uc, string configFilename, List<Control>? excepts = null, Action? onLoad = null, Action? onClose = null)
        {
            this.uc = uc;
            this.excepts = excepts ?? new();
            this.onLoadAction = onLoad;
            this.onCloseAction = onClose;

            configFile = InitConfigFile(configFilename);
            LoadConfigFromFile();

            uc.Load += ControlLoad;
            uc.Disposed += ControlClosing;
        }

        private string InitConfigFile(string filename)
        {
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configs");
            FileHelper.CreateFolder(folder);
            return Path.Combine(folder, $"{filename}.json");
        }
        private void LoadConfigFromFile(string content)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var parsed = JObject.Parse(content);
                    foreach (var prop in parsed)
                        jConfig[prop.Key] = prop.Value;
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }
        private void LoadConfigFromFile()
        {
            try
            {
                if (File.Exists(configFile))
                {
                    var content = File.ReadAllText(configFile!);
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        var parsed = JObject.Parse(content);
                        foreach (var prop in parsed)
                            jConfig[prop.Key] = prop.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        private void ControlLoad(object? sender, EventArgs e)
        {
            var controls = GetAllControls();
            foreach (var control in controls)
            {
                if (string.IsNullOrEmpty(control.Name) || excepts.Contains(control))
                    continue;

                var adapter = CreateBinderAdapter(control);
                if (adapter == null)
                    continue;

                if (jConfig.TryGetValue(adapter.Name, out var value))
                {
                    try { adapter.LoadValue(value); } catch (Exception ex) { LogManager.Error(ex); }
                }

                adapter.BindEvent(ValueChanged);
            }

            onLoadAction?.Invoke();
        }

        private void ControlClosing(object? sender, EventArgs e)
        {
            SaveAllControlValues();

            if (!string.IsNullOrEmpty(configFile))
            {
                try { File.WriteAllText(configFile!, jConfig.ToString()); } catch (Exception ex) { LogManager.Error(ex); }
            }

            onCloseAction?.Invoke();

            if (shouldExit && form != null)
                Environment.Exit(0);
        }

        private void SaveAllControlValues()
        {
            var controls = GetAllControls();
            foreach (var control in controls)
            {
                if (string.IsNullOrEmpty(control.Name) || excepts.Contains(control))
                    continue;

                var adapter = CreateGetterAdapter(control);
                if (adapter == null)
                    continue;

                try
                {
                    var value = adapter.GetValue();
                    if (value != null)
                        jConfig[adapter.Name] = JToken.FromObject(value);
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                }
            }
        }

        private void ValueChanged(object? sender, EventArgs e)
        {
            if (sender is not Control control || string.IsNullOrEmpty(control.Name) || excepts.Contains(control))
                return;

            var adapter = CreateGetterAdapter(control);
            if (adapter == null)
                return;

            try
            {
                var value = adapter.GetValue();
                if (value != null)
                {
                    jConfig[adapter.Name] = JToken.FromObject(value);
                    if (!string.IsNullOrEmpty(configFile))
                        File.WriteAllText(configFile!, jConfig.ToString());
                }
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
        }

        private List<Control> GetAllControls()
        {
            return form != null ? ControlHelper.GetControls(form) : uc != null ? ControlHelper.GetControls(uc) : new();
        }

        private IControlAdapter? CreateBinderAdapter(Control control)
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
                UIRadioButton uirb => new UIRadioButtonBinderAdapter(uirb),
                NumericUpDown nud => new NumericUpDownBinderAdapter(nud),
                UITimePicker picker => new UITimePickerBinderAdapter(picker),
                _ => null
            };
        }

        private IConfigurableControl? CreateGetterAdapter(Control control)
        {
            return control switch
            {
                TextBox tb => new TextBoxGetterAdapter(tb),
                UITextBox uitb => new UITextBoxGetterAdapter(uitb),
                CheckBox cb => new CheckBoxGetterAdapter(cb),
                UICheckBox uicb => new UICheckBoxGetterAdapter(uicb),
                ComboBox cb => new ComboBoxGetterAdapter(cb),
                UIComboBox uicb => new UIComboBoxGetterAdapter(uicb),
                RadioButton rb => new RadioButtonGetterAdapter(rb),
                UIRadioButton uirb => new UIRadioButtonGetterAdapter(uirb),
                NumericUpDown nud => new NumericUpDownGetterAdapter(nud),
                UITimePicker picker => new UITimePickerGetterAdapter(picker),
                _ => null
            };
        }

        public void AddValue(string key, object value)
        {
            try { jConfig[key] = JToken.FromObject(value); }
            catch (Exception ex) { LogManager.Error(ex); }
        }

        public string GetJsonString()
        {
            SaveAllControlValues();
            return jConfig.ToString(Formatting.None);
        }
    }

}
