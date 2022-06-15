using MessagePack;
using System.Text;

namespace MosaicArt.TestApp
{
    /// <summary>
    /// レポート
    /// </summary>
    [MessagePackObject(true)]
    public class Report
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public TimeSpan ElapsedTime;
        public int MiniImageWidth;
        public int MiniImageHeight;
        public int FastCompareCount;

        public Parameters Parameters = new();

        public Report() { }
        public void Save(string path)
        {
            var json = MessagePackSerializer.SerializeToJson(this);
            File.WriteAllText(path, json, Encoding.UTF8);
        }
    }
}