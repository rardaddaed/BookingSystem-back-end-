using AutoFixture;
using AutoMapper;
using BookingSystem.Domain;
using BookingSystem.Domain.Entities;
using BookingSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace BookingSystem.Application.Test
{
  public class BaseTestFixture
  {
    protected Fixture fixture;
    [SetUp]
    public async Task SetUp()
    {
      fixture = new Fixture();
    }
  }
}