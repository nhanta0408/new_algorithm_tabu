using System.Data;
using System.Globalization;
using TabuSearchImplement.AggregateModels.DeviceAggregate;
using TabuSearchImplement.AggregateModels.InputAggregate;
using TabuSearchImplement.AggregateModels.JobInforAggregate;
using TabuSearchImplement.AggregateModels.TechnicianAggregate;
using TabuSearchImplement.AggregateModels.WareHouseMaterialAggregate;
using TabuSearchImplement.AggregateModels.WorkAggregate;
using static TabuSearchImplement.Constant;

namespace TabuSearchImplement.Repository
{
    public class ObjectInputRepository : IObjectInputRepository
    {
        public static DateTime firstDateStart { get; set; }

        public ListJobInforReturn Implement(ObjectInput input)
        {
            //List<WorkObjectInput> listWorkObjects = input.works.JsonInput.ToList();
            //List<DeviceObjectInput> listDeviceObjects = input.devices.JsonInput.ToList();
            //List<TechnicianObjectInput> listTechnicianObjects = input.technicians.JsonInput.ToList();
            //List<WareHouseMaterialObjectInput> listWareHouseMaterialObjects = input.wareHouseMaterials.JsonInput.ToList();
            firstDateStart = input.firstDateStart;

            //Dictionary<string, List<List<DateTime>>> deviceBreakingTime = ConvertFromObjectToTable.getDeviceDictionary(listDeviceObjects);
            //Dictionary<string, List<List<DateTime>>> technicianWorkingTime = ConvertFromObjectToTable.getTechnicianDictionary(listTechnicianObjects);

            List<Work> works = new List<Work>();
            List<Device> devices = new List<Device>();
            List<Technician> technicians = new List<Technician>();
            List<WareHouseMaterial> wareHouseMaterials = new List<WareHouseMaterial>();
            List<Material> materials = new List<Material>();

            //var workTable = ConvertFromObjectToTable.ConvertObjectInputToWorksTable(listWorkObjects);
            //List<WareHouseMaterial> listwareHouseMaterials = CheckMaterial.getWareHouseMaterial(listWareHouseMaterialObjects);
            //List<Material> listMaterials = CheckMaterial.getListMaterial(listWorkObjects, listwareHouseMaterials);


            DataTable wareHouseMaterialTable = ConvertFromObjectToTable.ConvertToWareHouseMaterialTable(listwareHouseMaterials);
            DataTable materialTable = ConvertFromObjectToTable.ConvertToMaterialTable(listMaterials);

            List<Material> listMaterialAvailable = CheckMaterial.getListWorkAvailable(listwareHouseMaterials, listMaterials);
            List<Device> listDeviceNotEnoughTimes = CheckMaterial.returnDevicesNotEnoughTime(workTable, devices);
            DataTable workRemoveTable = TabuSearch.getWorkNotEnoughMaterialTable(workTable, listMaterialAvailable);
            DataTable workAvailableTable = TabuSearch.getWorkAvailableTable(workTable, listMaterialAvailable, listDeviceNotEnoughTimes);

            List<JobInfor> listJobAvailable = new List<JobInfor>();
            ListJobInforReturn listJobReturn = new ListJobInforReturn(scheduled: new List<JobInfor>(),
                                                                      rejected: new List<JobInfor>());
            if (workAvailableTable.Rows.Count > 1)
            {
                listJobAvailable = TabuSearch.tabuSearch(workAvailableTable, deviceBreakingTime, technicianWorkingTime,
                                                        wareHouseMaterialTable, materialTable,
                                                        listwareHouseMaterials, listMaterials);

                listJobReturn = CheckMaterial.listJobReturn(listWorkObjects, listJobAvailable, listDeviceNotEnoughTimes,
                                                            deviceBreakingTime, technicianWorkingTime, wareHouseMaterialTable,
                                                            materialTable, listwareHouseMaterials, listMaterials, workRemoveTable);
            }
            else
            {
                listJobReturn = CheckMaterial.listJobReturnForOneWork(workAvailableTable, deviceBreakingTime, technicianWorkingTime, wareHouseMaterialTable, materialTable, listwareHouseMaterials, listMaterials);
            }

            return listJobReturn;
        }
    }
}
