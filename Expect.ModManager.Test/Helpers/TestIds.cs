using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Test.Helpers
{
	public static class TestIds
	{
		public static IEnumerable<object[]> ModIdList()
		{
			yield return new object[] { new List<int> { 222880, 325739, 291737, 390003, 819355, 810803} };
		}

		public static IEnumerable<object[]> FileIdList()
		{
			yield return new object[] { new List<int> { 4417968, 4413039, 4344857, 4338944 } };
		}
		public static IEnumerable<object[]> ModFileIdList()
		{
			var data = new Dictionary<int, List<int>>
			{
				{250277, new List<int>{ 4338944, 4338924, 4301342}},
				{222880, new List<int>{ 4382373, 4382339, 4112137}},
				{291737, new List<int>{ 4382374, 4382344, 4112132}},
			};

			return data.Select(x => new object[] { x.Key, x.Value });
		}
	}
}
