using NewPMSTabu.Model;
using System.Data;
using System.Globalization;

namespace NewPMSTabu.Functions
{
    public class CheckMaterial
    {
        public static List<MaterialInfo> checkMaterialAvailable(List<WarehouseMaterial> listWareHouseMaterials, Work work)
        {
            var listMaterialInforLack = new List<MaterialInfo>();

            if (work.materials != null)
            {
                foreach (Material material in work.materials)
                {
                    var quantityInventory = listWareHouseMaterials.FirstOrDefault(x => x.MaterialInfo.code == material.materialInfo.code).CurrentQuantity;
                    var quantityOnWork = material.requiredQuantity;
                    if(quantityInventory < quantityOnWork)
                    {
                        listMaterialInforLack.Add(material.materialInfo);
                    }

                }
            }

            return listMaterialInforLack;
        }

        public static List<Work> findJobRemove(List<WarehouseMaterial> listWareHouseMaterials, List<Work> listWorks)
        {
            List<Work> listWorkNotAvailable = new List<Work>();
            
            foreach(Work work in listWorks)
            {
                if (work.materials != null)
                {
                    var listMaterialInforLack = checkMaterialAvailable(listWareHouseMaterials, work);
                    if (listMaterialInforLack.Count == 0)
                    {
                        foreach(Material material in work.materials)
                        {
                            var quantityInventory = listWareHouseMaterials.FirstOrDefault(x => x.MaterialInfo.code == material.materialInfo.code).CurrentQuantity;
                            decimal quantityOnWork = (decimal)material.requiredQuantity;
                            decimal newQuantityInventory = quantityInventory - quantityOnWork;

                            if (newQuantityInventory >= 0)
                            {
                                listWareHouseMaterials.FirstOrDefault(x => x.MaterialInfo.code == material.materialInfo.code).CurrentQuantity -= newQuantityInventory;
                            }
                        }
                    }
                    else
                    {
                        listWorkNotAvailable.Add(work);
                    }
                }
                else
                {
                    continue;
                }
            }

            return listWorkNotAvailable;
        }

        public static List<Work> checkWorkEnoughMaterial(List<WarehouseMaterial> listWareHouseMaterials, List<Work> works)
        {
            List<Work> listWorkNotAvailable = findJobRemove(listWareHouseMaterials, works);

            var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);

            foreach(Work workNotAvailable in listWorkNotAvailable)
            {
                foreach(Material material in workNotAvailable.materials)
                {
                    DateTime expectedDate = listWareHouseMaterials.FirstOrDefault(x => x.MaterialInfo.code == material.materialInfo.code).ExpectedDate;
                    if (FindPlannedDate.isInRange(mondayOfThisWeek, saturdayOfThisWeek, expectedDate) == false)
                    {
                        works.Remove(workNotAvailable);
                    }
                }
            }

            return works;
        }

        public static List<Work> findWorkNeedMaterial(List<WarehouseMaterial> listWareHouseMaterials, List<Work> listWorkAvailable)
        {
            List<Work> listWorkNotAvailable = findJobRemove(listWareHouseMaterials, listWorkAvailable);

            var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);

            List<Work> listWorkNeedMaterial = new List<Work>();

            foreach (Work work in listWorkNotAvailable)
            {

                foreach(Material material in work.materials)
                {
                    DateTime expectedDate = listWareHouseMaterials.FirstOrDefault(x => x.MaterialInfo.code == material.materialInfo.code).ExpectedDate;
                    if (FindPlannedDate.isInRange(mondayOfThisWeek, saturdayOfThisWeek, expectedDate) == true)
                    {
                        listWorkNeedMaterial.Add(work);
                    }
                }
            }

