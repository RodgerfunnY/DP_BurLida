namespace DP_BurLida.Data.ModelsData
{
    public class BrigadeModelData
    {
        public int Id { get; set; }
        public string NameBrigade { get; set; }
        public string Technic { get; set; }
        public string Info { get; set; }
        public int ResponsibleUserId { get; set; }
        public UserModelData ResponsibleUser { get; set; }
          
        public BrigadeModelData(int id, string nameBrigade, string technic,
                               string info, int responsibleUserId, UserModelData responsibleUser)
        {
            Id = id;
            NameBrigade = nameBrigade;
            Technic = technic;
            Info = info;
            ResponsibleUserId = responsibleUserId;
            ResponsibleUser = responsibleUser;
        }

        public BrigadeModelData() { }
    }
}
