using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using EdjCase.ICP.Candid.Models;
using Time = EdjCase.ICP.Candid.Models.UnboundedInt;

namespace Candid.Extv2Boom.Models
{
	public class Listing
	{
		[CandidName("locked")]
		public Listing.LockedInfo Locked { get; set; }

		[CandidName("price")]
		public ulong Price { get; set; }

		[CandidName("seller")]
		public Principal Seller { get; set; }

		public Listing(Listing.LockedInfo locked, ulong price, Principal seller)
		{
			this.Locked = locked;
			this.Price = price;
			this.Seller = seller;
		}

		public Listing()
		{
		}

		public class LockedInfo : OptionalValue<Time>
		{
			public LockedInfo()
			{
			}

			public LockedInfo(Time value) : base(value)
			{
			}
		}
	}
}