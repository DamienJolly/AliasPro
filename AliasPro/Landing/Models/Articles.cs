using System.Data.Common;

namespace AliasPro.Landing.Models
{
    using Database;

    internal class Articles : IArticles
    {
        internal Articles(DbDataReader reader)
        {
            Id = reader.ReadData<uint>("id");
            Title = reader.ReadData<string>("title");
            Message = reader.ReadData<string>("text");
            Caption = reader.ReadData<string>("caption");
            Type = reader.ReadData<int>("type");
            Link = reader.ReadData<string>("link");
            Image = reader.ReadData<string>("image");
        }

        public uint Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Caption { get; set; }
        public int Type { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
    }
}
