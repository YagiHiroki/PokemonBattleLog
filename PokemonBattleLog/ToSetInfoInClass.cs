using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PokemonBattleLog.Dto;

namespace PokemonBattleLog
{
    static class ToSetInfoInClass
    {
        /// <summary>
        /// 自分情報を記録
        /// </summary>
        /// <param name="data">データを保持するクラス</param>
        /// <param name="select">選出したポケモン</param>
        /// <param name="rate">対戦前レート</param>
        /// <param name="winOrLoseResult">勝敗結果</param>
        /// <param name="supplement">補足メモ内容</param>
        public static InputedData SetMeResult(InputedData data,
            string[] select, int rate, bool winOrLoseResult, string supplement)
        {
            data.MeSelectList.Add(select);
            data.MeRateList.Add(rate);
            data.Result.Add(winOrLoseResult);
            data.MemoList.Add(supplement);
            return data;
            //_InputedData.MeSelectList.Add(select);
            //_InputedData.MeRateList.Add(rate);
            //_InputedData.Result.Add(winOrLoseResult);
            //_InputedData.MemoList.Add(supplement);
        }
    }
}
