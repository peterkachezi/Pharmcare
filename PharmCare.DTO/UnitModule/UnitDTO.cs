

namespace PharmCare.DTO.UnitModule
{
   public  class UnitDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FullName => UnitValue + " " + Name;
        public int UnitValue { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string NewCreateDate { get { return CreateDate.ToShortDateString(); } }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
