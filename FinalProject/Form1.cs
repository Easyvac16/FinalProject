using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FinalProject
{

    public partial class Form1 : Form
    {
        private bool isPlayerXTurn;
        private bool isPlayerVsPlayer = false;
        private bool gameOver = false;
        private int player1ScorePvP = 0; // Результати для режиму "Player Vs Player"
        private int player2ScorePvP = 0;
        private int player1ScorePvB = 0; // Результати для режиму "Player Vs Bot"
        private int player2ScorePvB = 0;
        private string[,] gameBoard = new string[3, 3];

        public Form1()
        {
            InitializeComponent();
            OffPictureBox(); // Вимикаємо всі PictureBox на початку
            button3.Hide();
            button4.Hide();
            label1.Hide();
            label2.Hide();

        }

        private void OffPictureBox()
        {
            foreach (Control control in Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Enabled = false;
                }
            }
        }

        private void OnPictureBox()
        {
            foreach (Control control in Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Enabled = true;
                }
            }
        }

        private void PlayerSelect()
        {
            Random random = new Random();
            isPlayerXTurn = random.Next(2) == 0; // Вибираємо гравця випадковим чином
        }

        private void CheckWin()
        {
            bool isWin = false;
            string winner = "";

            // Перевірка виграшних комбінацій за допомогою методів CheckRow, CheckColumn, CheckDiagonal та CheckAntiDiagonal
            for (int row = 0; row < 3; row++)
            {
                if (CheckRow(row) || CheckColumn(row))
                {
                    isWin = true;
                    break;
                }
            }

            if (!isWin)
            {
                if (CheckDiagonal() || CheckAntiDiagonal())
                {
                    isWin = true;
                }
            }

            if (isWin)
            {
                if (isPlayerXTurn)
                {
                    winner = isPlayerVsPlayer ? "Нолики" : "Бот";
                    if (isPlayerVsPlayer)
                    {
                        player2ScorePvP += 1;
                    }
                    else
                    {
                        player1ScorePvB += 1;
                    }
                }
                else
                {
                    winner = isPlayerVsPlayer ? "Крестики" : "Гравець";
                    if (isPlayerVsPlayer)
                    {
                        player1ScorePvP += 1;
                    }
                    else
                    {
                        player2ScorePvB += 1;
                    }
                }

                MessageBox.Show($"Виграли {winner}!");

                // В залежності від обраного режиму, виводимо відповідні результати гри
                if (isPlayerVsPlayer)
                {
                    label1.Text = "" + player1ScorePvP;
                    label2.Text = "" + player2ScorePvP;
                }
                else
                {
                    label1.Text = "" + player1ScorePvB;
                    label2.Text = "" + player2ScorePvB;
                }

                HandleGameEnd();
            }
            else if (AllCellsAreFilled())
            {
                MessageBox.Show("Нічия!");
                HandleGameEnd();
            }
        }

        private bool CheckRow(int row)
        {
            return gameBoard[row, 0] != null && gameBoard[row, 0] == gameBoard[row, 1] && gameBoard[row, 1] == gameBoard[row, 2];
        }

        private bool CheckColumn(int col)
        {
            return gameBoard[0, col] != null && gameBoard[0, col] == gameBoard[1, col] && gameBoard[1, col] == gameBoard[2, col];
        }

        private bool CheckDiagonal()
        {
            return gameBoard[0, 0] != null && gameBoard[0, 0] == gameBoard[1, 1] && gameBoard[1, 1] == gameBoard[2, 2];
        }

        private bool CheckAntiDiagonal()
        {
            return gameBoard[0, 2] != null && gameBoard[0, 2] == gameBoard[1, 1] && gameBoard[1, 1] == gameBoard[2, 0];
        }

        private bool AllCellsAreFilled()
        {
            foreach (Control control in Controls)
            {
                if (control is PictureBox pictureBox && pictureBox.Image == null)
                {
                    return false;
                }
            }
            return true;
        }

        private void HandleGameEnd()
        {
            OffPictureBox();
            gameOver = true;
            ClearGameBoard();
        }

        private void ClearGameBoard()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    gameBoard[row, col] = null;
                }
            }
        }

        private void ChangeImage(PictureBox pictureBox)
        {
            if (pictureBox.Image == null)
            {
                if (comboBox1.SelectedIndex == 0)
                {
                    pictureBox.Image = isPlayerXTurn ? Properties.Resources.ImageX_Red : Properties.Resources.ImageO_Red;
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    pictureBox.Image = isPlayerXTurn ? Properties.Resources.ImageX : Properties.Resources.ImageO;
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    pictureBox.Image = isPlayerXTurn ? Properties.Resources.ImageX_Orange : Properties.Resources.ImageO_Orange;
                }

                isPlayerXTurn = !isPlayerXTurn;
            }
        }

        private void MakeBotMove()
        {
            Random random = new Random();

            // Вибираємо случайну пусту клітинку
            List<PictureBox> emptyCells = new List<PictureBox>();
            foreach (Control control in Controls)
            {
                if (control is PictureBox pictureBox && pictureBox.Image == null)
                {
                    emptyCells.Add(pictureBox);
                }
            }
            if (emptyCells.Count > 0)
            {

                int index = random.Next(emptyCells.Count);
                PictureBox selectedPictureBox = emptyCells[index];
                if (selectedPictureBox.Enabled == true)
                {
                    // Заповнюємо вибрану клітинку символом бота (наприклад, "X")
                    if (comboBox1.SelectedIndex == 0)
                    {
                        selectedPictureBox.Image = isPlayerXTurn ? Properties.Resources.ImageX_Red : Properties.Resources.ImageO_Red;
                    }
                    else if (comboBox1.SelectedIndex == 1)
                    {
                        selectedPictureBox.Image = isPlayerXTurn ? Properties.Resources.ImageX : Properties.Resources.ImageO;
                    }
                    else if (comboBox1.SelectedIndex == 2)
                    {
                        selectedPictureBox.Image = isPlayerXTurn ? Properties.Resources.ImageX_Orange : Properties.Resources.ImageO_Orange;
                    }
                    selectedPictureBox.Enabled = false;
                    // Переключаємо хід
                    isPlayerXTurn = !isPlayerXTurn;
                }
            }

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

            PictureBox clickedPictureBox = sender as PictureBox;

            int rowIndex = int.Parse(clickedPictureBox.Tag.ToString().Substring(0, 1));// Отримуємо 0 індекс з поля Tag в PictureBox і передаємо тільки 1 число
            int colIndex = int.Parse(clickedPictureBox.Tag.ToString().Substring(1, 1));

            if (isPlayerVsPlayer)
            {
                if (clickedPictureBox.Enabled)
                {
                    ChangeImage(clickedPictureBox);
                    gameBoard[rowIndex, colIndex] = isPlayerXTurn ? "X" : "O";
                    CheckWin();
                }

            }
            else
            {
                if (clickedPictureBox.Enabled)
                {
                    MakeBotMove();
                    gameBoard[rowIndex, colIndex] = isPlayerXTurn ? "X" : "O";
                    CheckWin();
                    ChangeImage(clickedPictureBox);
                    clickedPictureBox.Enabled = false;
                    gameBoard[rowIndex, colIndex] = isPlayerXTurn ? "X" : "O";
                    CheckWin();
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayerSelect();
            isPlayerVsPlayer = true;
            OnPictureBox();
            button1.Hide();
            button2.Hide();
            button3.Show();
            button4.Show();
            comboBox1.Hide();
            label1.Show();
            label2.Show();
            // Скидаємо результати гри для режиму "Player Vs Bot"
            player1ScorePvB = 0;
            player2ScorePvB = 0;
            // Встановлюємо відповідні результати гри для режиму "Player Vs Player"
            label1.Text = "" + player1ScorePvP;
            label2.Text = "" + player2ScorePvP;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PlayerSelect();
            isPlayerVsPlayer = false;
            OnPictureBox();
            button2.Hide();
            button1.Hide();
            button3.Show();
            button4.Show();
            comboBox1.Hide();
            label1.Show();
            label2.Show();
            player1ScorePvP = 0;
            player2ScorePvP = 0;
            label1.Text = "" + player1ScorePvB;
            label2.Text = "" + player2ScorePvB;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Image = null;
                }
            }
            button1.Show();
            button2.Show();
            comboBox1.Show();
            button3.Hide();
            button4.Hide();
            label1.Hide();
            label2.Hide();
            OffPictureBox();
            ClearGameBoard();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0 || comboBox1.SelectedIndex == 1 || comboBox1.SelectedIndex == 2)
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
            if (comboBox1.SelectedIndex == 0)
            {
                pictureBox10.Image = Properties.Resources.ImageX_Red;
                pictureBox11.Image = Properties.Resources.ImageO_Red;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                pictureBox10.Image = Properties.Resources.ImageX;
                pictureBox11.Image = Properties.Resources.ImageO;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                pictureBox10.Image = Properties.Resources.ImageX_Orange;
                pictureBox11.Image = Properties.Resources.ImageO_Orange;
            }

        }
    }
}