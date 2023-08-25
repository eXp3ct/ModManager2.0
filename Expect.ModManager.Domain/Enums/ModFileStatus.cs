using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Domain.Enums
{
	public enum ModFileStatus
	{
		Processing = 1,
		ChangesRequired = 2,
		UnderReview = 3,
		Approved = 4,
		Rejected = 5,
		MalwareDetected = 6,
		Deleted = 7,
		Archived = 8,
		Testing = 9,
		Released = 10,
		ReadForReview = 11,
		Deprecated = 12,
		Baking = 13,
		AwaitingPublishing = 14,
		FailedPublishing = 15,
	}
}
