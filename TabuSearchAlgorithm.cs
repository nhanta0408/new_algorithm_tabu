using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabuSearchImplement.AggregateModels.JobInforAggregate;
using TabuSearchImplement.AggregateModels.MaterialAggregate;
using TabuSearchImplement.AggregateModels.JobInforAggregate;
using TabuSearchImplement;
using static TabuSearchImplement.Constant;
using static TabuSearchImplement.Constant;

namespace TabuSearchImplement
{
    public class TabuSearch
    {
        /// <summary>
        /// Determine the length of Tabu List
        /// </summary>
        /// <param name="workAvailableTable"></param>
        /// <returns></returns>
        public static int getTenure(DataTable workAvailableTable)
        {
            //int numberOfWorks = workAvailableTable.Rows.Count;
            //if (numberOfWorks < 10)
            //{
            //    return 2;
            //}
            //else if (numberOfWorks < 20)
            //{
            //    return 5;
            //}
            //else
            //{
            //    return 35;
            //}

            return 30;
        }


        /// <summary>
        /// Initialize the solution in the order 1, 2, 3,..., workAvailableTable.Rows.Count
        /// </summary>
        /// <param name="workAvailableTable"></param>
        /// <returns></returns>
        public static List<int> initialSolution(DataTable workAvailableTable)
        {
            List<int> solution = new List<int>();
            foreach (DataRow row in workAvailableTable.Rows)
            {
                solution.Add(Convert.ToInt32(row["No"]));
            }

            return solution;
        }


