﻿using Xamarin.Forms;

namespace collNotes.ColorThemes
{
    public partial class Dark : ResourceDictionary
    {
        public static readonly Color PageBackgroundColor = Color.FromHex("616161");
        public static readonly Color SecondaryBackgroundColor = Color.FromHex("373737");
        public static readonly Color NavigationBarColor = Color.FromHex("388e3c");

        public static readonly Color PrimaryColor = Color.FromHex("388e3c");
        public static readonly Color SecondaryColor = Color.FromHex("388a8e");

        public static readonly Color PrimaryTextColor = Color.White;
        public static readonly Color SecondaryTextColor = Color.White;
        public static readonly Color TertiaryTextColor = Color.WhiteSmoke;

        public static readonly Color PrimaryColorLight = Color.FromHex("6abf69");
        public static readonly Color PrimaryColorDark = Color.FromHex("00600f");

        public static readonly Color SecondaryColorLight = Color.FromHex("6bbabe");
        public static readonly Color SecondaryColorDark = Color.FromHex("005c61");

        public Dark()
        {
            InitializeComponent();
        }
    }
}