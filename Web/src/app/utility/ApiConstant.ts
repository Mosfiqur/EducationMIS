import { environment } from 'src/environments/environment.prod';

export class ApiConstant {
    static baseUrl: string = environment.apiUrl;
    //auth
    static Login: string = ApiConstant.baseUrl + 'Auth/Login';

    //Dynamic Properties
    // static GetById: string = ApiConstant.baseUrl + 'DynamicProperties/GetById';    
    static GetBeneficiaryDynamicColumnById: string = ApiConstant.baseUrl + 'DynamicProperties/GetBeneficiaryDynamicColumnById';
    static GetFacilityDynamicColumnById: string = ApiConstant.baseUrl + 'DynamicProperties/GetFacilityDynamicColumnById';
    static GetMonitoringDynamicColumnById: string = ApiConstant.baseUrl + 'DynamicProperties/GetMonitoringDynamicColumnById';
    static GetBudgetDynamicColumnById: string = ApiConstant.baseUrl + 'DynamicProperties/GetBudgetDynamicColumnById';
    static GetTargetDynamicColumnById: string = ApiConstant.baseUrl + 'DynamicProperties/GetTargetDynamicColumnById';
    static GetUserDynamicColumnById: string = ApiConstant.baseUrl + 'DynamicProperties/GetUserDynamicColumnById';



    //static GetAllColumn: string = ApiConstant.baseUrl + 'DynamicProperties/GetAllColumn';
    static GetAllBeneficiaryColumn: string = ApiConstant.baseUrl + 'DynamicProperties/GetAllBeneficiaryColumn';
    static GetAllFacilityColumn: string = ApiConstant.baseUrl + 'DynamicProperties/GetAllFacilityColumn';
    static GetAllMonitoringColumn: string = ApiConstant.baseUrl + 'DynamicProperties/GetAllMonitoringColumn';
    static GetAllBudgetColumn: string = ApiConstant.baseUrl + 'DynamicProperties/GetAllBudgetColumn';
    static GetAllTargetColumn: string = ApiConstant.baseUrl + 'DynamicProperties/GetAllTargetColumn';
    static GetAllUserColumn: string = ApiConstant.baseUrl + 'DynamicProperties/GetAllUserColumn';

    // static deleteDynamicColumn:string = ApiConstant.baseUrl + 'DynamicProperties/DeleteDynamicColumn';
    static DeleteBudgetDynamicColumn:string = ApiConstant.baseUrl + 'DynamicProperties/DeleteBudgetDynamicColumn';
    static DeleteTargetDynamicColumn:string = ApiConstant.baseUrl + 'DynamicProperties/DeleteTargetDynamicColumn';
    static DeleteUserDynamicColumn:string = ApiConstant.baseUrl + 'DynamicProperties/DeleteUserDynamicColumn';

    static GetNumericColumn: string = ApiConstant.baseUrl + 'DynamicProperties/GetNumericColumn';
    // static GetColumnForIndicator: string = ApiConstant.baseUrl + 'DynamicProperties/GetColumnForIndicator';
    static GetBeneficiaryColumnForIndicator: string = ApiConstant.baseUrl + 'DynamicProperties/GetBeneficiaryColumnForIndicator';
    static GetFacilityColumnForIndicator: string = ApiConstant.baseUrl + 'DynamicProperties/GetFacilityColumnForIndicator';


    // static SaveDynamicColumn: string = ApiConstant.baseUrl + 'DynamicProperties/SaveDynamicColumn';
    static SaveBeneficiaryDynamicColumn: string = ApiConstant.baseUrl + 'DynamicProperties/SaveBeneficiaryDynamicColumn';
    static SaveFacilityDynamicColumn: string = ApiConstant.baseUrl + 'DynamicProperties/SaveFacilityDynamicColumn';
    static SaveMonitoringDynamicColumn: string = ApiConstant.baseUrl + 'DynamicProperties/SaveMonitoringDynamicColumn';
    static SaveBudgetDynamicColumn: string = ApiConstant.baseUrl + 'DynamicProperties/SaveBudgetDynamicColumn';
    static SaveTargetDynamicColumn: string = ApiConstant.baseUrl + 'DynamicProperties/SaveTargetDynamicColumn';
    static SaveUserDynamicColumn: string = ApiConstant.baseUrl + 'DynamicProperties/SaveUserDynamicColumn';

    static SaveBeneficiaryCell: string = ApiConstant.baseUrl + 'DynamicProperties/SaveBeneficiaryCell';
    static SaveFacilityCell: string = ApiConstant.baseUrl + 'DynamicProperties/SaveFacilityCell';
    static DeleteFacilityCell: string = ApiConstant.baseUrl + 'DynamicProperties/DeleteFacilityCell';

