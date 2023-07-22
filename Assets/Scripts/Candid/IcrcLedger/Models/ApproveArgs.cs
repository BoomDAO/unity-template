using TxIndex__2 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TxIndex__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using TxIndex = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Timestamp = System.UInt64;
using Subaccount__1 = System.Collections.Generic.List<System.Byte>;
using Subaccount = System.Collections.Generic.List<System.Byte>;
using QueryArchiveFn = EdjCase.ICP.Candid.Models.Values.CandidFunc;
using Memo = System.Collections.Generic.List<System.Byte>;
using Balance__2 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance__1 = EdjCase.ICP.Candid.Models.UnboundedUInt;
using Balance = EdjCase.ICP.Candid.Models.UnboundedUInt;
using EdjCase.ICP.Candid.Mapping;
using EdjCase.ICP.Candid.Models;
using Candid.IcrcLedger.Models;

namespace Candid.IcrcLedger.Models
{
	public class ApproveArgs
	{
		[CandidName("amount")]
		public Balance__1 Amount { get; set; }

		[CandidName("created_at_time")]
		public OptionalValue<ulong> CreatedAtTime { get; set; }

		[CandidName("expected_allowance")]
		public OptionalValue<UnboundedUInt> ExpectedAllowance { get; set; }

		[CandidName("expires_at")]
		public OptionalValue<ulong> ExpiresAt { get; set; }

		[CandidName("fee")]
		public OptionalValue<Balance__1> Fee { get; set; }

		[CandidName("from_subaccount")]
		public OptionalValue<Subaccount__1> FromSubaccount { get; set; }

		[CandidName("memo")]
		public OptionalValue<Memo> Memo { get; set; }

		[CandidName("spender")]
		public Account__1 Spender { get; set; }

		public ApproveArgs(Balance__1 amount, OptionalValue<ulong> createdAtTime, OptionalValue<UnboundedUInt> expectedAllowance, OptionalValue<ulong> expiresAt, OptionalValue<Balance__1> fee, OptionalValue<Subaccount__1> fromSubaccount, OptionalValue<Memo> memo, Account__1 spender)
		{
			this.Amount = amount;
			this.CreatedAtTime = createdAtTime;
			this.ExpectedAllowance = expectedAllowance;
			this.ExpiresAt = expiresAt;
			this.Fee = fee;
			this.FromSubaccount = fromSubaccount;
			this.Memo = memo;
			this.Spender = spender;
		}

		public ApproveArgs()
		{
		}
	}
}