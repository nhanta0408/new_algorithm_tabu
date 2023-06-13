using NewPMSTabu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPMSTabu.Functions
{
    public class FindPlannedDate
    {

        public static bool isInRange(DateTime? startDate, DateTime? endDate, DateTime? checkDate)
        {
            return (startDate <= checkDate) && (checkDate <= endDate);
        }


        public static Dictionary<string, List<List<DateTime>>> deviceStructure(Dictionary<string, List<List<DateTime>>> deviceDictionary)
        {
            Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime = new Dictionary<string, List<List<DateTime>>>();
            foreach (string key in deviceDictionary.Keys)
            {
                List<List<DateTime>> listDateTimeEmpty = new List<List<DateTime>>();
                maintenanceDeviceBreakTime.Add(key, listDateTimeEmpty);
            }

            return maintenanceDeviceBreakTime;
        }


        public static Dictionary<string, List<List<DateTime>>> technicianStructure(Dictionary<string, List<List<DateTime>>> technicianDictionary)
        {
            Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime = new Dictionary<string, List<List<DateTime>>>();
            foreach (string no in technicianDictionary.Keys)
            {
                List<List<DateTime>> listDateTimeEmpty = new List<List<DateTime>>();
                maintenanceTechnicianWorkTime.Add(no, listDateTimeEmpty);
            }

            return maintenanceTechnicianWorkTime;
        }

        public static List<string> shuffleList(List<string> list)
        {
            var random = new Random();
            var newShuffledList = new List<string>();
            var listcCount = list.Count;
            for (int i = 0; i < listcCount; i++)
            {
                var randomElementInList = random.Next(0, list.Count);
                newShuffledList.Add(list[randomElementInList]);
                list.Remove(list[randomElementInList]);
            }
            return newShuffledList;
        }

        public static JobInfor checkTimeAvailable(JobInfor workInfor,
                                                  List<Device> listDevice,
                                                  List<Technician> listTechnician)
        {
            var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
            var fridayOfNextWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Friday).AddDays(7);

            var lengthOfDeviceBreakingTimes = workInfor.Device.workingTimes.Length;
            var lengthOfTechnicianWorkingTimes = workInfor.Technician.workingTimes.Length;
            if (workInfor.StartPlannedDate > saturdayOfThisWeek || workInfor.EndPlannedDate > saturdayOfThisWeek || workInfor.EndPlannedDate > workInfor.Device.workingTimes[lengthOfDeviceBreakingTimes - 1].To || workInfor.EndPlannedDate > workInfor.Technician.workingTimes[lengthOfTechnicianWorkingTimes - 1].To)
            {
                workInfor.StartPlannedDate = fridayOfNextWeek;
                TimeSpan executionTime = TimeSpan.FromMinutes(workInfor.EstProcessTime);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
                workInfor.ArrayFail.IsOk = true;
                return workInfor;
            }

            bool checkDeviceAvailable = false;
            foreach (WorkingTime breakTime in workInfor.Device.workingTimes)
            {
                if (isInRange(breakTime.From, breakTime.To, workInfor.StartPlannedDate) && isInRange(breakTime.From, breakTime.To, workInfor.EndPlannedDate))
                {
                    checkDeviceAvailable = true;
                    break;
                }
            }

            bool checkTechnicianAvailable = false;
            List<string> listTechnicianId = listTechnician.Select(x => x.id).ToList();  
            List<string> newListTechnicianId = shuffleList(listTechnicianId);
            if (workInfor.Technician == null)
            {
                foreach (string id in newListTechnicianId)
                {
                    foreach (WorkingTime workTime in listTechnician.FirstOrDefault(x => x.id == id).workingTimes)
                    {
                        if (isInRange(workTime.From, workTime.To, workInfor.StartPlannedDate) && isInRange(workTime.From, workTime.To, workInfor.EndPlannedDate))
                        {
                            checkTechnicianAvailable = true;
                            break;
                        }
                    }

                    if (checkTechnicianAvailable && workInfor.Technician == null)
                    {
                        workInfor.Technician = listTechnician.FirstOrDefault(x => x.id == id);
                        break;
                    }
                }
            }
            else
            {
                foreach (WorkingTime workTime in workInfor.Technician.workingTimes)
                {
                    if (isInRange(workTime.From, workTime.To, workInfor.StartPlannedDate) && isInRange(workTime.From, workTime.To, workInfor.EndPlannedDate))
                    {
                        checkTechnicianAvailable = true;
                        break;
                    }
                }

                if (checkTechnicianAvailable == false)
                {
                    foreach (string id in newListTechnicianId)
                    {
                        foreach (WorkingTime workTime in listTechnician.FirstOrDefault(x => x.id == id).workingTimes)
                        {
                            if (isInRange(workTime.From, workTime.To, workInfor.StartPlannedDate) && isInRange(workTime.From, workTime.To, workInfor.EndPlannedDate))
                            {
                                checkTechnicianAvailable = true;
                                break;
                            }
                        }

                        if (checkTechnicianAvailable)
                        {
                            workInfor.Technician = listTechnician.FirstOrDefault(x => x.id == id);
                            break;
                        }
                    }
                }
            }


            if (checkDeviceAvailable && checkTechnicianAvailable)
            {
                bool checkTechnicianDuplicate = false;
                foreach (WorkingTime technicianMaintenanceTime in workInfor.Technician.maintenanceTimes)
                {
                    if (isInRange(technicianMaintenanceTime.From, technicianMaintenanceTime.To, workInfor.StartPlannedDate) || isInRange(technicianMaintenanceTime.From, technicianMaintenanceTime.To, workInfor.EndPlannedDate))
                    {
                        checkTechnicianDuplicate = true;
                        break;
                    }

                    if (isInRange(workInfor.StartPlannedDate, workInfor.EndPlannedDate, technicianMaintenanceTime.From) && isInRange(workInfor.StartPlannedDate, workInfor.EndPlannedDate, technicianMaintenanceTime.To))
                    {
                        checkTechnicianDuplicate = true;
                        break;
                    }
                }

                if (checkTechnicianDuplicate == true)
                {
                    workInfor.ArrayFail.TechnicianDuplicatedJob = true;
                }

                bool checkDeviceDuplicate = false;
                foreach (WorkingTime deviceBreakTime in workInfor.Device.maintenanceTimes)
                {
                    if (isInRange(deviceBreakTime.From, deviceBreakTime.To, workInfor.StartPlannedDate) || isInRange(deviceBreakTime.From, deviceBreakTime.To, workInfor.EndPlannedDate))
                    {
                        checkDeviceDuplicate = true;
                        break;
                    }
                }

                if (checkDeviceDuplicate == true)
                {
                    workInfor.ArrayFail.DeviceDuplicatedJob = true;
                }

                if (checkDeviceDuplicate == false && checkTechnicianDuplicate == false)
                {
                    workInfor.ArrayFail.IsOk = true;
                }
            }
            else if (checkDeviceAvailable == false)
            {
                workInfor.ArrayFail.DeviceBreakingTimeNotAvailable = true;
            }
            else if (checkTechnicianAvailable == false)
            {
                workInfor.ArrayFail.TechnicianWorkingTimeNotAvailable = true;
            }

            return workInfor;
        }

        public static JobInfor changePlannedDate(JobInfor workInfor,
                                                 List<Device> listDevice,
                                                 List<Technician> listTechnician,
                                                 Work workLackPart)
        {
            if (workInfor.ArrayFail.DeviceBreakingTimeNotAvailable == true)
            {
                if (workLackPart == null)
                {
                    //Console.WriteLine($"The job {workInfor.Id} has the fail 1 on the {workInfor.Device} and technician {workInfor.Technician}");
                    for (int i = 0; i < (workInfor.Device.workingTimes.Length - 1); i++)
                    {
                        if (workInfor.Device.workingTimes[i].From <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= workInfor.Device.workingTimes[i + 1].From)
                        {
                            workInfor.StartPlannedDate = workInfor.Device.workingTimes[i + 1].From.Add(TimeSpan.FromMinutes(1));
                            break;
                        }
                    }
                }
                else if (workLackPart != null)
                {
                    for (int i = 0; i < (workInfor.Device.workingTimes.Length - 1); i++)
                    {
                        if (workInfor.Device.workingTimes[i].From <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= workInfor.Device.workingTimes[i + 1].From)
                        {
                            workInfor.StartPlannedDate = workInfor.Device.workingTimes[i + 1].From.Add(TimeSpan.FromMinutes(1));
                            break;
                        }
                    }
                }

                TimeSpan executionTime = TimeSpan.FromMinutes(workInfor.EstProcessTime);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
            }

            if (workInfor.ArrayFail.TechnicianWorkingTimeNotAvailable == true)
            {
                if (workLackPart == null)
                {
                    if (workInfor.StartPlannedDate < workInfor.Technician.workingTimes[0].From)
                    {
                        workInfor.StartPlannedDate = workInfor.Technician.workingTimes[0].From.AddMinutes(1);
                    }
                    else
                    {
                        for (int i = 0; i < (workInfor.Technician.workingTimes.Length - 1); i++)
                        {
                            if (workInfor.Technician.workingTimes[i].From <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= workInfor.Technician.workingTimes[i + 1].From)
                            {
                                workInfor.StartPlannedDate = workInfor.Technician.workingTimes[i + 1].From.Add(TimeSpan.FromMinutes(1));
                                break;
                            }
                        }
                    }
                }
                else if (workLackPart != null)
                {
                    if (workInfor.StartPlannedDate < workInfor.Technician.workingTimes[0].From)
                    {
                        workInfor.StartPlannedDate = workInfor.Technician.workingTimes[0].From.AddMinutes(1);
                    }
                    else
                    {
                        for (int i = 0; i < (workInfor.Technician.workingTimes.Length - 1); i++)
                        {
                            if (workInfor.Technician.workingTimes[i].From <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= workInfor.Technician.workingTimes[i + 1].From)
                            {
                                workInfor.StartPlannedDate = workInfor.Technician.workingTimes[i + 1].From.Add(TimeSpan.FromMinutes(1));
                                break;
                            }
                        }
                    }
                }

                TimeSpan executionTime = TimeSpan.FromMinutes(workInfor.EstProcessTime);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
            }

            if (workInfor.ArrayFail.DeviceDuplicatedJob == true)
            {
                int numberOfWorkOnDevice = workInfor.Device.maintenanceTimes.Count;
                workInfor.StartPlannedDate = workInfor.Device.maintenanceTimes[numberOfWorkOnDevice - 1].To.Add(TimeSpan.FromMinutes(1));
                for (int i = 0; i < (workInfor.Device.workingTimes.Length - 1); i++)
                {
                    if (workInfor.Device.workingTimes[i].From <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= workInfor.Device.workingTimes[i + 1].From)
                    {
                        workInfor.StartPlannedDate = workInfor.Device.workingTimes[i + 1].From.Add(TimeSpan.FromMinutes(1));
                        break;
                    }
                }

                TimeSpan executionTime = TimeSpan.FromMinutes(workInfor.EstProcessTime);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
                //Console.WriteLine($"The job {workInfor.Id} with fail 3 has new planned start is {workInfor.StartPlannedDate}");
            }

            if (workInfor.ArrayFail.TechnicianDuplicatedJob == true)
            {
                if (workInfor.Technician.maintenanceTimes.Count > 8)
                {
                    Random rnd = new Random();
                    var randomId = rnd.Next(1, listTechnician.Count);
                    var technicianId = listTechnician[randomId];
                    workInfor.Technician = technicianId;
                }

                workInfor.StartPlannedDate = workInfor.StartPlannedDate.AddMinutes(1);
                workInfor.EndPlannedDate = workInfor.EndPlannedDate.AddMinutes(1);
                //Console.WriteLine($"The job {workInfor.Id} with fail 4 in technician {workInfor.Technician} with {workInfor.StartPlannedDate} - {workInfor.EndPlannedDate}");

                for (int i = 0; i < workInfor.Technician.maintenanceTimes.Count - 1; i++)
                {
                    if (isInRange(workInfor.Technician.maintenanceTimes[i].From, workInfor.Technician.maintenanceTimes[i].To, workInfor.StartPlannedDate) || isInRange(workInfor.Technician.maintenanceTimes[i].From, workInfor.Technician.maintenanceTimes[i].To, workInfor.EndPlannedDate))
                    {
                        workInfor.StartPlannedDate = workInfor.Technician.maintenanceTimes[i].To.Add(TimeSpan.FromMinutes(1));
                    }
                }

                TimeSpan executionTime = TimeSpan.FromMinutes(workInfor.EstProcessTime);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
            }

            workInfor = checkTimeAvailable(workInfor, listDevice, listTechnician);
            if (workInfor.ArrayFail.IsOk == true)
            {
                return workInfor;
            }
            else if (workInfor.ArrayFail.IsOk != true)
            {
                workInfor = changePlannedDate(workInfor, listDevice, listTechnician, workLackPart);
                return workInfor;
            }

            return workInfor;
        }



        public static JobInfor findPlannedDate(JobInfor workInfor,
                                               List<JobInfor> dataJobInfor,
                                               List<Device> listDevice,
                                               List<Technician> listTechnician,
                                               List<WarehouseMaterial> warehouseMaterials,
                                               Work workLackPart)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            string nameOfDevice = workInfor.Device.code;
            //Console.WriteLine("--------------------------------------------------------------");
            //Console.WriteLine($"Name of device: {nameOfDevice}. And this is job {workInfor.Id}");
            int numberWorkOnDevice = listDevice.FirstOrDefault(x => x.code == nameOfDevice).maintenanceTimes.Count;
            //Console.WriteLine($"Number of Work on device: {numberWorkOnDevice}");

            var random = new Random();
            var randomId = random.Next(1, listTechnician.Count);
            var technicianId = listTechnician.ToList()[randomId];
            workInfor.Technician = technicianId;
            workInfor.ArrayFail = new ArrayFail(false, false, false, false, false);

            DateTime lastestStartDate = new DateTime();
            var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
            if (workLackPart != null)
            {
                var listMaterialInfoLack = CheckMaterial.checkMaterialAvailable(warehouseMaterials, workLackPart);
                lastestStartDate = warehouseMaterials.OrderByDescending(x => x.ExpectedDate).ToList()[0].ExpectedDate;
                workInfor.StartPlannedDate = lastestStartDate;
            }
            else
            {
                if (numberWorkOnDevice == 0)
                {
                    if (FirstDateStart.firstDateStart < workInfor.Device.workingTimes[0].From)
                    {
                        workInfor.StartPlannedDate = workInfor.Device.workingTimes[0].From;
                    }
                    else
                    {
                        workInfor.StartPlannedDate = FirstDateStart.firstDateStart;
                    }
                }
                else
                {
                    workInfor.StartPlannedDate = workInfor.Device.maintenanceTimes[numberWorkOnDevice - 1].To.Add(TimeSpan.FromMinutes(1));
                }
            }

            TimeSpan executionTime = TimeSpan.FromMinutes((double)workInfor.EstProcessTime);
            workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);

            workInfor = checkTimeAvailable(workInfor, listDevice, listTechnician);

            if (workInfor.ArrayFail.IsOk == true)
            {
                WorkingTime listStartEndWorking = new WorkingTime(workInfor.StartPlannedDate, workInfor.EndPlannedDate);

                workInfor.Device.maintenanceTimes.Add(listStartEndWorking);
                workInfor.Technician.maintenanceTimes.Add(listStartEndWorking);
            }
            else
            {
                workInfor = changePlannedDate(workInfor,
                                              listDevice, listTechnician,
                                              workLackPart);
                workInfor = checkTimeAvailable(workInfor, listDevice, listTechnician);

                WorkingTime listStartEndWorking = new WorkingTime(workInfor.StartPlannedDate, workInfor.EndPlannedDate);

                workInfor.Device.maintenanceTimes.Add(listStartEndWorking);
                workInfor.Technician.maintenanceTimes.Add(listStartEndWorking);
            }

            return workInfor;
        }
    }
}
