﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sunny.Subd.Core.Utils
{
    public class StringConvert
    {
        private static string icon1 = "\ud83d\ude42|\ud83d\ude00|\ud83d\ude04|\ud83d\ude06|\ud83d\ude05|\ud83d\ude02|\ud83e\udd23|\ud83d\ude0a|\ud83d\ude0c|\ud83d\ude09|\ud83d\ude0f|\ud83d\ude0d|\ud83d\ude18|\ud83d\ude17|\ud83d\ude19|\ud83d\ude1a|\ud83e\udd17|\ud83d\ude33|\ud83d\ude43|\ud83d\ude07|\ud83d\ude08|\ud83d\ude1b|\ud83d\ude1d|\ud83d\ude1c|\ud83d\ude0b|\ud83e\udd24|\ud83e\udd13|\ud83d\ude0e|\ud83e\udd11|\ud83d\ude12|\ud83d\ude41|☹\ufe0f|\ud83d\ude1e|\ud83d\ude14|\ud83d\ude16|\ud83d\ude13|\ud83d\ude22|\ud83d\ude22|\ud83d\ude2d|\ud83d\ude1f|\ud83d\ude23|\ud83d\ude29|\ud83d\ude2b|\ud83d\ude15|\ud83e\udd14|\ud83d\ude44|\ud83d\ude24|\ud83d\ude20|\ud83d\ude21|\ud83d\ude36|\ud83e\udd10|\ud83d\ude10|\ud83d\ude11|\ud83d\ude2f|\ud83d\ude32|\ud83d\ude27|\ud83d\ude28|\ud83d\ude30|\ud83d\ude31|\ud83d\ude2a|\ud83d\ude34|\ud83d\ude2c|\ud83e\udd25|\ud83e\udd27|\ud83e\udd12|\ud83d\ude37|\ud83e\udd15|\ud83d\ude35|\ud83e\udd22|\ud83e\udd20|\ud83e\udd21|\ud83d\udc7f|\ud83d\udc79|\ud83d\udc7a|\ud83d\udc7b|\ud83d\udc80|\ud83d\udc7d|\ud83d\udc7e|\ud83e\udd16|\ud83d\udca9|\ud83c\udf83";

        private static string icon2 = "♥\ufe0f|❤\ufe0f|\ud83d\udc9b|\ud83d\udc9a|\ud83d\udc99|\ud83d\udc9c|\ud83d\udda4|\ud83d\udc96|\ud83d\udc9d|\ud83d\udc94|❣\ufe0f|\ud83d\udc95|\ud83d\udc9e|\ud83d\udc93|\ud83d\udc97|\ud83d\udc98|\ud83d\udc9f|\ud83d\udc8c|\ud83d\udc8b|\ud83d\udc44|\ud83d\udc84|\ud83d\udc8d|\ud83d\udcff|\ud83c\udf81|\ud83d\udc59|\ud83d\udc57|\ud83d\udc5a|\ud83d\udc55|\ud83d\udc58|\ud83c\udfbd|\ud83d\udc58|\ud83d\udc56|\ud83d\udc60|\ud83d\udc61|\ud83d\udc62|\ud83d\udc5f|\ud83d\udc5e|\ud83d\udc52|\ud83c\udfa9|\ud83c\udf93|\ud83d\udc51|⛑\ufe0f|\ud83d\udc53|\ud83d\udd76\ufe0f|\ud83c\udf02|\ud83d\udc5b|\ud83d\udc5d|\ud83d\udc5c|\ud83d\udcbc|\ud83c\udf92|\ud83d\udecd\ufe0f|\ud83d\uded2|\ud83c\udfad|\ud83c\udfa6|\ud83c\udfa8|\ud83e\udd39|\ud83c\udf8a|\ud83c\udf89|\ud83c\udf88|\ud83c\udfa7|\ud83c\udfb7|\ud83c\udfba|\ud83c\udfb8|\ud83c\udfbb|\ud83e\udd41|\ud83c\udfb9|\ud83c\udfa4|\ud83c\udfb5|\ud83c\udfb6|\ud83c\udfbc|⚽|\ud83c\udfc0|\ud83c\udfc8|⚾|\ud83c\udfd0|\ud83c\udfc9|\ud83c\udfb1|\ud83c\udfbe|\ud83c\udff8|\ud83c\udfd3|\ud83c\udfcf|\ud83c\udfd1|\ud83c\udfd2|\ud83e\udd45|⛸\ufe0f|\ud83c\udfbf|\ud83e\udd4a|\ud83e\udd4b|⛳|\ud83c\udfb3|\ud83c\udff9|\ud83c\udfa3|\ud83c\udfaf|\ud83d\udeb5|\ud83c\udf96\ufe0f|\ud83c\udfc5|\ud83e\udd47|\ud83e\udd48|\ud83e\udd49|\ud83c\udfc6";

        private static string icon3 = "\ud83c\udf4f|\ud83c\udf4e|\ud83c\udf50|\ud83c\udf4a|\ud83c\udf4b|\ud83c\udf4c|\ud83c\udf49|\ud83c\udf47|\ud83c\udf53|\ud83c\udf48|\ud83e\udd5d|\ud83e\udd51|\ud83c\udf4d|\ud83c\udf52|\ud83c\udf51|\ud83c\udf46|\ud83e\udd52|\ud83e\udd55|\ud83c\udf36|\ud83c\udf3d|\ud83c\udf45|\ud83e\udd54|\ud83c\udf60|\ud83c\udf30|\ud83e\udd5c|\ud83c\udf6f|\ud83e\udd50|\ud83c\udf5e|\ud83e\udd56|\ud83e\uddc0|\ud83e\udd5a|\ud83c\udf73|\ud83e\udd53|\ud83c\udf64|\ud83c\udf57|\ud83c\udf56|\ud83c\udf55|\ud83c\udf2d|\ud83c\udf54|\ud83c\udf5f|\ud83e\udd59|\ud83c\udf2e|\ud83c\udf2f|\ud83e\udd57|\ud83e\udd58|\ud83c\udf5d|\ud83c\udf5c|\ud83c\udf72|\ud83c\udf63|\ud83c\udf71|\ud83c\udf5b|\ud83c\udf5a|\ud83c\udf59|\ud83c\udf58|\ud83c\udf62|\ud83c\udf61|\ud83c\udf67|\ud83c\udf68|\ud83c\udf66|\ud83e\udd5e|\ud83c\udf70|\ud83c\udf82|\ud83c\udf6e|\ud83c\udf6d|\ud83c\udf65|\ud83c\udf6c|\ud83c\udf6b|\ud83c\udf7f|\ud83c\udf69|\ud83c\udf6a|\ud83c\udf7c|\ud83e\udd5b|☕|\ud83c\udf75|\ud83c\udf76|\ud83c\udf7a|\ud83c\udf7b|\ud83e\udd42|\ud83c\udf77|\ud83e\udd43|\ud83c\udf78|\ud83c\udf79|\ud83c\udf7e|\ud83e\udd44|\ud83c\udf74|\ud83c\udf7d";

        private static string icon4 = "\ud83d\ude3a|\ud83d\ude38|\ud83d\ude39|\ud83d\ude3b|\ud83d\ude3c|\ud83d\ude3d|\ud83d\ude40|\ud83d\ude3f|\ud83d\ude3e|\ud83d\udc31|\ud83d\udc36|\ud83d\udc30|\ud83d\udc2d|\ud83d\udc39|\ud83e\udd8a|\ud83d\udc3b|\ud83d\udc3c|\ud83d\udc28|\ud83d\udc2f|\ud83e\udd81|\ud83d\udc2e|\ud83d\udc17|\ud83d\udc37|\ud83d\udc3d|\ud83d\udc38|\ud83d\udc35|\ud83d\ude48|\ud83d\ude49|\ud83d\ude4a|\ud83e\udd8d|\ud83d\udc3a|\ud83d\udc11|\ud83d\udc10|\ud83d\udc0f|\ud83d\udc34|\ud83e\udd84|\ud83e\udd8c|\ud83e\udd8f|\ud83e\udd85|\ud83d\udc24|\ud83d\udc23|\ud83d\udc25|\ud83d\udc14|\ud83d\udc13|\ud83e\udd83|\ud83d\udc26|\ud83e\udd86|\ud83e\udd87|\ud83e\udd89|\ud83d\udd4a\ufe0f|\ud83d\udc27|\ud83d\udc15|\ud83d\udc29|\ud83d\udc08|\ud83d\udc07|\ud83d\udc01|\ud83d\udc00|\ud83d\udc3f|\ud83d\udc12|\ud83d\udc16|\ud83d\udc06|\ud83d\udc05|\ud83d\udc03|\ud83d\udc02|\ud83d\udc04|\ud83d\udc0e|\ud83d\udc2a|\ud83d\udc2b|\ud83d\udc18|\ud83d\udc0a|\ud83d\udc22|\ud83d\udc20|\ud83d\udc1f|\ud83d\udc21|\ud83d\udc2c|\ud83e\udd88|\ud83d\udc33|\ud83d\udc0b|\ud83e\udd91|\ud83d\udc19|\ud83e\udd90|\ud83d\udc1a|\ud83e\udd80|\ud83e\udd82|\ud83e\udd8e|\ud83d\udc0d|\ud83d\udc1b|\ud83d\udc1c|\ud83d\udd77\ufe0f|\ud83d\udd78\ufe0f|\ud83d\udc1e|\ud83e\udd8b|\ud83d\udc1d|\ud83d\udc0c|\ud83d\udc32|\ud83d\udc09|\ud83d\udc3e";

        private static string icon5 = "\ud83c\udf3c|\ud83c\udf38|\ud83c\udf3a|\ud83c\udff5\ufe0f|\ud83c\udf3b|\ud83c\udf37|\ud83c\udf39|\ud83e\udd40|\ud83d\udc90|\ud83c\udf3e|\ud83c\udf8b|☘|\ud83c\udf40|\ud83c\udf43|\ud83c\udf42|\ud83c\udf41|\ud83c\udf31|\ud83c\udf3f|\ud83c\udf8d|\ud83c\udf35|\ud83c\udf34|\ud83c\udf33|\ud83c\udf33|\ud83c\udf84|\ud83c\udf44|\ud83c\udf0e|\ud83c\udf0d|\ud83c\udf0f|\ud83c\udf1c|\ud83c\udf1b|\ud83c\udf15|\ud83c\udf16|\ud83c\udf17|\ud83c\udf18|\ud83c\udf11|\ud83c\udf12|\ud83c\udf13|\ud83c\udf14|\ud83c\udf1a|\ud83c\udf1d|\ud83c\udf19|\ud83d\udcab|⭐|\ud83c\udf1f|✨|⚡|\ud83d\udd25|\ud83d\udca5|☄\ufe0f|\ud83c\udf1e|☀\ufe0f|\ud83c\udf24\ufe0f|⛅|\ud83c\udf25\ufe0f|\ud83c\udf26\ufe0f|☁\ufe0f|\ud83c\udf27\ufe0f|⛈\ufe0f|\ud83c\udf29\ufe0f|\ud83c\udf28\ufe0f|\ud83c\udf08|\ud83d\udca7|\ud83d\udca6|☂\ufe0f|☔|\ud83c\udf0a|\ud83c\udf2b|\ud83c\udf2a|\ud83d\udca8|❄|\ud83c\udf2c|⛄|☃\ufe0f";

        private static string icon6 = "\ud83d\ude97|\ud83d\ude95|\ud83d\ude99|\ud83d\ude8c|\ud83d\ude8e|\ud83c\udfce|\ud83d\ude93|\ud83d\ude91|\ud83d\ude92|\ud83d\ude90|\ud83d\ude9a|\ud83d\ude9b|\ud83d\ude9c|\ud83d\udef4|\ud83d\udeb2|\ud83d\udef5|\ud83c\udfcd|\ud83d\ude98|\ud83d\ude96|\ud83d\ude8d|\ud83d\ude94|\ud83d\udea8|\ud83d\udcba|✈|\ud83d\udeeb|\ud83d\udeec|\ud83d\udee9|\ud83d\ude81|\ud83d\ude80|\ud83d\udef0|\ud83d\udea1|\ud83d\udea0|\ud83d\ude9f|\ud83d\ude83|\ud83d\ude8b|\ud83d\ude9e|\ud83d\ude9d|\ud83d\ude84|\ud83d\ude85|\ud83d\ude88|\ud83d\ude82|\ud83d\ude86|\ud83d\ude8a|\ud83d\ude87|\ud83d\ude89|\ud83d\udef6|⛵|\ud83d\udee5|\ud83d\udea4|\ud83d\udea2|⛴|\ud83d\udef3|⚓|\ud83d\udea7|⛽|\ud83d\ude8f|\ud83d\udea6|\ud83d\udea5|\ud83d\udee3|\ud83d\udee4|\ud83c\udfd7|\ud83c\udfed|\ud83c\udfe0|\ud83c\udfe1|\ud83c\udfd8|\ud83c\udfda|\ud83c\udfe2|\ud83c\udfec|\ud83c\udfe4|\ud83c\udfe3|\ud83c\udfe5|\ud83c\udfe6|\ud83c\udfea|\ud83c\udfeb|\ud83c\udfe8|\ud83c\udfe9|\ud83c\udfdb|\ud83c\udff0|\ud83c\udfef|\ud83c\udfdf\ufe0f|⛪|\ud83d\udc92|\ud83d\udd4c|\ud83d\udd4d|\ud83d\udd4b|⛩|\ud83d\uddfc|\ud83d\uddff|\ud83d\uddfd|\ud83d\uddfa|\ud83c\udfaa|\ud83c\udfa0|\ud83c\udfa1|\ud83c\udfa2|⛲|⛱|\ud83c\udfd6|\ud83c\udfdd|\ud83c\udfd5|⛺|\ud83d\uddfe|⛰|\ud83c\udfd4|\ud83d\uddfb|\ud83c\udf0b|\ud83c\udfde|\ud83c\udfdc|\ud83c\udf05|\ud83c\udf04|\ud83c\udf91|\ud83c\udf20|\ud83c\udf87|\ud83c\udf86|\ud83c\udfd9|\ud83c\udf07|\ud83c\udf06|\ud83c\udf03|\ud83c\udf0c|\ud83c\udf09|\ud83c\udf01";

        private static string icon7 = "\ud83d\udcf1|\ud83d\udcf2|\ud83d\udcbb|\ud83d\udda5|⌨|\ud83d\udda8|\ud83d\uddb1|\ud83d\uddb2|\ud83d\udd79|\ud83c\udfae|\ud83d\udcbd|\ud83d\udcbe|\ud83d\udcbf|\ud83d\udcc0|\ud83d\udcfc|\ud83d\udcf7|\ud83d\udcf8|\ud83d\udcf9|\ud83c\udfa5|\ud83d\udcfd|\ud83c\udf9e|\ud83c\udfac|\ud83d\udcde|☎|\ud83d\udcdf|\ud83d\udce0|\ud83d\udcfa|\ud83d\udcfb|\ud83c\udf99|\ud83c\udf9a|\ud83c\udf9b|\ud83d\udce1|\ud83d\udce2|\ud83d\udce3|\ud83d\udd14|\ud83d\udca1|\ud83d\udd6f|\ud83d\udd26|\ud83d\udd0b|\ud83d\udd0c|⌚|⏱|⏲|⏰|\ud83d\udd70|⌛|⏳|\ud83d\udd2e|\ud83d\udc8e|\ud83c\udfb2|\ud83c\udfb0|\ud83d\udcb8|\ud83d\udcb5|\ud83d\udcb4|\ud83d\udcb6|\ud83d\udcb7|\ud83d\udcb0|\ud83d\udcb3|\ud83d\udcb2|\ud83d\udcb1|⚖|\ud83d\udd2b|\ud83d\udca3|\ud83d\udd2a|\ud83d\udde1|⚔|\ud83d\udee1|\ud83d\udeac|⚰|⚱|\ud83d\udddc\ufe0f|\ud83d\udd27|\ud83d\udd28|⚒|\ud83d\udee0|⛏|\ud83d\udd29|⚙|⛓|\ud83d\udc88|\ud83c\udf21|\ud83d\udc8a|\ud83d\udc89|⚗|\ud83d\udd2c|\ud83d\udd2d|\ud83d\udebf|\ud83d\udec1|\ud83d\udebd|\ud83d\udece|\ud83d\udd11|\ud83d\udddd|\ud83d\udeaa|\ud83d\udecb|\ud83d\udecf|\ud83d\uddbc|\ud83c\udffa|\ud83d\uddd1|\ud83d\udee2|\ud83d\udd73|\ud83c\udfee|\ud83c\udf8f|\ud83c\udf8e|\ud83c\udf90|\ud83c\udfab|\ud83c\udf9f\ufe0f|\ud83c\udf80|\ud83c\udf97\ufe0f|\ud83d\udcef|✉|\ud83d\udce9|\ud83d\udce8|\ud83d\udce7|\ud83d\udce6|\ud83d\udcea|\ud83d\udceb|\ud83d\udcec|\ud83d\udced|\ud83d\udcee|\ud83d\udce5|\ud83d\udce4|\ud83d\udcdc|\ud83d\udcc3|\ud83d\udcc4|\ud83d\udcd1|\ud83d\udcca|\ud83d\udcc8|\ud83d\udcc9|\ud83d\uddd2|\ud83d\udcc5|\ud83d\udcc6|\ud83d\uddd3|\ud83d\udcc7|\ud83d\uddc3|\ud83d\uddf3|\ud83d\uddc4|\ud83d\udccb|\ud83d\udcc1|\ud83d\udcc2|\ud83d\uddc2|\ud83d\udcd3|\ud83d\udcd4|\ud83d\udcd2|\ud83d\udcd5|\ud83d\udcd7|\ud83d\udcd8|\ud83d\udcd9|\ud83d\udcda|\ud83d\udcd6|\ud83d\uddde|\ud83d\udcf0|\ud83d\udcdd|✏|\ud83d\udd8a|\ud83d\udd8d|\ud83d\udd8c|\ud83d\udd8b|✒|\ud83d\udccc|\ud83d\udccd|\ud83d\udcce|\ud83d\udd87|\ud83d\udd16|\ud83c\udff7|\ud83d\udd17|\ud83d\udd0d|\ud83d\udd0e|\ud83d\udcd0|\ud83d\udccf|✂|\ud83d\udd12|\ud83d\udd13|\ud83d\udd0f|\ud83d\udd10";

        private static string icon8 = "\ud83d\ude42|\ud83d\ude00|\ud83d\ude04|\ud83d\ude06|\ud83d\ude05|\ud83d\ude02|\ud83e\udd23|\ud83d\ude0a|\ud83d\ude0c|\ud83d\ude09|\ud83d\ude0d|\ud83d\ude18|\ud83d\ude17|\ud83d\ude19|\ud83d\ude1a|\ud83e\udd17|\ud83d\ude33|\ud83d\ude43|\ud83d\ude1b|\ud83d\ude1d|\ud83d\ude1c|\ud83d\ude0b|\ud83e\udd24|\ud83e\udd13|\ud83d\ude0e";

        private static string number = "0\ufe0f\u20e3|1\ufe0f\u20e3|2\ufe0f\u20e3|3\ufe0f\u20e3|4\ufe0f\u20e3|5\ufe0f\u20e3|6\ufe0f\u20e3|7\ufe0f\u20e3|8\ufe0f\u20e3|9\ufe0f\u20e3";
        private static string GetIcon(string type, Random rd)
        {
            string result = "";
            List<string> list = new List<string>();
            try
            {
                switch (type)
                {
                    case "[r3]":
                        list = icon3.Split('|').ToList();
                        result = list[rd.Next(0, list.Count)];
                        break;
                    case "[r8]":
                        list = icon8.Split('|').ToList();
                        result = list[rd.Next(0, list.Count)];
                        break;
                    case "[t]":
                        result = DateTime.Now.ToString("HH:mm:ss");
                        break;
                    case "[r6]":
                        list = icon6.Split('|').ToList();
                        result = list[rd.Next(0, list.Count)];
                        break;
                    case "[r7]":
                        list = icon7.Split('|').ToList();
                        result = list[rd.Next(0, list.Count)];
                        break;
                    case "[r4]":
                        list = icon4.Split('|').ToList();
                        result = list[rd.Next(0, list.Count)];
                        break;
                    case "[d]":
                        result = DateTime.Now.ToString("dd/MM/yyyy");
                        break;
                    case "[r5]":
                        list = icon5.Split('|').ToList();
                        result = list[rd.Next(0, list.Count)];
                        break;
                    case "[r2]":
                        list = icon2.Split('|').ToList();
                        result = list[rd.Next(0, list.Count)];
                        break;
                    case "[r1]":
                        list = icon1.Split('|').ToList();
                        result = list[rd.Next(0, list.Count)];
                        break;
                }
            }
            catch
            {
                return result;
            }
            return result;
        }
        private static Random rd = new Random();
        private static List<string> lstKey = new List<string> { "[r1]", "[r2]", "[r3]", "[r4]", "[r5]", "[r6]", "[r7]", "[r8]", "[d]", "[t]" };
        public static string SpinText(string text)
        {
            string pattern = "{[^{}]*}";
            Match match = Regex.Match(text, pattern);
            while (match.Success)
            {
                string[] array = text.Substring(match.Index + 1, match.Length - 2).Split('|');
                text = text.Substring(0, match.Index) + array[rd.Next(array.Length)] + text.Substring(match.Index + match.Length);
                match = Regex.Match(text, pattern);
            }
            text = ProcessString(text);
            return text;
        }
        public static string ProcessString(string input)
        {
            string text = "";
            try
            {
                string text2 = "";
                for (int i = 0; i < lstKey.Count; i++)
                {
                    text2 = lstKey[i];
                    if (input.Contains(text2))
                    {
                        List<string> list = input.Split(new string[1] { text2 }, StringSplitOptions.None).ToList();
                        for (int j = 0; j < list.Count - 1; j++)
                        {
                            text = text + list[j] + GetIcon(text2, rd);
                        }
                        text += list[list.Count - 1];
                        input = text;
                        text = "";
                    }
                }
                MatchCollection matchCollection = Regex.Matches(input, "\\[n(.*?)\\]");
                for (int k = 0; k < matchCollection.Count; k++)
                {
                    List<string> list2 = input.Split(new string[1] { matchCollection[k].Value }, StringSplitOptions.None).ToList();
                    for (int l = 0; l < list2.Count - 1; l++)
                    {
                        text = text + list2[l] + CreateRandomNumber(Convert.ToInt32(matchCollection[k].Groups[1].Value));
                    }
                    text += list2[list2.Count - 1];
                    input = text;
                    text = "";
                }
                matchCollection = Regex.Matches(input, "\\[s(.*?)\\]");
                for (int m = 0; m < matchCollection.Count; m++)
                {
                    List<string> list3 = input.Split(new string[1] { matchCollection[m].Value }, StringSplitOptions.None).ToList();
                    for (int n = 0; n < list3.Count - 1; n++)
                    {
                        text = text + list3[n] + CreateRandomString(Convert.ToInt32(matchCollection[m].Groups[1].Value));
                    }
                    text += list3[list3.Count - 1];
                    input = text;
                    text = "";
                }
                matchCollection = Regex.Matches(input, "\\[q(.*?)\\]");
                for (int num = 0; num < matchCollection.Count; num++)
                {
                    List<string> list4 = input.Split(new string[1] { matchCollection[num].Value }, StringSplitOptions.None).ToList();
                    for (int num2 = 0; num2 < list4.Count - 1; num2++)
                    {
                        text = text + list4[num2] + GetNumber(matchCollection[num].Groups[1].Value);
                    }
                    text += list4[list4.Count - 1];
                    input = text;
                    text = "";
                }
                return input;
            }
            catch
            {
                return input;
            }
        }
        public static string CreateRandomString(int int_2, Random random_1 = null)
        {
            string text = "";
            if (random_1 == null)
            {
                random_1 = rd;
            }
            string text2 = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < int_2; i++)
            {
                text += text2[random_1.Next(0, text2.Length)];
            }
            return text;
        }
        public static string CreateRandomNumber(int DD89F9AF, Random random_1 = null)
        {
            string text = "";
            if (random_1 == null)
            {
                random_1 = new Random();
            }
            string text2 = "0123456789";
            for (int i = 0; i < DD89F9AF; i++)
            {
                text += text2[random_1.Next(0, text2.Length)];
            }
            return text;
        }
        private static string GetNumber(string input)
        {
            string text = "";
            try
            {
                string text2 = "";
                List<string> list = number.Split('|').ToList();
                for (int i = 0; i < input.Length; i++)
                {
                    text2 = input[i].ToString();
                    if (IsNumber(text2))
                    {
                        text2 = list[Convert.ToInt32(text2)];
                    }
                    text += text2;
                }
                return text;
            }
            catch
            {
                return text;
            }
        }
        public static bool IsNumber(string pValue)
        {
            if (pValue == "")
            {
                return false;
            }
            for (int i = 0; i < pValue.Length; i++)
            {
                if (!char.IsDigit(pValue[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
