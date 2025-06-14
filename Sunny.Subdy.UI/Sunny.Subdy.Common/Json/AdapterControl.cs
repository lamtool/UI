using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunny.UI;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Sunny.Subdy.Common.Json
{
    public class CheckBoxGetterAdapter : IConfigurableControl
    {
        private readonly CheckBox control;
        public CheckBoxGetterAdapter(CheckBox c) => control = c;
        public string Name => control.Name;
        public object? GetValue() => control.Checked;
    }

    public class TextBoxGetterAdapter : IConfigurableControl
    {
        private readonly TextBox control;
        public TextBoxGetterAdapter(TextBox c) => control = c;
        public string Name => control.Name;
        public object? GetValue() => control.Text;
    }

    public class ComboBoxGetterAdapter : IConfigurableControl
    {
        private readonly ComboBox control;
        public ComboBoxGetterAdapter(ComboBox c) => control = c;
        public string Name => control.Name;
        public object? GetValue() => control.SelectedIndex;
    }

    public class UIComboBoxGetterAdapter : IConfigurableControl
    {
        private readonly UIComboBox control;
        public UIComboBoxGetterAdapter(UIComboBox c) => control = c;
        public string Name => control.Name;
        public object? GetValue() => control.SelectedIndex;
    }

    public class UITextBoxGetterAdapter : IConfigurableControl
    {
        private readonly UITextBox control;
        public UITextBoxGetterAdapter(UITextBox c) => control = c;
        public string Name => control.Name;
        public object? GetValue() => control.Text;
    }

    public class NumericUpDownGetterAdapter : IConfigurableControl
    {
        private readonly NumericUpDown control;
        public NumericUpDownGetterAdapter(NumericUpDown c) => control = c;
        public string Name => control.Name;
        public object? GetValue() => control.Value;
    }

    public class RadioButtonGetterAdapter : IConfigurableControl
    {
        private readonly RadioButton control;
        public RadioButtonGetterAdapter(RadioButton c) => control = c;
        public string Name => control.Name;
        public object? GetValue() => control.Checked;
    }

    // === Binder Adapters (LoadValue + BindEvent) ===
    public class TextBoxBinderAdapter : IControlAdapter
    {
        private readonly TextBox control;
        public TextBoxBinderAdapter(TextBox c) => control = c;
        public string Name => control.Name;
        public void LoadValue(JToken value) => control.Text = value.ToString();
        public void BindEvent(EventHandler handler) => control.TextChanged += handler;
    }

    public class UITextBoxBinderAdapter : IControlAdapter
    {
        private readonly UITextBox control;
        public UITextBoxBinderAdapter(UITextBox c) => control = c;
        public string Name => control.Name;
        public void LoadValue(JToken value) => control.Text = value.ToString();
        public void BindEvent(EventHandler handler) => control.TextChanged += handler;
    }

    public class CheckBoxBinderAdapter : IControlAdapter
    {
        private readonly CheckBox control;
        public CheckBoxBinderAdapter(CheckBox c) => control = c;
        public string Name => control.Name;
        public void LoadValue(JToken value) => control.Checked = value.ToObject<bool>();
        public void BindEvent(EventHandler handler) => control.CheckedChanged += handler;
    }

    public class UICheckBoxBinderAdapter : IControlAdapter
    {
        private readonly UICheckBox control;
        public UICheckBoxBinderAdapter(UICheckBox c) => control = c;
        public string Name => control.Name;
        public void LoadValue(JToken value) => control.Checked = value.ToObject<bool>();
        public void BindEvent(EventHandler handler) => control.CheckedChanged += handler;
    }

   
    public class ComboBoxBinderAdapter : IControlAdapter
    {
        private readonly ComboBox control;
        public ComboBoxBinderAdapter(ComboBox c) => control = c;
        public string Name => control.Name;
        public void LoadValue(JToken value) => control.SelectedIndex = value.ToObject<int>();
        public void BindEvent(EventHandler handler) => control.SelectedIndexChanged += handler;
    }

    public class UIComboBoxBinderAdapter : IControlAdapter
    {
        private readonly UIComboBox control;
        public UIComboBoxBinderAdapter(UIComboBox c) => control = c;
        public string Name => control.Name;
        public void LoadValue(JToken value) => control.SelectedIndex = value.ToObject<int>();
        public void BindEvent(EventHandler handler) => control.SelectedIndexChanged += handler;
    }

    public class RadioButtonBinderAdapter : IControlAdapter
    {
        private readonly RadioButton control;
        public RadioButtonBinderAdapter(RadioButton c) => control = c;
        public string Name => control.Name;
        public void LoadValue(JToken value) => control.Checked = value.ToObject<bool>();
        public void BindEvent(EventHandler handler) => control.CheckedChanged += handler;
    }

    public class NumericUpDownBinderAdapter : IControlAdapter
    {
        private readonly NumericUpDown control;
        public NumericUpDownBinderAdapter(NumericUpDown c) => control = c;
        public string Name => control.Name;
        public void LoadValue(JToken value) => control.Value = value.ToObject<decimal>();
        public void BindEvent(EventHandler handler) => control.ValueChanged += handler;
    }
}
