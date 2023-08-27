using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.CurseApiClient.Exceptions
{
	internal class FetchingException : Exception
	{
		private readonly HttpStatusCode _statusCode;
		private readonly Uri _requestUri;
		public FetchingException(HttpStatusCode statusCode, Uri requestUri)
		{
			_requestUri = requestUri;
			_statusCode = statusCode;
		}

		public override string Message => $"Status code: {_statusCode}, Request Uri: {_requestUri}";

	}
}
