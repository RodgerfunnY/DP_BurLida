namespace DP_BurLida.Data.ModelsData
{
    public class SkladModelData
    {
        public int Id { get; set; }
        public string NameSubjecte { get; set; }
        public string Category { get; set; }
        public string Info { get; set; }
        public int Quantity { get; set; }

        public SkladModelData(int id, string nameSubjecte, string category,
                               string info, int quantity)
        {
            Id = id;
            NameSubjecte = nameSubjecte;
            Category = category;
            Info = info;
            Quantity = quantity;
        }

        public SkladModelData() { }
    }
}
