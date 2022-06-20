using MessagePack;
using System.Drawing;
using System.Text;

namespace MosaicArt.TestApp
{
#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
    [MessagePackObject(true)]
    public class Parameters
    {
        /// <summary>
        /// 同時実行タスクの最大数
        /// </summary>
        public int MaxDegreeOfParallelism;
        /// <summary>
        /// 乱数生成器のシード値
        /// </summary>
        public int RandomSeed;

        /// <summary>
        /// 素材を格納しているフォルダ
        /// </summary>
        public string ResourceDirectoryPath = "";
        /// <summary>
        /// 素材動画を分割する枚数
        /// </summary>
        public int MovieSliceCount;

        /// <summary>
        /// 目標画像のパス
        /// </summary>
        public string TargetImagePath = "";
        /// <summary>
        /// 目標画像の横軸の分割数
        /// </summary>
        public int DivisionsX;
        /// <summary>
        /// 目標画像の縦軸の分割数
        /// </summary>
        public int DivisionsY;

        public void Save(string path)
        {
            var json = MessagePackSerializer.SerializeToJson(this);
            File.WriteAllText(path, json, Encoding.UTF8);
        }
    }
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
}