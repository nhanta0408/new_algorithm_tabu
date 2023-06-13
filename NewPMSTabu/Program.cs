using NewPMSTabu.Functions;
using NewPMSTabu.Model;

var firstDateStart = FirstDateStart.firstDateStart;

List<Work> works = new List<Work>();
List<Device> devices = new List<Device>();
List<Technician> technicians = new List<Technician>();
List<WarehouseMaterial> wareHouseMaterials = new List<WarehouseMaterial>();
List<Material> materials = new List<Material>();

List<Work> listWorkEnoughMaterial = CheckMaterial.checkWorkEnoughMaterial(wareHouseMaterials, works);
List<Work> listWorkEnoughTimeOnDevice = CheckMaterial.returnDevicesNotEnoughTime(listWorkEnoughMaterial, devices);

List<JobInfor> listJobAvailable = new List<JobInfor>();
ListJobInforReturn listJobReturn = new ListJobInforReturn(scheduled: new List<JobInfor>(),
                                                          rejected: new List<JobInfor>());
listJobAvailable = TabuSearch.tabuSearch(listWorkEnoughTimeOnDevice, devices, technicians, wareHouseMaterials);
//if (listWorkEnoughTimeOnDevice.Count > 1)
//{
//    listJobAvailable = TabuSearch.tabuSearch(listWorkEnoughTimeOnDevice, devices, technicians, wareHouseMaterials);

//    //listJobReturn = CheckMaterial.listJobReturn(listWorkObjects, listJobAvailable, listDeviceNotEnoughTimes,
//    //                                            deviceBreakingTime, technicianWorkingTime, wareHouseMaterialTable,
//    //                                            materialTable, listwareHouseMaterials, listMaterials, workRemoveTable);
//}
//else
//{
//    listJobReturn = CheckMaterial.listJobReturnForOneWork(workAvailableTable, deviceBreakingTime, technicianWorkingTime, wareHouseMaterialTable, materialTable, listwareHouseMaterials, listMaterials);
//}