    //Grid view
    // static SaveGridView: string = ApiConstant.baseUrl + 'GridView/Add';
    static SaveBeneficiaryGridView: string = ApiConstant.baseUrl + 'GridView/AddBeneficiaryGridView';
    static SaveFacilityGridView: string = ApiConstant.baseUrl + 'GridView/AddFacilityGridView';

    // static UpdateGridView: string = ApiConstant.baseUrl + 'GridView/Update';
    static UpdateBeneficiaryGridView: string = ApiConstant.baseUrl + 'GridView/UpdateBeneficiaryGridView';
    static UpdateFacilityGridView: string = ApiConstant.baseUrl + 'GridView/UpdateFacilityGridView';

    // static GetGridView: string = ApiConstant.baseUrl + 'GridView/GetAll';
    static GetBeneficiaryGridView: string = ApiConstant.baseUrl + 'GridView/GetAllGridViewBeneficiary';
    static GetFacilityGridView: string = ApiConstant.baseUrl + 'GridView/GetAllGridViewFacility';

    // static GetGridById: string = ApiConstant.baseUrl + 'GridView/GetById';    
    static GetByIdBeneficiary: string = ApiConstant.baseUrl + 'GridView/GetByIdBeneficiary';
    static GetByIdFacility: string = ApiConstant.baseUrl + 'GridView/GetByIdFacility';

    // static AddColumnToGrid: string = ApiConstant.baseUrl + 'GridView/AddColumnToView';
    static AddBeneficiaryColumnToView: string = ApiConstant.baseUrl + 'GridView/AddBeneficiaryColumnToView';
    static AddFacilityColumnToView: string = ApiConstant.baseUrl + 'GridView/AddFacilityColumnToView';

    // static deleteColumnToViewGrid:string = ApiConstant.baseUrl + 'GridView/DeleteColumnToView';
    static DeleteBeneficiaryColumnToView:string = ApiConstant.baseUrl + 'GridView/DeleteBeneficiaryColumnToView';
    static DeleteFacilityColumnToView:string = ApiConstant.baseUrl + 'GridView/DeleteFacilityColumnToView';


    //Instance
    static GetCompleteInstance: string = ApiConstant.baseUrl + 'Schedule/GetCompletedInstances';
    static GetRunningInstance: string = ApiConstant.baseUrl + 'Schedule/GetRunningInstances';
    //  static GetNotPendingInstances: string = ApiConstant.baseUrl + 'Schedule/GetNotPendingInstances';    
    static GetBeneficiaryNotPendingInstances: string = ApiConstant.baseUrl + 'Schedule/GetBeneficiaryNotPendingInstances';
    static GetFacilityNotPendingInstances: string = ApiConstant.baseUrl + 'Schedule/GetFacilityNotPendingInstances';

    // static GetInstancesStatusWise: string = ApiConstant.baseUrl + 'Schedule/GetInstancesStatusWise';
    static GetBeneficiaryInstancesStatusWise: string = ApiConstant.baseUrl + 'Schedule/GetBeneficiaryInstancesStatusWise';
    static GetFacilityInstancesStatusWise: string = ApiConstant.baseUrl + 'Schedule/GetFacilityInstancesStatusWise';


    //Indicator
    // static GetIndicatorByEntityType: string = ApiConstant.baseUrl + 'Indicator/GetIndicatorsByEntityType';
    // static GetIndicatorsByInstance: string = ApiConstant.baseUrl + 'Indicator/GetIndicatorsByInstance';
    static GetBeneficiaryIndicatorsByInstance: string = ApiConstant.baseUrl + 'Indicator/GetBeneficiaryIndicatorsByInstance';
    static GetFacilityIndicatorsByInstance: string = ApiConstant.baseUrl + 'Indicator/GetFacilityIndicatorsByInstance';

    // static SaveIndicator: string = ApiConstant.baseUrl + 'Indicator/Add';
    static SaveBeneficiaryIndicator: string = ApiConstant.baseUrl + 'Indicator/AddBeneficiaryIndicator';
    static SaveFacilityIndicator: string = ApiConstant.baseUrl + 'Indicator/AddFacilityIndicator';

    //  static UpdateIndicator: string = ApiConstant.baseUrl + 'Indicator/Update';
    static UpdateBeneficiaryIndicator: string = ApiConstant.baseUrl + 'Indicator/UpdateBeneficiaryIndicator';
    static UpdateFacilityIndicator: string = ApiConstant.baseUrl + 'Indicator/UpdateFacilityIndicator';

    // static DeleteIndicator: string = ApiConstant.baseUrl + 'Indicator/Delete';
    static DeleteBeneficiaryIndicator: string = ApiConstant.baseUrl + 'Indicator/DeleteBeneficiaryIndicator';
    static DeleteFacilityIndicator: string = ApiConstant.baseUrl + 'Indicator/DeleteFacilityIndicator';