        /// <summary>
        /// Calculate the value of the objective function, the objective function is the sum of the early/delay times of the maintenance orders
        /// The Value of Object Function performed by minutes
        /// </summary>
        /// <param name="solution"></param>
        /// <param name="workAvailableTable"></param>
        /// <returns></returns>
        public static double objectValue(List<int> solution, DataTable workAvailableTable,
                                         List<JobInfor> bestListWorkInfor,
                                         Dictionary<string, List<List<DateTime>>> deviceDictionary,
                                         Dictionary<string, List<List<DateTime>>> technicianDictionary,
                                         Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime,
                                         Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime,
                                         DataTable wareHouseMaterialTable,
                                         DataTable materialTable,
                                         List<Material> listWorkAvailableChangedId,
                                         List<WareHouseMaterial> listwareHouseMaterials,
                                         List<Material> listMaterials)
        {
            DateTime startDate = DateTime.Now;
            double objectValue = 0;
            double totalminutes = 0;
            double maxLate = 0;
            int numberOfWorkLate = 0;
            DateTime endDate = DateTime.Now;

            List<Material> listWorkLackPart = TabuSearchImplement.CheckMaterial.getListWorkLackPartId(listwareHouseMaterials, listMaterials);

            List<int> solutionEnough = new List<int>();
            List<int> solutionLack = new List<int>();

            foreach (int item in solution)
            {
                bool isCheck = true;
                foreach (Material work in listWorkLackPart)
                {
                    if (item == work.Id)
                    {
                        isCheck = false;
                    }
                }

                if (isCheck)
                {
                    solutionEnough.Add(item);
                }
                else
                {
                    solutionLack.Add(item);
                }
            }

            foreach (int job in solutionEnough)
            {
                Material temp = new Material();
                JobInfor workInfor = new JobInfor();
                foreach (JobInfor workTemp in bestListWorkInfor)
                {
                    if (workTemp.Id == job)
                    {
                        workInfor = workTemp;
                    }
                }
                workInfor = FindPlannedDate.findPlannedDate(workInfor,
                                                            deviceDictionary, technicianDictionary,
                                                            maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime,
                                                            listwareHouseMaterials,
                                                            listWorkAvailableChangedId, temp);
                startDate = workInfor.StartPlannedDate;
                endDate = workInfor.EndPlannedDate;

                for(int i = 0; i < bestListWorkInfor.Count; i++)
                {
                    if (bestListWorkInfor[i].Id == workInfor.Id)
                    {
                        bestListWorkInfor[i] = workInfor;
                        break;
                    }
                }

                DateTime dueDate = Convert.ToDateTime((string)workAvailableTable.Rows[job - 1]["DueDate"]);
                int priority = int.Parse(workAvailableTable.Rows[job - 1]["Priority"].ToString());
                double differenceMinutes = 0;
                //if (endDate >= dueDate)
                //{
                //    // The unit of value returned from Ticks property is 10^-7 ticks/second
                //    differenceMinutes = TimeSpan.FromTicks((endDate - dueDate).Ticks).TotalMinutes;
                //    differenceMinutes = Math.Round(differenceMinutes, 0);
                //    //Console.WriteLine($"The delay time in each job = {differenceMinutes}");
                //}
                //else if (endDate < dueDate)
                //{
                //    differenceMinutes = TimeSpan.FromTicks((dueDate - endDate).Ticks).TotalMinutes;

                //    differenceMinutes = Math.Round(differenceMinutes, 0);
                //    //Console.WriteLine($"The early time in each job = {differenceMinutes}");
                //}

                if (endDate > dueDate)
                {
                    differenceMinutes = TimeSpan.FromTicks((endDate - dueDate).Ticks).TotalMinutes;
                    differenceMinutes = Math.Round(differenceMinutes, 0);
                }


                if (differenceMinutes != 0)
                {
                    numberOfWorkLate += 1;

                    if (maxLate < differenceMinutes)
                    {
                        maxLate = differenceMinutes;
                    }
                    totalminutes += differenceMinutes;
                    objectValue += differenceMinutes*priority;
                }

            }

            foreach (int job in solutionLack)
            {
                JobInfor workInfor = new JobInfor();
                foreach (JobInfor workTemp in bestListWorkInfor)
                {
                    if (workTemp.Id == job)
                    {
                        workInfor = workTemp;
                    }
                }

                Material workLackPart = new Material();
                foreach (Material work in listWorkLackPart)
                {
                    if (job == work.Id)
                    {
                        workLackPart = work;
                    }
                }

                workInfor = FindPlannedDate.findPlannedDate(workInfor,
                                                            deviceDictionary, technicianDictionary,
                                                            maintenanceDeviceBreakTime, maintenanceTechnicianWorkTime,
                                                            listwareHouseMaterials,
                                                            listWorkAvailableChangedId, workLackPart);
                startDate = workInfor.StartPlannedDate;
                endDate = workInfor.EndPlannedDate;

                for (int i = 0; i < bestListWorkInfor.Count; i++)
                {
                    if (bestListWorkInfor[i].Id == workInfor.Id)
                    {
                        bestListWorkInfor[i] = workInfor;
                        break;
                    }
                }

                DateTime dueDate = Convert.ToDateTime((string)workAvailableTable.Rows[job - 1]["DueDate"]);
                int priority = int.Parse(workAvailableTable.Rows[job - 1]["Priority"].ToString());

                double differenceMinutes = 0;
                //if (endDate >= dueDate)
                //{
                //    // The unit of value returned from Ticks property is 10^-7 ticks/second
                //    differenceMinutes = TimeSpan.FromTicks((endDate - dueDate).Ticks).TotalMinutes;
                //    differenceMinutes = Math.Round(differenceMinutes, 0);
                //    //Console.WriteLine($"The delay time in each job = {differenceMinutes}");
                //}
                //else if (endDate < dueDate)
                //{
                //    differenceMinutes = TimeSpan.FromTicks((dueDate - endDate).Ticks).TotalMinutes;

                //    differenceMinutes = Math.Round(differenceMinutes, 0);
                //    //Console.WriteLine($"The early time in each job = {differenceMinutes}");
                //}

                if (endDate > dueDate)
                {
                    differenceMinutes = TimeSpan.FromTicks((endDate - dueDate).Ticks).TotalMinutes;
                    differenceMinutes = Math.Round(differenceMinutes, 0);
                }

                
                if (differenceMinutes != 0)
                {
                    numberOfWorkLate += 1;
                    if (maxLate < differenceMinutes)
                    {
                        maxLate = differenceMinutes;
                    }

                    totalminutes += differenceMinutes;
                    objectValue += differenceMinutes * priority;
                }
            }
            //Console.WriteLine("----------------------");
            //Console.WriteLine($"The total minutes {totalminutes}");
            //Console.WriteLine($"The max different minutes: {maxLate}");
            //Console.WriteLine($"The object Value: {objectValue}");
            //Console.WriteLine($"Number of Work Late: {numberOfWorkLate}");
            //Console.WriteLine("----------------------");

            return objectValue;
        }


