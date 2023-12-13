using EdjCase.ICP.Candid.Mapping;
using Candid.Extv2Boom.Models;
using TokenIdentifier = System.String;

namespace Candid.Extv2Boom.Models
{
	public class BalanceRequest
	{
		[CandidName("token")]
		public TokenIdentifier Token { get; set; }

		[CandidName("user")]
		public User User { get; set; }

		public BalanceRequest(TokenIdentifier token, User user)
		{
			this.Token = token;
			this.User = user;
		}

		public BalanceRequest()
		{
		}
	}
}