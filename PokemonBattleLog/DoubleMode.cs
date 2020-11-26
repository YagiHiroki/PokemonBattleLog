using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;    //入出力
using PokemonBattleLog.Data;
using PokemonBattleLog.Output;
using System.Diagnostics;

namespace PokemonBattleLog
{
    public partial class DoubleMode : Form
    {
        //#region 選出したポケモン
        ///// <summary>
        ///// 自分が選出したポケモン
        ///// </summary>
        //private List<string> _InputedData.MeSelectedPokemons = new List<string>();

        ///// <summary>
        ///// 相手が選出したポケモン
        ///// </summary>
        //private List<string> _InputedData.EnemySelectedPokemons = new List<string>();
        //#endregion

        ///// <summary>
        ///// 自分のパーティ
        ///// </summary>
        //string[] MeParty = new string[6];

        ///// <summary>
        ///// 相手のパーティ
        ///// </summary>
        //List<string[]> EnemyPartyList = new List<string[]>();

        ///// <summary>
        ///// 自分の選出
        ///// </summary>
        //List<string[]> MeSelectList = new List<string[]>();

        ///// <summary>
        ///// 相手の選出
        ///// </summary>
        //List<string[]> EnemySelectList = new List<string[]>();

        ///// <summary>
        ///// 自分のレート
        ///// </summary>
        //List<int> MeRateList = new List<int>();

        ///// <summary>
        ///// 相手のレート
        ///// </summary>
        //List<int> EnemyRateList = new List<int>();

        ///// <summary>
        ///// 勝敗判定
        ///// </summary>
        //List<bool> Result = new List<bool>();

        ///// <summary>
        ///// 戦法などのメモ
        ///// </summary>
        //List<string> MemoList = new List<string>();


