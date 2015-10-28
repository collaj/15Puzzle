using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _15Puzzle
{
    public partial class MainForm : Form
    {
        private int MARGIN = 5;
        private int PADDING = 2;
        private int TILE_SIZE = 60;
        private int ANIMATION_SPEED = 2;


        public MainForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        internal void buildGame()
        {

            //           two margins     padding between the tiles         all the tiles          just makes it look better
            int height = (MARGIN * 2) + (PADDING * (Game.LENGTH - 1)) + (Game.LENGTH * TILE_SIZE) - 2;
            int width =  (MARGIN * 2) + (PADDING * (Game.LENGTH - 1)) + (Game.LENGTH * TILE_SIZE) - 2;
            gamePanel.Size = new System.Drawing.Size(width, height);

            for (int i = 0; i < Game.LENGTH; i++)
            {
                for (int j = 0; j < Game.LENGTH; j++)
                {
                    if (i == Game.LENGTH - 1 && j == Game.LENGTH - 1)
                        continue;

                    int value = (i * Game.LENGTH) + j + 1;
                    int locX = MARGIN + ((TILE_SIZE + PADDING) * j);
                    int locY = MARGIN + ((TILE_SIZE + PADDING) * i);

                    Label label = new Label();
                    label.Text = value.ToString();
                    label.Name = label.Text;
                    label.BackColor = (value % 2 == 0) ? Color.White : Color.Red;
                    label.ForeColor = (value % 2 == 0) ? Color.Black : Color.White;
                    label.Font = new Font("Courier", 25);
                    label.Size = new System.Drawing.Size(TILE_SIZE, TILE_SIZE);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    label.Location = new Point(locX, locY);
                    gamePanel.Controls.Add(label);



                }
            }



        }

        internal void startNewGame()
        {
            gamePanel.Controls.Clear();

            int[,] board = Game.getCurrentState().getState();
            for (int i = 0; i < Game.LENGTH; i++)
            {
                for (int j = 0; j < Game.LENGTH; j++)
                {
                    int value = board[i, j];

                    if (value == 0)
                        continue;

                    int locX = MARGIN + ((TILE_SIZE + PADDING) * j);
                    int locY = MARGIN + ((TILE_SIZE + PADDING) * i);

                    Label label = new Label();
                    label.Text = value.ToString();
                    label.Name = label.Text;
                    label.BackColor = (value % 2 == 0) ? Color.White : Color.Red;
                    label.ForeColor = (value % 2 == 0) ? Color.Black : Color.White;
                    label.Font = new Font("Courier", 25);
                    label.Size = new System.Drawing.Size(TILE_SIZE, TILE_SIZE);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    label.Location = new Point(locX, locY);
                    gamePanel.Controls.Add(label);

                }
            }
        }


        internal void updatePanels(string action, int tileValue)
        {

            Control con = gamePanel.Controls.Find(tileValue.ToString(), true)[0];
            switch (action)
            {
                case "left":
                    animatePanel(con, new Point(con.Location.X - (TILE_SIZE + PADDING), con.Location.Y));
                    break;
                case "right":
                    animatePanel(con, new Point(con.Location.X + (TILE_SIZE + PADDING), con.Location.Y));
                    break;
                case "up":
                    animatePanel(con, new Point(con.Location.X, con.Location.Y - (TILE_SIZE + PADDING)));
                    break;
                case "down":
                    animatePanel(con, new Point(con.Location.X, con.Location.Y + (TILE_SIZE + PADDING)));
                    break;
            }

        }

        private void animatePanel(Control control, Point newLocation)
        {
            Point currentLocation = control.Location;
            int delta_x;
            if (newLocation.X - currentLocation.X > 0)
                delta_x = ANIMATION_SPEED;
            else if (newLocation.X - currentLocation.X < 0)
                delta_x = -1 * ANIMATION_SPEED;
            else
                delta_x = 0;

            int delta_y;
            if (newLocation.Y - currentLocation.Y > 0)
                delta_y = ANIMATION_SPEED;
            else if (newLocation.Y - currentLocation.Y < 0)
                delta_y = -1 * ANIMATION_SPEED;
            else
                delta_y = 0;

            while (control.Location.X != newLocation.X | control.Location.Y != newLocation.Y)
            {
                int new_x = newLocation.X - control.Location.X;
                if (Math.Abs(new_x) < ANIMATION_SPEED)
                    delta_x = new_x;

                int new_y = newLocation.Y - control.Location.Y;
                if (Math.Abs(new_y) < ANIMATION_SPEED)
                    delta_y = new_y;
                
                control.Location = new Point(control.Location.X + delta_x, control.Location.Y + delta_y);
            }

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left && Game.gameOn)
            {
                State newState = MoveBlank.moveBlank(Game.getCurrentState(), "right");
                if (!newState.Equals(Game.getCurrentState())) {
                    int[] blankLoc = Game.findBlankLocation(Game.getCurrentState().getState());
                    int value = Game.getValueAtPosition(blankLoc[0], blankLoc[1] + 1);
                    updatePanels("left", value);

                    Game.setCurrentState(newState);
                    if (Game.isGoalState())
                    {
                        Game.gameOn = false;
                        winScreen();
                    }
                }
                return true;
            }
            else if (keyData == Keys.Right && Game.gameOn)
            {
                State newState = MoveBlank.moveBlank(Game.getCurrentState(), "left");
                if (!newState.Equals(Game.getCurrentState()))
                {
                    int[] blankLoc = Game.findBlankLocation(Game.getCurrentState().getState());
                    int value = Game.getValueAtPosition(blankLoc[0], blankLoc[1] - 1);
                    updatePanels("right", value);

                    Game.setCurrentState(newState);
                    if (Game.isGoalState())
                    {
                        Game.gameOn = false;
                        winScreen();
                    }
                }
                return true;
            }
            else if (keyData == Keys.Up && Game.gameOn)
            {
                State newState = MoveBlank.moveBlank(Game.getCurrentState(), "down");
                if (!newState.Equals(Game.getCurrentState()))
                {
                    int[] blankLoc = Game.findBlankLocation(Game.getCurrentState().getState());
                    int value = Game.getValueAtPosition(blankLoc[0] + 1, blankLoc[1]);
                    updatePanels("up", value);

                    Game.setCurrentState(newState);
                    if (Game.isGoalState())
                    {
                        Game.gameOn = false;
                        winScreen();
                    }
                }
                return true;
            }
            else if (keyData == Keys.Down && Game.gameOn)
            {
                State newState = MoveBlank.moveBlank(Game.getCurrentState(), "up");
                if (!newState.Equals(Game.getCurrentState()))
                {
                    int[] blankLoc = Game.findBlankLocation(Game.getCurrentState().getState());
                    int value = Game.getValueAtPosition(blankLoc[0] - 1, blankLoc[1]);
                    updatePanels("down", value);

                    Game.setCurrentState(newState);
                    if (Game.isGoalState())
                    {
                        Game.gameOn = false;
                        winScreen();
                    }
                }
                return true;
            }
            else 
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }


        private void winScreen()
        {
            System.Windows.Forms.MessageBox.Show("You Win!!!");
        }


        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Game.newGame();
            startNewGame();
        }


        private void changeSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int size;
            if (Int32.TryParse(Microsoft.VisualBasic.Interaction.InputBox("Please input the size\nSize must be between 2 and 10"), out size) && size > 1 && size <= 10)
            {
                Game.LENGTH = size;
                newGameToolStripMenuItem_Click(sender, e);
            }
            
        }


    }
}