        /// <summary>
        /// Create a frame consisting of: Conversion Pair and Objective Function Value
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public static Dictionary<List<int>, double> tabuStructure(List<int> solution)
        {
            //tabuAttribute(conversion pair, move value)
            Dictionary<List<int>, double> tabuAttribute = new Dictionary<List<int>, double>();

            foreach (int i in solution)
            {
                if (i < solution.Count)
                {
                    List<int> listTemp = new List<int> { i, i + 1 };
                    tabuAttribute.Add(listTemp, 0);
                }
            }

            //foreach (var kvp in tabuAttribute)
            //{
            //    Console.WriteLine(kvp.Key[0].ToString() + " - " + kvp.Key[1].ToString());
            //    Console.WriteLine(kvp.Value);
            //}

            return tabuAttribute;
        }


        /// <summary>
        /// Takes a list (solution) returns a new neighbor solution with i, j swapped
        /// </summary>
        /// <param name="solution"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static List<int> swapPairs(List<int> solution, int i, int j)
        {
            int iIndex = solution.IndexOf(i);
            int jIndex = solution.IndexOf(j);
            int temp = solution[iIndex];
            solution[iIndex] = solution[jIndex];
            solution[jIndex] = temp;
            return solution;
        }


        /// <summary>
        /// Check: Have the considerd pair existed in Tabu List? 
        /// If it existed, return true. Otherwise, return false
        /// </summary>
        /// <param name="bestPair"></param>
        /// <param name="tabuList"></param>
        /// <returns></returns>
        public static bool checkTabuList(List<int> bestPair, List<List<int>> tabuList)
        {
            for (int i = 0; i < tabuList.Count; i++)
            {
                if ((bestPair[0] == tabuList[i][0]) && (bestPair[1] == tabuList[i][1]))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<List<int>> updateTabuList(List<int> bestMove, List<List<int>> tabuList)
        {
            if (tabuList.Count < 5)
            {
                tabuList.Add(bestMove);
            }
            else
            {
                for (int i = 0; i < (tabuList.Count - 1); i++)
                {
                    tabuList[i] = tabuList[i + 1];
                }
                tabuList[tabuList.Count - 1] = bestMove;
            }

            return tabuList;
        }


        /// <summary>
        /// Returns the swap pair have minimum objective function value in each iteration. 
        /// In addition, check: Have this pair already existed in the Tabu List? If not, add it to Tabu List!
        /// </summary>
        /// <param name="tabuAttribute"></param>
        /// <param name="tabuList"></param>
        /// <returns></returns>
        public static List<int> getBestPair(Dictionary<List<int>, double> tabuAttribute, List<List<int>> tabuList)
        {
            //Find the minimum Move Value in Tabu Attribute dictionary
            var keyMinMoveValue = tabuAttribute.MinBy(item => item.Value).Key;
            double minMoveValue = tabuAttribute[keyMinMoveValue];

            var listKey = tabuAttribute.Keys.ToList();
            var listValue = tabuAttribute.Values.ToList();

            //There can be many pairs with the same minimum value
            List<List<int>> listKeyMinValue = new List<List<int>>();
            List<int> bestPair = new List<int>();
            foreach (List<int> key in listKey)
            {
                if (tabuAttribute[key] == minMoveValue)
                {
                    listKeyMinValue.Add(key);
                }
            }

            //Console.WriteLine($"The number of Pair have the same minimum Object Value: {listKeyMinValue.Count}");

            for (int index = 0; index < listKeyMinValue.Count; index++)
            {
                bestPair = listKeyMinValue[index];
                //Console.WriteLine(bestPair[0].ToString() + " - " + bestPair[1].ToString());
                if (checkTabuList(bestPair, tabuList) == false)
                {
                    //tabuList.Add(bestPair);
                    tabuList = updateTabuList(bestPair, tabuList);
                    break;
                }
            }
            return bestPair;
        }

        public static DataTable getWorkNotEnoughMaterialTable(DataTable workTable, List<Material> listMaterialAvailable)
        {
            DataTable workRemoveTable = new DataTable();
            workRemoveTable.Columns.Add("No");
            workRemoveTable.Columns.Add("Priority");
            workRemoveTable.Columns.Add("Device");
            workRemoveTable.Columns.Add("Work");
            workRemoveTable.Columns.Add("DueDate");
            workRemoveTable.Columns.Add("ExecutionTime");

            var listRowRemove = new List<DataRow>();
            foreach (DataRow row in workTable.Rows)
            {
                int noOfRow = Convert.ToInt32(row["No"]);
                bool check = false;
                foreach (Material sparePartOnWork in listMaterialAvailable)
                {
                    if (noOfRow == sparePartOnWork.Id)
                    {
                        check = true;
                    }
                }

                if (check == false)
                {
                    DataRow newRow = workRemoveTable.NewRow();
                    newRow["No"] = row["No"];
                    newRow["Priority"] = row["Priority"];
                    newRow["Device"] = row["Device"];
                    newRow["Work"] = row["Work"];
                    newRow["DueDate"] = row["DueDate"];
                    newRow["ExecutionTime"] = row["ExecutionTime"];
                    workRemoveTable.Rows.Add(newRow);
                }
            }

            Console.WriteLine($"The length of workRemoveTable.Rows: {workRemoveTable.Rows.Count}");
            return workRemoveTable;
        }

        public static DataTable getWorkAvailableTable(DataTable workTable, List<Material> listsparePartAvailable, List<string> listDeviceNotEnoughTimes)
        {
            var listRowRemove = new List<DataRow>();
            Console.WriteLine($"The length of listDeviceNotEnoughTimes: {listDeviceNotEnoughTimes.Count}");
            foreach (DataRow row in workTable.Rows)
            {
                int noOfRow = Convert.ToInt32(row["No"]);
                bool check = false;
                foreach (Material sparePartOnWork in listsparePartAvailable)
                {
                    if (noOfRow == sparePartOnWork.Id)
                    {
                        check = true;
                    }
                }

                foreach (string device in listDeviceNotEnoughTimes)
                {
                    if (row["Device"].ToString() == device)
                    {
                        Console.WriteLine(row["No"].ToString() + " is enough time");
                        check = false;
                    }
                }

                if (check == false)
                {
                    Console.WriteLine(row["No"].ToString() + " is deleted");
                    listRowRemove.Add(row);
                }
            }

            foreach (DataRow row in listRowRemove)
            {
                workTable.Rows.Remove(row);
            }

            int i = 1;
            foreach (DataRow row in workTable.Rows)
            {
                row["No"] = i++;
            }

            //Console.WriteLine("Work Table available:");
            //foreach (DataRow row in workTable.Rows)
            //{
            //    Console.WriteLine(row["No"] + " " + row["Priority"] + " " + row["Device"] + " " + row["Work"] + " " + row["DueDate"] + " " + row["ExecutionTime"]);
            //}
            return workTable;
        }


        /// <summary>
        /// The implementation Tabu Search algorithm with short-term memory and pair swap as Tabu attribute
        /// </summary>
        /// <param name="workAvailableTable"></param>
        /// <returns></returns>
        public static List<JobInfor> tabuSearch(DataTable workAvailableTable,
                                                Dictionary<string, List<List<DateTime>>> deviceDictionary,
                                                Dictionary<string, List<List<DateTime>>> technicianDictionary,
                                                DataTable wareHouseMaterialTable,
                                                DataTable materialTable,
                                                List<WareHouseMaterial> listwareHouseMaterials,
                                                List<Material> listMaterials)
        {
            List<JobInfor> bestListWorkInfor = GetData.dataJobInfor(workAvailableTable);
            //for (int i = 0; i < workAvailableTable.Rows.Count; i++)
            //{
            //    JobInfor workInfor = new JobInfor();
            //    bestListWorkInfor.Add(workInfor);
            //}

            int tenure = getTenure(workAvailableTable);
            Console.WriteLine($"The Tenure: {tenure}");
            List<List<int>> tabuList = new List<List<int>>();
            List<int> listBestPair = new List<int>();

            List<Material> listWorkAvailableChangedId = CheckMaterial.getListWorkAvailableChangedId(listwareHouseMaterials, listMaterials);

            Dictionary<string, List<List<DateTime>>> maintenanceDeviceBreakTime = FindPlannedDate.deviceStructure(deviceDictionary);
            Dictionary<string, List<List<DateTime>>> maintenanceTechnicianWorkTime = FindPlannedDate.technicianStructure(technicianDictionary);
            List<int> currentSolution = initialSolution(workAvailableTable);
            for (int i = 0; i < currentSolution.Count; i++)
            {
                bestListWorkInfor[i].Id = currentSolution[i];
            }

            double bestObjectValue = objectValue(currentSolution, workAvailableTable,
                                                 bestListWorkInfor, deviceDictionary,
                                                 technicianDictionary, maintenanceDeviceBreakTime,
                                                 maintenanceTechnicianWorkTime, wareHouseMaterialTable,
                                                 materialTable, listWorkAvailableChangedId,
                                                 listwareHouseMaterials, listMaterials);
            List<int> bestSolution = currentSolution;

            int iterations = 200;
            int terminate = 0;
            List<double> listObjectValue = new List<double>();
            while (terminate < iterations)
            {
                // Searching the whole neighborhood of the current solution
                Dictionary<List<int>, double> dictTabuAttribute = tabuStructure(bestSolution);

                //Calculate the objective function value corresponding to each swap pair in TabuAttribute
                var listKey = dictTabuAttribute.Keys.ToList();

                foreach (List<int> key in listKey)
                {
                    List<int> candidateSolution = swapPairs(bestSolution, key[0], key[1]);
                    //foreach (int item in candidateSolution)
                    //{
                    //    Console.Write(item + " - ");
                    //}

                    maintenanceDeviceBreakTime = FindPlannedDate.deviceStructure(deviceDictionary);
                    maintenanceTechnicianWorkTime = FindPlannedDate.technicianStructure(technicianDictionary);

                    double candidateObjectValue = objectValue(candidateSolution, workAvailableTable,
                                                              bestListWorkInfor, deviceDictionary,
                                                              technicianDictionary, maintenanceDeviceBreakTime,
                                                              maintenanceTechnicianWorkTime, wareHouseMaterialTable,
                                                              materialTable, listWorkAvailableChangedId,
                                                              listwareHouseMaterials, listMaterials);
                    //Console.WriteLine(key[0].ToString() + " - " + key[1].ToString());
                    //Console.WriteLine(candidateObjectValue);
                    dictTabuAttribute[key] = candidateObjectValue;
                }

                maintenanceDeviceBreakTime = FindPlannedDate.deviceStructure(deviceDictionary);
                maintenanceTechnicianWorkTime = FindPlannedDate.technicianStructure(technicianDictionary);

                //Select the move with the lowest ObjValue in the neighborhood (minimization)              
                listBestPair = getBestPair(dictTabuAttribute, tabuList);
                //Console.WriteLine("---------------------------------------------------------");
                //Console.WriteLine($"The size of ListBestPair = {listBestPair.Count}");
                //Console.WriteLine($"The Best Pair : {listBestPair[0]} - {listBestPair[1]}");

                //Console.WriteLine("The elements in Tabu List");
                //for (int i = 0; i < tabuList.Count; i++)
                //{
                //    Console.Write(tabuList[i][0].ToString() + " - " + tabuList[i][1].ToString() + "||");
                //}
                //Console.WriteLine();
                //Console.WriteLine("---------------------------------------------------------");


                if (listBestPair.Count > 0)
                {
                    currentSolution = swapPairs(bestSolution, listBestPair[0], listBestPair[1]);
                    double currentObjectValue = objectValue(currentSolution, workAvailableTable,
                                                            bestListWorkInfor, deviceDictionary,
                                                            technicianDictionary, maintenanceDeviceBreakTime,
                                                            maintenanceTechnicianWorkTime, wareHouseMaterialTable,
                                                            materialTable, listWorkAvailableChangedId,
                                                            listwareHouseMaterials, listMaterials);

                    if (currentObjectValue < bestObjectValue)
                    {
                        bestSolution = currentSolution;
                        bestObjectValue = currentObjectValue;
                    }
                    //Console.WriteLine($"The Best Pair in terminal {terminate}: {listBestPair[0]} - {listBestPair[1]}");
                    //Console.WriteLine($"The Object Value for Best Pair in terminal {terminate}: {bestObjectValue}");
                }

                listObjectValue.Add(bestObjectValue);
                terminate += 1;
            }

            //Console.WriteLine("The best Solution with the minimum Object Value");
            //foreach (int item in bestSolution)
            //{
            //    Console.Write(item + " - ");
            //}
            //Console.WriteLine($"The best object Value: {bestObjectValue}");

            //List<DateTime> listStartDate = new List<DateTime>();
            //foreach (JobInfor workInfor in bestListWorkInfor)
            //{
            //    listStartDate.Add(workInfor.StartPlannedDate);
            //}

            //listStartDate.Sort();

            //Console.WriteLine();
            //Console.WriteLine("Print planned date for all job in Timeline");
            //for (int i = 0; i < listStartDate.Count; i++)
            //{
            //    foreach (JobInfor workInfor in bestListWorkInfor)
            //    {
            //        if (workInfor.StartPlannedDate == listStartDate[i])
            //        {
            //            Console.WriteLine($"The job: {workInfor.Id}. Planned Date: {workInfor.StartPlannedDate} - {workInfor.EndPlannedDate}. Device: {workInfor.Device}. Technician: {workInfor.Technician}");
            //            break;
            //        }
            //    }
            //}

            //Console.WriteLine();
            //Console.WriteLine("Print all of maintenance device's record");
            //foreach (string key in maintenanceDeviceBreakTime.Keys)
            //{
            //    Console.WriteLine($"The name of device: {key}");
            //    List<DateTime> listStartDevice = new List<DateTime>();
            //    foreach (List<DateTime> listTime in maintenanceDeviceBreakTime[key])
            //    {
            //        listStartDevice.Add(listTime[0]);
            //    }
            //    listStartDevice.Sort();

            //    for (int i = 0; i < listStartDevice.Count; i++)
            //    {
            //        foreach (List<DateTime> listTime in maintenanceDeviceBreakTime[key])
            //        {
            //            if (listTime[0] == listStartDevice[i])
            //            {
            //                Console.WriteLine(listTime[0].ToString() + " " + listTime[1].ToString());
            //            }
            //        }
            //    }
            //}

            //Console.WriteLine();
            //Console.WriteLine("Print all of maintenance technician's record");
            //foreach (string key in maintenanceTechnicianWorkTime.Keys)
            //{
            //    Console.WriteLine($"The id of technician: {key}");
            //    List<DateTime> listStartTechnician = new List<DateTime>();
            //    foreach (List<DateTime> listTime in maintenanceTechnicianWorkTime[key])
            //    {
            //        listStartTechnician.Add(listTime[0]);
            //    }
            //    listStartTechnician.Sort();

            //    for (int i = 0; i < listStartTechnician.Count; i++)
            //    {
            //        foreach (List<DateTime> listTime in maintenanceTechnicianWorkTime[key])
            //        {
            //            if (listTime[0] == listStartTechnician[i])
            //            {
            //                Console.WriteLine(listTime[0].ToString() + " " + listTime[1].ToString());
            //            }
            //        }
            //    }
            //}
            //Console.WriteLine();


            //foreach (string key in maintenanceTechnicianWorkTime.Keys)
            //{
            //    Console.WriteLine($"The sequence of technician is checked: {key}");
            //    List<DateTime> listStartTechnician = new List<DateTime>();
            //    foreach (List<DateTime> listTime in maintenanceTechnicianWorkTime[key])
            //    {
            //        listStartTechnician.Add(listTime[0]);
            //    }
            //    listStartTechnician.Sort();

            //    for (int i = 0; i < listStartTechnician.Count; i++)
            //    {
            //        foreach (List<DateTime> listTime in maintenanceTechnicianWorkTime[key])
            //        {
            //            if (listTime[0] == listStartTechnician[i])
            //            {
            //                Console.WriteLine(listTime[0].ToString());
            //            }
            //        }
            //    }
            //    Console.WriteLine("-------------------------------------------");

            //    for (int i = 0; i < listStartTechnician.Count; i++)
            //    {
            //        foreach (List<DateTime> listTime in maintenanceTechnicianWorkTime[key])
            //        {
            //            if (listTime[0] == listStartTechnician[i])
            //            {
            //                Console.WriteLine(listTime[1].ToString());
            //            }
            //        }
            //    }
            //    Console.WriteLine("-------------------------------------------");
            //}

            List<JobInfor> newListJobInfor = new List<JobInfor>();
            foreach (int item in bestSolution)
            {
                //Console.Write(item + " - ");
                foreach (JobInfor workInfor in bestListWorkInfor)
                {
                    if (item == workInfor.Id)
                    {
                        newListJobInfor.Add(workInfor);
                        break;
                    }
                }
            }

            //foreach (double item in listObjectValue)
            //{
            //    Console.WriteLine(item);
            //}

            //foreach(JobInfor work in newListJobInfor)
            //{
            //    Console.WriteLine($"The work id: {work.Id} on device: {work.Device} and technician: {work.Technician} has due date: {work.DueDate} and planned date: {work.StartPlannedDate} - {work.EndPlannedDate}");
            //}
            return newListJobInfor;
        }
    }

}
