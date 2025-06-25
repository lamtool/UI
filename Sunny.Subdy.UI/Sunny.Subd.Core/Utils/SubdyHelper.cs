using System.Text;

namespace Sunny.Subd.Core.Utils
{
    public class SubdyHelper
    {
        private static readonly Random _random = new Random();
        public static List<string> Shuffle(List<string> inputList, int count = -1)
        {
            List<string> shuffledList = new List<string>(inputList);
            int n = shuffledList.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                string temp = shuffledList[i];
                shuffledList[i] = shuffledList[j];
                shuffledList[j] = temp;
            }
            if (count == -1 || count > n)
            {
                count = n;
            }

            return shuffledList.Take(count).ToList();
        }
        public static int RandomValue(double min, double max)
        {
            if (min > max)
            {
                return _random.Next(Convert.ToInt32(max), Convert.ToInt32(min));
            }
            return _random.Next(Convert.ToInt32(min), Convert.ToInt32(max));
        }
        public static string RandomString(string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", int length = 0)
        {
            if (length == 0)
            {
                length = _random.Next(5, 50);
            }
            return new string(Enumerable.Repeat(chars, length)
               .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
        public static string RandomPassword(int length = 12, bool word = true, bool digit = true, bool special = true)
        {
            if (length == 0)
            {
                return string.Empty;
            }
            string s = "";
            if (word)
            {
                s += "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }
            if (digit)
            {
                s += "1234567890";
            }
            if (special)
            {
                s += "!@#$%^&*()_+";
            }
            if (s.Length == 0)
            {
                s = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }
            StringBuilder res = new StringBuilder();
            while (0 < length--)
            {
                res.Append(s[_random.Next(s.Length)]);
            }
            return res.ToString();
        }
        public static string GetStringRandom(List<string> lines)
        {
            return lines[_random.Next(lines.Count)];
        }

        public static readonly List<string> FirstnameVN = new List<string>
{
    "Nguyễn",
    "Trần",
    "Lê",
    "Phạm",
    "Hoàng",
    "Huỳnh",
    "Phan",
    "Vũ",
    "Võ",
    "Đặng",
    "Bùi",
    "Đỗ",
    "Hồ",
    "Ngô",
    "Dương",
    "Lý",
    "Cao",
    "Đinh",
    "Lưu",
    "Trương",
    "Tạ",
    "Mai",
    "Đào",
    "Nguyễn",
    "Trần",
    "Lê",
    "Phạm",
    "Hoàng",
    "Huỳnh",
    "Phan",
    "Vũ",
    "Võ",
    "Đặng",
    "Bùi",
    "Đỗ",
    "Hồ",
    "Ngô",
    "Dương",
    "Lý",
    "Cao",
    "Đinh",
    "Lưu",
    "Trương",
    "Khổng",
    "Quách",
    "Tô",
    "Lương",
    "Châu",
    "Tống",
    "Hàn",
    "Thái",
    "Kiều",
    "Tăng",
    "Mạc",
    "Triệu",
    "La",
    "Vương",
    "Uông",
    "Vi",
    "Từ",
    "Thạch"

};
        public static readonly List<string> LastnameVN = new List<string>
        {
     "Diệu Hương",
    "Khánh Ngân",
    "Ngọc Anh",
    "Thanh Hương",
    "Phương Anh",
    "Tuyết Mai",
    "Bảo Ngọc",
    "Thùy Linh",
    "Thu Trang",
    "Thảo Vy",
    "Minh Châu",
    "Quỳnh Anh",
    "Lan Chi",
    "Hồng Nhung",
    "Hoài An",
    "Mỹ Duyên",
    "Hải Yến",
    "Kim Ngân",
    "Nhật Lệ",
    "Ánh Tuyết",
    "Minh Quân",
    "Đức Anh",
    "Huy Hoàng",
    "Tuấn Anh",
    "Quang Minh",
    "Bảo Long",
    "Gia Huy",
    "Khánh Duy",
    "Anh Vũ",
    "Thanh Tùng",
    "Văn Khánh",
    "Hoàng Nam",
    "Công Thành",
    "Chí Bảo",
    "Hồng Phúc",
    "Duy Khang",
    "Tấn Phát",
    "Thành Đạt",
    "Việt Hoàng",
    "Anh Khoa",
    "Văn Thanh",
    "Minh Tuấn",
    "Quốc Huy",
    "Hoàng Nam",
    "Đức Hưng",
    "Thanh Tùng",
    "Minh Hùng",
    "Quang Duy",
    "Huy Hoàng",
    "Anh Tuấn",
    "Khắc Kiên",
    "Tấn Tài",
    "Quang Huy",
    "Trung Thành",
    "Duy Phương",
    "Tiến Thành",
    "Bảo Long",
    "Minh Khánh",
    "Khắc Tùng",
    "Đức Anh",
    "Thị Lan",
    "Thị Hương",
    "Thị Mai",
    "Thị Lan",
    "Thị Minh",
    "Thị Thu",
    "Thị Hồng",
    "Thị Bích",
    "Thị Kim",
    "Thị Yến",
    "Thị Thanh",
    "Thị Hồng",
    "Thị Phương",
    "Thị Thùy",
    "Thị Vân",
    "Thị Lan",
    "Thị Như",
    "Thị Bảo",
    "Thị Thu",
    "Thị Kim",
    "Vũ Minh",
    "Tùng Anh",
    "Bảo Trân",
    "Thanh Mai",
    "Quỳnh Anh",
    "Hồng Nhung",
    "Hải Yến",
    "Kim Ngọc",
    "Thảo Vy",
    "Quỳnh Chi",
    "Ngọc Anh",
    "Thanh Hương",
    "Phương Anh",
    "Tuyết Mai",
    "Bảo Ngọc",
    "Thùy Linh",
    "Thu Trang",
    "Thảo Vy",
    "Minh Châu",
    "Quỳnh Anh",
    "Lan Chi",
    "Hồng Nhung",
    "Hoài An",
    "Mỹ Duyên",
    "Hải Yến",
    "Kim Ngân",
    "Nhật Lệ",
    "Ánh Tuyết",
    "Quỳnh Lan",
    "Hoàng My",
    "Ngọc Lan",
    "Ngọc Hân",
    "Mai Anh",
    "Hồng Ngọc",
    "Thúy Quỳnh",
    "Thảo Nguyên",
    "Bích Ngọc",
    "Thanh Ngân",
    "Thư Kỳ",
    "Thiên Kim",
    "Hoàng Oanh",
    "Ngọc Liên",
    "Thảo Linh",
    "Thanh Bình",
    "Diễm My",
    "Diệu Linh",
    "Tường Vy",
    "Hoàng Phương",
    "Hà Linh",
    "Nguyễn Thanh",
    "Ngọc Diệp",
    "Hoàng Như",
    "Thúy Vân",
    "Tường Vi",
    "Quỳnh Mai",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Văn",
    "Ngọc",
    "Thanh",
    "Hữu",
    "Minh",
    "Anh",
    "Tuấn",
    "Quốc",
    "Đức",
    "Gia",
    "Trọng",
    "Phúc",
    "Khánh",
    "Xuân",
    "Bảo",
    "Duy",
    "Công",
    "Chí",
    "Tấn",
    "Nhật",
    "Huy",
    "Kim",
    "Mỹ",
    "Phương",
    "Tuyết",
    "Quỳnh",
    "Hải",
    "Lam",
    "Thiên",
    "Diệu",
    "Hoàng",
    "Trung",
    "Anh",
    "Bảo",
    "Cường",
    "Đức",
    "Duy",
    "Hoàng",
    "Huy",
    "Hùng",
    "Khánh",
    "Khoa",
    "Long",
    "Minh",
    "Nam",
    "Nguyên",
    "Phúc",
    "Quang",
    "Sơn",
    "Tâm",
    "Thắng",
    "Thành",
    "Thiện",
    "Tiến",
    "Toàn",
    "Trung",
    "Tuấn",
    "Việt",
    "Vinh",
    "An",
    "Anh",
    "Bích",
    "Chi",
    "Diệp",
    "Dung",
    "Duyên",
    "Giang",
    "Hạnh",
    "Hoa",
    "Hương",
    "Khánh",
    "Lan",
    "Linh",
    "Ly",
    "Mai",
    "My",
    "Ngọc",
    "Nhung",
    "Nhi",
    "Oanh",
    "Phương",
    "Quỳnh",
    "Thảo",
    "Thư",
    "Thùy",
    "Trang",
    "Tuyết",
    "Vy",
    "Yến",
    "Diệu Hương",
    "Khánh Ngân",
    "Ngọc Anh",
    "Thanh Hương",
    "Phương Anh",
    "Tuyết Mai",
    "Bảo Ngọc",
    "Thùy Linh",
    "Thu Trang",
    "Thảo Vy",
    "Minh Châu",
    "Quỳnh Anh",
    "Lan Chi",
    "Hồng Nhung",
    "Hoài An",
    "Mỹ Duyên",
    "Hải Yến",
    "Kim Ngân",
    "Nhật Lệ",
    "Ánh Tuyết",
    "Minh Quân",
    "Đức Anh",
    "Huy Hoàng",
    "Tuấn Anh",
    "Quang Minh",
    "Bảo Long",
    "Gia Huy",
    "Khánh Duy",
    "Anh Vũ",
    "Thanh Tùng",
    "Văn Khánh",
    "Hoàng Nam",
    "Công Thành",
    "Chí Bảo",
    "Hồng Phúc",
    "Duy Khang",
    "Tấn Phát",
    "Thành Đạt",
    "Việt Hoàng",
    "Anh Khoa",
    "Văn Thanh",
    "Minh Tuấn",
    "Quốc Huy",
    "Hoàng Nam",
    "Đức Hưng",
    "Thanh Tùng",
    "Minh Hùng",
    "Quang Duy",
    "Huy Hoàng",
    "Anh Tuấn",
    "Khắc Kiên",
    "Tấn Tài",
    "Quang Huy",
    "Trung Thành",
    "Duy Phương",
    "Tiến Thành",
    "Bảo Long",
    "Minh Khánh",
    "Khắc Tùng",
    "Đức Anh",
    "Thị Lan",
    "Thị Hương",
    "Thị Mai",
    "Thị Lan",
    "Thị Minh",
    "Thị Thu",
    "Thị Hồng",
    "Thị Bích",
    "Thị Kim",
    "Thị Yến",
    "Thị Thanh",
    "Thị Hồng",
    "Thị Phương",
    "Thị Thùy",
    "Thị Vân",
    "Thị Lan",
    "Thị Như",
    "Thị Bảo",
    "Thị Thu",
    "Thị Kim",
    "Vũ Minh",
    "Tùng Anh",
    "Bảo Trân",
    "Thanh Mai",
    "Quỳnh Anh",
    "Hồng Nhung",
    "Hải Yến",
    "Kim Ngọc",
    "Thảo Vy",
    "Quỳnh Chi",
    "Ngọc Anh",
    "Thanh Hương",
    "Phương Anh",
    "Tuyết Mai",
    "Bảo Ngọc",
    "Thùy Linh",
    "Thu Trang",
    "Thảo Vy",
    "Minh Châu",
    "Quỳnh Anh",
    "Lan Chi",
    "Hồng Nhung",
    "Hoài An",
    "Mỹ Duyên",
    "Hải Yến",
    "Kim Ngân",
    "Nhật Lệ",
    "Ánh Tuyết",
    "Quỳnh Lan",
    "Hoàng My",
    "Ngọc Lan",
    "Ngọc Hân",
    "Mai Anh",
    "Hồng Ngọc",
    "Thúy Quỳnh",
    "Thảo Nguyên",
    "Bích Ngọc",
    "Thanh Ngân",
    "Thư Kỳ",
    "Thiên Kim",
    "Hoàng Oanh",
    "Ngọc Liên",
    "Thảo Linh",
    "Thanh Bình",
    "Diễm My",
    "Diệu Linh",
    "Tường Vy",
    "Hoàng Phương",
    "Hà Linh",
    "Nguyễn Thanh",
    "Ngọc Diệp",
    "Hoàng Như",
    "Thúy Vân",
    "Tường Vi",
    "Quỳnh Mai",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
    "Hoài Thương",
    "Mỹ Linh",
    "Hải Đăng",
    "Kim Anh",
    "Nhật Minh",
    "Ánh Dương",
    "Minh Tâm",
    "Đức Huy",
    "Huyền Trang",
    "Tuấn Kiệt",
    "Quang Huy",
    "Gia Bảo",
    "Khánh Linh",
    "Anh Thư",
    "Thanh Bình",
    "Diễm Hương",
    "Diệu Thảo",
    "Tường An",
    "Hoàng Dương",
    "Hà My",
    "Nguyễn Hương",
    "Ngọc Diễm",
    "Hoàng Lan",
    "Thúy Hằng",
    "Tường Vi",
    "Quỳnh Như",
    "Diễm Quỳnh",
    "Thanh Nhàn",
    "Thảo Hương",
    "Tâm Anh",
    "Bảo Tuyết",
    "Linh Chi",
    "Kim Liên",
    "Thành Hưng",
    "Bích Liên",
    "Trúc Ly",
    "Linh Trang",
    "Diệu Hương",
    "Kim Liên",
    "Thùy Linh",
    "Bích Tuyết",
    "Thu Thủy",
    "Quỳnh Dương",
    "Đoàn Thu",
    "Như Quỳnh",
    "Bích Thu",
    "Diệu Duyên",
    "Hữu Phước",
    "Minh Khang",
    "Ngọc Bích",
    "Thanh Tâm",
    "Phương Thảo",
    "Tuyết Lan",
    "Bảo Trân",
    "Thùy Dung",
    "Thu Hương",
    "Thảo Nhi",
    "Minh Thư",
    "Quỳnh Trang",
    "Lan Hương",
    "Hồng Hạnh",
        };
        public static readonly List<string> LastnameRandom = new List<string>
        {
             "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
  "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin",
  "Lee", "Perez", "Thompson", "White", "Harris",
  "Do", "Nguyen", "Tran", "Le", "Pham", "Huynh", "Vo", "Dinh", "Hoang", "Lam", "Phan", "Bui", "Đang",
  "Suzuki", "Sato", "Takahashi", "Tanaka", "Watanabe", "Yamamoto", "Kobayashi", "Yoshida", "Yamada", "Sasaki",
  "Wang", "Li", "Zhang", "Liu", "Chen", "Yang", "Huang", "Zhao", "Wu", "Zhou",
  "Kim", "Lee", "Park", "Choi", "Jung", "Kang", "Jo", "Yoon", "Jang", "Han",
  "Khan", "Ali", "Singh", "Ahmed", "Kumar",
  "Garcia", "Martinez", "Rodriguez", "Hernandez", "Gonzalez", "Lopez", "Perez", "Sanchez", "Ramirez", "Torres",
  "Rahman", "Uddin", "Begum", "Hossain", "Islam",
  "Tan", "Zhou", "Xu", "Ma",
  "Inoue", "Nakamura", "Saito", "Hashimoto",
  "Jung", "Kang", "Oh", "Jang",
  "Akter", "Chowdhury", "Hasan", "Alam", "Mahmud", "Biswas", "Das", "Saha", "Sarkar", "Bhuiyan",
  "Al-Ghamdi", "Al-Qahtani", "Al-Dossari", "Al-Harbi", "Al-Jaziri", "Al-Maktoum", "Al-Nahyan", "Al-Saud", "Al-Hashimi", "Al-Farsi",
  "Al-Zahrani", "Al-Shammar", "Al-Otaibi", "Al-Khalifa", "Al-Thani", "Al-Sabah", "Al-Sharif", "Al-Khoury", "Daoud", "Haddad",
  "Miller", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez",
  "Thompson", "White", "Harris", "Clark", "Lewis", "Robinson", "Walker", "Hall", "Young", "Allen",
  "Wright", "King", "Green", "Baker", "Adams", "Nelson", "Hill", "Roberts", "Campbell", "Stewart",
  "Collins", "Bailey", "Reed", "Kelly", "Howard", "Gray", "Cox", "Ford", "Perry", "Bennett",
  "Wood", "Jenkins", "Barnes", "Sanders", "Ross", "Morales", "Griffin", "Gutierrez", "Ruiz", "Diaz",
  "Peterson", "Fisher", "Hayes", "Long", "Reynolds", "James", "Murray", "Wagner", "Cole", "Sullivan",
  "Freeman", "Webb", "Tucker", "Jordan", "Rogers", "Crawford", "Nichols", "Monroe", "Mendoza", "Ferguson",
  "Simpson", "Hudson", "Hanson", "Arnold", "Atkins", "Little", "Weaver", "Francis", "Henry", "Curtis",
  "Stevens", "Hoffman", "Hunter", "Nash", "Gilbert", "Garrett", "Welch", "Byrd", "Nicholson", "Lyons",
  "Osborne", "Mccoy", "Powers", "Schultz", "Richards", "Russell", "Wheeler", "Hines", "Dunn", "West",
  "Stone", "Hart", "Saunders", "Griffith", "Willis", "Sharp", "Horton", "Bowman", "Dennis", "Watkins",
   "O'brien", "Mccarthy", "Kennedy", "Fitzpatrick", "Ryan", "O'neill", "Doherty", "Gallagher", "Connolly", "Walsh",
  "Mcdonald", "Hughes", "Griffin", "Murphy", "Kelly", "Byrne", "Doyle", "Brady", "Brennan", "Quinn",
  "Carroll", "O'connor", "O'reilly", "Farrell", "Daly", "O'sullivan", "O'rourke", "O'donnell", "O'hara", "O'malley",
  // Thêm nhiều họ Trung Đông/Ả Rập
  "Al-Masri",    // Phổ biến ở Ai Cập, Palestine, Jordan, Syria, Lebanon
  "Al-Khatib",   // Phổ biến ở Syria, Lebanon, Palestine
  "Al-Najjar",   // Phổ biến ở Palestine, Jordan, Syria, Lebanon
  "Al-Husseini", // Họ có liên quan đến dòng dõi của Tiên tri Muhammad
  "Al-Tamimi",   // Họ Ả Rập cổ
  "Al-Ansari",    // Họ có liên quan đến những người ủng hộ Tiên tri Muhammad ở Medina
  "Al-Ahmad",     // Phổ biến ở nhiều nước Ả Rập
  "Al-Khalil",    // Phổ biến ở Palestine
  "Al-Sayegh",    // Phổ biến ở Lebanon, Syria
  "Al-Dabbagh",   // Phổ biến ở Syria
  "Al-Sayed",     // Họ có liên quan đến dòng dõi của Tiên tri Muhammad
  "Al-Amin",      // Họ có nghĩa là "người trung thực"
  "Al-Fadl",      // Họ có nghĩa là "ân sủng"
  "Al-Hakim",     // Họ có nghĩa là "người khôn ngoan"
  "Al-Rashid",    // Họ có nghĩa là "người ngay thẳng"
  "Al-Basri",     // Họ có nguồn gốc từ thành phố Basra
  "Al-Baghdadi",  // Họ có nguồn gốc từ thành phố Baghdad
  "Al-Iraqi",     // Họ có nghĩa là "người Iraq"
  "Al-Misri",     // Họ có nghĩa là "người Ai Cập"
  "Al-Sham"
        };
        public static readonly List<string> FirstnameRandom = new List<string>
        {
            "Alice", "Bob", "Charlie", "David", "Eva", "Frank", "Grace", "Henry", "Ivy", "Jack",
  "Karen", "Liam", "Mia", "Noah", "Olivia", "Peter", "Quinn", "Rachel", "Sam", "Tina",
  "Uriel", "Victoria", "William", "Xavier", "Yara", "Zoe",
  "An", "Binh", "Chi", "Dung", "Huy", "Lan", "Minh", "Nhi", "Phuong", "Quang", "Thao", "Tuan", "Vy", "Xuan", "Yen",
  "Mohammed", "Fatima", "Ahmed", "Sara", "Maria", "Jose", "Sofia", "Juan",
  "Anna", "Ben", "Chloe", "Daniel", "Ella", "Felix", "Hannah", "Isaac", "Julia", "Kevin",
  "Lily", "Max", "Nora", "Oscar", "Paige", "Ryan", "Sophia", "Thomas", "Uma", "Victor",
  "Wendy", "Xena", "Yasin", "Zara",
  "Aisha", "Rahman", "Nur", "Shorif",
  "Ji-hoon", "Seo-yeon", "Min-jun", "Eun-ji",
  "Mohammad", "Mehedi", "Abu", "Imran", "Abdullah-al", "Nazmul", "Tanvir", "Saiful", "Masud", "Sohel",
  "Shamim", "Nahid", "Abir", "Sagor", "Nusrat",
  "Abdullah", "Ali", "Omar", "Khalid", "Yousef", "Faisal", "Majid", "Rami", "Tariq", "Zain",
  "Layla", "Noura", "Amira", "Jana", "Reem", "Sara", "Hana", "Yasmin", "Zahra", "Malak",
  "Ethan", "Isabella", "Jacob", "Madison", "Michael", "Emily", "Alexander", "Abigail", "Daniel", "Chloe",
  "Matthew", "Elizabeth", "Joseph", "Mia", "Andrew", "Sofia", "David", "Evelyn", "James", "Harper",
  "Benjamin", "Amelia", "Anthony", "Ella", "Nicholas", "Avery", "Joshua", "Scarlett", "Christopher", "Grace",
  "Dylan", "Victoria", "Ryan", "Riley", "Brandon", "Lily", "Christian", "Addison", "Jonathan", "Aubrey",
   "Gabriel", "Zoey", "Samuel", "Madison", "Nathan", "Penelope", "Zachary", "Layla", "Caleb", "Brooklyn",
  "Adrian", "Hazel", "Owen", "Eleanor", "Julian", "Aurora", "Isaac", "Stella", "Cameron", "Violet",
  "Jose", "Maria", "Carlos", "Isabella", "Miguel", "Sofia", "Javier", "Valentina", "Alejandro", "Camila",
  "Mateo", "Emma", "Sebastian", "Martina", "Santiago", "Olivia", "Angel", "Mia", "Samuel", "Sofia",
  "Diego", "Valentina", "Leonardo", "Camila", "Adrian", "Isabella", "Martin", "Sofia", "Nicolas", "Emma",
  "Lucas", "Olivia", "Joaquin", "Valentina", "Tomas", "Sofia", "Vicente", "Isabella", "Benjamin", "Emma",
  "Pedro", "Sofia", "Juan", "Valentina", "Pablo", "Isabella", "Andres", "Emma", "Felipe", "Olivia",
  "Javier", "Camila", "Manuel", "Martina", "Ignacio", "Valentina", "Cristobal", "Emma", "Matias", "Sofia",

  // Thêm nhiều tên Trung Đông/Ả Rập
  "Youssef",    // Nam (biến thể của Yousef)
  "Hassan",     // Nam
  "Mahmoud",    // Nam
  "Khaled",     // Nam (biến thể của Khalid)
  "Ibrahim",    // Nam
  "Mustafa",    // Nam
  "Ahmed",      // Nam (đã có, nhưng rất phổ biến)
  "Sami",       // Nam
  "Rayan",      // Nam
  "Karim",      // Nam
  "Salma",      // Nữ
  "Aya",        // Nữ
  "Lina",       // Nữ
  "Dina",       // Nữ
  "Hala",       // Nữ
  "Nadine",     // Nữ
  "Rania",      // Nữ
  "Farah",      // Nữ
  "Leila",      // Nữ (biến thể của Layla)
  "Joudi"       // Nữ
        };
        public static readonly List<string> Countries = new List<string>
{
    "Random",
    "AF | 93", // Afghanistan
    "AL | 355", // Albania
    "DZ | 213", // Algeria
    "AS | 1-684", // American Samoa
    "AD | 376", // Andorra
    "AO | 244", // Angola
    "AI | 1-264", // Anguilla
    "AG | 1-268", // Antigua and Barbuda
    "AR | 54", // Argentina
    "AM | 374", // Armenia
    "AW | 297", // Aruba
    "AU | 61", // Australia
    "AT | 43", // Austria
    "AZ | 994", // Azerbaijan
    "BS | 1-242", // Bahamas
    "BH | 973", // Bahrain
    "BD | 880", // Bangladesh
    "BB | 1-246", // Barbados
    "BY | 375", // Belarus
    "BE | 32", // Belgium
    "BZ | 501", // Belize
    "BJ | 229", // Benin
    "BM | 1-441", // Bermuda
    "BT | 975", // Bhutan
    "BO | 591", // Bolivia
    "BA | 387", // Bosnia and Herzegovina
    "BW | 267", // Botswana
    "BR | 55", // Brazil
    "IO | 246", // British Indian Ocean Territory
    "VG | 1-284", // British Virgin Islands
    "BN | 673", // Brunei
    "BG | 359", // Bulgaria
    "BF | 226", // Burkina Faso
    "BI | 257", // Burundi
    "KH | 855", // Cambodia
    "CM | 237", // Cameroon
    "CA | 1", // Canada
    "CV | 238", // Cape Verde
    "KY | 1-345", // Cayman Islands
    "CF | 236", // Central African Republic
    "TD | 235", // Chad
    "CL | 56", // Chile
    "CN | 86", // China
    "CX | 61", // Christmas Island
    "CC | 61", // Cocos Islands
    "CO | 57", // Colombia
    "KM | 269", // Comoros
    "CD | 243", // Congo (Democratic Republic of the)
    "CG | 242", // Congo (Republic of the)
    "CK | 682", // Cook Islands
    "CR | 506", // Costa Rica
    "HR | 385", // Croatia
    "CU | 53", // Cuba
    "CW | 599", // Curacao
    "CY | 357", // Cyprus
    "CZ | 420", // Czech Republic
    "DK | 45", // Denmark
    "DJ | 253", // Djibouti
    "DM | 1-767", // Dominica
    "DO | 1-809", // Dominican Republic
    "TL | 670", // East Timor
    "EC | 593", // Ecuador
    "EG | 20", // Egypt
    "SV | 503", // El Salvador
    "GQ | 240", // Equatorial Guinea
    "ER | 291", // Eritrea
    "EE | 372", // Estonia
    "ET | 251", // Ethiopia
    "FK | 500", // Falkland Islands
    "FO | 298", // Faroe Islands
    "FJ | 679", // Fiji
    "FI | 358", // Finland
    "FR | 33", // France
    "GF | 594", // French Guiana
    "PF | 689", // French Polynesia
    "GA | 241", // Gabon
    "GM | 220", // Gambia
    "GE | 995", // Georgia
    "DE | 49", // Germany
    "GH | 233", // Ghana
    "GI | 350", // Gibraltar
    "GR | 30", // Greece
    "GL | 299", // Greenland
    "GD | 1-473", // Grenada
    "GP | 590", // Guadeloupe
    "GU | 1-671", // Guam
    "GT | 502", // Guatemala
    "GG | 44-1481", // Guernsey
    "GN | 224", // Guinea
    "GW | 245", // Guinea-Bissau
    "GY | 592", // Guyana
    "HT | 509", // Haiti
    "HN | 504", // Honduras
    "HK | 852", // Hong Kong
    "HU | 36", // Hungary
    "IS | 354", // Iceland
    "IN | 91", // India
    "ID | 62", // Indonesia
    "IR | 98", // Iran
    "IQ | 964", // Iraq
    "IE | 353", // Ireland
    "IM | 44-1624", // Isle of Man
    "IL | 972", // Israel
    "IT | 39", // Italy
    "CI | 225", // Ivory Coast
    "JM | 1-876", // Jamaica
    "JP | 81", // Japan
    "JE | 44-1534", // Jersey
    "JO | 962", // Jordan
    "KZ | 7", // Kazakhstan
    "KE | 254", // Kenya
    "KI | 686", // Kiribati
    "KW | 965", // Kuwait
    "KG | 996", // Kyrgyzstan
    "LA | 856", // Laos
    "LV | 371", // Latvia
    "LB | 961", // Lebanon
    "LS | 266", // Lesotho
    "LR | 231", // Liberia
    "LY | 218", // Libya
    "LI | 423", // Liechtenstein
    "LT | 370", // Lithuania
    "LU | 352", // Luxembourg
    "MO | 853", // Macau
    "MK | 389", // North Macedonia (formerly Macedonia)
    "MG | 261", // Madagascar
    "MW | 265", // Malawi
    "MY | 60", // Malaysia
    "MV | 960", // Maldives
    "ML | 223", // Mali
    "MT | 356", // Malta
    "MH | 692", // Marshall Islands
    "MQ | 596", // Martinique
    "MR | 222", // Mauritania
    "MU | 230", // Mauritius
    "YT | 262", // Mayotte
    "MX | 52", // Mexico
    "FM | 691", // Micronesia
    "MD | 373", // Moldova
    "MC | 377", // Monaco
    "MN | 976", // Mongolia
    "ME | 382", // Montenegro
    "MS | 1-664", // Montserrat
    "MA | 212", // Morocco
    "MZ | 258", // Mozambique
    "MM | 95", // Myanmar
    "NA | 264", // Namibia
    "NR | 674", // Nauru
    "NP | 977", // Nepal
    "NL | 31", // Netherlands
    "NC | 687", // New Caledonia
    "NZ | 64", // New Zealand
    "NI | 505", // Nicaragua
    "NE | 227", // Niger
    "NG | 234", // Nigeria
    "NU | 683", // Niue
    "NF | 672", // Norfolk Island
    "KP | 850", // North Korea
    "MP | 1-670", // Northern Mariana Islands
    "NO | 47", // Norway
    "OM | 968", // Oman
    "PK | 92", // Pakistan
    "PW | 680", // Palau
    "PS | 970", // Palestine
    "PA | 507", // Panama
    "PG | 675", // Papua New Guinea
    "PY | 595", // Paraguay
    "PE | 51", // Peru
    "PH | 63", // Philippines
    "PN | 64", // Pitcairn
    "PL | 48", // Poland
    "PT | 351", // Portugal
    "PR | 1-787", // Puerto Rico
    "QA | 974", // Qatar
    "RE | 262", // Reunion
    "RO | 40", // Romania
    "RU | 7", // Russia
    "RW | 250", // Rwanda
    "BL | 590", // Saint Barthelemy
    "SH | 290", // Saint Helena
    "KN | 1-869", // Saint Kitts and Nevis
    "LC | 1-758", // Saint Lucia
    "MF | 590", // Saint Martin
    "PM | 508", // Saint Pierre and Miquelon
    "VC | 1-784", // Saint Vincent and the Grenadines
    "WS | 685", // Samoa
    "SM | 378", // San Marino
    "ST | 239", // Sao Tome and Principe
    "SA | 966", // Saudi Arabia
    "SN | 221", // Senegal
    "RS | 381", // Serbia
    "SC | 248", // Seychelles
    "SL | 232", // Sierra Leone
    "SG | 65", // Singapore
    "SX | 1-721", // Sint Maarten
    "SK | 421", // Slovakia
    "SI | 386", // Slovenia
    "SB | 677", // Solomon Islands
    "SO | 252", // Somalia
    "ZA | 27", // South Africa
    "KR | 82", // South Korea
    "SS | 211", // South Sudan
    "ES | 34", // Spain
    "LK | 94", // Sri Lanka
    "SD | 249", // Sudan
    "SR | 597", // Suriname
    "SJ | 47", // Svalbard and Jan Mayen
    "SZ | 268", // Swaziland (Eswatini)
    "SE | 46", // Sweden
    "CH | 41", // Switzerland
    "SY | 963", // Syria
    "TW | 886", // Taiwan
    "TJ | 992", // Tajikistan
    "TZ | 255", // Tanzania
    "TH | 66", // Thailand
    "TG | 228", // Togo
    "TK | 690", // Tokelau
    "TO | 676", // Tonga
    "TT | 1-868", // Trinidad and Tobago
    "TN | 216", // Tunisia
    "TR | 90", // Turkey
    "TM | 993", // Turkmenistan
    "TC | 1-649", // Turks and Caicos Islands
    "TV | 688", // Tuvalu
    "UG | 256", // Uganda
    "UA | 380", // Ukraine
    "AE | 971", // United Arab Emirates
    "GB | 44", // United Kingdom
    "US | 1", // United States
    "UY | 598", // Uruguay
    "VI | 1-340", // US Virgin Islands
    "UZ | 998", // Uzbekistan
    "VU | 678", // Vanuatu
    "VA | 379", // Vatican City
    "VE | 58", // Venezuela
    "VN | 84", // Viet Nam
    "WF | 681", // Wallis and Futuna
    "YE | 967", // Yemen
    "ZM | 260", // Zambia
    "ZW | 263" // Zimbabwe
};
    }
}
