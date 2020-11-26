using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using PokemonBattleLog.Dto;

namespace PokemonBattleLog.Output
{
    class OutputResult
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OutputResult()
        {
        }

        /// <summary>
        /// 自分のパーティを書き込む
        /// </summary>
        public void Save(string fileName, InputedData _InputedData)
        {
            StreamWriter SW = new StreamWriter(fileName, false, Encoding.GetEncoding("shift_jis"));
            for (int i = 0; i < 6; i++)
            {
                //MeParty[]では曖昧になる
                //SW.Write(_InputedData.MeParty[i] + ",");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void WriteTemplate()
        {

        }


        /// <summary>
        /// CSV書き込み
        /// </summary>
        /// <param name="fileName">保存するCSVの絶対パス</param>
        /// <param name="_InputedData">保存するPT情報</param>
        public void SaveInCSV(string fileName, InputedData _InputedData)
        {
            StreamWriter SW = new StreamWriter(fileName, false, Encoding.GetEncoding("shift_jis"));
            SW.WriteLine("回数,レート,相手レート,選出1,2,3,4,相手PT1,2,3,4,5,6,相手選出1,2,3,4,勝敗,メモ");

            int i = 0;
            while (_InputedData.Result.Count != 0)
            {
                //回数, 自分レート, 相手レート
                SW.Write((i + 1) + "," + _InputedData.MeRateList[i] + "," + _InputedData.EnemyRateList[i] + ",");

                //自分の選出
                SaveMeSelect(SW, _InputedData, i);

                ////自分の選出
                //for (int j = 0; j < 4; j++)
                //{
                //    if (_InputedData.MeSelectList[i][j] != "")
                //    {
                //        SW.Write(_InputedData.MeSelectList[i][j]);
                //    }
                //    else
                //    {
                //        //自分の選出が3匹以下であるときは空白を作る
                //    }
                //    SW.Write(",");
                //}
                ////相手のパーティ
                for (int j = 0; j < 6; j++)
                {
                    if (_InputedData.EnemyPartiesList[i][j] != "")
                    {
                        SW.Write(_InputedData.EnemyPartiesList[i][j]);
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
                    if (_InputedData.EnemySelectList[i][j] != "")
                    {
                        SW.Write(_InputedData.EnemySelectList[i][j]);
                    }
                    else
                    {
                        //相手の選出が3匹以下であるときは空白を作る
                    }
                    SW.Write(",");
                }
                //勝敗
                if (_InputedData.Result[0])
                {
                    SW.Write("w");
                }
                else
                {
                    SW.Write("l");
                }
                SW.Write(",");

                _InputedData.Result.RemoveAt(0);

                //メモ
                SW.Write(_InputedData.MemoList[i]);

                //出力するセルを改行する
                SW.WriteLine(",");
                i++;
            }
            SW.Close();
        }

        /// <summary>
        /// 自分の選出を書き込む
        /// </summary>
        /// <param name="SW">書き込むファイル</param>
        /// <param name="inputedData">選出情報</param>
        /// <param name="battleTimes">対戦回数-1</param>
        private void SaveMeSelect(StreamWriter SW, InputedData inputedData, int battleTimes)
        {
            for (int j = 0; j < 4; j++)
            {
                if (inputedData.MeSelectList[battleTimes][j] != "")
                {
                    SW.Write(inputedData.MeSelectList[battleTimes][j]);
                }
                SW.Write(",");
            }
        }



        /// <summary>
        /// 既存のCSVに追記する
        /// </summary>
        /// <param name="fileName">保存するCSVの絶対パス</param>
        /// <param name="_InputedData">保存するPT情報</param>
        /// <param name="isOverwrite">上書きするかどうか</param>
        public void SaveInCSV2(string fileName, InputedData _InputedData, bool isOverwrite)
        {
            int recordNumber = 1;

            //存在確認
            //対戦回数を確認する
            if (File.Exists(fileName))
            {
                using (StreamReader SR = new StreamReader(fileName))
                {
                    int lineCount = 0;
                    while (SR.Peek() > 0)
                    {
                        SR.ReadLine();
                        lineCount++;
                    }
                    recordNumber = lineCount - 2;
                }
            }

            ////追記の場合、対戦回数を確認する
            //try
            //{
            //    using (StreamReader SR = new StreamReader(fileName))
            //    {
            //        int lineCount = 0;
            //        while (SR.Peek() > 0)
            //        {
            //            SR.ReadLine();
            //            lineCount++;
            //        }
            //        recordNumber = lineCount-2;
            //    }
            //}
            //catch (FileNotFoundException)
            //{
            //}
            //finally
            //{
            //}

            using(StreamWriter SW= new StreamWriter(fileName, isOverwrite, Encoding.GetEncoding("shift_jis"))){
                //streamWriterはusingの中だけで使う
                //usingの中でだけ書き込み処理をする


                //新規作成の場合、1行目にヘッダ情報を書き込む
                if (!isOverwrite)
                {
                    //データをもつクラス（InputedData）と、ｓ
                    //データを作るクラス（ラッパー）を作って、
                    //OutputResultでは、書くだけの状態を作る
                    //xx,xx,xx,xx,...のテキストを持たせる

                    SW.WriteLine("自分PT1,2,3,4,5,6");
                    
                    for (int j = 0; j < 6; j++)
                    {
                        if (_InputedData.MeParty[j] != "")
                        {
                            SW.Write(_InputedData.MeParty[j]);
                        }
                        else
                        {
                            //相手のパーティが5匹以下である時は空白を作る
                        }
                        SW.Write(",");
                    }
                    SW.WriteLine("");
                    SW.WriteLine("回数,レート,相手レート,選出1,2,3,4,相手PT1,2,3,4,5,6,相手選出1,2,3,4,勝敗,メモ");
                }

                int i = 0;
                while (_InputedData.Result.Count != 0)
                {
                    //回数, 自分レート, 相手レート
                    SW.Write(recordNumber + "," + _InputedData.MeRateList[i] + "," + _InputedData.EnemyRateList[i] + ",");

                    ////自分の選出
                    //SaveMeSelect(SW, _InputedData, i);

                    //自分の選出
                    for (int j = 0; j < 4; j++)
                    {
                        if (_InputedData.MeSelectList[i][j] != "")
                        {
                            SW.Write(_InputedData.MeSelectList[i][j]);
                        }
                        else
                        {
                            //自分の選出が3匹以下であるときは空白を作る
                        }
                        SW.Write(",");
                    }
                    ////相手のパーティ
                    for (int j = 0; j < 6; j++)
                    {
                        if (_InputedData.EnemyPartiesList[i][j] != "")
                        {
                            SW.Write(_InputedData.EnemyPartiesList[i][j]);
                        }
                        else
                        {
                            //相手のパーティが5匹以下である時は空白を作る
                        }
                        SW.Write(",");
                    }
                    
                    //相手の選出
                    //for (int j = 0; j < 4; j++)
                    //{
                        //空白が入ってるならそれが出力されるなら、if文が必要ない
                        //タブがスペースに置き換わっている（VSの設定）
                        //string.join
                        //

                        //

                        //if (_InputedData.EnemySelectList[i][j] != "")
                        //{
                        //    SW.Write(_InputedData.EnemySelectList[i][j]);
                        //}
                        //else
                        //{
                        //    //相手の選出が3匹以下であるときは空白を作る
                        //}
                        
                        SW.Write(string.Join(",", _InputedData.EnemySelectList[i])+",");
                    //}

                    //勝敗
                    if (_InputedData.Result[0])
                    {
                        SW.Write("w");
                    }
                    else
                    {
                        SW.Write("l");
                    }
                    SW.Write(",");

                    //true="w", false="l"のresultメソッドを仕込む


                    _InputedData.Result.RemoveAt(0);

                    //メモ
                    SW.Write(_InputedData.MemoList[i]);

                    //出力するセルを改行する
                    SW.WriteLine(",");
                    i++;
                    recordNumber++;
                }

            }
            
        }
    }
}
