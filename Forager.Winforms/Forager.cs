﻿using Forager.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Forager.WinForms {
    public partial class Forager : Form {
        private Bitmap _grassBitmap;
        private int _fieldSize = 10;
        private Bitmap[] _shroomImages;
        private int _numShrooms = 8;
        private readonly string[] _shroomName = new string[] {
            "Fly Agaric",
            "Chanterelle",
            "Honey Fungus",
            "Giant Puffball",
            "Shaggy Inkcap",
            "Common Morel",
            "King Oyster",
            "Rosy Brittlegill",
            "Penny Bun",
            "Grey Oyster"
        };
        private Cell[][] _cells;
        private Cell _lastClicked;
        private HashSet<Cell> _tourCells;
        private Cell[] _shroomCells;
        private int _tourDistance;
        private int _goalDistance;
        private int _streak = 0;
        private int _topStreak = 0;
        private int _numInTour;
        private Cell _start;

        public Forager() {
            InitializeComponent();
            _grassBitmap = new Bitmap(new Bitmap(Directory.GetCurrentDirectory() + @"\Images\grass.jpeg"), new Size(40, 40));
            
            _shroomImages = new Bitmap[_fieldSize];
            _cells = new Cell[_fieldSize][];
            for (int i = 0; i < _fieldSize; i++) {
                _cells[i] = new Cell[_fieldSize];

                var orig = new Bitmap(Directory.GetCurrentDirectory() + $@"\Images\{i % 10}.jpg");
                _shroomImages[i] = new Bitmap(orig, new Size(38, 38));

                for (int j = 0; j < _fieldSize; j++) {
                    var cell = new Cell { Row = i, Col = j, State = CellState.Grass };
                    _cells[i][j] = cell;
                    var picBox = new PictureBox {
                        Location = new Point(43 * j, 43 * i),
                        Size = new Size(40, 40),
                        Image = _grassBitmap,
                        Tag = cell
                    };
                    picBox.MouseClick += PicBox_MouseClick;
                    cell.PictureBox = picBox;
                    panel1.Controls.Add(picBox);
                }
            }
        }

        private void PicBox_MouseClick(object sender, MouseEventArgs e) {
            var cell = (Cell)((PictureBox)sender).Tag;

            if (cell.State != CellState.Shroom)
                return;

            _numInTour++;
            cell.PictureBox.MouseClick -= PicBox_MouseClick;
            cell.PictureBox.MouseEnter -= Shroom_MouseEnter;

            if (_lastClicked == null) {
                _start = cell;
                _start.PictureBox.BackColor = Utils.Colours[0];
                _lastClicked = _start;
                resetBoardButton.Enabled = true;
                return;
            }

            var color = Utils.Colours[_tourCells.Count()];
            if (_numInTour < _numShrooms) {
                cell.PictureBox.BackColor = Utils.Colours[_tourCells.Count() + 1];
            }

            DrawRouteToCell(cell, color);

            _tourDistance += cell.DistanceTo(_lastClicked);
            _tourCells.Add(cell);
            distanceLabel.Text = _tourDistance.ToString();
            _lastClicked = cell;

            if (_numInTour == _numShrooms) {
                _start.PictureBox.MouseEnter += Shroom_MouseEnter;
                _start.PictureBox.MouseClick += PicBox_MouseClick;
            }

            if (cell != _start)
                return;

            if (_tourDistance <= _goalDistance) {
                MessageBox.Show("Congratulations! You've achieved the goal distance.");
                _streak++;
                if (_streak > _topStreak) {
                    _topStreak++;
                    topStreakLabel.Text = _topStreak.ToString();
                }
                if (_topStreak > 0 && !resetStreakButton.Enabled) {
                    resetStreakButton.Enabled = true;
                }
            } else {
                MessageBox.Show("Uh oh! There is a better route.");
                _streak = 0;
            }

            streakLabel.Text = _streak.ToString();
        }

        private void DrawRouteToCell(Cell cell, Color color) {
            if (cell.Row != _lastClicked.Row) {
                int rowChange = cell.Row > _lastClicked.Row ? -1 : 1;
                for (int r = cell.Row + rowChange; r != _lastClicked.Row; r = r + rowChange) {
                    var routeCell = _cells[r][cell.Col];
                    var img = new Bitmap(routeCell.PictureBox.Image);
                    var g = Graphics.FromImage(img);
                    g.DrawRectangle(new Pen(color, 5), 19, 0, 2, 39);
                    g.Save();
                    routeCell.PictureBox.Image = img;
                    routeCell.IsOriginal = false;
                }

                if (cell.Col != _lastClicked.Col) {
                    var routeCell = _cells[_lastClicked.Row][cell.Col];
                    var img = new Bitmap(routeCell.PictureBox.Image);
                    var g = Graphics.FromImage(img);
                    g.DrawRectangle(new Pen(color, 5), 19, 19, 2, 2);
                    g.Save();
                    routeCell.PictureBox.Image = img;
                    routeCell.IsOriginal = false;
                }
            }

            if (cell.Col != _lastClicked.Col) {
                int colChange = cell.Col > _lastClicked.Col ? -1 : 1;
                for (int c = cell.Col + colChange; c != _lastClicked.Col; c = c + colChange) {
                    var routeCell = _cells[_lastClicked.Row][c];
                    var img = new Bitmap(routeCell.PictureBox.Image);
                    var g = Graphics.FromImage(img);
                    g.DrawRectangle(new Pen(color, 5), 0, 19, 39, 2);
                    g.Save();
                    routeCell.PictureBox.Image = img;
                    routeCell.IsOriginal = false;
                }
            }
        }

        private void newGameButton_Click(object sender, EventArgs e) {
            resetBoardButton.Enabled = false;
            distanceLabel.Text = "0";
            for (int i = 0; i < _fieldSize; i++) {
                for (int j = 0; j < _fieldSize; j++) {
                    var cell = _cells[i][j];
                    cell.SetImage(_grassBitmap);
                    cell.State = CellState.Grass;
                    cell.PictureBox.MouseEnter -= Shroom_MouseEnter;
                    cell.PictureBox.MouseLeave -= Shroom_MouseLeave;
                    cell.PictureBox.MouseClick -= PicBox_MouseClick;
                    cell.PictureBox.MouseClick += PicBox_MouseClick;
                    cell.RemoveToolTipText();
                }
            }

            var rnd = new Random();
            _shroomCells = new Cell[_numShrooms];
            var num = 0;
            while (num < _numShrooms) {
                var iM = rnd.Next(0, _fieldSize);
                var jM = rnd.Next(0, _fieldSize);
                var cell = _cells[iM][jM];
                if (cell.IsShroom)
                    continue;

                cell.State = CellState.Shroom;
                cell.SetImage(_shroomImages[num]);
                cell.PictureBox.MouseEnter += Shroom_MouseEnter;
                cell.PictureBox.MouseLeave += Shroom_MouseLeave;
                cell.SetToolTipText(_shroomName[num]);
                _shroomCells[num] = cell;
                num++;
            }

            _lastClicked = null;
            _tourCells = new HashSet<Cell>();
            var matrix = new int[_numShrooms][];

            for (int i = 0; i < _numShrooms; i++) {
                var source = _shroomCells[i];
                matrix[i] = new int[_numShrooms];
                for (int j = 0; j < _numShrooms; j++) {
                    var target = _shroomCells[j];
                    matrix[i][j] = source.DistanceTo(target);
                }
            }

            var tsp = new TSP(matrix);
            var tour = tsp.Solve();
            _goalDistance = tour.Cost;
            goalLabel.Text = _goalDistance.ToString();
            _tourDistance = 0;
            _numInTour = 0;
        }

        private void Shroom_MouseEnter(object sender, EventArgs e) => Cursor = Cursors.Hand;

        private void Shroom_MouseLeave(object sender, EventArgs e) => Cursor = Cursors.Default;

        private void ResetBoard(object sender, EventArgs e) {
            _numInTour = 0;
            _tourDistance = 0;
            _lastClicked = null;
            _start = null;
            distanceLabel.Text = "0";
            for (int i = 0; i < _fieldSize; i++) {
                for (int j = 0; j < _fieldSize; j++) {
                    var cell = _cells[i][j];
                    cell.ResetImage();

                    if (cell.State == CellState.Shroom) {
                        cell.PictureBox.BackColor = Color.Transparent;
                        cell.PictureBox.MouseEnter += Shroom_MouseEnter;
                        cell.PictureBox.MouseLeave += Shroom_MouseLeave;
                        cell.PictureBox.MouseClick -= PicBox_MouseClick;
                        cell.PictureBox.MouseClick += PicBox_MouseClick;
                    }
                }
            }
            _tourCells = new HashSet<Cell>();
        }

        private void ResetStreak(object sender, EventArgs e) {
            _streak = 0;
            _topStreak = 0;
            streakLabel.Text = "0";
            topStreakLabel.Text = "0";
            resetStreakButton.Enabled = false;
        }

        private void EditDifficulty(object sender, EventArgs e) {
            var dlg = new DifficultyDialog();
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.OK) {
                _numShrooms = dlg.NumShrooms;
            }

            switch (_numShrooms) {
                case 6:
                    difficultyLabel.Text = "Easy";
                    break;
                case 8:
                    difficultyLabel.Text = "Medium";
                    break;
                default:
                    difficultyLabel.Text = "Hard";
                    break;
            }
        }
    }
}
