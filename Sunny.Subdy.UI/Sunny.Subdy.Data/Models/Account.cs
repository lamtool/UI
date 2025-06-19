using System.ComponentModel;

namespace Sunny.Subdy.Data.Models
{
    public class Account : INotifyPropertyChanged
    {
        private Guid _id;
        private string? _uid = "";
        private string? _password = "";
        private string? _phone = "";
        private string? _towFA = "";
        private string? _cookie = "";
        private string? _token = "";
        private string? _proxy = "";
        private string? _email = "";
        private string? _passMail = "";
        private string? _userAgent = "";
        private string? _fullName = "";
        private string? _recentInteraction = "";
        private string? _state = "";
        private string? _status = "";
        private string? _result = "";
        private string? _emailAddress = "";
        private string? _userName = "";
        private string? _nameFolder = "";
        private bool _isView = true;
        private bool _checked = false;
        private bool _running = false;
        [AppDbContext.SqlKey]
        public Guid Id
        {
            get => _id;
            set { if (_id != value) { _id = value; OnPropertyChanged(nameof(Id)); } }
        }
        public string? Uid
        {
            get => _uid;
            set { if (_uid != value) { _uid = value; OnPropertyChanged(nameof(Uid)); } }
        }

        public string? Password
        {
            get => _password;
            set { if (_password != value) { _password = value; OnPropertyChanged(nameof(Password)); } }
        }

        public string? Phone
        {
            get => _phone;
            set { if (_phone != value) { _phone = value; OnPropertyChanged(nameof(Phone)); } }
        }

        public string? TowFA
        {
            get => _towFA;
            set { if (_towFA != value) { _towFA = value; OnPropertyChanged(nameof(TowFA)); } }
        }

        public string? Cookie
        {
            get => _cookie;
            set { if (_cookie != value) { _cookie = value; OnPropertyChanged(nameof(Cookie)); } }
        }

        public string? Token
        {
            get => _token;
            set { if (_token != value) { _token = value; OnPropertyChanged(nameof(Token)); } }
        }

        public string? Proxy
        {
            get => _proxy;
            set { if (_proxy != value) { _proxy = value; OnPropertyChanged(nameof(Proxy)); } }
        }

        public string? Email
        {
            get => _email;
            set { if (_email != value) { _email = value; OnPropertyChanged(nameof(Email)); } }
        }

        public string? PassMail
        {
            get => _passMail;
            set { if (_passMail != value) { _passMail = value; OnPropertyChanged(nameof(PassMail)); } }
        }

        public string? UserAgent
        {
            get => _userAgent;
            set { if (_userAgent != value) { _userAgent = value; OnPropertyChanged(nameof(UserAgent)); } }
        }

        public string? FullName
        {
            get => _fullName;
            set { if (_fullName != value) { _fullName = value; OnPropertyChanged(nameof(FullName)); } }
        }

        public string? RecentInteraction
        {
            get => _recentInteraction;
            set { if (_recentInteraction != value) { _recentInteraction = value; OnPropertyChanged(nameof(RecentInteraction)); } }
        }

        public string? State
        {
            get => _state;
            set { if (_state != value) { _state = value; OnPropertyChanged(nameof(State)); } }
        }

        public string? Status
        {
            get => _status;
            set { if (_status != value) { _status = value; OnPropertyChanged(nameof(Status)); } }
        }

        public string? Result
        {
            get => _result;
            set { if (_result != value) { _result = value; OnPropertyChanged(nameof(Result)); } }
        }

        public string? EmailAdress
        {
            get => _emailAddress;
            set { if (_emailAddress != value) { _emailAddress = value; OnPropertyChanged(nameof(EmailAdress)); } }
        }

        public string? UserName
        {
            get => _userName;
            set { if (_userName != value) { _userName = value; OnPropertyChanged(nameof(UserName)); } }
        }

        public string? NameFolder
        {
            get => _nameFolder;
            set { if (_nameFolder != value) { _nameFolder = value; OnPropertyChanged(nameof(NameFolder)); } }
        }
        public bool Checked
        {
            get => _checked;
            set { if (_checked != value) { _checked = value; OnPropertyChanged(nameof(Checked)); } }
        }
        public bool Running
        {
            get => _running;
            set { if (_running != value) { _running = value; OnPropertyChanged(nameof(Running)); } }
        }
        public bool IsView
        {
            get => _isView;
            set { if (_isView != value) { _isView = value; OnPropertyChanged(nameof(IsView)); } }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
