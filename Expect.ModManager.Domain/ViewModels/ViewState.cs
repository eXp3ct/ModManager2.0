using Expect.ModManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Expect.ModManager.Domain.ViewModels
{
	public class ViewState : INotifyPropertyChanged
	{
		private int _gameId;
		public int GameId
		{
			get { return _gameId; }
			set
			{
				if (_gameId != value)
				{
					_gameId = value;
					OnPropertyChanged(nameof(GameId));
				}
			}
		}

		private int _classId;
		public int ClassId
		{
			get { return _classId; }
			set
			{
				if (_classId != value)
				{
					_classId = value;
					OnPropertyChanged(nameof(ClassId));
				}
			}
		}

		private int _categoryId;
		public int CategoryId
		{
			get { return _categoryId; }
			set
			{
				if (_categoryId != value)
				{
					_categoryId = value;
					OnPropertyChanged(nameof(CategoryId));
				}
			}
		}

		private string _gameVersion;
		public string GameVersion
		{
			get { return _gameVersion; }
			set
			{
				if (_gameVersion != value)
				{
					_gameVersion = value;
					OnPropertyChanged(nameof(GameVersion));
				}
			}
		}

		private string _searchFilter;
		public string SearchFilter
		{
			get { return _searchFilter; }
			set
			{
				if (_searchFilter != value)
				{
					_searchFilter = value;
					OnPropertyChanged(nameof(SearchFilter));
				}
			}
		}

		private SearchSortFields _sortField;
		public SearchSortFields SortField
		{
			get { return _sortField; }
			set
			{
				if (_sortField != value)
				{
					_sortField = value;
					OnPropertyChanged(nameof(SortField));
				}
			}
		}

		private string _sortOrder;
		public string SortOrder
		{
			get { return _sortOrder; }
			set
			{
				if (_sortOrder != value)
				{
					_sortOrder = value;
					OnPropertyChanged(nameof(SortOrder));
				}
			}
		}

		private ModLoaderType _modLoaderType;
		public ModLoaderType ModLoaderType
		{
			get { return _modLoaderType; }
			set
			{
				if (_modLoaderType != value)
				{
					_modLoaderType = value;
					OnPropertyChanged(nameof(ModLoaderType));
				}
			}
		}

		private int _gameVersionTypeId;
		public int GameVersionTypeId
		{
			get { return _gameVersionTypeId; }
			set
			{
				if (_gameVersionTypeId != value)
				{
					_gameVersionTypeId = value;
					OnPropertyChanged(nameof(GameVersionTypeId));
				}
			}
		}

		private int _authorId;
		public int AuthorId
		{
			get { return _authorId; }
			set
			{
				if (_authorId != value)
				{
					_authorId = value;
					OnPropertyChanged(nameof(AuthorId));
				}
			}
		}

		private string _slug;
		public string Slug
		{
			get { return _slug; }
			set
			{
				if (_slug != value)
				{
					_slug = value;
					OnPropertyChanged(nameof(Slug));
				}
			}
		}

		private int _index;
		public int Index
		{
			get { return _index; }
			set
			{
				if (_index != value)
				{
					_index = value;
					OnPropertyChanged(nameof(Index));
				}
			}
		}

		private int _pageSize;
		public int PageSize
		{
			get { return _pageSize; }
			set
			{
				if (_pageSize != value)
				{
					_pageSize = value;
					OnPropertyChanged(nameof(PageSize));
				}
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
