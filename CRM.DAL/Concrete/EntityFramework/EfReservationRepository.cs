﻿using CRM.DAL.Abstract;
using CRM.DAL.Concrete.EntityFramework.DataManagement;
using CRM.Entity.Poco;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Concrete.EntityFramework
{
    public class EfReservationRepository : EfRepository<Reservation>, IReservationRepository
    {
        public EfReservationRepository(DbContext context) : base(context)
        {
        }
    }
}
