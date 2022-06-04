using MessagePack;
using System.Drawing;
using System.Text;

namespace MosaicArt.Core
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// 複数の画像の情報
    /// </summary>
    [MessagePackObject(true)]
    public class ImagesInfo : VersionInfo
    {
        #region 定数
        public const int CurrentVersion = 1;
        /// <summary>
        /// 拡張子
        /// </summary>
        public const string PathExtension = ".imagesinfo";
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
                    Bitmap bitmap = new Bitmap(file);
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

            //var count = Math.Min(1000, imageInfos.Count());
            //imageInfos = imageInfos.OrderBy(item => item.PrimaryCompare(imageInfo)).Take(count);

            count = Math.Min(10, imageInfos.Count());
            imageInfos = imageInfos.OrderBy(item => item.SecondaryCompare(imageInfo)).Take(count);

            return imageInfos.MinBy(item => item.Compare(imageInfo));
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}