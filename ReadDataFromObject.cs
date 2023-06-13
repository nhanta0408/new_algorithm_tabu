using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TabuSearchImplement.AggregateModels.DeviceAggregate;
using TabuSearchImplement.AggregateModels.InputAggregate;
using TabuSearchImplement.AggregateModels.MaterialAggregate;
using TabuSearchImplement.AggregateModels.TechnicianAggregate;
using TabuSearchImplement.AggregateModels.WareHouseMaterialAggregate;
using TabuSearchImplement.AggregateModels.WorkAggregate;
using static TabuSearchImplement.Constant;

namespace TabuSearchImplement
{
    public class ReadDataFromObject
    {
        public static ObjectInput CreateInputs()
        {
            string filePath = @"D:\Scheduling Maintenance\data input\JsonInputTabuSearchAlgorithmService.txt";
            StreamReader sr = new StreamReader(filePath);
            string json = sr.ReadToEnd();
            //Console.WriteLine(json);
            ObjectInput objectInput = JsonSerializer.Deserialize<ObjectInput>(json);
            return objectInput;
        }

        public static List<WorkObjectInput> CreateWorks()
        {
            string filePath = @"D:\Scheduling Maintenance\data input\WorksJson_40.txt";
            StreamReader sr = new StreamReader(filePath);
            string json = sr.ReadToEnd();
            //Console.WriteLine(json);
            WorkObjectInput[] workObject = JsonSerializer.Deserialize<WorkObjectInput[]>(json);

            List<WorkObjectInput> listWorkObjects = workObject.ToList();
            return listWorkObjects;
        }

        public static List<DeviceObjectInput> CreateDevices()
        {
            string filePath = @"D:\Scheduling Maintenance\data input\DevicesJsonPrevious.txt";
            StreamReader sr = new StreamReader(filePath);
            string json = sr.ReadToEnd();
            //Console.WriteLine(json);
            DeviceObjectInput[] deviceObject = JsonSerializer.Deserialize<DeviceObjectInput[]>(json);

            List<DeviceObjectInput> listDeviceObjects = deviceObject.ToList();

            return listDeviceObjects;
        }

        public static List<TechnicianObjectInput> CreateTechnicians()
        {
            string filePath = @"D:\Scheduling Maintenance\data input\TechniciansJson_10.txt";
            StreamReader sr = new StreamReader(filePath);
            string json = sr.ReadToEnd();
            //Console.WriteLine(json);
            TechnicianObjectInput[] technicianObject = JsonSerializer.Deserialize<TechnicianObjectInput[]>(json);

            List<TechnicianObjectInput> listTechnicianObjects = technicianObject.ToList();
            //Console.WriteLine($"The technician Json is Deserialize successful with the lengt: {listTechnicianObjects.Count}");

            return listTechnicianObjects;
        }


        public static List<WareHouseMaterialObjectInput> CreateWareHouseMaterials()
        {
            string filePath = @"D:\Scheduling Maintenance\data input\WareHouseMaterialsJson.txt";
            StreamReader sr = new StreamReader(filePath);
            string json = sr.ReadToEnd();
            //Console.WriteLine(json);
            WareHouseMaterialObjectInput[] wareHouseMaterialObject = JsonSerializer.Deserialize<WareHouseMaterialObjectInput[]>(json);

            List<WareHouseMaterialObjectInput> listWareHouseMaterialObjects = wareHouseMaterialObject.ToList();
            return listWareHouseMaterialObjects;
        }

        public static List<WareHouseMaterialClass> CreateSpareParts()
        {
            //string filePath = @"D:\Scheduling Maintenance\data input\sparePartList.csv";

            string filePath = @"D:\Scheduling Maintenance\data input\SparePartList.csv";
            List<WareHouseMaterialClass> listSparePartObjects = new List<WareHouseMaterialClass>();
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        var sparePartObject = new WareHouseMaterialClass();
                        sparePartObject.id = line.Split(',')[0];
                        sparePartObject.name = line.Split(',')[1];
                        sparePartObject.quantity = line.Split(',')[2];
                        sparePartObject.minimumQuantity = line.Split(',')[3];
                        sparePartObject.isAddition = line.Split(',')[4];
                        sparePartObject.expectedPartDate = line.Split(',')[5];
                        listSparePartObjects.Add(sparePartObject);
                    }

                    //Preprocessing to sort job orders, the higher the priority get the smaller the order number
                    //workTable.DefaultView.Sort = "Priority";
                    //workTable = workTable.DefaultView.ToTable();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //foreach (DataRow row in sparePartTable.Rows)
            //{
            //    Console.WriteLine(row["No"] + " " + row["Part"] + " " + row["Quantity"] + " " + row["MinimumQuantity"] + " " + row["IsAddition"] + " " + row["ExpectedPartDate"]);
            //}

            return listSparePartObjects;
        }

        /// <summary>
        /// Create a DataTable of spare Part in Warehouse
        /// </summary>
        /// <returns></returns>
        public static List<MaterialClass> CreateSparePartOnWorkObjectInput()
        {
            //string filePath = @"D:\Scheduling Maintenance\data input\sparePartList.csv";

            string filePath = @"D:\Scheduling Maintenance\data input\SparePartOnWork.csv";
            List<MaterialClass> listSparePartOnWorkObjects = new List<MaterialClass>();
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    reader.ReadLine();
                    while ((line = reader.ReadLine()) != null)
                    {
                        var sparePartObjectOnWork = new MaterialClass();
                        sparePartObjectOnWork.Id = line.Split(',')[0];
                        sparePartObjectOnWork.Priority = line.Split(',')[1];
                        sparePartObjectOnWork.Device = line.Split(',')[2];
                        sparePartObjectOnWork.Work = line.Split(',')[3];
                        sparePartObjectOnWork.DueDate = line.Split(',')[4];
                        sparePartObjectOnWork.ExcutionTime = line.Split(',')[5];
                        sparePartObjectOnWork.PartList = line.Split(',')[6];
                        sparePartObjectOnWork.SequencePartList = line.Split(',')[7];
                        sparePartObjectOnWork.QuantityPart = line.Split(',')[8];
                        listSparePartOnWorkObjects.Add(sparePartObjectOnWork);
                    }

                    //Preprocessing to sort job orders, the higher the priority get the smaller the order number
                    //workTable.DefaultView.Sort = "Priority";
                    //workTable = workTable.DefaultView.ToTable();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //foreach (DataRow row in sparePartOnWorkTable.Rows)
            //{
            //    Console.WriteLine(row["No"] + " " + row["Priority"] + " " + row["Device"] + " " + row["Work"] + " " + row["DueDate"] + " " + row["ExecutionTime"] + " " + row["PartList"] + " " + row["SequencePartList"] + " " + row["QuantityPart"]);
            //}

            return listSparePartOnWorkObjects;
        }
    }
}
