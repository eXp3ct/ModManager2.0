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
	public class ExportSelectedModsQuery : IRequest<string>
	{
		public string FolderPath { get; set; }

		public ExportSelectedModsQuery(string folderPath)
		{
			FolderPath = folderPath;
		}
	}

	public class ExportSelectedModsQueryHandler : IRequestHandler<ExportSelectedModsQuery, string>
	{
		private readonly IList<Mod> _selectedMods;

		public ExportSelectedModsQueryHandler(IList<Mod> selectedMods)
		{
			_selectedMods = selectedMods;
		}

		public async Task<string> Handle(ExportSelectedModsQuery request, CancellationToken cancellationToken)
		{
			var json = JsonConvert.SerializeObject(_selectedMods, Formatting.Indented);

			var path = $"{request.FolderPath}/modpack{DateTime.Now.ToString("yyyy-MM-dd")}.json";

			await File.WriteAllTextAsync(path, json, cancellationToken);

			return path;
		}
	}
}
