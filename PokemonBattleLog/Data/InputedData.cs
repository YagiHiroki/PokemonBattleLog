using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleLog.Data
{
    class InputedData
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>結果を出力させた後、持ってるデータを破棄するため
        /// 初期化をまとめておく</remarks>
        public InputedData()
        {
            //パーティ
            MeParty = new string[6];
            
            EnemyPartiesList = new List<string[]>();

            //選出したポケモン
            _MeSelectedPokemons = new List<string>();
            _EnemySelectedPokemons = new List<string>();

            //全試合の選出したポケモンのリスト
            _MeSelectList = new List<string[]>();
            _EnemySelectList = new List<string[]>();

            //レートの値
            _MeRateList = new List<int>();
            _EnemyRateList = new List<int>();

            //勝敗結果
            _Result = new List<bool>();
            
            //メモ欄
            _MemoList = new List<string>();
        }

        #region パーティ
        /// <summary>
        /// 自分のパーティ
        /// </summary>
        public string[] MeParty { get; private set; }
        //private string[] _MeParty;
        //public string[] MeParty
        //{
        //    get { return _MeParty; }
        //    set { _MeParty = value; }
        //}
        

        /// <summary>
        /// 相手のパーティ
        /// </summary>
        public List<string[]> EnemyPartiesList { get; private set; }
        //private List<string[]> _EnemyPartiesList;
        //public List<string[]> EnemyPartiesList
        //{
        //    get { return _EnemyPartiesList; }
        //    set { _EnemyPartiesList = value;}
        //}
        #endregion

        #region 選出したポケモン
        /// <summary>
        /// 自分が1試合で選出したポケモン
        /// </summary>
        private List<string> _MeSelectedPokemons;
        public List<string> MeSelectedPokemons
        {
            get { return _MeSelectedPokemons; }
            set { _MeSelectedPokemons = value; }
        }

        /// <summary>
        /// 相手が1試合で選出したポケモン
        /// </summary>
        private List<string> _EnemySelectedPokemons;
        public List<string> EnemySelectedPokemons 
        {
            get { return _EnemySelectedPokemons;}
            set { _EnemySelectedPokemons = value;}
        }
        #endregion

        #region 全試合の選出結果リスト
        /// <summary>
        /// 自分の全試合の選出結果
        /// </summary>
        private List<string[]> _MeSelectList;
        public List<string[]> MeSelectList
        {
            get { return _MeSelectList; }
            set { _MeSelectList = value; }
        }

        /// <summary>
        /// 全相手の選出結果
        /// </summary>
        private List<string[]> _EnemySelectList;
        public List<string[]> EnemySelectList
        {
            get { return _EnemySelectList; }
            set { _EnemySelectList = value;  }
        }
        #endregion

        #region レート
        /// <summary>
        /// 自分のレート
        /// </summary>
        private List<int> _MeRateList;
        public List<int> MeRateList
        {
            get { return _MeRateList; }
            set { _MeRateList = value; }
        }

        /// <summary>
        /// 相手のレート
        /// </summary>
        private List<int> _EnemyRateList;
        public List<int> EnemyRateList
        {
            get { return _EnemyRateList; }
            set { _EnemyRateList = value; }
        }
        #endregion

        /// <summary>
        /// 勝敗判定
        /// </summary>
        private List<bool> _Result;
        public List<bool> Result
        {
            get { return _Result; }
            set { _Result = value; }
        }

        /// <summary>
        /// 戦法などのメモ
        /// </summary>
        private List<string> _MemoList;
        public List<string> MemoList
        {
            get { return _MemoList; }
            set { _MemoList = value; }
        }
    }
}
