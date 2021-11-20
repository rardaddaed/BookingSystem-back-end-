using System;
using System.Linq.Expressions;
using BookingSystem.Domain.Entities;
using BookingSystem.Domain.Models;

namespace BookingSystem.Domain.Extensions
{
    public static partial class BookingLevelMapper
    {
        public static BookingLevelDto AdaptToDto(this BookingLevel p1)
        {
            return p1 == null ? null : new BookingLevelDto()
            {
                BookingLevelId = p1.BookingLevelId,
                Name = p1.Name,
                Alias = p1.Alias,
                BlueprintUrl = p1.BlueprintUrl,
                BlueprintWidth = p1.BlueprintWidth,
                BlueprintHeight = p1.BlueprintHeight,
                MaxBooking = p1.MaxBooking,
                Locked = p1.Locked,
                CreatedUtc = p1.CreatedUtc,
                ModifiedUtc = p1.ModifiedUtc
            };
        }
        public static BookingLevelDto AdaptTo(this BookingLevel p2, BookingLevelDto p3)
        {
            if (p2 == null)
            {
                return null;
            }
            BookingLevelDto result = p3 ?? new BookingLevelDto();
            
            result.BookingLevelId = p2.BookingLevelId;
            result.Name = p2.Name;
            result.Alias = p2.Alias;
            result.BlueprintUrl = p2.BlueprintUrl;
            result.BlueprintWidth = p2.BlueprintWidth;
            result.BlueprintHeight = p2.BlueprintHeight;
            result.MaxBooking = p2.MaxBooking;
            result.Locked = p2.Locked;
            result.CreatedUtc = p2.CreatedUtc;
            result.ModifiedUtc = p2.ModifiedUtc;
            return result;
            
        }
        public static Expression<Func<BookingLevel, BookingLevelDto>> ProjectToDto => p4 => new BookingLevelDto()
        {
            BookingLevelId = p4.BookingLevelId,
            Name = p4.Name,
            Alias = p4.Alias,
            BlueprintUrl = p4.BlueprintUrl,
            BlueprintWidth = p4.BlueprintWidth,
            BlueprintHeight = p4.BlueprintHeight,
            MaxBooking = p4.MaxBooking,
            Locked = p4.Locked,
            CreatedUtc = p4.CreatedUtc,
            ModifiedUtc = p4.ModifiedUtc
        };
    }
}