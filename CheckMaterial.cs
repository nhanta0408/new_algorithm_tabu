using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabuSearchImplement.AggregateModels.JobInforAggregate;
using TabuSearchImplement.AggregateModels.WareHouseMaterialAggregate;
using TabuSearchImplement.AggregateModels.WorkAggregate;
using static TabuSearchImplement.Constant;

namespace TabuSearchImplement
{
    public class CheckMaterial
    {
        public static List<WareHouseMaterial> getWareHouseMaterial(List<WareHouseMaterialObjectInput> listWareHouseMaterialObjects)
        {
            var wareHouseMaterials = new List<WareHouseMaterial>();
            int i = 1;
            var listMaterialCode = new List<string>();
            foreach(var wareHouseMaterial in listWareHouseMaterialObjects)
            {
                bool check = false;
                foreach(var item in listMaterialCode)
                {
                    if (wareHouseMaterial.materialInfo.code == item)
                    {
                        check = true; 
                        break;
                    }
                }

                if (check == false)
                {
                    int id = i;
                    listMaterialCode.Add(wareHouseMaterial.materialInfo.code);
                    string code = wareHouseMaterial.materialInfo.code;
                    int minimumQuantity = wareHouseMaterial.materialInfo.minimumQuantity;
                    string isAdd = "False";
                    DateTime expectedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (wareHouseMaterial.materialInfo.expectedDate != "")
                    {
                        isAdd = "True";
                        expectedDate = DateTime.ParseExact(wareHouseMaterial.materialInfo.expectedDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    }

                    int quantity = 0;
                    foreach (var temp in listWareHouseMaterialObjects)
                    {
                        if (code == temp.materialInfo.code)
                        {
                            quantity += 1;
                        }
                    }

                    var wareHouseMaterialTemp = new WareHouseMaterial(id, code, quantity, minimumQuantity, isAdd, expectedDate);
                    wareHouseMaterials.Add(wareHouseMaterialTemp);
                    i++;
                }           
            }

            //foreach (var material in wareHouseMaterials)
            //{
            //    Console.WriteLine($"Material Id: {material.Id} - Material Code: {material.Code} - Material Quantity: {material.Quantity} - Material Minimum Quantity: {material.MinimumQuantity} - Is Request Add: {material.IsRequestAdd} - ExpectedDate: {material.ExpectedAddDate}");
            //}

            return wareHouseMaterials;
        }

        public static List<Material> getListMaterial(List<WorkObjectInput> listWorkObjects, List<WareHouseMaterial> listWareHouseMaterials) 
        {
            var listMaterials = new List<Material>();
            var material = new Material();
            List<string>? listNamePart = new List<string>();
            List<int>? listSequencePart = new List<int>();
            List<int>? listQuantityPart = new List<int>();

            foreach(WorkObjectInput workObject in listWorkObjects)
            {
                //Console.WriteLine($"The work {workObject.id} has material length {workObject.materials.Length}");
                if (workObject.id != "")
                {
                    material = new Material();
                    material.Id = int.Parse(workObject.id);
                    material.Priority = int.Parse(workObject.priority);
                    material.Device = workObject.deviceCode;
                    material.Work = workObject.problem;
                    material.DueDate = workObject.dueDate;
                    material.ExcutionTime = int.Parse(workObject.estProcessTime);

                    listNamePart = new List<string>();
                    listSequencePart = new List<int>();
                    listQuantityPart = new List<int>();

                    if (workObject.materials.Length > 0)
                    {
                        foreach (MaterialOnWork materialOnWork in workObject.materials)
                        {
                            listNamePart.Add(materialOnWork.materialInfo.code);
                            //Console.WriteLine($"The work {workObject.id} has material code {materialOnWork.materialInfo.code}");
                            foreach (WareHouseMaterial wareHouseMaterial in listWareHouseMaterials)
                            {
                                if (wareHouseMaterial.Code == materialOnWork.materialInfo.code)
                                {
                                    listSequencePart.Add(wareHouseMaterial.Id);
                                    //Console.WriteLine($"The work {workObject.id} has material sequence {wareHouseMaterial.Id}");
                                    break;
                                }
                            }

                            listQuantityPart.Add(int.Parse(materialOnWork.quantity));
                            //Console.WriteLine($"The work {workObject.id} has material quantity {materialOnWork.quantity}");
                        }
                    }
                    else
                    {
                        listNamePart = null;
                        listSequencePart = null;
                        listQuantityPart = null;
                    }
                }
                //else
                //{
                //    foreach (MaterialOnWork materialOnWork in workObject.materials)
                //    {
                //        listNamePart.Add(materialOnWork.materialInfo.code);
                //        foreach (WareHouseMaterial wareHouseMaterial in listWareHouseMaterials)
                //        {
                //            if (wareHouseMaterial.Code == materialOnWork.materialInfo.code)
                //            {
                //                listSequencePart.Add(wareHouseMaterial.Id);
                //                break;
                //            }
                //        }

                //        listQuantityPart.Add(int.Parse(materialOnWork.quantity));
                //    }
                //}

                material.ListNamePart = listNamePart;
                material.ListSequencePart = listSequencePart;
                material.ListQuantityPart = listQuantityPart;
                listMaterials.Add(material);
            }

            

            //Console.WriteLine();
            //foreach (var sparepart in listMaterials)
            //{
            //    Console.WriteLine("---------------------------");
            //    Console.WriteLine($"{sparepart.Id} {sparepart.Priority} {sparepart.Device} {sparepart.Work} {sparepart.DueDate} {sparepart.ExcutionTime}");
            //    if (sparepart.ListNamePart != null)
            //    {
            //        for (int j = 0; j < sparepart.ListNamePart.Count; j++)
            //        {
            //            Console.WriteLine(sparepart.ListNamePart[j].ToString() + " - " + sparepart.ListSequencePart[j].ToString() + " - " + sparepart.ListQuantityPart[j].ToString());
            //        }
            //    }
            //}

            return listMaterials;
        }

        public static List<int> checkMaterialAvailable(List<WareHouseMaterial> listWareHouseMaterials, List<Material> listMaterials, Material material)
        {
            var listSequencePartLack = new List<int>();
            //Console.WriteLine($"The job {sparePartOnWork.Id} need {sparePartOnWork.listSequencePart.Count} parts");
            for (int i = 0; i < material.ListSequencePart.Count; i++)
            {
                if (material.ListSequencePart[i] != 0)
                {
                    //Console.WriteLine($"The part {sparePartOnWork.listSequencePart[i]} is considered");
                    var quantityPartInventory = listWareHouseMaterials[material.ListSequencePart[i] - 1].Quantity;
                    var quantityPartOnWork = material.ListQuantityPart[i];
                    if (quantityPartInventory < quantityPartOnWork)
                    {
                        //Console.WriteLine($"The job {sparePartOnWork.Id} lack the sequence spare part {sparePartOnWork.listSequencePart[i]}");
                        listSequencePartLack.Add(material.ListSequencePart[i]);
                    }

                    //var newQuantityPart = quantityPartInventory - quantityPartOnWork;
                    //if (newQuantityPart >= 0)
                    //{
                    //    Console.WriteLine($"The job {sparePartOnWork.Id} - the sequence part {sparePartOnWork.listSequencePart[i]} consumed: {quantityPartOnWork}. And the new quantity part inventory: {newQuantityPart}");
                    //}
                }
            }

            return listSequencePartLack;
        }

        public static Dictionary<Material, List<int>> findJobRemove(List<WareHouseMaterial> listsparePart, List<Material> listsparePartOnWork)
        {
            
            Dictionary<Material, List<int>> dictJobRemove = new Dictionary<Material, List<int>>();

            //Console.WriteLine();
            foreach (Material sparePartOnWork in listsparePartOnWork)
            {
                if (sparePartOnWork.ListSequencePart != null)
                {
                    var listSequencePartLack = checkMaterialAvailable(listsparePart, listsparePartOnWork, sparePartOnWork);

                    //Console.WriteLine();
                    if (listSequencePartLack.Count == 0)
                    {
                        for (int i = 0; i < sparePartOnWork.ListSequencePart.Count; i++)
                        {
                            var quantityPartInventory = listsparePart[sparePartOnWork.ListSequencePart[i] - 1].Quantity;
                            var quantityPartOnWork = sparePartOnWork.ListQuantityPart[i];
                            var newQuantityPart = quantityPartInventory - quantityPartOnWork;
                            if (newQuantityPart >= 0)
                            {
                                listsparePart[sparePartOnWork.ListSequencePart[i] - 1].Quantity = newQuantityPart;
                                //Console.WriteLine($"The job {sparePartOnWork.Id} - the sequence part {sparePartOnWork.ListSequencePart[i]} consumed: {quantityPartOnWork}. And the new quantity part inventory: {newQuantityPart}");
                            }
                        }
                    }
                    else
                    {
                        dictJobRemove.Add(sparePartOnWork, listSequencePartLack);
                        //Console.WriteLine($"The job {sparePartOnWork.Id} is removed");
                        //foreach (int item in listSequencePartLack)
                        //{
                        //    Console.WriteLine($"Because this job {sparePartOnWork.Id} is lacked: the sequence part {item}");
                        //}
                    }
                }
                else
                {
                    //Console.WriteLine($"The job {sparePartOnWork.Id} doesn't need to use the spare part");
                    //Console.WriteLine("*************************************************************");
                    continue;
                }
                //Console.WriteLine("*************************************************************");
            }
            return dictJobRemove;
        }

        public static List<Material> getListWorkAvailable(List<WareHouseMaterial> listWareHouseMaterials, List<Material> listMaterials)
        {
            Dictionary<Material, List<int>> dictJobRemove = findJobRemove(listWareHouseMaterials, listMaterials);
            //var mondayOfThisWeek = DateTime.ParseExact("23/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //var saturdayOfThisWeek = DateTime.ParseExact("28/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);

            foreach (Material sparePartOnWork in dictJobRemove.Keys)
            {
                foreach (int sequence in dictJobRemove[sparePartOnWork])
                {
                    DateTime? expectedDate = listWareHouseMaterials[sequence - 1].ExpectedAddDate;
                    if (FindPlannedDate.isInRange(mondayOfThisWeek, saturdayOfThisWeek, expectedDate) == false)
                    {
                        listMaterials.Remove(sparePartOnWork);
                    }
                }
            }

            //Console.WriteLine();
            //foreach (var sparepart in listsparePartOnWork)
            //{
            //    Console.WriteLine($"{sparepart.Id} {sparepart.Priority} {sparepart.Device} {sparepart.Work} {sparepart.DueDate} {sparepart.ExcutionTime}");
            //}

            return listMaterials;
        }

        public static List<Material> getListWorkLackPartId(List<WareHouseMaterial> listWareHouseMaterials, List<Material> listMaterials)
        {
            Dictionary<Material, List<int>> dictJobRemove = findJobRemove(listWareHouseMaterials, listMaterials);
            //var mondayOfThisWeek = DateTime.ParseExact("23/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //var saturdayOfThisWeek = DateTime.ParseExact("28/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);

            List<Material> listWorkLackPartAvailable = new List<Material>();

            foreach (Material sparePartOnWork in dictJobRemove.Keys)
            {
                foreach (int sequence in dictJobRemove[sparePartOnWork])
                {
                    DateTime? expectedDate = listWareHouseMaterials[sequence - 1].ExpectedAddDate;
                    if (FindPlannedDate.isInRange(mondayOfThisWeek, saturdayOfThisWeek, expectedDate) == true)
                    {
                        listWorkLackPartAvailable.Add(sparePartOnWork);
                    }
                }
            }

            //Console.WriteLine();
            //foreach (var sparepart in listWorkLackPartAvailable)
            //{
            //    Console.WriteLine($"{sparepart.Id} {sparepart.Priority} {sparepart.Device} {sparepart.Work} {sparepart.DueDate} {sparepart.ExcutionTime}");
            //}

            return listWorkLackPartAvailable;
        }


        public static List<Material> getListWorkAvailableChangedId(List<WareHouseMaterial> listWareHouseMaterials, List<Material> listMaterials)
        {
            List<Material> listMaterialsLocal = getListWorkAvailable(listWareHouseMaterials, listMaterials);

            int j = 1;
            foreach (Material material in listMaterialsLocal)
            {
                material.Id = j++;
            }

            //foreach (var sparepart in listsparePartOnWork)
            //{
            //    Console.WriteLine($"{sparepart.Id} {sparepart.Priority} {sparepart.Device} {sparepart.Work} {sparepart.DueDate} {sparepart.ExcutionTime}");
            //}

            return listMaterialsLocal;
        }

        public static List<int> returnListSequencePartLackOnWork(List<WareHouseMaterial> listWareHouseMaterials, List<Material> listMaterials, Material workNeedCheck)
        {
            var listSequenceMaterialLack = new List<int>();
            //Console.WriteLine("1");
            foreach (Material material in listMaterials)
            {
                if (material.ListSequencePart[0] != 0)
                {
                    listSequenceMaterialLack = checkMaterialAvailable(listWareHouseMaterials, listMaterials, material);
                    //Console.WriteLine("2");
                    if (listSequenceMaterialLack.Count == 0)
                    {
                        //Console.WriteLine("3");
                        for (int i = 0; i < material.ListSequencePart.Count; i++)
                        {
                            var quantityPartInventory = listWareHouseMaterials[material.ListSequencePart[i] - 1].Quantity;
                            var quantityPartOnWork = material.ListQuantityPart[i];
                            var newQuantityPart = quantityPartInventory - quantityPartOnWork;
                            if (newQuantityPart >= 0)
                            {
                                listWareHouseMaterials[material.ListSequencePart[i] - 1].Quantity = newQuantityPart;
                            }
                        }
                    }
                    else
                    {
                        //Console.WriteLine("4");
                        if (material.Id == workNeedCheck.Id)
                        {
                            //Console.WriteLine($"The job {workNeedCheck.Id} is removed");
                            //foreach (int item in listSequenceMaterialLack)
                            //{
                            //    Console.WriteLine($"Because this job {material.Id} is lacked: the sequence part {item}");
                            //}

                            return listSequenceMaterialLack; 
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
            return listSequenceMaterialLack;
        }

        public static List<Device> returnDevicesNotEnoughTime(DataTable workTable, List<Device> devices)
        {
            Dictionary<Device, double> totalBreakingTimeOnDevice = new Dictionary<Device, double>();
            Dictionary<Device, double> totalMaintenanceTimeOnDevice = new Dictionary<Device, double>();
            foreach (Device device in devices)
            {
                double totalBreakingTime = 0;
                foreach(WorkingTime dateTimes in device.workingTimes)
                {
                    totalBreakingTime += TimeSpan.FromTicks((dateTimes.To - dateTimes.From).Ticks).TotalMinutes;
                }
                totalBreakingTimeOnDevice.Add(device, totalBreakingTime);
                totalMaintenanceTimeOnDevice.Add(device, 0);

                foreach (DataRow row in workTable.Rows)
                {
                    if (row["Device"] == device.code)
                    {
                        totalMaintenanceTimeOnDevice[device] += double.Parse(row["ExecutionTime"].ToString());
                    }
                }
            }

            List<Device> deviceNotEnoughTimes = new List<Device>();
            foreach (Device device in devices)
            {
                if (totalMaintenanceTimeOnDevice[device] > totalBreakingTimeOnDevice[device])
                {
                    deviceNotEnoughTimes.Add(device);
                }
            }

            //foreach (string device in deviceNotEnoughTimes)
            //{
            //    Console.WriteLine($"The device {device} is not enough time to implement works");
            //}
            return deviceNotEnoughTimes;
        }

        public static ListJobInforReturn listJobReturnForOneWork(DataTable workAvailableTable,
                                                                 Dictionary<string, List<List<DateTime>>> deviceDictionary,
                                                                 Dictionary<string, List<List<DateTime>>> technicianDictionary,
                                                                 DataTable wareHouseMaterialTable, DataTable materialTable,
                                                                 List<WareHouseMaterial> listwareHouseMaterials,
                                                                 List<Material> listMaterials)
        {
            JobInfor jobInfor = new JobInfor();
            jobInfor.Id = 1;
            jobInfor.Priority = int.Parse(workAvailableTable.Rows[0]["Priority"].ToString());
            jobInfor.Device = workAvailableTable.Rows[0]["Device"].ToString();
            jobInfor.Work = workAvailableTable.Rows[0]["Work"].ToString();
            jobInfor.Technician = "";
            jobInfor.DueDate = DateTime.ParseExact(workAvailableTable.Rows[0]["DueDate"].ToString(), "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
            jobInfor.StartPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            jobInfor.EndPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            jobInfor.EstProcessTime = int.Parse(workAvailableTable.Rows[0]["ExecutionTime"].ToString());

            Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime = FindPlannedDate.deviceStructure(deviceDictionary);
            Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime = FindPlannedDate.technicianStructure(technicianDictionary);
            List<Material> listWorkAvailableChangedId = CheckMaterial.getListWorkAvailableChangedId(listwareHouseMaterials, listMaterials);

            Material temp = new Material();
            JobInfor newJob = FindPlannedDate.findPlannedDate(jobInfor, deviceDictionary, technicianDictionary, maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime, listwareHouseMaterials, listWorkAvailableChangedId, temp);
            var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);

            ListJobInforReturn listJobInforReturn = new ListJobInforReturn(scheduled: new List<JobInfor>(),
                                                               rejected: new List<JobInfor>());

            if (newJob.StartPlannedDate > saturdayOfThisWeek || newJob.StartPlannedDate < mondayOfThisWeek)
            {
                listJobInforReturn.Rejected.Add(newJob);
            }
            else
            {
                listJobInforReturn.Scheduled.Add(newJob);
            }

            return listJobInforReturn;
        }


        public static ListJobInforReturn listJobReturn(List<WorkObjectInput> listWorkObjects, List<JobInfor> listJobAvailable, 
                                                   List<string> listDeviceNotEnoughTimes, 
                                                   Dictionary<string, List<List<DateTime>>> deviceDictionary,
                                                   Dictionary<string, List<List<DateTime>>> technicianDictionary,
                                                   DataTable wareHouseMaterialTable, DataTable materialTable,
                                                   List<WareHouseMaterial> listwareHouseMaterials,
                                                   List<Material> listMaterials,
                                                   DataTable workRemoveTable)
        {
            var workTable = ConvertFromObjectToTable.ConvertObjectInputToWorksTable(listWorkObjects);
            //Console.WriteLine($"The length of listDeviceNotEnoughTimes: {listDeviceNotEnoughTimes.Count}");
            Dictionary<string, List<JobInfor>> dictJobUnavailble =  new Dictionary<string, List<JobInfor>>();
            Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime = FindPlannedDate.deviceStructure(deviceDictionary);
            Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime = FindPlannedDate.technicianStructure(technicianDictionary);
            List<Material> listWorkAvailableChangedId = CheckMaterial.getListWorkAvailableChangedId(listwareHouseMaterials, listMaterials);

            foreach (string device in listDeviceNotEnoughTimes)
            {
                dictJobUnavailble.Add(device, new List<JobInfor>());
            }

            var listRowRemoveNotEnoughTimes = new List<DataRow>();
            foreach (DataRow row in workTable.Rows)
            {
                int noOfRow = Convert.ToInt32(row["No"]);
                foreach (string device in listDeviceNotEnoughTimes)
                {
                    //Console.WriteLine($"Work {noOfRow} on device {row["Device"].ToString()} compared with {device}");
                    if (row["Device"].ToString() == device)
                    {
                        //Console.WriteLine($"Work {noOfRow} on device {device} is removed");
                        listRowRemoveNotEnoughTimes.Add(row);
                        break;
                    }
                }
            }

            //Console.WriteLine($"The length of listRowRemove: {listRowRemove.Count}");
            var fridayOfNextWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Friday).AddDays(7);

            int numberOfJobAvailable = listJobAvailable.Count + 1;

            foreach (DataRow workRow in listRowRemoveNotEnoughTimes)
            {
                JobInfor jobInfor = new JobInfor();
                jobInfor.Id = numberOfJobAvailable++;
                jobInfor.Priority = int.Parse(workRow["Priority"].ToString());
                jobInfor.Device = workRow["Device"].ToString();
                jobInfor.Work = workRow["Work"].ToString();
                jobInfor.Technician = "";
                jobInfor.DueDate = DateTime.ParseExact(workRow["DueDate"].ToString(), "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
                jobInfor.StartPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                jobInfor.EndPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                jobInfor.EstProcessTime = int.Parse(workRow["ExecutionTime"].ToString());
                dictJobUnavailble[workRow["Device"].ToString()].Add(jobInfor);
            }

            var listJobUnavailble = new List<JobInfor>();
            foreach (string device in dictJobUnavailble.Keys)
            {
                List<JobInfor> listJobSortPriority = dictJobUnavailble[device].OrderByDescending(x => x.Priority).ToList();
                foreach (JobInfor jobDescendingPriority in listJobSortPriority)
                {
                    //Console.WriteLine($"The priority of job unavailable is: {jobDescendingPriority.Priority}");
                    Material temp = new Material();
                    JobInfor newJob = FindPlannedDate.findPlannedDate(jobDescendingPriority, deviceDictionary, technicianDictionary, maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime, listwareHouseMaterials, listWorkAvailableChangedId, temp);
                    listJobUnavailble.Add(newJob);
                }
            }

            Console.WriteLine($"The length of workAvailableTable.Rows: {workRemoveTable.Rows.Count}");

            foreach (DataRow workRow in workRemoveTable.Rows)
            {
                JobInfor jobInfor = new JobInfor();
                jobInfor.Id = numberOfJobAvailable++;
                jobInfor.Priority = int.Parse(workRow["Priority"].ToString());
                jobInfor.Device = workRow["Device"].ToString();
                jobInfor.Work = workRow["Work"].ToString();
                jobInfor.Technician = "";
                jobInfor.DueDate = DateTime.ParseExact(workRow["DueDate"].ToString(), "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
                jobInfor.StartPlannedDate = fridayOfNextWeek;
                TimeSpan executionTime = TimeSpan.FromMinutes(jobInfor.EstProcessTime);
                jobInfor.EndPlannedDate = jobInfor.StartPlannedDate.Add(executionTime);
                jobInfor.EstProcessTime = int.Parse(workRow["ExecutionTime"].ToString());
                listJobUnavailble.Add(jobInfor);
            }


            foreach (JobInfor jobTemp in listJobUnavailble)
            {
                listJobAvailable.Add(jobTemp);
            }

            ListJobInforReturn listJobInforReturn = new ListJobInforReturn(scheduled: new List<JobInfor>(),
                                                                           rejected: new List<JobInfor>());

            //var mondayOfThisWeek = DateTime.ParseExact("23/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //var saturdayOfThisWeek = DateTime.ParseExact("28/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
            foreach (JobInfor work in listJobAvailable)
            {
                if (work.StartPlannedDate > saturdayOfThisWeek || work.StartPlannedDate < mondayOfThisWeek)
                {
                    listJobInforReturn.Rejected.Add(work);
                }
                else
                {
                    listJobInforReturn.Scheduled.Add(work);
                }
            }

            Console.WriteLine($"List Job Infor Return Scheduled: ");
            foreach (JobInfor work in listJobInforReturn.Scheduled)
            {
                Console.WriteLine($"The work id: {work.Id} on device: {work.Device} and technician: {work.Technician} has due date: {work.DueDate} and planned date: {work.StartPlannedDate} - {work.EndPlannedDate}");
            }

            Console.WriteLine($"List Job Infor Return Rejected: ");
            foreach (JobInfor work in listJobInforReturn.Rejected)
            {
                Console.WriteLine($"The work id: {work.Id} on device: {work.Device} and technician: {work.Technician} has due date: {work.DueDate} and planned date: {work.StartPlannedDate} - {work.EndPlannedDate}");
            }

            return listJobInforReturn;
        }
    }
}
