using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;

namespace Candid.World.Models
{
	public class CanisterOutputMessage
	{
		[CandidName("client_key")]
		public ClientKey ClientKey { get; set; }

		[CandidName("content")]
		public List<byte> Content { get; set; }

		[CandidName("key")]
		public string Key { get; set; }

		public CanisterOutputMessage(ClientKey clientKey, List<byte> content, string key)
		{
			this.ClientKey = clientKey;
			this.Content = content;
			this.Key = key;
		}

		public CanisterOutputMessage()
		{
		}
	}
}