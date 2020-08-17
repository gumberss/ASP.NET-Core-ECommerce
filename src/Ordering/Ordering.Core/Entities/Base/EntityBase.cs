﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Core.Entities.Base
{
    public abstract class EntityBase<TId> : IEntityBase<TId>
    {
        public virtual TId Id { get; protected set; }

        int? _requestHashCode;

        public bool IsTransient()
        {
            return Id.Equals(default(TId));
        }

        public override bool Equals(object obj)
        {
            if (obj is null || !(obj is EntityBase<TId>))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (EntityBase<TId>)obj;

            if (item.IsTransient() || IsTransient())
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestHashCode.HasValue)
                    _requestHashCode = Id.GetHashCode() ^ 31;

                return _requestHashCode.Value;
            }

            return base.GetHashCode();
        }

        public static bool operator ==(EntityBase<TId> left, EntityBase<TId> right)
        {
            if (Equals(left, null))
                return Equals(right, null);

            return left.Equals(right);
        }

        public static bool operator !=(EntityBase<TId> left, EntityBase<TId> right)
        {
            return !(left == right);
        }
    }
}
