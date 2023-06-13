namespace TabuSearchImplement
{
    public class Constant
    {
        public class WareHouseMaterial
        {
            public int Id { get; set; }
            public string? Code { get; set; }
            public int? Quantity { get; set; }
            public int? MinimumQuantity { get; set; }
            public string? IsRequestAdd { get; set; }
            public DateTime ExpectedAddDate { get; set; }

            public WareHouseMaterial(int id, string? code, int? quantity, int? minimumQuantity, string? isRequestAdd, DateTime expectedAddDate)
            {
                Id = id;
                Code = code;
                Quantity = quantity;
                MinimumQuantity = minimumQuantity;
                IsRequestAdd = isRequestAdd;
                ExpectedAddDate = expectedAddDate;
            }
        }

        public class Material
        {
            public int? Id { get; set; }
            public int? Priority { get; set; }
            public string? Device { get; set; }
            public string? Work { get; set; }
            public DateTime? DueDate { get; set; }
            public int? ExcutionTime { get; set; }
            public List<string>? ListNamePart { get; set; }
            public List<int>? ListSequencePart { get; set; }
            public List<int>? ListQuantityPart { get; set; }

            public Material()
            {

            }

            public Material(int? id, int? priority, string? device, string? work, DateTime? dueDate, int? excutionTime, List<string>? listNamePart, List<int>? listSequencePart, List<int>? listQuantityPart)
            {
                Id = id;
                Priority = priority;
                Device = device;
                Work = work;
                DueDate = dueDate;
                ExcutionTime = excutionTime;
                ListNamePart = listNamePart;
                ListSequencePart = listSequencePart;
                ListQuantityPart = listQuantityPart;
            }
        }

        

        public class WareHouseMaterialClass
        {
            public string id { get; set; }
            public string name { get; set; }
            public string quantity { get; set; }
            public string minimumQuantity { get; set; }
            public string isAddition { get; set; }
            public string expectedPartDate { get; set; }

            public WareHouseMaterialClass()
            {

            }

            public WareHouseMaterialClass(string id, string name, string quantity, string minimumQuantity, string isAddition, string expectedPartDate)
            {
                this.id = id;
                this.name = name;
                this.quantity = quantity;
                this.minimumQuantity = minimumQuantity;
                this.isAddition = isAddition;
                this.expectedPartDate = expectedPartDate;
            }
        }

        public class MaterialClass
        {
            public string Id { get; set; }
            public string Priority { get; set; }
            public string Device { get; set; }
            public string Work { get; set; }
            public string DueDate { get; set; }
            public string ExcutionTime { get; set; }
            public string PartList { get; set; }
            public string SequencePartList { get; set; }
            public string QuantityPart { get; set; }

            public MaterialClass()
            {

            }

            public MaterialClass(string id, string priority, string device, string work, string dueDate, string excutionTime, string partList, string sequencePartList, string quantityPart)
            {
                Id = id;
                Priority = priority;
                Device = device;
                Work = work;
                DueDate = dueDate;
                ExcutionTime = excutionTime;
                PartList = partList;
                SequencePartList = sequencePartList;
                QuantityPart = quantityPart;
            }
        }

        public class DeviceClass
        {
            public string Id { get; set; }
            public string DeviceName { get; set; }
            public string Date { get; set; }
            public string From1 { get; set; }
            public string To1 { get; set; }
            public string From2 { get; set; }
            public string To2 { get; set; }

            public DeviceClass()
            {

            }

            public DeviceClass(string id, string deviceName, string date, string from1, string to1, string from2, string to2)
            {
                Id = id;
                DeviceName = deviceName;
                Date = date;
                From1 = from1;
                To1 = to1;
                From2 = from2;
                To2 = to2;
            }
        }

        public class TechnicianClass
        {
            public string Id { get; set; }
            public string TechnicianName { get; set; }
            public string Date { get; set; }
            public string From1 { get; set; }
            public string To1 { get; set; }
            public string From2 { get; set; }
            public string To2 { get; set; }

            public TechnicianClass()
            {

            }

            public TechnicianClass(string id, string technicianName, string date, string from1, string to1, string from2, string to2)
            {
                Id = id;
                TechnicianName = technicianName;
                Date = date;
                From1 = from1;
                To1 = to1;
                From2 = from2;
                To2 = to2;
            }
        }
    }
}
