namespace Entities.RequestFeatures {
	
	public class PagedList<T> : List<T> {

		public PaginationMetadata Metadata { get; set; }

		public PagedList(List<T> items, int count, int pageNumber, int pageSize) {

			Metadata = new PaginationMetadata {

				TotalCount = count,
				PageSize = pageSize,
				CurrentPage = pageNumber,
				TotalPages = (int)Math.Ceiling(count / (double)pageSize)
			};

			AddRange(items);
		}

		public static PagedList<T> ToPagedList(IEnumerable<T> source, int count, int pageNumber, int pageSize) =>
			new PagedList<T>(source.ToList(), count, pageNumber, pageSize);
	}
}
