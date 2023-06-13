using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabuSearchImplement.AggregateModels.JobInforAggregate;
using TabuSearchImplement.AggregateModels.MaterialAggregate;
using TabuSearchImplement.AggregateModels.JobInforAggregate;
using static TabuSearchImplement.Constant;
using static TabuSearchImplement.Constant;
using TabuSearchImplement.Repository;

namespace TabuSearchImplement
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
                                                  Dictionary<string, List<List<DateTime>>> deviceDictionary,
                                                  Dictionary<string, List<List<DateTime>>> technicianDictionary,
                                                  Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime,
                                                  Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime)
        {
            //var mondayOfThisWeek = DateTime.ParseExact("23/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //var saturdayOfThisWeek = DateTime.ParseExact("28/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //var fridayOfNextWeek = DateTime.ParseExact("05/05/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
            var fridayOfNextWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Friday).AddDays(7);

            var lengthOfDevice = deviceDictionary[workInfor.Device].Count;
            var lengthOfTechnician = technicianDictionary[workInfor.Technician.ToString()].Count;
            if (workInfor.StartPlannedDate > saturdayOfThisWeek || workInfor.EndPlannedDate > saturdayOfThisWeek || workInfor.EndPlannedDate > deviceDictionary[workInfor.Device][lengthOfDevice - 1][1] || workInfor.EndPlannedDate > technicianDictionary[workInfor.Technician.ToString()][lengthOfTechnician - 1][1])
            {
                //Console.WriteLine($"The work Id {workInfor.Id} on device {workInfor.Device} and technician {workInfor.Technician} has planned date {workInfor.StartPlannedDate} - {workInfor.EndPlannedDate} out of week");
                workInfor.StartPlannedDate = fridayOfNextWeek;
                TimeSpan executionTime = TimeSpan.FromMinutes(workInfor.EstProcessTime);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
                workInfor.ArrayFail[0] = 1;
                return workInfor;
            }

            bool checkDeviceAvailable = false;
            //Console.WriteLine($"The work {workInfor.Id} check time available with planned date: {workInfor.StartPlannedDate} - {workInfor.EndPlannedDate}");
            foreach (List<DateTime> listBreakTime in deviceDictionary[workInfor.Device])
            {
                //Console.WriteLine($"The break time of device {workInfor.Device} is: {listBreakTime[0]} - {listBreakTime[1]}");
                if (isInRange(listBreakTime[0], listBreakTime[1], workInfor.StartPlannedDate) && isInRange(listBreakTime[0], listBreakTime[1], workInfor.EndPlannedDate))
                {
                    //Console.WriteLine($"The break time of device {workInfor.Device} is available in range: {listBreakTime[0]} - {listBreakTime[1]}");
                    checkDeviceAvailable = true;
                    break;
                }
            }

            bool checkTechnicianAvailable = false;
            List<string> listNoTechnician = technicianDictionary.Keys.ToList();
            List<string> newListNoTechnician = shuffleList(listNoTechnician);
            if (workInfor.Technician == "")
            {
                foreach (string id in newListNoTechnician)
                {
                    foreach (List<DateTime> listWorkTime in technicianDictionary[id])
                    {
                        //Console.WriteLine($"The work time of technician {no} is: {listWorkTime[0]} - {listWorkTime[1]}");
                        if (isInRange(listWorkTime[0], listWorkTime[1], workInfor.StartPlannedDate) && isInRange(listWorkTime[0], listWorkTime[1], workInfor.EndPlannedDate))
                        {
                            //Console.WriteLine($"The work time of technician {no} is available in range: {listWorkTime[0]} - {listWorkTime[1]}");
                            checkTechnicianAvailable = true;
                            break;
                        }
                    }

                    if (checkTechnicianAvailable && workInfor.Technician == "")
                    {
                        workInfor.Technician = id;
                        break;
                    }
                }
            }
            else if (workInfor.Technician != "")
            {
                foreach (List<DateTime> listWorkTime in technicianDictionary[workInfor.Technician.ToString()])
                {
                    //Console.WriteLine($"The work time of technician {no} is: {listWorkTime[0]} - {listWorkTime[1]}");
                    if (isInRange(listWorkTime[0], listWorkTime[1], workInfor.StartPlannedDate) && isInRange(listWorkTime[0], listWorkTime[1], workInfor.EndPlannedDate))
                    {
                        //Console.WriteLine($"The work time of technician {workInfor.Technician} is available in range: {listWorkTime[0]} - {listWorkTime[1]}");
                        checkTechnicianAvailable = true;
                        break;
                    }
                }

                if (checkTechnicianAvailable == false)
                {
                    foreach (string id in newListNoTechnician)
                    {
                        foreach (List<DateTime> listWorkTime in technicianDictionary[id])
                        {
                            //Console.WriteLine($"The work time of technician {no} is: {listWorkTime[0]} - {listWorkTime[1]}");
                            if (isInRange(listWorkTime[0], listWorkTime[1], workInfor.StartPlannedDate) && isInRange(listWorkTime[0], listWorkTime[1], workInfor.EndPlannedDate))
                            {
                                //Console.WriteLine($"The work time of technician {no} is available in range: {listWorkTime[0]} - {listWorkTime[1]}");
                                checkTechnicianAvailable = true;
                                break;
                            }
                        }

                        if (checkTechnicianAvailable)
                        {
                            workInfor.Technician = id;
                            break;
                        }
                    }
                }
            }
            

            if (checkDeviceAvailable && checkTechnicianAvailable)
            {
                bool checkTechnicianDuplicate = false;
                //Console.WriteLine($"Check the fail 4 for job {workInfor.Id}: {workInfor.StartPlannedDate} - {workInfor.EndPlannedDate}");
                foreach (List<DateTime> listTechnicianWorkTime in maintenanceTechnicianWorkTime[workInfor.Technician.ToString()])
                {
                    if (isInRange(listTechnicianWorkTime[0], listTechnicianWorkTime[1], workInfor.StartPlannedDate) || isInRange(listTechnicianWorkTime[0], listTechnicianWorkTime[1], workInfor.EndPlannedDate))
                    {
                        //Console.WriteLine($"Job {workInfor.Id} has fail 4 with duplicated time: {listTechnicianWorkTime[0]} - {listTechnicianWorkTime[1]}");
                        checkTechnicianDuplicate = true;
                        break;
                    }

                    if (isInRange(workInfor.StartPlannedDate, workInfor.EndPlannedDate, listTechnicianWorkTime[0]) && isInRange(workInfor.StartPlannedDate, workInfor.EndPlannedDate, listTechnicianWorkTime[1]))
                    {
                        //Console.WriteLine($"{listTechnicianWorkTime[0]} - {listTechnicianWorkTime[1]} exist inside the planned date of job {workInfor.Id}");
                        checkTechnicianDuplicate = true;
                        break;
                    }
                }

                if (checkTechnicianDuplicate == true)
                {
                    workInfor.ArrayFail[4] = 1;
                }

                bool checkDeviceDuplicate = false;
                foreach (List<DateTime> listDeviceBreakTime in maintenanceDeviceBreakTime[workInfor.Device])
                {
                    if (isInRange(listDeviceBreakTime[0], listDeviceBreakTime[1], workInfor.StartPlannedDate) || isInRange(listDeviceBreakTime[0], listDeviceBreakTime[1], workInfor.EndPlannedDate))
                    {
                        checkDeviceDuplicate = true;
                        break;
                    }
                }

                if (checkDeviceDuplicate == true)
                {
                    workInfor.ArrayFail[3] = 1;
                }

                if (checkDeviceDuplicate == false && checkTechnicianDuplicate == false)
                {
                    // If everything is ok
                    workInfor.ArrayFail[0] = 1;
                }
            }
            else if (checkDeviceAvailable == false)
            {
                // If the break time of device is not available, return 1
                workInfor.ArrayFail[1] = 1;
            }
            else if (checkTechnicianAvailable == false)
            {
                // If the work time of technician is not available, return 2
                workInfor.ArrayFail[2] = 1;
            }

            return workInfor;
        }

        public static JobInfor changePlannedDate(JobInfor workInfor,
                                                 Dictionary<string, List<List<DateTime>>> deviceDictionary,
                                                 Dictionary<string, List<List<DateTime>>> technicianDictionary,
                                                 Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime,
                                                 Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime,
                                                 Material workLackPart)
        {
            if (workInfor.ArrayFail[1] == 1)
            {
                if (workLackPart.Id == null)
                {
                    //Console.WriteLine($"The job {workInfor.Id} has the fail 1 on the {workInfor.Device} and technician {workInfor.Technician}");
                    for (int i = 0; i < (deviceDictionary[workInfor.Device].Count - 1); i++)
                    {
                        if (deviceDictionary[workInfor.Device][i][0] <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= deviceDictionary[workInfor.Device][i + 1][0])
                        {
                            workInfor.StartPlannedDate = deviceDictionary[workInfor.Device][i + 1][0].Add(TimeSpan.FromMinutes(1));
                            break;
                        }
                    }
                }
                else if (workLackPart.Id != null)
                {
                    for (int i = 0; i < (deviceDictionary[workInfor.Device].Count - 1); i++)
                    {
                        if (deviceDictionary[workInfor.Device][i][0] <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= deviceDictionary[workInfor.Device][i + 1][0])
                        {
                            workInfor.StartPlannedDate = deviceDictionary[workInfor.Device][i + 1][0].Add(TimeSpan.FromMinutes(1));
                            break;
                        }
                    }
                }

                //Console.WriteLine($"The job {workInfor.Id} with fail 1 has new planned start is {workInfor.StartPlannedDate} on the device {workInfor.Device}");

                TimeSpan executionTime = TimeSpan.FromMinutes(workInfor.EstProcessTime);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
            }

            if (workInfor.ArrayFail[2] == 1)
            {
                if (workLackPart.Id == null)
                {
                    //Console.WriteLine($"The job {workInfor.Id} has the fail 2 on the {workInfor.Device} and technician {workInfor.Technician} with the planned start before {workInfor.StartPlannedDate}");
                    if (workInfor.StartPlannedDate < technicianDictionary[workInfor.Technician.ToString()][0][0])
                    {
                        workInfor.StartPlannedDate = technicianDictionary[workInfor.Technician.ToString()][0][0].AddMinutes(1);
                    }
                    else
                    {
                        for (int i = 0; i < (technicianDictionary[workInfor.Technician.ToString()].Count - 1); i++)
                        {
                            if (technicianDictionary[workInfor.Technician.ToString()][i][0] <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= technicianDictionary[workInfor.Technician.ToString()][i + 1][0])
                            {
                                workInfor.StartPlannedDate = technicianDictionary[workInfor.Technician.ToString()][i + 1][0].Add(TimeSpan.FromMinutes(1));
                                break;
                            }
                        }
                    }
                }
                else if (workLackPart.Id != null)
                {
                    if (workInfor.StartPlannedDate < technicianDictionary[workInfor.Technician.ToString()][0][0])
                    {
                        workInfor.StartPlannedDate = technicianDictionary[workInfor.Technician.ToString()][0][0].AddMinutes(1);
                    }
                    else 
                    {
                        for (int i = 0; i < (technicianDictionary[workInfor.Technician.ToString()].Count - 1); i++)
                        {
                            if (technicianDictionary[workInfor.Technician.ToString()][i][0] <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= technicianDictionary[workInfor.Technician.ToString()][i + 1][0])
                            {
                                workInfor.StartPlannedDate = technicianDictionary[workInfor.Technician.ToString()][i + 1][0].Add(TimeSpan.FromMinutes(1));
                                break;
                            }
                        }
                    }
                }

                //Console.WriteLine($"The job {workInfor.Id} with fail 2 has new planned start is {workInfor.StartPlannedDate} and sequence of technician {workInfor.Technician}");

                TimeSpan executionTime = TimeSpan.FromMinutes(workInfor.EstProcessTime);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
            }

            if (workInfor.ArrayFail[3] == 1)
            {
                int numberOfWorkOnDevice = maintenanceDeviceBreakTime[workInfor.Device].Count;
                workInfor.StartPlannedDate = maintenanceDeviceBreakTime[workInfor.Device][numberOfWorkOnDevice - 1][1].Add(TimeSpan.FromMinutes(1));
                for (int i = 0; i < (deviceDictionary[workInfor.Device].Count - 1); i++)
                {
                    if (deviceDictionary[workInfor.Device][i][0] <= workInfor.StartPlannedDate && workInfor.StartPlannedDate <= deviceDictionary[workInfor.Device][i + 1][0])
                    {
                        workInfor.StartPlannedDate = deviceDictionary[workInfor.Device][i + 1][0].Add(TimeSpan.FromMinutes(1));
                        break;
                    }
                }

                TimeSpan executionTime = TimeSpan.FromMinutes(workInfor.EstProcessTime);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
                //Console.WriteLine($"The job {workInfor.Id} with fail 3 has new planned start is {workInfor.StartPlannedDate}");
            }

            if (workInfor.ArrayFail[4] == 1)
            {
                if (maintenanceTechnicianWorkTime[workInfor.Technician.ToString()].Count > 8)
                {
                    Random rnd = new Random();
                    var randomId = rnd.Next(1, maintenanceTechnicianWorkTime.Count);
                    var technicianId = maintenanceTechnicianWorkTime.Keys.ToList()[randomId];
                    workInfor.Technician = technicianId;
                }

                workInfor.StartPlannedDate = workInfor.StartPlannedDate.AddMinutes(1);
                workInfor.EndPlannedDate = workInfor.EndPlannedDate.AddMinutes(1);
                //Console.WriteLine($"The job {workInfor.Id} with fail 4 in technician {workInfor.Technician} with {workInfor.StartPlannedDate} - {workInfor.EndPlannedDate}");

                for (int i = 0; i < maintenanceTechnicianWorkTime[workInfor.Technician.ToString()].Count - 1; i++)
                {
                    if (isInRange(maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][0], maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][1], workInfor.StartPlannedDate) || isInRange(maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][0], maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][1], workInfor.EndPlannedDate))
                    {
                        //Console.WriteLine($"The job {workInfor.Id} is duplicated in technician {workInfor.Technician} with duplicated job: {maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][0]} - {maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][1]}");
                        workInfor.StartPlannedDate = maintenanceTechnicianWorkTime[workInfor.Technician.ToString()][i][1].Add(TimeSpan.FromMinutes(1));
                    }
                }

                //Console.WriteLine($"The job {workInfor.Id} with fail 4 has new planned start is {workInfor.StartPlannedDate}");

                TimeSpan executionTime = TimeSpan.FromMinutes(workInfor.EstProcessTime);
                workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
            }

            workInfor = checkTimeAvailable(workInfor, deviceDictionary, technicianDictionary,
                                           maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime);
            if (workInfor.ArrayFail[0] == 1)
            {
                return workInfor;
            }
            else if (workInfor.ArrayFail[0] != 1)
            {
                workInfor = changePlannedDate(workInfor,
                                              deviceDictionary, technicianDictionary,
                                              maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime,
                                              workLackPart);
                return workInfor;
            }

            return workInfor;
        }



        public static JobInfor findPlannedDate(JobInfor workInfor,
                                               Dictionary<string, List<List<DateTime>>> deviceDictionary,
                                               Dictionary<string, List<List<DateTime>>> technicianDictionary,
                                               Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime,
                                               Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime,
                                               List<WareHouseMaterial> listWareHouseMaterials,
                                               List<Material> listWorkAvailableChangedId,
                                               Material workLackPart)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;

            string nameOfDevice = workInfor.Device;
            //Console.WriteLine("--------------------------------------------------------------");
            //Console.WriteLine($"Name of device: {nameOfDevice}. And this is job {workInfor.Id}");
            int numberWorkOnDevice = maintenanceDeviceBreakTime[nameOfDevice].Count;
            //Console.WriteLine($"Number of Work on device: {numberWorkOnDevice}");

            var random = new Random();
            var randomId = random.Next(1, technicianDictionary.Count);
            var technicianId = technicianDictionary.Keys.ToList()[randomId];
            workInfor.Technician = technicianId;
            workInfor.ArrayFail = new int[5];

            DateTime lastestStartDate = new DateTime();
            //var mondayOfThisWeek = DateTime.ParseExact("23/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //var saturdayOfThisWeek = DateTime.ParseExact("28/04/2023 17:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            var mondayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var saturdayOfThisWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Saturday);
            if (workLackPart.Id != null)
            {
                //Console.WriteLine($"The job {workInfor.Id} is lack spare part");
                var listSequencePartLack = TabuSearchImplement.CheckMaterial.returnListSequencePartLackOnWork(listWareHouseMaterials, listWorkAvailableChangedId, workLackPart);
                //Console.WriteLine($"The length of listSequencePartLack: {listSequencePartLack.Count}");
                lastestStartDate = listWareHouseMaterials[listSequencePartLack[0] - 1].ExpectedAddDate;
                //Console.WriteLine($"The sequence part is {listSequencePartLack[0]} with expected add date {listWareHouseMaterials[listSequencePartLack[0] - 1].ExpectedAddDate}");

                if (listSequencePartLack.Count >= 2)
                {
                    foreach (var sequence in listSequencePartLack)
                    {
                        //Console.WriteLine($"The sequence part is {sequence} with expected add date {listWareHouseMaterials[sequence - 1].ExpectedAddDate}");
                        if (listWareHouseMaterials[sequence].ExpectedAddDate > lastestStartDate)
                        {
                            lastestStartDate = listWareHouseMaterials[sequence].ExpectedAddDate;
                        }
                    }
                }


                workInfor.StartPlannedDate = lastestStartDate;
                //Console.WriteLine($"The start date in job {workInfor.Id} is {workInfor.StartPlannedDate}");
            }
            else
            {
                if (numberWorkOnDevice == 0)
                {
                    if (ObjectInputRepository.firstDateStart < deviceDictionary[workInfor.Device][0][0])
                    {
                        workInfor.StartPlannedDate = deviceDictionary[workInfor.Device][0][0];
                    }
                    else
                    {
                        workInfor.StartPlannedDate = ObjectInputRepository.firstDateStart;
                    }
                    //workInfor.StartPlannedDate = deviceDictionary[workInfor.Device][0][0].Add(TimeSpan.FromMinutes(1));
                    //Console.WriteLine($"The start date in job {workInfor.Id} on device {workInfor.Device} is {workInfor.StartPlannedDate}");

                }
                else
                {
                    workInfor.StartPlannedDate = maintenanceDeviceBreakTime[workInfor.Device][numberWorkOnDevice - 1][1].Add(TimeSpan.FromMinutes(1));
                    //Console.WriteLine($"The start date in job {workInfor.Id} on device {workInfor.Device} is {workInfor.StartPlannedDate}");
                }
            }

            TimeSpan executionTime = TimeSpan.FromMinutes(workInfor.EstProcessTime);
            workInfor.EndPlannedDate = workInfor.StartPlannedDate.Add(executionTime);
            //Console.WriteLine($"The end date in job {workInfor.Id} on device {workInfor.Device} is {workInfor.EndPlannedDate}");

            workInfor = checkTimeAvailable(workInfor, deviceDictionary,
                                           technicianDictionary, maintenanceDeviceBreakTime,
                                           maintenanceTechnicianWorkTime);
            //Console.WriteLine($"The Fail's number: {workInfor.ArrayFail[0]} - {workInfor.ArrayFail[1]} - {workInfor.ArrayFail[2]} - {workInfor.ArrayFail[3]} - {workInfor.ArrayFail[4]}");

            if (workInfor.ArrayFail[0] == 1)
            {
                List<DateTime> listStartEndWorking = new List<DateTime> { workInfor.StartPlannedDate, workInfor.EndPlannedDate };

                List<List<DateTime>> listListDeviceBreakingTime = maintenanceDeviceBreakTime[workInfor.Device];
                List<List<DateTime>> listListTempDevice = new List<List<DateTime>>();
                foreach (List<DateTime> listTemp in listListDeviceBreakingTime)
                {
                    listListTempDevice.Add(listTemp);
                }
                listListTempDevice.Add(listStartEndWorking);
                maintenanceDeviceBreakTime[workInfor.Device] = new List<List<DateTime>>();
                maintenanceDeviceBreakTime[workInfor.Device] = listListTempDevice;


                List<List<DateTime>> listListTechnicianWorkingTime = maintenanceTechnicianWorkTime[workInfor.Technician.ToString()];
                List<List<DateTime>> listListTempTechnician = new List<List<DateTime>>();
                foreach (List<DateTime> listTemp in listListTechnicianWorkingTime)
                {
                    listListTempTechnician.Add(listTemp);
                }
                listListTempTechnician.Add(listStartEndWorking);

                maintenanceTechnicianWorkTime[workInfor.Technician.ToString()] = new List<List<DateTime>>();
                maintenanceTechnicianWorkTime[workInfor.Technician.ToString()] = listListTempTechnician;
            }
            else
            {
                // Get the modified date by changePlannedDate
                workInfor = changePlannedDate(workInfor,
                                              deviceDictionary, technicianDictionary,
                                              maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime,
                                              workLackPart);
                // Get the sequence of Technician perform this job
                workInfor = checkTimeAvailable(workInfor, deviceDictionary,
                                               technicianDictionary, maintenanceDeviceBreakTime,
                                               maintenanceTechnicianWorkTime);

                List<DateTime> listStartEndWorking = new List<DateTime> { workInfor.StartPlannedDate, workInfor.EndPlannedDate };
                // Add the record into maintenanceDeviceBreakTime
                List<List<DateTime>> listListDeviceBreakingTime = maintenanceDeviceBreakTime[workInfor.Device];
                List<List<DateTime>> listListTempDevice = new List<List<DateTime>>();
                foreach (List<DateTime> listTemp in listListDeviceBreakingTime)
                {
                    listListTempDevice.Add(listTemp);
                }
                listListTempDevice.Add(listStartEndWorking);
                maintenanceDeviceBreakTime[workInfor.Device] = new List<List<DateTime>>();
                maintenanceDeviceBreakTime[workInfor.Device] = listListTempDevice;

                // Add the record into maintenanceTechnicianWorkTime
                List<List<DateTime>> listListTechnicianWorkingTime = maintenanceTechnicianWorkTime[workInfor.Technician.ToString()];
                List<List<DateTime>> listListTempTechnician = new List<List<DateTime>>();
                foreach (List<DateTime> listTemp in listListTechnicianWorkingTime)
                {
                    listListTempTechnician.Add(listTemp);
                }
                listListTempTechnician.Add(listStartEndWorking);
                maintenanceTechnicianWorkTime[workInfor.Technician.ToString()] = new List<List<DateTime>>();
                maintenanceTechnicianWorkTime[workInfor.Technician.ToString()] = listListTempTechnician;
            }

            //Console.WriteLine();
            //Console.WriteLine($"The start and end planned date are: {workInfor.StartPlannedDate} - {workInfor.EndPlannedDate}");
            //Console.WriteLine();
            ////Print all of maintenance device's record
            //foreach (string key in maintenanceDeviceBreakTime.Keys)
            //{
            //    Console.WriteLine($"The name of device is checked: {key}");
            //    foreach (List<DateTime> listTime in maintenanceDeviceBreakTime[key])
            //    {
            //        Console.WriteLine(listTime[0].ToString() + " " + listTime[1].ToString());
            //    }
            //}

            //Console.WriteLine();
            //// Print all of maintenance technician's record
            //foreach (string key in maintenanceTechnicianWorkTime.Keys)
            //{
            //    Console.WriteLine($"The sequence of technician is checked: {key}");
            //    foreach (List<DateTime> listTime in maintenanceTechnicianWorkTime[key])
            //    {
            //        Console.WriteLine(listTime[0].ToString() + " " + listTime[1].ToString());
            //    }
            //}
            //Console.WriteLine();

            return workInfor;
        }
    }
}
