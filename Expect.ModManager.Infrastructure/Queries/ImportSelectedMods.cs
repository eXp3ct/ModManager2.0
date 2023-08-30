using Expect.ModManager.Domain.Models;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Infrastructure.Queries
{
	public class ImportSelectedModsQuery : IRequest<IEnumerable<Mod>?>
	{
		public string FilePath { get; set; }

		public ImportSelectedModsQuery(string filePath)
		{
			FilePath = filePath;
		}
	}

	public class ImportSelectedModsQueryHandler : IRequestHandler<ImportSelectedModsQuery, IEnumerable<Mod>?>
	{
		public async Task<IEnumerable<Mod>?> Handle(ImportSelectedModsQuery request, CancellationToken cancellationToken)
		{
			var json = await File.ReadAllTextAsync(request.FilePath, cancellationToken);

			var mods = JsonConvert.DeserializeObject<IEnumerable<Mod>>(json);

			return mods;
		}
	}
}
