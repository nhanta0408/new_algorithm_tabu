using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabuSearchImplement.AggregateModels.DeviceAggregate;
using TabuSearchImplement.AggregateModels.MaterialAggregate;
using TabuSearchImplement.AggregateModels.TechnicianAggregate;
using TabuSearchImplement.AggregateModels.WorkAggregate;
using static TabuSearchImplement.Constant;

namespace TabuSearchImplement
{
    public class ConvertFromObjectToTable
    {
        public static DataTable ConvertObjectInputToWorksTable(List<WorkObjectInput> listWorkObjects)
        {
            DataTable workTable = new DataTable();
            workTable.Columns.Add("No");
            workTable.Columns.Add("Priority");
            workTable.Columns.Add("Device");
            workTable.Columns.Add("Work");
            workTable.Columns.Add("DueDate");
            workTable.Columns.Add("ExecutionTime");
            foreach(WorkObjectInput temp in listWorkObjects)
            {
                DataRow newRow = workTable.NewRow();
                newRow["No"] = temp.id;
                newRow["Priority"] = temp.priority;
                newRow["Device"] = temp.deviceCode;
                newRow["Work"] = temp.problem;
                newRow["DueDate"] = temp.dueDate.ToString("yyyy/MM/dd HH:mm");
                newRow["ExecutionTime"] = temp.estProcessTime;
                workTable.Rows.Add(newRow);
            }


            //foreach (DataRow row in workTable.Rows)
            //{
            //    Console.WriteLine(row["No"] + " " + row["Priority"] + " " + row["Device"] + " " + row["Work"] + " " + row["DueDate"] + " " + row["ExecutionTime"]);
            //}

            return workTable;
        }

        public static DataTable ConvertClassToDeviceTable(List<DeviceClass> listDeviceObjects)
        {
            DataTable deviceTable = new DataTable();
            deviceTable.Columns.Add("No");
            deviceTable.Columns.Add("Device");
            deviceTable.Columns.Add("Date");
            deviceTable.Columns.Add("From1");
            deviceTable.Columns.Add("To1");
            deviceTable.Columns.Add("From2");
            deviceTable.Columns.Add("To2");

            foreach(DeviceClass temp  in listDeviceObjects)
            {
                DataRow newRow = deviceTable.NewRow();
                newRow["No"] = temp.Id;
                newRow["Device"] = temp.DeviceName;
                newRow["Date"] = temp.Date;
                newRow["From1"] = temp.From1;
                newRow["To1"] = temp.To1;
                newRow["From2"] = temp.From2;
                newRow["To2"] = temp.To2;
                deviceTable.Rows.Add(newRow);
            }
            
            //foreach (DataRow row in deviceTable.Rows)
            //{
            //    Console.WriteLine(row["No"] + " " + row["Device"] + " " + row["Date"] + " " + row["From1"] + " " + row["To1"] + " " + row["From2"] + " " + row["To2"]);
            //}

            return deviceTable;
        }


        public static DataTable ConvertClassToTechnicianTable(List<TechnicianClass> listTechnicianObjects)
        {
            string filePath = @"../../../resources/Technicians.csv";
            DataTable technicianTable = new DataTable();
            technicianTable.Columns.Add("No");
            technicianTable.Columns.Add("Technician");
            technicianTable.Columns.Add("Date");
            technicianTable.Columns.Add("From1");
            technicianTable.Columns.Add("To1");
            technicianTable.Columns.Add("From2");
            technicianTable.Columns.Add("To2");

            foreach(TechnicianClass temp in listTechnicianObjects)
            {
                DataRow newRow = technicianTable.NewRow();
                newRow["No"] = temp.Id;
                newRow["Technician"] = temp.TechnicianName;
                newRow["Date"] = temp.Date;
                newRow["From1"] = temp.From1;
                newRow["To1"] = temp.To1;
                newRow["From2"] = temp.From2;
                newRow["To2"] = temp.To2;
                technicianTable.Rows.Add(newRow);
            }
            
            //foreach (DataRow row in technicianTable.Rows)
            //{
            //    Console.WriteLine(row["No"] + " " + row["Technician"] + " " + row["Date"] + " " + row["From1"] + " " + row["To1"] + " " + row["From2"] + " " + row["To2"]);
            //}

            return technicianTable;
        }


