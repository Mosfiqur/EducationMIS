package com.unicef.mis.util;

import android.content.Context;
import androidx.fragment.app.Fragment;

import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;

import java.util.ArrayList;
import java.util.List;

public class Singleton {
    private static Singleton ourInstance = new Singleton();
    private Context context;
    private Fragment whichFragmentItIs;
    private int id, idInstance, offlineStatus, benificiaryId, entityDynamicColumnId;

    private String title, benificiaryName, beneficiaryProgressId, actionId, unhcrId, scheduleName
            ,scheduleDate, endDate, facilityCode;


    private int sex, los;
    private boolean disabled;
    private List<ScheduledInstance> scheduledInstances = new ArrayList<ScheduledInstance>();

    private List <String> values;

    private List<Schedule> scheduleList;

    private int intValueForTest;
    private int entityType, facilityId, beneficiaryId;
    private int networkStatus;

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public String getDataColectionDate() {
        return dataColectionDate;
    }

    public void setDataColectionDate(String dataColectionDate) {
        this.dataColectionDate = dataColectionDate;
    }

    private int status;

    private Integer CampId, BlockId, SubBlock;

    private String  facilityName, campName, programmingPartnerName, ImplementationPartnerName, dataColectionDate, blockName, subBlockName;


    private Singleton() {
    }

    public static Singleton getInstance() {
        return ourInstance;
    }

    public Context getContext() {
        if (context == null) {
            throw new RuntimeException("Context is null in Singleton singleton. Check if setContext() is called properly.");
        }
        return context;
    }

    public void setContext(Context context) {
        this.context = context.getApplicationContext();
    }



    public void setWhichFragmentItIs(Fragment whichFragmentItIs) {
        this.whichFragmentItIs = whichFragmentItIs;
    }

    public Fragment getWhichFragmentItIs() {
        return whichFragmentItIs;
    }

    public static Singleton getOurInstance() {
        return ourInstance;
    }

    public static void setOurInstance(Singleton ourInstance) {
        Singleton.ourInstance = ourInstance;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getIdInstance() {
        return idInstance;
    }

    public void setIdInstance(int idInstance) {
        this.idInstance = idInstance;
    }

    public List<ScheduledInstance> getScheduledInstances() {
        return scheduledInstances;
    }

    public void setScheduledInstances(List<ScheduledInstance> scheduledInstances) {
        this.scheduledInstances = scheduledInstances;
    }

    public int getBenificiaryId() {
        return benificiaryId;
    }

    public void setBenificiaryId(int benificiaryId) {
        this.benificiaryId = benificiaryId;
    }

    public String getBenificiaryName() {
        return benificiaryName;
    }

    public void setBenificiaryName(String benificiaryName) {
        this.benificiaryName = benificiaryName;
    }

    public int getEntityDynamicColumnId() {
        return entityDynamicColumnId;
    }

    public void setEntityDynamicColumnId(int entityDynamicColumnId) {
        this.entityDynamicColumnId = entityDynamicColumnId;
    }

    public List<String> getValues() {
        return values;
    }

    public void setValues(List<String> values) {
        this.values = values;
    }

    public int getOfflineStatus() {
        return offlineStatus;
    }

    public void setOfflineStatus(int offlineStatus) {
        this.offlineStatus = offlineStatus;
    }

    public int getIntValueForTest() {
        return intValueForTest;
    }

    public void setIntValueForTest(int intValueForTest) {
        this.intValueForTest = intValueForTest;
    }

    public String getBeneficiaryProgressId() {
        return beneficiaryProgressId;
    }

    public void setBeneficiaryProgressId(String beneficiaryProgressId) {
        this.beneficiaryProgressId = beneficiaryProgressId;
    }

    public String getActionId() {
        return actionId;
    }

    public void setActionId(String actionId) {
        this.actionId = actionId;
    }

    public List<Schedule> getScheduleList() {
        return scheduleList;
    }

    public void setScheduleList(List<Schedule> scheduleList) {
        this.scheduleList = scheduleList;
    }

    public int getEntityType() {
        return entityType;
    }

    public void setEntityType(int entityType) {
        this.entityType = entityType;
    }

    public int getNetworkStatus() {
        return networkStatus;
    }

    public void setNetworkStatus(int networkStatus) {
        this.networkStatus = networkStatus;
    }

    public int getFacilityId() {
        return facilityId;
    }

    public void setFacilityId(int facilityId) {
        this.facilityId = facilityId;
    }

    public String getFacilityName() {
        return facilityName;
    }

    public void setFacilityName(String facilityName) {
        this.facilityName = facilityName;
    }

    public String getCampName() {
        return campName;
    }

    public void setCampName(String campName) {
        this.campName = campName;
    }

    public String getProgrammingPartnerName() {
        return programmingPartnerName;
    }

    public void setProgrammingPartnerName(String programmingPartnerName) {
        this.programmingPartnerName = programmingPartnerName;
    }

    public String getImplementationPartnerName() {
        return ImplementationPartnerName;
    }

    public void setImplementationPartnerName(String implementationPartnerName) {
        ImplementationPartnerName = implementationPartnerName;
    }

    public String getUnhcrId() {
        return unhcrId;
    }

    public void setUnhcrId(String unhcrId) {
        this.unhcrId = unhcrId;
    }

    public int getBeneficiaryId() {
        return beneficiaryId;
    }

    public void setBeneficiaryId(int beneficiaryId) {
        this.beneficiaryId = beneficiaryId;
    }

    public Integer getCampId() {
        return CampId;
    }

    public void setCampId(Integer campId) {
        CampId = campId;
    }

    public Integer getBlockId() {
        return BlockId;
    }

    public void setBlockId(Integer blockId) {
        BlockId = blockId;
    }

    public Integer getSubBlock() {
        return SubBlock;
    }

    public void setSubBlock(Integer subBlock) {
        SubBlock = subBlock;
    }

    public String getScheduleName() {
        return scheduleName;
    }

    public void setScheduleName(String scheduleName) {
        this.scheduleName = scheduleName;
    }

    public String getScheduleDate() {
        return scheduleDate;
    }

    public void setScheduleDate(String scheduleDate) {
        this.scheduleDate = scheduleDate;
    }

    public String getEndDate() {
        return endDate;
    }

    public void setEndDate(String endDate) {
        this.endDate = endDate;
    }

    public String getBlockName() {
        return blockName;
    }

    public void setBlockName(String blockName) {
        this.blockName = blockName;
    }

    public String getSubBlockName() {
        return subBlockName;
    }

    public void setSubBlockName(String subBlockName) {
        this.subBlockName = subBlockName;
    }

    public String getFacilityCode() {
        return facilityCode;
    }

    public void setFacilityCode(String facilityCode) {
        this.facilityCode = facilityCode;
    }

    public int getSex() {
        return sex;
    }

    public void setSex(int sex) {
        this.sex = sex;
    }

    public int getLos() {
        return los;
    }

    public void setLos(int los) {
        this.los = los;
    }

    public boolean isDisabled() {
        return disabled;
    }

    public void setDisabled(boolean disabled) {
        this.disabled = disabled;
    }
}
