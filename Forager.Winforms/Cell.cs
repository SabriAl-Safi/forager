﻿using Forager.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forager.WinForms {
    public class Cell {
        public PictureBox PictureBox { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public CellState State { get; set; }
        public bool IsShroom => State == CellState.Shroom;
        public bool IsOriginal { get; set; }

        private Bitmap _originalImage;
        private ToolTip _toolTip = new ToolTip();

        public int DistanceTo(Cell other) => Math.Abs(Row - other.Row) + Math.Abs(Col - other.Col);

        public void SetImage(Bitmap image) {
            _originalImage = image;
            PictureBox.Image = image;
            PictureBox.BackColor = Color.Transparent;
            IsOriginal = true;
        }

        public void ResetImage() {
            if (IsOriginal)
                return;
            PictureBox.Image = _originalImage;
            PictureBox.BackColor = Color.Transparent;
        }

        public void SetToolTipText(string text) {
            _toolTip.SetToolTip(PictureBox, text);
        }

        public void RemoveToolTipText() => _toolTip.RemoveAll();
    }
}
