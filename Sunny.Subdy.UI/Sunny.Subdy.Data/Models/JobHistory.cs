namespace Sunny.Subdy.Data.Models
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using System.Threading;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public class JobHistory : INotifyPropertyChanged
    {
        private Guid _id = Guid.NewGuid();
        private string? _uid = string.Empty;
        private string? _platform = string.Empty;

        // Action count
        private double? _like = 0, _love = 0, _care = 0, _wow = 0, _haha = 0, _angry = 0, _sad = 0;
        private double? _share = 0, _follow = 0, _likePage = 0, _joinGroup = 0, _likeComment = 0;
        private double? _total = 0;
        private double? _like_Xu = 0, _love_Xu = 0, _care_Xu = 0, _wow_Xu = 0, _haha_Xu = 0, _angry_Xu = 0, _sad_Xu = 0;
        private double? _share_Xu = 0, _follow_Xu = 0, _likePage_Xu = 0, _joinGroup_Xu = 0, _likeComment_Xu = 0;
        private double? _total_Xu = 0;


        // Skip counts
        private double? _like_skip = 0, _love_skip = 0, _care_skip = 0, _wow_skip = 0, _haha_skip = 0, _angry_skip = 0, _sad_skip = 0;
        private double? _share_skip = 0, _follow_skip = 0, _likePage_skip = 0, _joinGroup_skip = 0, _likeComment_skip = 0;
        private double? _total_skip = 0;

        private string? _dateTime = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler == null) return;

            var context = SynchronizationContext.Current;
            if (context != null)
            {
                context.Post(_ => handler(this, new PropertyChangedEventArgs(propertyName)), null);
            }
            else
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Sample property with notification
        public Guid Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string? Uid
        {
            get => _uid;
            set { _uid = value; OnPropertyChanged(); }
        }
        public string? Platform
        {
            get => _platform;
            set { _platform = value; OnPropertyChanged(); }
        }
        public double? Like
        {
            get => _like;
            set { _like = value; OnPropertyChanged(); }
        }

        public double? Love
        {
            get => _love;
            set { _love = value; OnPropertyChanged(); }
        }

        public double? Care
        {
            get => _care;
            set { _care = value; OnPropertyChanged(); }
        }

        public double? Wow
        {
            get => _wow;
            set { _wow = value; OnPropertyChanged(); }
        }

        public double? Haha
        {
            get => _haha;
            set { _haha = value; OnPropertyChanged(); }
        }

        public double? Angry
        {
            get => _angry;
            set { _angry = value; OnPropertyChanged(); }
        }

        public double? Sad
        {
            get => _sad;
            set { _sad = value; OnPropertyChanged(); }
        }

        public double? Share
        {
            get => _share;
            set { _share = value; OnPropertyChanged(); }
        }

        public double? Follow
        {
            get => _follow;
            set { _follow = value; OnPropertyChanged(); }
        }

        public double? LikePage
        {
            get => _likePage;
            set { _likePage = value; OnPropertyChanged(); }
        }

        public double? JoinGroup
        {
            get => _joinGroup;
            set { _joinGroup = value; OnPropertyChanged(); }
        }

        public double? LikeComment
        {
            get => _likeComment;
            set { _likeComment = value; OnPropertyChanged(); }
        }

        public double? Total
        {
            get => _total;
            set { _total = value; OnPropertyChanged(); }
        }

        public double? Like_Xu
        {
            get => _like_Xu;
            set { _like_Xu = value; OnPropertyChanged(); }
        }

        public double? Love_Xu
        {
            get => _love_Xu;
            set { _love_Xu = value; OnPropertyChanged(); }
        }

        public double? Care_Xu
        {
            get => _care_Xu;
            set { _care_Xu = value; OnPropertyChanged(); }
        }

        public double? Wow_Xu
        {
            get => _wow_Xu;
            set { _wow_Xu = value; OnPropertyChanged(); }
        }

        public double? Haha_Xu
        {
            get => _haha_Xu;
            set { _haha_Xu = value; OnPropertyChanged(); }
        }

        public double? Angry_Xu
        {
            get => _angry_Xu;
            set { _angry_Xu = value; OnPropertyChanged(); }
        }

        public double? Sad_Xu
        {
            get => _sad_Xu;
            set { _sad_Xu = value; OnPropertyChanged(); }
        }

        public double? Share_Xu
        {
            get => _share_Xu;
            set { _share_Xu = value; OnPropertyChanged(); }
        }

        public double? Follow_Xu
        {
            get => _follow_Xu;
            set { _follow_Xu = value; OnPropertyChanged(); }
        }

        public double? LikePage_Xu
        {
            get => _likePage_Xu;
            set { _likePage_Xu = value; OnPropertyChanged(); }
        }

        public double? JoinGroup_Xu
        {
            get => _joinGroup_Xu;
            set { _joinGroup_Xu = value; OnPropertyChanged(); }
        }

        public double? LikeComment_Xu
        {
            get => _likeComment_Xu;
            set { _likeComment_Xu = value; OnPropertyChanged(); }
        }

        public double? Total_Xu
        {
            get => _total_Xu;
            set { _total_Xu = value; OnPropertyChanged(); }
        }

        public double? Like_Skip
        {
            get => _like_skip;
            set { _like_skip = value; OnPropertyChanged(); }
        }

        public double? Love_Skip
        {
            get => _love_skip;
            set { _love_skip = value; OnPropertyChanged(); }
        }

        public double? Care_Skip
        {
            get => _care_skip;
            set { _care_skip = value; OnPropertyChanged(); }
        }

        public double? Wow_Skip
        {
            get => _wow_skip;
            set { _wow_skip = value; OnPropertyChanged(); }
        }

        public double? Haha_Skip
        {
            get => _haha_skip;
            set { _haha_skip = value; OnPropertyChanged(); }
        }

        public double? Angry_Skip
        {
            get => _angry_skip;
            set { _angry_skip = value; OnPropertyChanged(); }
        }

        public double? Sad_Skip
        {
            get => _sad_skip;
            set { _sad_skip = value; OnPropertyChanged(); }
        }

        public double? Share_Skip
        {
            get => _share_skip;
            set { _share_skip = value; OnPropertyChanged(); }
        }

        public double? Follow_Skip
        {
            get => _follow_skip;
            set { _follow_skip = value; OnPropertyChanged(); }
        }

        public double? LikePage_Skip
        {
            get => _likePage_skip;
            set { _likePage_skip = value; OnPropertyChanged(); }
        }

        public double? JoinGroup_Skip
        {
            get => _joinGroup_skip;
            set { _joinGroup_skip = value; OnPropertyChanged(); }
        }

        public double? LikeComment_Skip
        {
            get => _likeComment_skip;
            set { _likeComment_skip = value; OnPropertyChanged(); }
        }

        public double? Total_Skip
        {
            get => _total_skip;
            set { _total_skip = value; OnPropertyChanged(); }
        }

        public string? DateTime
        {
            get => _dateTime;
            set { _dateTime = value; OnPropertyChanged(); }
        }
    }

}