        /// <summary>
        /// 入力されたデータを持つ
        /// </summary>
        InputedData _InputedData = new InputedData();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DoubleMode()
        {
            InitializeComponent();
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

        /// <summary>
        /// 結果を出力
        /// </summary>
        private void Output_Click(object sender, EventArgs e)
        {
            String saveDirectoryPath = Directory.GetCurrentDirectory() + "\\data";

            //dataフォルダがない場合は作成する
            if (!Directory.Exists(saveDirectoryPath))
            {
                Directory.CreateDirectory(saveDirectoryPath);
            }

            sfd.InitialDirectory = saveDirectoryPath;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;

                //fileNameの存在を確認する
                bool a = File.Exists(fileName);
                
                OutputResult output = new OutputResult();
                output.SaveInCSV2(fileName, _InputedData, a);

                //inputedDataの情報を初期化する

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
            List<CheckBox> checkboxs = new List<CheckBox>() { MeSelected1, MeSelected2, MeSelected3, MeSelected4, MeSelected5, MeSelected6 };
            checkboxs.ForEach(x =>
            {
                x.Checked = false;
                x.CheckedChanged += (s, e) => Debug.WriteLine("checked " + ((CheckBox)s).Text);
            });

            //メモ欄を空白にする
            Memo.Text = "";

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
            if(string.IsNullOrEmpty(_InputedData.MeParty[0])){
                _InputedData.MeParty[0] = MePokemon1.Text;
                _InputedData.MeParty[1] = MePokemon2.Text;
                _InputedData.MeParty[2] = MePokemon3.Text;
                _InputedData.MeParty[3] = MePokemon4.Text;
                _InputedData.MeParty[4] = MePokemon5.Text;
                _InputedData.MeParty[5] = MePokemon6.Text;

            }

            string[] meSelectList = new string[4];
            meSelectList[0] = MeSelectedPokemon1.Text;
            meSelectList[1] = MeSelectedPokemon2.Text;
            meSelectList[2] = MeSelectedPokemon3.Text;
            meSelectList[3] = MeSelectedPokemon4.Text;
            int meRate = Int32.Parse(MeRate.Value.ToString());
            string memo = Memo.Text;

            //SetMeResult(meSelectList, meRate, result, memo);
            _InputedData = ToSetInfoInClass.SetMeResult(_InputedData, meSelectList, meRate, result, memo);

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
            enemySelected[3] = EnemySelectedPokemon4.Text;
            int enemyRate = Int32.Parse(EnemyRate.Value.ToString());

            SetEnemyResult(enemyParty, enemySelected, enemyRate);
        }

        ///// <summary>
        ///// 自分情報を記録
        ///// </summary>
        ///// <param name="select">選出したポケモン</param>
        ///// <param name="rate">対戦前レート</param>
        ///// <param name="winOrLoseResult">勝敗結果</param>
        ///// <param name="supplement">補足メモ内容</param>
        //public void SetMeResult(string[] select, int rate, bool winOrLoseResult, string supplement)
        //{
        //    _InputedData.MeSelectList.Add(select);
        //    _InputedData.MeRateList.Add(rate);
        //    _InputedData.Result.Add(winOrLoseResult);
        //    _InputedData.MemoList.Add(supplement);
        //}

        /// <summary>
        /// 相手情報を記録
        /// </summary>
        /// <param name="party">パーティ</param>
        /// <param name="select">選出したポケモン</param>
        /// <param name="rate">対戦前レート</param>
        public void SetEnemyResult(string[] party, string[] select, int rate)
        {
            _InputedData.EnemyPartiesList.Add(party);
            _InputedData.EnemySelectList.Add(select);
            _InputedData.EnemyRateList.Add(rate);
        }

        #region 自分のポケモンの選出
        /// <summary>
        /// ポケモン1の選出
        /// </summary>
        private void MeSelected1_CheckedChanged(object sender, EventArgs e)
        {
            //foreach(Control item in MeSeletedChecks.Controls)
            //HACK:チェックマークを4つ以上つけないように制限を設けるか

            //ポケモンが未入力の場合、チェックさせない
            if (MePokemon1.Text == "")
            {
                MeSelected1.Checked = false;
                return;
            }

            if (MeSelected1.Checked)
            {
                _InputedData.MeSelectedPokemons.Add(MePokemon1.Text);
            }
            else
            {
                _InputedData.MeSelectedPokemons.RemoveAt(_InputedData.MeSelectedPokemons.IndexOf(MePokemon1.Text));
            }
            ShowMeSelectedPokemons();
        }

        /// <summary>
        /// ポケモン2の選出
        /// </summary>
        private void MeSelected2_CheckedChanged(object sender, EventArgs e)
        {
            //ポケモンが未入力の場合、チェックさせない
            if (MePokemon2.Text == "")
            {
                MeSelected2.Checked = false;
                return;
            }
            if (MeSelected2.Checked)
            {
                _InputedData.MeSelectedPokemons.Add(MePokemon2.Text);
            }
            else
            {
                _InputedData.MeSelectedPokemons.RemoveAt(_InputedData.MeSelectedPokemons.IndexOf(MePokemon2.Text));
            }
            ShowMeSelectedPokemons();
        }

        /// <summary>
        /// ポケモン3の選出
        /// </summary>
        private void MeSelected3_CheckedChanged(object sender, EventArgs e)
        {
            //ポケモンが未入力の場合、チェックさせない
            if (MePokemon3.Text == "")
            {
                MeSelected3.Checked = false;
                return;
            }
            if (MeSelected3.Checked)
            {
                _InputedData.MeSelectedPokemons.Add(MePokemon3.Text);
            }
            else
            {
                _InputedData.MeSelectedPokemons.RemoveAt(_InputedData.MeSelectedPokemons.IndexOf(MePokemon3.Text));
            }
            ShowMeSelectedPokemons();
        }

        /// <summary>
        /// ポケモン4の選出
        /// </summary>
        private void MeSelected4_CheckedChanged(object sender, EventArgs e)
        {
            //ポケモンが未入力の場合、チェックさせない
            if (MePokemon4.Text == "")
            {
                MeSelected4.Checked = false;
                return;
            }
            if (MeSelected4.Checked)
            {
                _InputedData.MeSelectedPokemons.Add(MePokemon4.Text);
            }
            else
            {
                _InputedData.MeSelectedPokemons.RemoveAt(_InputedData.MeSelectedPokemons.IndexOf(MePokemon4.Text));
            }
            ShowMeSelectedPokemons();
        }

        /// <summary>
        /// ポケモン5の選出
        /// </summary>
        private void MeSelected5_CheckedChanged(object sender, EventArgs e)
        {
            //ポケモンが未入力の場合、チェックさせない
            if (MePokemon5.Text == "")
            {
                MeSelected5.Checked = false;
                return;
            }
            if (MeSelected5.Checked)
            {
                _InputedData.MeSelectedPokemons.Add(MePokemon5.Text);
            }
            else
            {
                _InputedData.MeSelectedPokemons.RemoveAt(_InputedData.MeSelectedPokemons.IndexOf(MePokemon5.Text));
            }
            ShowMeSelectedPokemons();
        }

        /// <summary>
        /// ポケモン6の選出
        /// </summary>
        private void MeSelected6_CheckedChanged(object sender, EventArgs e)
        {
            //ポケモンが未入力の場合、チェックさせない
            if (MePokemon6.Text == "")
            {
                MeSelected6.Checked = false;
                return;
            }
            if (MeSelected6.Checked)
            {
                _InputedData.MeSelectedPokemons.Add(MePokemon6.Text);
            }
            else
            {
                _InputedData.MeSelectedPokemons.RemoveAt(_InputedData.MeSelectedPokemons.IndexOf(MePokemon6.Text));
            }
            ShowMeSelectedPokemons();
        }
        #endregion

        /// <summary>
        /// 選出ポケモンの表示
        /// </summary>
        private void ShowMeSelectedPokemons()
        {
            if (_InputedData.MeSelectedPokemons.Count == 0)
            {
                MeSelectedPokemon1.Text = "";
                MeSelectedPokemon2.Text = "";
                MeSelectedPokemon3.Text = "";
                MeSelectedPokemon4.Text = "";
            }
            else if (_InputedData.MeSelectedPokemons.Count == 1)
            {
                MeSelectedPokemon1.Text = _InputedData.MeSelectedPokemons.ElementAt(0);
                MeSelectedPokemon2.Text = "";
                MeSelectedPokemon3.Text = "";
                MeSelectedPokemon4.Text = "";
            }
            else if (_InputedData.MeSelectedPokemons.Count == 2)
            {
                MeSelectedPokemon1.Text = _InputedData.MeSelectedPokemons.ElementAt(0);
                MeSelectedPokemon2.Text = _InputedData.MeSelectedPokemons.ElementAt(1);
                MeSelectedPokemon3.Text = "";
                MeSelectedPokemon4.Text = "";
            }
            else if (_InputedData.MeSelectedPokemons.Count == 3)
            {
                MeSelectedPokemon1.Text = _InputedData.MeSelectedPokemons.ElementAt(0);
                MeSelectedPokemon2.Text = _InputedData.MeSelectedPokemons.ElementAt(1);
                MeSelectedPokemon3.Text = _InputedData.MeSelectedPokemons.ElementAt(2);
                MeSelectedPokemon4.Text = "";
            }
            else if (_InputedData.MeSelectedPokemons.Count == 4)
            {
                MeSelectedPokemon1.Text = _InputedData.MeSelectedPokemons.ElementAt(0);
                MeSelectedPokemon2.Text = _InputedData.MeSelectedPokemons.ElementAt(1);
                MeSelectedPokemon3.Text = _InputedData.MeSelectedPokemons.ElementAt(2);
                MeSelectedPokemon4.Text = _InputedData.MeSelectedPokemons.ElementAt(3);
                //List<TextBox> texboxes;
            }

        }

        //リストを、チェックボックスの操作から作成し、
        //そのリストを


        #region 相手ポケモンの選出
        /// <summary>
        /// 相手ポケモン1の選出
        /// </summary>
        private void EnemySelected1_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemyPokemon1.Text == "")
            {
                EnemySelected1.Checked = false;
                return;
            }
            if (EnemySelected1.Checked)
            {
                _InputedData.EnemySelectedPokemons.Add(EnemyPokemon1.Text);
            }
            else
            {
                _InputedData.EnemySelectedPokemons.RemoveAt(_InputedData.EnemySelectedPokemons.IndexOf(EnemyPokemon1.Text));
            }
            ShowEnemySelectedPokemons();
        }

