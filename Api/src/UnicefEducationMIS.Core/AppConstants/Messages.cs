using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UnicefEducationMIS.Core.AppConstants
{
    public class Messages
    {
        public const string NoInstanceIndicatorFound = "Selected instance has no indicator";
        public const string MissingIndicator = "The following indicator is not present: {0}";
        public const string RecordNotFound = "The record you are trying to update is not found.";

        public const string MultiValueNotSupportedByColumn = "Multiple value is not supported by column {0}";
        public const string TokenExpired = "Token expired.";
        public const string InvalidInstanceId = "Invalid instance is selected";

        public const string InvalidListTypeDataException = "List type data value must be integer.";
        public const string ListAlreadyUsedException = "List already used.";

        public const string EmailAlreadyTaken = "The email {0} is already in use.";
        public const string CampMandatory = "Camp is mandatory.";
        public const string TargetPopulationInvalid = "Target population invalid";
        public const string CoordindateIsOutsideOfCamp = "The coordindate is outside of the camp's boundary";
        public const string InstanceAlreadyRunning = "An Instance is already running";
        public const string InstanceNotRunning = "Instance is not running.";
        public const string MultipleStatusSelected = "Selected facilities has multiple status";
        public const string CannotApproveWhichNotCollected= "Either not collected or approved already";
        public const string CannotRecollectWhichNotCollected = "Either not collected or approved already";
        public const string ItemIsNonEditable = "This item is not editable";
        public const string CompletedInstanceDataNotEditable = "Completed instance's data is not editable";
        

        public static string InvalidCampBlockStatusOrType =
            "Either of these is invalid: Camp, Block, Facility Status, Facility Type.";

        public static string UnionNotInUpazilla = "Union does not belong to Upazilla";
        public static string CampNotInUnion = "Camp does not belong to Union";
        public const string BlockNotInCamp = "Block does not belong to camp";
        public const string SubBlockNotInBlock = "Sub block does not belong to block";

        public const string ColumnNameAlreadyExists = "Column name already exits";
    }
}
