using EdjCase.ICP.Candid.Mapping;
using Candid.World.Models;
using System.Collections.Generic;

namespace Candid.World.Models
{
	public class WebsocketMessage
	{
		[CandidName("client_key")]
		public ClientKey ClientKey { get; set; }

		[CandidName("content")]
		public List<byte> Content { get; set; }

		[CandidName("is_service_message")]
		public bool IsServiceMessage { get; set; }

		[CandidName("sequence_num")]
		public ulong SequenceNum { get; set; }

		[CandidName("timestamp")]
		public ulong Timestamp { get; set; }

		public WebsocketMessage(ClientKey clientKey, List<byte> content, bool isServiceMessage, ulong sequenceNum, ulong timestamp)
		{
			this.ClientKey = clientKey;
			this.Content = content;
			this.IsServiceMessage = isServiceMessage;
			this.SequenceNum = sequenceNum;
			this.Timestamp = timestamp;
		}

		public WebsocketMessage()
		{
		}
	}
}