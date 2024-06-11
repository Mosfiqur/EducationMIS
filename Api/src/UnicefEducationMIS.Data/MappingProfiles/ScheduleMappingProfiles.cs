using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Data.MappingProfiles
{
    public class ScheduleMappingProfiles : Profile
    {
        public ScheduleMappingProfiles()
        {
            CreateMap<ScheduleViewModel, Schedule>()
                .ForMember(schedule => schedule.Status, cfg => cfg.MapFrom(vm => vm.Status == 0 ? ScheduleStatus.Pending: vm.Status))                
                .ReverseMap();

            CreateMap<InstanceViewModel, Instance>()
                .ForMember(instance => instance.Status, cfg => cfg.MapFrom(vm => vm.Status == 0 ? InstanceStatus.Pending : vm.Status))
                .ReverseMap()
                .ForMember(x => x.ScheduleName, cfg => cfg.MapFrom(s => s.Schedule.ScheduleName))
                .ForMember(x=> x.StartDate, cfg => cfg.MapFrom(s => s.GetStartDate()))
                .ForMember(x=> x.EndDate, cfg => cfg.MapFrom(s => s.GetEndDate()))
                ;
            CreateMap<FrequencyViewModel, Frequency>().ReverseMap();

            CreateMap<DateTime, InstanceViewModel>()
                .ForMember(x => x.DataCollectionDate, cfg => cfg.MapFrom(date => date));
        }
    }
}
