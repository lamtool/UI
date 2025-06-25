﻿using Sunny.Subd.Core.Models;
using System.Collections.Concurrent;

namespace Sunny.Subd.Core.Utils
{
    public class XpathManager
    {
        private static readonly ConcurrentDictionary<XpathType, List<string>> _xpathGroups = new();
        static XpathManager()
        {
            _xpathGroups.TryAdd(XpathType.Captcha, new List<string>
      {
          $"//*[contains(text, \"Enter the characters you see\")]",
      });
            _xpathGroups.TryAdd(XpathType.CP282, new List<string>
          {
             "//*[@text=\"Type the text\"]",
             "//*[contains(@content-desc, \"confirm you're human to use your account\")]",
             $"//*[contains(text, \"Record a video of yourself\")]",
             $"//*[contains(text, \"Enter the characters you see\")]",
             $"//*[contains(text, \"Vietnam (+84)\")]",
             $"//*[contains(text, \"United States of America (+1)\")]",
             $"//*[contains(text, \"Type the text\")]",
             $"//*[contains(text, \"We disabled your account\")]",
             $"//*[contains(text, \"Access Denied\")]",
             $"//*[contains(text, \"Appeal\")]",
             $"//*[contains(text, \"Send by SMS\")]",
             "//*[@text=\"Enter text\"]",
             "//*[@text=\"This page is currently not displayed\"]",
             $"//*[contains(text, \"we suspended your account\")]",
             $"//*[contains(text, \"read more about this rule\")]",
             $"//*[contains(text, \"Upload image or take photo\")]",
             $"//*[contains(text, \"Account Temporarily Unavailable\")]",
          });
            _xpathGroups.TryAdd(XpathType.CP956, new List<string>
          {
             $"//*[contains(text, \"Check your WhatsApp messages\")]",
             $"//*[contains(text, \"check your email\")]",
          });
            _xpathGroups.TryAdd(XpathType.Block, new List<string>
          {
             $"//*[contains(text, \"We limit how often you can post\")]",
             $"//*[contains(text, \"Dismiss\")]",
             $"//*[contains(text, \"Your account is restricted\")]",
             $"//*[contains(text, \"we added restrictions to your account\")]",
          });
            _xpathGroups.TryAdd(XpathType.Success, new List<string>
          {
            "//*[contains(@content-desc, 'Go to profile')]",
             $"//*[contains(text, \"Allow Facebook to access\")]",
             $"//*[contains(text, \"Continue in English\")]",
             $"//*[contains(text, \"Find friends\")]",
             $"//*[contains(text, \"Save your login info\")]",
             $"//*[contains(text, \"Access to contacts\")]",
             $"//*[contains(text, \"Add number\")]",
             $"//*[contains(text, \"Allow Facebook to access your\")]",
             $"//*[contains(text, \"Is this your account\")]",
             $"//*[contains(text, \"Log in using another device\")]",
             $"//*[contains(text, \"Add number\")]",
             $"//*[contains(text, \"Allow Facebook to access your\")]",
             $"//*[contains(text, \"Is this your account\")]",
             "//*[@text=\"Add a profile picture\"]",
             "//*[@text=\"Add a mobile number to your account\"]"
          });
            _xpathGroups.TryAdd(XpathType.Loading, new List<string>
      {
          "//*[@content-desc=\"Đang tải\"]",
          "//*[@content-desc=\"Loading…\"]"
      });
            _xpathGroups.TryAdd(XpathType.Logout, new List<string>
      {
          $"//*[contains(text, \"Unable to log in\")]",
          $"//*[contains(text, \"Wrong Credentials\")]",
          $"//*[contains(text, \"Invalid username or password\")]",
          $"//*[contains(text, \"The password you entered is incorrect\")]",
          $"//*[contains(text, \"Unable to log in\")]",
      });
            _xpathGroups.TryAdd(XpathType.NavigationButton, new List<string>
      {

          "//*[@text=\"Continue using English (US)\"]",
          "//*[@text=\"Get started\"]",
          "//*[@text=\"Log in\"]",
          "//*[@content-desc=\"Log in\"]",
          "//*[@text=\"Deny\"]",
          "//*[@content-desc=\"Next\"]",
          "//*[@text=\"Tiếp\"]",
          "//*[@text=\"Next\"]",
          "//*[@text=\"Bỏ qua\"]",
          "//*[@text=\"Skip\"]",
          "//*[@text=\"Lúc khác\"]",
          "//*[@text=\"Later\"]",
          "//*[@text=\"Not now\"]",
          "//*[@text=\"Later\"]",
          "//*[@text=\"Don't allow\"]",
          "//*[@content-desc=\"Not now\"]",
          "//*[@content-desc=\"Save\"]",
          "//*[@content-desc=\"Dismiss\"]",
          "//*[@text=\"OK\"]",
          "//*[@content-desc=\"Continue in English (US)\"]",
          "//*[@content-desc=\"Continue\"]",
          "//*[@text=\"Continue\"]",
      });
            _xpathGroups.TryAdd(XpathType.TowFA, new List<string>
      {
          $"//*[contains(text, \"Generate a code from your authentication app and enter it to log in\")]",
          $"//*[contains(text, \"Check your notifications on another device\")]",
          $"//*[contains(text, \"Authentication app, Get a code from your authentication app.\")]",
          $"//*[contains(text, \"Go to your authentication app\")]",
      });
            _xpathGroups.TryAdd(XpathType.CashApp, new List<string>
      {
          $"//*[contains(text, \"Session Expired\")]",
      });
            _xpathGroups.TryAdd(XpathType.InputUserName, new List<string>
      {
          $"//*[contains(text, \"Phone or email\")]",
          $"//*[contains(text, \"Mobile number or email\")]",
          $"//*[@text=\"Log into another account\"]",
          "//*[@text=\"Mobile number or email\"]",
          $"//*[contains(text, \"Create new account\")]",
      });
            _xpathGroups.TryAdd(XpathType.InputPassword, new List<string>
      {
          $"//*[contains(text, \"Enter Password\")]",
      });

            _xpathGroups.TryAdd(XpathType.Regsiner_Facebook, new List<string>
            {

                "//*[@text=\"Male\"]",
                "//*[@text=\"Female\"]",
                "//*[@text=\"Add a mobile number to your account\"]",
                "//*[@text=\"Add a profile picture\"]",
                "//*[@text=\"Save login info?\"]",
                "//*[@text=\"Add friends\"]",
                "//*[@text=\"Turn on contact uploading\"]",
                "//*[@text=\"What is your mobile number?\"]",
                "//*[@text=\"Sign up with email\"]",
                "//*[@text=\"Choose your name\"]",
                "//*[@content-desc=\"Go to profile\"]",
                "//*[@text=\"This page is currently not displayed\"]",
                "//*[@content-desc=\"Appeal\"]",
                "//*[@text=\"Enter text\"]",
                "//*[@text=\"I agree\"]",
                "//*[@text=\"Select your name\"]",
                "//*[@text=\"Couldn't create account\"]",
                "//*[@text=\"Enter email\"]",
                "//*[@text=\"Confirm with email\"]",
                "//*[@text=\"I didn't receive a code\"]",
                "//*[@text=\"Please log in again.\"]",
                "//*[@text=\"IMPORT CONTACTS\"]",
                "//*[@text=\"Sign up\"]",
                "//*[@text=\"Get started\"]",
                "//*[@text=\"Create new account\"]",
                "//*[@content-desc=\"Create new account\"]",
                "//*[@content-desc=\"Join Facebook\"]",
                "//*[@content-desc=\"No, create new account\"]",
                "//*[@content-desc=\"Create new Facebook account\"]",
                "//*[@text=\"Continue creating account\"]",
                "//*[@content-desc=\"Continue creating account\"]",
                "//*[@text=\"No, create account\"]",
                "//*[@text=\"What's your name?\"]",
                "//*[@text=\"When is your date of birth?\"]",
                "//*[@text=\"SET\"]",
                "//*[@text=\"What is your gender?\"]",
                "//*[@text=\"What is your email?\"]",
                "//*[@text=\"Sign up with mobile number\"]",
                "//*[@text=\"Create a password\"]",
                "//*[contains(@text, \"sent to\")]",

            });
            _xpathGroups.TryAdd(XpathType.ExistEmail, new List<string>
            {
                "//*[@text=\"This Page Isn't Available Right Now\"]",
                "//*[@text=\"There is already an account linked to this email address.\"]",
                "//*[@text=\"An account already exists linked to this email.\"]",
            });
            _xpathGroups.TryAdd(XpathType.Confim_Register, new List<string>
            {
                "//*[@text=\"Continue creating account\"]",
 "//*[@text=\"Confirm by email\"]",
 "//*[@text=\"Enter an email\"]",
 "//*[@text=\"I didn’t get the code\"]",
 "//*[@text=\"Continue using English (US)\"]",

 "//*[@text=\"Enter email\"]",
 "//*[@text=\"Confirm with email\"]",
 "//*[@text=\"I didn't receive a code\"]",
 "//*[@text=\"I agree\"]",
 "//*[@text=\"Please log in again.\"]",
 "//*[@text=\"Sign up\"]",
 "//*[@text=\"Create new account\"]",
 "//*[@text=\"Create new account\"]",
 "//*[@content-desc=\"Create new account\"]",
      "//*[@text=\"Get started\"]",
 "//*[@content-desc=\"Join Facebook\"]",
 "//*[@text=\"Get started\"]",
 "//*[@content-desc=\"No, create new account\"]",
            });
        }
        public static List<string> Get(XpathType group)
        {
            return _xpathGroups.TryGetValue(group, out var list) ? new List<string>(list) : new List<string>();
        }

        public static List<string> Combine(params object[] groupsOrXpaths)
        {
            var result = new List<string>();
            foreach (var item in groupsOrXpaths)
            {
                if (item is XpathType groupName)
                    result.AddRange(Get(groupName));
                else if (item is IEnumerable<string> list)
                    result.AddRange(list);
            }
            return result;
        }

        public static void AddCustomGroup(XpathType key, List<string> xpaths)
        {
            _xpathGroups[key] = xpaths;
        }
    }
}