        /// <summary>
        /// 相手ポケモン2の選出
        /// </summary>
        private void EnemySelected2_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemyPokemon2.Text == "")
            {
                EnemySelected2.Checked = false;
                return;
            }
            if (EnemySelected2.Checked)
            {
                _InputedData.EnemySelectedPokemons.Add(EnemyPokemon2.Text);
            }
            else
            {
                _InputedData.EnemySelectedPokemons.RemoveAt(_InputedData.EnemySelectedPokemons.IndexOf(EnemyPokemon2.Text));
            }
            ShowEnemySelectedPokemons();
        }

        /// <summary>
        /// 相手ポケモン3の選出
        /// </summary>
        private void EnemySelected3_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemyPokemon3.Text == "")
            {
                EnemySelected3.Checked = false;
                return;
            }
            if (EnemySelected3.Checked)
            {
                _InputedData.EnemySelectedPokemons.Add(EnemyPokemon3.Text);
            }
            else
            {
                _InputedData.EnemySelectedPokemons.RemoveAt(_InputedData.EnemySelectedPokemons.IndexOf(EnemyPokemon3.Text));
            }
            ShowEnemySelectedPokemons();
        }

        /// <summary>
        /// 相手ポケモン4の選出
        /// </summary>
        private void EnemySelected4_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemyPokemon4.Text == "")
            {
                EnemySelected4.Checked = false;
                return;
            }
            if (EnemySelected4.Checked)
            {
                _InputedData.EnemySelectedPokemons.Add(EnemyPokemon4.Text);
            }
            else
            {
                _InputedData.EnemySelectedPokemons.RemoveAt(_InputedData.EnemySelectedPokemons.IndexOf(EnemyPokemon4.Text));
            }
            ShowEnemySelectedPokemons();
        }

        /// <summary>
        /// 相手ポケモン5の選出
        /// </summary>
        private void EnemySelected5_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemyPokemon5.Text == "")
            {
                EnemySelected5.Checked = false;
                return;
            }
            if (EnemySelected5.Checked)
            {
                _InputedData.EnemySelectedPokemons.Add(EnemyPokemon5.Text);
            }
            else
            {
                _InputedData.EnemySelectedPokemons.RemoveAt(_InputedData.EnemySelectedPokemons.IndexOf(EnemyPokemon5.Text));
            }
            ShowEnemySelectedPokemons();
        }

        /// <summary>
        /// 相手ポケモン6の選出
        /// </summary>
        private void EnemySelected6_CheckedChanged(object sender, EventArgs e)
        {
            if (EnemyPokemon6.Text == "")
            {
                EnemySelected6.Checked = false;
                return;
            }
            if (EnemySelected6.Checked)
            {
                _InputedData.EnemySelectedPokemons.Add(EnemyPokemon6.Text);
            }
            else
            {
                _InputedData.EnemySelectedPokemons.RemoveAt(_InputedData.EnemySelectedPokemons.IndexOf(EnemyPokemon6.Text));
            }
            ShowEnemySelectedPokemons();
        }
        #endregion


        /// <summary>
        /// 相手選出ポケモンの表示
        /// </summary>
        private void ShowEnemySelectedPokemons()
        {
                EnemySelectedPokemon1.Text =(_InputedData.EnemySelectedPokemons.Count <= 0) ? "" : _InputedData.EnemySelectedPokemons.ElementAt(0);
                EnemySelectedPokemon2.Text = (_InputedData.EnemySelectedPokemons.Count <= 1) ? "" : _InputedData.EnemySelectedPokemons.ElementAt(1);
                EnemySelectedPokemon3.Text = (_InputedData.EnemySelectedPokemons.Count <= 2) ? "" : _InputedData.EnemySelectedPokemons.ElementAt(2);
                EnemySelectedPokemon4.Text = (_InputedData.EnemySelectedPokemons.Count <= 3) ? "" : _InputedData.EnemySelectedPokemons.ElementAt(3);
            if (_InputedData.EnemySelectedPokemons.Count == 0)
            {
                EnemySelectedPokemon1.Text = "";
                EnemySelectedPokemon2.Text = "";
                EnemySelectedPokemon3.Text = "";
                EnemySelectedPokemon4.Text = "";
            }
            else if (_InputedData.EnemySelectedPokemons.Count == 1)
            {
            }
            else if (_InputedData.EnemySelectedPokemons.Count == 2)
            {
                EnemySelectedPokemon1.Text = _InputedData.EnemySelectedPokemons.ElementAt(0);
                EnemySelectedPokemon2.Text = _InputedData.EnemySelectedPokemons.ElementAt(1);
                EnemySelectedPokemon3.Text = "";
                EnemySelectedPokemon4.Text = "";
            }
            else if (_InputedData.EnemySelectedPokemons.Count == 3)
            {
                EnemySelectedPokemon1.Text = _InputedData.EnemySelectedPokemons.ElementAt(0);
                EnemySelectedPokemon2.Text = _InputedData.EnemySelectedPokemons.ElementAt(1);
                EnemySelectedPokemon3.Text = _InputedData.EnemySelectedPokemons.ElementAt(2);
                EnemySelectedPokemon4.Text = "";
            }
            else if (_InputedData.EnemySelectedPokemons.Count == 4)
            {
                EnemySelectedPokemon1.Text = _InputedData.EnemySelectedPokemons.ElementAt(0);
                EnemySelectedPokemon2.Text = _InputedData.EnemySelectedPokemons.ElementAt(1);
                EnemySelectedPokemon3.Text = _InputedData.EnemySelectedPokemons.ElementAt(2);
                EnemySelectedPokemon4.Text = _InputedData.EnemySelectedPokemons.ElementAt(3);
            }
        }


        private void Origin_Load(object sender, EventArgs e)
        {
            AutoCompleteStringCollection autoCompleteList = new AutoCompleteStringCollection();
            testText.AutoCompleteCustomSource = autoCompleteList;

            //候補リストに項目を追加（初期候補）
            autoCompleteList.AddRange(
                    new string[] { "charactor", "value", "learning", "hate" }
                );

            //CSVから一覧を読み込ませて、それをセットする
        }


        private void testText_TextChanged(object sender, EventArgs e)
        {

            ////候補リストに項目を追加
            //String newItem = testText.Text.Trim();
            //if(!string.IsNullOrEmpty(newItem)
            //     && !autoCompleteList.Contains(newItem)){
            //         autoCompleteList.Add(newItem);
            //}

        }

        ///// <summary>
        ///// CSV書き込み
        ///// </summary>
        ///// <param name="fileName">保存するCSVの絶対パス</param>
        //public void SaveInCSV(string fileName)
        //{
        //    StreamWriter SW = new StreamWriter(fileName, false, Encoding.GetEncoding("shift_jis"));
        //    SW.WriteLine("回数,レート,相手レート,選出1,2,3,4,相手PT1,2,3,4,5,6,相手選出1,2,3,4,勝敗,メモ");

        //    int i = 0;
        //    while (_InputedData.Result.Count != 0)
        //    {
        //        //回数, 自分レート, 相手レート
        //        SW.Write((i + 1) + "," + _InputedData.MeRateList[i] + "," + _InputedData.EnemyRateList[i] + ",");

        //        //自分の選出
        //        for(int j = 0; j < 4 ; j++){
        //            if (_InputedData.MeSelectList[i][j] != "")
        //            {
        //                SW.Write(_InputedData.MeSelectList[i][j]);
        //            }
        //            else
        //            {
        //                //自分の選出が3匹以下であるときは空白を作る
        //            }
        //            SW.Write(",");
        //        }
        //        //相手のパーティ
        //        for (int j = 0; j < 6; j++)
        //        {
        //            if (_InputedData.EnemyPartiesList[i][j] != "")
        //            {
        //                SW.Write(_InputedData.EnemyPartiesList[i][j]);
        //            }
        //            else
        //            {
        //                //相手のパーティが5匹以下である時は空白を作る
        //            }
        //            SW.Write(",");
        //        }
        //        //相手の選出
        //        for (int j = 0; j < 4; j++)
        //        {
        //            if (_InputedData.EnemySelectList[i][j] != "")
        //            {
        //                SW.Write(_InputedData.EnemySelectList[i][j]);
        //            }
        //            else
        //            {
        //                //相手の選出が3匹以下であるときは空白を作る
        //            }
        //            SW.Write(",");
        //        }
        //        //勝敗
        //        if (_InputedData.Result[0])
        //        {
        //            SW.Write("w");
        //        }
        //        else
        //        {
        //            SW.Write("l");
        //        }
        //        SW.Write(",");

        //        _InputedData.Result.RemoveAt(0);

        //        //メモ
        //        SW.Write(_InputedData.MemoList[i]);

        //        //出力するセルを改行する
        //        SW.WriteLine(",");
        //        i++;
        //    }
        //    SW.Close();
        //}

    }


    /// <summary>
    /// 情報保持、CSV書き込み
    /// </summary>
    public class Process
    {

    }
}
