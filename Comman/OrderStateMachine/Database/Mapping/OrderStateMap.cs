﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderStateMachine.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStateMachine.Database.Mapping
{
    public class OrderStateMap : SagaClassMap<OrderState>
    {
        protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(100);
            entity.Property(x => x.PaymentId).HasMaxLength(100);
            entity.Property(x => x.Products).HasMaxLength(100);
            entity.Property(x => x.CartId).HasMaxLength(100);
        }
    }
}
