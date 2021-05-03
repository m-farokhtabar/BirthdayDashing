﻿using System;

namespace BirthdayDashing.Domain.Base
{
    public abstract class Entity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        protected void Validate<T>(in string propertyName, T value, Func<T, bool> isValid)
        {
            if (!isValid.Invoke(value))
                throw new Exception($"{propertyName} is not valid");
        }
    }
}
