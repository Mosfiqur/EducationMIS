export class ApiConstant {
    static baseUrl: string = "https://phantomasp.kaz.com.bd/api/"; 
    //static baseUrl: string = "https://localhost:44395/api/"; 
    //auth
    static Login: string = ApiConstant.baseUrl + 'Auth/Login';

    //Dynamic Properties
    static GetAllColumn: string = ApiConstant.baseUrl + 'DynamicProperties/GetAllColumn';
    static GetColumnForIndicator: string = ApiConstant.baseUrl + 'DynamicProperties/GetColumnForIndicator';
    static SaveDynamicColumn: string = ApiConstant.baseUrl + 'DynamicProperties/SaveDynamicColumn';
    static SaveBeneficiaryCell: string = ApiConstant.baseUrl + 'DynamicProperties/SaveBeneficiaryCell';
    static SaveFacilityCell: string = ApiConstant.baseUrl + 'DynamicProperties/SaveFacilityCell';

    //Grid view
    static SaveGridView: string = ApiConstant.baseUrl + 'GridView/Add';
    static UpdateGridView: string = ApiConstant.baseUrl + 'GridView/Update';
    static GetGridView: string = ApiConstant.baseUrl + 'GridView/GetAll';
    static GetGridById: string = ApiConstant.baseUrl + 'GridView/GetById';
    static AddColumnToGrid: string = ApiConstant.baseUrl + 'GridView/AddColumnToView';

    //Instance
    static GetCompleteInstance: string = ApiConstant.baseUrl + 'Schedule/GetCompletedInstances';
    static GetRunningInstance: string = ApiConstant.baseUrl + 'Schedule/GetRunningInstances';
    static GetNotPendingInstances: string = ApiConstant.baseUrl + 'Schedule/GetNotPendingInstances'

    //Indicator
    static GetIndicatorByEntityType: string = ApiConstant.baseUrl + 'Indicator/GetIndicatorsByEntityType';
    static GetIndicatorsByInstance: string = ApiConstant.baseUrl + 'Indicator/GetIndicatorsByInstance';
    static SaveIndicator: string = ApiConstant.baseUrl + 'Indicator/Add';
    static DeleteIndicator: string = ApiConstant.baseUrl + 'Indicator/Delete';
    static GetFacilityIndicator: string = ApiConstant.baseUrl + 'Indicator/GetFacilityIndicator';
    static GetBeneficiaryIndicator: string = ApiConstant.baseUrl + 'Indicator/GetBeneficiaryIndicator';

    //List Data type
    static GetAllListDataType: string = ApiConstant.baseUrl + "ListDataType/GetAll";
    static GetByIdListDataType: string = ApiConstant.baseUrl + "ListDataType/GetById";
    static AddListDataType: string = ApiConstant.baseUrl + "ListDataType/add";
    static UpdateListDataType: string = ApiConstant.baseUrl + "ListDataType/update";


    //Beneficiary
    static SaveBeneficiary: string = ApiConstant.baseUrl + 'Beneficiary/Add';
    static UpdateBeneficiary: string = ApiConstant.baseUrl + 'Beneficiary/Update';
    static GetAllBeneficiary: string = ApiConstant.baseUrl + 'Beneficiary/GetAll';
    static GetByFacilityId:string = ApiConstant.baseUrl + 'Beneficiary/GetByFacilityId';
    static GetBeneficiaryByViewId: string = ApiConstant.baseUrl + 'Beneficiary/GetAllByViewId';
    static GetBeneficiaryById: string = ApiConstant.baseUrl + 'Beneficiary/GetById';
    static DeleteBeneficiary: string = ApiConstant.baseUrl + 'Beneficiary/Delete';
    static DeactivateBeneficiary:string = ApiConstant.baseUrl + 'Beneficiary/DeactivateBeneficiary';
    static ImportBeneficiaries: string = ApiConstant.baseUrl + 'Beneficiary/ImportBeneficiaries';
    static ImportBeneficiariesVersionData: string = ApiConstant.baseUrl + 'Beneficiary/ImportBeneficiariesVersionData';

    //Facility
    static GetAllForDevice:string = ApiConstant.baseUrl + 'Facility/GetAllForDevice';
    static GetAllFacilityObject: string = ApiConstant.baseUrl + 'Facility/GetAllByBeneficiaryInstance';
    static SaveFacility: string = ApiConstant.baseUrl + 'Facility/Add';
    static UpdateFacility: string = ApiConstant.baseUrl + 'Facility/Update';
    static GetFacilityByViewId: string = ApiConstant.baseUrl + 'Facility/GetAllByViewId';
    static GetFacilityById: string = ApiConstant.baseUrl + 'Facility/GetById';
    static DeleteFacility: string = ApiConstant.baseUrl + 'Facility/Delete';
    static AssignTeacher: string = ApiConstant.baseUrl + 'Facility/AssignTeacher';
    static GetTeacher: string = ApiConstant.baseUrl + 'User/GetTeachers';
    static ImportFacilities: string = ApiConstant.baseUrl + 'Facility/ImportFacilities';
    static ImportFacilityVersionData: string = ApiConstant.baseUrl + 'Facility/ImportFacilitiesVersionData';


    //Approval
    static GetSubmittedBeneficiaries: string = ApiConstant.baseUrl + 'DataApproval/GetSubmittedBeneficiaries';
    static GetSubmittedFacilities: string = ApiConstant.baseUrl + 'DataApproval/GetSubmittedFacilities';
    static ApproveBeneficiaries: string = ApiConstant.baseUrl + 'DataApproval/ApproveBeneficiaries';
    static ApproveFacilities: string = ApiConstant.baseUrl + 'DataApproval/ApproveFacilities';
    static RecollectBeneficiaries: string = ApiConstant.baseUrl + 'DataApproval/RecollectBeneficiaries';
    static RecollectFacility: string = ApiConstant.baseUrl + 'DataApproval/RecollectFacilities';

    //Education Sector Partner
    static EducationSectorPartner: string = ApiConstant.baseUrl + 'EducationSectorPartner/GetAllEsp';

    //Common
    static GetCamps: string = ApiConstant.baseUrl + 'Common/GetCamps';
    static GetBlocks: string = ApiConstant.baseUrl + 'Common/GetBlocks';
    static GetSubBlocks: string = ApiConstant.baseUrl + 'Common/GetSubBlocks';
    static GetUnions: string = ApiConstant.baseUrl + 'Common/GetUnions';
    static GetUpazilas: string = ApiConstant.baseUrl + 'Common/GetUpazilas';
    static GetCampWithBlockSubBlock: string = ApiConstant.baseUrl + 'Common/GetCampWithBlockSubBlock';

    //Reporting
    static Generate5WReport: string = ApiConstant.baseUrl + 'Report/Generate5WReport';


    static createUser: string = ApiConstant.baseUrl + "User/CreateUser";
    static updateUser: string = ApiConstant.baseUrl + "User/UpdateUser";
    static getUsers: string = ApiConstant.baseUrl + "User/GetUsers";
    static getUserById: string = ApiConstant.baseUrl + "User/GetUserById";
    static resetPassword: string = ApiConstant.baseUrl + "User/ResetPassword";
    static getDesignations: string = ApiConstant.baseUrl + "User/GetDesignations";
    static deleteUser: string = ApiConstant.baseUrl + "User/DeleteUser";

    static getEspList: string = ApiConstant.baseUrl + "EducationSectorPartner/GetAllEsp";


    static createRole: string = ApiConstant.baseUrl + "Role/CreateRole";
    static updateRole: string = ApiConstant.baseUrl + "Role/UpdateRole";
    static getRoles: string = ApiConstant.baseUrl + "Role/GetRoles";
    static getRoleById: string = ApiConstant.baseUrl + "Role/GetRoleById";
    static deleteRole: string = ApiConstant.baseUrl + "Role/DeleteRole";
    static getLevels: string = ApiConstant.baseUrl + "Role/GetLevels";

    static getPermissionPersets: string = ApiConstant.baseUrl + "Permission/GetPermissionPresets";
    static getPermissionsByPersetId: string = ApiConstant.baseUrl + "Permission/GetPermissionsByPresetId";
    static getPermissions: string = ApiConstant.baseUrl + "Permission/GetPermissions";

    static getAllMFs: string = ApiConstant.baseUrl + "MonitoringFramework/GetAll";
    static createMF: string = ApiConstant.baseUrl + "MonitoringFramework/Add";
    static updateMF: string = ApiConstant.baseUrl + "MonitoringFramework/Update";
    static insertMFDynamicColumnCell: string = ApiConstant.baseUrl + "MonitoringFramework/InsertDynamicCell";
    static updateMFDynamicColumnCell: string = ApiConstant.baseUrl + "MonitoringFramework/UpdateDynamicCell";
    static deleteMFDynamicColumnCell: string = ApiConstant.baseUrl + "MonitoringFramework/DeleteDynamicCell";

    //notification
    static getAllNotification: string = ApiConstant.baseUrl + "Notificaiton/GetAll";
    static readNotifiaction: string = ApiConstant.baseUrl + "Notificaiton/ReadNotification";
    static clearNotification: string = ApiConstant.baseUrl + "Notificaiton/ClearNotification";

}