        public static DataTable ConvertObjectInputToTechnicianTable(List<TechnicianObjectInput> listTechnicianObjects)
        {
            DataTable technicianTable = new DataTable();
            technicianTable.Columns.Add("No");
            technicianTable.Columns.Add("Technician");
            technicianTable.Columns.Add("From1");
            technicianTable.Columns.Add("To1");
            technicianTable.Columns.Add("From2");
            technicianTable.Columns.Add("To2");

            foreach (TechnicianObjectInput temp in listTechnicianObjects)
            {
                DataRow newRow = technicianTable.NewRow();
                newRow["No"] = temp.id;
                newRow["Technician"] = temp.name;
                foreach(WorkingTime[] time in temp.workingTimes)
                {
                    newRow["From1"] = time[0].from.ToString();
                    newRow["To1"] = time[0].to.ToString();
                    newRow["From2"] = time[1].from.ToString();
                    newRow["To2"] = time[1].to.ToString();
                }
                technicianTable.Rows.Add(newRow);
            }

            //foreach (DataRow row in technicianTable.Rows)
            //{
            //    Console.WriteLine(row["No"] + " " + row["Technician"] + " " + row["From1"] + " " + row["To1"] + " " + row["From2"] + " " + row["To2"]);
            //}

            return technicianTable;
        }


        public static DataTable ConvertToWareHouseMaterialTable(List<WareHouseMaterial> listwareHouseMaterials)
        {
            DataTable sparePartTable = new DataTable();
            sparePartTable.Columns.Add("No");
            sparePartTable.Columns.Add("Part");
            sparePartTable.Columns.Add("Quantity");
            sparePartTable.Columns.Add("MinimumQuantity");
            sparePartTable.Columns.Add("IsAddition");
            sparePartTable.Columns.Add("ExpectedPartDate");

            foreach(WareHouseMaterial temp in listwareHouseMaterials)
            {
                DataRow newRow = sparePartTable.NewRow();
                newRow["No"] = temp.Id;
                newRow["Part"] = temp.Code;
                newRow["Quantity"] = temp.Quantity;
                newRow["MinimumQuantity"] = temp.MinimumQuantity;
                newRow["IsAddition"] = temp.IsRequestAdd;
                newRow["ExpectedPartDate"] = temp.ExpectedAddDate;
                sparePartTable.Rows.Add(newRow);
            }
            
            //foreach (DataRow row in sparePartTable.Rows)
            //{
            //    Console.WriteLine(row["No"] + " " + row["Part"] + " " + row["Quantity"] + " " + row["MinimumQuantity"] + " " + row["IsAddition"] + " " + row["ExpectedPartDate"]);
            //}

            return sparePartTable;
        }

        public static DataTable ConvertToMaterialTable(List<Material> listMaterial)
        {
            DataTable sparePartOnWorkTable = new DataTable();
            sparePartOnWorkTable.Columns.Add("No");
            sparePartOnWorkTable.Columns.Add("Priority");
            sparePartOnWorkTable.Columns.Add("Device");
            sparePartOnWorkTable.Columns.Add("Work");
            sparePartOnWorkTable.Columns.Add("DueDate");
            sparePartOnWorkTable.Columns.Add("ExecutionTime");
            sparePartOnWorkTable.Columns.Add("PartList");
            sparePartOnWorkTable.Columns.Add("SequencePartList");
            sparePartOnWorkTable.Columns.Add("QuantityPart");

            foreach(Material temp in listMaterial)
            {
                DataRow newRow = sparePartOnWorkTable.NewRow();
                newRow["No"] = temp.Id;
                newRow["Priority"] = temp.Priority;
                newRow["Device"] = temp.Device;
                newRow["Work"] = temp.Work;
                newRow["DueDate"] = temp.DueDate;
                newRow["ExecutionTime"] = temp.ExcutionTime;
                newRow["PartList"] = temp.ListNamePart;
                newRow["SequencePartList"] = temp.ListSequencePart;
                newRow["QuantityPart"] = temp.ListQuantityPart;
                sparePartOnWorkTable.Rows.Add(newRow);
            }
            
            //foreach (DataRow row in sparePartOnWorkTable.Rows)
            //{
            //    Console.WriteLine(row["No"] + " " + row["Priority"] + " " + row["Device"] + " " + row["Work"] + " " + row["DueDate"] + " " + row["ExecutionTime"] + " " + row["PartList"] + " " + row["SequencePartList"] + " " + row["QuantityPart"]);
            //}

            return sparePartOnWorkTable;
        }

