﻿using Geometry;
using GLStyle;
using SharpGL;
using System;
using System.Collections.Generic;
using ThreadSafety;

namespace GLCore
{
    /// <summary>
    /// 複合點
    /// </summary>
    public class MultiPair : IMultiPair
    {
        /// <summary>
        /// 執行緒鎖
        /// </summary>
        private readonly object key = new object();

        /// <summary>
        /// 建立一個空物件集合
        /// </summary>
        public MultiPair(string styleName) { StyleName = styleName; }

        /// <summary>
        /// 初始化物件集合
        /// </summary>
        public MultiPair(string styleName, IEnumerable<IPair> pair)
        {
            StyleName = styleName;
            Geometry.SaftyEdit(true, list => list.AddRangeIfNotNull(pair));
        }

        /// <summary>
        /// 幾何座標集合
        /// </summary>
        public ISafty<List<IPair>> Geometry { get; } = new Safty<List<IPair>>(new List<IPair>());

        /// <summary>
        /// 樣式
        /// </summary>
        public IPairStyle Style { get { return StyleManager.GetStyle(StyleName) as IPairStyle; } }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        public string StyleName { get; set; }

        /// <summary>
        /// 透明的
        /// </summary>
        public bool Transparent { get { return Style?.BackgroundColor.A != 255; } }

        /// <summary>
        /// 最後一次產生頂點陣列的時間
        /// </summary>
        private DateTime LastGenVertexArrayTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 頂點陣列
        /// </summary>
        private int[] VertexArray { get; set; } = null;

        /// <summary>
        /// 繪圖
        /// </summary>
        public void Draw(OpenGL gl)
        {
            lock (key)
            {
                if (Style == null) return;
                GenVertexArray();

                if (VertexArray == null) return;

                if (Style.Size > 0) gl.PointSize(Style.Size);
                gl.Color(Style.BackgroundColor.GetFloats());

                gl.DrawArrays(OpenGL.GL_POINTS, VertexArray.Length / 2, VertexArray);
            }
        }

        /// <summary>
        /// 產生頂點陣列，加速繪圖
        /// </summary>
        private void GenVertexArray()
        {
            lock (key)
            {
                // 若已經產生頂點陣列，則返回
                if (VertexArray != null && LastGenVertexArrayTime >= Geometry.LastEditTime) return;

                LastGenVertexArrayTime = Geometry.LastEditTime;

                VertexArray = new int[Geometry.SaftyEdit(list => list.Count * 2)];
                int index = 0;

                Geometry.SaftyEdit(false, list => list.ForEach(pair =>
                {
                    VertexArray[index] = pair.X;
                    index++;
                    VertexArray[index] = pair.Y;
                    index++;
                }));
            }
        }
    }
}