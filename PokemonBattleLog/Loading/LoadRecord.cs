using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace PokemonBattleLog.Loading
{
    class LoadRecord
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LoadRecord()
        {
        }

        /// <summary>
        /// csvの情報を読み出す
        /// </summary>
        /// <param name="fileName">開くファイル名</param>
        public void Load(string fileName)
        {
            StreamReader SR = new StreamReader(fileName, Encoding.GetEncoding("shift_jis"), false);
            
            //1行目。「自分PT1,2,3,4,5,6」
            SR.ReadLine();

            //2行目。
            SR.ReadLine().Split(',');

            //3行目。「レート,相手レート,...」
            SR.ReadLine();
        }
    }
}