        public static Dictionary<string, List<List<DateTime>>> getDeviceDictionary(List<DeviceObjectInput> listDeviceObjects)
        {
            Dictionary<string, List<List<DateTime>>> deviceDictionary = new Dictionary<string, List<List<DateTime>>>();
            List<DateTime> listDateTimeTemp = new List<DateTime>();
            List<List<DateTime>> listListTimeDevice = new List<List<DateTime>>();

            //var firstDateOfWeek = DateTime.ParseExact("24/04/2023 00:00", "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            //var endDateOfWeek = DateTime.ParseExact("28/04/2023 23:59", "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

            var firstDateOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var endDateOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);

            // We create a listDevice contains all of device's name and listDateTimeTemp contains all of break time of all devices.
            foreach (DeviceObjectInput device in listDeviceObjects)
            {
                listListTimeDevice = new List<List<DateTime>>();
                if (device.workingTimes != null)
                {
                    if (device.workingTimes.Length > 0)
                    {
                        for (int i = 0; i < device.workingTimes.Length; i++)
                        {
                            if (i == 0)
                            {
                                listDateTimeTemp = new List<DateTime>
                            {
                                firstDateOfWeek,
                                device.workingTimes[i].from
                            };
                                listListTimeDevice.Add(listDateTimeTemp);
                            }
                            else
                            {
                                listDateTimeTemp = new List<DateTime>
                            {
                                device.workingTimes[i - 1].to,
                                device.workingTimes[i].from
                            };
                                listListTimeDevice.Add(listDateTimeTemp);
                            }
                        }

                        listDateTimeTemp = new List<DateTime>
                            {
                                device.workingTimes[device.workingTimes.Length - 1].to,
                                endDateOfWeek
                            };
                        listListTimeDevice.Add(listDateTimeTemp);
                    }
                    else
                    {
                        listDateTimeTemp = new List<DateTime>
                            {
                                firstDateOfWeek,
                                endDateOfWeek
                            };
                        listListTimeDevice.Add(listDateTimeTemp);
                    }
                }

                deviceDictionary.Add(device.code, listListTimeDevice);
            }

            //Print all of dictionary
            //foreach (string key in deviceDictionary.Keys)
            //{
            //    Console.WriteLine($"The name of device: {key}");
            //    foreach (List<DateTime> listTime in deviceDictionary[key])
            //    {
            //        Console.WriteLine(listTime[0].ToString() + " " + listTime[1].ToString());
            //    }
            //}

            return deviceDictionary;
        }


        public static Dictionary<string, List<List<DateTime>>> getTechnicianDictionary(List<TechnicianObjectInput> listTechnicianObjects)
        {
            Dictionary<string, List<List<DateTime>>> technicianDictionary = new Dictionary<string, List<List<DateTime>>>();
            List<DateTime> listDateTimeTemp = new List<DateTime>();
            List<List<DateTime>> listListTimeTechnician = new List<List<DateTime>>();

            foreach (TechnicianObjectInput technician in listTechnicianObjects)
            {
                listListTimeTechnician = new List<List<DateTime>>();
                foreach (WorkingTime[] workingTimeArray in technician.workingTimes)
                {
                    listDateTimeTemp = new List<DateTime>
                    {
                        workingTimeArray[0].from,
                        workingTimeArray[0].to
                    };
                    listListTimeTechnician.Add(listDateTimeTemp);
                    listDateTimeTemp = new List<DateTime>
                    {
                        workingTimeArray[1].from,
                        workingTimeArray[1].to
                    };
                    listListTimeTechnician.Add(listDateTimeTemp);
                }
                technicianDictionary.Add(technician.id, listListTimeTechnician);
            }


            // Print all of dictionary
            //foreach (string key in technicianDictionary.Keys)
            //{
            //    Console.WriteLine($"The id of technician: {key}");
            //    foreach (List<DateTime> listTime in technicianDictionary[key])
            //    {
            //        Console.WriteLine(listTime[0].ToString() + " " + listTime[1].ToString());
            //    }
            //}

            return technicianDictionary;
        }
    }
}
