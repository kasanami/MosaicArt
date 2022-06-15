using MessagePack;
using System.Drawing;
using System.Text;

namespace MosaicArt
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// 複数の画像の情報
    /// </summary>
    [MessagePackObject(true)]
    public class ImagesInfo : VersionInfo
    {
        #region 定数
        public const int CurrentVersion = 2;
        /// <summary>
        /// 拡張子
        /// </summary>
        public const string PathExtension = ".imagesinfo";
        /// <summary>
        /// FastCompareで絞り込む数
        /// </summary>
        public const int FastCompareCount = 10;
        #endregion 定数

        public List<ImageInfo> ImageInfos { get; init; } = new List<ImageInfo>();
        public ImagesInfo() : base(CurrentVersion)
        {
        }
        public ImagesInfo(string directory) : base(CurrentVersion)
        {
            var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                try
                {
                    Bitmap bitmap = new (file);
                    ImageInfos.Add(new ImageInfo(file, bitmap));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // 読み込み失敗したら次へ
                }
            }
        }
        public void Save(string path)
        {
            byte[] bytes = MessagePackSerializer.Serialize(this);
            File.WriteAllBytes(path, bytes);
#if DEBUG
            // デバッグ用、JSONで確認
            var json = MessagePackSerializer.ConvertToJson(bytes);
            File.WriteAllText(path + ".json", json, Encoding.UTF8);
#endif
        }
        /// <summary>
        /// 画像ファイルが存在するか確認する
        /// すべてあればtrue
        /// </summary>
        /// <returns></returns>
        public bool ExistsFile()
        {
            foreach (var imageInfo in ImageInfos)
            {
                if (string.IsNullOrEmpty(imageInfo.Path) == false)
                {
                    if (File.Exists(imageInfo.Path) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 重複を削除する
        /// （ほぼ同じ画像をリストから削除する）
        /// </summary>
        public void RemoveDuplicates()
        {
            if (ImageInfos.Count <= 1)
            {
                return;
            }
            Parallel.ForEach(ImageInfos, item =>
            {
                item.MiniImage.UpdateBytesSum();
            });
            for (int i = 0; i < ImageInfos.Count; i++)
            {
                for (int j = i + 1; j < ImageInfos.Count; j++)
                {
                    if (ImageInfos[i].MiniImage.BytesSum != ImageInfos[j].MiniImage.BytesSum)
                    {
                        continue;
                    }
                    // NOTE:MiniImageが同じでもAverageRgbが同じとは限らない
                    if (ImageInfos[i].MiniImage.Equals(ImageInfos[j].MiniImage))
                    {
                        ImageInfos.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        public static new ImagesInfo Load(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return MessagePackSerializer.Deserialize<ImagesInfo>(bytes);
        }

        /// <summary>
        /// 引数のImageInfoに最も近いImageInfoを返す。
        /// </summary>
        public ImageInfo? GetNear(ImageInfo imageInfo)
        {
            IEnumerable<ImageInfo> imageInfos = ImageInfos;
            int count;

            // 予約済みは除外
            imageInfos = imageInfos.Where(item => item.IsReserved == false);
            // 高速比較である程度絞り込む
            count = Math.Min(FastCompareCount, imageInfos.Count());
            imageInfos = imageInfos.OrderBy(item => item.FastCompare(imageInfo)).Take(count);
            // 厳密に選ぶ
            return imageInfos.MinBy(item => item.Compare(imageInfo));
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}