            return listWorkNeedMaterial;
        }


        public static List<Work> returnDevicesNotEnoughTime(List<Work> listWorks, List<Device> devices)
        {
            var listWorkOnDeviceNotEnoughtTime = new List<Work>();

            Dictionary<Device, double> totalBreakingTimeOnDevice = new Dictionary<Device, double>();
            Dictionary<Device, double> totalMaintenanceTimeOnDevice = new Dictionary<Device, double>();
            foreach (Device device in devices)
            {
                double totalBreakingTime = 0;
                foreach (WorkingTime dateTimes in device.workingTimes)
                {
                    totalBreakingTime += TimeSpan.FromTicks((dateTimes.To - dateTimes.From).Ticks).TotalMinutes;
                }
                totalBreakingTimeOnDevice.Add(device, totalBreakingTime);
                totalMaintenanceTimeOnDevice.Add(device, 0);
                
                foreach(Work work in listWorks)
                {
                    if (work.device.code == device.code)
                    {
                        totalMaintenanceTimeOnDevice[device] += work.executionTime;
                    }
                }
            }

            List<Device> listDeviceNotEnoughTime = new List<Device>();
            foreach (Device device in devices)
            {
                if (totalMaintenanceTimeOnDevice[device] > totalBreakingTimeOnDevice[device])
                {
                    listDeviceNotEnoughTime.Add(device);
                }
            }

            foreach(Device device in listDeviceNotEnoughTime)
            {
                var listWorkCheck = listWorks.Where(x => x.device.code == device.code).ToList();
                foreach (var workTemp in listWorkCheck)
                {
                    listWorkOnDeviceNotEnoughtTime.Add(workTemp);
                    listWorks.Remove(workTemp);
                }
            }

            return listWorks;
        }

        //public static ListJobInforReturn listJobReturnForOneWork(DataTable workAvailableTable,
        //                                                         Dictionary<string, List<List<DateTime>>> deviceDictionary,
        //                                                         Dictionary<string, List<List<DateTime>>> technicianDictionary,
        //                                                         DataTable wareHouseMaterialTable, DataTable materialTable,
        //                                                         List<WareHouseMaterial> listwareHouseMaterials,
        //                                                         List<Material> listMaterials)
        //{
        //    JobInfor jobInfor = new JobInfor();
        //    jobInfor.Id = 1;
        //    jobInfor.Priority = int.Parse(workAvailableTable.Rows[0]["Priority"].ToString());
        //    jobInfor.Device = workAvailableTable.Rows[0]["Device"].ToString();
        //    jobInfor.Work = workAvailableTable.Rows[0]["Work"].ToString();
        //    jobInfor.Technician = "";
        //    jobInfor.DueDate = DateTime.ParseExact(workAvailableTable.Rows[0]["DueDate"].ToString(), "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
        //    jobInfor.StartPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    jobInfor.EndPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    jobInfor.EstProcessTime = int.Parse(workAvailableTable.Rows[0]["ExecutionTime"].ToString());

        //    Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime = FindPlannedDate.deviceStructure(deviceDictionary);
        //    Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime = FindPlannedDate.technicianStructure(technicianDictionary);
        //    List<Material> listWorkAvailableChangedId = CheckMaterial.getListWorkAvailableChangedId(listwareHouseMaterials, listMaterials);

        //    Material temp = new Material();
        //    JobInfor newJob = FindPlannedDate.findPlannedDate(jobInfor, deviceDictionary, technicianDictionary, maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime, listwareHouseMaterials, listWorkAvailableChangedId, temp);
        //    var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
        //    var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);

        //    ListJobInforReturn listJobInforReturn = new ListJobInforReturn(scheduled: new List<JobInfor>(),
        //                                                       rejected: new List<JobInfor>());

        //    if (newJob.StartPlannedDate > saturdayOfThisWeek || newJob.StartPlannedDate < mondayOfThisWeek)
        //    {
        //        listJobInforReturn.Rejected.Add(newJob);
        //    }
        //    else
        //    {
        //        listJobInforReturn.Scheduled.Add(newJob);
        //    }

        //    return listJobInforReturn;
        //}


        //public static ListJobInforReturn listJobReturn(List<WorkObjectInput> listWorkObjects, List<JobInfor> listJobAvailable,
        //                                               List<string> listDeviceNotEnoughTimes,
        //                                               Dictionary<string, List<List<DateTime>>> deviceDictionary,
        //                                           Dictionary<string, List<List<DateTime>>> technicianDictionary,
        //                                           DataTable wareHouseMaterialTable, DataTable materialTable,
        //                                           List<WareHouseMaterial> listwareHouseMaterials,
        //                                           List<Material> listMaterials,
        //                                           DataTable workRemoveTable)
        //{
        //    var workTable = ConvertFromObjectToTable.ConvertObjectInputToWorksTable(listWorkObjects);
        //    //Console.WriteLine($"The length of listDeviceNotEnoughTimes: {listDeviceNotEnoughTimes.Count}");
        //    Dictionary<string, List<JobInfor>> dictJobUnavailble = new Dictionary<string, List<JobInfor>>();
        //    Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime = FindPlannedDate.deviceStructure(deviceDictionary);
        //    Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime = FindPlannedDate.technicianStructure(technicianDictionary);
        //    List<Material> listWorkAvailableChangedId = CheckMaterial.getListWorkAvailableChangedId(listwareHouseMaterials, listMaterials);

        //    foreach (string device in listDeviceNotEnoughTimes)
        //    {
        //        dictJobUnavailble.Add(device, new List<JobInfor>());
        //    }

        //    var listRowRemoveNotEnoughTimes = new List<DataRow>();
        //    foreach (DataRow row in workTable.Rows)
        //    {
        //        int noOfRow = Convert.ToInt32(row["No"]);
        //        foreach (string device in listDeviceNotEnoughTimes)
        //        {
        //            //Console.WriteLine($"Work {noOfRow} on device {row["Device"].ToString()} compared with {device}");
        //            if (row["Device"].ToString() == device)
        //            {
        //                //Console.WriteLine($"Work {noOfRow} on device {device} is removed");
        //                listRowRemoveNotEnoughTimes.Add(row);
        //                break;
        //            }
        //        }
        //    }

        //    //Console.WriteLine($"The length of listRowRemove: {listRowRemove.Count}");
        //    var fridayOfNextWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Friday).AddDays(7);

        //    int numberOfJobAvailable = listJobAvailable.Count + 1;

        //    foreach (DataRow workRow in listRowRemoveNotEnoughTimes)
        //    {
        //        JobInfor jobInfor = new JobInfor();
        //        jobInfor.Id = numberOfJobAvailable++;
        //        jobInfor.Priority = int.Parse(workRow["Priority"].ToString());
        //        jobInfor.Device = workRow["Device"].ToString();
        //        jobInfor.Work = workRow["Work"].ToString();
        //        jobInfor.Technician = "";
        //        jobInfor.DueDate = DateTime.ParseExact(workRow["DueDate"].ToString(), "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
        //        jobInfor.StartPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        jobInfor.EndPlannedDate = DateTime.ParseExact("01/01/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //        jobInfor.EstProcessTime = int.Parse(workRow["ExecutionTime"].ToString());
        //        dictJobUnavailble[workRow["Device"].ToString()].Add(jobInfor);
        //    }

        //    var listJobUnavailble = new List<JobInfor>();
        //    foreach (string device in dictJobUnavailble.Keys)
        //    {
        //        List<JobInfor> listJobSortPriority = dictJobUnavailble[device].OrderByDescending(x => x.Priority).ToList();
        //        foreach (JobInfor jobDescendingPriority in listJobSortPriority)
        //        {
        //            //Console.WriteLine($"The priority of job unavailable is: {jobDescendingPriority.Priority}");
        //            Material temp = new Material();
        //            JobInfor newJob = FindPlannedDate.findPlannedDate(jobDescendingPriority, deviceDictionary, technicianDictionary, maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime, listwareHouseMaterials, listWorkAvailableChangedId, temp);
        //            listJobUnavailble.Add(newJob);
        //        }
        //    }

        //    Console.WriteLine($"The length of workAvailableTable.Rows: {workRemoveTable.Rows.Count}");

        //    foreach (DataRow workRow in workRemoveTable.Rows)
        //    {
        //        JobInfor jobInfor = new JobInfor();
        //        jobInfor.Id = numberOfJobAvailable++;
        //        jobInfor.Priority = int.Parse(workRow["Priority"].ToString());
        //        jobInfor.Device = workRow["Device"].ToString();
        //        jobInfor.Work = workRow["Work"].ToString();
        //        jobInfor.Technician = "";
        //        jobInfor.DueDate = DateTime.ParseExact(workRow["DueDate"].ToString(), "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);
        //        jobInfor.StartPlannedDate = fridayOfNextWeek;
        //        TimeSpan executionTime = TimeSpan.FromMinutes(jobInfor.EstProcessTime);
        //        jobInfor.EndPlannedDate = jobInfor.StartPlannedDate.Add(executionTime);
        //        jobInfor.EstProcessTime = int.Parse(workRow["ExecutionTime"].ToString());
        //        listJobUnavailble.Add(jobInfor);
        //    }


        //    foreach (JobInfor jobTemp in listJobUnavailble)
        //    {
        //        listJobAvailable.Add(jobTemp);
        //    }

        //    ListJobInforReturn listJobInforReturn = new ListJobInforReturn(scheduled: new List<JobInfor>(),
        //                                                                   rejected: new List<JobInfor>());

        //    //var mondayOfThisWeek = DateTime.ParseExact("23/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
        //    //var saturdayOfThisWeek = DateTime.ParseExact("28/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

        //    var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
        //    var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
        //    foreach (JobInfor work in listJobAvailable)
        //    {
        //        if (work.StartPlannedDate > saturdayOfThisWeek || work.StartPlannedDate < mondayOfThisWeek)
        //        {
        //            listJobInforReturn.Rejected.Add(work);
        //        }
        //        else
        //        {
        //            listJobInforReturn.Scheduled.Add(work);
        //        }
        //    }

        //    Console.WriteLine($"List Job Infor Return Scheduled: ");
        //    foreach (JobInfor work in listJobInforReturn.Scheduled)
        //    {
        //        Console.WriteLine($"The work id: {work.Id} on device: {work.Device} and technician: {work.Technician} has due date: {work.DueDate} and planned date: {work.StartPlannedDate} - {work.EndPlannedDate}");
        //    }

        //    Console.WriteLine($"List Job Infor Return Rejected: ");
        //    foreach (JobInfor work in listJobInforReturn.Rejected)
        //    {
        //        Console.WriteLine($"The work id: {work.Id} on device: {work.Device} and technician: {work.Technician} has due date: {work.DueDate} and planned date: {work.StartPlannedDate} - {work.EndPlannedDate}");
        //    }

        //    return listJobInforReturn;
        //}
    }
}
