using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;    //入出力

namespace PokemonBattleLog
{
    public partial class Single : Form
    {
        #region 選出したポケモン
        /// <summary>
        /// 自分が選出したポケモン
        /// </summary>
        private List<string> MeSelectedPokemons = new List<string>();
        
        /// <summary>
        /// 相手が選出したポケモン
        /// </summary>
        private List<string> EnemySelectedPokemons = new List<string>();
        #endregion

        /// <summary>
        /// 自分のパーティ
        /// </summary>
        string[] MeParty = new string[6];

        /// <summary>
        /// 相手のパーティ
        /// </summary>
        List<string[]> EnemyPartyList = new List<string[]>();

        /// <summary>
        /// 自分の選出
        /// </summary>
        List<string[]> MeSelectList = new List<string[]>();

        /// <summary>
        /// 相手の選出
        /// </summary>
        List<string[]> EnemySelectList = new List<string[]>();

        /// <summary>
        /// 自分のレート
        /// </summary>
        List<int> MeRateList = new List<int>();

        /// <summary>
        /// 相手のレート
        /// </summary>
        List<int> EnemyRateList = new List<int>();

        /// <summary>
        /// 勝敗判定
        /// </summary>
        List<bool> Result = new List<bool>();
        
        /// <summary>
        /// 戦法などのメモ
        /// </summary>
        List<string> MemoList = new List<string>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Single()
        {
            InitializeComponent();
        }

        private void Origin_Load(object sender, EventArgs e)
        {

        }

        #region 勝敗ボタン
        /// <summary>
        /// 結果を「勝利」にして記録
        /// </summary>
        private void Win_Click(object sender, EventArgs e)
        {
            HoldInfomation(true);

            InitializeEnemyInfo();
            InitializeMeInfo();
        }

        /// <summary>
        /// 結果を「敗北」にして記録
        /// </summary>
        private void Lose_Click(object sender, EventArgs e)
        {
            HoldInfomation(false);

            InitializeEnemyInfo();
            InitializeMeInfo();
        }
        #endregion

        SaveFileDialog sfd = new SaveFileDialog();

