using AutoMapper;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Domain
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<BookingLevel, BookingLevelDto>();
      CreateMap<CreateBookingLevelDto, BookingLevel>();
      CreateMap<UpdateBookingLevelDto, BookingLevel>();
    }
  }
}
