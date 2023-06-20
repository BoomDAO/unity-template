using TokenIndex = System.UInt32;
using TokenIdentifier = System.String;
using AccountIdentifier = System.String;
using EdjCase.ICP.Candid.Mapping;
using Candid.WorldHub.Models;
using System;
using EdjCase.ICP.Candid.Models;

namespace Candid.WorldHub.Models
{
	[Variant(typeof(EntityPermissionTag))]
	public class EntityPermission
	{
		[VariantTagProperty()]
		public EntityPermissionTag Tag { get; set; }

		[VariantValueProperty()]
		public System.Object? Value { get; set; }

		public EntityPermission(EntityPermissionTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected EntityPermission()
		{
		}

		public static EntityPermission ReceiveQuantityCap(EntityPermission.ReceiveQuantityCapInfo info)
		{
			return new EntityPermission(EntityPermissionTag.ReceiveQuantityCap, info);
		}

		public static EntityPermission ReduceExpirationCap(EntityPermission.ReduceExpirationCapInfo info)
		{
			return new EntityPermission(EntityPermissionTag.ReduceExpirationCap, info);
		}

		public static EntityPermission RenewExpirationCap(EntityPermission.RenewExpirationCapInfo info)
		{
			return new EntityPermission(EntityPermissionTag.RenewExpirationCap, info);
		}

		public static EntityPermission SpendQuantityCap(EntityPermission.SpendQuantityCapInfo info)
		{
			return new EntityPermission(EntityPermissionTag.SpendQuantityCap, info);
		}

		public EntityPermission.ReceiveQuantityCapInfo AsReceiveQuantityCap()
		{
			this.ValidateTag(EntityPermissionTag.ReceiveQuantityCap);
			return (EntityPermission.ReceiveQuantityCapInfo)this.Value!;
		}

		public EntityPermission.ReduceExpirationCapInfo AsReduceExpirationCap()
		{
			this.ValidateTag(EntityPermissionTag.ReduceExpirationCap);
			return (EntityPermission.ReduceExpirationCapInfo)this.Value!;
		}

		public EntityPermission.RenewExpirationCapInfo AsRenewExpirationCap()
		{
			this.ValidateTag(EntityPermissionTag.RenewExpirationCap);
			return (EntityPermission.RenewExpirationCapInfo)this.Value!;
		}

		public EntityPermission.SpendQuantityCapInfo AsSpendQuantityCap()
		{
			this.ValidateTag(EntityPermissionTag.SpendQuantityCap);
			return (EntityPermission.SpendQuantityCapInfo)this.Value!;
		}

		private void ValidateTag(EntityPermissionTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}

		public class ReceiveQuantityCapInfo
		{
			[CandidName("capPerInterval")]
			public double CapPerInterval { get; set; }

			[CandidName("intervalDuration")]
			public UnboundedUInt IntervalDuration { get; set; }

			public ReceiveQuantityCapInfo(double capPerInterval, UnboundedUInt intervalDuration)
			{
				this.CapPerInterval = capPerInterval;
				this.IntervalDuration = intervalDuration;
			}

			public ReceiveQuantityCapInfo()
			{
			}
		}

		public class ReduceExpirationCapInfo
		{
			[CandidName("capPerInterval")]
			public UnboundedUInt CapPerInterval { get; set; }

			[CandidName("intervalDuration")]
			public UnboundedUInt IntervalDuration { get; set; }

			public ReduceExpirationCapInfo(UnboundedUInt capPerInterval, UnboundedUInt intervalDuration)
			{
				this.CapPerInterval = capPerInterval;
				this.IntervalDuration = intervalDuration;
			}

			public ReduceExpirationCapInfo()
			{
			}
		}

		public class RenewExpirationCapInfo
		{
			[CandidName("capPerInterval")]
			public UnboundedUInt CapPerInterval { get; set; }

			[CandidName("intervalDuration")]
			public UnboundedUInt IntervalDuration { get; set; }

			public RenewExpirationCapInfo(UnboundedUInt capPerInterval, UnboundedUInt intervalDuration)
			{
				this.CapPerInterval = capPerInterval;
				this.IntervalDuration = intervalDuration;
			}

			public RenewExpirationCapInfo()
			{
			}
		}

		public class SpendQuantityCapInfo
		{
			[CandidName("capPerInterval")]
			public double CapPerInterval { get; set; }

			[CandidName("intervalDuration")]
			public UnboundedUInt IntervalDuration { get; set; }

			public SpendQuantityCapInfo(double capPerInterval, UnboundedUInt intervalDuration)
			{
				this.CapPerInterval = capPerInterval;
				this.IntervalDuration = intervalDuration;
			}

			public SpendQuantityCapInfo()
			{
			}
		}
	}

	public enum EntityPermissionTag
	{
		[CandidName("receiveQuantityCap")]
		[VariantOptionType(typeof(EntityPermission.ReceiveQuantityCapInfo))]
		ReceiveQuantityCap,
		[CandidName("reduceExpirationCap")]
		[VariantOptionType(typeof(EntityPermission.ReduceExpirationCapInfo))]
		ReduceExpirationCap,
		[CandidName("renewExpirationCap")]
		[VariantOptionType(typeof(EntityPermission.RenewExpirationCapInfo))]
		RenewExpirationCap,
		[CandidName("spendQuantityCap")]
		[VariantOptionType(typeof(EntityPermission.SpendQuantityCapInfo))]
		SpendQuantityCap
	}
}