        /// <summary>
        /// 結果を出力
        /// </summary>
        private void Output_Click(object sender, EventArgs e)
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                SaveInCSV(fileName);
            }
        }

        #region 情報初期化
        /// <summary>
        /// 相手情報を初期化
        /// </summary>
        public void InitializeEnemyInfo()
        {
            // 相手選出チェックボックスを全て外す.
            // チェックボックスを外すと選出情報も削除されるため、明示的に選出情報の削除は行わない.
            EnemySelected1.Checked = EnemySelected2.Checked = EnemySelected3.Checked =
                EnemySelected4.Checked = EnemySelected5.Checked = EnemySelected6.Checked = false;
            // 相手パーティ情報を削除.
            EnemyPokemon1.Text = EnemyPokemon2.Text = EnemyPokemon3.Text =
                EnemyPokemon4.Text = EnemyPokemon5.Text = EnemyPokemon6.Text = "";
            
            // 相手レートを1500に設定.
            EnemyRate.Value = 1500;
        }

        /// <summary>
        /// 自分情報を初期化
        /// </summary>
        public void InitializeMeInfo()
        {
            // 自分選出チェックボックスを全て外す.
            // チェックボックスを外すと選出情報も削除されるため、明示的に選出情報の削除は行わない.
            MeSelected1.Checked = MeSelected2.Checked = MeSelected3.Checked =
                MeSelected4.Checked = MeSelected5.Checked = MeSelected6.Checked = false;

            // カーソルを自分レートに移動.
            MeRate.Focus();
        }
        #endregion

        /// <summary>
        /// 各コントロールの値を変数に格納
        /// </summary>
        /// <param name="result">WIN / LOSEどちらのボタンを押したか
        /// true = WIN, false = LOSE</param>
        public void HoldInfomation(bool result)
        {
            //各コントロールの値を各変数に格納する
            string[] meSelectList = new string[4];
            meSelectList[0] = MeSelectedPokemon1.Text;
            meSelectList[1] = MeSelectedPokemon2.Text;
            meSelectList[2] = MeSelectedPokemon3.Text;
            int meRate = Int32.Parse(MeRate.Value.ToString());
            string memo = Memo.Text;
            
            SetMeResult(meSelectList, meRate, result, memo);

            string[] enemyParty = new string[6];
            enemyParty[0] = EnemyPokemon1.Text;
            enemyParty[1] = EnemyPokemon2.Text;
            enemyParty[2] = EnemyPokemon3.Text;
            enemyParty[3] = EnemyPokemon4.Text;
            enemyParty[4] = EnemyPokemon5.Text;
            enemyParty[5] = EnemyPokemon6.Text;
            string[] enemySelected = new string[4];
            enemySelected[0] = EnemySelectedPokemon1.Text;
            enemySelected[1] = EnemySelectedPokemon2.Text;
            enemySelected[2] = EnemySelectedPokemon3.Text;
            int enemyRate = Int32.Parse(EnemyRate.Value.ToString());

            SetEnemyResult(enemyParty, enemySelected, enemyRate);
        }

        /// <summary>
        /// 自分情報を記録
        /// </summary>
        /// <param name="select">選出したポケモン</param>
        /// <param name="rate">対戦前レート</param>
        /// <param name="winOrLoseResult">勝敗結果</param>
        /// <param name="supplement">補足メモ内容</param>
        public void SetMeResult(string[] select, int rate, bool winOrLoseResult, string supplement)
        {
            MeSelectList.Add(select);
            MeRateList.Add(rate);
            Result.Add(winOrLoseResult);
            MemoList.Add(supplement);
        }

        /// <summary>
        /// 相手情報を記録
        /// </summary>
        /// <param name="party">パーティ</param>
        /// <param name="select">選出したポケモン</param>
        /// <param name="rate">対戦前レート</param>
        public void SetEnemyResult(string[] party, string[] select, int rate)
        {
            EnemyPartyList.Add(party);
            EnemySelectList.Add(select);
            EnemyRateList.Add(rate);
        }

        #region 自分のポケモンの選出
        /// <summary>
        /// ポケモン1の選出
        /// </summary>
        private void MeSelected1_CheckedChanged(object sender, EventArgs e)
        {
            //foreach(Control item in MeSeletedChecks.Controls)
            //HACK:チェックマークを4つ以上つけないように制限を設けるか
            if (MeSelected1.Checked)
            {
                MeSelectedPokemons.Add(MePokemon1.Text);
            }
            else
            {
                MeSelectedPokemons.RemoveAt(MeSelectedPokemons.IndexOf(MePokemon1.Text));
            }
            RefreshMeSelectedPokemons();
        }

        /// <summary>
        /// ポケモン2の選出
        /// </summary>
        private void MeSelected2_CheckedChanged(object sender, EventArgs e)
        {
            if (MeSelected2.Checked)
            {
                MeSelectedPokemons.Add(MePokemon2.Text);
            }
            else
            {
                MeSelectedPokemons.RemoveAt(MeSelectedPokemons.IndexOf(MePokemon2.Text));
            }
            RefreshMeSelectedPokemons();
        }

        /// <summary>
        /// ポケモン3の選出
        /// </summary>
        private void MeSelected3_CheckedChanged(object sender, EventArgs e)
        {
            if (MeSelected3.Checked)
            {
                MeSelectedPokemons.Add(MePokemon3.Text);
            }
            else
            {
                MeSelectedPokemons.RemoveAt(MeSelectedPokemons.IndexOf(MePokemon3.Text));
            }
            RefreshMeSelectedPokemons();
        }

        /// <summary>
        /// ポケモン4の選出
        /// </summary>
        private void MeSelected4_CheckedChanged(object sender, EventArgs e)
        {
            if (MeSelected4.Checked)
            {
                MeSelectedPokemons.Add(MePokemon4.Text);
            }
            else
            {
                MeSelectedPokemons.RemoveAt(MeSelectedPokemons.IndexOf(MePokemon4.Text));
            }
            RefreshMeSelectedPokemons();
        }

        /// <summary>
        /// ポケモン5の選出
        /// </summary>
        private void MeSelected5_CheckedChanged(object sender, EventArgs e)
        {
            if (MeSelected5.Checked)
            {
                MeSelectedPokemons.Add(MePokemon5.Text);
            }
            else
            {
                MeSelectedPokemons.RemoveAt(MeSelectedPokemons.IndexOf(MePokemon5.Text));
            }
            RefreshMeSelectedPokemons();
        }

        /// <summary>
        /// ポケモン6の選出
        /// </summary>
        private void MeSelected6_CheckedChanged(object sender, EventArgs e)
        {
            if (MeSelected6.Checked)
            {
                MeSelectedPokemons.Add(MePokemon6.Text);
            }
            else
            {
                MeSelectedPokemons.RemoveAt(MeSelectedPokemons.IndexOf(MePokemon6.Text));
            }
            RefreshMeSelectedPokemons();
        }
        #endregion

        /// <summary>
        /// 選出ポケモンの表示
        /// </summary>
        private void RefreshMeSelectedPokemons()
        {
            if (MeSelectedPokemons.Count == 0)
            {
                MeSelectedPokemon1.Text = "";
                MeSelectedPokemon2.Text = "";
                MeSelectedPokemon3.Text = "";
            }
            else if (MeSelectedPokemons.Count == 1)
            {
                MeSelectedPokemon1.Text = MeSelectedPokemons.ElementAt(0);
                MeSelectedPokemon2.Text = "";
                MeSelectedPokemon3.Text = "";
            }
            else if (MeSelectedPokemons.Count == 2)
            {
                MeSelectedPokemon1.Text = MeSelectedPokemons.ElementAt(0);
                MeSelectedPokemon2.Text = MeSelectedPokemons.ElementAt(1);
                MeSelectedPokemon3.Text = "";
            }
            else if (MeSelectedPokemons.Count == 3)
            {
                MeSelectedPokemon1.Text = MeSelectedPokemons.ElementAt(0);
                MeSelectedPokemon2.Text = MeSelectedPokemons.ElementAt(1);
                MeSelectedPokemon3.Text = MeSelectedPokemons.ElementAt(2);
            }
        }

        #region 相手ポケモンの選出
        /// <summary>
        /// 相手ポケモン1の選出
        /// </summary>
        private void EnemySelected1_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemySelected1.Checked)
            {
                EnemySelectedPokemons.Add(EnemyPokemon1.Text);
            }
            else
            {
                EnemySelectedPokemons.RemoveAt(EnemySelectedPokemons.IndexOf(EnemyPokemon1.Text));
            }
            RefreshEnemySelectedPokemons();
        }

        /// <summary>
        /// 相手ポケモン2の選出
        /// </summary>
        private void EnemySelected2_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemySelected2.Checked)
            {
                EnemySelectedPokemons.Add(EnemyPokemon2.Text);
            }
            else 
            {
                EnemySelectedPokemons.RemoveAt(EnemySelectedPokemons.IndexOf(EnemyPokemon2.Text));
            }
            RefreshEnemySelectedPokemons();
        }

        /// <summary>
        /// 相手ポケモン3の選出
        /// </summary>
        private void EnemySelected3_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemySelected3.Checked)
            {
                EnemySelectedPokemons.Add(EnemyPokemon3.Text);
            }
            else
            {
                EnemySelectedPokemons.RemoveAt(EnemySelectedPokemons.IndexOf(EnemyPokemon3.Text));
            }
            RefreshEnemySelectedPokemons();
        }

        /// <summary>
        /// 相手ポケモン4の選出
        /// </summary>
        private void EnemySelected4_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemySelected4.Checked)
            {
                EnemySelectedPokemons.Add(EnemyPokemon4.Text);
            }
            else
            {
                EnemySelectedPokemons.RemoveAt(EnemySelectedPokemons.IndexOf(EnemyPokemon4.Text));
            }
            RefreshEnemySelectedPokemons();
        }

        /// <summary>
        /// 相手ポケモン5の選出
        /// </summary>
        private void EnemySelected5_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemySelected5.Checked)
            {
                EnemySelectedPokemons.Add(EnemyPokemon5.Text);
            }
            else
            {
                EnemySelectedPokemons.RemoveAt(EnemySelectedPokemons.IndexOf(EnemyPokemon5.Text));
            }
            RefreshEnemySelectedPokemons();
        }

        /// <summary>
        /// 相手ポケモン6の選出
        /// </summary>
        private void EnemySelected6_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemySelected6.Checked)
            {
                EnemySelectedPokemons.Add(EnemyPokemon6.Text);
            }
            else
            {
                EnemySelectedPokemons.RemoveAt(EnemySelectedPokemons.IndexOf(EnemyPokemon6.Text));
            }
            RefreshEnemySelectedPokemons();
        }
        #endregion


        /// <summary>
        /// 相手選出ポケモンの表示
        /// </summary>
        private void RefreshEnemySelectedPokemons()
        {
            if (EnemySelectedPokemons.Count == 0)
            {
                EnemySelectedPokemon1.Text = "";
                EnemySelectedPokemon2.Text = "";
                EnemySelectedPokemon3.Text = "";
            }
            else if (EnemySelectedPokemons.Count == 1)
            {
                EnemySelectedPokemon1.Text = EnemySelectedPokemons.ElementAt(0);
                EnemySelectedPokemon2.Text = "";
                EnemySelectedPokemon3.Text = "";
            }
            else if (EnemySelectedPokemons.Count == 2)
            {
                EnemySelectedPokemon1.Text = EnemySelectedPokemons.ElementAt(0);
                EnemySelectedPokemon2.Text = EnemySelectedPokemons.ElementAt(1);
                EnemySelectedPokemon3.Text = "";
            }
            else if (EnemySelectedPokemons.Count == 3)
            {
                EnemySelectedPokemon1.Text = EnemySelectedPokemons.ElementAt(0);
                EnemySelectedPokemon2.Text = EnemySelectedPokemons.ElementAt(1);
                EnemySelectedPokemon3.Text = EnemySelectedPokemons.ElementAt(2);
            }

        }

        /// <summary>
        /// CSV書き込み
        /// </summary>
        /// <param name="fileName">保存するCSVの絶対パス</param>
        public void SaveInCSV(string fileName)
        {
            StreamWriter SW = new StreamWriter(fileName, false, Encoding.GetEncoding("shift_jis"));
            SW.WriteLine("回数,レート,相手レート,選出1,2,3,4,相手PT1,2,3,4,5,6,相手選出1,2,3,4,勝敗,メモ");

            int i = 0;
            while(Result.Count != 0)
            {
                //回数, 自分レート, 相手レート
                SW.Write((i + 1) + "," + MeRateList[i] + "," + EnemyRateList[i] + ",");
                
                //自分の選出
                for(int j = 0; j < 4 ; j++){
                    if (MeSelectList[i][j] != "")
                    {
                        SW.Write(MeSelectList[i][j]);
                    }
                    else
                    {
                        //自分の選出が3匹以下であるときは空白を作る
                    }
                    SW.Write(",");
                }
                //相手のパーティ
                for (int j = 0; j < 6; j++)
                {
                    if (EnemyPartyList[i][j] != "")
                    {
                        SW.Write(EnemyPartyList[i][j]);
                    }
                    else
                    {
                        //相手のパーティが5匹以下である時は空白を作る
                    }
                    SW.Write(",");
                }
                //相手の選出
                for (int j = 0; j < 4; j++)
                {
                    if (EnemySelectList[i][j] != "")
                    {
                        SW.Write(EnemySelectList[i][j]);
                    }
                    else
                    {
                        //相手の選出が3匹以下であるときは空白を作る
                    }
                    SW.Write(",");
                }
                //勝敗
                if (Result[0])
                {
                    SW.Write("w");
                }
                else
                {
                    SW.Write("l");
                }
                SW.Write(",");

                Result.RemoveAt(0);

                //メモ
                SW.Write(MemoList[i]);
                
                //出力するセルを改行する
                SW.WriteLine(",");
                i++;
            }
            SW.Close();
        }

    }


    /// <summary>
    /// 情報保持、CSV書き込み
    /// </summary>
    public class Process
    {

    }
}