    //Beneficiary
    static SaveBeneficiary: string = ApiConstant.baseUrl + 'Beneficiary/Add';
    static UpdateBeneficiary: string = ApiConstant.baseUrl + 'Beneficiary/Update';
    static GetAllBeneficiary: string = ApiConstant.baseUrl + 'Beneficiary/GetAll';
    static GetBeneficiaryByViewId: string = ApiConstant.baseUrl + 'Beneficiary/GetAllByViewId';
    static GetAllByInstanceId: string = ApiConstant.baseUrl + 'Beneficiary/GetAllByInstanceId';
    static GetBeneficiaryById: string = ApiConstant.baseUrl + 'Beneficiary/GetById';
    static DeleteBeneficiary: string = ApiConstant.baseUrl + 'Beneficiary/Delete';
    static ActiveBeneficiary: string = ApiConstant.baseUrl + 'Beneficiary/ActivateBeneficiary';
    static DeactiveBeneficiary: string = ApiConstant.baseUrl + 'Beneficiary/DeactivateBeneficiary';
    static ImportBeneficiaries: string = ApiConstant.baseUrl + 'Beneficiary/ImportBeneficiaries';
    static ImportBeneficiariesVersionData: string = ApiConstant.baseUrl + 'Beneficiary/ImportBeneficiariesVersionData';

    //Facility
    static GetAllFacilityObject: string = ApiConstant.baseUrl + 'Facility/GetAll';
    static GetAllFacilityObjectByBeneficiaryInstance: string = ApiConstant.baseUrl + 'Facility/GetAllByBeneficiaryInstance';
    static GetAllFacilityLatest: string = ApiConstant.baseUrl + 'Facility/GetAllLatest';
    static GetAllFilteredDataFacilityObject: string = ApiConstant.baseUrl + 'Facility/GetAllFilteredData';
    static SaveFacility: string = ApiConstant.baseUrl + 'Facility/Add';
    static UpdateFacility: string = ApiConstant.baseUrl + 'Facility/Update';
    static GetFacilityByViewId: string = ApiConstant.baseUrl + 'Facility/GetAllByViewId';
    static GetFacilityByInstanceId: string = ApiConstant.baseUrl + 'Facility/GetAllByInstanceId';
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
    static ApproveInactiveBeneficiaries: string = ApiConstant.baseUrl + 'DataApproval/ApproveInactivateBeneficiaries';
    static ApproveFacilities: string = ApiConstant.baseUrl + 'DataApproval/ApproveFacilities';
    static RecollectBeneficiaries: string = ApiConstant.baseUrl + 'DataApproval/RecollectBeneficiaries';
    static RecollectFacility: string = ApiConstant.baseUrl + 'DataApproval/RecollectFacilities';

    //Education Sector Partner
    static EducationSectorPartner: string = ApiConstant.baseUrl + 'EducationSectorPartner/GetAllEsp';

    //Common
    static GetCapms: string = ApiConstant.baseUrl + 'Common/GetCamps';
    static GetBlocks: string = ApiConstant.baseUrl + 'Common/GetBlocks';
    static GetSubBlocks: string = ApiConstant.baseUrl + 'Common/GetSubBlocks';
    static GetUnions: string = ApiConstant.baseUrl + 'Common/GetUnions';
    static GetUpazilas: string = ApiConstant.baseUrl + 'Common/GetUpazilas';

    //Reporting
    static Generate5WReport: string = ApiConstant.baseUrl + 'Report/Generate5WReport';
    static GenerateCampWiseReport: string = ApiConstant.baseUrl + 'Report/GenerateCampWiseReport';

    static createUser: string = ApiConstant.baseUrl + "User/CreateUser";
    static updateUser: string = ApiConstant.baseUrl + "User/UpdateUser";
    static getUsers: string = ApiConstant.baseUrl + "User/GetUsers";
    static getUserById: string = ApiConstant.baseUrl + "User/GetUserById";    
    static updateProfileInfo: string = ApiConstant.baseUrl + "User/updateProfileInfo";
    static resetPassword: string = ApiConstant.baseUrl + "User/ResetPassword";
    static resetOwnPassword: string = ApiConstant.baseUrl + "Auth/ResetPassword";
    static changePassword: string = ApiConstant.baseUrl + "Auth/ChangePassword";
    static requestPasswordResetMail: string = ApiConstant.baseUrl + "Auth/ForgotPassword";
    static validatePasswordResetToken: string = ApiConstant.baseUrl + "Auth/ValidatePasswordResetToken";
    static getDesignations: string = ApiConstant.baseUrl + "User/GetDesignations";
    static deleteUser: string = ApiConstant.baseUrl + "User/DeleteUser";
    static deleteUsers: string = ApiConstant.baseUrl + "User/DeleteUsers";

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

    //notification
    static getAllNotification: string = ApiConstant.baseUrl + "Notificaiton/GetAll";
    static readNotifiaction: string = ApiConstant.baseUrl + "Notificaiton/ReadNotification";
    static clearNotification: string = ApiConstant.baseUrl + "Notificaiton/ClearNotification";
}