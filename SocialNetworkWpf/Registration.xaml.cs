﻿using DAL.Concrete;
using DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace SocialNetworkWpf
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        UserDal dal = new UserDal();
        UserService dals = new UserService();
        public Registration()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        public void Reset()
        {
            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxEmail.Text = "";
            textBoxInterests.Text = "";
            passwordBox1.Password = "";
            passwordBoxConfirm.Password = "";
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                textBoxEmail.Focus();
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid email.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
                textBoxEmail.Focus();
            }
            else
            {
                string firstname = textBoxFirstName.Text;
                string lastname = textBoxLastName.Text;
                string email = textBoxEmail.Text;
                string password = passwordBox1.Password;
                string text = textBoxInterests.Text;
                List<string> interests = text.Split(',')
                         .Select(item => item.Trim())
                         .Where(item => !string.IsNullOrEmpty(item))
                         .ToList();
                if (passwordBox1.Password.Length == 0)
                {
                    errormessage.Text = "Enter password.";
                    passwordBox1.Focus();
                }
                else if (passwordBoxConfirm.Password.Length == 0)
                {
                    errormessage.Text = "Enter Confirm password.";
                    passwordBoxConfirm.Focus();
                }
                else if (passwordBox1.Password != passwordBoxConfirm.Password)
                {
                    errormessage.Text = "Confirm password must be same as password.";
                    passwordBoxConfirm.Focus();
                }
                else
                {
                    errormessage.Text = "";
                    Users user = new Users()
                    {
                        Email = email,
                        Password = dal.HashPassword(password),
                        FirstName = firstname,
                        LastName = lastname,
                        Interests = interests,
                        PostsId = [],
                        FollowersId = [],
                        FollowingId = []
                    };
                    dals.Add(user);
                    errormessage.Text = "You have Registered successfully.";
                    Reset();
                }
            }
        }
    }
}
