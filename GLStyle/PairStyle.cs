﻿using IniFiles;

namespace GLStyle
{
    /// <summary>
    /// 點樣式
    /// </summary>
    internal class PairStyle : IPairStyle
    {
        /// <summary>
        /// 從 *.ini 檔案讀取樣式設定
        /// </summary>
        public PairStyle(string filePath, string section)
        {
            BackgroundColor = new Color(INI.Read(filePath, section, nameof(BackgroundColor), Color.GreenYellow128));
            Layer = INI.Read(filePath, section, nameof(Layer), 0);
            Size = INI.Read(filePath, section, nameof(Size), 1.0f);
        }

        /// <summary>
        /// 底色
        /// </summary>
        public IColor BackgroundColor { get; }

        /// <summary>
        /// 圖層位置
        /// </summary>
        public int Layer { get; }

        /// <summary>
        /// 點大小
        /// </summary>
        public float Size { get; }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        public string StyleType { get { return nameof(IPairStyle); } }
    }
}